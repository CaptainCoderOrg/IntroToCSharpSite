namespace CaptainCoder;

public class MockDatabase : IItemDatabase
{
    public Filter[] Filters => new Filter[] { new("All"), new("Weapons", new string[] { "All Weapons" , "Axes", "Swords", "Daggers" }), new("Armor", new string[] { "All Armor" , "Helmets" , "Chestplates" , "Pants" , "Boots" }) };

    private List<StoreItem> AllItems = new();

    public MockDatabase()
    {
        AllItems.Add(new("Axe", "Weapons", "Axes", 1000, "A simple, but effective axe.",
        "/imgs/store/weapons/axe.png"));
        AllItems.Add(new("Iron Sword", "Weapons", "Swords", 1500, "A durable sword made of iron.",
            "/imgs/store/weapons/IronSword.png"));
        AllItems.Add(new("Golden Sword", "Weapons", "Swords", 30000, "A strong and fast sword, but breaks easily.",
            "/imgs/store/weapons/GoldSword.png"));
        AllItems.Add(new("Golden Axe", "Weapons", "Axes", 3000, "A very heavy hitting axe, but not super durable.",
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

    public List<StoreItem> GetSubItems(string subFilter) {
        if (subFilter.Contains("all")) {
            return GetItems(subFilter.Substring(3));
        } 
        return AllItems.Where(item => item.SubType == subFilter).ToList();
    }
} 