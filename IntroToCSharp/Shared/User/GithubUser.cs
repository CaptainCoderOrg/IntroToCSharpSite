using System.Text.Json;
using MudBlazor;

namespace CaptainCoder;

public class GitHubUser : User
{
    public GitHubUser(JsonDocument loginData)
    {
        this.UID = loginData.RootElement.GetProperty("uid").GetString();
        JsonElement providerData = loginData.RootElement.GetProperty("providerData");        
        this.DisplayName = SafeGetDisplayName(loginData.RootElement);
        
        this.DoLogin();
    }

    private string? SafeGetDisplayName(JsonElement root) {
        try {
            return root.GetProperty("providerData")[0].GetProperty("displayName").GetString();
        }
        catch {
            NotificationService.Service.Add("An error occurred while logging into GitHub.", MudBlazor.Severity.Error).AndForget();
            UserService.Service.Logout().AndForget();
            return null;
        }
    }
}