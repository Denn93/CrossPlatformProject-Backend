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
        private const String GetResultLinks = @"<a href=""(/cv/\d+/.*?.html)"">.*?</a>.*?";

        private Education[] _educations;

        private readonly EducationMapper _educationMapper;

        public CvEnVacatureBankNl()
        {
            _educationMapper = new EducationMapper();
            _educations = _educationMapper.Get(0);

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

                foreach (string resultLink in from Match result in pageResults select result.Groups[1].Value)
                {
                    Cvs.Add(GetResultToResultSet(resultLink));
                    i++;
                    Console.WriteLine(i);
                }
            }   
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
            const String resultTitlePattern = @"<h2>(.*?)</h2>";
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

            var cv = new Cv();

            Url = String.Format(BaseURL, link);
            CrawlerData = urlHandler();

            cv.Title = GetCrawlerData(resultTitlePattern);
            cv.Date = Convert.ToDateTime(GetCrawlerData(resultDatePattern));
            cv.Profession = GetCrawlerData(resultProfessionPattern);
            cv.Discipline = GetCrawlerData(resultDisciplinePattern);
            cv.Hours = GetCrawlerData(resultHoursPattern);
            cv.City = GetCrawlerData(resultCityPattern);
            cv.Province = GetCrawlerData(resultProvincePattern);
            cv.Sex = GetCrawlerData(resultSexPattern);
            cv.Age = Convert.ToInt16(GetCrawlerData(resultAgePattern ,null ,true));
            cv.WorkExperience = GetCrawlerData(resultExperiencePattern);
            cv.Education = GetCrawlerData(resultEducationPattern);
            cv.EducationLevel = DetermineEducation(GetCrawlerData(resultEducationLevelPattern));
            cv.Source = DetermineSource();

            Console.WriteLine();
            Console.WriteLine();

            return cv;
        }
    }
}
