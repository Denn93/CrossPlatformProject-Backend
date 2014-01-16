using System;
using System.Net;
using DataAccessObjects;

namespace CrawlerBatch.Mappers
{
    /// <summary>
    /// SourceMapper class. Inherits van AMapper class. Met Generic Source.cs en MatchyBackEnd.Source.cs
    /// </summary>
    public class SourceMapper : AMapper<Source, MatchyBackEnd.Source>
    {
        /// <summary>
        /// Insert Methode. Deze methode zorgt ervoor dat de gecrawlde data naar de webservice gestuurd kan worden voor insert
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Inserted id</returns>
        public override int Insert(Source source)
        {
            int insertId = 0;
            try
            {
                insertId = MatchyBackend.AddSource(MapToService(source));
            }
            catch (WebException ex)
            {
                Console.WriteLine("BackEnd time out. Reconnecting.....: " + ex.Message);
            }

            if (insertId == 0)
                Console.WriteLine("Source not added!!");

            return insertId;
        }

        /// <summary>
        /// Deze methode haalt data op. Op basis van TDao Generic wordt er gekeken welke return type er moet komen en werlke data er opgehaald moet worden
        /// </summary>
        /// <param name="id">Optionele id veld. Wanneer 0 wordt alles opgehaald uit de webservice</param>
        /// <returns>Array met data. Van datatype TDao Generic</returns>
        public override Source[] Get(int id = 0)
        {
            MatchyBackEnd.Source[] sources = MatchyBackend.GetSource(0);

            Source[] resultlist = new Source[sources.Length];

            int i = 0;
            foreach (var source in sources)
            {
                Source result = MapFromService(source);

                resultlist[i] = result;
                i++;
            }

            return resultlist;
        }

        /// <summary>
        /// Deze methode zorgt ervoor dat de data vanuit deze crawler omgezet kan worden naar de datatypes van de backEnd. 
        /// Dus een handmatige serializer. Omdat we een ingebouwde serializer niet werkend kregen. Terwijl de datatypes exact 
        /// overeenkwamen vanwege import library met gebruikte dataobjecten
        /// </summary>
        /// <param name="source">Het data object dat omgezet moet worden</param>
        /// <returns>Het data object van de backEnd</returns>
        public override MatchyBackEnd.Source MapToService(Source source)
        {
            return new MatchyBackEnd.Source() { SourceId = source.SourceId, Description = source.Description };
        }

        /// <summary>
        /// Deze methode zorgt ervoor dat de data vanuit de backend omgezet kan worden naar de datatypes van deze crawler. 
        /// Dus een handmatige serializer. Omdat we een ingebouwde serializer niet werkend kregen. Terwijl de datatypes exact 
        /// overeenkwamen vanwege import library met gebruikte dataobjecten
        /// </summary>
        /// <param name="source">Het data object dat omgezet moet worden</param>
        /// <returns>Het data object van deze crawler</returns>
        public override Source MapFromService(MatchyBackEnd.Source source)
        {
            return new Source() {SourceId = source.SourceId, Description = source.Description};
        }
    }
}
