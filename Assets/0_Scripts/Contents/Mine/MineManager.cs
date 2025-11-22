using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MineData, MineUI 관리
/// Mine에 object를 추가하고 제거하는 모든 작업을 여기에 요청해서 처리
/// </summary>
[DisallowMultipleComponent]
public class MineManager : MonoBehaviour, ISaveHandler
{
    public static MineManager Instance { get; private set; }

    [SerializeField] private Transform _lineContainer;
    [SerializeField] private Transform _lineAddPosition;

    private Transform _lastLineGroup;

    private MineState _state;
    private MineDomain _domain;

    // <Depth, View>
    private readonly Dictionary<int, LineView> _lines = new();

    void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        #region CreateTempState 

        // TODO: 나중에 저장된 데이터 불러오기
        var dto = new Mine.DTO {
            Id = "Player1_Mine",
            CurrentDepth = 0,
            Lines = new()
        };
        {
            var line = new Mine.LineDTO {
                Depth = 0,
                IsTopLine = true,
                //Rocks = null,
                Rocks = new() {
                new Mine.RockDTO { Id = "001", Hp = 0 },
                new Mine.RockDTO { Id = "002", Hp = 0 },
                new Mine.RockDTO { Id = "003", Hp = 0 },
                new Mine.RockDTO { Id = "004", Hp = 0 },
                new Mine.RockDTO { Id = "005", Hp = 0 },
                new Mine.RockDTO { Id = "006", Hp = 0 },
                new Mine.RockDTO { Id = "007", Hp = 0 },
                new Mine.RockDTO { Id = "008", Hp = 0 },
                new Mine.RockDTO { Id = "009", Hp = 0 },
                new Mine.RockDTO { Id = "00A", Hp = 0 },
                new Mine.RockDTO { Id = "00B", Hp = 0 },
                new Mine.RockDTO { Id = "00C", Hp = 6 },
                new Mine.RockDTO { Id = "00D", Hp = 6 },
                new Mine.RockDTO { Id = "00E", Hp = 6 },
                new Mine.RockDTO { Id = "00F", Hp = 6 }
                },
                Veins = new() {
                    //new Mine.VeinDTO { Id = "010", Pos = "005", Type = (int)VeinType.Bauxite },
                    //new Mine.VeinDTO { Id = "020", Pos = "00B", Type = (int)MineralType.Coal }
                }
            };
            dto.Lines.Add(line);
        }
        {
            var line = new Mine.LineDTO {
                Depth = 1,
                IsTopLine = false,
                Rocks = new() {
                new Mine.RockDTO { Id = "101", Hp = 6 },
                new Mine.RockDTO { Id = "102", Hp = 6 },
                new Mine.RockDTO { Id = "103", Hp = 6 },
                new Mine.RockDTO { Id = "104", Hp = 6 },
                new Mine.RockDTO { Id = "105", Hp = 6 },
                new Mine.RockDTO { Id = "106", Hp = 6 },
                new Mine.RockDTO { Id = "107", Hp = 6 },
                new Mine.RockDTO { Id = "108", Hp = 6 },
                new Mine.RockDTO { Id = "109", Hp = 6 },
                new Mine.RockDTO { Id = "10A", Hp = 6 },
                new Mine.RockDTO { Id = "10B", Hp = 6 },
                new Mine.RockDTO { Id = "10C", Hp = 6 },
                new Mine.RockDTO { Id = "10D", Hp = 6 },
                new Mine.RockDTO { Id = "10E", Hp = 6 },
                new Mine.RockDTO { Id = "10F", Hp = 6 }
                },
                Veins = new() {
                    //new Mine.VeinDTO { Id = "110", Pos = "10A", Type = (int)MineralType.Coal },
                    new Mine.VeinDTO { Id = "120", Pos = "102", Type = (int)MineralType.Coal }
                }
            };
            dto.Lines.Add(line);
        }
        {
            var line = new Mine.LineDTO {
                Depth = 2,
                IsTopLine = false,
                Rocks = new() {
                new Mine.RockDTO { Id = "201", Hp = 6 },
                new Mine.RockDTO { Id = "202", Hp = 6 },
                new Mine.RockDTO { Id = "203", Hp = 6 },
                new Mine.RockDTO { Id = "204", Hp = 6 },
                new Mine.RockDTO { Id = "205", Hp = 6 },
                new Mine.RockDTO { Id = "206", Hp = 6 },
                new Mine.RockDTO { Id = "207", Hp = 6 },
                new Mine.RockDTO { Id = "208", Hp = 6 },
                new Mine.RockDTO { Id = "209", Hp = 6 },
                new Mine.RockDTO { Id = "20A", Hp = 6 },
                new Mine.RockDTO { Id = "20B", Hp = 6 },
                new Mine.RockDTO { Id = "20C", Hp = 6 },
                new Mine.RockDTO { Id = "20D", Hp = 6 },
                new Mine.RockDTO { Id = "20E", Hp = 6 },
                new Mine.RockDTO { Id = "20F", Hp = 6 }
                },
                Veins = new() {
                    //new Mine.VeinDTO { Id = "210", Pos = "20C", Type = (int)MineralType.Coal },
                    //new Mine.VeinDTO { Id = "220", Pos = "204", Type = (int)MineralType.Coal }
                }
            };
            dto.Lines.Add(line);
        }
        {
            var line = new Mine.LineDTO {
                Depth = 3,
                IsTopLine = false,
                Rocks = new() {
                new Mine.RockDTO { Id = "301", Hp = 6 },
                new Mine.RockDTO { Id = "302", Hp = 6 },
                new Mine.RockDTO { Id = "303", Hp = 6 },
                new Mine.RockDTO { Id = "304", Hp = 6 },
                new Mine.RockDTO { Id = "305", Hp = 6 },
                new Mine.RockDTO { Id = "306", Hp = 6 },
                new Mine.RockDTO { Id = "307", Hp = 6 },
                new Mine.RockDTO { Id = "308", Hp = 6 },
                new Mine.RockDTO { Id = "309", Hp = 6 },
                new Mine.RockDTO { Id = "30A", Hp = 6 },
                new Mine.RockDTO { Id = "30B", Hp = 6 },
                new Mine.RockDTO { Id = "30C", Hp = 6 },
                new Mine.RockDTO { Id = "30D", Hp = 6 },
                new Mine.RockDTO { Id = "30E", Hp = 6 },
                new Mine.RockDTO { Id = "30F", Hp = 6 }
                },
                Veins = new() {
                    //new Mine.VeinDTO { Id = "210", Pos = "20C", Type = (int)MineralType.Coal },
                    //new Mine.VeinDTO { Id = "220", Pos = "204", Type = (int)MineralType.Coal }
                }
            };
            dto.Lines.Add(line);
        }
        #endregion

        //_domain = new MineDomain(_state, new DefaultMineRules(), seed: 12345);
        _state = new();
        Mine.Mapper.ApplyFromDTO(dto, _state);
        _domain = new MineDomain(_state, new DefaultMineRules(), 12345);

        _domain.OnRockDamaged += HandleRockDamaged;
        _domain.OnRockBroken += HandleRockBroken;
        _domain.OnLineAdded += HandleLineAdded;
        _domain.OnVeinClicked += HandleVeinDamaged;
        _domain.OnLineClear += HandleLineClear;

        ReBuildAll();
        _domain.BreakIfHpZero();
    }
    void Start()
    {
        Managers.Save.Register(this);
    }

    void OnRockClicked(int rockId)
    {
        Debug.Log($"Rock Clicked: {rockId}");

        _domain.ClickRock(rockId, damage: 6);
    }

    void OnVeinClick(int veinId)
    {
        Debug.Log($"Vein Clicked: {veinId}");

        _domain.ClickVein(veinId, damage: 1);
    }

    #region EventHandlers
    private void HandleRockDamaged(int rockId, int remainingHp)
    {
        Debug.Log($"Rock {rockId} Damaged, Remaining HP: {remainingHp}");

        var rock = FindRockState(rockId, out var line);
        if (rock == null || line == null) return;

        if (_lines.TryGetValue(line.Depth, out var lineView))
            if (lineView.TryGetRockView(rockId, out var rockView)) {
                rock.Hp = remainingHp;
                rockView.Refresh(rock);
                rockView.PlayDoTween();
            }
    }

    private void HandleRockBroken(int rockId)
    {
        Debug.Log($"Rock {rockId} Broken");

        var rock = FindRockState(rockId, out var line);
        if (rock == null || line == null) return;

        RockView rockView = null;
        LineView lineView = null;
        if (_lines.TryGetValue(line.Depth, out lineView) &&
            lineView.TryGetRockView(rockId, out rockView)) {
            rockView.PlayBreak();
        }

        var vein = line.Veins.Find(x => x.Pos == rockId);
        if (vein == null) return;
        var veinView = SpawnVeinView(vein);
        lineView.AddVeinView(veinView);
    }

    private void HandleLineAdded(int lineDepth)
    {
        Debug.Log($"Line {lineDepth} Added");

        var line = _state.Lines.Find(l => l.Depth == lineDepth);
        if (line == null) return;

        AddLineView(line);
    }

    private void HandleVeinDamaged(int veinId, int damage)
    {
        Debug.Log($"Vein {veinId} Clicked, Damage: {damage}");

        var vein = FindVeinState(veinId, out var line);
        if (vein == null || line == null) return;

        if (_lines.TryGetValue(line.Depth, out var lineView) &&
            lineView.TryGetVeinView(veinId, out var veinView)) {
            veinView.PlayDoTween();
            //TODO: 자원 획득 처리
        }
    }

    private void HandleLineClear(int lineDepth)
    {
        _lines[lineDepth].RemoveRock();
    }
    #endregion

    /// <summary>
    /// MineState 데이터를 기반으로 모든 UI를 재구성
    /// </summary>
    void ReBuildAll()
    {
        //NOTE: Canvas 강제 갱신
        Canvas.ForceUpdateCanvases();
        //NOTE: 기존 UI 제거
        foreach (Transform child in _lineContainer) {
            if (child.GetComponent<LineView>() != null)
                Destroy(child.gameObject);
        }

        foreach (var line in _state.Lines) {
            AddLineView(line);
        }
        //NOTE: Canvas 강제 갱신
        Canvas.ForceUpdateCanvases();
    }

    #region View Spawning
    void AddLineView(LineState line)
    {
        var lineView = SpawnLineView(line);
        lineView.transform.SetParent(_lineContainer, false);
        lineView.transform.SetSiblingIndex(_lineAddPosition.GetSiblingIndex());
        _lines[line.Depth] = lineView;
    }

    LineView SpawnLineView(LineState line)
    {
        var lineView = Managers.Resource.Instantiate("UI/Mine/LineView").GetOrAddComponent<LineView>();
        lineView.BuildFrom(line, SpawnRockView, SpawnVeinView);
        return lineView;
    }

    RockView SpawnRockView(RockState rock)
    {
        var rockView = Managers.Resource.Instantiate("UI/Mine/RockView").GetOrAddComponent<RockView>();
        rockView.Bind(rock, OnRockClicked);
        return rockView;
    }

    VeinView SpawnVeinView(VeinState vein)
    {
        var veinView = Managers.Resource.Instantiate("UI/Mine/VeinView").GetOrAddComponent<VeinView>();
        veinView.Bind(vein, OnVeinClick);
        return veinView;
    }
    #endregion

    #region Save/Load
    public bool OnSaveRequest(GlobalDTO dto)
    {
        Debug.Log("MineManager OnSaveRequest");
        return Mine.Mapper.MakeDTO(_state, out dto.Mine);
    }

    public bool OnLoadRequest(GlobalDTO dto)
    {
        Debug.Log("MineManager OnLoadRequest");
        var result = Mine.Mapper.ApplyFromDTO(dto.Mine, _state);

        Reload();

        return result;
    }
    #endregion

    #region Utility
    private void Reload()
    {
        ReBuildAll();
        _domain.BreakIfHpZero();
    }

    /// <summary>
    /// rockId로 RockState와 해당 Rock 이 속한 LineState를 찾음
    /// </summary>
    RockState FindRockState(int rockId, out LineState lineOut)
    {
        foreach (var line in _state.Lines) {
            var r = line.Rocks?.Find(x => x.Id == rockId);
            if (r != null) {
                lineOut = line;
                return r;
            }
        }
        lineOut = null;
        return null;
    }

    VeinState FindVeinState(int veinId, out LineState lineOut)
    {
        foreach (var line in _state.Lines) {
            var v = line.Veins?.Find(x => x.Id == veinId);
            if (v != null) {
                lineOut = line;
                return v;
            }
        }
        lineOut = null;
        return null;
    }

    /// <summary>
    /// 깊이와 인덱스(0~14)로 해당 위치에 RockState를 찾음
    /// </summary>
    public RockState TryGetRockAt(int depth, int index)
    {
        return _state.Lines.Find(l => l.Depth == depth)?
            .Rocks?.Find(r => r.Id == Util.MakeRockId(depth, index));
    }

    public void TryAttackRockAt(int depth, int index, int damage)
    {
        var rock = TryGetRockAt(depth, index);
        if (rock != null) {
            _domain.ClickRock(rock.Id, damage);
        }
    }

    public void TryAttackRockByState(RockState rock, int damage)
    {
        if (rock != null) {
            _domain.ClickRock(rock.Id, damage);
        }
    }

    public RockView TryGetRockViewAt(int depth, int index)
    {
        if (_lines.TryGetValue(depth, out var lineView)) {
            if (lineView.TryGetRockView(Util.MakeRockId(depth, index), out var rockView)) {
                return rockView;
            }
        }
        return null;
    }

    public LineView TryGetLineView(int depth)
    {
        if (_lines.TryGetValue(depth, out var lineView)) {
            return lineView;
        }
        return null;
    }
    #endregion

    #region Worker Robot
    public void SpawnWorker()
    {
        var worker = Managers.Resource.Instantiate("Worker", _lineContainer);
        worker.GetComponent<Worker>().Init(this);
    }
    #endregion
}
