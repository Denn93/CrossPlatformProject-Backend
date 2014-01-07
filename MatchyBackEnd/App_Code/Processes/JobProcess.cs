﻿using System;
using System.Collections.Generic;
using System.Data;
using DataAccessObjects;
using Database;

namespace Processes
{
    /// <summary>
    /// Summary description for JobProcess
    /// </summary>
    public class JobProcess : AProcess<Job>
    {
        private readonly List<String> _defaultSelect = new List<string> { "job_id", "crawlerID", "companyID", "source_ID", "education_ID", "title", "description", "place_date", "employment"}; 

        public override Job[] Get(int id = 0, List<KeyValuePair<string, string>> @where = null)
        {
            _dbHandler = DbHandler.Instance;

            var result = new DataTable();

            if (id >= 0)
                result = _dbHandler.Select(_defaultSelect, "jobs", where);
            else if (id == -1)
                return new Job[] { new Job() };

            var jobList = new Job[result.Rows.Count];

            for (int i = 0; i < result.Rows.Count; i++)
                jobList[i] = ResultToObject(result.Rows[i]);

            return jobList;
        }

        public override int Add(Job obj)
        {
            var resultId = 0;

            _dbHandler = DbHandler.Instance;

            var where = new List<KeyValuePair<String, String>>();
            where.Add(new KeyValuePair<string, string>("crawlerID", obj.CrawlerID.ToString()));
            where.Add(new KeyValuePair<string, string>("source_ID", obj.Source.SourceId.ToString()));
            var jobs = Get(0, where);

            var companyProcess = new CompanyProcess();
            int companyId = companyProcess.Add(obj.Company);

         
            var insertData = new List<KeyValuePair<String, String>>();
            insertData.Add(new KeyValuePair<string, string>("crawlerID", obj.CrawlerID.ToString()));
            insertData.Add(new KeyValuePair<string, string>("companyID", companyId.ToString()));
            insertData.Add(new KeyValuePair<string, string>("source_ID", obj.Source.SourceId.ToString()));
            insertData.Add(new KeyValuePair<string, string>("education_ID", obj.Education.EducationId.ToString()));
            insertData.Add(new KeyValuePair<string, string>("title", obj.JobTitle));
            insertData.Add(new KeyValuePair<string, string>("description", obj.JobDescription));
            insertData.Add(new KeyValuePair<string, string>("place_date", obj.JobPlaceDate));
            insertData.Add(new KeyValuePair<string, string>("employment", obj.JobHours));

            resultId = jobs.Length == 0 ? _dbHandler.Insert("jobs", insertData) : jobs[0].JobID;

            obj.DetailJob.JobId = resultId;
            var detailJobProcess = new DetailJobProcess();
            detailJobProcess.Add(obj.DetailJob);

            return resultId;
        }

        public override int Delete(int id, List<KeyValuePair<string, string>> @where = null)
        {
            throw new NotImplementedException();
        }

        public override int Update(Job obj)
        {
            throw new NotImplementedException();
        }

        protected override Job ResultToObject(DataRow data)
        {
            var result = new Job();
            var educationProcess = new EducationProcess();
            var sourceProcess = new SourceProcess();
            var detailJobProcess = new DetailJobProcess();
            var companyProcess = new CompanyProcess();

            result.JobID = Convert.ToInt32(data["job_ID"].ToString());
            result.CrawlerID = Convert.ToInt32(data["crawlerID"].ToString());
            result.Company = companyProcess.Get(Convert.ToInt32(data["companyID"].ToString()))[0];
            result.Source = sourceProcess.Get(Convert.ToInt32(data["source_ID"].ToString()))[0];
            result.Education = educationProcess.Get(Convert.ToInt32(data["education_ID"].ToString()))[0];

            result.JobTitle = data["title"].ToString();
            result.JobDescription = data["description"].ToString();
            result.JobPlaceDate = data["place_date"].ToString();
            result.JobHours = data["employment"].ToString();

            var whereDetail = new List<KeyValuePair<string, string>>();
            whereDetail.Add(new KeyValuePair<string, string>("job_ID", result.JobID.ToString()));
            result.DetailJob = (detailJobProcess.Get(0, whereDetail).Length == 0) ? new DetailJob() : detailJobProcess.Get(0, whereDetail)[0];

            return result;
        }
    }
}