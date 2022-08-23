using System.Text.Json;

namespace CaptainCoder;

public class GoogleUser : User
{
    public GoogleUser(JsonDocument loginData)
    {
        this.UID = loginData.RootElement.GetProperty("uid").GetString();
        this.DisplayName = loginData.RootElement.GetProperty("displayName").GetString();
        this.Email = loginData.RootElement.GetProperty("email").GetString();
        this.DoLogin();
    }
}