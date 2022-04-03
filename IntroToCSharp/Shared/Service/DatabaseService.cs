using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

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
    public static void Init(IJSRuntime JS)
    {
        if (JS == null) throw new ArgumentNullException("JSRuntime must be non-null.");
        if (Service.JS != null) throw new InvalidOperationException("Cannot initialize database more than once.");
        Service.JS = JS;
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
        if (data == null) throw new ArgumentNullException("Data cannot be null.");
        object[] args = { path, data!, DotNetObjectReference.Create(handler) };
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
    public async Task Ref<T>(string path, IChangeHandler<T> handler)
    {
        object[] args = { path, DotNetObjectReference.Create(handler) };
        var JS = await GetRuntime();
        await JS.InvokeVoidAsync("refDatabase", args);
    }
}

/// <summary>
/// A IResultHandler is used to handle database events that can succeed
/// or fail.
/// </summary>
public interface IResultHandler
{
    void OnSuccess();
    void OnException(string exception);
}

/// <summary>
/// A IChangeHandler is used to observe data within the database.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IChangeHandler<T>
{
    void OnChange(T result);
    void OnException(string exception);
}

/// <summary>
/// A DataReference is a helper class for reading and writing data within the database.
/// </summary>
/// <typeparam name="T"></typeparam>
public class DataReference<T> : IResultHandler, IChangeHandler<T>
{
    private readonly string _path;
    private T? _currentVal;
    private bool _hasVal = false;
    private bool _hasRef = false;
    private event Action<T>? _dataChanged;

    /// <summary>
    /// This event is triggered when the data at the specified path changes.
    /// </summary>
    public event Action<T>? DataChanged
    {
        add
        {
            if (value == null) throw new ArgumentNullException("Cannot register null event listener.");
            if (_hasVal) value.Invoke(CurrentVal);
            _dataChanged += value;
            if (!_hasRef)
            {
                DatabaseService.Service.Ref<T>(_path, this).AndForget();
                _hasRef = true;
            }
        }
        remove => _dataChanged -= value;
    }

    private T CurrentVal
    {
        get
        {
            if (_currentVal == null) throw new InvalidOperationException("No CurrentVal found.");
            return _currentVal;
        }
        set
        {
            _currentVal = value;
            _hasVal = true;
        }
    }

    /// <summary>
    /// Constructs a DataReference specifying the path within the database.
    /// </summary>
    /// <param name="path"></param>
    public DataReference(string path)
    {
        this._path = path;
    }

    /// <summary>
    /// Attempts to set the data value within the database.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public async void Set(T data)
    {
        if (data == null) throw new NullReferenceException("Data cannot be null.");
        await DatabaseService.Service.Set<T>(_path, data, this);
    }

    /// <summary>
    /// Attempts to remove the data from the database
    /// </summary>
    /// <returns></returns>
    public async void Remove() => await DatabaseService.Service.Remove(_path, this);

    [JSInvokable]
    public virtual async void OnSuccess() => await NotificationService.Service.Add("Data Synced.", Severity.Success);

    [JSInvokable]
    public virtual async void OnException(string exception) => await NotificationService.Service.Add(exception, Severity.Warning);

    [JSInvokable]
    public virtual void OnChange(T data)
    {
        CurrentVal = data;
        _dataChanged?.Invoke(data);
    }
}