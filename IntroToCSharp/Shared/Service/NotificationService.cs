using MudBlazor;

public class NotificationService
{
    public static NotificationService Service { get; } = new NotificationService();

    public static void Init(ISnackbar snackbar)
    {
        if (snackbar == null) throw new ArgumentNullException("Snackbar must not be null.");
        if (Service._snackbar != null) throw new InvalidOperationException("Cannot initialize NotificationService multiple times.");
        Service._snackbar = snackbar;
    }

    private ISnackbar? _snackbar;
    public async Task<ISnackbar> GetSnackbar()
    {
        await Task.Run(() => { while (_snackbar == null ) Thread.Sleep(100); });
        return _snackbar!;
    }

    private NotificationService() {}

    /* Start: Delegate Methods for ISnackbar */
    public async Task<Snackbar> Add(string message, Severity severity = Severity.Normal, Action<SnackbarOptions> configure = null!) => (await GetSnackbar()).Add(message, severity, configure);
    public async void Clear() => (await GetSnackbar()).Clear();
    public async void Remove(Snackbar snackbar) => (await GetSnackbar()).Remove(snackbar);
    public async void Dispose() => (await GetSnackbar()).Dispose();
    /* End: Delegate Methods for ISnackbar */
}