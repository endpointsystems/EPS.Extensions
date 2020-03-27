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
    public class YamlMarkdown<T> where T : new()
    {
        private IDeserializer yaml;
        private ISerializer yamlSerializer;

        public YamlMarkdown(IDeserializer deserializerBuilder = null)
        {
            init(deserializerBuilder);
        }

        public YamlMarkdown(string filePath, IDeserializer deserializerBuilder = null) : this(deserializerBuilder)
        {
            Parse(filePath);
        }

        public YamlMarkdown(TextReader reader, IDeserializer deserializerBuilder = null) : this(deserializerBuilder)
        {
            Parse(reader);
        }

        private void init(IDeserializer deserializerBuilder = null)
        {
            yaml = deserializerBuilder != null ? deserializerBuilder : new DeserializerBuilder().Build();
            yamlSerializer = new Serializer();
        }

        /// <summary>
        /// Parses a file from the file system.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <returns>The deserialized object.</returns>
        public T Parse(string path)
        {
            T t;
            FileName = Path.GetFileNameWithoutExtension(path);
            var text = File.ReadAllText(path);
            using (var input = new StringReader(text))
            {
                var parser = new Parser(input);
                parser.Consume<StreamStart>();
                parser.Consume<DocumentStart>();
                try
                {
                    t = yaml.Deserialize<T>(parser);
                }
                catch (SyntaxErrorException se)
                {
                    throw new SyntaxErrorException("An exception occured parsing the YAML. Check the dash " +
                                                   "separators and the YAML syntax before trying again. Further " +
                                                   "details can be found in the original inner exception.", se);
                }
                catch (YamlException ye)
                {
                    throw new SyntaxErrorException($"An exception occured parsing {path} - {ye.Message} ");
                }

                parser.Consume<DocumentEnd>();
                Markdown = input.ReadToEnd();
                Html = Render(Markdown);
            }

            DataObject = t;
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
            parser.Consume<StreamStart>();
            parser.Consume<DocumentStart>();
            try
            {
                t = yaml.Deserialize<T>(parser);
            }
            catch (SyntaxErrorException se)
            {
                throw new SyntaxErrorException("An exception occured parsing the YAML. Check the dash " +
                                               "separators and the YAML syntax before trying again. Further " +
                                               "details can be found in the original inner exception.", se);
            }
            catch (YamlException ye)
            {
                throw new SyntaxErrorException($"An exception occured parsing text - {ye.Message} ");
            }
            parser.Consume<DocumentEnd>();
            Markdown = textReader.ReadToEnd();
            Html = Render(Markdown);
            DataObject = t;
            return t;
        }

        /// <summary>
        /// Save the YAML and markdown to a file.
        /// </summary>
        /// <param name="obj">The typed object you wish to save</param>
        /// <param name="markdown">The markdown you wish to save with it</param>
        /// <param name="path">The path to persist data to.</param>
        /// <remarks>
        /// This method saves your YAML and content as a YAML-flavored Markdown file.
        /// </remarks>
        public void Save(T obj, string markdown, string path)
        {
            var y = yamlSerializer.Serialize(obj);
            var sb = new StringBuilder();
            sb.AppendLine("---");
            sb.Append(y);
            sb.AppendLine("---");
            sb.Append(markdown);
            File.WriteAllText(path, sb.ToString());
        }

        public void Save(string markdown, string path)
        {
            Save(DataObject, markdown, path);
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

        public string FileName { get; set; }
        public T DataObject { get; set; }

        /// <summary>
        /// Gets the markdown pulled from the YAML/Markdown file.
        /// </summary>
        public string Markdown { get; set; }

        /// <summary>
        /// Gets the parsed Markdown from the YAML/Markdown file.
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string Html { get; set; }

    }
}
