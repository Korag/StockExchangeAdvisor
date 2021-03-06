﻿using System;
using System.IO;
using System.Net.Http;
using Utility;

namespace ObserverDesignPattern
{
    //ConcreteObserver
    public class ContentDownloader : IObserver
    {
        public const string RemoteQuotesURL = "https://info.bossa.pl/pub/metastock/mstock/mstall.zip";

        public string PathToQuotesDirectoryInZipFileFormat { get; set; }
        public string PathToUnpackedQuotesDirectory { get; set; }

        public ContentDownloader()
        {
            PathToQuotesDirectoryInZipFileFormat = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\QuotesDownloader\\ZIP\\mstall.zip"));
            PathToUnpackedQuotesDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\QuotesDownloader\\DownloadedQuotes\\"));
        }

        public void DownloadNewQuotes()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var awaiter = client.GetAsync(RemoteQuotesURL).GetAwaiter();
                    var result = awaiter.GetResult();
                    var zipByteArray = result.Content.ReadAsByteArrayAsync().Result;

                    ZipHelper.SaveZipArchiveFromByteArray(zipByteArray, PathToQuotesDirectoryInZipFileFormat);
                }

                ZipHelper.ExtractZipDirectory(PathToQuotesDirectoryInZipFileFormat, PathToUnpackedQuotesDirectory);
            }
            catch (Exception)
            {
                Console.WriteLine("There was an error during process of downloading Quotes from external source.");
                throw;
            }
        }
    }
}
