using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

public class DatabaseService
{
    internal readonly IJSRuntime JS;
    private static DatabaseService? s_Database;

    public static async Task<DatabaseService> GetDatabase()
    {
        await Task.Run(() => { while (s_Database == null) Thread.Sleep(100); });
        return s_Database!;
    }

    public static void InitDatabase(IJSRuntime JS)
    {
        s_Database = new DatabaseService(JS);
    }

    private DatabaseService(IJSRuntime JS)
    {
        this.JS = JS;
    }
}

public class SetDataCall<T>
{
    private readonly string _path;
    public SetDataCall(string path)
    {
        this._path = path;
    }

    public async void Set(T data)
    {
        if (data == null) throw new NullReferenceException("Data cannot be null.");
        object[] args = {_path, data, DotNetObjectReference.Create(this)};
        DatabaseService db = await DatabaseService.GetDatabase();
        await db.JS.InvokeAsync<string>("setDatabase", args);
    }

    [JSInvokable]
    public virtual async void OnSuccess()
    {
       ISnackbar snackbar = await NotificationService.GetSnackbar();
       snackbar.Add("Data Synced.", Severity.Success);
    }

    [JSInvokable]
    public virtual async void OnException(string exception)
    {
        ISnackbar snackbar = await NotificationService.GetSnackbar();
        snackbar.Add(exception, Severity.Warning);
    }
}