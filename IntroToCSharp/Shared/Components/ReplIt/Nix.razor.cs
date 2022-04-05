namespace IntroToCSharp.Shared.Components.ReplIt
{

    public sealed partial class Nix
    {
        public string MinHeight { get; set; } = "500px";
        public string ResizeHeight { get; private set; } = "0px";
        public string Url = "https://replit.com/@JosephCollard/C-Project?lite=true";
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