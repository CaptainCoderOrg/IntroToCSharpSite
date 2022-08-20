using System.Reflection;

namespace CaptainCoder;

/// <summary>
/// The PageRegistry is a singleton object that maintains the items that
/// appear in the NavMenu component. To add a page to the registry
/// add a static PageRef field to a Page. The simplest way to 
/// add sections is to modify the constructor.
/// </summary>
public class PageRegistry
{

    public static readonly PageRegistry Instance = new PageRegistry().Init();
    private readonly Dictionary<string, MenuItem> _items = new ();

    private PageRegistry()
    {
        int order = 0;
        AddSection("Getting Started", order++);
        AddSection("Activities", order++);
        AddSection("Lessons", order++);
        AddSection("Basics", "Lessons", order++);
        AddSection("Variables", "Lessons", order++);
        AddSection("Challenges", order++);
        AddSection("Journal", order++);
        AddSection("Social", order++);
    }

    private PageRegistry Init()
    {
        var type = typeof(Page);
        var pages = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p));
        foreach(var page in pages)
        {
            FieldInfo? field = page.GetField("PageRef");
            if (field == null) continue;
            PageRef? pageRef = (PageRef?)field.GetValue(null);
            if (pageRef == null) continue;
            this.AddPage((PageRef)pageRef);
        }
        return this;
    }

    /// <summary>
    /// Adds the specified section to the registry specifying its
    /// order in the list.
    /// </summary>
    private MenuItem AddSection(string name, int order)
    {
        if (!_items.TryGetValue(name, out MenuItem? item) || item == null)
        {
            item = new MenuItem(name, null, order, true);
            _items[name] = item;
        }
        else
        {
            item.Order = order;
        }
        return item;
    }

    /// <summary>
    /// Adds the specified subsection to the registry specifying its
    /// order in the parent section.
    /// </summary>
    private MenuItem AddSection(string name, string parent, int order)
    {
        MenuItem item = AddSection(name, order);
        MenuItem parentItem = GetParent(parent);
        item.Parent = parentItem;
        parentItem.Children.Add(item);
        return item;
    }

    private MenuItem GetParent(string parent)
    {
        if (!_items.TryGetValue(parent, out MenuItem? parentItem) || parentItem == null)
        {
            parentItem = new MenuItem(parent, null, -1, true);
            _items[parent] = parentItem;
        }
        return parentItem;
    }

    private void AddPage(PageRef p)
    {
        if (_items.ContainsKey(p.Name)) throw new InvalidOperationException($"The specified page {p.Name} already exists.");
        MenuItem parent = GetParent(p.Parent);
        MenuItem page = new (p.Name, p.Href, p.Order, p.IsAdventure, parent);
        page.Parent = parent;
        parent.Children.Add(page);
        _items[p.Name] = page;
    }

    public IEnumerable<MenuItem> RootElements => _items.Values.Where(item => item.Parent == null).OrderBy(item => item.Order);

}

/// <summary>
/// A MenuItem represents the data necessary for an item to 
/// be rendered on the NavMenu.
/// </summary>
public class MenuItem : IComparable<MenuItem>
{
    private readonly string _name;
    private readonly string? _href;
    private readonly bool _isAdventure;

    private readonly List<MenuItem> _children = new ();

    public MenuItem(string name, string? href, int order, bool isAdventure, MenuItem? parent = null)
    {
        this._name = name;
        this._href = href;
        this.Order = order;
        this.Parent = parent;
        this._isAdventure = isAdventure;
    }

    public MenuItem? Parent { get; set; } = null;
    public List<MenuItem> Children {
        get {
            _children.Sort();
            return _children;
        }
    }
    public string Name => _name;
    public string? Href => _href;
    public bool IsAdventure => _isAdventure;
    public int Order {get; set;}
    public int CompareTo(MenuItem? other) => this.Order.CompareTo(other?.Order);
    public override string ToString() =>  $"MenuItem {Name}: {Href}";
}