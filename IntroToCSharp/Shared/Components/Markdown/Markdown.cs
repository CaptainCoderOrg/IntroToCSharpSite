using Markdig;
using MudBlazor;


public class MarkdownPiper{

    public string ContentProducer(string content){
        var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        return content = Markdown.ToHtml(content, pipeline);
    }
}