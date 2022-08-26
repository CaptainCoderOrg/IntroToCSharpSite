using Microsoft.AspNetCore.Components;

namespace CaptainCoder;

/// <summary>
/// A PageRef is used to create a page reference on a subclass of the CaptainCoder.Page.
/// Within a subclass of a CaptainCoder.Page, if you add a static PageRef PageRef, it will
/// "automagically" be added to the NavMenu. If you specify the IsAdventure variable to be
/// false, the page will always appear in the menu while in Adventure Mode
/// </summary>
public readonly record struct PageRef(string Name, string Href, int Order, string Parent, bool IsAdventure = true);

public class Page : ComponentBase
{
    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        Utils.ScrollToTop();
        await CheckPage();
    }

    protected async Task CheckPage() {
        
    }

}
