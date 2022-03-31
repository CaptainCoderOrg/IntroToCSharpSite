namespace CaptainCoder
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.JSInterop;
    using Radzen;

    public class ReplIt : ComponentBase
    {
        [Parameter, EditorRequired]
        public string Path { get; set; } = null!;

        [Parameter]
        public bool OutputOnly { get; set; } = true;

        [Inject]
        public TooltipService TooltipService { get; set; } = null!;
        
        [Inject]
        private IJSRuntime JS { get; set; } = null!;
        public string Url => $"https://replit.com/@{Path}?{Options}";
        private string Options => "lite=true" + (OutputOnly ? "&outputonly=true" : "");
        public void ShowTooltip(ElementReference elementReference, string message) => TooltipService.Open(elementReference, message, new TooltipOptions(){ Duration = 3000, Position = TooltipPosition.Right});
        public async void ReloadIFrame()
        {
            await JS.InvokeVoidAsync("ReloadIFrame", Path);
        }
    }
}