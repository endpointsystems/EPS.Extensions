using EPS.Extensions.Test.Types;
using EPS.Extensions.YamlMarkdown;
using Machine.Specifications;

namespace EPS.Extensions.Test
{
    public class yaml_raw_test
    {
        private static string inPath = "test/in/raw_in.md";
        private static string outPath = "test/out/raw_out.md";

        private static YamlRaw<Article> yamlArticle;
        private static Article article;

        private Because of_the_parsing = () =>
        {
            yamlArticle = new YamlRaw<Article>();
            article = yamlArticle.Parse(inPath);
        };

        private It should_serialize_correctly = () =>
        {
            var content = yamlArticle.Content;
            yamlArticle.Save(article, content, outPath);
        };

    }
}
