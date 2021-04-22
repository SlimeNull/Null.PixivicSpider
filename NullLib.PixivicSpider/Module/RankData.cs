using System;
using System.Collections.Generic;
using System.Text;

namespace NullLib.PixivicSpider.Module
{
    public class RankData : NetPackage
    {
        public IllustrationInfo[] data { get; set; }
    }

    public class Artistpreview
    {
        public int id { get; set; }
        public string name { get; set; }
        public string account { get; set; }
        public string avatar { get; set; }
        public bool isFollowed { get; set; }
    }

    public class Tag
    {
        public string name { get; set; }
        public string translatedName { get; set; }
        public int? id { get; set; }
    }

    public class Imageurl
    {
        public string squareMedium { get; set; }
        public string medium { get; set; }
        public string large { get; set; }
        public string original { get; set; }
    }

}
