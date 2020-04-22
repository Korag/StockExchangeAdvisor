using HtmlAgilityPack;
using System;
using System.IO;
using System.Linq;
using Utility;

namespace ObserverDesignPattern
{
    //ConcreteSubject
    public class NewContentChecker : AContentDownloader
    {
        public const string WebsiteURL = "https://info.bossa.pl/pub/metastock/mstock/";
        public const string NameOfFileWithLastDownloadedDateTime = "LDDateTime.json";
        public const string NameOfSearchedInnerTextInDocumentNode = "baza w formacie txt";

        public string LastDownloadDateTimeFileURL { get; set; }
        public DateTime LastDownloadDateTime { get; set; }

        public NewContentChecker()
        {
            LastDownloadDateTimeFileURL = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, $"..\\..\\..\\JsonFile\\{NameOfFileWithLastDownloadedDateTime}"));
            GetLastDownloadDateTime();
        }

        public void GetLastDownloadDateTime()
        {
            try
            {
                using (StreamReader reader = new StreamReader(LastDownloadDateTimeFileURL))
                {
                    string json = reader.ReadToEnd();
                    LastDownloadDateTime = JsonSerializer.JsonStringToDateTime(json);
                }
            }
            catch (Exception)
            {
                RequestDownloadNewContent();
            }
        }

        public void SetLastDownloadedTime(DateTime dateTime)
        {
            LastDownloadDateTime = dateTime;

            using (StreamWriter writer = new StreamWriter(LastDownloadDateTimeFileURL))
            {
                string json = JsonSerializer.DateTimeToJsonString(LastDownloadDateTime);
                writer.WriteLine(json);
            }
        }

        public void CheckNewContent()
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlNode htmlStructure = htmlWeb.Load(WebsiteURL).DocumentNode;

            int index = htmlStructure.InnerText.IndexOf(NameOfSearchedInnerTextInDocumentNode);
            DateTime dateOfQuotesUpdateOnRemote = DateTime.Parse(htmlStructure.InnerText.Substring(index-24, 16));

            if (dateOfQuotesUpdateOnRemote > LastDownloadDateTime)
            {
                RequestDownloadNewContent();
            }
        }

        public void RequestDownloadNewContent()
        {
            SetLastDownloadedTime(DateTime.Now);
            Notify();
        }

    }
}
