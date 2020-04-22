using ObserverDesignPattern;
using System;
using System.Threading;

namespace QuotesDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            TimeSpan startTimeSpan = TimeSpan.Zero;
            TimeSpan periodTimeSpan = TimeSpan.FromMinutes(15);

            NewContentChecker contentChecker = new NewContentChecker();
            ContentDownloader contentDownloader = new ContentDownloader();
            contentChecker.Attach(contentDownloader);

            var timer = new Timer((e) =>
            {
                contentChecker.CheckNewContent();

            }, null, startTimeSpan, periodTimeSpan);

            Console.ReadLine();
        }
    }
}
