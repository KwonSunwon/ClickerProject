using static Util;

public static class MineMapper
{
    public const int CURRENT_VERSION = 1;

    public static bool ToDTO(MineState state, out MineSaveDTO dto)
    {
        dto = new MineSaveDTO {
            Id = state.Id,
            CurrentDepth = state.CurrentDepth,
            Lines = new()
        };

        foreach (var line in state.Lines) {
            var lineDTO = new LineSaveDTO {
                Depth = line.Depth,
                IsTopLine = line.IsTopLine,
                Rocks = null,
                Veins = new()
            };

            if (line.Rocks != null) {
                lineDTO.Rocks = new();
                foreach (var rock in line.Rocks) {
                    var rockDTO = new RockSaveDTO {
                        Id = GetHex(rock.Id),
                        Hp = rock.Hp
                    };
                    lineDTO.Rocks.Add(rockDTO);
                }
            }

            foreach (var vein in line.Veins) {
                var veinDTO = new VeinSaveDTO {
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

    public static bool FromDTO(MineSaveDTO dto, MineState state)
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
                        Hp = rockDTO.Hp
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
