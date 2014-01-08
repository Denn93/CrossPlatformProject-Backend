using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Job = DataAccessObjects.Job;

/// <summary>
/// Summary description for JobMapping
/// </summary>
public class JobMapping
{
    public MatchyMatcher.MatchyBackend.Job mapToService(Job job)
    {
        CompanyMapping companyMapping = new CompanyMapping();
        EducationMapper educationMapping = new EducationMapper();
        DetailJobMapper detailMapping = new DetailJobMapper();

        MatchyMatcher.MatchyBackend.Job backEndUser = new MatchyMatcher.MatchyBackend.Job()
        {
            JobID = job.JobID,
            Company = companyMapping.MapToService(job.Company),
            Education = educationMapping.MapToService(job.Education),
            DetailJob = detailMapping.MapToService(job.DetailJob),
            JobTitle = job.JobTitle,
            JobDescription = job.JobDescription,
            JobPlaceDate = job.JobPlaceDate,
            JobHours = job.JobHours
        };


        return backEndUser;
    }

    public Job mapFromService(MatchyMatcher.MatchyBackend.Job job)
    {
        CompanyMapping companyMapping = new CompanyMapping();
        EducationMapper educationMapping = new EducationMapper();
        DetailJobMapper detailMapping = new DetailJobMapper();

        return new Job()
        {
            JobID = job.JobID,
            Company = companyMapping.MapFromService(job.Company),
            Education = educationMapping.MapFromService(job.Education),
            DetailJob = detailMapping.MapFromService(job.DetailJob),
            JobTitle = job.JobTitle,
            JobDescription = job.JobDescription,
            JobPlaceDate = job.JobPlaceDate,
            JobHours = job.JobHours
        };
    }
}