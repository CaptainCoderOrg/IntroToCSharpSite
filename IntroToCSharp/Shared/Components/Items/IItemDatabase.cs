namespace CaptainCoder;

public interface IItemDatabase
{
    public string[] Filters {get;}
    public List<StoreItem> GetItems(string filter);
}