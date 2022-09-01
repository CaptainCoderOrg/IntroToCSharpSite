using System.Text.Json;
using IntroToCSharp.Shared.Layout;
using Microsoft.JSInterop;
using MudBlazor;

namespace CaptainCoder;

/// <summary>
/// The DatabaseService is a singleton service has direct access
/// to setting, removing, and referencing data. For most purposes,
/// the <see cref="DataReference">DataReference</see> should be
/// used for reading and writing to the database.
/// </summary>
public class DatabaseService
{
    /// <summary>
    /// Retrieves the DatabaseService
    /// </summary>
    public static DatabaseService Service { get; } = new DatabaseService();
    internal List<UserInfo>? _userInfo = null;
    private IJSRuntime? JS;

    /// <summary>
    /// Initializes the DatabaseService. This method should be called once during
    /// initialization of the application.
    /// </summary>
    private static void Init(IJSRuntime JS)
    {
        if (Service.JS != null) throw new InvalidOperationException("Cannot initialize database more than once.");
        Service.JS = JS ?? throw new ArgumentNullException(nameof(JS), "JSRuntime must be non-null.");
    }

    static DatabaseService()
    {
        MainLayout.OnInit += (JS, _, _) => Init(JS);
    }

    private DatabaseService() { }

    private async Task<IJSRuntime> GetRuntime()
    {
        await Task.Run(() => { while (JS == null) Thread.Sleep(100); });
        return JS!;
    }

    /// <summary>
    /// Given the path and data to set, attempts to set the value in the database.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="data"></param>
    /// <param name="handler"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task Set<T>(string path, T data, IResultHandler handler)
    {
        if (data == null) throw new ArgumentNullException(nameof(data), "Data cannot be null.");
        object[] args = { path, JsonSerializer.Serialize(data), DotNetObjectReference.Create(handler) };
        var JS = await GetRuntime();
        await JS.InvokeVoidAsync("setDatabase", args);
    }

    /// <summary>
    /// Given the path to remove, attempts to remove the value from the database.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    /// <returns></returns>
    public async Task Remove(string path, IResultHandler handler)
    {
        object[] args = { path, DotNetObjectReference.Create(handler) };
        var JS = await GetRuntime();
        await JS.InvokeVoidAsync("removeDatabase", args);
    }

    /// <summary>
    /// Given a path, attempts to reference the path in the database.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task Ref(string path, IChangeHandler handler)
    {
        object[] args = { path, DotNetObjectReference.Create(handler) };
        var JS = await GetRuntime();
        await JS.InvokeVoidAsync("refDatabase", args);
    }

    /// <summary>
    /// Queries the database specifically for ALL user info. This
    /// happens exactly once and the data is cached. Multiple calls
    /// to this method will not query the database a second time.
    /// </summary>
    public async Task GetUserInfo(Action<List<UserInfo>> callback)
    {
        if (_userInfo != null)
        {
            callback(_userInfo);
            return;
        }
        IChangeHandler handler = new UserInfoHandler(callback);
        object[] args = { "/users", DotNetObjectReference.Create(handler) };
        var JS = await GetRuntime();
        await JS.InvokeVoidAsync("getDatabase", args);
    }
}

internal class UserInfoHandler : IChangeHandler
{
    private Action<List<UserInfo>> callback;

    internal UserInfoHandler(Action<List<UserInfo>> callback)
    {
        this.callback = callback;
    }
    [JSInvokable]
    public void OnChange(string result)
    {
        List<UserInfo> data = new ();
        var jsonDocument = JsonDocument.Parse(result);
        foreach(var el in jsonDocument.RootElement.EnumerateObject())
        {
            if (el.Value.TryGetProperty("email", out JsonElement email)) {
                data.Add(new UserInfo(el.Name, email.GetString()!));
            }
        }
        data.Sort((a, b) => a.Email.CompareTo(b.Email));
        DatabaseService.Service._userInfo = data;
        callback(DatabaseService.Service._userInfo);
    }

    [JSInvokable]
    public void OnException(string exception)
    {
        throw new NotImplementedException();
    }
}