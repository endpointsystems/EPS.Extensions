using System;
using EPS.Extensions.Test.Types;
using EPS.Extensions.YamlMarkdown;
using Machine.Specifications;
using Machine.Specifications.Model;
using Xunit;

namespace EPS.Extensions.Test
{
    public class yaml_test
    {
        private static string inPath = "~/test/in/in.md";
        private static string outPath = "~/test/out/out.md";

        private static YamlMarkdown<Article> yamlArticle;
        private static Article article;

        private Because of_the_parsing = () =>
        {
            yamlArticle = new YamlMarkdown<Article>();
            article = yamlArticle.Parse(inPath);
        };

        private It should_serialize_correctly = () =>
        {
            var markup = yamlArticle.Markdown;
            yamlArticle.Save(article, markup, outPath);
        };
    }
}
