namespace CaptainCoder
{
    using Microsoft.AspNetCore.Components;
    public class ReplIt : ComponentBase
    {
        [Parameter, EditorRequired]
        public string Path { get; set; } = null!;

        [Parameter]
        public bool OutputOnly { get; set; } = true;
        public string Url => $"https://replit.com/@{Path}?{Options}";
        private string Options => "lite=true" + (OutputOnly ? "&outputonly=true" : "");
        
    }
}