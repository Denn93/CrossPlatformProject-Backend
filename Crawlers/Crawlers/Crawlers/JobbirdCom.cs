using System;
using System.Linq;
using System.Text.RegularExpressions;
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

        #region Pattern Constants
        private const String GetLastPagePattern = @"<li class=""pager-last last"">.*?<a href="".*?page=(\d+).*?"" class=""active"">";
        private const String GetResultsPattern = @"<a class=""linkVacature .*?"" .*?>(.*?)</a>";

        private const String ResultCompanyPattern = @"<table.*? onClick=\""redirect\('/nl/vacature/(\d+)/.*?\)"">";
        private const String ResultSearchCriteria = @"<div class=""zoekCriteria"">(.*?)</div>";
        private const String ResultTitlePattern = @"<span class=""titel"">(.*?)</span>";
        private const String ResultDescriptionPattern = @"<div class=""omschrijving"">(.*?)</div>";

        private const String ResultLinkPattern = @"<table class=""tabelVacature"" onClick=""redirect\('(.*?)'\)"">";
        private const String ResultDataPattern = @"<div class=""vacatureSubtitels"">(.*?)</div><div class=""VacatureDetail-buttons"">";
        private const String ResultCompanyLinkPattern = @"</div><a href=""(.*?)""><abbr title=""Bedrijfspresentatie"">";
        private const String ResultCompanyTelPattern = @"<span class=""field-content bedrijfspres-tel"">(.*?)</span>";
        private const String ResultCompanyEmailPattern = @"<span class=""field-content bedrijfspres-email"">.*?<a.*?>(.*?)</a>";
        private const String ResultCompanyAboutPattern = @"<div class=""views-field views-field-php-2"">.*?->\s+</span>(.*?)<a.*?>Volledige omschrijving</a>";
        private const String ResultCompanyNamePattern = @"<td valign=""middle""><h1>(.*?)</h1></td>";

        private const string SearchCriteriaSplitPattern = @"\s-\s";
        #endregion

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

        /// <summary>
        /// Deze methode zal de vacature helemaal openen. En vervolgens de data die hier in staat crawlen en verwerken. 
        /// Deze methode throws een NotSupportedException zodat een crawler die deze methode niet overrided hem ook niet kan gebruiken. 
        /// Wanneer deze abstract is moet hij geimplementeerd worden en dat is nu niet het geval.
        /// </summary>
        /// <param name="input"><De input data waarin gecrawled moet gaan worden/param>
        /// <param name="company">Het company object die mede hier ook gevuld wordt. </param>
        /// <returns>Deze methode returned vervolgens een DetailJob</returns>
        protected override DetailJob GetDetailledInfo(string input, Company company)
        {
            DetailJob detailJob = new DetailJob();

            String tempData = urlHandler(BaseURL + GetCrawlerData(ResultLinkPattern, input));

            detailJob.Data = GetCrawlerData(ResultDataPattern, tempData);

            if (!GetCrawlerData(ResultCompanyLinkPattern, tempData).Equals(StringValueNotFound))
            {
                tempData = urlHandler(BaseURL + GetCrawlerData(ResultCompanyLinkPattern, tempData));

                company.CompanyDescription = GetCrawlerData(ResultCompanyAboutPattern, tempData);
                company.CompanyEmail = GetCrawlerData(ResultCompanyEmailPattern, tempData);
                company.CompanyTel = GetCrawlerData(ResultCompanyTelPattern, tempData);
                company.CompanyName = GetCrawlerData(ResultCompanyNamePattern, tempData);
            }
            else
            {
                company.CompanyDescription = StringValueNotFound;
                company.CompanyEmail = StringValueNotFound;
                company.CompanyTel = StringValueNotFound;
                company.CompanyName = StringValueNotFound;
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
            String[] result = Regex.Split(input, SearchCriteriaSplitPattern);

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

            job.JobHours = job.JobHours ?? StringValueNotFound;
            job.Education = job.Education ?? new Education();
        }

        /// <summary>
        /// Deze methode wordt gebruikt om te bepalen wat de Education is van een bepaalde string. Er wordt gekeken in de database en vanuit
        /// daar wordt er bepaald wat het educationID is. Bestaat hij niet dan zal hij deze toevoegen in database en een id terugsturen
        /// </summary>
        /// <param name="input">De string die naar education gezet moet worden</param>
        /// <returns>Een Education Object. Met educationID en description</returns>
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
