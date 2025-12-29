using System;
using System.Collections.Generic;

using static Util;

namespace Mine
{
    [Serializable]
    public class DTO
    {
        public string Id;
        public int CurrentDepth;
        public List<LineDTO> Lines;
    }

    [Serializable]
    public class LineDTO
    {
        public int Depth;
        public bool IsTopLine;
        public List<RockDTO> Rocks;
        public List<VeinDTO> Veins;
    }

    [Serializable]
    public class RockDTO
    {
        public string Id;  // HEX (depth * 100) + 1 ~ E
        public int Hp;
    }

    [Serializable]
    public class VeinDTO
    {
        public string Id;    // HEX (depth * 100) + (1 ~ E * 10)
        public string Pos;   // Rock Id
        public int Type;
    }

    public static class Mapper
    {
        public const int CURRENT_VERSION = 1;

        public static bool MakeDTO(MineState state, out DTO dto)
        {
            dto = new DTO {
                Id = state.Id,
                CurrentDepth = state.CurrentDepth,
                Lines = new()
            };

            foreach (var line in state.Lines) {
                var lineDTO = new LineDTO {
                    Depth = line.Depth,
                    IsTopLine = line.IsTopLine,
                    Rocks = null,
                    Veins = new()
                };

                if (line.Rocks != null) {
                    lineDTO.Rocks = new();
                    foreach (var rock in line.Rocks) {
                        var rockDTO = new RockDTO {
                            Id = GetHex(rock.Id),
                            Hp = rock.Hp,
                        };
                        lineDTO.Rocks.Add(rockDTO);
                    }
                }

                foreach (var vein in line.Veins) {
                    var veinDTO = new VeinDTO {
                        Id = GetHex(vein.Id),
                        Pos = GetHex(vein.Pos),
                        Type = vein.Type
                    };
                    lineDTO.Veins.Add(veinDTO);
                }

                if (lineDTO.Veins.Count != line.Veins.Count)
                    return false;

                dto.Lines.Add(lineDTO);
            }

            if (dto.Lines.Count != state.Lines.Count)
                return false;
            return true;
        }

        public static bool ApplyFromDTO(DTO dto, MineState state)
        {
            state.Id = dto.Id;
            state.CurrentDepth = dto.CurrentDepth;
            state.Lines = new();

            foreach (var lineDTO in dto.Lines) {
                var line = new LineState {
                    Depth = lineDTO.Depth,
                    IsTopLine = lineDTO.IsTopLine,
                    Rocks = null,
                    Veins = new()
                };

                if (lineDTO.Rocks != null && lineDTO.Rocks.Count != 0) {
                    line.Rocks = new();
                    foreach (var rockDTO in lineDTO.Rocks) {
                        var rock = new RockState {
                            Id = GetDec(rockDTO.Id),
                            Hp = rockDTO.Hp,
                            //TODO: 임시로 강제 주입, 나중에 Domain 쪽에서 한 번에 처리하거나 다른 방식으로 한 번에 처리
                            MaxHp = Math.Max(6, (int)(line.Depth * 0.5f))
                        };
                        line.Rocks.Add(rock);
                    }
                }

                foreach (var veinDTO in lineDTO.Veins) {
                    var vein = new VeinState {
                        Id = GetDec(veinDTO.Id),
                        Pos = GetDec(veinDTO.Pos),
                        Type = veinDTO.Type
                    };
                    line.Veins.Add(vein);
                }

                if (lineDTO.Veins.Count != line.Veins.Count)
                    return false;

                state.Lines.Add(line);
            }
            if (dto.Lines.Count != state.Lines.Count)
                return false;
            return true;
        }
    }
}