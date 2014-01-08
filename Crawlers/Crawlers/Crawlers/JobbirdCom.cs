using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using CrawlerBatch.Mappers;
using DataAccessObjects;
using Match = System.Text.RegularExpressions.Match;

namespace CrawlerBatch.Crawlers
{
    class JobbirdCom : ACrawler
    {
        private const String CrawlerName = "JobBirdCom";

        private const String BaseURL = "http://www.jobbird.com";
        private const String URL = "http://www.jobbird.com/nl/kandidaat/vacature-zoekresultaat?{0}{1}{2}search=it";

        private const String GetLastPagePattern = @"<li class=""pager-last last"">.*?<a href="".*?page=(\d+).*?"" class=""active"">";
        private const String GetResultsPattern = @"<a class=""linkVacature .*?"" .*?>(.*?)</a>";

        private const String ResultCompanyPattern = @"<table.*? onClick=\""redirect\('/nl/vacature/(\d+)/.*?\)"">";
        private const String ResultSearchCriteria = @"<div class=""zoekCriteria"">(.*?)</div>";
        private const String ResultTitlePattern = @"<span class=""titel"">(.*?)</span>";
        private const String ResultDescriptionPattern = @"<div class=""omschrijving"">(.*?)</div>";

       // private enum EductionLevels { HBO, MBO, Universitair, HAVO, [Description("VMBO / MAVO")]VMBO, VWO, LBO};
  
        public JobbirdCom()
        {
            CurrentPage = 0;
            Url = String.Format(URL, "","","");
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

            for (int currentPage = CurrentPage; currentPage < Pages; currentPage++)
            {
                CurrentPage = currentPage;
                Console.WriteLine(CurrentPage);

                if (currentPage == 5)
                    break;

                if (currentPage != 0)
                    OpenNewPage();

                

                var pageResults = Regex.Matches(CrawlerData, GetResultsPattern, RegexOptions.Singleline);

                var i = 0;
                foreach (string resultContent in from Match result in pageResults select result.Groups[1].Value)
                {
                    Jobs.Add(GetResultToResultSet(resultContent));
                    i++;
                    Console.WriteLine(i.ToString());
                }
            }

            Console.WriteLine(String.Format("Crawling {0} Complete", CrawlerName));
        }

        /// <summary>
        /// Deze methode haalt de hoeveelheid pagina's op voor de crawler. Bijv de pagina die gecrawled 
        /// moet worden bevat 52 pagina's waardoor gescrolled moet worden
        /// </summary>
        protected override void GetPageNumbers()
        {
            MatchCollection pageMatches = Regex.Matches(CrawlerData, GetLastPagePattern, RegexOptions.Singleline);

            Pages = Convert.ToInt32(pageMatches[0].Groups[1].Value);
        }

        /// <summary>
        /// Deze methode zorgt ervoor dat de crawler naar een nieuwe pagina kan navigeren
        /// </summary>
        protected override void OpenNewPage()
        {
            Url = String.Format(URL, "page=", CurrentPage, "&");

            CrawlerData = urlHandler();
        }

        /// <summary>
        /// Deze methode zorgd ervoor dat de opgehaalde string vertaald kan worden naar een Job object die vervolgens in de Job List komt te staan
        /// </summary>
        /// <param name="result">Een string van de opgehaalde html van een specifieke vacature</param>
        /// <returns>Een Job object dat gecrawlde data bevat van een vacature</returns>
        public Job GetResultToResultSet(String result)
        {
            var job = new Job();
            var company = new Company();

            job.CrawlerID = Convert.ToInt32(GetCrawlerData(ResultCompanyPattern, result, true));
            job.JobTitle = GetCrawlerData(ResultTitlePattern, result);
            job.JobDescription = GetCrawlerData(ResultDescriptionPattern, result);
            job.JobPlaceDate = DateTime.Now.ToString();
            job.Source = DetermineSource();

            GetSearchCriteriaFromResult(GetCrawlerData(ResultSearchCriteria, result), job, company);
            company.CompanyDate = DateTime.Now.ToString();

            job.DetailJob = GetDetailledInfo(result, company);

            job.Company = company;
            return job;
        }

        protected override DetailJob GetDetailledInfo(string input, Company company)
        {
            const String resultLinkPattern = @"<table class=""tabelVacature"" onClick=""redirect\('(.*?)'\)"">";
            const String resultDataPattern = @"<div class=""vacatureSubtitels"">(.*?)</div><div class=""VacatureDetail-buttons"">";
            const String resultCompanyLinkPattern = @"</div><a href=""(.*?)""><abbr title=""Bedrijfspresentatie"">";
            const String resultCompanyTelPattern = @"<span class=""field-content bedrijfspres-tel"">(.*?)</span>";
            const String resultCompanyEmailPattern = @"<span class=""field-content bedrijfspres-email"">.*?<a.*?>(.*?)</a>";
            const String resultCompanyAboutPattern = @"<div class=""views-field views-field-php-2"">.*?->\s+</span>(.*?)<a.*?>Volledige omschrijving</a>";
            const String resultCompanyNamePattern = @"<td valign=""middle""><h1>(.*?)</h1></td>";

            DetailJob detailJob = new DetailJob();

            String tempData = urlHandler(BaseURL + GetCrawlerData(resultLinkPattern, input));

            detailJob.Data = GetCrawlerData(resultDataPattern, tempData);

            if (!GetCrawlerData(resultCompanyLinkPattern, tempData).Equals("Niet Beschikbaar"))
            {
                tempData = urlHandler(BaseURL + GetCrawlerData(resultCompanyLinkPattern, tempData));

                company.CompanyDescription = GetCrawlerData(resultCompanyAboutPattern, tempData);
                company.CompanyEmail = GetCrawlerData(resultCompanyEmailPattern, tempData);
                company.CompanyTel = GetCrawlerData(resultCompanyTelPattern, tempData);
                company.CompanyName = GetCrawlerData(resultCompanyNamePattern, tempData);
            }
            else
            {
                company.CompanyDescription = "Niet Beschikbaar";
                company.CompanyEmail = "Niet Beschikbaar";
                company.CompanyTel = "Niet Beschikbaar";
                company.CompanyName = "Niet Beschikbaar";
            }
            
            return detailJob;
        }

        /// <summary>
        /// Deze methode zorgt ervoor dat de het zoekcriteria die in de resultaten staat per vacature wordt opgesplits en in de juiste 
        /// variabelen wordt geplaatst
        /// </summary>
        /// <param name="input">De zoekcriteria</param>
        /// <param name="job">Een referentie van het Job object</param>
        /// <param name="company">Een referentie van het Company object</param>
        private void GetSearchCriteriaFromResult(String input, Job job, Company company)
        {
            const string pattern = @"\s-\s";

            String[] result = Regex.Split(input, pattern);

            foreach (string t in result)
            {
                bool isEducation = false;
                bool isNumber = false;

                Education education = DetermineEducation(t);

                if (education != null)
                {
                    isEducation = true;
                    job.Education = education;
                }

                if (Regex.IsMatch(t, @"^\d"))
                {
                    job.JobHours = t;
                    isNumber = true;
                }
                    

                if (!t.Contains("Topbaan") && !t.Contains("zoekscore:") && !isEducation && !isNumber)
                    company.CompanyCity = t;
            }

            job.JobHours = job.JobHours ?? "Niet beschikbaar";
            job.Education = job.Education ?? new Education();
        }

        protected override Education DetermineEducation(string input)
        {
            Education result = null;

            EducationMapper mapper = new EducationMapper();

            foreach (var education in mapper.Get())
            {
                if (education.Name.Contains(input))
                {
                    result = education;   
                    break;
                }       
            }

            return result;
        }
    }
}
