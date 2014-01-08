using System;
using DataAccessObjects;

namespace CrawlerBatch.Mappers
{
    /// <summary>
    /// EducationMapper class. Inherits van AMapper class. Met Generic Education.cs en MatchyBackEnd.Education.cs
    /// </summary>
    public class EducationMapper : AMapper<Education, MatchyBackEnd.Education>
    {
        /// <summary>
        /// Insert Methode. Deze methode zorgt ervoor dat de gecrawlde data naar de webservice gestuurd kan worden voor insert
        /// </summary>
        /// <param name="education"></param>
        /// <returns>Inserted id</returns>
        public override int Insert(Education education)
        {
            int insertId = MatchyBackend.AddEdu(MapToService(education));

            if (insertId == 0)
                Console.WriteLine("Education not inserted");

            return insertId;
        }

        /// <summary>
        /// Deze methode haalt data op. Op basis van TDao Generic wordt er gekeken welke return type er moet komen en werlke data er opgehaald moet worden
        /// </summary>
        /// <param name="id">Optionele id veld. Wanneer 0 wordt alles opgehaald uit de webservice</param>
        /// <returns>Array met data. Van datatype TDao Generic</returns>
        public override Education[] Get(int id = 0)
        {
            MatchyBackEnd.Education[] educations = MatchyBackend.GetEdu(id);

            var result = new Education[educations.Length];

            var i = 0;
            foreach (var education in educations)
            {
                result[i] = MapFromService(education);
                i++;
            }

            return result;
        }

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
