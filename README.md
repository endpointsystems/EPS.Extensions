# EPS.Extensions

The purpose of this repository is to provide quick and useful utility libraries and whatever for ASP.NET Core development. There's nothing more frustrating than running down a rabbit hole trying to do what should be simple tasks. 

Here's what we've got so far:

## UrlFriendly

Create friendly slugs/URLs from text containing European and other non-URL friendly characters. The inspiration for this library came from [Johan Boström's blog](https://www.johanbostrom.se/blog/how-to-create-a-url-and-seo-friendly-string-in-csharp-text-to-slug-generator/). He's got a GitHub with this code, but it's a simple executable demonstrating its usability. Now we've got a NuGet package for it. 

NuGet package can be found [here](https://www.nuget.org/packages/EPS.Extensions.UrlFriendly/).

## YamlMarkdown

One simple class with one simple function - parsing Markdown files with YAML front-end matter in them. Code for this came from [Mark Heath's blog](https://markheath.net/post/markdown-html-yaml-front-matter) where he talks about doing it and gives a bit of context. I spent some time trying to find something that already worked before spending a good couple of hours making it all work. 

This code deserializes the YAML to the type you pass in, and gives you the Markdown and generic HTML of the rest of the file. 

To use this code: 

```c#
var ym = new YamlMarkdown<Article>();
var article = ym.Parse("~/articles/myArticle.md");
// var article = ym.DataObject also works
var html = ym.Html;
var markdown = ym.Markdown;
```

10/17/2019 update: we've added [Html2Markdown](https://github.com/baynezy/Html2Markdown) to our package in order to create two string extensions methods:

```c#
var convertedHtml = markdown.ToHtml();
var convertedMd = html.ToMarkdown();
```
Now you can effortlessly switch between the two.

NuGet package can be found [here](https://www.nuget.org/packages/EPS.Extensions.YamlMarkdown/).

## SiteMapIndex
When you have thousands of URLs in your project, organizing them into site maps is crucial for search engine indexing. This project helps you build site maps and site map indexes. See the [example project](https://github.com/endpointsystems/EPS.Extensions/tree/master/samples/EPS.Samples.SiteMapIndex) for a demonstration of it in action. 

NuGet package can be found [here](https://www.nuget.org/packages/EPS.Extensions.SiteMapIndex/).

## Unique

This project is meant to serve as a quick and easy way to generate unique alphanumeric sequences, which can be used for identifiers, passwords or for other purposes. Its functionality is loosely based on the `System.Web.Security.Membership.GeneratePassword` method from the legacy ASP.NET library. 

NuGet package can be found [here](https://www.nuget.org/packages/EPS.Extensions.Unique/) . A sample csx script file can be found in the samples/scripts folder.

## DynamicTableEntityJsonSerializer

This project is a slight variation of the [DynamicTableEntityJsonSerializer](https://www.nuget.org/packages/DynamicTableEntityJsonSerializer/) package put together by [DoguArslan](https://www.nuget.org/profiles/DoguArslan) on NuGet. The only difference between his package - and ours - is that his serializes fields to explicit objects with the EdmType included, while we make ours into simple properties that allow the entity to be converted into an [ExpandoObject](https://docs.microsoft.com/en-us/dotnet/api/system.dynamic.expandoobject?view=netcore-3.1) so its properties can be accessed.

NuGet package can be found [here](https://www.nuget.org/packages/EPS.Extensions.DynamicTableEntityJsonSerializer/).

## B2CGraphUtil

This library lets you perform administrative tasks against Graph objects in your Azure Active Directory B2C directory instance. 
