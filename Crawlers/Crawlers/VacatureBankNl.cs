using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crawlers
{
    class VacatureBankNl : AVacCrawler
    {
        private String url = "http://www.nationalevacaturebank.nl/vacature/zoeken/overzicht/relevant/query/ICT+OF+OF+OF+PHP+OF+OF+OF" +
                                    "JAVA+OF+OF+OF+.NET+OF+HTML+OF+CSS+OF+JavaScript+OF+Programmeur%2C+OF+Ontwikkelaar%2C+OF+IT%2C+OF+C%23/" +
                                    "distance/%3E+75km/output/html/items_per_page/10/page/ {0} /ignore_ids";

        private const String getLastPagePattern = @"rel=""nofollow"">(\d+)</a>\s+</li>\s+</ul>";
        private const String getResultsPattern = @"<a class=""span-18 result-item-link"" href=""(.*?)"".*?</a>";

        public VacatureBankNl() : base()
        {
            CurrentPage = 1;
            Url = url;
            CrawlerData = urlHandler();
        }

        public override void Process()
        {
            Console.WriteLine("Crawling started");
            getPageNumbers();

            for (int currentPage = CurrentPage; currentPage <= Pages; currentPage++)
            {
                CurrentPage = currentPage;
                Console.WriteLine(CurrentPage);

                if (CurrentPage != 1)
                    openNewPage();

                MatchCollection pageResults = Regex.Matches(CrawlerData, getResultsPattern, RegexOptions.Singleline);

                foreach(Match result in pageResults) {
                    String resultURL = result.Groups[1].Value;

                    CrawlerData = urlHandler(resultURL);

                    Jobs.Add(getResultToVacture());
                }
            }

            Console.WriteLine("Crawling Nationale VacaturebankComplete");
            Console.ReadKey();
        }

        private void getPageNumbers()
        {
            MatchCollection pageMatches = Regex.Matches(crawlerData, getLastPagePattern, RegexOptions.Singleline);

            this.pages = Convert.ToInt32(pageMatches[0].Groups[1].Value);
        }

        private void openNewPage()
        {
            Url = this.url;

            CrawlerData = urlHandler();
        }

        private Job getResultToVacture()
        {
            Job job = new Job();
            Company company = new Company();

            String metaInfoPattern = @"<meta name=""title"" content=""(.*?)"" />";
            String metaInfo = (Regex.Matches(CrawlerData, metaInfoPattern, RegexOptions.Singleline))[0].Groups[1].Value;
            String[] jobInfo = metaInfo.Split('|');

            Console.WriteLine(jobInfo[0] + ", " + jobInfo[1]);

            //String jobTitelPattern = @"<h1>(.*?)</h1>";
            //MatchCollection result = Regex.Matches(CrawlerData, jobTitelPattern, RegexOptions.Singleline);

            job.JobTitle = jobInfo[0];
            company.CompanyName = jobInfo[1];


            job.Company = company;
            //String companyTitlePattern = @"<dl>.*?<dt>Bedrijf:</dt>.*?<dd>(.*?)</dd>";
            //company.CompanyName = (Regex.Matches(CrawlerData, companyTitlePattern, RegexOptions.Singleline))[0].Groups[0].Value;

            //String jobDescriptionPattern = @"<h2>Functieomschrijving</h2>.*?<p>(.*?)</p>";
            //job.JobDescription = (Regex.Matches(CrawlerData, jobDescriptionPattern, RegexOptions.Singleline))[0].Groups[0].Value;


            return job;
        }
    }
}
