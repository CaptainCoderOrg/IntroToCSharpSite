namespace CaptainCoder
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.JSInterop;

    public class ReplIt : ComponentBase
    {
        [Parameter, EditorRequired]
        public string Path { get; set; } = null!;

        [Parameter]
        public bool OutputOnly { get; set; } = true;
        [Parameter]
        public Action? OnClose { get; set; } = null;
                
        [Inject]
        private IJSRuntime JS { get; set; } = null!;
        public string Url => $"https://replit.com/@{Path}?{Options}";
        private string Options => "lite=true" + (OutputOnly ? "&outputonly=true" : "");
        public async void ReloadIFrame()
        {
            await JS.InvokeVoidAsync("ReloadIFrame", Path);
        }
    }
}