using System.Text.Json.Serialization;

namespace CaptainCoder;
/// <summary>
/// The user's stats for keeping track of stats like XP and Gold. It is constructed by providing a XP and gold amount.
/// </summary>
public class UserStats
{
    /// <summary>
    /// The amount of XP.
    /// </summary>
    /// <value>Amount of XP.</value>
    public int XP { get; private set; }
    [JsonIgnore]
    public int XPTowardLevel { get => this.XP - XPForLevel(this.Level); }
    [JsonIgnore]
    public int XPNeeded { get => NextLevelXP - XPForLevel(this.Level); }
    /// <summary>
    /// The level of the user.
    /// </summary>
    /// <returns>The level.</returns>
    [JsonIgnore]
    public int Level { get => XPToLevel(this.XP); }
    [JsonIgnore]
    public int NextLevelXP { get => XPForLevel(this.Level + 1); }
    /// <summary>
    /// The user's amount of gold.
    /// </summary>
    /// <value>Amount of gold</value>
    public int Gold { get => GoldAcquired - this.GoldSpent; }

    public int GoldSpent {get; set;}
    public int GoldAcquired {get; set;}
    /// <summary>
    /// Constructs the user's xp and gold.
    /// </summary>
    /// <param name="xp">The amopunt of xp.</param>
    /// <param name="gold">The amount of gold.</param>
    public UserStats(int xp, int goldAcquired, int goldSpent)
    {
        this.XP = xp;
        this.GoldAcquired = goldAcquired;
        this.GoldSpent = goldSpent;
    }
    public static int XPToLevel(int xp)
    {
        return 1 + (int)Math.Sqrt(xp / 100);
    }
    public static int XPForLevel(int level)
    {
        level--;
        return 100 * level * level;
    }

    public override string ToString()
    {
        return $"UserStats: XP: {this.XP}, Gold: {this.Gold}";
    }

    /// <summary>
    /// Default UserStats object.
    /// </summary>
    /// <returns>A default UserStats.</returns>
    public readonly static UserStats Default = new(0, 0, 0);
}
/// <summary>
/// A UserInventory is how the program keeps track of what is in the user's inventory. It is constructed without parameters.
/// </summary>
public class UserInventory
{
    /// <summary>
    /// Default userInventory object.
    /// </summary>
    /// <returnsA default UserInvetory.></returns>
    public static readonly UserInventory Default = new UserInventory();
    private readonly List<StoreItem> _items;
    /// <summary>
    /// A list of items in inventory.
    /// </summary>
    /// <value>The store items.</value>
    public StoreItem[] Items
    {
        get => _items.ToArray();
        set
        {
            _items.Clear();
            _items.AddRange(value);
        }
    }
    /// <summary>
    /// Constructs a new empty UserInventory.
    /// </summary>
    public UserInventory()
    {
        _items = new();
    }
    /// <summary>
    /// Constructs a user inventory with a list of items.
    /// </summary>
    /// <param name="items">The store items in the inventory.</param>
    public UserInventory(List<StoreItem> items)
    {
        _items = new(items);
    }
    private UserInventory(UserInventory old, StoreItem item, bool add = true) : this(old._items)
    {
        if (add)
        {
            _items.Add(item);
        }
        else
        {
            bool removed = _items.Remove(item);
        }
    }
    /// <summary>
    /// Determines if this UserInventory contains given item
    /// </summary>
    /// <param name="toCheck">Item to look for.</param>
    /// <returns>True if this UserInventory contains toCheck. Returns false otherwise.</returns>
    public bool Contains(StoreItem toCheck) => _items.Contains(toCheck);
    /// <summary>
    /// Adds the given item to the user inventory.
    /// </summary>
    /// <param name="toAdd">The item to add.</param>
    /// <returns>A new user inventory with the item included.</returns>
    public UserInventory AddItem(StoreItem toAdd) => new UserInventory(this, toAdd, true);
    /// <summary>
    /// Removes the given item from the user's inventory
    /// </summary>
    /// <param name="toRemove">The item to remove.</param>
    /// <returns>A new user inventory with the item removed.</returns>
    public UserInventory RemoveItem(StoreItem toRemove) => new UserInventory(this, toRemove, false);
}