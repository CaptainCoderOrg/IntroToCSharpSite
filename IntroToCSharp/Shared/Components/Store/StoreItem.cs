namespace CaptainCoder;

/// <summary>
/// A StoreItem represents an Item that can be bought in the store or sold in the inventory. It is constructed by providing a name, type, subtype, cost, description, and image.
/// </summary>
public class StoreItem
{
    /// <summary>
    /// Default StoreItem.
    /// </summary>
    /// <returns>A default StoreItem.</returns>
    public static readonly StoreItem Default = new StoreItem("Loading...", "Loading...", "Loading...", 1_000_000, "Loading...", "/imgs/store/loading.png");
    /// <summary>
    /// The name of the item.
    /// </summary>
    /// <value>A string name.</value>
    public string Name { get; }
    /// <summary>
    /// The type of the item.
    /// </summary>
    /// <value>A string type.</value>
    public string Type { get; }
    /// <summary>
    /// The subType of the item.
    /// </summary>
    /// <value>A string subType.</value>
    public string SubType { get; }
    /// <summary>
    /// The cost of the item.
    /// </summary>
    /// <value>An int cost.</value>
    public int Cost { get; }
    /// <summary>
    /// The description of the item.
    /// </summary>
    /// <value>A string description</value>
    public string Description { get; }
    /// <summary>
    /// The image of the item.
    /// </summary>
    /// <value>The image.</value>
    public string Image { get; }
    /// <summary>
    /// Constructs a new store item with a name, type, subtype, cost, description and image.
    /// </summary>
    /// <param name="name">The name of the item.</param>
    /// <param name="type">The type of item.</param>
    /// <param name="subType">The subtype.</param>
    /// <param name="cost">The cost.</param>
    /// <param name="description">The description.</param>
    /// <param name="image">The image.</param>
    public StoreItem(string name, string type, string subType, int cost, string description, string image)
    {
        this.Name = name;
        this.Type = type;
        this.SubType = subType;
        this.Cost = cost;
        this.Description = description;
        this.Image = image;
    }
    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj == this) return true;
        if (!(obj is StoreItem)) return false;
        StoreItem asItem = (StoreItem)obj;
        return asItem.Name == this.Name;
    }
    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    /// <inheritdoc/>
    public override string? ToString()
    {
        return base.ToString();
    }
}