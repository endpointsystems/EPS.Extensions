using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EPS.Extensions.SiteMapIndex
{
    public class SiteMapIndex
    {
        /// <summary>
        /// Create a new instance of the <see cref="SiteMapIndex"/>.
        /// </summary>
        public SiteMapIndex()
        {
            SiteMaps = new List<SiteMap>();
        }
        /// <summary>
        /// Create a new instance of the <see cref="SiteMapIndex"/>.
        /// </summary>
        /// <param name="siteMapIndexPath">The full <see cref="Uri"/> of the sitemap index location.</param>
        public SiteMapIndex(Uri siteMapIndexPath)
        {
            SiteMaps = new List<SiteMap>();
            SiteMapIndexPath = siteMapIndexPath;
        }

        /// <summary>
        /// Add a <see cref="SiteMap"/> to the <see cref="SiteMaps"/> collection.
        /// </summary>
        /// <param name="siteMap"></param>
        public void AddSiteMap(SiteMap siteMap)
        {
            SiteMaps.Add(siteMap);
        }

        /// <summary>
        /// Gets or sets a collection of <see cref="SiteMap"/> objects for the index.
        /// </summary>
        public List<SiteMap> SiteMaps { get; set; }

        /// <summary>
        /// Gets or sets the full path of the <see cref="SiteMapIndex"/>.
        /// </summary>
        public Uri SiteMapIndexPath { get; set; }

        /// <summary>
        /// Create a site map index consisting of the <see cref="SiteMaps"/>.
        /// </summary>
        /// <returns>A <see cref="MemoryStream"/> containing the sitemap XML.</returns>
        public async Task<MemoryStream> Parse()
        {
            var ms = new MemoryStream();
            return (MemoryStream) await Parse(ms);
        }

        /// <summary>
        /// Create a site map index consisting of the <see cref="SiteMaps"/>.
        /// </summary>
        /// <returns>A <see cref="Stream"/> containing the sitemap XML.</returns>
        /// <param name="stream">The stream to put the sitemap XML into.</param>
        public async Task<Stream> Parse(Stream stream)
        {
            var writer = XmlWriter.Create(stream,
                new XmlWriterSettings
                {
                    Async = true, Encoding = Encoding.UTF8, NewLineHandling = NewLineHandling.None,
                    OmitXmlDeclaration = false, NewLineOnAttributes = false, Indent = false,
                    WriteEndDocumentOnClose = true,CloseOutput = false,NewLineChars = string.Empty,
                    IndentChars = string.Empty
                });
            writer.WriteStartElement("sitemapindex",Constants.URLSET);
            foreach (var sm in SiteMaps)
            {
                await writer.WriteStartElementAsync(string.Empty,"sitemap",string.Empty);
                await writer.WriteElementStringAsync(string.Empty, "loc", string.Empty,
                    sm.SiteMapPath.ToString());
                if (sm.LastModified > DateTime.MinValue)
                    await writer.WriteElementStringAsync(string.Empty, "lastmod", string.Empty,
                        sm.LastModified.ToString("yyyy-MM-dd"));
                await writer.WriteEndElementAsync();
            }
            await writer.WriteEndElementAsync();
            writer.Close();
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}
