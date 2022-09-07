using System.Text.Json;
using MudBlazor;

namespace CaptainCoder;

public class GitHubUser : User
{
    public GitHubUser(JsonDocument loginData)
    {
        try {
            this.UID = loginData.RootElement.GetProperty("uid").GetString();
            JsonElement providerData = loginData.RootElement.GetProperty("providerData");
            this.DisplayName = loginData.RootElement.GetProperty("providerData")[0].GetProperty("displayName").GetString();
            this.ProviderID = providerData[0].GetProperty("providerId").ToString();
            this.Email = providerData[0].GetProperty("email").ToString();
            this.DoLogin(loginData.RootElement);
         }
        catch {
            NotificationService.Service.Add("An error occurred while logging into GitHub.", MudBlazor.Severity.Error).AndForget();
            UserService.Service.Logout().AndForget();
        }
    }
}