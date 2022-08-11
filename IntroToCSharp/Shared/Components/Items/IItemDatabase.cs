namespace CaptainCoder;

public interface IItemDatabase
{
    public Filter[] Filters { get; }
    public List<StoreItem> GetItems(string filter);
    public List<StoreItem> GetSubItems(string subFilter);
}

public abstract class AbstractItemDatabase : IItemDatabase
{
    private readonly List<StoreItem> AllItems;
    public abstract Filter[] Filters { get; }
    public AbstractItemDatabase(List<StoreItem> allItems)
    {
        Console.WriteLine($"AbstractItemDatabase: {allItems.Count}");
        this.AllItems = allItems;
    }

    public List<StoreItem> GetItems(string filter)
    {
        if (filter == "All")
        {
            return AllItems;
        }
        return AllItems.Where(item => item.Type == filter).ToList();
    }

    public List<StoreItem> GetSubItems(string subFilter)
    {
        if (subFilter.StartsWith("All"))
        {
            return GetItems(subFilter.Substring(4));
        }
        return AllItems.Where(item => item.SubType == subFilter).ToList();
    }
}