using System.IO;
using System.Text;
using Markdig;
using Markdig.Parsers;
using Markdig.Renderers;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace EPS.Extensions.YamlMarkdown
{
    /// <summary>
    /// Deserialize the YAML and return the Markdown and HTML.
    /// </summary>
    /// <typeparam name="T">The data type to deserialize from the YAML.</typeparam>
    public class YamlMarkdown<T> where T: new()
    {
        private readonly IDeserializer yaml;
        private string markdown;
        private string html;

        public YamlMarkdown()
        {
            yaml = new DeserializerBuilder()
                .Build();
        }

        /// <summary>
        /// Parses a file from the file system.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <returns>The deserialized object.</returns>
        public T Parse(string path)
        {
            T t;
            var text = File.ReadAllText(path);
            using (var input = new StringReader(text))
            {
                var parser = new Parser(input);
                parser.Expect<StreamStart>();
                parser.Expect<DocumentStart>();
                try
                {
                    t = yaml.Deserialize<T>(parser);
                }
                catch (SyntaxErrorException se)
                {
                    throw new SyntaxErrorException("An exception occured parsing the YAML. Check the dash " +
                                                   "separators and the YAML syntax before trying again. Further " +
                                                   "details can be found in the original inner exception.",se);
                }
                
                parser.Expect<DocumentEnd>();
                markdown = input.ReadToEnd();
                html = Render(markdown);
            }

            return t;
        }

        /// <summary>
        /// Parse the <see cref="TextReader"/> and return the deserialized YAML.
        /// </summary>
        /// <param name="textReader">The <see cref="TextReader"/> to deserialize.</param>
        /// <returns></returns>
        public T Parse(TextReader textReader)
        {
            T t;
            var parser = new Parser(textReader);
            parser.Expect<StreamStart>();
            parser.Expect<DocumentStart>();
            try
            {
                t = yaml.Deserialize<T>(parser);
            }
            catch (SyntaxErrorException se)
            {
                throw new SyntaxErrorException("An exception occured parsing the YAML. Check the dash " +
                                               "separators and the YAML syntax before trying again. Further " +
                                               "details can be found in the original inner exception.",se);
            }
            parser.Expect<DocumentEnd>();
            markdown = textReader.ReadToEnd();
            html = Render(markdown);
            return t;
        }

        /// <summary>
        /// Render the Markdown data to generic HTML.
        /// </summary>
        /// <param name="markup">The Markdown to mark up.</param>
        /// <returns>Generic HTML markup.</returns>
        private static string Render(string markup)
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            var render = new HtmlRenderer(sw);
            var pipeline = new MarkdownPipelineBuilder()
                .Build();
            pipeline.Setup(render);
            var doc = MarkdownParser.Parse(markup);
            render.Render(doc);
            sw.Flush();
            return sb.ToString();
        }

        /// <summary>
        /// Gets the markdown pulled from the YAML/Markdown file.
        /// </summary>
        public string Markdown
        {
            get { return markdown; }
        }

        /// <summary>
        /// Gets the parsed Markdown from the YAML/Markdown file.
        /// </summary>
        public string Html
        {
            get { return html; }
        }
    }
}