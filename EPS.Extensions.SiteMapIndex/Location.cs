using System;

namespace EPS.Extensions.SiteMapIndex
{
    public class Location
    {
        public string Url { get; set; }
        public DateTime LastMod { get; set; }
        public ChangeFrequency Frequency { get; set; }
        public double Priority { get; set; }

    }

    public enum ChangeFrequency
    {
        Always,
        Hourly,
        Daily,
        Weekly,
        Monthly,
        Yearly,
        Never
    }
}
