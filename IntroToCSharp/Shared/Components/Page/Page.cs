using Microsoft.AspNetCore.Components;

namespace CaptainCoder
{
    
    public class Page : ComponentBase
    {

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            await Utils.ScrollToTop();
            Console.WriteLine("OnAfterRendered");
        }

    }

}