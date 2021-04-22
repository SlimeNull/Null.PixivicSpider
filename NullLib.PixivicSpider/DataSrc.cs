using Newtonsoft.Json;
using NullLib.PixivicSpider.Module;
using NullLib.HttpWebHelper;
using System;
using System.Collections.Generic;
using System.Net;
using System.IO;

namespace NullLib.PixivicSpider
{
    public class DataSrc
    {
        public HttpHelper HttpHelper { get; set; } = new HttpHelper()
        {
            RequestOption = new HttpRequestOption()
            {
                Accept = "application/json, text/plain, */*",
                Referer = "https://pixivic.com/",
                PreAction = new UserAgentGenerator().PutRandomUserAgent,

                Headers = new WebHeaderCollection()
                {
                    { "Accept-Encoding", "gzip, deflate" },
                    { "Accept-Language", "en-US,en;q=0.9,zh-CN;q=0.8,zh;q=0.7" },
                    { "Origin", "https://pixivic.com" }
                }
            },
            ResponseOption = new HttpResponseOption()
            {
                AutoDecompress = true,
            }
        };
        public void ProcessUrl(ref string str)
        {
            str = str.Replace("i.pximg.net", "o.acgpic.net");  // acgpic.net 也可
        }
        public HttpWebRequest GenRankQuery(int page, int pageSize, DateTime date, RankMode mode)
        {
            return HttpHelper.CreateGetRequest("https://pix.ipv4.host/ranks", new Dictionary<string, string>()
            {
                { "page", page.ToString() },
                { "pageSize", pageSize.ToString() },
                { "date", date.ToString("yyyy-MM-dd") },
                { "mode", mode.ToString().ToLower() }
            });
        }
        public RankData GetRankQuery(int page, int pageSize, DateTime date, RankMode mode)
        {
            return JsonConvert.DeserializeObject<RankData>(
                HttpHelper.GetResponseText(GenRankQuery(page, pageSize, date, mode)));
        }
        private bool SelfDownloadImage(IllustrationInfo info, ImageSize size, NameType type, string dir)
        {
            if (info == null || info.imageUrls == null || info.imageUrls.Length == 0)
                return false;
            string downloadUrl = size switch
            {
                ImageSize.SquareMedium => info.imageUrls[0].squareMedium,
                ImageSize.Medium => info.imageUrls[0].medium,
                ImageSize.Large => info.imageUrls[0].large,
                ImageSize.Original => info.imageUrls[0].original,
                _ => null,
            };
            string name = type switch
            {
                NameType.ById => info.id + Path.GetExtension(downloadUrl),
                NameType.ByUrl => Path.GetFileName(downloadUrl),
                NameType.ByTitle => info.title + Path.GetExtension(downloadUrl),
                NameType.ByArtistAndTitle => info.artistPreView.name + info.title + Path.GetExtension(downloadUrl),
                _ => null,
            };
            string path = Path.Combine(dir, name);
            try { _ = new FileInfo(path); } catch (Exception) { return false; }
            ProcessUrl(ref downloadUrl);
            using (FileStream fs = File.OpenWrite(path))
            {
                HttpWebRequest request = HttpHelper.CreateGetRequest(downloadUrl);
                using Stream stream = HttpHelper.GetResponseStream(request);
                stream.CopyTo(fs);
            }
            return true;
        }
        public bool DownloadImage(IllustrationInfo info, ImageSize size, NameType type, string dir)
        {
            if (!Directory.Exists(dir))
                return false;
            return SelfDownloadImage(info, size, type, dir);
        }
        public bool DownloadImages(IList<IllustrationInfo> infos, ImageSize size, NameType type, string dir)
        {
            if (!Directory.Exists(dir))
                return false;
            bool result = true;
            foreach (var info in infos)
                result &= SelfDownloadImage(info, size, type, dir);
            return result;
        }
        public bool DownloadImages(IList<IllustrationInfo> infos, ImageSize size, NameType type, string dir, EventHandler<DownloadProgressArgs> callback)
        {
            if (!Directory.Exists(dir))
                return false;
            bool result = true;
            for (int i = 0, len = infos.Count; i < len; i++)
            {
                callback.Invoke(this, new DownloadProgressArgs((double)i / len, infos[i]));
                result &= SelfDownloadImage(infos[i], size, type, dir);
            }
            return result;
        }
    }
}
