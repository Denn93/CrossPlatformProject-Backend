using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crawlers
{
    class VacatureBankNl : AVacCrawler
    {
        private const String URL = "http://www.nationalevacaturebank.nl/vacature/zoeken/overzicht/relevant/query/ICT+OF+OF+OF+PHP+OF+OF+OF" 
                                 + "JAVA+OF+OF+OF+.NET+OF+HTML+OF+CSS+OF+JavaScript+OF+Programmeur%2C+OF+Ontwikkelaar%2C+OF+IT%2C+OF+C%23/" 
                                 + "distance/%3E+75km/output/html/items_per_page/10/page/ {0} /ignore_ids";

        private const String GetLastPagePattern = @"rel=""nofollow"">(\d+)</a>\s+</li>\s+</ul>";
        private const String GetResultsPattern = @"<li id=""vacature-\d+"" class=""result-item .*?"">(.*?)</li>";

        private const String ResultDatePattern = @"<div class=""result-item-date"">.*?<a href="".*?"">(.*?)</a>.*?</div>";
        private const String ResultCompanyPattern = @"<h3 class=""result-item-company"">.*?<a .*?>(.*?)</a>.*?</h3>";
        private const String ResultTitlePattern = @"<h2 class=""result-item-title .*?"">.*?<a .*? title=""(.*?)"">.*?</a>.*?</h2>";
        private const String ResultCityPattern = @"<div class=""result-item-city"">.*?<span>.*?<a href="".*?"">(.*?)</a>.*?</span>.*?</div>";
        private const String ResultDescriptionPattern = @"<div class=""result-item-body"">.*?<p>.*?<a href="".*?"">(.*?)</a>.*?</p>.*?</div>";
        private const String ResultHoursPattern = @"<div class=""result-item-hours"">.*?<span>.*?<a .*?>(.*?)</a>.*?</span>.*?</div>";
        private const String ResultEducationPattern = @"<div class=""result-item-educationlevel"">.*?<a .*?>(.*?)</a>.*?</div>";

        public VacatureBankNl() 
        {
            CurrentPage = 1;
            Url = URL;
            CrawlerData = urlHandler();
        }

        public override void Process()
        {
            Console.WriteLine("Crawling started");
            GetPageNumbers();

            for (int currentPage = CurrentPage; currentPage <= Pages; currentPage++)
            {
                CurrentPage = currentPage;
                Console.WriteLine(CurrentPage);

                if (currentPage != 1)
                    OpenNewPage();

                MatchCollection pageResults = Regex.Matches(CrawlerData, GetResultsPattern, RegexOptions.Singleline);

                foreach(Match result in pageResults) {
                    String resultContent = result.Groups[1].Value;

                    Jobs.Add(GetResultToJob(resultContent));
                }
            }

            Console.WriteLine("Crawling Nationale Vacaturebank Complete");
            Console.ReadKey();
        }

        private void GetPageNumbers()
        {
            MatchCollection pageMatches = Regex.Matches(CrawlerData, GetLastPagePattern, RegexOptions.Singleline);

            this.Pages = Convert.ToInt32(pageMatches[0].Groups[1].Value);
        }

        private void OpenNewPage()
        {
            Url = String.Format(URL, CurrentPage);

            CrawlerData = urlHandler();
        }

        private Job GetResultToJob(String result)
        {
            Job job = new Job();
            Company company = new Company();

            job.JobPlaceDate = StringToDateTime((Regex.Matches(result, ResultDatePattern, RegexOptions.Singleline))[0].Groups[1].Value);

            company.CompanyName = (Regex.Matches(result, ResultCompanyPattern, RegexOptions.Singleline))[0].Groups[1].Value;
            job.JobTitle = (Regex.Matches(result, ResultTitlePattern, RegexOptions.Singleline))[0].Groups[1].Value;
            company.CompanyCity = (Regex.Matches(result, ResultCityPattern, RegexOptions.Singleline))[0].Groups[1].Value;
            job.JobDescription = (Regex.Matches(result, ResultDescriptionPattern, RegexOptions.Singleline))[0].Groups[1].Value;
            job.JobHours = (Regex.Matches(result, ResultHoursPattern, RegexOptions.Singleline))[0].Groups[1].Value;
            job.JobEducation = (Regex.Matches(result, ResultEducationPattern, RegexOptions.Singleline))[0].Groups[1].Value;

            Console.WriteLine(job.JobTitle);
            Console.WriteLine(job.JobDescription);
            Console.WriteLine(job.JobPlaceDate);
            Console.WriteLine(job.JobHours);



            job.Company = company;
            return job;
        }

        private DateTime StringToDateTime(String date)
        {
            String[] dateInfo = date.Split('-');

            int day = Convert.ToInt32(dateInfo[0]);
            int month = Convert.ToInt32(dateInfo[1]);;
            int year = Convert.ToInt32(dateInfo[2]); ;

            return new DateTime(year, month, day);
        }
    }
}
