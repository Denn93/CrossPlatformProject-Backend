using System;
using System.Text.RegularExpressions;
using DataAccessObjects;
using Match = System.Text.RegularExpressions.Match;

namespace CrawlerBatch.Crawlers
{
    public class VacatureBankNl : ACrawler
    {
        private const String CrawlerName = "VacaturebankNL";

        private const String URL =
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
        private const String ResultEducationPattern =  @"<div class=""result-item-educationlevel"">.*?<a .*?>(.*?)</a>.*?</div>";

        private const String ResultCompanyTelPattern = @"Tel.:(.*?)<";
        private const String ResultCompanyEmailPattern = @"\b[a-zA-Z0-9.-]+@[a-zA-Z0-9.-]+\.[a-zA-Z0-9.-]+\b";
        private const String ResultCompanyAboutPattern = @"<h2>Bedrijfsprofiel</h2>\s+<p>(.*?)</div>";

        private const String ResultLinkPattern = @"<a class=""span-18 result-item-link"" href=""(.*?)"">.*?</a> ";
        private const String ResultDataPattern = @"<div .*? id=""vacature-details"">(.*?)</div>\s+<div id=""abuse-link"" class=""span-auto-r"">.*?";

        #endregion

        private int _crawlerId;

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
                    _crawlerId = Convert.ToInt32(result.Groups[1].Value);

                    var resultContent = result.Groups[2].Value;

                    Jobs.Add(GetResultToResultSet(resultContent));
                }
            }
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

        /// <summary>
        /// Deze methode zal de vacature helemaal openen. En vervolgens de data die hier in staat crawlen en verwerken. 
        /// Deze methode throws een NotSupportedExceptio zodat een crawler die deze methode niet overrided hem ook niet kan gebruiken. 
        /// Wanneer deze abstract is moet hij geimplementeerd worden en dat is nu niet het geval.
        /// </summary>
        /// <param name="input"><De input data waarin gecrawled moet gaan worden/param>
        /// <param name="company">Het company object die mede hier ook gevuld wordt. </param>
        /// <returns>Deze methode returned vervolgens een DetailJob</returns>
        protected override DetailJob GetDetailledInfo(string input, Company company)
        {
            DetailJob detailJob = new DetailJob();
            String tempData = urlHandler(GetCrawlerData(ResultLinkPattern, input));

            detailJob.Data = GetCrawlerData(ResultDataPattern, tempData);
            company.CompanyTel = GetCrawlerData(ResultCompanyTelPattern, tempData);
            company.CompanyEmail = GetCrawlerData(ResultCompanyEmailPattern, tempData);
            company.CompanyDescription = GetCrawlerData(ResultCompanyAboutPattern, tempData);

            return detailJob;
        }

        /// <summary>
        /// Deze methode zorgd ervoor dat de opgehaalde string vertaald kan worden naar een Job object die vervolgens in de Job List komt te staan
        /// </summary>
        /// <param name="result">Een string van de opgehaalde html van een specifieke vacature of cv</param>
        /// <returns>Een Job object dat gecrawlde data bevat van een vacature</returns>
        private Job GetResultToResultSet(String result)
        {
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
