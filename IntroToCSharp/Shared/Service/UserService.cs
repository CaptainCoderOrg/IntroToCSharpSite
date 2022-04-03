using IntroToCSharp.Shared;
using Microsoft.JSInterop;

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
        MainLayout.OnInit += (JS, Snackbar) => Init(JS);
    }

    private IJSRuntime? _runtime;
    private User _userData = User.NoUser;
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

    private UserService() {}

    private void SetRuntime(IJSRuntime runtime)
    {
        this._runtime = runtime;
        // On initialization, register this object to be notified when the user changes
        runtime.InvokeVoidAsync("onAuthStateChanged", DotNetObjectReference.Create(this));
    }

    private async Task<IJSRuntime> GetRuntime()
    {
        await Task.Run(() => { while(this._runtime == null) Thread.Sleep(100); });
        return this._runtime!;
    }

    public async Task Login() => await (await GetRuntime()).InvokeVoidAsync("firebaseLogin");
    public async Task Logout() => await (await GetRuntime()).InvokeVoidAsync("firebaseLogout");

    [JSInvokable]
    /// Callback when the user changes. The response is a JSON object or "null" if the user is
    /// not authenticated.
    public void UpdateUser(string response) => UserData = new User(response);
}