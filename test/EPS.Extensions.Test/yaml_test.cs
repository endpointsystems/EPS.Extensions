using System;
using EPS.Extensions.Test.Types;
using EPS.Extensions.YamlMarkdown;
using Machine.Specifications;
using Machine.Specifications.Model;
using Xunit;
using YamlDotNet.Serialization;

namespace EPS.Extensions.Test
{
    public class yaml_test
    {
        private static string inPath = "assets/in.md";
        private static string outPath = "assets/out.md";

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

        private It should_parse_with_unmatched_properties = () =>
        {
            yamlArticle = new YamlMarkdown<Article>(
                new DeserializerBuilder()
                    .IgnoreUnmatchedProperties()
                    .Build()
            );
            article = yamlArticle.Parse(inPath);
        };
    }
}
