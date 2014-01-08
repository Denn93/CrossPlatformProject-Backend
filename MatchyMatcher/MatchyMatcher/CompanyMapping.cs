using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccessObjects;

/// <summary>
/// Summary description for CompanyMapping
/// </summary>
public class CompanyMapping
{
    public MatchyMatcher.MatchyBackend.Company MapToService(Company company)
    {
        MatchyMatcher.MatchyBackend.Company result = new MatchyMatcher.MatchyBackend.Company();

        if (company != null)
        {
            return new MatchyMatcher.MatchyBackend.Company()
            {
                CompanyCity = company.CompanyCity,
                CompanyDate = company.CompanyDate,
                CompanyDescription = company.CompanyDescription,
                CompanyEmail = company.CompanyEmail,
                CompanyTel = company.CompanyTel,
                CompanyID = company.CompanyID,
                CompanyName = company.CompanyName
            };
        }   
        else
          return result;
    }

    public Company MapFromService(MatchyMatcher.MatchyBackend.Company company)
    {
        Company result = new Company();

        if (company.CompanyID != 0)
        {
            return new Company()
            {
                CompanyCity = company.CompanyCity,
                CompanyDate = company.CompanyDate,
                CompanyDescription = company.CompanyDescription,
                CompanyEmail = company.CompanyEmail,
                CompanyTel = company.CompanyTel,
                CompanyID = company.CompanyID,
                CompanyName = company.CompanyName
            };
        }
        else
            return result;
    }

}