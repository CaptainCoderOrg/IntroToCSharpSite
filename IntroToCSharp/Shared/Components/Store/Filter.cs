namespace CaptainCoder;

public class Filter {
    public string Name {get; }= "";
    public string[] subFilters {get; }
    public bool hasSubFilters;

    public Filter(string name, string[] subFilters) {
        this.Name = name;
        this.subFilters = subFilters;
        this.hasSubFilters = subFilters.Length > 0;
    }

    public Filter(string name) {
        this.Name = name;
        this.subFilters = new string[] {};
        this.hasSubFilters = false;
    }


}