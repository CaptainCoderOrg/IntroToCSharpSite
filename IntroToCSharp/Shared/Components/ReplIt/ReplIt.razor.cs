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
        [Parameter]
        public bool ShareSource { get; set; } = false;
        [Parameter]
        public string MinHeight { get; set; } = "250px";
        public string ResizeHeight { get; private set; } = "0px";

        [Inject]
        private IJSRuntime JS { get; set; } = null!;
        public string Url => $"https://replit.com/@{Path}?{Options}";
        private string Options => "lite=true" + (OutputOnly ? "&outputonly=true" : "");
        public async Task ReloadIFrame()
        {
            ResizeHeight = "0px";
            await JS.InvokeVoidAsync("ReloadIFrame", Path);
        }

        public async void ResizeFrame()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(1000);
                ResizeHeight = MinHeight;
                StateHasChanged();
            });
        }
    }
}