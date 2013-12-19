using System;
using DataAccessObjects;

namespace CrawlerBatch.Mappers
{
    public class CvMapper : AMapper<Cv, MatchyBackEnd.Cv>
    {
        public override int Insert(Cv obj)
        {
            int insertId = MatchyBackend.AddCV(MapToService(obj));

            if (insertId == 0)
                Console.WriteLine("Cv niet kunnen toevoegen");
            else
                Console.WriteLine(insertId);

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
                           Education = cv.Education,
                           EducationLevel = eduMapper.MapToService(cv.EducationLevel),
                           Hours = cv.Hours,
                           Profession = cv.Profession,
                           Province = cv.Province,
                           Sex = cv.Sex,
                           Title = cv.Title,
                           WorkExperience = cv.WorkExperience,
                           Source = sourceMapper.MapToService(cv.Source)
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
                Education = cv.Education,
                EducationLevel = eduMapper.MapFromService(cv.EducationLevel),
                Hours = cv.Hours,
                Profession = cv.Profession,
                Province = cv.Province,
                Sex = cv.Sex,
                Title = cv.Title,
                WorkExperience = cv.WorkExperience,
                Source = sourceMapper.MapFromService(cv.Source)
            };
        }
    }
}
