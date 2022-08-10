namespace CaptainCoder;

public class MockDatabase : IItemDatabase
{
    public string[] Filters => new string[] { "All", "Weapons", "Armor" };

    private List<StoreItem> AllItems = new();

    public MockDatabase()
    {
        // AllItems.Add();
        AllItems.Add(new("Axe", "Weapon", 1000, "A simple, but effective axe.",
        "/imgs/store/weapons/axe.png"));
        AllItems.Add(new("Iron Sword", "Weapon", 1500, "A durable sword made of iron.",
            "/imgs/store/weapons/IronSword.png"));
        AllItems.Add(new("Golden Sword", "Weapon", 30000000, "A strong and fast sword, but breaks easily.",
            "/imgs/store/weapons/GoldSword.png"));
        AllItems.Add(new ("Golden Axe", "Weapon", 3000, "A very heavy hitting axe, but not super durable.",
            "/imgs/store/weapons/GoldAxe.png"));
    }

    public List<StoreItem> GetItems(string filter)
    {
        if (filter == "All")
        {
            return AllItems;
        }
        return null;
        // return AllItems.Where(???).ToList();
    }
}