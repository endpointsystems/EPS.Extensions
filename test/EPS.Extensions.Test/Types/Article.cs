using System;

namespace EPS.Extensions.Test.Types
{
    public class Article
    {
        public int Id { get; set; }
        public DateTime dateModified { get; set; }
        public DateTime datePublished { get; set; }
        public string headline { get; set; }

        //for Algolia
        public string[] ids { get; set; }

        public string slug { get; set; }
        public string description { get; set; }
        //Images must be in .jpg, .png, or. gif format. For best results, provide multiple high-resolution images
        //(minimum of 300,000 pixels when multiplying width and height) with the following
        //aspect ratios: 16x9, 4x3, and 1x1.
        public string[] image { get; set; }

        //save the original image ID
        public string imageId { get; set; }
        public string body { get; set; }

        public string[] categories { get; set; }

        public string author { get; set; }

    }
}
