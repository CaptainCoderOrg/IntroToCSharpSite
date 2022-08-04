namespace CaptainCoder;

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