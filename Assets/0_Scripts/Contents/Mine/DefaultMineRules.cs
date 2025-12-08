using System;
using System.Collections.Generic;
using System.IO;
using static Util;

public class DefaultMineRules : IMineRules
{
    private GlobalVeinPlan _plan;

    public IReadOnlyList<VeinState> PlanVeinToLine(LineState line, Random rng)
    {
        if (_plan == null) {
            _plan = new(GlobalVeinPlanGenerator.Generate(rng, 500));
        }

        var veinList = _plan.Get(line.Depth);

        if (veinList.Count == 0) {
            return Array.Empty<VeinState>();
        }

        var veinStates = new List<VeinState>(veinList.Count);

        var occupied = new HashSet<int>();
        int idx = 0;
        foreach (var vein in veinList) {
            var veinState = new VeinState {
                Id = MakeVeinId(line.Depth, idx++)
            };

            do {
                var rockIdx = rng.Next(0, line.Rocks.Count);
                veinState.Pos = line.Rocks[rockIdx].Id;
            } while (!occupied.Add(veinState.Pos));

            veinState.Type = (int)vein;

            veinStates.Add(veinState);
        }

        return veinStates;

        //// 1. 개수 결정 
        //var veinCount = rng.Next(2, 5); //TODO: 임시로 1~2개 랜덤 -> 깊이에 따라 개수 변경, 특정 층 고정 규칙 추가
        //List<VeinState> veins = new(veinCount);
        //var occupied = new HashSet<int>();

        //for (int i = 0; i < veinCount; i++) {
        //    var vein = new VeinState {
        //        Id = MakeVeinId(line.Depth, i)
        //    };

        //    // 2. 위치 결정
        //    do {
        //        var idx = rng.Next(0, line.Rocks.Count);
        //        vein.Pos = line.Rocks[idx].Id;
        //    } while (!occupied.Add(vein.Pos));

        //    // 3. 종류 결정
        //    vein.Type = rng.Next(0, (int)MineralType.MaxNum - 1);

        //    veins.Add(vein);
        //}

        //return veins;
    }

    public int RocksPerLine() => 15;

    public int RockHpForDepth(int depth)
    {
        //TODO: 밸런스 조정 필요
        //return Math.Max(6, (int)(depth * 0.5f));

        var finalHp = (int)(100 * (1 + 0.015f * depth * depth));
        return finalHp;
    }

    public int GetLineMaintainCount() => 4;
}

[Serializable]
public class MiningDepthInfoData
{
    public string Name;
    public int MinDepth;
    public int MaxDepth;
    public int FirstAppearStart;
    public int FirstAppearEnd;
}

[Serializable]
public class MDIDWrapper
{
    public List<MiningDepthInfoData> MiningDepthInfo;
}

static public class MiningDepthInfo
{
    static private readonly Dictionary<MineralType, MiningDepthInfoData> _dict = new();

    static public void Init()
    {
        if (_dict.Count != 0) return;

        string path = Path.Combine(UnityEngine.Application.streamingAssetsPath, "Data", "MiningDepthInfo.json");
        var file = File.ReadAllText(path);
        var wrapper = UnityEngine.JsonUtility.FromJson<MDIDWrapper>(file);

        foreach (var info in wrapper.MiningDepthInfo) {
            if (Enum.TryParse<MineralType>(info.Name, out var type)) {
                _dict[type] = info;
            }
            else {
                UnityEngine.Debug.LogWarning($"MiningDepthInfoData에서 MineralType으로 변환 실패: {info.Name}");
            }
        }
    }

    static public Dictionary<MineralType, MiningDepthInfoData> Get()
    {
        if (_dict.Count == 0) {
            Init();
        }
        return _dict;
    }

    static public MiningDepthInfoData TryGet(MineralType type)
    {
        if (_dict.Count == 0) {
            Init();
        }
        if (_dict.TryGetValue(type, out var info)) {
            return info;
        }
        return null;
    }
}

public class GlobalVeinPlan
{
    private readonly Dictionary<int, List<MineralType>> _dict;

    public GlobalVeinPlan(Dictionary<int, List<MineralType>> veinsByDepth)
    {
        _dict = veinsByDepth;
    }

    public IReadOnlyList<MineralType> Get(int depth)
    {
        if (_dict.TryGetValue(depth, out var list)) {
            return list;
        }
        return Array.Empty<MineralType>();
    }
}

public static class GlobalVeinPlanGenerator
{
    private const int INTERVAL_KM = 7;
    private const int JITTER_KM = 2;
    private const int MAX_VEINS_COUNT = 3;

    public static Dictionary<int, List<MineralType>> Generate(System.Random rng, int maxDepth)
    {
        var dict = new Dictionary<int, List<MineralType>>(maxDepth);
        var occupancy = new Dictionary<int, int>();

        foreach (var vein in MiningDepthInfo.Get()) {
            GenerateForMineral(vein.Key, rng, maxDepth, dict, occupancy);
        }

        return dict;
    }

    private static void GenerateForMineral(
        MineralType type,
        System.Random rng,
        int maxDepth,
        Dictionary<int, List<MineralType>> table,
        Dictionary<int, int> occupancy)
    {
        var info = MiningDepthInfo.TryGet(type);
        if (info == null) return;

        int firstAppearDepth = rng.Next(info.FirstAppearStart, info.FirstAppearEnd + 1);
        firstAppearDepth = Math.Clamp(firstAppearDepth, info.MinDepth, info.MaxDepth);

        TryPlaceVein(type, firstAppearDepth, maxDepth, table, occupancy);

        for (int baseDepth = firstAppearDepth + INTERVAL_KM;
            baseDepth < info.MaxDepth;
            baseDepth += INTERVAL_KM) {
            int jitter = rng.Next(-JITTER_KM, JITTER_KM + 1);
            int targetDepth = baseDepth + jitter;
            targetDepth = Math.Clamp(targetDepth, info.MinDepth, info.MaxDepth);

            TryPlaceVein(type, targetDepth, maxDepth, table, occupancy);
        }
    }

    private static bool TryPlaceVein(
        MineralType type,
        int depth,
        int maxDepth,
        Dictionary<int, List<MineralType>> table,
        Dictionary<int, int> occupancy
        )
    {
        if (depth > maxDepth) return false;

        occupancy.TryGetValue(depth, out int count);
        if (count > MAX_VEINS_COUNT) {
            // 이미 해당 깊이에 최대 개수 초과
            // 다른 깊이에 배치 시도

            bool found = false;

            for (int offset = 1; offset <= JITTER_KM; offset++) {
                int down = depth + offset;
                int up = depth - offset;

                if (down >= 0 && occupancy.GetValueOrDefault(down) < MAX_VEINS_COUNT) {
                    depth = down;
                    found = true;
                    break;
                }

                if (up <= maxDepth && occupancy.GetValueOrDefault(up) < MAX_VEINS_COUNT) {
                    depth = up;
                    found = true;
                    break;
                }
            }

            if (!found) {
                return false;
            }
        }

        if (!table.TryGetValue(depth, out var list)) {
            list = new List<MineralType>();
            table[depth] = list;
        }

        list.Add(type);
        occupancy[depth] = occupancy.GetValueOrDefault(depth) + 1;

        return true;
    }
}