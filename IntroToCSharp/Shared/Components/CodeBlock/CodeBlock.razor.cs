namespace CaptainCoder
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.JSInterop;
    public class CodeBlock : ComponentBase
    {
        private bool allowCopy = false;
        private string? _output;
        [Inject]
        private IJSRuntime? JS { get; set; }

        [Inject]
        private HttpClient? HTTP { get; set; }
        public int SelectedIndex { get; set; } = 0;
        [Parameter]
        public string Language { get; set; } = "csharp";

        [Parameter]
        public string Filename { get; set; } = String.Empty;

        [Parameter]
        public bool AllowCopy
        {
            get => allowCopy;
            set => allowCopy = value;
        }

        [Parameter]
        public string? ReplIt { get; set; } = null;

        public string Output
        {
            get => _output == null ? "Loading..." : _output;
            private set => _output = value;
        }

        public string GetAllowCopy() => AllowCopy ? "" : "no-copy";

        protected override async void OnAfterRender(bool firstRender)
        {
            await JS!.InvokeVoidAsync("RenderCode", $"{Language}/{Filename}", Output);
            // if (ReplIt != null) await JS!.InvokeVoidAsync("RenderCode", $"ReplIt/{Language}/{Filename}", Output);
        }

        protected override async Task OnInitializedAsync()
        {
            Output = await HTTP!.GetStringAsync($"examples/{Language}/{Filename}");
            Output = Output.Replace("<", "&lt;");
            Output = Output.Replace(">", "&gt;");
        }
    }
}