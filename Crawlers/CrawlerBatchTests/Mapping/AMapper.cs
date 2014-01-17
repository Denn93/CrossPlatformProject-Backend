using System;
using CrawlerBatch.MatchyBackEnd;

namespace CrawlerBatchTests.Mapping
{
    public abstract class AMapper<TDao, TServicedao>
    {
        /// <summary>
        /// Deze methode zorgt ervoor dat de data vanuit deze crawler omgezet kan worden naar de datatypes van de backEnd. 
        /// Dus een handmatige serializer. Omdat we een ingebouwde serializer niet werkend kregen. Terwijl de datatypes exact 
        /// overeenkwamen vanwege import library met gebruikte dataobjecten
        /// </summary>
        /// <param name="obj">Het data object dat omgezet moet worden</param>
        /// <returns>Het data object van de backEnd</returns>
        public abstract TServicedao MapToService(TDao obj);

        /// <summary>
        /// Deze methode zorgt ervoor dat de data vanuit de backend omgezet kan worden naar de datatypes van deze crawler. 
        /// Dus een handmatige serializer. Omdat we een ingebouwde serializer niet werkend kregen. Terwijl de datatypes exact 
        /// overeenkwamen vanwege import library met gebruikte dataobjecten
        /// </summary>
        /// <param name="obj">Het data object dat omgezet moet worden</param>
        /// <returns>Het data object van deze crawler</returns>
        public abstract TDao MapFromService(TServicedao obj);

    }
}
