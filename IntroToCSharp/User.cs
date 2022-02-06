using System.Text.Json;
/// <summary>
/// A User contains information about the current user of the application
/// </summary>
public class User
{
    /// <summary>
    /// The User's display name
    /// </summary>
    /// <value></value>
    public string DisplayName { get; }

    /// <summary>
    /// The User's email address
    /// </summary>
    /// <value></value>
    public string Email { get; }

    /// <summary>
    /// true if the user is logged in and false otherwise
    /// </summary>
    /// <value></value>
    public bool IsLoggedIn { get; }

    /// <summary>
    /// Given an authentication response, constructs a User object from that JSON data.
    /// </summary>
    /// <param name="response">"null" if the user is not authenticated otherwise a JSON string with 'displayName' and 'email'</param>
    public User(string response)
    {
        if (response == "null")
        {
            this.DisplayName = null;
            this.Email = null;
            this.IsLoggedIn = false;
        }
        else
        {
            var result = JsonDocument.Parse(response);
            this.DisplayName = result.RootElement.GetProperty("displayName").GetString();
            this.Email = result.RootElement.GetProperty("email").GetString();
            this.IsLoggedIn = true;
        }
    }

    public override string ToString()
    {
        return $"User {{ {DisplayName}, {Email} }}";
    }
}