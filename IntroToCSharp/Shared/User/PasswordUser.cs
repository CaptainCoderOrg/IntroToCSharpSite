using System.Text.Json;

namespace CaptainCoder;
public class PasswordUser : User
{

    public PasswordUser(JsonDocument loginData)
    {
        this.UID = loginData.RootElement.GetProperty("user").GetProperty("uid").GetString();
        this.DisplayName = loginData.RootElement.GetProperty("user").GetProperty("email").GetString();
        this.DoLogin();
    }

}