using IntroToCSharp.Shared.Layout;
using MudBlazor;

/// <summary>
/// The NotificationService is a singleton that is used throughout
/// the application to display Snackbar messages to the user.
/// </summary>
public class NotificationService
{
    /// <summary>
    /// Gets the NotificationService
    /// </summary>
    public static NotificationService Service { get; } = new NotificationService();

    internal static void Debug(string message)
    {
        Service.Add(message, Severity.Warning).AndForget();
    }

    /// <summary>
    /// This method should be called once during initialization of the application
    /// to specify the ISnackbar to use for displaying messages.
    /// </summary>
    /// <param name="snackbar">The Snackbar to use for displaying messages.</param>
    private static void Init(ISnackbar snackbar)
    {
        if (snackbar == null) throw new ArgumentNullException("Snackbar must not be null.");
        if (Service._snackbar != null) throw new InvalidOperationException("Cannot initialize NotificationService multiple times.");
        Service._snackbar = snackbar;
    }

    static NotificationService()
    {
        MainLayout.OnInit += (JS, Snackbar) => Init(Snackbar);
    }

    private ISnackbar? _snackbar;

    /// <summary>
    /// Waits for the Snackbar service to be injected before returning it.
    /// </summary>
    /// <returns>The Snackbar service</returns>
    public async Task<ISnackbar> GetSnackbar()
    {
        await Task.Run(() => { while (_snackbar == null ) Thread.Sleep(100); });
        return _snackbar!;
    }

    private NotificationService() {}

    /* Start: Delegate Methods for ISnackbar */
    /// <summary>
    /// A convenience method for adding messages to the underlying Snackbar.
    /// </summary>2
    public async Task<Snackbar> Add(string message, Severity severity = Severity.Normal, Action<SnackbarOptions> configure = null!) => (await GetSnackbar()).Add(message, severity, configure);

    /// <summary>
    /// A convenience method for clearing messages from the underlying Snackbar.
    /// </summary>2
    public async void Clear() => (await GetSnackbar()).Clear();
    /* End: Delegate Methods for ISnackbar */
}