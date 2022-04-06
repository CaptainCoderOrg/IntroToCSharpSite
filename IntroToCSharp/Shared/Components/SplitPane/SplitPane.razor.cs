using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace IntroToCSharp.Shared.Components.SplitPane
{
    public sealed partial class SplitPane : ComponentBase
    {
        private static int s_NEXT_ID = 0;
        private readonly int _id = s_NEXT_ID++;
        private bool BottomHidden = true;
        private bool PreventReload = false;
        private string _topStyle = "height:66%";

        [Inject]
        internal IJSRuntime JS { get; set; } = null!;
        [Inject]
        internal ILocalStorageService localStorage { get; set; } = null!;

        /// <summary>
        /// The content to be displayed in the top of this SplitPane
        /// </summary>
        [Parameter, EditorRequired]
        public RenderFragment TopContent { get; set; } = null!;

        /// <summary>
        /// The content to be displayed in the bottom of this SplitPane
        /// </summary>
        [Parameter, EditorRequired]
        public RenderFragment BottomContent { get; set; } = null!;
        private string TopStyle => BottomHidden ? "height:100%" : _topStyle;
        private string ResizerStyle => BottomHidden ? "display:none" : "";
        private string BottomStyle => BottomHidden ? "display:none" : "";
        public string V_ID => $"VerticalSplitPane-{_id}";
        public string TOP_ID => $"VerticalSplitPane-Top-{_id}";
        protected async override void OnAfterRender(bool firstRender)
        {
            await JS.InvokeVoidAsync("SplitPane", V_ID);
        }

        protected async override void OnInitialized()
        {
            bool hasHeight = await localStorage.ContainKeyAsync("console-height");
            if (!hasHeight) return;
            _topStyle = await localStorage.GetItemAsync<string>("console-height");
        }

        public async void ToggleBottomPanel()
        {
            if (!BottomHidden)
            {
                _topStyle = await JS.InvokeAsync<string>("GetSplitPaneHeight", TOP_ID);
                _topStyle = $"height: {_topStyle}";
                await localStorage.SetItemAsync<string>("console-height", _topStyle);
            }
            BottomHidden = !BottomHidden;
            PreventReload = true;
            StateHasChanged();
        }
    }
}