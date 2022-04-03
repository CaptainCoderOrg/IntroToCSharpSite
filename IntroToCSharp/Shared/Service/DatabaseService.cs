using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

public class DatabaseService
{
    private IJSRuntime? JS;
    public static DatabaseService Service { get; } = new DatabaseService();

    public static void Init(IJSRuntime JS)
    {
        if (JS == null) throw new ArgumentNullException("JSRuntime must be non-null.");
        if (Service.JS != null) throw new InvalidOperationException("Cannot initialize database more than once.");
        Service.JS = JS;
    }

    private DatabaseService() {}

    private async Task<IJSRuntime> GetRuntime()
    {
        await Task.Run(() => {while (JS == null) Thread.Sleep(100); });
        return JS!;
    }

    public async Task Set<T>(string path, T data, IResultHandler handler)
    {
        if (data == null) throw new ArgumentNullException("Data cannot be null.");
        object[] args = {path, data!, DotNetObjectReference.Create(handler)};
        var JS = await GetRuntime();
        await JS.InvokeVoidAsync("setDatabase", args);
    }

    public async Task Remove(string path, IResultHandler handler)
    {
        object[] args = {path, DotNetObjectReference.Create(handler)};
        var JS = await GetRuntime();
        await JS.InvokeVoidAsync("removeDatabase", args);
    }

    public async Task Ref<T>(string path, IChangeHandler<T> handler)
    {
        object[] args = {path, DotNetObjectReference.Create(handler)};
        var JS = await GetRuntime();
        await JS.InvokeVoidAsync("refDatabase", args);
    }
}

public interface IResultHandler
{
    void OnSuccess();
    void OnException(string exception);
}

public interface IChangeHandler<T>
{
    void OnChange(T result);
    void OnException(string exception);
}

public class DataReference<T> : IResultHandler, IChangeHandler<T>
{
    private readonly string _path;
    public event Action<T>? DataChanged;
    public DataReference(string path)
    {
        this._path = path;
        // TODO: Only call this the first time something is subscribed to DataChanged
        DatabaseService.Service.Ref<T>(path, this).AndForget();
    }

    public async void Set(T data)
    {
        if (data == null) throw new NullReferenceException("Data cannot be null.");
        await DatabaseService.Service.Set<T>(_path, data, this);
    }

    public async void Remove() => await DatabaseService.Service.Remove(_path, this);

    [JSInvokable]
    public virtual async void OnSuccess() => await NotificationService.Service.Add("Data Synced.", Severity.Success);

    [JSInvokable]
    public virtual async void OnException(string exception) => await NotificationService.Service.Add(exception, Severity.Warning);

    [JSInvokable]
    public virtual void OnChange(T data) => DataChanged?.Invoke(data);
}