using System;
using System.Net;
using CrawlerBatchTests.Mapping;
using DataAccessObjects;
using CrawlerBatch;

namespace CrawlerBatchTests.Mapping
{
    /// <summary>
    /// SourceMapper class. Inherits van AMapper class. Met Generic Source.cs en MatchyBackEnd.Source.cs
    /// </summary>
    public class SourceMapper : AMapper<Source, MatchyBackEnd.Source>
    {
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
