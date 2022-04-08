public static class Utils
{
    const string BadChars = ".#$[]";

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
}