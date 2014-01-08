using System;
using System.Collections.Generic;
using System.Net;
using DataAccessObjects;

namespace CrawlerBatch.Mappers
{
    /// <summary>
    /// CvMapper class. Inherits van AMapper class. Met Generic Cv.cs en MatchyBackEnd.Cv.cs
    /// </summary>
    public class CvMapper : AMapper<Cv, MatchyBackEnd.Cv>
    {
        /// <summary>
        /// Insert Methode. Deze methode zorgt ervoor dat de gecrawlde data naar de webservice gestuurd kan worden voor insert
        /// </summary>
        /// <param name="cv"></param>
        /// <returns>Inserted id</returns>
        public override int Insert(Cv cv)
        {
            int insertId = 0;
            try
            {
                insertId = MatchyBackend.AddCv(MapToService(cv));
               
                if (insertId != 0)
                    Console.WriteLine(insertId);
            }
            catch (WebException ex)
            {
                Console.WriteLine("er is een fout met deze Cv. Kan niet toegevoegd worden. ");
            }

            return insertId;

        }

        /// <summary>
        /// Deze methode zorgt ervoor dat de data vanuit deze crawler omgezet kan worden naar de datatypes van de backEnd. 
        /// Dus een handmatige serializer. Omdat we een ingebouwde serializer niet werkend kregen. Terwijl de datatypes exact 
        /// overeenkwamen vanwege import library met gebruikte dataobjecten
        /// </summary>
        /// <param name="cv">Het data object dat omgezet moet worden</param>
        /// <returns>Het data object van de backEnd</returns>
        public override MatchyBackEnd.Cv MapToService(Cv cv)
        {
            var eduMapper = new EducationMapper();
            var sourceMapper = new SourceMapper();

            return new MatchyBackEnd.Cv()
            {
                           CvID = cv.CvID,
                           Age = cv.Age,
                           City = cv.City,
                           Date = cv.Date,
                           Discipline = cv.Discipline,
                           EducationHistory = cv.EducationHistory,
                           EducationLevel = eduMapper.MapToService(cv.EducationLevel),
                           Hours = cv.Hours,
                           Profession = cv.Profession,
                           Province = cv.Province,
                           Sex = cv.Sex,
                           WorkExperience = cv.WorkExperience,
                           Source = sourceMapper.MapToService(cv.Source),
                           Personal = cv.Personal,
                           Interests = cv.Interests,
                           JobRequirements = cv.JobRequirements,
                           CrawlerId = cv.CrawlerId
                       };
        }

        /// <summary>
        /// Deze methode zorgt ervoor dat de data vanuit de backend omgezet kan worden naar de datatypes van deze crawler. 
        /// Dus een handmatige serializer. Omdat we een ingebouwde serializer niet werkend kregen. Terwijl de datatypes exact 
        /// overeenkwamen vanwege import library met gebruikte dataobjecten
        /// </summary>
        /// <param name="cv">Het data object dat omgezet moet worden</param>
        /// <returns>Het data object van deze crawler</returns>
        public override Cv MapFromService(MatchyBackEnd.Cv cv)
        {
            var eduMapper = new EducationMapper();
            var sourceMapper = new SourceMapper();

            return new Cv()
            {
                CvID = cv.CvID,
                Age = cv.Age,
                City = cv.City,
                Date = cv.Date,
                Discipline = cv.Discipline,
                EducationHistory = cv.EducationHistory,
                EducationLevel = eduMapper.MapFromService(cv.EducationLevel),
                Hours = cv.Hours,
                Profession = cv.Profession,
                Province = cv.Province,
                Sex = cv.Sex,
                WorkExperience = cv.WorkExperience,
                Source = sourceMapper.MapFromService(cv.Source),
                Personal = cv.Personal,
                Interests = cv.Interests,
                JobRequirements = cv.JobRequirements,
                CrawlerId = cv.CrawlerId
            };
        }
    }
}
