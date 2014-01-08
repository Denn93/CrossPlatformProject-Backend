using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Match = DataAccessObjects.Match;

/// <summary>
/// Summary description for DetailJobMapper
/// </summary>
public class MatchyMapper
{
    public MatchyMatcher.MatchyBackend.Match MapToService(Match match)
    {

        JobMapping jobmap = new JobMapping();
        CvMapping cvmap = new CvMapping();

        MatchyMatcher.MatchyBackend.Match backEndMatch = new MatchyMatcher.MatchyBackend.Match()
        {

            Cv = cvmap.mapToService(match.Cv),
            Job = jobmap.mapToService(match.Job),
            Score = match.Score

        };


        return backEndMatch;
    }

    public Match MapFromService(MatchyMatcher.MatchyBackend.Match match)
    {

        JobMapping jobmap = new JobMapping();
        CvMapping cvmap = new CvMapping();

        return new Match()
        {

            Cv = cvmap.mapFromService(match.Cv),
            Job = jobmap.mapFromService(match.Job),
            Score = match.Score

        };
    }
}