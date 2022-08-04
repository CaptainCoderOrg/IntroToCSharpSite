using MudBlazor;
using System.Text.Json;
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

    public UserStats(int xp)
    {
        this.XP = xp;
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

    public readonly static UserStats Default = new (0);

    public static DataReference<UserStats> DataReference(string path, UserStats? defaultValue = null, string? niceName = null)
    {
        return new UserDataDataReference(path, defaultValue, niceName);
    }
}

internal class UserDataDataReference : DataReference<UserStats>
{
    public UserDataDataReference(string path, UserStats? defaultValue = null, string? niceName = null) : base(path, defaultValue, niceName) { }

    public override async void Set(UserStats data, bool notifyOnSuccess = true)
    {
        IResultHandler handler = notifyOnSuccess ? this : IResultHandler.Default;
        string jsonData = JsonSerializer.Serialize(data);
        await DatabaseService.Service.Set<string>(Path, jsonData, handler);
    }

    protected override void HandleChange(string data)
    {
        try
        {
            string jsonString = JsonDocument.Parse(data).RootElement.GetString()!;
            Val = JsonSerializer.Deserialize<UserStats>(jsonString);
        }
        catch (JsonException)
        {
            NotificationService.Service.Add($"Error loading {NiceName}. Expected UserData but found: {data}", Severity.Error).AndForget();
        }
        catch
        {
            NotificationService.Service.Add($"Error loading {NiceName}.", Severity.Error).AndForget();
        }
    }
}