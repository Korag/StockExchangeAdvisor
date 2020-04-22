namespace ObserverDesignPattern
{
    //Subject
    public abstract class AContentDownloader
    {
        public delegate void StatusDownloadNewQuotes();
        public event StatusDownloadNewQuotes OnNewContentUpdate = null;

        public void Attach(ContentDownloader downloader)
        {
            OnNewContentUpdate += new StatusDownloadNewQuotes(downloader.DownloadNewQuotes);
        }

        public void Detach(ContentDownloader downloader)
        {
            OnNewContentUpdate -= new StatusDownloadNewQuotes(downloader.DownloadNewQuotes);
        }

        public void Notify()
        {
            if (OnNewContentUpdate != null)
            {
                OnNewContentUpdate();
            }
        }
    }
}
