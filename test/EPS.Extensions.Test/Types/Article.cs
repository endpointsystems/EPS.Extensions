using System;

namespace EPS.Extensions.Test.Types
{
    public class Article
    {
        public DateTime Updated { get; set; }
        public DateTime Published { get; set; }
        public string Title { get; set; } = null!;

        //for Algolia
        public string[] ids { get; set; } = null!;

        public string slug { get; set; } = null!;
        public string Description { get; set; } = null!;

        //save the original image ID
        public string Image { get; set; } = null!;
        public string body { get; set; } = null!;
        public string Keywords { get; set; } = null!;
        public string[] Categories { get; set; } = null!;

        public string author { get; set; } = null!;

    }
}
