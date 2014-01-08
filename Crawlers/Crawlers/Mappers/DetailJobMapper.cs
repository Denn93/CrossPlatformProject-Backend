using System;
using DataAccessObjects;

namespace CrawlerBatch.Mappers
{
    /// <summary>
    /// DetailJobMapper class. Inherits van AMapper class. Met Generic DetailJob.cs en MatchyBackEnd.DetailJob.cs
    /// </summary>
    public class DetailJobMapper : AMapper<DetailJob, MatchyBackEnd.DetailJob>
    {
        /// <summary>
        /// Insert Methode. Deze methode zorgt ervoor dat de gecrawlde data naar de webservice gestuurd kan worden voor insert
        /// </summary>
        /// <param name="detailJob"></param>
        /// <returns>Inserted id</returns>
        public override int Insert(DetailJob detailJob)
        {
            int insertId = MatchyBackend.AddDetailJob(MapToService(detailJob));

            if (insertId == 0)
                Console.WriteLine("DetailJob not inserted. Error, Error");

            return insertId;
        }

        /// <summary>
        /// Deze methode zorgt ervoor dat de data vanuit deze crawler omgezet kan worden naar de datatypes van de backEnd. 
        /// Dus een handmatige serializer. Omdat we een ingebouwde serializer niet werkend kregen. Terwijl de datatypes exact 
        /// overeenkwamen vanwege import library met gebruikte dataobjecten
        /// </summary>
        /// <param name="detailJob">Het data object dat omgezet moet worden</param>
        /// <returns>Het data object van de backEnd</returns>
        public override MatchyBackEnd.DetailJob MapToService(DetailJob detailJob)
        {
            return new MatchyBackEnd.DetailJob() { Data = detailJob.Data, DetailJobId = detailJob.DetailJobId, JobId = detailJob.JobId };
        }

        /// <summary>
        /// Deze methode zorgt ervoor dat de data vanuit de backend omgezet kan worden naar de datatypes van deze crawler. 
        /// Dus een handmatige serializer. Omdat we een ingebouwde serializer niet werkend kregen. Terwijl de datatypes exact 
        /// overeenkwamen vanwege import library met gebruikte dataobjecten
        /// </summary>
        /// <param name="detailJob">Het data object dat omgezet moet worden</param>
        /// <returns>Het data object van deze crawler</returns>
        public override DetailJob MapFromService(MatchyBackEnd.DetailJob detailJob)
        {
            return new DetailJob() {Data = detailJob.Data, DetailJobId = detailJob.DetailJobId, JobId = detailJob.JobId};
        }
    }
}
