using System;
using System.Collections.Generic;
using System.IO;
using Carter;
using EPS.Extensions.SiteMapIndex;
using LazyCache;
using Microsoft.AspNetCore.Mvc;

namespace EPS.Samples.SiteMapIndex.Modules
{
    public class SiteMapModule: CarterModule
    {
        private readonly IAppCache cache;
        public SiteMapModule(IAppCache appCache)
        {
            cache = appCache;
            cache.GetOrAdd("sitemap1.xml", () => getSiteMap("https://localhost:10623/sitemap1.xml"));
            cache.GetOrAdd("sitemap2.xml",() => getSiteMap("https://localhost:10623/sitemap2.xml"));
            cache.GetOrAdd("sitemap3.xml",() => getSiteMap("https://localhost:10623/sitemap3.xml"));
            cache.GetOrAdd("sitemap.xml",() => getSiteMapIndex("https://localhost:10623/sitemap.xml"));
            var list = new List<string>();
            list.AddRange(new []{"sitemap1.xml","sitemap2.xml","sitemap3.xml","sitemap.xml"});
            foreach (var item in list)
            {
                Get($"/{item}", async (req, resp) =>
                {
                    if (item.Equals("sitemap.xml"))
                    {
                        var ms = await cache.Get<Extensions.SiteMapIndex.SiteMapIndex>("sitemap.xml").Parse();
                        resp.ContentType = "application/xml";
                        resp.StatusCode = 200;
                        await ms.CopyToAsync(resp.Body);
                        return;
                    }

                    var smap = cache.Get<SiteMap>(item);
                    var ms2 = await smap.Parse();
                    resp.ContentType = "application/xml";
                    resp.StatusCode = 200;
                    await ms2.CopyToAsync(resp.Body);
                    return;
                });
            }
        }

        protected ContentResult streamContent(Stream stream)
        {
            var sr = new StreamReader(stream);
            return new ContentResult() {Content = sr.ToString(), ContentType = "application/xml", StatusCode = 200};
        }

        protected Extensions.SiteMapIndex.SiteMapIndex getSiteMapIndex(string path)
        {
            var sm1 = cache.Get<SiteMap>("sitemap1.xml");
            var sm2 = cache.Get<SiteMap>("sitemap2.xml");
            var sm3 = cache.Get<SiteMap>("sitemap3.xml");
            var smap = new Extensions.SiteMapIndex.SiteMapIndex(new Uri(path));
            smap.SiteMaps.AddRange(new []{sm1,sm2,sm3});
            return smap;
        }
        protected SiteMap getSiteMap(string path)
        {
            var uri = new Uri(path);
            var urlBase = uri.GetLeftPart(UriPartial.Path);
            var smap = new SiteMap(new Uri(path));

            var stack = new Stack<Location>();
            for (int i = 0; i < 50000; i++)
            {
                stack.Push(new Location
                {
                    Frequency = ChangeFrequency.Weekly,
                    Priority = 0.2,
                    Url = urlBase + $"/something/{Guid.NewGuid():D}",
                    LastMod = DateTime.Now
                });
            }

            smap.Locations = stack;
            return smap;
        }
    }
}
