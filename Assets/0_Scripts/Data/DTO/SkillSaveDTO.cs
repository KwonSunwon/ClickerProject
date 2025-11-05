using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillSaveDTO
{
    public List<int> Id;
    public List<int> Level;
}

public class SkillMapper
{
    public static bool ToDTO(Dictionary<int, SkillNodeData> skills, out SkillSaveDTO dto)
    {
        dto = new SkillSaveDTO();

        dto.Id = new List<int>();
        dto.Level = new List<int>();

        foreach (var kvp in skills) {
            dto.Id.Add(kvp.Key);
            dto.Level.Add(kvp.Value.Level);
        }

        if (dto.Id.Count != dto.Level.Count || dto.Id.Count != skills.Count) {
            Debug.LogError("Error @SkillMapper - ToDTO: Mismatch in counts during Skill to DTO conversion");
            return false;
        }
        return true;
    }

    public static bool FromDTO(SkillSaveDTO dto, Dictionary<int, SkillNodeData> skills)
    {
        var s = dto as SkillSaveDTO;
        for (int i = 0; i < s.Id.Count; i++) {
            int key = s.Id[i];
            if (!skills.ContainsKey(key)) {
                Debug.LogError("Error @SkillMapper - FromDTO: Skill key not found in skills dictionary");
                return false;
            }
            skills[key].Level = s.Level[i];
        }
        return true;
    }
}