using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace IntroToCSharp.Shared.Components.Nix
{

    public sealed partial class Nix
    {
        private Dictionary<string, string> Projects = new Dictionary<string, string>();
        private static int s_NEXT_ID = 0;
        private string? _url = "https://replit.com/@JosephCollard/C-Project?lite=true";
        private string Url
        {
            get => _url == null ? "" : _url;
            set
            {
                _url = value;
                StateHasChanged();
            }
        }
        private int _id = s_NEXT_ID++;
        [Inject]
        IDialogService DialogService { get; set; } = null!;
        [Inject]
        internal IJSRuntime JS { get; set; } = null!;
        [Parameter]
        public string MinHeight { get; set; } = "500px";
        [Parameter]
        public Action? OnClose { get; set; } = null;
        public string ResizeHeight { get; private set; } = "0px";
        public string ID => $"nix-{_id}";
        protected override void OnInitialized()
        {
            Projects["Project 1"] = "https://replit.com/@JosephCollard/C-Project?lite=true";
            Projects["Project 2"] = "https://replit.com/@JosephCollard/AskName?lite=true";
            OpenConfig();
        }
        private async void ResizeFrame()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(1000);
                ResizeHeight = MinHeight;
                StateHasChanged();
            });
        }

        private async Task ReloadIFrame()
        {
            ResizeHeight = "0px";
            await JS.InvokeVoidAsync("ReloadIFrame", ID);
        }

        private void OpenConfig()
        {
            DialogService.Show<ConfigPanel>();
        }

        private void ChangeProject(ChangeEventArgs e)
        {
            Url = e.Value?.ToString()!;
        }
    }
}