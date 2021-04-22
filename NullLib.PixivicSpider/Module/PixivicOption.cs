using System;
using System.Collections.Generic;
using System.Text;

namespace NullLib.PixivicSpider.Module
{
    public enum RankMode
    {
        Day, Week, Month, Female, Male
    }
    public enum ImageSize
    {
        SquareMedium, Medium, Large, Original
    }
    public enum NameType
    {
        ById, ByTitle, ByUrl, ByArtistAndTitle
    }
    public class DownloadProgressArgs : EventArgs
    {
        public DownloadProgressArgs(double progress, IllustrationInfo current)
        {
            (Progress, Current) = (progress, current);
        }
        public double Progress { get; }
        public IllustrationInfo Current { get; }
    }
}
