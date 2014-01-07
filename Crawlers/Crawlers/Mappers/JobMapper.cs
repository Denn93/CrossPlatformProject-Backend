using System;
using System.Net;
using DataAccessObjects;

namespace CrawlerBatch.Mappers
{
    public class JobMapper : AMapper<Job, MatchyBackEnd.Job>
    {
        public override int Insert(Job obj)
        {
            int insertId = 0;

            try
            {
                insertId = MatchyBackend.AddJob(MapToService(obj));

                if (insertId != 0)
                    Console.WriteLine(insertId.ToString());
            }
            catch (WebException ex)
            {
                Console.WriteLine("Er is iets mis met de vacature");
            }
            
            return insertId;
        }

        public override Job[] Get(int id = 0)
        {
            throw new NotImplementedException();
        }

        public override MatchyBackEnd.Job MapToService(Job job)
        {
            var companyMapper = new CompanyMapper();
            var educationMapper = new EducationMapper();
            var detailJobMapper = new DetailJobMapper();
            var sourceMapper = new SourceMapper();

            return new MatchyBackEnd.Job()
                       {
                            JobID = job.JobID,
                            Company = companyMapper.MapToService(job.Company),
                            JobDescription = job.JobDescription,
                            Education = educationMapper.MapToService(job.Education),
                            CrawlerID = job.CrawlerID,
                            DetailJob = detailJobMapper.MapToService(job.DetailJob),
                            JobHours = job.JobHours,
                            JobPlaceDate = job.JobPlaceDate,
                            JobTitle = job.JobTitle,
                            Source = sourceMapper.MapToService(job.Source)
                       };
        }

        public override Job MapFromService(MatchyBackEnd.Job job)
        {
            var companyMapper = new CompanyMapper();
            var educationMapper = new EducationMapper();
            var detailJobMapper = new DetailJobMapper();
            var sourceMapper = new SourceMapper();

            return new Job()
                        {
                            JobID = job.JobID,
                            Company = companyMapper.MapFromService(job.Company),
                            JobDescription = job.JobDescription,
                            Education = educationMapper.MapFromService(job.Education),
                            CrawlerID = job.CrawlerID,
                            DetailJob = detailJobMapper.MapFromService(job.DetailJob),
                            JobHours = job.JobHours,
                            JobPlaceDate = job.JobPlaceDate,
                            JobTitle = job.JobTitle,
                            Source = sourceMapper.MapFromService(job.Source)
                        };
        }
    }
}
