using CaptainCoder;
using IntroToCSharp.Shared.Layout;
using Microsoft.JSInterop;
using System.Text.Json;

public class UserService
{
    public static UserService Service { get; } = new UserService();

    private static void Init(IJSRuntime JS)
    {
        if (JS == null) throw new ArgumentNullException("IJSRuntime cannot be null.");
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
    private event Action<User>? _onUserChange;
    public event Action<User> OnUserChange
    {
        add
        {
            if (value == null) throw new ArgumentNullException("Cannot register null event.");
            value.Invoke(UserData);
            _onUserChange += value;
        }
        remove => _onUserChange -= value;
    }

    private User UserData
    {
        get => _userData;
        set
        {
            _userData = value;
            _onUserChange?.Invoke(value);
        }
    }

    private UserService() { }

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
    public async Task Logout() => await (await GetRuntime()).InvokeVoidAsync("firebaseLogout");

    [JSInvokable]
    /// Callback when the user changes. The response is a JSON object or "null" if the user is
    /// not authenticated.
    public void UpdateUser(string response)
    {
        UserData = new User(response);
        if (UserData.UserStatsRef != null)
        {
            UserData.UserStatsRef.DataChanged += (userStats) => this._userStats = userStats!;
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

    /// <summary>
    /// Updates the current users XP by adding the specified amount of XP to
    /// the User. This updates the reference in the database.
    /// </summary>
    /// <param name="xpToGive">The amount to give (or remove)</param>
    public bool GiveXP(int xpToGive) {
        if (!_userData.IsLoggedIn) return false;
        UserStats newStats = new (_userStats.XP + xpToGive);
        _userData.UserStatsRef?.Set(newStats);
        return true;
    }
}