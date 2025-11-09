using IngameDebugConsole;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager
{
    private List<ISaveHandler> _participants = new List<ISaveHandler>();

    public void Init()
    {
#if UNITY_EDITOR
        DebugLogConsole.AddCommandInstance("SaveAll", "Saves the game state to a file.", "Save", this);
        DebugLogConsole.AddCommandInstance("LoadAll", "Loads the game state from a file.", "Load", this);
#endif
    }

    public void Register(ISaveHandler saveHandler)
    {
        _participants.Add(saveHandler);
    }

    public void Unregister(ISaveHandler saveHandler)
    {
        _participants.Remove(saveHandler);
    }

    //TODO: 테스트 완료 후 필요 없는 매개변수 제거
    public void Save(bool isJsonTest = false)
    {
        var dto = new GlobalDTO {
            CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
        };

        foreach (var participant in _participants) {
            if (!participant.OnSaveRequest(dto)) {
                Debug.LogError($"Error @SaveManager - Save: @{participant.GetType().Name} Save request failed");
                return;
            }
        }

        try {
            var json = JsonUtility.ToJson(dto);
#if UNITY_EDITOR
            if (isJsonTest) {
                File.WriteAllText(GetSavePath(), json);
            }
            else {
                File.WriteAllBytes(GetSavePath(), SecureDataManager.Encrypt(json));
            }
#else
        File.WriteAllBytes(GetSavePath(), SecureDataManager.Encrypt(json));
#endif
        }
        catch (Exception e) {
            Debug.LogError($"Error @SaveManager - Save: Exception during file write - {e.Message}");
        }
    }

    public void Load()
    {
        var dto = new GlobalDTO();
        try {
            var str = File.ReadAllText(GetSavePath());
            dto = JsonUtility.FromJson<GlobalDTO>(str);
            if (dto == null) {
                Debug.LogError("Error @SaveManager - Load: DTO load fail");
                return;
            }
        }
        catch (Exception e) {
            Debug.LogError($"Error @SaveManager - Load: Exception during file read - {e.Message}");
        }

        foreach (var participant in _participants) {
            if (!participant.OnLoadRequest(dto)) {
                Debug.LogError($"Error @SaveManager - Load: @{participant.GetType().Name} Load request failed");
                return;
            }
        }
    }

    private string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, "saveFile.sav");
    }
}