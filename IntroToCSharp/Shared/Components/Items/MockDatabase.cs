namespace CaptainCoder;

public class MockDatabase : AbstractItemDatabase
{
    public override Filter[] Filters => new Filter[] { 
        new("All"), 
        new("Weapons", new string[] { "All Weapons" , "Axes", "Swords", "Daggers" }), 
        new("Armor", new string[] { "All Armor" , "Helmets" , "Chestplates" , "Pants" , "Boots" }) 
    };

    private static List<StoreItem> AllItems = new();
    static MockDatabase() {
        AllItems.Add(new("Axe", "Weapons", "Axes", 1000, "A simple, but effective axe.",
        "/imgs/store/weapons/axe.png"));
        AllItems.Add(new("Iron Sword", "Weapons", "Swords", 1500, "A durable sword made of iron.",
            "/imgs/store/weapons/IronSword.png"));
        AllItems.Add(new("Golden Sword", "Weapons", "Swords", 30000, "A strong and fast sword, but breaks easily.",
            "/imgs/store/weapons/GoldSword.png"));
        AllItems.Add(new("Golden Axe", "Weapons", "Axes", 3000, "A very heavy hitting axe, but not super durable.",
            "/imgs/store/weapons/GoldAxe.png"));
    }

    public MockDatabase() : base(AllItems) {}
}