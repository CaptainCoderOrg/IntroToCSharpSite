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
}