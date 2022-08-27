using System.Text.Json.Serialization;

namespace CaptainCoder;
/// <summary>
/// The user's stats for keeping track of stats like XP and Gold. It is constructed by providing a XP and gold amount.
/// </summary>
public class UserPages
{
    public static readonly UserPages Default = new ();
    private static UserPages GetDefault()
    {
        UserPages pages = new ();
        pages.AddPage("Introduction", PageStatus.New);
        Console.WriteLine("Getting Default Pages");
        return pages;
    }

    public Dictionary<string, PageStatus> Pages { get; set; } = new ();

    public bool Contains(MenuItem item) {
        if (Pages.ContainsKey(item.Name)) return true;
        foreach (MenuItem child in item.Children)
        {
            if (this.Contains(child)) return true;
        }
        return false;
    }

    public PageStatus GetStatus(MenuItem item) {
        if (this.Pages.TryGetValue(item.Name, out PageStatus status)) return status;
        return FindStatus(item);
    }

    public PageStatus GetStatus(PageRef page) {
        if (this.Pages.TryGetValue(page.Name, out PageStatus status)) return status;
        return PageStatus.Undiscovered;
    }

    private PageStatus FindStatus(MenuItem item) {
        HashSet<PageStatus> statuses = new ();
        statuses.Add(PageStatus.Undiscovered);
        foreach (MenuItem child in item.Children)
        {
            statuses.Add(GetStatus(child));
        }
        if (statuses.Contains(PageStatus.New)) return PageStatus.New;
        if (statuses.Contains(PageStatus.Started)) return PageStatus.Started;
        if (statuses.Contains(PageStatus.Complete)) return PageStatus.Complete;
        return PageStatus.Undiscovered;
    }

    public bool Contains(PageRef pageRef) => Pages.ContainsKey(pageRef.Name);

    public void AddPage(string name, PageStatus status) => this.Pages[name] = status;
}

public enum PageStatus
{
    Undiscovered,
    Complete,
    Started,
    New
}
