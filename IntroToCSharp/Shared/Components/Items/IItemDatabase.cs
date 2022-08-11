namespace CaptainCoder;

public interface IItemDatabase
{
    /// <summary>
    /// Allows us to sort through the different types of items in the store and inventory.
    /// </summary>
    /// <value>A array of objects used to sort the store items.</value>
    public Filter[] Filters { get; }
    /// <summary>
    /// Gets the items that fits the chosen filter.
    /// </summary>
    /// <param name="filter">The filter to sort items by.</param>
    /// <returns>List of StoreItems that fit the given filter.</returns>
    public List<StoreItem> GetItems(string filter);
    /// <summary>
    /// Gets the items that fits teh chosen sub item filter.
    /// </summary>
    /// <param name="subFilter">The subfilter to sort items by.</param>
    /// <returns>List of StoreItems that fit the given subFilter.</returns>
    public List<StoreItem> GetSubItems(string subFilter);
}
/// <summary>
/// AbstractItemDatabase is a class for keeping track of and displaying store items in the inventory or the store. It is constructed by providing a list of all of the items that can be displayed.
/// </summary>
public abstract class AbstractItemDatabase : IItemDatabase
{
    private readonly List<StoreItem> AllItems;
    /// <summary>
    /// Allows us to sort through the different types of items in the store and inventory.
    /// </summary>
    /// <value>A array of objects used to sort the store items.</value>
    public abstract Filter[] Filters { get; }
    /// <summary>
    /// Constructs a new AbstractItemDatabase with the given list of allItems.
    /// </summary>
    /// <param name="allItems">All of the items that can be displayed using this database.</param>
    public AbstractItemDatabase(List<StoreItem> allItems)
    {
        this.AllItems = allItems;
    }
    /// <summary>
    /// Gets the items that fits the chosen filter.
    /// </summary>
    /// <param name="filter">The filter to sort items by.</param>
    /// <returns>List of StoreItems that fit the given filter.</returns>
    public List<StoreItem> GetItems(string filter)
    {
        if (filter == "All")
        {
            return AllItems;
        }
        return AllItems.Where(item => item.Type == filter).ToList();
    }
    /// <summary>
    /// Gets the items that fits the chosen sub item filter.
    /// </summary>
    /// <param name="subFilter">The subfilter to sort items by.</param>
    /// <returns>List of StoreItems that fit the given subFilter.</returns>
    public List<StoreItem> GetSubItems(string subFilter)
    {
        if (subFilter.StartsWith("All"))
        {
            return GetItems(subFilter.Substring(4));
        }
        return AllItems.Where(item => item.SubType == subFilter).ToList();
    }
}