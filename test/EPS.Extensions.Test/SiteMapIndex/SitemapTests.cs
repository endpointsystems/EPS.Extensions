using System;
using System.Collections.Generic;
using System.Xml;
using EPS.Extensions.SiteMapIndex;
using Machine.Specifications;

namespace EPS.Extensions.Test.SiteMapIndex
{
    public class SiteMapTestBase
    {
        protected static SiteMap siteMap;
        protected static SiteMapConfig config;
    }
    public class sitemap_test: SiteMapTestBase
    {
        //private static SiteMap siteMap;

        protected Establish context = () =>
        {
            siteMap = new SiteMap(new Uri("https://my.superlong.fancy.website.com/sitemap.xml"));
        };

        protected It should_generate_a_site_map = async () =>
        {
            await using var ms = await siteMap.Parse(() =>
            {
                var locations = new Stack<Location>();
                for (int i = 0; i < 50000; i++)
                {
                    var loc = new Location
                    {
                        Frequency = ChangeFrequency.Daily,
                        Priority = 0.1,
                        Url = $"https://my.fancy.website.com/{Guid.NewGuid():N}",
                        LastMod = DateTime.Now
                    };
                    locations.Push(loc);
                }
                return locations;
            });

            int j = 0;
            using var reader = XmlReader.Create(ms, new XmlReaderSettings
            {
                Async = true,
                CloseInput = true,
            });
            while (await reader.ReadAsync())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "url") j++;
            }
            j.ShouldEqual(50000);
        };

        protected It should_give_back_stack_objects_because_of_file_size = async () =>
        {
            var stack = new Stack<Location>();
            for (int i = 0; i < 50000; i++)
            {
                stack.Push(new Location
                {
                    Frequency = ChangeFrequency.Always,
                    Priority = 0.2,
                    Url = $"https://my.superlong.fancy.website.com/somekindofquery?id={Guid.NewGuid():N}",
                    LastMod = DateTime.Now
                });
            }

            var smap = new SiteMap(new Uri("https://my.longandfancy.website.com/sitemap.xml"), new SiteMapConfig()
            {
                maxLocationCount = 50000,
                maxFileSize = 50000 //50k
            });

            var ms = await smap.Parse(stack);
            stack.Count.ShouldBeLessThan(50000);
            var count = stack.Count;

            using var reader = XmlReader.Create(ms, new XmlReaderSettings {Async = true, CloseInput = true});
            int j = 0;
            while (await reader.ReadAsync())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "url") j++;
            }

            j.ShouldEqual(50000 - count);
        };

        protected It should_give_back_stack_objects_because_of_stack_size = async () =>
        {
            var stack = new Stack<Location>();
            for (int i = 0; i < 50001; i++)
            {
                stack.Push(new Location
                {
                    Frequency = ChangeFrequency.Always,
                    Priority = 0.2,
                    Url = $"https://my.superlong.fancy.website.com/somekindofquery?id={Guid.NewGuid():N}",
                    LastMod = DateTime.Now
                });
            }

            var smap = new SiteMap(new Uri("https://my.longandfancy.website.com/sitemap.xml"), true);

            var ms = await smap.Parse(stack);
            stack.Count.ShouldEqual(1);

            using var reader = XmlReader.Create(ms, new XmlReaderSettings {Async = true, CloseInput = true});
            int j = 0;
            while (await reader.ReadAsync())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "url") j++;
            }

            j.ShouldEqual(50000);
        };
    }
}
