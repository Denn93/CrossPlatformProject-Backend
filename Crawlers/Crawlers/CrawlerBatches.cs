using System;
using System.Collections.Generic;
using CrawlerBatch.Mappers;
using DataAccessObjects;

namespace CrawlerBatch.Crawlers
{
    class CrawlerBatches
    {
        private List<ACrawler> _vacCrawlers;
        private List<ACrawler> _aCvCrawlers;

        private List<List<Job>> _aCrawlerJobs;
        private List<List<Cv>> _aCrawlerCVs;

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
                Console.WriteLine(String.Format("Gestarte crawler: {0}", cvCrawler.Name));

                cvCrawler.Process();
                _aCrawlerCVs.Add(cvCrawler.Cvs);
            }

            foreach (var vacCrawler in _vacCrawlers)
            {
                Console.WriteLine(String.Format("Gestarte crawler: {0}", vacCrawler.Name));

                vacCrawler.Process();
                _aCrawlerJobs.Add(vacCrawler.Jobs);
            }
        }

        /// <summary>
        /// Deze methode zal alle data die is gevonden doorsturen naar de webservice 
        /// </summary>
        public void SubmitData()
        {
            foreach (var crawlerJobs in _aCrawlerJobs)
            {
                foreach (var job in crawlerJobs)
                {
                    JobMapper mapper = new JobMapper();

                    mapper.Insert(job);
                }
            }


            foreach(var crawlerCvs in _aCrawlerCVs)
            {
                foreach (var cv in crawlerCvs)
                {
                    CvMapper mapper = new CvMapper();

                    mapper.Insert(cv);
                }


            }

            Console.ReadKey();
        }
    }
}
