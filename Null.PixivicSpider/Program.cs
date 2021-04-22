using System;
using System.IO;
using NullLib.PixivicSpider;
using NullLib.PixivicSpider.Module;

namespace Null.PixivicSpider
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0 || !int.TryParse(args[0], out int count))
            {
                Console.WriteLine("No number specified, use 1000? Y/N");
                if (Console.ReadLine().Equals("Y", StringComparison.OrdinalIgnoreCase))
                    count = 1000;
                else
                    return;
            }
            DataSrc dataSrc = new DataSrc();
            Console.WriteLine("Getting rank info...");
            RankData rankData = dataSrc.GetRankQuery(1, count, new DateTime(2021, 4, 19), RankMode.Day);
            string dir = "./Pixivic";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            dataSrc.DownloadImages(rankData.data, ImageSize.Original, NameType.ById, dir, (sender, args) =>
            {
                Console.WriteLine(string.Format("Downloading... {0:F2}% of {1}, current task:{2}-{3}", args.Progress * 100, rankData.data.Length, args.Current.artistPreView.name, args.Current.title));
            });
        }
    }
}
