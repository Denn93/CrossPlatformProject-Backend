using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrawlerBatch.Crawlers;

namespace CrawlerBatchTests
{
    [TestClass]
    public class CrawlerTests
    {
        [TestMethod]
        public void CrawlerBeforeStartTest()
        {
            ACrawler mockCrawler = new MockCrawler();

            Assert.IsNotNull(mockCrawler.Name);
            Assert.AreEqual(1, mockCrawler.CurrentPage);
            Assert.AreEqual("http://www.", mockCrawler.Url.Substring(0, 11));
            Assert.IsFalse(mockCrawler.HasStarted);
        }

        [TestMethod]
        public void Processing()
        {
            MockCrawler mockCrawler = new MockCrawler();
            mockCrawler.Process();
            Assert.AreEqual(mockCrawler.TotalMatches, mockCrawler.Jobs.Count);
        }
    }
}
