using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
 
        protected AVacCrawler()
        {}

        /// <summary>
        /// Methode Process bevat een filter die toegepast wordt op de gevonden data
        /// </summary>
        public abstract void Process();

        /// <summary>
        /// Deze methode bevat zorgt eroor dat de data van url wordt opgehaald
        /// </summary>
        public virtual String urlHandler()
        {
            WebClient webClient = new System.Net.WebClient();
            string result = webClient.DownloadString(url);

            return result;
        }

        public String Url 
        { 
            set { url = value; } 
        }


        /// <summary>
        /// CrawlerData Properties
        /// </summary>
        public String CrawlerData
        {
            get { return crawlerData; }
            set { crawlerData = value; }
        }
    }
}
