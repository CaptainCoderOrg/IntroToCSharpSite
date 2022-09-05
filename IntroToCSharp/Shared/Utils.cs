using Blazored.LocalStorage;
using IntroToCSharp.Shared.Layout;
using Microsoft.JSInterop;
using MudBlazor;

public static class Utils
{
    const string BadChars = ".#$[]',!?";
    private static IJSRuntime? s_JS;
    private static ISnackbar? s_SnackBar;
    private static ILocalStorageService? s_LocalStorage;

    static Utils()
    {
        MainLayout.OnInit += (JS, SnackBar, LocalStorage) =>
        {
            Utils.s_JS = JS;
            Utils.s_SnackBar = SnackBar;
            Utils.s_LocalStorage = LocalStorage;
        };
    }

    public static string FormatGold(int amount)
    {
        if (amount < 1_000)
        {
            return $"{amount}";
        }
        if (amount < 1_000_000)
        {
            string hundreds = $"{amount % 1000}".PadLeft(3, '0');
            string thousands = $"{amount / 1000}";
            return $"{thousands},{hundreds}";
        }
        return FormatGold(amount / 1000, amount % 1000, 1);
    }

    private static string FormatGold(int amount, int mod, int thousands)
    {
        if (amount >= 1000)
        {
            return FormatGold(amount / 1000, amount % 1000, thousands + 1);
        }
        string[] vals = { "h", "t", "M", "B", "T", "q", "Q", "s", "S" };
        string dec = $"{mod}".PadLeft(3, '0');
        return $"{amount}.{dec} {vals[thousands]}";
    }

    public static async Task<IJSRuntime> GetJSRunTime()
    {
        await Task.Run(() => { while (s_JS == null) Thread.Sleep(10); });
        return s_JS!;
    }

    public static async Task<ISnackbar> GetSnackbar()
    {
        await Task.Run(() => { while (s_JS == null) Thread.Sleep(10); });
        return s_SnackBar!;
    }

    public static async Task<ILocalStorageService> GetLocalStorage()
    {
        await Task.Run(() => { while (s_LocalStorage == null) Thread.Sleep(10); });
        return s_LocalStorage!;
    }

    /// <summary>
    /// Returns true if the browser client is a mobile device and false otherwise.
    /// </summary>
    public static async Task<bool> IsMobileClient()
    {
        IJSRuntime js = await GetJSRunTime();
        bool isMobileClient = await js.InvokeAsync<bool>("IsMobileClient");
        return isMobileClient;
    }

    /// <summary>
    /// Retrieves the browser client information
    /// </summary>
    public static async Task<string> GetClient()
    {
        IJSRuntime js = await GetJSRunTime();
        string client = await js.InvokeAsync<string>("GetClient");
        return client;
    }

    public static int RoundToNearest(this int amount, int roundAmount) => roundAmount * (amount / roundAmount);

    public static string GetAOrAn(string word) {
        if (word == string.Empty) return "a";
        char first = word.ToLower()[0];
        if ("aeiou".Contains(first)) return "an";
        return "a";
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
    public static void ScrollIntoView(string id) {
        GetJSRunTime().ContinueWith(runtime => runtime.Result.InvokeVoidAsync("ScrollIntoView", id).AndForget());
    }
}