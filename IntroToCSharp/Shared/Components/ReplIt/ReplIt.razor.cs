namespace IntroToCSharp.Shared.Components.ReplIt
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.JSInterop;

    public sealed partial class ReplIt : ComponentBase
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
        public string MinHeight { get; set; } = "302px";
        private string Height => $"calc(46px + {MinHeight})";
        public string ResizeHeight { get; private set; } = "0px";

        [Inject]
        private IJSRuntime JS { get; set; } = null!;
        public string Url => $"https://replit.com/@{Path}?{Options}";
        private string Options => "embed=true" + (OutputOnly ? "&outputonly=1" : "");
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