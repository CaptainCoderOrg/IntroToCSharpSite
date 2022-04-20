using Blazored.LocalStorage;
using IntroToCSharp.Shared.Layout;
using Microsoft.JSInterop;
using MudBlazor;

public static class Utils
{
    const string BadChars = ".#$[]";
    private static IJSRuntime? s_JS;
    private static ISnackbar? s_SnackBar;
    private static ILocalStorageService? s_LocalStorage;

    static Utils()
    {
        MainLayout.OnInit += (JS, SnackBar, LocalStorage) => {
            Utils.s_JS = JS;
            Utils.s_SnackBar = SnackBar;
            Utils.s_LocalStorage = LocalStorage;
        };
    }

    public static async Task<IJSRuntime> GetJSRunTime()
    {
        await Task.Run(() => { while(s_JS == null) Thread.Sleep(10); } );
        return s_JS!;
    }

    public static async Task<ISnackbar> GetSnackbar()
    {
        await Task.Run(() => { while(s_JS == null) Thread.Sleep(10); } );
        return s_SnackBar!;
    }

    public static async Task<ILocalStorageService> GetLocalStorage()
    {
        await Task.Run(() => { while(s_LocalStorage == null) Thread.Sleep(10); } );
        return s_LocalStorage!;
    }

    /// <summary>
    /// Sanitize a database path by replacing illegal characters with underscores.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string SanitizeDBName(string name)
    {
        foreach (char ch in BadChars)
        {
            name = name.Replace(ch, '_');
        }
        return name;
    }

    public async static void ScrollToTop() => await (await GetJSRunTime()).InvokeVoidAsync("ScrollToTop");
}