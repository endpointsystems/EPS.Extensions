using System.IO;
using System.Text;
using Html2Markdown;
using Markdig;
using Markdig.Parsers;
using Markdig.Renderers;

namespace EPS.Extensions.YamlMarkdown;

public static class StringExtensions
{
    public static string ToHtml(this string markdown)
    {
        var sb = new StringBuilder();
        using var sw = new StringWriter(sb);
        var render = new HtmlRenderer(sw);
        var pipeline = new MarkdownPipelineBuilder().Build();
        pipeline.Setup(render);
        var doc = MarkdownParser.Parse(markdown);
        render.Render(doc);
        sw.Flush();
        return sb.ToString();
    }

    public static string ToMarkdown(this string html)
    {
        if (string.IsNullOrEmpty(html)) return string.Empty;
        var converter = new Converter();
        return converter.Convert(html);
    }
}
