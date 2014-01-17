using System;
using System.Text.RegularExpressions;
using CrawlerBatch.Crawlers;
using CrawlerBatch.MatchyBackEnd;
using Match = System.Text.RegularExpressions.Match;
using CrawlerBatch.Data

namespace CrawlerBatchTests
{
    public class MockCrawler : ACrawler
    {
        private const String CrawlerName = "MockCrawler";
        private const String BaseUrl =
            "http://www.nationalevacaturebank.nl/vacature/zoeken/overzicht/relevant/query/ICT+OF+OF+OF+PHP+OF+OF+OF"
            + "JAVA+OF+OF+OF+.NET+OF+HTML+OF+CSS+OF+JavaScript+OF+Programmeur%2C+OF+Ontwikkelaar%2C+OF+IT%2C+OF+C%23/"
            + "distance/%3E+75km/output/html/items_per_page/10/page/ {0} /ignore_ids";

        #region Pattern Constants

        private const String GetLastPagePattern = @"rel=""nofollow"">(\d+)</a>\s+</li>\s+</ul>";
        private const String GetResultsPattern = @"<li id=""vacature-(\d+)"" class=""result-item .*?"">(.*?)</li>";
        private const String ResultDatePattern = @"<div class=""result-item-date"">.*?<a href="".*?"">(.*?)</a>.*?</div>";
        private const String ResultCompanyPattern = @"<h3 class=""result-item-company"">.*?<a .*?>(.*?)</a>.*?</h3>";
        private const String ResultTitlePattern = @"<h2 class=""result-item-title .*?"">.*?<a .*? title=""(.*?)"">.*?</a>.*?</h2>";
        private const String ResultCityPattern = @"<div class=""result-item-city"">.*?<span>.*?<a href="".*?"">(.*?)</a>.*?</span>.*?</div>";
        private const String ResultDescriptionPattern = @"<div class=""result-item-body"">.*?<p>.*?<a href="".*?"">(.*?)</a>.*?</p>.*?</div>";
        private const String ResultHoursPattern = @"<div class=""result-item-hours"">.*?<span>.*?<a .*?>(.*?)</a>.*?</span>.*?</div>";
        private const String ResultEducationPattern = @"<div class=""result-item-educationlevel"">.*?<a .*?>(.*?)</a>.*?</div>";

        private const String ResultCompanyTelPattern = @"Tel.:(.*?)<";
        private const String ResultCompanyEmailPattern = @"\b[a-zA-Z0-9.-]+@[a-zA-Z0-9.-]+\.[a-zA-Z0-9.-]+\b";
        private const String ResultCompanyAboutPattern = @"<h2>Bedrijfsprofiel</h2>\s+<p>(.*?)</div>";

        private const String ResultLinkPattern = @"<a class=""span-18 result-item-link"" href=""(.*?)"">.*?</a> ";
        private const String ResultDataPattern = @"<div .*? id=""vacature-details"">(.*?)</div>\s+<div id=""abuse-link"" class=""span-auto-r"">.*?";

        #endregion

        public int TotalMatches;
        private int _crawlerId ;



        public MockCrawler()
        {
            CurrentPage = 1;
            TotalMatches = 0;
            Url = BaseUrl;
            Name = CrawlerName;
            HasStarted = false;
        }

        public override void Process()
        {
            HasStarted = true;
            CrawlerData = urlHandler();

            GetPageNumbers();

            for (var currentPage = CurrentPage; currentPage <= Pages; currentPage++)
            {
                CurrentPage = currentPage;
                Console.WriteLine(CurrentPage);

                if (currentPage == 3)
                    break;

                var pageResults = Regex.Matches(CrawlerData, GetResultsPattern, RegexOptions.Singleline);

                foreach (Match result in pageResults)
                {
                    _crawlerId = Convert.ToInt32(result.Groups[1].Value);
                    var resultContent = result.Groups[2].Value;

                    TotalMatches++;
                    Jobs.Add(GetResultToResultSet(resultContent));
                }
            }
        }

        protected override void GetPageNumbers()
        {
            var pageMatches = Regex.Matches(CrawlerData, GetLastPagePattern, RegexOptions.Singleline);

            Pages = Convert.ToInt32(pageMatches[0].Groups[1].Value);
        }

        protected override void OpenNewPage()
        {
            Url = String.Format(BaseUrl, CurrentPage);

            CrawlerData = urlHandler();
        }

        /// <summary>
        /// Deze methode zorgd ervoor dat de opgehaalde string vertaald kan worden naar een Job object die vervolgens in de Job List komt te staan
        /// </summary>
        /// <param name="result">Een string van de opgehaalde html van een specifieke vacature of cv</param>
        /// <returns>Een Job object dat gecrawlde data bevat van een vacature</returns>
        private Job GetResultToResultSet(String result)
        {
            var sourceMapper = new SourceMapper();
            var job = new Job();
            var company = new Company();

            job.CrawlerID = _crawlerId;
            job.JobPlaceDate = StringToDateTime(GetCrawlerData(ResultDatePattern, result)).ToString();
            job.JobTitle = GetCrawlerData(ResultTitlePattern, result);
            job.JobDescription = GetCrawlerData(ResultDescriptionPattern, result);
            job.JobHours = GetCrawlerData(ResultHoursPattern, result);
            job.Education = DetermineEducation(GetCrawlerData(ResultEducationPattern, result));
            job.Source = DetermineSource();

            company.CompanyName = GetCrawlerData(ResultCompanyPattern, result);
            company.CompanyCity = GetCrawlerData(ResultCityPattern, result);
            company.CompanyDate = DateTime.Now.ToString();

            job.DetailJob = GetDetailledInfo(result, company);

            job.Company = company;

            return job;
        }
    }
}
