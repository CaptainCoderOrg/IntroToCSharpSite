namespace CaptainCoder;

public class PseudoUser : User
{
    public PseudoUser(string uid) : base()
    {
        this.UID = uid;
        this.IsLoggedIn = true;
        this.Role = "admin";
        this.DarkMode = DataReference.Bool($"/users/{this.UID}/prefs/DarkMode", false, "Dark Mode").View;
        this.UserStatsRef = DataReference.Json<UserStats>($"/users/{this.UID}/users_stats", UserStats.Default, "User Stats").View;
        this.UserInventoryRef = DataReference.Json<UserInventory>($"/users/{this.UID}/inventory", UserInventory.Default, "User Inventory").View;
        this.UserPagesRef = DataReference.Json<UserPages>($"/users/{this.UID}/pages", UserPages.Default, "Book").View;
        DataReference<string> displayNameRef = DataReference.String($"/users/{uid}/displayName", "User Not Found", "Display Name").View;
        this.DisplayName = "Pseudo: ...";
        displayNameRef.DataChangedEvent += (displayName) => {
            this.DisplayName = $"Pseduo: {displayName}";
            UserService.Service.NotifyUserUpdated();
        };
    }
}