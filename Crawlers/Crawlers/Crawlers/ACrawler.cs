using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using CrawlerBatch.Mappers;
using DataAccessObjects;

namespace CrawlerBatch.Crawlers
{
    public abstract class ACrawler
    {
        protected const string CrawlerStarted = "Crawler {0} has started";
        protected const string CrawlerCompleted = "Crawler {0} has been completed";

        protected const string StringValueNotFound = "Niet Beschikbaar";
        protected const string IntValueNotFound = "0";

        private const string TimeOutException = "Connection Time out: ";

        /// <summary>
        /// Protected constructor initialiseerd de List met vacatures 
        /// </summary>
        protected ACrawler()
        {
            Jobs = new List<Job>();
            Cvs = new List<Cv>();
            DetailJobs = new List<DetailJob>();
        }
        /// <summary>
        /// Template Method. Deze methode voert alle acties in een bepaalde volgorde uit. Zodat bij elke crawler de volgorde van uitvoer gelijk is. 
        /// </summary>
        public virtual void Template()
        {
            Console.WriteLine(CrawlerStarted, Name);

            Process();

            Console.WriteLine(CrawlerCompleted, Name);
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
        /// <returns>Een response String van de opgehaalde pagina</returns>
        protected virtual String urlHandler(String url = null)
        {
            String result = "";
            try
            {
                WebRequest request = WebRequest.Create(url ?? Url);
                request.Timeout = 5000;
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();

                StreamReader reader = new StreamReader(responseStream);
                result = reader.ReadToEnd();
            }
            catch (WebException ex)
            {
                Console.WriteLine(TimeOutException + ex.Message);
                result = urlHandler(url);
            }

            return WebUtility.HtmlDecode(result);
        }

        /// <summary>
        /// Deze methode zorgt ervoor dat een bepaalde pattern in een stuk tekst wordt opgezogt. 
        /// </summary>
        /// <param name="pattern">De pattern die aangeleverd moet worden</param>
        /// <param name="data">De data waarin gezogd moet worden. Wanneer leeg zal CrawlerData gepakt worden</param>
        /// <param name="isInt">Standaard false. Wanneer true zal de methode wanneer geen resultaat, 0 ipv Niet beschikbaar terug sturen</param>
        /// <param name="groupId">/Wanneer een pattern meerdere groups bevat kan deze waarde mee gestuurd worden. Standaard 1. De eerste groep</param>
        /// <returns>Het resultaat van de gezochte pattern in de data</returns>
        protected virtual String GetCrawlerData(String pattern, String data = null, Boolean isInt = false, int groupId = 1)
        {
            if (data == null)
                data = CrawlerData;

            var resultRegex = Regex.Matches(data, pattern, RegexOptions.Singleline);
            String resultString;

            if (!isInt)
                resultString = (resultRegex.Count == 0 || resultRegex[0].Groups[groupId].Value.Trim().Equals("Â") || resultRegex[0].Groups[groupId].Value.Trim().Equals("-") || resultRegex[0].Groups[groupId].Value.Trim().Equals("")) ? StringValueNotFound : resultRegex[0].Groups[groupId].Value;
            else
                resultString = (resultRegex.Count == 0 || resultRegex[0].Groups[groupId].Value.Trim().Equals("Â") || resultRegex[0].Groups[groupId].Value.Trim().Equals("-") || resultRegex[0].Groups[groupId].Value.Trim().Equals("")) ? IntValueNotFound : resultRegex[0].Groups[groupId].Value;

            return resultString;
        }

        /// <summary>
        /// Deze methode wordt gebruikt om te bepalen wat de Education is van een bepaalde string. Er wordt gekeken in de database en vanuit
        /// daar wordt er bepaald wat het educationID is. Bestaat hij niet dan zal hij deze toevoegen in database en een id terugsturen
        /// </summary>
        /// <param name="input">De string die naar education gezet moet worden</param>
        /// <returns>Een Education Object. Met educationID en description</returns>
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

        /// <summary>
        /// Deze methode wordt gebruikt om te bepalen wat de Source is van een bepaalde string. Er wordt gekeken in de database en vanuit
        /// daar wordt er bepaald wat het sourceID is. Bestaat hij niet dan zal hij deze toevoegen in database en een id terugsturen
        /// </summary>
        /// <returns>Een Source Object. Met sourceID en description</returns>
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

        /// <summary>
        /// Deze methode zal de vacature helemaal openen. En vervolgens de data die hier in staat crawlen en verwerken. 
        /// Deze methode throws een NotSupportedExceptio zodat een crawler die deze methode niet overrided hem ook niet kan gebruiken. 
        /// Wanneer deze abstract is moet hij geimplementeerd worden en dat is nu niet het geval.
        /// </summary>
        /// <param name="input"><De input data waarin gecrawled moet gaan worden/param>
        /// <param name="company">Het company object die mede hier ook gevuld wordt. </param>
        /// <returns>Deze methode returned vervolgens een DetailJob</returns>
        protected virtual DetailJob GetDetailledInfo(String input, Company company)
        {
            throw new NotSupportedException(); 
        }

        /// <summary>
        /// Handmatige converter van String naar een DataTime object
        /// </summary>
        /// <param name="date">Bevat de datum die geconverteerd moet worden</param>
        /// <returns>Een DateTime object met de datum uit String date</returns>
        protected virtual DateTime StringToDateTime(String date)
        {
            var dateInfo = date.Split('-');

            var day = Convert.ToInt32(dateInfo[0]);
            var month = Convert.ToInt32(dateInfo[1]);
            var year = Convert.ToInt32(dateInfo[2]);

            return new DateTime(year, month, day);
        }

        #region Properties

        /// <summary>
        /// Url Getter Setter
        /// </summary>
        public string Url { get; set; }
        
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

        public Boolean HasStarted { get; set; }

        #endregion
    }
}
