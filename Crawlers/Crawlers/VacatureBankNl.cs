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
        private const String URL = "http://www.nationalevacaturebank.nl/vacature/zoeken/overzicht/relevant/query/ICT+OF+OF+OF+PHP+OF+OF+OF" +
                                    "JAVA+OF+OF+OF+.NET+OF+HTML+OF+CSS+OF+JavaScript+OF+Programmeur%2C+OF+Ontwikkelaar%2C+OF+IT%2C+OF+C%23/" +
                                    "distance/%3E+75km/output/html/items_per_page/50/page/1/ignore_ids";

        private const String PagePattern = @"<ul class=""pagination-pages"">.*?</ul>";

        public VacatureBankNl()
        {
            Url = URL;
        }

        public override void Process()
        {
            crawlerData = urlHandler(); 

            var r = new Regex(PagePattern, RegexOptions.Singleline);

            foreach (Match match in r.Matches(crawlerData))
            {
                Console.WriteLine(match.Value);
            }
            Console.ReadKey();

            
        }
    }
}
