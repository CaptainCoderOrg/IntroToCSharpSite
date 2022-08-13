using System.Text.Json;

namespace CaptainCoder;

public class GitHubUser : User
{
    public GitHubUser(JsonDocument loginData)
    {
        this.UID = loginData.RootElement.GetProperty("uid").GetString();
        this.DisplayName = loginData.RootElement.GetProperty("displayName").GetString();
        this.DoLogin();
    }
}