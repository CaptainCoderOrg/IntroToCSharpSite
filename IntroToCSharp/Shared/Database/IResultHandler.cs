using Microsoft.JSInterop;
using MudBlazor;

namespace CaptainCoder;

/// <summary>
/// A IResultHandler is used to handle database events that can succeed
/// or fail.
/// </summary>
public interface IResultHandler
{
    public readonly static IResultHandler Default = new DefaultResultHandler();

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

internal class DefaultResultHandler : IResultHandler
{
    [JSInvokable]
    public async void OnException(string exception) => await NotificationService.Service.Add(exception, Severity.Warning);
    [JSInvokable]
    public void OnSuccess() { }
}
