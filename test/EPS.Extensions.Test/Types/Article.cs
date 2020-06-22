using System;

namespace EPS.Extensions.Test.Types
{
    public class Article
    {
        public DateTime Updated { get; set; }
        public DateTime Published { get; set; }
        public string Title { get; set; }

        //for Algolia
        public string[] ids { get; set; }

        public string slug { get; set; }
        public string Description { get; set; }

        //save the original image ID
        public string Image { get; set; }
        public string body { get; set; }
        public string Keywords { get; set; }
        public string[] Categories { get; set; }

        public string author { get; set; }

    }
}
