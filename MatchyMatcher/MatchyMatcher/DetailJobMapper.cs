using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DetailJob = DataAccessObjects.DetailJob;

/// <summary>
/// Summary description for DetailJobMapper
/// </summary>
public class DetailJobMapper
{
    public MatchyMatcher.MatchyBackend.DetailJob MapToService(DetailJob detailJob)
    {
        MatchyMatcher.MatchyBackend.DetailJob backEndDetailJob = new MatchyMatcher.MatchyBackend.DetailJob()
        {
            DetailJobId = detailJob.DetailJobId,
            JobId = detailJob.JobId,
            Data = detailJob.Data
        };


        return backEndDetailJob;
    }

    public DetailJob MapFromService(MatchyMatcher.MatchyBackend.DetailJob detailJob)
    {
        return new DetailJob()
        {
            DetailJobId = detailJob.DetailJobId,
            JobId = detailJob.JobId,
            Data = detailJob.Data
        };
    }
}