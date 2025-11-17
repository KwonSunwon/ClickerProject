using System;
using System.Collections.Generic;
using UnityEngine;

namespace Skill
{
    [Serializable]
    public class DTO
    {
        public List<int> Id;
        public List<int> Level;
    }

    public class Mapper
    {
        public static bool MakeDTO(Dictionary<int, SkillNodeData> skills, out DTO dto)
        {
            dto = new DTO();

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

        public static bool ApplyFromDTO(DTO dto, Dictionary<int, SkillNodeData> skills)
        {
            for (int i = 0; i < dto.Id.Count; i++) {
                int key = dto.Id[i];
                if (!skills.ContainsKey(key)) {
                    Debug.LogError("Error @SkillMapper - FromDTO: Skill key not found in skills dictionary");
                    return false;
                }
                skills[key].Level = dto.Level[i];
            }
            return true;
        }
    }
}