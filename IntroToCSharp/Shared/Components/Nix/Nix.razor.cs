using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace IntroToCSharp.Shared.Components.Nix
{

    public sealed partial class Nix
    {
        private User? UserData;
        private Dictionary<string, string> Projects = new Dictionary<string, string>();
        private string? _selected;
        private static int s_NEXT_ID = 0;
        private string? _url;
        private bool IsLoggedIn => UserData != null && UserData.IsLoggedIn;
        private bool IsEmpty => Projects.Count == 0;
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
            UserService.Service.OnUserChange += OnUserChanged;
        }
        private void OnUserChanged(User userData)
        {
            this.UserData = userData;
            if (this.UserData.ProjectData != null)
            {
                this.UserData.ProjectData.DataChanged += UpdateProjects;
            }
            if (this.UserData.DefaultProject != null)
            {
                this.UserData.DefaultProject.DataChanged += UpdateDefaultProject;
            }
            StateHasChanged();
        }

        private void UpdateProjects(string? data)
        {
            this.Projects = JsonSerializer.Deserialize<Dictionary<string, string>>(data!)!;
            if (_selected != null && !this.Projects.ContainsKey(_selected))
            {
                _selected = Projects.First().Key;
                UserService.Service.UpdateDefaultProject(Projects[_selected]);
                this.SetURL(Projects[_selected]);
            }
            StateHasChanged();
        }

        private void UpdateDefaultProject(string? defaultProject)
        {
            if (_url == null && defaultProject != null)
            {
                if (Projects.ContainsValue(defaultProject))
                {
                    _selected = Projects.First(x => x.Value == defaultProject).Key;
                }
                else
                {
                    _selected = Projects.First().Key;
                }
                this.SetURL(Projects[_selected]);
            }
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

        private void SetURL(string? project)
        {
            Url = $"{ConfigPanel.PREFIX}{project}?lite=true";
        }

        private void ChangeProject(ChangeEventArgs e)
        {
            UserService.Service.UpdateDefaultProject(e.Value?.ToString()!);
            _selected = Projects.First(x => x.Value == e.Value?.ToString()!).Key;
            SetURL(e.Value?.ToString());
        }
    }
}