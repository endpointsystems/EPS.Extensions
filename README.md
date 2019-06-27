# EPS.Extensions

The purpose of this repository is to provide quick and useful utility libraries and whatever for ASP.NET Core development. There's nothing more frustrating than running down a rabbit hole trying to do what should be simple tasks. 

Here's what we've got so far:

## UrlFriendly

Create friendly slugs/URLs from text containing European and other non-URL friendly characters. The inspiration for this library came from [Johan Boström's blog](https://www.johanbostrom.se/blog/how-to-create-a-url-and-seo-friendly-string-in-csharp-text-to-slug-generator/). He's got a GitHub with this code, but it's a simple executable demonstrating its usability. Now we've got a NuGet package for it. 

## YamlMarkdown

One simple class with one simple function - parsing Markdown files with YAML front-end matter in them. Code for this came from [Mark Heath's blog](https://markheath.net/post/markdown-html-yaml-front-matter) where he talks about doing it and gives a bit of context. I spent some time trying to find something that already worked before spending a good couple of hours making it all work. 

This code deserializes the YAML to the type you pass in, and gives you the Markdown and generic HTML of the rest of the file. 

