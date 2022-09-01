using IntroToCSharp.Shared.Layout;
using Microsoft.JSInterop;
using System.Text.Json;

namespace CaptainCoder;

/// <summary>
/// UserService provides methods for keeping track of the user and preforming important actions. It is constructed without parameters.
/// </summary>
public class UserService
{
    /// <summary>
    /// A new UserService.
    /// </summary>
    /// <returns>A new UserService.</returns>
    public static UserService Service { get; } = new UserService();

    private static void Init(IJSRuntime JS)
    {
        if (JS == null) throw new ArgumentNullException(nameof(JS), "IJSRuntime cannot be null.");
        if (Service._runtime != null) throw new InvalidOperationException("UserService cannot be initialized multiple times.");
        Service.SetRuntime(JS);
    }

    static UserService()
    {
        MainLayout.OnInit += (JS, _, _) => Init(JS);
    }

    private IJSRuntime? _runtime;
    private User _userData = User.NoUser;
    private UserStats _userStats = CaptainCoder.UserStats.Default;
    private UserInventory _userInventory = CaptainCoder.UserInventory.Default;
    private UserPages _userPages = CaptainCoder.UserPages.Default;
    private event Action<User>? UserChangedEvent;
    public event Action<User> OnUserChange
    {
        add
        {
            if (value == null) throw new ArgumentNullException(nameof(value), "Cannot register null event.");
            value.Invoke(UserData);
            UserChangedEvent += value;
        }
        remove => UserChangedEvent -= value;
    }

    private User UserData
    {
        get => _userData;
        set
        {
            _userData = value;
            UserChangedEvent?.Invoke(value);
        }
    }

    public void NotifyUserUpdated() => UserChangedEvent?.Invoke(UserData);

    private User? _originalUser;

    private UserService() { }

    public void LoginAsPsuedoUser(string uid)
    {
        _originalUser = _userData;
        UserData = new PseudoUser(uid);
    }

    public void LogoutFromPsuedoUser()
    {
        if (UserData.GetType() != typeof(PseudoUser)) return;
        UserData = _originalUser ?? User.NoUser;
    }

    private void SetRuntime(IJSRuntime runtime)
    {
        this._runtime = runtime;
        // On initialization, register this object to be notified when the user changes
        runtime.InvokeVoidAsync("onAuthStateChanged", DotNetObjectReference.Create(this));
    }

    private async Task<IJSRuntime> GetRuntime()
    {
        await Task.Run(() => { while (this._runtime == null) Thread.Sleep(100); });
        return this._runtime!;
    }

    public async Task GoogleLogin() => await (await GetRuntime()).InvokeVoidAsync("firebaseGoogleLogin");
    public async Task GitHubLogin() => await (await GetRuntime()).InvokeVoidAsync("firebaseGitHubLogin");
    public async Task EmailLogin(string email, string password) => await (await GetRuntime()).InvokeVoidAsync("firebaseEmailLogin", email, password);
    public async Task Logout() => await (await GetRuntime()).InvokeVoidAsync("firebaseLogout");

    public User GetNonUpdatingUser() => _userData;

    [JSInvokable]
    /// <summary>
    /// Callback when the user changes. The response is a JSON object or "null" if the user is not authenticated.
    /// </summary>
    /// <param name="response">The response</param>
    public void UpdateUser(string response)
    {
        UserData = User.Create(response);
        if (UserData.UserStatsRef != null)
        {
            UserData.UserStatsRef.DataChangedEvent += (userStats) => this._userStats = userStats!;
        }
        if (UserData.UserPagesRef != null)
        {
            UserData.UserPagesRef.DataChangedEvent += (userPages) => this._userPages = userPages!;
        }
        if (UserData.UserInventoryRef != null)
        {
            UserData.UserInventoryRef.DataChangedEvent += (userInventory) =>
            {
                this._userInventory = userInventory!;
            };
        }

    }

    /// <summary>
    /// Sets the DarkModePreference of the current user
    /// </summary>
    public void UpdateDarkModePreference(bool value) => _userData.DarkMode?.Set(value, true);

    /// <summary>
    /// Updates the ProjectData of the current user
    /// </summary>
    /// <param name="projects"></param>
    public void UpdateProjectData(Dictionary<string, string> projects) => _userData.ProjectData?.Set(JsonSerializer.Serialize(projects));
    public void UpdateDefaultProject(string projectName) => _userData.DefaultProject?.Set(projectName);

    /// <summary>
    /// Given a path to a question and an answer to that question, updates the
    /// answer in the database.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="answer"></param>
    public void UpdateQuestionAnswer(string path, string answer)
    {
        if (_userData.IsLoggedIn)
        {
            DataReference<string> reference = DataReference.String($"/users/{_userData.UID}/{path}");
            reference.Set(answer);
        }
    }

    public DataReference<bool>? GetAdventureActivityReference(IAdventureActivity activity)
    {
        if (!_userData.IsLoggedIn) return null;
        return DataReference.Bool($"/users/{_userData.UID}/adventure-activity/{activity.DBName}");
    }

    /// <summary>
    /// Given a relative path for the logged in user, saves the specified
    /// data as a JSON object. If the object cannot be converted to JSON,
    /// this method will fail.
    /// </summary>
    public void SaveJsonData<T>(string path, T data)
    {
        if (_userData.IsLoggedIn)
        {
            DataReference<T> reference = DataReference.Json<T>($"/users/{_userData.UID}/{path}");
            reference.Set(data);
        }
    }

    /// <summary>
    /// Given a relative path for the logged in user, retrieves a
    /// DataReference to an object in the database at that path.
    /// If the user is not logged in, this method returns null.
    /// </summary>
    public DataReference<T>? GetJsonDataReference<T>(string path, string? niceName = null)
    {
        if (_userData.IsLoggedIn)
        {
            DataReference<T> reference = DataReference.Json<T>($"/users/{_userData.UID}/{path}", default, niceName);
            return reference;
        }
        return null;
    }

    /// <summary>
    /// Attempts to get a reference to an answer in the database. This method
    /// returns true if there is a user logged in and they can access the
    /// specified path. Otherwise, this method returns false and the reference
    /// is not set.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="reference"></param>
    /// <returns></returns>
    public bool TryGetQuestionAnswer(string path, out DataReference<string> reference)
    {
        reference = null!;
        if (!_userData.IsLoggedIn) return false;
        reference = DataReference.String($"/users/{_userData.UID}/{path}");
        return true;
    }

    /// <summary>
    /// Given a path to a task and the state of the task, updates the
    /// task in the database.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="answer"></param>
    public void UpdateTaskItem(string path, bool isChecked)
    {
        if (_userData.IsLoggedIn)
        {
            DataReference<bool> reference = DataReference.Bool($"/users/{_userData.UID}/{path}");
            reference.Set(isChecked);
        }
    }

    /// <summary>
    /// Attempts to get a reference to a task in the database. This method
    /// returns true if there is a user logged in and they can access the
    /// specified path. Otherwise, this method returns false and the reference
    /// is not set.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="reference"></param>
    /// <returns></returns>
    public bool TryGetTask(string path, out DataReference<bool> reference)
    {
        reference = null!;
        if (!_userData.IsLoggedIn) return false;
        reference = DataReference.Bool($"/users/{_userData.UID}/{path}");
        return true;
    }

    public bool GiveXPAndGold(int xpToGive, int goldToGive)
    {
        int goldAcquired = _userStats.GoldAcquired + Math.Max(0, goldToGive);
        int goldSpent = _userStats.GoldAcquired + Math.Min(0, goldToGive);
        UserStats newStats = new(_userStats.XP + xpToGive, _userStats.GoldAcquired + goldToGive, _userStats.GoldSpent);
        _userData.UserStatsRef?.Set(newStats);
        return true;
    }

    /// <summary>
    /// Updates the current users XP by adding the specified amount of XP to
    /// the User. This updates the reference in the database.
    /// </summary>
    /// <param name="xpToGive">The amount to give (or remove)</param>
    public bool GiveXP(int xpToGive) => GiveXPAndGold(xpToGive, 0);

    /// <summary>
    /// Updates the current users Gold by adding the specified amount of Gold to
    /// the User. This updates the reference in the database.
    /// </summary>
    /// <param name="goldToGive">The amount of gold to give(or remove)</param>
    /// <returns>False if user is not logged in. Returns true otherwise.</returns>
    public bool GiveGold(int goldToGive) => GiveXPAndGold(0, goldToGive);

    /// <summary>
    /// Updates the users gold based on if the user buys the item from the store. The item is added to the user's inventory.
    /// </summary>
    /// <param name="toBuy">The item to buy.</param>
    /// <returns>False if the user is not logged in. Returns true otherwise.</returns>
    public bool BuyItem(StoreItem toBuy)
    {
        if (!_userData.IsLoggedIn) return false;
        this.GiveGold(-toBuy.Cost);
        UserInventory newInventory = _userInventory.AddItem(toBuy);
        _userData.UserInventoryRef?.Set(newInventory);
        return true;
    }
    /// <summary>
    /// Updates the user's gold based on if the user sells the item. And removes the given item from the user's inventory.
    /// </summary>
    /// <param name="toSell">The item to sell.</param>
    /// <param name="value">The amount the item is sold for.</param>
    /// <returns>False if user is not logged in. Returns true otherwise.</returns>
    public bool SellItem(StoreItem toSell, int value)
    {
        if (!_userData.IsLoggedIn) return false;
        this.GiveGold(value);
        UserInventory newInventory = _userInventory.RemoveItem(toSell);
        _userData.UserInventoryRef?.Set(newInventory);
        return true;
    }

    public bool AddPage(PageRef toAdd)
    {
        if (!_userData.IsLoggedIn) return false;
        if (_userData.UserPagesRef == null) return false;
        _userPages.AddPage(toAdd.Name, PageStatus.New);
        _userData.UserPagesRef.Set(_userPages);
        return true;
    }

    public bool UpdatePage(PageRef toUpdate, PageStatus newStatus)
    {
        if (!_userData.IsLoggedIn) return false;
        if (_userData.UserPagesRef == null) return false;
        _userPages.AddPage(toUpdate.Name, newStatus);
        _userData.UserPagesRef.Set(_userPages);
        return true;
    }

    public bool ResetBook()
    {
        if (!_userData.IsLoggedIn) return false;
        if (_userData.UserPagesRef == null) return false;
        _userPages = UserPages.Default;
        _userData.UserPagesRef.Set(_userPages);
        return true;
    }
}