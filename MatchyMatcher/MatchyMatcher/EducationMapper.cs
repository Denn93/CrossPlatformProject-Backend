using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccessObjects;

/// <summary>
/// Summary description for EducationMapper
/// </summary>
public class EducationMapper
{
    public MatchyMatcher.MatchyBackend.Education MapToService(Education education)
    {

        MatchyMatcher.MatchyBackend.Education result = new MatchyMatcher.MatchyBackend.Education();

        if (education != null)
        {
            return new MatchyMatcher.MatchyBackend.Education { EducationId = education.EducationId, Name = education.Name };
        }
        else
            return result;
    }

    public Education MapFromService(MatchyMatcher.MatchyBackend.Education education)
    {
        var result = new Education { EducationId = education.EducationId, Name = education.Name };

        return result;
    }
}