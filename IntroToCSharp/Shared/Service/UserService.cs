using Microsoft.JSInterop;

public class UserService
{
    public static UserService Service { get; } = new UserService();

    public static void Init(IJSRuntime JS)
    {
        if (JS == null) throw new ArgumentNullException("IJSRuntime cannot be null.");
        if (Service._runtime != null) throw new InvalidOperationException("UserService cannot be initialized multiple times.");
        Service.SetRuntime(JS);
    }

    private IJSRuntime? _runtime;
    private User? UserData;
    public event Action<User>? OnLogin;
    public event Action? OnLogout;

    private UserService() {}

    private void SetRuntime(IJSRuntime runtime)
    {
        this._runtime = runtime;
        // On initialization, register this object to be notified when the user changes
        runtime.InvokeVoidAsync("onAuthStateChanged", DotNetObjectReference.Create(this));
    }

    [JSInvokable]
    /// Callback when the user changes. The response is a JSON object or "null" if the user is
    /// not authenticated.
    public void UpdateUser(string response)
    {
        UserData = new User(response);

        if (UserData.UID != null)
        {
            OnLogin?.Invoke(UserData);
        }
        else
        {
            OnLogout?.Invoke();
        }
    }
}