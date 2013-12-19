using CrawlerBatch.Crawlers;

namespace CrawlerBatch
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Main Actions

            var crawlerBatches = new CrawlerBatches();

            crawlerBatches.BuildCrawlers();
            crawlerBatches.StartCrawlers();
            crawlerBatches.SubmitData();

            #endregion
        }
    }
}
