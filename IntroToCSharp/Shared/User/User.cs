using CaptainCoder;
using System.Text.Json;
using MudBlazor;

namespace CaptainCoder;
/// <summary>
/// A User contains information about the current user of the application
/// </summary>
public abstract class User
{
    public readonly static User NoUser = new NoUser();
    /// <summary>
    /// The User's display name
    /// </summary>
    /// <value></value>
    public string? DisplayName { get; protected set; }

    public string? UID { get; protected set; }
    public string? ProviderID { get; protected set; }

    /// <summary>
    /// The first letter of the User's display name.
    /// </summary>
    public char AvatarLetter => DisplayName != null ? DisplayName.ToUpper()[0] : '?';

    /// <summary>
    /// The User's email address
    /// </summary>
    /// <value></value>
    public string? Email { get; protected set; }

    /// <summary>
    /// true if the user is logged in and false otherwise
    /// </summary>
    /// <value></value>
    public bool IsLoggedIn { get; protected set; }

    public string Role { get; set; } = "user";

    public string? TokenManager { get; private set; } = null;

    /// <summary>
    /// A Reference to this users DarkMode preference.
    /// </summary>
    public DataReference<bool>? DarkMode;

    /// <summary>
    /// A Reference to this users ProjectData.
    /// </summary>
    public DataReference<string>? ProjectData;
    public DataReference<string>? DefaultProject;
    public DataReference<UserStats>? UserStatsRef { get; protected set; }
    public DataReference<UserInventory>? UserInventoryRef { get; protected set; }
    public DataReference<UserPages>? UserPagesRef {get; protected set; }
    protected Dictionary<string, string>? _projects;

    public Dictionary<string, string> Projects
    {
        get => _projects ?? new Dictionary<string, string>();
    }

    /// <summary>
    /// Given an authentication response, constructs a User object from that JSON data.
    /// </summary>
    /// <param name="response">"null" if the user is not authenticated otherwise a JSON string with 'displayName' and 'email'</param>
    public User()
    {
        this.DisplayName = null;
        this.UID = null;
        this.Email = null;
        this.IsLoggedIn = false;
        this.DarkMode = null;
        this.ProjectData = null;
        this._projects = null;
        this.DefaultProject = null;
        this.UserStatsRef = null;
        this.TokenManager = null;
    }

    public override string ToString()
    {
        return $"User {{ {DisplayName}, {Email} }}";
    }

    internal protected void DoLogin(JsonElement? root = null)
    {
        if (root != null) InitTokenManager((JsonElement)root);
        this.IsLoggedIn = true;
        this.DarkMode = DataReference.Bool($"/users/{this.UID}/prefs/DarkMode", false, "Dark Mode");
        this.UserStatsRef = DataReference.Json<UserStats>($"/users/{this.UID}/users_stats", UserStats.Default, "User Stats");
        this.UserInventoryRef = DataReference.Json<UserInventory>($"/users/{this.UID}/inventory", UserInventory.Default, "User Inventory");
        this.UserPagesRef = DataReference.Json<UserPages>($"/users/{this.UID}/pages", UserPages.Default, "Book");
        DataReference.String($"/users/{this.UID}/email", "No Email").Set(this.Email ?? "Email Null");
        DataReference.String($"/users/{this.UID}/providerId", "No Provider").Set(this.ProviderID ?? "No Provider");
        DataReference.String($"/users/{this.UID}/displayName", "No Display Name").Set(this.DisplayName ?? "No Display Name");
        DataReference.String($"/users/{this.UID}/role", "user").DataChangedEvent += (newRole => {
            this.Role = newRole ?? "user";
            UserService.Service.NotifyUserUpdated();
        });

    }

    protected void InitTokenManager(JsonElement root) {
        if(root.TryGetProperty("stsTokenManager", out JsonElement tokenManager)) {
            this.TokenManager = tokenManager.GetProperty("refreshToken").GetString();
        }
    }

    internal static User Create(string loginResponse)
    {
        if (loginResponse == "null")
        {
            return new NoUser();
        }
        var jsonDocument = JsonDocument.Parse(loginResponse);
        if (jsonDocument.RootElement.TryGetProperty("providerData", out JsonElement providerData))
        {
            string providerId = providerData[0].GetProperty("providerId").GetString()!;
            try
            {
                return providerId switch
                {
                    "password" => new PasswordUser(jsonDocument),
                    "google.com" => new GoogleUser(jsonDocument),
                    "github.com" => new GitHubUser(jsonDocument),
                    _ => throw new Exception($"Invalid providerID: {providerId}")
                };
            }
            catch
            {
                NotificationService.Service.Add("An error occurred while logging in.", MudBlazor.Severity.Error).AndForget();
                UserService.Service.Logout().AndForget();
            }
        }
        return new NoUser();
    }
}