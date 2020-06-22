using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EPS.Extensions.SiteMapIndex
{
    public class SiteMap
    {
        private readonly SiteMapConfig config;
        /// <summary>
        /// Use this constructor when you just want a SiteMap and you're not planning to go over the size limits.
        /// </summary>
        /// <param name="siteMapPath">The <see cref="Uri"/> of the sitemap you're building.</param>
        public SiteMap(Uri siteMapPath)
        {
            SiteMapPath = siteMapPath;
            config = new SiteMapConfig
            {
                maxFileSize = 0,
                maxLocationCount = 0
            };
        }

        /// <summary>
        /// Use this constructor if you want to limit your sitemap to a certain file size or url count.
        /// </summary>
        /// <param name="siteMapPath">The <see cref="Uri"/> of the sitemap you're building</param>
        /// <param name="siteMapConfig">The sitemap configuration restraints</param>
        public SiteMap(Uri siteMapPath, SiteMapConfig siteMapConfig)
        {
            SiteMapPath = siteMapPath;
            config = siteMapConfig;
        }

        public SiteMap(Uri siteMapPath, bool stayCompliant)
        {
            SiteMapPath = siteMapPath;
            if (!stayCompliant) return;
            config = new SiteMapConfig
            {
                maxFileSize = 50000000,
                maxLocationCount = 50000
            };
        }

        /// <summary>
        /// The path to the sitemap being built.
        /// </summary>
        public Uri SiteMapPath { get; }

        /// <summary>
        /// The date this sitemap was last modified.
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="Location"/>
        /// </summary>
        public Stack<Location> Locations { get; set; }

        /// <summary>
        /// Parse the <see cref="Locations"/> we saved in this object.
        /// </summary>
        /// <returns></returns>
        public async Task<MemoryStream> Parse()
        {
            return await Parse(Locations);
        }

        /// <summary>
        /// Parse the stack of <see cref="Location"/> objects into our <see cref="SiteMap"/> <see cref="MemoryStream"/>.
        /// </summary>
        /// <param name="getStack">The function containing the <see cref="Location"/> stack objects.</param>
        /// <returns>A <see cref="MemoryStream"/> containing the XML.</returns>
        /// <exception cref="SiteMapException">Thrown if the stack given is bigger than the <see cref="SiteMapConfig.maxLocationCount"/>.</exception>
        public async Task<MemoryStream> Parse(Func<Stack<Location>> getStack)
        {
            return await Parse(getStack.Invoke());
        }

        /// <summary>
        /// Parse the stack of <see cref="Location"/> objects into our <see cref="SiteMap"/> <see cref="MemoryStream"/>.
        /// </summary>
        /// <param name="stack">The stack of <see cref="Location"/> objects to go into our <see cref="SiteMap"/></param>
        /// <returns>A <see cref="MemoryStream"/> containing the XML.</returns>
        /// <exception cref="SiteMapException">Thrown if the stack given is bigger than the <see cref="SiteMapConfig.maxLocationCount"/>.</exception>
        public async Task<MemoryStream> Parse(Stack<Location> stack)
        {
            int fileSize = 39; //xml declaration

            // if (stack.Count > 0 && config.maxLocationCount > 0 && stack.Count > config.maxLocationCount)
            //     throw new SiteMapException(
            //         $"Location count is {stack.Count} exceeding max location count of {config.maxLocationCount}");

            var ms = new MemoryStream();
            var writer = XmlWriter.Create(ms,
                new XmlWriterSettings
                {
                    Async = true, Encoding = Encoding.UTF8, NewLineHandling = NewLineHandling.None,
                    OmitXmlDeclaration = false, NewLineOnAttributes = false, Indent = false,
                    WriteEndDocumentOnClose = true,CloseOutput = false,NewLineChars = string.Empty,
                    IndentChars = string.Empty
                });
            writer.WriteStartElement("urlset",Constants.URLSET);
            fileSize += 61; //urlset plus namespace

            int longestLine = 0;
            int urlCount = 0;

            while (stack.Count > 0)
            {
                writer.WriteStartElement("url");
                // counts include all open and close tags for XML
                fileSize += 5;
                int lineSize = 5;
                var loc = stack.Pop();
                await writer.WriteElementStringAsync(string.Empty,"loc",String.Empty, loc.Url);

                fileSize += 11 + loc.Url.Length; //loc
                lineSize += 11 + loc.Url.Length;

                if (loc.LastMod > DateTime.MinValue)
                {
                    await writer.WriteElementStringAsync(
                        string.Empty, "lastmod", string.Empty, loc.LastMod.ToString("yyyy-MM-dd"));
                    fileSize += 29;
                    lineSize += 29;
                }
                await writer.WriteElementStringAsync(
                    string.Empty, "changefreq", string.Empty, loc.Frequency.ToString().ToLower());

                fileSize += 25 + loc.Frequency.ToString().Length;
                lineSize += 25 + loc.Frequency.ToString().Length;

                if (loc.Priority > 0.0)
                {
                    await writer.WriteElementStringAsync(
                        string.Empty, "priority", string.Empty, loc.Priority.ToString("0.#",CultureInfo.CurrentCulture));
                    fileSize += 24;
                    lineSize += 24;
                }
                await writer.WriteEndElementAsync();
                fileSize += 6;
                lineSize += 6;

                /* if we add a line at least as long as the current one and it's too long for our max size, then
                 * we should quit while we're ahead.
                 */
                urlCount++;

                // calculate our longest line
                if (lineSize > longestLine) longestLine = lineSize;

                // if we're going to leak over we should stop now
                if (config.maxLocationCount > 0 && urlCount >= config.maxLocationCount) break;
                if (config.maxFileSize > 0 && fileSize + longestLine > config.maxFileSize) break;
            }
            await writer.WriteEndElementAsync();
            writer.Close();
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }
    }
}
