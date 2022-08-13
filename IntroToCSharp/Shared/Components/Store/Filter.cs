namespace CaptainCoder;

/// <summary>
/// A Filter is a class for allowing us to filter the display of inventory and store based on item type and subtype. It is constructed by providing a top most type name and an optional string[] of subfilters.
/// </summary>
public class Filter
{
    /// <summary>
    /// The name of the type.
    /// </summary>
    /// <value>name</value>
    public string Name { get; } = "";
    /// <summary>
    /// The sub filter for sort items by subtype.
    /// </summary>
    /// <value>A string array of subfilter names.</value>
    public string[] subFilters { get; }
    /// <summary>
    /// Determine if the filter has any sub filters.
    /// </summary>
    public bool hasSubFilters;
    /// <summary>
    /// Constructs a new filter with the given name and subfilter array.
    /// </summary>
    /// <param name="name">Name of filter.</param>
    /// <param name="subFilters">Array of subfilters.</param>
    public Filter(string name, string[] subFilters)
    {
        this.Name = name;
        this.subFilters = subFilters;
        this.hasSubFilters = subFilters.Length > 0;
    }
    /// <summary>
    /// Constructs a new filter with a given name and no subfilters.
    /// </summary>
    /// <param name="name">Name of the filter</param>
    public Filter(string name)
    {
        this.Name = name;
        this.subFilters = new string[] { };
        this.hasSubFilters = false;
    }
}