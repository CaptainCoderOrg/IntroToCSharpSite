namespace CaptainCoder;

public interface IItemDatabase
{
    public Filter[] Filters {get;}
    public List<StoreItem> GetItems(string filter);
}