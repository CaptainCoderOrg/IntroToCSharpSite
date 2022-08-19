using Microsoft.AspNetCore.Components;

namespace CaptainCoder;

/// <summary>
/// A PageRef is used to create a page reference on a subclass of the CaptainCoder.Page.
/// Within a subclass of a CaptainCoder.Page, if you add a static PageRef PageRef, it will
/// "automagically" be added to the NavMenu.
/// </summary>
public readonly record struct PageRef(string Name, string Href, int Order, string Parent);

public class Page : ComponentBase
{

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        Utils.ScrollToTop();
    }

}
