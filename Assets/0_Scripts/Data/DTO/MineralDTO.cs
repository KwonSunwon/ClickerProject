using System;
using System.Collections.Generic;

namespace Mineral
{
    [Serializable]
    public class DTO
    {
        public List<int> Id;
        public List<String> Amount;
    }

    public static class Mapper
    {
        public static bool CreateDTO(Dictionary<MineralType, MineralSlot> minerals, out DTO dto)
        {
            dto = new DTO();
            dto.Id = new List<int>();
            dto.Amount = new List<String>();
            foreach (var kvp in minerals) {
                dto.Id.Add((int)kvp.Key);
                dto.Amount.Add(kvp.Value.Amount.ToString());
            }
            if (dto.Id.Count != dto.Amount.Count || dto.Id.Count != minerals.Count) {
                UnityEngine.Debug.LogError("Error @MineralMapper - ToDTO: Mismatch in counts during Mineral to DTO conversion");
                return false;
            }
            return true;
        }

        public static bool ApplyFromDTO(DTO dto, Dictionary<MineralType, MineralSlot> minerals)
        {
            var s = dto as DTO;
            for (int i = 0; i < s.Id.Count; i++) {
                MineralType key = (MineralType)s.Id[i];
                if (!minerals.ContainsKey(key)) {
                    UnityEngine.Debug.LogError("Error @MineralMapper - FromDTO: Mineral key not found in minerals dictionary");
                    return false;
                }
                minerals[key].Amount = new BigNumber(s.Amount[i]);
            }
            return true;
        }

    }
}