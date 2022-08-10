namespace CaptainCoder;

public class StoreItem
{
    public static readonly StoreItem Default = new StoreItem("Loading...", "Loading...", 1_000_000, "Loading...", "/imgs/store/loading.png");
    public string Name { get; }
    public string Type { get; }
    public int Cost { get; }
    public string Description { get; }
    public string Image { get; }

    public StoreItem(string name, string type, int cost, string description, string image)
    {
        this.Name = name;
        this.Type = type;
        this.Cost = cost;
        this.Description = description;
        this.Image = image;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj == this) return true;
        if (!(obj is StoreItem)) return false;
        StoreItem asItem = (StoreItem)obj;
        return asItem.Name == this.Name;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string? ToString()
    {
        return base.ToString();
    }
}