using MudBlazor;

public class NotificationService
{
    private static ISnackbar? s_Snackbar;

    public static async Task<ISnackbar> GetSnackbar()
    {
        await Task.Run(() => { while (s_Snackbar == null) Thread.Sleep(100); });
        return s_Snackbar!;
    }

    public static void InitSnackbar(ISnackbar snackbar)
    {
        s_Snackbar = snackbar;
    }
}