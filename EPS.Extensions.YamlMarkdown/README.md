# YamlMarkdown
This NuGet package combines [YamlDotNet](https://www.nuget.org/packages/YamlDotNet/) and the [Markdig](https://www.nuget.org/packages/Markdig/) Markdown parsing engine and makes a dead-simple class for parsing a file or TextReader object and deserializing your YAML object type as well as giving you your content in the original Markdown and generic HTML renderings.

The [Markdig plugin](https://github.com/xoofx/markdig) uses a YAML front matter extension that will parse a YAML front matter *into* the MarkdownDocument.

Ours treats the YAML front matter as a completely separate object to be used for other purposes (SEO metadata in our use case).

## How It Works 

```c#
// Article in this case is a class of metadata properties 
// saved in the YAML
var parser = new YamlMarkdown<Article>();

// we get the Article object here
var article = parser.Parse("article.md");

//...and here
Console.WriteLine(parser.DataObject);

//...and the markup (separate from the YAML) here
Console.WriteLine(parser.Markdown);

//...and the parsed HTML here
Console.WriteLine(parser.Html);

```
