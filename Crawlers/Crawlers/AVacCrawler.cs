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
        protected AVacCrawler()
        {
            Jobs = new List<Job>();
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
            var webClient = new System.Net.WebClient();

            if (url == null)
                result = webClient.DownloadString(Url);
            else
                result = webClient.DownloadString(url);

            return WebUtility.HtmlDecode(result);
        }

        public string Url { protected get; set; }

        public int CurrentPage { get; set; }

        public int Pages { get; set; }

        /// <summary>
        /// CrawlerData Properties
        /// </summary>
        public string CrawlerData { get; set; }

        public List<Job> Jobs { get; set; }
    }
}
