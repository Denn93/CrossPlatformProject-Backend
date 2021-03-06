﻿using System;
using System.Net;
using CrawlerBatchTests.Mapping;
using DataAccessObjects;

namespace CrawlerBatch.Mapping
{
    /// <summary>
    /// JobMapper class. Inherits van AMapper class. Met Generic Job.cs en MatchyBackEnd.Job.cs
    /// </summary>
    public class JobMapper : AMapper<Job, MatchyBackEnd.Job>
    {
        /// <summary>
        /// Deze methode zorgt ervoor dat de data vanuit deze crawler omgezet kan worden naar de datatypes van de backEnd. 
        /// Dus een handmatige serializer. Omdat we een ingebouwde serializer niet werkend kregen. Terwijl de datatypes exact 
        /// overeenkwamen vanwege import library met gebruikte dataobjecten
        /// </summary>
        /// <param name="job">Het data object dat omgezet moet worden</param>
        /// <returns>Het data object van de backEnd</returns>
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

        /// <summary>
        /// Deze methode zorgt ervoor dat de data vanuit de backend omgezet kan worden naar de datatypes van deze crawler. 
        /// Dus een handmatige serializer. Omdat we een ingebouwde serializer niet werkend kregen. Terwijl de datatypes exact 
        /// overeenkwamen vanwege import library met gebruikte dataobjecten
        /// </summary>
        /// <param name="job">Het data object dat omgezet moet worden</param>
        /// <returns>Het data object van deze crawler</returns>
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
