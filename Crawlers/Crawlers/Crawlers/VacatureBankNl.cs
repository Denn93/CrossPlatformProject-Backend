using System;
using System.Text.RegularExpressions;
using DataAccessObjects;
using Match = System.Text.RegularExpressions.Match;

namespace CrawlerBatch.Crawlers
{
    internal class VacatureBankNl : ACrawler
    {
        private const String CrawlerName = "VacaturebankNL";

        private const String URL =
            "http://www.nationalevacaturebank.nl/vacature/zoeken/overzicht/relevant/query/ICT+OF+OF+OF+PHP+OF+OF+OF"
            + "JAVA+OF+OF+OF+.NET+OF+HTML+OF+CSS+OF+JavaScript+OF+Programmeur%2C+OF+Ontwikkelaar%2C+OF+IT%2C+OF+C%23/"
            + "distance/%3E+75km/output/html/items_per_page/10/page/ {0} /ignore_ids";

        private const String GetLastPagePattern = @"rel=""nofollow"">(\d+)</a>\s+</li>\s+</ul>";
        private const String GetResultsPattern = @"<li id=""vacature-(\d+)"" class=""result-item .*?"">(.*?)</li>";
        private const String ResultDatePattern = @"<div class=""result-item-date"">.*?<a href="".*?"">(.*?)</a>.*?</div>";
        private const String ResultCompanyPattern = @"<h3 class=""result-item-company"">.*?<a .*?>(.*?)</a>.*?</h3>";
        private const String ResultTitlePattern = @"<h2 class=""result-item-title .*?"">.*?<a .*? title=""(.*?)"">.*?</a>.*?</h2>";
        private const String ResultCityPattern = @"<div class=""result-item-city"">.*?<span>.*?<a href="".*?"">(.*?)</a>.*?</span>.*?</div>";
        private const String ResultDescriptionPattern = @"<div class=""result-item-body"">.*?<p>.*?<a href="".*?"">(.*?)</a>.*?</p>.*?</div>";
        private const String ResultHoursPattern = @"<div class=""result-item-hours"">.*?<span>.*?<a .*?>(.*?)</a>.*?</span>.*?</div>";
        private const String ResultEducationPattern =  @"<div class=""result-item-educationlevel"">.*?<a .*?>(.*?)</a>.*?</div>";

        private int _crawlerID;

        public VacatureBankNl()
        {
            CurrentPage = 1;
            Url = URL;

            Name = CrawlerName;
        }

        /// <summary>
        /// Methode Process bevat een filter die toegepast wordt op de gevonden data
        /// </summary>
        public override void Process()
        {
            CrawlerData = urlHandler();

            Console.WriteLine("Crawling started");
            GetPageNumbers();

            for (var currentPage = CurrentPage; currentPage <= Pages; currentPage++)
            {
                CurrentPage = currentPage;
                Console.WriteLine(CurrentPage);

                if (currentPage != 1)
                    OpenNewPage();

                var pageResults = Regex.Matches(CrawlerData, GetResultsPattern, RegexOptions.Singleline);

                foreach (Match result in pageResults)
                {
                    _crawlerID = Convert.ToInt32(result.Groups[1].Value);

                    var resultContent = result.Groups[2].Value;

                    Jobs.Add(GetResultToResultSet(resultContent));
                }
            }

            Console.WriteLine("Crawling Nationale Vacaturebank Complete");
            Console.ReadKey();
        }

        /// <summary>
        /// Deze methode haalt de hoeveelheid pagina's op voor de crawler. Bijv de pagina die gecrawled 
        /// moet worden bevat 52 pagina's waardoor gescrolled moet worden
        /// </summary>
        protected override void GetPageNumbers()
        {
            var pageMatches = Regex.Matches(CrawlerData, GetLastPagePattern, RegexOptions.Singleline);

            Pages = Convert.ToInt32(pageMatches[0].Groups[1].Value);
        }

        /// <summary>
        /// Deze methode zorgt ervoor dat de crawler naar een nieuwe pagina kan navigeren
        /// </summary>
        protected override void OpenNewPage()
        {
            Url = String.Format(URL, CurrentPage);

            CrawlerData = urlHandler();
        }

        protected override DetailJob GetDetailledInfo(string input)
        {
            DetailJob detailJob = new DetailJob();
            const String resultLinkPattern = @"<a class=""span-18 result-item-link"" href=""(.*?)"">.*?</a> ";
            const String resultDataPattern = @"<div .*? id=""vacature-details"">(.*?)</div>\s+<div id=""abuse-link"" class=""span-auto-r"">.*?";

            String tempData = urlHandler(GetCrawlerData(resultLinkPattern, input));

            detailJob.Data = GetCrawlerData(resultDataPattern, tempData);

            return detailJob;
        }

        /// <summary>
        /// Deze methode zorgd ervoor dat de opgehaalde string vertaald kan worden naar een Job object die vervolgens in de Job List komt te staan
        /// </summary>
        /// <param name="result">Een string van de opgehaalde html van een specifieke vacature</param>
        /// <returns>Een Job object dat gecrawlde data bevat van een vacature</returns>
        private Job GetResultToResultSet(String result)
        {
            var job = new Job();
            var company = new Company();

            job.CrawlerID = _crawlerID;
            job.JobPlaceDate = StringToDateTime(GetCrawlerData(ResultDatePattern, result));
            job.JobTitle = GetCrawlerData(ResultTitlePattern, result);
            job.JobDescription = GetCrawlerData(ResultDescriptionPattern, result);
            job.JobHours = GetCrawlerData(ResultHoursPattern, result);
            job.Education = DetermineEducation(GetCrawlerData(ResultEducationPattern, result));
            job.Source = DetermineSource();

            company.CompanyName = GetCrawlerData(ResultCompanyPattern, result);
            company.CompanyCity = GetCrawlerData(ResultCityPattern, result);
            company.CompanyDate = DateTime.Now;

            job.Company = company;

            job.DetailJob = GetDetailledInfo(result);

            return job;
        }

        /// <summary>
        /// Handmatige converter van String naar een DataTime object
        /// </summary>
        /// <param name="date">Bevat de datum die geconverteerd moet worden</param>
        /// <returns>Een DateTime object met de datum uit String date</returns>
        private DateTime StringToDateTime(String date)
        {
            var dateInfo = date.Split('-');

            var day = Convert.ToInt32(dateInfo[0]);
            var month = Convert.ToInt32(dateInfo[1]);
            var year = Convert.ToInt32(dateInfo[2]);

            return new DateTime(year, month, day);
        }
    }
}
