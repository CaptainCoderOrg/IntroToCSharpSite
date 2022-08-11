namespace CaptainCoder;
/// <summary>
/// InventoryDatabase is a class for keeping track of and displaying store items in the inventory. It is constructed by providing a list of all of the items that can be displayed.
/// </summary>
public class InventoryDatabase : AbstractItemDatabase
{
    /// <inheritdoc/>
    public override Filter[] Filters => GetAllFilter();
    /// <summary>
    /// Constructs an inventory database with the items in the store
    /// </summary>
    /// <param name="allItems"></param>
    public InventoryDatabase(List<StoreItem> allItems) : base(allItems) { }
    /// <summary>
    /// Constructs an InventorDatabase with the given inventory.
    /// </summary>
    /// <param name="inventory">The user's inventory</param>
    public InventoryDatabase(UserInventory inventory) : base(inventory.Items.ToList()) { }
    private Filter[] GetAllFilter()
    {
        Filter all = new("All");
        Dictionary<string, HashSet<string>> filters = new();
        foreach (StoreItem item in this.GetItems("All"))
        {
            string filterName = item.Type;
            string subFilterName = item.SubType;
            if (!filters.ContainsKey(filterName))
            {
                filters[filterName] = new();
                filters[filterName].Add($"All {filterName}");
            }
            filters[filterName].Add(subFilterName);
        }
        Filter[] filterArr = new Filter[filters.Count + 1];
        filterArr[0] = all;
        int ix = 1;
        foreach (string name in filters.Keys)
        {
            filterArr[ix++] = new(name, filters[name].ToArray());
        }
        return filterArr;
    }
}