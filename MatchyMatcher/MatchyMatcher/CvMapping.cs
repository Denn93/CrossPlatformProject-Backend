using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccessObjects;

/// <summary>
/// Summary description for CVMapping
/// </summary>
public class CvMapping
{
    public MatchyMatcher.MatchyBackend.Cv mapToService(Cv cv)
    {
        var eduMapper = new EducationMapper();
        var sourceMapper = new SourceMapper();

        MatchyMatcher.MatchyBackend.Cv result = new MatchyMatcher.MatchyBackend.Cv();

        if (cv != null)
        {
            return new MatchyMatcher.MatchyBackend.Cv()
            {
                Name = cv.Name,
                CvID = cv.CvID,
                Age = cv.Age,
                Sex = cv.Sex,
                Interests = cv.Interests,
                Personal = cv.Personal,
                City = cv.City,
                //Date = cv.Date,
                Discipline = cv.Discipline,
                EducationHistory = cv.EducationHistory,
                EducationLevel = eduMapper.MapToService(cv.EducationLevel),
                Hours = cv.Hours,
                Profession = cv.Profession,
                Province = cv.Province,
                //Title = cv.Title,
                WorkExperience = cv.WorkExperience,
                Source = sourceMapper.MapToService(cv.Source)
            };
        }
        else
            return result;
    }

    public Cv mapFromService(MatchyMatcher.MatchyBackend.Cv cv)
    {
        var eduMapper = new EducationMapper();
        var sourceMapper = new SourceMapper();

        if (cv.CvID != 0)
        {
            return new Cv()
            {
                Name = cv.Name,
                CvID = cv.CvID,
                Age = cv.Age,
                Sex = cv.Sex,
                Interests = cv.Interests,
                Personal = cv.Personal,
                City = cv.City,
                //Date = cv.Date,
                Discipline = cv.Discipline,
                EducationHistory = cv.EducationHistory,
                EducationLevel = eduMapper.MapFromService(cv.EducationLevel),
                Hours = cv.Hours,
                Profession = cv.Profession,
                Province = cv.Province,
                // Title = cv.Title,
                WorkExperience = cv.WorkExperience,
                Source = sourceMapper.MapFromService(cv.Source)
            };
        }
        else
        {
            Cv result = new Cv();
            result.EducationLevel = eduMapper.MapFromService(cv.EducationLevel);
            result.Source = sourceMapper.MapFromService(cv.Source);
            return result;
        }

    }
}