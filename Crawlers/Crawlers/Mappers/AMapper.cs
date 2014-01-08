using System;
using CrawlerBatch.MatchyBackEnd;

namespace CrawlerBatch.Mappers
{
    public abstract class AMapper<TDao, TServicedao>
    {
        protected MatchyService MatchyBackend;

        protected AMapper()
        {
            MatchyBackend = new MatchyService();
        }

        /// <summary>
        /// Insert Methode. Deze methode zorgt ervoor dat de gecrawlde data naar de webservice gestuurd kan worden voor insert
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Inserted id</returns>
        public virtual int Insert(TDao obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deze methode haalt data op. Op basis van TDao Generic wordt er gekeken welke return type er moet komen en werlke data er opgehaald moet worden
        /// </summary>
        /// <param name="id">Optionele id veld. Wanneer 0 wordt alles opgehaald uit de webservice</param>
        /// <returns>Array met data. Van datatype TDao Generic</returns>
        public virtual TDao[] Get(int id = 0)
        {
            throw new NotImplementedException();
        }

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
