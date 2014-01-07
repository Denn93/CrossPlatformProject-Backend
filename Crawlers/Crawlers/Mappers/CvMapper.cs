using System;
using System.Collections.Generic;
using System.Net;
using DataAccessObjects;

namespace CrawlerBatch.Mappers
{
    public class CvMapper : AMapper<Cv, MatchyBackEnd.Cv>
    {
        public override int Insert(Cv obj)
        {
            int insertId = 0;
            try
            {
                insertId = MatchyBackend.AddCv(MapToService(obj));
               
                if (insertId != 0)
                    Console.WriteLine(insertId);
            }
            catch (WebException ex)
            {
                Console.WriteLine("er is een fout met deze Cv. Kan niet toegevoegd worden. ");
            }

            return insertId;

        }

        public override Cv[] Get(int id = 0)
        {
            throw new NotImplementedException();
        }

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
