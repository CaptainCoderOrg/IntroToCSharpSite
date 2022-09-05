using System.Text.Json;
using Microsoft.JSInterop;
using MudBlazor;

namespace CaptainCoder;

/// <summary>
/// The DataReference class provides static factory methods for creating and obtaining references to objects in the database.
/// </summary>
public static class DataReference
{
    public static DataReference<bool> Bool(string path, bool defaultValue = false, string? niceName = null) => BoolDataReference.GetRef(path, defaultValue, niceName);
    public static DataReference<string> String(string path, string defaultValue = "", string? niceName = null) => StringDataReference.GetRef(path, defaultValue, niceName);
    public static DataReference<T> Json<T>(string path, T? defaultValue = default, string? niceName = null) => new JsonDataReference<T>(path, defaultValue, niceName);
}

public class DataReferenceView<T> : DataReference<T>
{
    public override event Action<T?>? DataChangedEvent
    {
        add => this.reference.DataChangedEvent += value;
        remove => this.reference.DataChangedEvent -= value;
    }

    internal override T? Val  {get => this.reference.Val; set {}}

    private DataReference<T> reference;

    public DataReferenceView(DataReference<T> reference) : base(reference.Path, reference.DefaultValue, reference.NiceName)
    {
        this.reference = reference;
    }

    public override async Task<T?> GetValue()
    {
        T? result = await reference.GetValue();
        return result;
    }

    public override void Remove(bool notifyOnSuccess = true) { }
    [JSInvokable]
    public override void OnSuccess() => this.reference.OnSuccess();
    [JSInvokable]
    public override void OnException(string exception) => this.reference.OnException(exception);

    public override void Set(T data, bool notifyOnSuccess = true) { }

    [JSInvokable]
    public override void OnChange(string? data) => this.reference.OnChange(data);

    internal override void HandleChange(string data) => this.reference.HandleChange(data);
}

/// <summary>
/// A DataReference is a helper class for reading and writing data within the database.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class DataReference<T> : IResultHandler, IChangeHandler
{
    private readonly string? _niceName;
    private readonly T? _defaultValue;
    private T? _val;
    private bool _hasVal = false;
    private bool _hasRef = false;
    private event Action<T?>? DataChanged;
    private readonly string _path;

    public DataReference<T> View => new DataReferenceView<T>(this);

    /// <summary>
    /// The nice name for this data reference, if one exists.
    /// </summary>
    public string? NiceName => _niceName;

    /// <summary>
    /// The absolute path to this DataReference within the database.
    /// </summary>
    public string Path => _path;

    public T? DefaultValue => _defaultValue;

    /// <summary>
    /// This event is triggered when the data at the specified path changes.
    /// </summary>
    public virtual event Action<T?>? DataChangedEvent
    {
        add
        {
            if (value == null) throw new ArgumentNullException(nameof(value), "Cannot register null event listener.");
            if (_hasVal) value.Invoke(Val);
            DataChanged += value;
            if (!_hasRef)
            {
                DatabaseService.Service.Ref(_path, this).AndForget();
                _hasRef = true;
            }
        }
        remove => DataChanged -= value;
    }

    public virtual async Task<T?> GetValue()
    {
        if (_hasRef) return _val;
        await DatabaseService.Service.Ref(_path, this);
        return _val;
    }

    internal virtual T? Val
    {
        get => _val;
        set
        {
            // If the current value is already equal to this value, no need to update.
            if (EqualityComparer<T>.Default.Equals(_val, value) && _hasVal) return;
            _val = value;
            DataChanged?.Invoke(_val);
            _hasVal = true;
        }
    }

    /// <summary>
    /// Constructs a DataReference specifying the path within the database.
    /// </summary>
    /// <param name="path"></param>
    internal protected DataReference(string path, T? defaultValue = default, string? niceName = null)
    {
        this._path = path;
        this._defaultValue = defaultValue;
        this._niceName = niceName;
    }

    /// <summary>
    /// Attempts to remove the data from the database
    /// </summary>
    /// <returns></returns>
    public virtual async void Remove(bool notifyOnSuccess = true)
    {
        IResultHandler handler = notifyOnSuccess ? this : IResultHandler.Default;
        await DatabaseService.Service.Remove(_path, handler);
    }

    /// <inheritdoc/>
    [JSInvokable]
    public virtual void OnSuccess()
    {
        string message = "Data Synced";
        message += _niceName == null ? "" : $": {_niceName}";
        NotificationService.Service.Add(message, Severity.Success).AndForget();
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
    public virtual void OnChange(string? data)
    {
        if (data == null)
        {
            Val = _defaultValue;
            return;
        }
        HandleChange(data);
    }

    internal abstract void HandleChange(string data);
}

internal class JsonDataReference<T> : DataReference<T>
{

    public JsonDataReference(string path, T? defaultValue = default, string? niceName = null) : base(path, defaultValue, niceName) { }

    public override async void Set(T data, bool notifyOnSuccess = true)
    {
        IResultHandler handler = notifyOnSuccess ? this : IResultHandler.Default;
        string jsonData = JsonSerializer.Serialize(data);
        await DatabaseService.Service.Set<string>(Path, jsonData, handler);
    }
    /// <inheritdoc/>
    internal override void HandleChange(string data)
    {
        try
        {
            string jsonString = JsonDocument.Parse(data).RootElement.GetString()!;
            Val = JsonSerializer.Deserialize<T>(jsonString);
        }
        catch (JsonException jse)
        {
            NotificationService.Service.Add($"Error loading {NiceName}. Expected UserData but found: {data}", Severity.Error).AndForget();
            Console.Error.WriteLine(jse);
        }
        catch (Exception e)
        {
            NotificationService.Service.Add($"Error loading {NiceName}.", Severity.Error).AndForget();
            Console.Error.WriteLine(e);
        }
    }
}

internal class BoolDataReference : DataReference<bool>
{
    private static readonly Dictionary<string, BoolDataReference> DataRefs = new();
    internal static DataReference<bool> GetRef(string path, bool defaultValue = false, string? niceName = null)
    {
        if (!DataRefs.TryGetValue(path, out BoolDataReference? value))
        {
            value = new BoolDataReference(path, defaultValue, niceName);
        }
        return value;
    }
    private BoolDataReference(string path, bool defaultValue = false, string? niceName = null) : base(path, defaultValue, niceName) { }

    /// <inheritdoc/>
    internal override void HandleChange(string data)
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
    /// <inheritdoc/>
    public override async void Set(bool data, bool notifyOnSuccess = true)
    {
        IResultHandler handler = notifyOnSuccess ? this : IResultHandler.Default;
        await DatabaseService.Service.Set<bool>(Path, data, handler);
    }
}

internal class StringDataReference : DataReference<string>
{
    private static readonly Dictionary<string, StringDataReference> DataRefs = new();
    internal static DataReference<string> GetRef(string path, string defaultValue = "", string? niceName = null)
    {
        if (!DataRefs.TryGetValue(path, out StringDataReference? value))
        {
            value = new StringDataReference(path, defaultValue, niceName);
        }
        return value;
    }
    private StringDataReference(string path, string defaultValue = "", string? niceName = null) : base(path, defaultValue, niceName) { }
    /// <inheritdoc/>
    internal override void HandleChange(string data)
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
    /// <inheritdoc/>
    public override async void Set(string data, bool notifyOnSuccess = true)
    {
        IResultHandler handler = notifyOnSuccess ? this : IResultHandler.Default;
        await DatabaseService.Service.Set<string>(Path, data, handler);
    }
}