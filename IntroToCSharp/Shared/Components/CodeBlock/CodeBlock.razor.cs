namespace IntroToCSharp.Shared.Components.CodeBlock
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.JSInterop;
    public sealed partial class CodeBlock : ComponentBase
    {
        private static readonly Dictionary<string, string> s_Cache = new Dictionary<string, string>();
        private static int s_NextID = 0;
        private bool allowCopy = false;
        private string? _output;
        private int _id = s_NextID++;
        private string ID => $"{_id}/{Language}/{Filename}";
        [Inject]
        private IJSRuntime JS { get; set; } = null!;

        [Inject]
        private HttpClient HTTP { get; set; } = null!;
        private int SelectedIndex { get; set; } = 0;
        [Parameter]
        public string MaxHeight { get; set; } = "650px";
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
        private bool IsReplItOpen { get; set; } = false;
        private string ReplItStyle => ReplIt != null && IsReplItOpen ? "min-height:250px; overflow:hidden;" : "display:none";
        private string PlayButtonStyle => ReplIt != null && !IsReplItOpen ? "" : "display:none";

        public string Output
        {
            get => _output == null ? "Loading..." : _output;
            private set => _output = value;
        }

        public string GetAllowCopy() => AllowCopy ? "" : "no-copy";

        protected override async void OnAfterRender(bool firstRender)
        {
            await JS.InvokeVoidAsync("RenderCode", ID, Output);
        }

        protected override async Task OnInitializedAsync()
        {
            string key = $"examples/{Language}/{Filename}";
            if (!s_Cache.TryGetValue(key, out string? output))
            {
                output = await HTTP.GetStringAsync(key);
                output = output.Replace("<", "&lt;");
                output = output.Replace(">", "&gt;");
                s_Cache[key] = output;
            }
            Output = output;
        }
    }
}