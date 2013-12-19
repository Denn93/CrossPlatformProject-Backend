using System;
using DataAccessObjects;

namespace CrawlerBatch.Mappers
{
    public class EducationMapper : AMapper<Education, MatchyBackEnd.Education>
    {
        public override int Insert(Education obj)
        {
            int insertId = MatchyBackend.AddEdu(MapToService(obj));

            if (insertId == 0)
                Console.WriteLine("Education not inserted");

            return insertId;
        }

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

        public override MatchyBackEnd.Education MapToService(Education education)
        {
            var result = new MatchyBackEnd.Education { EducationId = education.EducationId, Name = education.Name };

            return result;
        }

        public override Education MapFromService(MatchyBackEnd.Education education)
        {
            var result = new Education {EducationId = education.EducationId, Name = education.Name};

            return result;
        }
    }
}
