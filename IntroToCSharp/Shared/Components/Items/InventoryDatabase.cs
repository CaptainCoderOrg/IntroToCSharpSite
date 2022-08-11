namespace CaptainCoder;

public class InventoryDatabase : AbstractItemDatabase
{
    public override Filter[] Filters => GetAllFilter();

    public InventoryDatabase(List<StoreItem> allItems) : base(allItems) {}
    public InventoryDatabase(UserInventory inventory) : base(inventory.Items.ToList()) {}

    private Filter[] GetAllFilter() {
        Filter all = new ("All");
        Dictionary<string, HashSet<string>> filters = new ();
        foreach(StoreItem item in this.GetItems("All")){
            string filterName = item.Type;
            string subFilterName = item.SubType;
            if (!filters.ContainsKey(filterName))
            {
                filters[filterName] = new ();
            }
            filters[filterName].Add(subFilterName);
        }
        Filter[] filterArr = new Filter[filters.Count + 1];
        filterArr[0] = all;
        int ix = 1;
        foreach(string name in filters.Keys)
        {
            filterArr[ix++] = new (name, filters[name].ToArray());
        }
        return filterArr;
    }
    
}