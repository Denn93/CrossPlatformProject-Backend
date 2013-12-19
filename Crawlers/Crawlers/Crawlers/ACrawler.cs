using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using CrawlerBatch.Mappers;
using DataAccessObjects;

namespace CrawlerBatch.Crawlers
{
    abstract class ACrawler
    {
        /// <summary>
        /// Protected constructor initialiseerd de List met vacatures 
        /// </summary>
        protected ACrawler()
        {
            Jobs = new List<Job>();
            Cvs = new List<Cv>();
            DetailJobs = new List<DetailJob>();
            Company comp = new Company();
        }

        /// <summary>
        /// Methode Process bevat een filter die toegepast wordt op de gevonden data
        /// </summary>
        public abstract void Process();

        /// <summary>
        /// Deze methode haalt de hoeveelheid pagina's op voor de crawler. Bijv de pagina die gecrawled 
        /// moet worden bevat 52 pagina's waardoor gescrolled moet worden
        /// </summary>
        protected abstract void GetPageNumbers();
      
        /// <summary>
        /// Deze methode zorgt ervoor dat de crawler naar een nieuwe pagina kan navigeren
        /// </summary>
        protected abstract void OpenNewPage();

        /// <summary>
        /// Deze methode zorgt ervoor dat de url opgehaald kan worden. Dit is bij elke crawler gelijk. 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected virtual String urlHandler(String url = null)
        {
            var webClient = new WebClient();

            var result = webClient.DownloadString(url ?? Url);

            return WebUtility.HtmlDecode(result);
        }

        protected virtual String GetCrawlerData(String pattern, String data = null, Boolean isInt = false, int groupId = 1)
        {
            if (data == null)
                data = CrawlerData;

            var resultRegex = Regex.Matches(data, pattern, RegexOptions.Singleline);
            String resultString;

            if (!isInt)
                resultString = (resultRegex.Count == 0) ? "Niet Beschikbaar" : resultRegex[0].Groups[groupId].Value;
            else
                resultString = (resultRegex.Count == 0) ? "0" : resultRegex[0].Groups[groupId].Value;

            Console.WriteLine(resultString);

            return resultString;
        }

        protected virtual Education DetermineEducation(String input)
        {
            Education result = null;

            var mapper = new EducationMapper();
            var educations = mapper.Get();

            foreach (var education in educations)
            {
                if (education.Name.Contains(input))
                {
                    result = education;
                    break;
                }
            }

            if (result == null)
            {
                result = new Education() { Name = input.Trim() };
                result.EducationId = mapper.Insert(result);
            }

            return result;
        }

        protected virtual Source DetermineSource()
        {
            Source result = null;

            var mapper = new SourceMapper();
            var sources = mapper.Get();

            foreach (var source in sources)
            {
                if (source.Description.Equals(Name))
                {
                    result = source;
                    break;
                }
            }

            if (result == null)
            {
                result = new Source() { Description = Name };
                result.SourceId = mapper.Insert(result);
            }

            return result;
        }

        protected virtual DetailJob GetDetailledInfo(String input)
        {
            throw new NotSupportedException(); 
        }


        /// <summary>
        /// Url Getter Setter
        /// </summary>
        public string Url { protected get; set; }
        
        /// <summary>
        /// CurrentPage Getter Setter
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Pages Getter Setter
        /// </summary>
        public int Pages { get; set; }

        /// <summary>
        /// CrawlerData Getter Setter
        /// </summary>
        public string CrawlerData { get; set; }

        /// <summary>
        /// Job List Getter Setter
        /// </summary>
        public List<Job> Jobs { get; set; }

        /// <summary>
        /// Cv List Getter Setter
        /// </summary>
        public List<Cv> Cvs { get; set; }

        /// <summary>
        /// DetailJob Getter Setter
        /// </summary>
        public List<DetailJob> DetailJobs { get; set; }

        /// <summary>
        /// Crawler Name Getter Setter
        /// </summary>
        public String Name { get; set; }
    }
}
