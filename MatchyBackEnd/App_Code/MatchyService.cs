﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;
using DataAccessObjects;
using Database;
using Features;
using Processes;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://145.24.222.183/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class MatchyService : System.Web.Services.WebService
{
    [WebMethod]
    public int AddUser(User user)
    {
        return new UserProcess().Add(user);
    }

    [WebMethod]
    public User[] GetUser(int id)
    {
        return new UserProcess().Get(id);
    }

    [WebMethod]
    public int AddJob(Job job)
    {
        return new JobProcess().Add(job);
    }

    [WebMethod]
    public Job[] GetJob(int ID)
    {
        return new JobProcess().Get(ID);
    }

    [WebMethod]
    public Job[] GetJobByLimit(int begin)
    {
        return new JobProcess().Get(0, null, new KeyValuePair<string, string>(), String.Format("LIMIT {0},{1}", begin, 10));
    }

    [WebMethod]
    public Job[] GetJobByCompany(int companyID)
    {
        var where = new List<KeyValuePair<string, string>>();
        where.Add(new KeyValuePair<string, string>("companyID", companyID.ToString()));

        return new JobProcess().Get(0, where);
    }

    [WebMethod]
    public int AddCv(Cv cv)
    {
        return new CvProcess().Add(cv);
    }

    [WebMethod]
    public Cv[] GetCv(int id)
    {
        return new CvProcess().Get(id);
    }

    [WebMethod]
    public Cv[] GetCvByLimit(int begin)
    {
        return new CvProcess().Get(0, null, new KeyValuePair<string, string>(), String.Format("LIMIT {0},{1}", begin, 10));
    }


    [WebMethod]
    public int AddMatch(Match match)
    {
        return new MatchesProcess().Add(match);
    }

    [WebMethod]
    public Match[] GetMatch(int id)
    {
        return new MatchesProcess().Get(id);
    }

    [WebMethod]
    public Match[] GetMatchByCv(int cv_id, int limit)
    {
        var where = new List<KeyValuePair<string, string>>();
        where.Add(new KeyValuePair<string, string>("cv_ID", cv_id.ToString()));
        where.Add(new KeyValuePair<string, string>("score", "1"));

        var sqlOperator = new KeyValuePair<String, String>("score", ">");

        return new MatchesProcess().Get(0, where, sqlOperator, "ORDER BY score DESC, date Limit " + limit);
    }

    [WebMethod]
    public Match[] GetMatchByCompany(int companyID, int limit)
    {
        var whereCompanyID = new List<KeyValuePair<string, string>>();
        whereCompanyID.Add(new KeyValuePair<string, string>("companyID", companyID.ToString()));

        var jobID = new JobProcess().Get(0, whereCompanyID).Length == 0 ? -1 : new JobProcess().Get(0, whereCompanyID)[0].JobID;
       
        var whereJobID = new List<KeyValuePair<string, string>>();
        whereJobID.Add(new KeyValuePair<string, string>("job_ID", jobID.ToString()));
        whereJobID.Add(new KeyValuePair<string, string>("score", "1"));

        var sqlOperator = new KeyValuePair<String, String>("score", ">");


        return new MatchesProcess().Get(0, whereJobID, sqlOperator, "ORDER BY score DESC, date LIMIT " + limit);
    }

    [WebMethod]
    public Education[] GetEdu(int id)
    {
        return new EducationProcess().Get(id);
    }

    [WebMethod]
    public int AddEdu(Education education)
    {
        return new EducationProcess().Add(education);
    }

    [WebMethod]
    public Source[] GetSource(int id)
    {
        return new SourceProcess().Get(id);
    }

    [WebMethod]
    public int AddSource(Source source)
    {
        return new SourceProcess().Add(source);
    }

    [WebMethod]
    public Company[] GetCompany(int ID)
    {
        return new CompanyProcess().Get(ID);
    }

    //date moet jaar-maand-dag uur:minuut:seconde
    [WebMethod]
    public int AddCompany(Company company)
    {
        return new CompanyProcess().Add(company);
    }

    [WebMethod]
    public User Login2(User user)
    {
        return new UserProcess().Login(user);
    }

    [WebMethod]
    public int AddDetailJob(DetailJob job)
    {
        return new DetailJobProcess().Add(job);
    }

    [WebMethod]
    public DetailJob[] GetDetailJob(int ID)
    {
        return new DetailJobProcess().Get(ID);
    }

    [WebMethod]
    public Job[] SearchByEmployee(Search search)
    {
        return new Searching(search).SearchByCv();
    }
    
    [WebMethod]
    public Cv[] SearchByCompany(Search search)
    {
        return new Searching(search).SearchByJob();
    }

    [WebMethod]
    public void GetBranche(int id)
    {
        new BrancheProcess().InsertFromFile();
    }
}
