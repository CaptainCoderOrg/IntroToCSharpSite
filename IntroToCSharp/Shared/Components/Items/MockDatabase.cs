namespace CaptainCoder;

public class MockDatabase : IItemDatabase
{
    public string[] Filters => new string[] { "All", "Weapons", "Armor" };

    private List<StoreItem> AllItems = new();

    public MockDatabase()
    {
        AllItems.Add(new("Axe", "Weapons", 1000, "A simple, but effective axe.",
        "/imgs/store/weapons/axe.png"));
        AllItems.Add(new("Iron Sword", "Weapons", 1500, "A durable sword made of iron.",
            "/imgs/store/weapons/IronSword.png"));
        AllItems.Add(new("Golden Sword", "Weapons", 30000, "A strong and fast sword, but breaks easily.",
            "/imgs/store/weapons/GoldSword.png"));
        AllItems.Add(new("Golden Axe", "Armor", 3000, "A very heavy hitting axe, but not super durable.",
            "/imgs/store/weapons/GoldAxe.png"));
    }

    public List<StoreItem> GetItems(string filter)
    {
        Console.WriteLine($"Getting Items with filter: {filter}");
        if (filter == "All")
        {
            return AllItems;
        }
        return AllItems.Where(item => item.Type == filter).ToList();
    }
}