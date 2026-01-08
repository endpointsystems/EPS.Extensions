using System;
using System.IO;
using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace EPS.Extensions.YamlMarkdown;

/// <summary>
/// Deserialize the YAML and return the unparsed message body.
/// </summary>
/// <typeparam name="T">The data type to deserialize from the YAML.</typeparam>
public class YamlRaw<T> where T : new()
{
    private IDeserializer _yaml = null!;
    private ISerializer _yamlSerializer = null!;

    public YamlRaw()
    {
        Init();
    }

    public YamlRaw(string filePath)
    {
        FilePath = filePath;
        FileName = Path.GetFileNameWithoutExtension(filePath);
        Init();
        Parse(filePath);
    }

    public YamlRaw(TextReader reader)
    {
        Init();
        Parse(reader);
    }

    private void Init()
    {
        _yaml = new DeserializerBuilder().Build();
        _yamlSerializer = new Serializer();
    }

    public T Parse(string path)
    {
        T t;
        var text = File.ReadAllText(path);
        using (var input = new StringReader(text))
        {
            var parser = new Parser(input);
            parser.Consume<StreamStart>();
            parser.Consume<DocumentStart>();
            try
            {
                t = _yaml.Deserialize<T>(parser);
            }
            catch (SyntaxErrorException se)
            {
                throw new SyntaxErrorException("An exception occurred parsing the YAML. Check the dash " +
                                               "separators and the YAML syntax before trying again. Further " +
                                               "details can be found in the original inner exception.", se);
            }
            catch (YamlException ye)
            {
                throw new SyntaxErrorException($"An exception occurred parsing {path} - {ye.Message} ");
            }

            parser.Consume<DocumentEnd>();
            Content = input.ReadToEnd();
        }

        DataObject = t;
        return t;
    }

    /// <summary>
    /// Parse the <see cref="TextReader"/> and return the deserialized YAML.
    /// </summary>
    /// <param name="textReader">The <see cref="TextReader"/> to deserialize.</param>
    /// <returns>The deserialized object.</returns>
    public T Parse(TextReader textReader)
    {
        T t;
        var parser = new Parser(textReader);
        parser.Consume<StreamStart>();
        parser.Consume<DocumentStart>();
        try
        {
            t = _yaml.Deserialize<T>(parser);
        }
        catch (SyntaxErrorException se)
        {
            throw new SyntaxErrorException("An exception occurred parsing the YAML. Check the dash " +
                                           "separators and the YAML syntax before trying again. Further " +
                                           "details can be found in the original inner exception.", se);
        }
        catch (YamlException ye)
        {
            throw new SyntaxErrorException($"An exception occurred parsing text - {ye.Message} ");
        }
        parser.Consume<DocumentEnd>();
        Content = textReader.ReadToEnd();
        DataObject = t;
        return t;
    }

    /// <summary>
    /// Save the YAML and markdown to a file.
    /// </summary>
    /// <param name="obj">The typed object you wish to save</param>
    /// <param name="content">The content you wish to save with it</param>
    /// <param name="path">The path to persist data to.</param>
    /// <remarks>
    /// This method saves your YAML and content as a YAML-flavored Markdown file. This method also does not update
    /// the object itself.
    /// </remarks>
    public void Save(T obj, string content, string path)
    {
        ArgumentNullException.ThrowIfNull(obj);
        var y = _yamlSerializer.Serialize(obj);
        var sb = new StringBuilder();
        sb.AppendLine("---");
        sb.Append(y);
        sb.AppendLine("---");
        sb.Append(content);
        File.WriteAllText(path, sb.ToString());
    }

    public void Save(string content, string path)
    {
        Save(DataObject, content, path);
    }

    public string Content { get; set; } = null!;

    public string FileName { get; set; } = null!;
    public string FilePath { get; set; } = null!;
    public T DataObject { get; set; } = default!;
}
