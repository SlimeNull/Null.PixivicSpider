namespace NullLib.PixivicSpider.Module
{
    public class IllustrationInfo
    {
        public int id { get; set; }
        public int artistId { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string caption { get; set; }
        public Artistpreview artistPreView { get; set; }
        public Tag[] tags { get; set; }
        public Imageurl[] imageUrls { get; set; }
        public string[] tools { get; set; }
        public string createDate { get; set; }
        public int pageCount { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int sanityLevel { get; set; }
        public int restrict { get; set; }
        public int totalView { get; set; }
        public int totalBookmarks { get; set; }
        public bool isLiked { get; set; }
        public int xrestrict { get; set; }
    }

}
