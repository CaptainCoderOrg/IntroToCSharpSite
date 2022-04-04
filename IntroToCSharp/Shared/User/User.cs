using System.Text.Json;
/// <summary>
/// A User contains information about the current user of the application
/// </summary>
public class User
{
    public static User NoUser = new User("null");
    /// <summary>
    /// The User's display name
    /// </summary>
    /// <value></value>
    public string? DisplayName { get; }

    public string? UID { get; }

    /// <summary>
    /// The first letter of the User's display name.
    /// </summary>
    public char AvatarLetter => DisplayName != null ? DisplayName.ToUpper()[0] : '?';

    /// <summary>
    /// The User's email address
    /// </summary>
    /// <value></value>
    public string? Email { get; }

    /// <summary>
    /// true if the user is logged in and false otherwise
    /// </summary>
    /// <value></value>
    public bool IsLoggedIn { get; }

    /// <summary>
    /// A Reference to this users DarkMode preference.
    /// </summary>
    public DataReference<bool>? DarkMode;

    /// <summary>
    /// Given an authentication response, constructs a User object from that JSON data.
    /// </summary>
    /// <param name="response">"null" if the user is not authenticated otherwise a JSON string with 'displayName' and 'email'</param>
    public User(string response)
    {
        if (response == "null")
        {
            this.DisplayName = null;
            this.UID = null;
            this.Email = null;
            this.IsLoggedIn = false;
            this.DarkMode = null;
        }
        else
        {
            var result = JsonDocument.Parse(response);
            this.UID = result.RootElement.GetProperty("uid").GetString();
            this.DisplayName = result.RootElement.GetProperty("displayName").GetString();
            // TODO: Detect which service authenticated this user (Google / Github provided different values)
            // this.Email = result.RootElement.GetProperty("email").GetString();
            this.IsLoggedIn = true;
            // TODO(jcollard 2022-04-04): default to localstorage.DarkMode?
            this.DarkMode = DataReference.Bool($"/users/{this.UID}/prefs/DarkMode", false, "Dark Mode");
        }
    }

    public override string ToString()
    {
        return $"User {{ {DisplayName}, {Email} }}";
    }
}