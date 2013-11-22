using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawlers
{
    class Crawler
    {
        /// <summary>
        /// Deze methode zal alle crawlers gaan bouwen
        /// </summary>
        public void BuildCrawlers()
        {
//            AVacCrawler vacatureBankNl = new VacatureBankNl();
//            vacatureBankNl.Process();

            AVacCrawler jobbirdCom = new JobbirdCom();
            jobbirdCom.Process();
        }

        /// <summary>
        /// Deze methode zal de gebouwde crawlers gaan starteb
        /// </summary>
        public void StartCrawlers()
        {
            
        }

        /// <summary>
        /// Deze methode zal alle data die is gevonden doorsturen naar de webservice 
        /// </summary>
        public void SubmitData()
        {
            
        }
    }
}
