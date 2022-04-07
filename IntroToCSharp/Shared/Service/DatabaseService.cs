using System.Text.Json;
using IntroToCSharp.Shared.Layout;
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
    private static void Init(IJSRuntime JS)
    {
        if (JS == null) throw new ArgumentNullException("JSRuntime must be non-null.");
        if (Service.JS != null) throw new InvalidOperationException("Cannot initialize database more than once.");
        Service.JS = JS;
    }

    static DatabaseService()
    {
        MainLayout.OnInit += (JS, Snackbar) => Init(JS);
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

/// <summary>
/// A IResultHandler is used to handle database events that can succeed
/// or fail.
/// </summary>
public interface IResultHandler
{
    static IResultHandler Default = new DefaultResultHandler();

    /// <summary>
    /// A JSInvokable method which is called when the database interaction is completed successfully.
    /// </summary>
    void OnSuccess();

    /// <summary>
    /// A JSInvokable method which is called when an exception is raised by the database.
    /// </summary>
    /// <param name="exception"></param>
    void OnException(string exception);
}

public class DefaultResultHandler : IResultHandler
{
    [JSInvokable]
    public async void OnException(string exception) => await NotificationService.Service.Add(exception, Severity.Warning);
    [JSInvokable]
    public void OnSuccess() { }
}

/// <summary>
/// A IChangeHandler is used to observe data within the database.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IChangeHandler
{
    /// <summary>
    /// A JSInvokable method which is called when a value change has been detected.
    /// </summary>
    /// <param name="result">A JSON string containing the new value.</param>
    void OnChange(string result);

    /// <summary>
    /// A JSInvokable method which is called when an exception is raised by the database.
    /// </summary>
    /// <param name="exception"></param>
    void OnException(string exception);
}

public class DataReference
{
    public static DataReference<bool> Bool(string path, bool defaultValue = false, string? niceName = null) => BoolDataReference.GetRef(path, defaultValue, niceName);
    public static DataReference<string> String(string path, string defaultValue = "", string? niceName = null) => StringDataReference.GetRef(path, defaultValue, niceName);
}

/// <summary>
/// A DataReference is a helper class for reading and writing data within the database.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class DataReference<T> : IResultHandler, IChangeHandler
{
    private readonly string? _niceName;
    private T? _defaultValue;
    private T? _val;
    private bool _hasVal = false;
    private bool _hasRef = false;
    private event Action<T?>? _dataChanged;
    private readonly string _path;

    /// <summary>
    /// The nice name for this data reference, if one exists.
    /// </summary>
    public string? NiceName => _niceName;

    /// <summary>
    /// The absolute path to this DataReference within the database.
    /// </summary>
    public string Path => _path;

    /// <summary>
    /// This event is triggered when the data at the specified path changes.
    /// </summary>
    public event Action<T?>? DataChanged
    {
        add
        {
            if (value == null) throw new ArgumentNullException("Cannot register null event listener.");
            if (_hasVal) value.Invoke(Val);
            _dataChanged += value;
            if (!_hasRef)
            {
                DatabaseService.Service.Ref(_path, this).AndForget();
                _hasRef = true;
            }
        }
        remove => _dataChanged -= value;
    }

    protected T? Val
    {
        get => _val;
        set
        {
            // If the current value is already equal to this value, no need to update.
            if (EqualityComparer<T>.Default.Equals(_val, value) && _hasVal) return;
            _val = value;
            _dataChanged?.Invoke(_val);
            _hasVal = true;
        }
    }

    /// <summary>
    /// Constructs a DataReference specifying the path within the database.
    /// </summary>
    /// <param name="path"></param>
    protected DataReference(string path, T? defaultValue = default(T), string? niceName = null)
    {
        this._path = path;
        this._defaultValue = defaultValue;
        this._niceName = niceName;
    }

    /// <summary>
    /// Attempts to remove the data from the database
    /// </summary>
    /// <returns></returns>
    public async void Remove(bool notifyOnSuccess = true)
    {
        IResultHandler handler = notifyOnSuccess ? this : IResultHandler.Default;
        await DatabaseService.Service.Remove(_path, handler);
    }

    /// <inheritdoc/>
    [JSInvokable]
    public virtual async void OnSuccess()
    {
        string message = "Data Synced";
        message += _niceName == null ? "" : $": {_niceName}";
        await NotificationService.Service.Add(message, Severity.Success);
    }

    /// <inheritdoc/>
    [JSInvokable]
    public virtual void OnException(string exception) => IResultHandler.Default.OnException(exception);

    /// <summary>
    /// Attempts to set the data value within the database. If notifyOnSuccess is set to true
    /// an on screen notification will be displayed when the Database returns successfully
    /// </summary>
    /// <param name="data">The value to set</param>
    /// <param name="notifyOnSuccess"></param>
    public abstract void Set(T data, bool notifyOnSuccess = true);

    /// <inheritdoc/>
    [JSInvokable]
    public void OnChange(string? data)
    {
        if (data == null)
        {
            Val = _defaultValue;
            return;
        }
        HandleChange(data);
    }

    protected abstract void HandleChange(string data);

}

internal class BoolDataReference : DataReference<bool>
{
    private static readonly Dictionary<string, BoolDataReference> DataRefs = new Dictionary<string, BoolDataReference>();
    internal static DataReference<bool> GetRef(string path, bool defaultValue = false, string? niceName = null)
    {
        if (!DataRefs.TryGetValue(path, out BoolDataReference? value))
        {
            value = new BoolDataReference(path, defaultValue, niceName);
        }
        return value;
    }
    private BoolDataReference(string path, bool defaultValue = false, string? niceName = null) : base(path, defaultValue, niceName) { }

    protected override void HandleChange(string data)
    {
        try
        {
            Val = JsonDocument.Parse(data).RootElement.GetBoolean();
        }
        catch (InvalidOperationException)
        {
            NotificationService.Service.Add($"Error loading {NiceName}. Expected bool but found: {data}", Severity.Error).AndForget();
        }
        catch
        {
            NotificationService.Service.Add($"Error loading {NiceName}.", Severity.Error).AndForget();
        }
    }

    public override async void Set(bool data, bool notifyOnSuccess = true)
    {
        IResultHandler handler = notifyOnSuccess ? this : IResultHandler.Default;
        await DatabaseService.Service.Set<bool>(Path, data, handler);
    }
}

internal class StringDataReference : DataReference<string>
{
    private static readonly Dictionary<string, StringDataReference> DataRefs = new Dictionary<string, StringDataReference>();
    internal static DataReference<string> GetRef(string path, string defaultValue = "", string? niceName = null)
    {
        if (!DataRefs.TryGetValue(path, out StringDataReference? value))
        {
            value = new StringDataReference(path, defaultValue, niceName);
        }
        return value;
    }
    private StringDataReference(string path, string defaultValue = "", string? niceName = null) : base(path, defaultValue, niceName) { }

    protected override void HandleChange(string data)
    {
        try
        {
            Val = JsonDocument.Parse(data).RootElement.GetString();
        }
        catch (InvalidOperationException)
        {
            NotificationService.Service.Add($"Error loading {NiceName}. Expected bool but found: {data}", Severity.Error).AndForget();
        }
        catch
        {
            NotificationService.Service.Add($"Error loading {NiceName}.", Severity.Error).AndForget();
        }
    }

    public override async void Set(string data, bool notifyOnSuccess = true)
    {
        IResultHandler handler = notifyOnSuccess ? this : IResultHandler.Default;
        await DatabaseService.Service.Set<string>(Path, data, handler);
    }
}