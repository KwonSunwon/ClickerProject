using System;

[Serializable]
public class GlobalDTO
{
    //public int Version;

    //public string ProfileId;
    //public string GameVersion;

    public long CreatedAt;
    //public long UpdatedAt;

    //public int Seed;

    public Mine.DTO Mine;
    public Skill.DTO Skill;
    public Mineral.DTO Mineral;
}