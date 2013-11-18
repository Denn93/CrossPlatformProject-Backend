using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Crawlers
{
    abstract class AVacCrawler
    {
        /// <summary>
        /// Protected String url bevat de url die de crawler nodig heeft
        /// Protected String CrawlerData bevat de data gevonden door de UrlHandler
        /// </summary>
        protected String url;
        protected String crawlerData;
        protected List<Job> jobs;

        protected int currentPage;
        protected int pages;
 
        protected AVacCrawler()
        {
            jobs = new List<Job>();
        }

        /// <summary>
        /// Methode Process bevat een filter die toegepast wordt op de gevonden data
        /// </summary>
        public abstract void Process();

        /// <summary>
        /// Deze methode bevat zorgt eroor dat de data van url wordt opgehaald
        /// </summary>
        public virtual String urlHandler(String url = null)
        {
            String result;
            WebClient webClient = new System.Net.WebClient();

            if (url == null)
                result = webClient.DownloadString(this.url);
            else
                result = webClient.DownloadString(url);

            return WebUtility.HtmlDecode(result);
        }

        public String Url 
        { 
            set { url = String.Format(value, CurrentPage); } 
        }

        public int CurrentPage 
        {
            set { currentPage = value; }
            get { return currentPage; }
        }

        public int Pages 
        { 
            get { return pages; } 
            set { pages = value; } 
        }

        /// <summary>
        /// CrawlerData Properties
        /// </summary>
        public String CrawlerData
        {
            get { return crawlerData; }
            set { crawlerData = value; }
        }

        public List<Job> Jobs
        {
            get { return jobs; }
            set { jobs = value; } 
        }
    }
}
