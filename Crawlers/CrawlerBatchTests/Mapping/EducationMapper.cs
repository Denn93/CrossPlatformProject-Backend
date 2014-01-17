using System;
using System.Net;
using CrawlerBatchTests.Mapping;
using DataAccessObjects;

namespace CrawlerBatchTests.Mapping
{
    /// <summary>
    /// EducationMapper class. Inherits van AMapper class. Met Generic Education.cs en MatchyBackEnd.Education.cs
    /// </summary>
    public class EducationMapper : AMapper<Education, MatchyBackEnd.Education>
    {
        /// <summary>
        /// Deze methode zorgt ervoor dat de data vanuit deze crawler omgezet kan worden naar de datatypes van de backEnd. 
        /// Dus een handmatige serializer. Omdat we een ingebouwde serializer niet werkend kregen. Terwijl de datatypes exact 
        /// overeenkwamen vanwege import library met gebruikte dataobjecten
        /// </summary>
        /// <param name="education">Het data object dat omgezet moet worden</param>
        /// <returns>Het data object van de backEnd</returns>
        public override MatchyBackEnd.Education MapToService(Education education)
        {
            var result = new MatchyBackEnd.Education { EducationId = education.EducationId, Name = education.Name };

            return result;
        }

        /// <summary>
        /// Deze methode zorgt ervoor dat de data vanuit de backend omgezet kan worden naar de datatypes van deze crawler. 
        /// Dus een handmatige serializer. Omdat we een ingebouwde serializer niet werkend kregen. Terwijl de datatypes exact 
        /// overeenkwamen vanwege import library met gebruikte dataobjecten
        /// </summary>
        /// <param name="education">Het data object dat omgezet moet worden</param>
        /// <returns>Het data object van deze crawler</returns>
        public override Education MapFromService(MatchyBackEnd.Education education)
        {
            var result = new Education {EducationId = education.EducationId, Name = education.Name};

            return result;
        }
    }
}
