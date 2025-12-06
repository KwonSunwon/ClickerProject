using System;
using System.Collections.Generic;
using System.IO;
using static Util;

public class DefaultMineRules : IMineRules
{
    public IReadOnlyList<VeinState> PlanVeinToLine(LineState line, Random rng)
    {
        // 1. 개수 결정 
        var veinCount = rng.Next(2, 5); //TODO: 임시로 1~2개 랜덤 -> 깊이에 따라 개수 변경, 특정 층 고정 규칙 추가
        List<VeinState> veins = new(veinCount);
        var occupied = new HashSet<int>();

        for (int i = 0; i < veinCount; i++) {
            var vein = new VeinState {
                Id = MakeVeinId(line.Depth, i)
            };

            // 2. 위치 결정
            do {
                var idx = rng.Next(0, line.Rocks.Count);
                vein.Pos = line.Rocks[idx].Id;
            } while (!occupied.Add(vein.Pos));

            // 3. 종류 결정
            vein.Type = rng.Next(0, (int)MineralType.MaxNum - 1);

            veins.Add(vein);
        }

        return veins;
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

public class MiningEfficiency
{
    string Name;
    string Category;
    int MinDepth;
    int MaxDepth;
    float BasicMiner;
    float ImprovedMiner;
    float HighPerformanceMiner;
    float PrecisionMiner;
    float HighTechMiner;
    float AlienMiner;

    static private Dictionary<MineralType, MiningEfficiency> _efficiencies = null;

    private const string PATH = "Data/MiningEfficiency";

    public MiningEfficiency Get(MineralType type)
    {
        if (_efficiencies == null) {
            List<MiningEfficiency> wrappingList = new();
            var file = File.ReadAllText(PATH);
            UnityEngine.JsonUtility.FromJsonOverwrite(file, wrappingList);

            _efficiencies = new Dictionary<MineralType, MiningEfficiency>();
            foreach (var efficiency in wrappingList) {
                var mineralType = (MineralType)Enum.Parse(typeof(MineralType), efficiency.Name);
                _efficiencies[mineralType] = efficiency;
            }
        }
        return _efficiencies[type];
    }
}