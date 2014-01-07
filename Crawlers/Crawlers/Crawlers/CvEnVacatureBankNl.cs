using System;
using System.Linq;
using System.Text.RegularExpressions;
using CrawlerBatch.Mappers;
using DataAccessObjects;
using Match = System.Text.RegularExpressions.Match;

namespace CrawlerBatch.Crawlers
{
    class CvEnVacatureBankNl : ACrawler
    {
        private const String BaseURL = "http://cvenvacaturebank.nl{0}";
        private const String URL = "http://cvenvacaturebank.nl/cv?o={0}&sector=25";
        private const String CrawlerName = "CvenVacatureBank";

        private const String GetLastPagePattern = @"<h2>CV</h2>.*?<b>.*?</b>.*?<b>(.*?)</b>.*?";
        private const String GetResultLinks = @"<a href=""(/cv/(\d+)/.*?.html)"">.*?</a>.*?";

        private int _crawlerID;

        public CvEnVacatureBankNl()
        {
            CurrentPage = 0;
            Url = String.Format(URL, "");
            Name = CrawlerName;
        }

        public override void Process()
        {
            CrawlerData = urlHandler();
            GetPageNumbers();

            int i = 0;
            for (int currentPage = CurrentPage; currentPage < Pages; currentPage++)
            {
                CurrentPage = currentPage;
                Console.WriteLine(CurrentPage);

                if (currentPage != 0)
                    OpenNewPage();

                var pageResults = Regex.Matches(CrawlerData, GetResultLinks, RegexOptions.Singleline);

                foreach (Match pageResult in pageResults)
                {
                    String resultLink = pageResult.Groups[1].Value;
                    _crawlerID = Convert.ToInt32(pageResult.Groups[2].Value);

                    Cvs.Add(GetResultToResultSet(resultLink));
                    i++;
                    Console.WriteLine(i);
                }
//                foreach (string resultLink in from Match result in pageResults select result.Groups[1].Value)
//                {
//                    Cvs.Add(GetResultToResultSet(resultLink));
//                    i++;
//                    Console.WriteLine(i);
//                }
            }

            Console.WriteLine(String.Format("Crawling {0} Complete", CrawlerName));
        }

        protected override void GetPageNumbers()
        {
            var pageMatches = Regex.Matches(CrawlerData, GetLastPagePattern, RegexOptions.Singleline);

            var results = Convert.ToInt32(pageMatches[0].Groups[1].Value);

            Pages = (int) Math.Ceiling(Convert.ToDouble(results/20));
        }

        protected override void OpenNewPage()
        {
            Url = String.Format(URL, CurrentPage * 20);

            CrawlerData = urlHandler();
        }

        public Cv GetResultToResultSet(string link)
        {
            const String resultProfessionPattern = @"<span>Beroep</span>.*?<div class=jump>(.*?)</div>";
            const String resultDatePattern = @"<span>Geplaatst op</span>.*?<div class=jump>(.*?)</div>";
            const String resultDisciplinePattern = @"<span>Vakgebied</span>.*?<div class=jump>(.*?)</div>";
            const String resultEducationLevelPattern = @"<span>Niveau</span>.*?<div class=jump>(.*?)</div>";
            const String resultHoursPattern = @"<span>Dienstverband</span>.*?<div class=jump>(.*?)</div>";
            const String resultCityPattern = @"<span>Woonplaats</span>.*?<div class=jump>(.*?)</div>";
            const String resultProvincePattern = @"<span>Provincie</span>.*?<div class=jump>(.*?)</div>"; 
            const String resultSexPattern = @"<span>Geslacht</span>.*?<div class=jump>(.*?)</div>";
            const String resultAgePattern = @"<span>Leeftijd</span>.*?<div class=jump>([1][0-9][0-9]|[0-9][0-9]|[0-9]).*?</div>";
            const String resultExperiencePattern = @"<span>Werkervaring en stages</span>.*?<div class=jump>(.*?)</div>";
            const String resultEducationPattern = @"<span>Opleiding</span>.*?<div class=jump>(.*?)</div>";
            const String resultPersonalPattern = @"<span>Persoonlijk</span>.*?<div class=jump>(.*?)</div>";
            const String resultInterestPattern = @"<span>Kennis, hobby en interesses</span>.*?<div class=jump>(.*?)</div>";
            const String resultJobRequirementsPattern = @"<span>Zoekt in baan</span>.*?<div class=jump>(.*?)</div>";
            
            var cv = new Cv();

            Url = String.Format(BaseURL, link);
            CrawlerData = urlHandler();

            cv.CrawlerId = _crawlerID;
            cv.Date = StringToDateTime(GetCrawlerData(resultDatePattern)).ToString("yyyy-MM-dd HH:mm:ss");
            cv.Profession = GetCrawlerData(resultProfessionPattern);
            cv.Discipline = GetCrawlerData(resultDisciplinePattern);
            cv.Hours = GetCrawlerData(resultHoursPattern);
            cv.City = GetCrawlerData(resultCityPattern);
            cv.Province = GetCrawlerData(resultProvincePattern);
            cv.Sex = GetCrawlerData(resultSexPattern); 
            cv.Age = Convert.ToInt16(GetCrawlerData(resultAgePattern ,null ,true));
            cv.WorkExperience = GetCrawlerData(resultExperiencePattern);
            cv.EducationHistory = GetCrawlerData(resultEducationPattern);
            cv.EducationLevel = DetermineEducation(GetCrawlerData(resultEducationLevelPattern));
            cv.Source = DetermineSource();

            cv.Personal = GetCrawlerData(resultPersonalPattern);
            cv.Interests = GetCrawlerData(resultInterestPattern);
            cv.JobRequirements = GetCrawlerData(resultJobRequirementsPattern);

            //Console.WriteLine();
            //Console.WriteLine();

            return cv;
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
