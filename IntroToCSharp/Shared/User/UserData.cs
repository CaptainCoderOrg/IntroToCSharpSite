using System.Text.Json.Serialization;

namespace CaptainCoder;
public class UserStats
{
    public int XP { get; private set; }
    [JsonIgnore]
    public int XPTowardLevel { get => this.XP - XPForLevel(this.Level); }
    [JsonIgnore]
    public int XPNeeded { get => NextLevelXP - XPForLevel(this.Level); }
    [JsonIgnore]
    public int Level { get => XPToLevel(this.XP); }
    [JsonIgnore]
    public int NextLevelXP { get => XPForLevel(this.Level+1); }
    public int Gold { get; private set; }

    public UserStats(int xp, int gold)
    {
        this.XP = xp;
        this.Gold = gold;
    }

    public static int XPToLevel(int xp)
    {
        return 1 + (int)Math.Sqrt(xp/100);
    }

    public static int XPForLevel(int level)
    {
        level--;
        return 100 * level * level;
    }


    public readonly static UserStats Default = new (0, 0);
}