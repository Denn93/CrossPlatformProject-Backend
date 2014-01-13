using System;
using System.Collections.Generic;
using System.Diagnostics;
using CrawlerBatch.Crawlers;
using CrawlerBatch.Mappers;
using DataAccessObjects;

namespace CrawlerBatch
{
    /// <summary>
    /// CrawlerBatches class. Deze class zorgt ervoor dat alle crawlers gestart worden en dat ze de data versturen naar de webservice
    /// </summary>
    public class CrawlerBatches
    {
        private List<ACrawler> _vacCrawlers;
        private List<ACrawler> _aCvCrawlers;

        private List<List<Job>> _aCrawlerJobs;
        private List<List<Cv>> _aCrawlerCVs;

        /// <summary>
        /// Deze methode maakt alle crawler variablen aan. Deze zet hij in een list. Zodat later hierdoor gelooped kan worden
        /// </summary>
        public void BuildCrawlers()
        {
            _vacCrawlers = new List<ACrawler> {new VacatureBankNl(), new JobbirdCom()};
            _aCvCrawlers = new List<ACrawler> {new CvEnVacatureBankNl()};

            _aCrawlerJobs = new List<List<Job>>();
            _aCrawlerCVs = new List<List<Cv>>();
        }

        /// <summary>
        /// Deze methode zal de gebouwde crawlers gaan starten
        /// </summary>
        public void StartCrawlers()
        {
            foreach (var cvCrawler in _aCvCrawlers)
            {
                cvCrawler.Template();
                _aCrawlerCVs.Add(cvCrawler.Cvs);
            }

            foreach (var vacCrawler in _vacCrawlers)
            {
                vacCrawler.Template();
                _aCrawlerJobs.Add(vacCrawler.Jobs);
            }
        }

        /// <summary>
        /// Deze methode zal alle data die is gevonden doorsturen naar de webservice 
        /// </summary>
        public void SubmitData()
        {
            foreach (var crawlerJobs in _aCrawlerJobs)
                foreach (var job in crawlerJobs)
                {
                    var mapper = new JobMapper();
                    mapper.Insert(job);
                }

            foreach(var crawlerCvs in _aCrawlerCVs)
                foreach (var cv in crawlerCvs)
                {
                    var mapper = new CvMapper();
                    mapper.Insert(cv);
                }

            Console.WriteLine("Done Crawling");
            Console.ReadKey();
            
        }
    }
}
