namespace CaptainCoder
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.JSInterop;
    public class CodeBlock : ComponentBase
    {
        private string language = "csharp";
        private bool allowCopy = false;
        private string? _output;
        private string _filename = String.Empty;

        [Inject]
        private IJSRuntime? JS { get; set; }

        [Inject]
        private HttpClient? HTTP { get; set; }

        [Parameter]
        public string Language
        {
            get => language;
            set => language = value;
        }

        [Parameter]
        public string Filename { get => _filename; set => _filename = value; }

        [Parameter]
        public bool AllowCopy
        {
            get => allowCopy;
            set => allowCopy = value;
        }

        public string Output
        {
            get => _output == null ? "Loading..." : _output;
            private set => _output = value;
        }

        public string GetAllowCopy() => AllowCopy ? "" : "no-copy";

        protected override async void OnAfterRender(bool firstRender)
        {
            await JS!.InvokeVoidAsync("RenderCode", $"{Language}/{Filename}", Output);
        }

        protected override async Task OnInitializedAsync()
        {
            Output = await HTTP!.GetStringAsync($"examples/{Language}/{Filename}");
            Output = Output.Replace("<", "&lt;");
            Output = Output.Replace(">", "&gt;");
        }
    }
}