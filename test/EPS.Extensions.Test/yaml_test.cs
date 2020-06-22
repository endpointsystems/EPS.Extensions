using System;
using EPS.Extensions.Test.Types;
using EPS.Extensions.YamlMarkdown;
using Machine.Specifications;

namespace EPS.Extensions.Test
{
    public class yaml_test
    {
        private static string inPath = AppDomain.CurrentDomain.BaseDirectory + "/test/in/md_in.md";
        private static string outPath = AppDomain.CurrentDomain.BaseDirectory + "test/out/md_out.md";

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
