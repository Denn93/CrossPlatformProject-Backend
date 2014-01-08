using System;
using System.Text.RegularExpressions;
using DataAccessObjects;
using Match = System.Text.RegularExpressions.Match;

namespace CrawlerBatch.Crawlers
{
    class CvEnVacatureBankNl : ACrawler
    {
        private const String BaseURL = "http://cvenvacaturebank.nl{0}";
        private const String URL = "http://cvenvacaturebank.nl/cv?o={0}&sector=25";
        private const String CrawlerName = "CvenVacatureBank";

        #region Pattern Constants

        private const String GetLastPagePattern = @"<h2>CV</h2>.*?<b>.*?</b>.*?<b>(.*?)</b>.*?";
        private const String GetResultLinks = @"<a href=""(/cv/(\d+)/.*?.html)"">.*?</a>.*?";

        private const String ResultProfessionPattern = @"<span>Beroep</span>.*?<div class=jump>(.*?)</div>";
        private const String ResultDatePattern = @"<span>Geplaatst op</span>.*?<div class=jump>(.*?)</div>";
        private const String ResultDisciplinePattern = @"<span>Vakgebied</span>.*?<div class=jump>(.*?)</div>";
        private const String ResultEducationLevelPattern = @"<span>Niveau</span>.*?<div class=jump>(.*?)</div>";
        private const String ResultHoursPattern = @"<span>Dienstverband</span>.*?<div class=jump>(.*?)</div>";
        private const String ResultCityPattern = @"<span>Woonplaats</span>.*?<div class=jump>(.*?)</div>";
        private const String ResultProvincePattern = @"<span>Provincie</span>.*?<div class=jump>(.*?)</div>";
        private const String ResultSexPattern = @"<span>Geslacht</span>.*?<div class=jump>(.*?)</div>";
        private const String ResultAgePattern = @"<span>Leeftijd</span>.*?<div class=jump>([1][0-9][0-9]|[0-9][0-9]|[0-9]).*?</div>";
        private const String ResultExperiencePattern = @"<span>Werkervaring en stages</span>.*?<div class=jump>(.*?)</div>";
        private const String ResultEducationPattern = @"<span>Opleiding</span>.*?<div class=jump>(.*?)</div>";
        private const String ResultPersonalPattern = @"<span>Persoonlijk</span>.*?<div class=jump>(.*?)</div>";
        private const String ResultInterestPattern = @"<span>Kennis, hobby en interesses</span>.*?<div class=jump>(.*?)</div>";
        private const String ResultJobRequirementsPattern = @"<span>Zoekt in baan</span>.*?<div class=jump>(.*?)</div>";

        #endregion

        private int _crawlerID;

        public CvEnVacatureBankNl()
        {
            CurrentPage = 0;
            Url = String.Format(URL, "");
            Name = CrawlerName;
        }

        /// <summary>
        /// Methode Process bevat een filter die toegepast wordt op de gevonden data
        /// </summary>
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

            }
        }

        /// <summary>
        /// Deze methode haalt de hoeveelheid pagina's op voor de crawler. Bijv de pagina die gecrawled 
        /// moet worden bevat 52 pagina's waardoor gescrolled moet worden
        /// </summary>
        protected override void GetPageNumbers()
        {
            var pageMatches = Regex.Matches(CrawlerData, GetLastPagePattern, RegexOptions.Singleline);

            var results = Convert.ToInt32(pageMatches[0].Groups[1].Value);

            Pages = (int) Math.Ceiling(Convert.ToDouble(results/20));
        }

        /// <summary>
        /// Deze methode zorgt ervoor dat de crawler naar een nieuwe pagina kan navigeren
        /// </summary>
        protected override void OpenNewPage()
        {
            Url = String.Format(URL, CurrentPage * 20);

            CrawlerData = urlHandler();
        }

        /// <summary>
        /// Deze methode zorgt ervoor dat de opgehaalde string vertaald kan worden naar een Cv object die vervolgens in de Cv List komt te staan
        /// </summary>
        /// <param name="link">Een string van de opgehaalde html van een specifieke vacature</param>
        /// <returns>Een Cv object dat gecrawlde data bevat van een vacature</returns>
        public Cv GetResultToResultSet(string link)
        {
            var cv = new Cv();

            Url = String.Format(BaseURL, link);
            CrawlerData = urlHandler();

            cv.CrawlerId = _crawlerID;
            cv.Date = StringToDateTime(GetCrawlerData(ResultDatePattern)).ToString("yyyy-MM-dd HH:mm:ss");
            cv.Profession = GetCrawlerData(ResultProfessionPattern);
            cv.Discipline = GetCrawlerData(ResultDisciplinePattern);
            cv.Hours = GetCrawlerData(ResultHoursPattern);
            cv.City = GetCrawlerData(ResultCityPattern);
            cv.Province = GetCrawlerData(ResultProvincePattern);
            cv.Sex = GetCrawlerData(ResultSexPattern); 
            cv.Age = Convert.ToInt16(GetCrawlerData(ResultAgePattern ,null ,true));
            cv.WorkExperience = GetCrawlerData(ResultExperiencePattern);
            cv.EducationHistory = GetCrawlerData(ResultEducationPattern);
            cv.EducationLevel = DetermineEducation(GetCrawlerData(ResultEducationLevelPattern));
            cv.Source = DetermineSource();

            cv.Personal = GetCrawlerData(ResultPersonalPattern);
            cv.Interests = GetCrawlerData(ResultInterestPattern);
            cv.JobRequirements = GetCrawlerData(ResultJobRequirementsPattern);

            return cv;
        }
    }
}
