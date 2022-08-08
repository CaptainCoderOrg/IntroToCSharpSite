namespace CaptainCoder;

public class StoreItem 
{
    public string Name {get; }
    public string Type {get; }
    public int Cost {get; }
    public string Description {get;}
    public string Image {get;}

    public StoreItem(string name, string type, int cost, string description, string image) {
        this.Name = name;
        this.Type = type;
        this.Cost = cost;
        this.Description = description;
        this.Image = image;
    }
}