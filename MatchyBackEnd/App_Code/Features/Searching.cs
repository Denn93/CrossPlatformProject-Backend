using System;
using System.Collections.Generic;
using System.Data;
using DataAccessObjects;
using Database;
using Processes;

namespace Features
{
    /// <summary>
    /// Summary description for SearchProcess
    /// </summary>
    public class Searching
    {
        private readonly List<String> _defaultSelectJob = new List<string> { "j.job_id", "j.crawlerID", "j.companyID", "j.source_ID", "j.education_ID", "j.title", "j.description", "j.place_date", "j.employment" };
        private readonly List<String> _defaultSelectCv = new List<string> { "c.cv_id", "c.crawlerID", "c.education_id", "c.source_id", "c.name", "c.personal", "c.interests", "c.jobrequirements", 
                                                                          "c.email", "c.city", "c.place_date", "c.hours", "c.profession", "c.discipline", "c.province", "c.age", "c.experience", "c.education", "c.sex"};

        public static string BaseInnerJoinsJob = "INNER JOIN Education e On e.education_ID = j.education_ID " + 
                                                "INNER JOIN DetailJob dtjob ON dtjob.job_ID = j.job_ID " + 
                                                "INNER JOIN Company c ON c.company_ID = j.companyID " + 
                                                "INNER JOIN Source s ON s.source_ID = j.source_ID ";

        public static string BaseInnerJoinsCv = "INNER JOIN Education e On e.education_ID = c.education_ID " + 
                                               "INNER JOIN Source s ON s.source_ID = c.source_ID ";

        public const string BaseWhereLikeJob = "(j.title LIKE {0} OR " +
                                               "j.description LIKE {0} OR " +
                                               "c.description LIKE {0} OR " +
                                               "dtjob.data LIKE {0}) ";

        public const string BaseWhereLikeCv = "(c.name LIKE {0} OR c.personal LIKE {0} " +
                                               "OR c.interests LIKE {0} OR c.jobrequirements LIKE {0} OR " +
                                               "c.profession LIKE {0} OR c.discipline LIKE {0} OR " +
                                               "c.experience LIKE {0} OR c.education LIKE {0} )";

        private Search _search;
        private DbHandler _dbHandler;

        public Searching(Search search)
        {
            _search = search;
        }

        public Job[] SearchByCv()
        {
            _dbHandler = DbHandler.Instance;
            var jobProcess = new JobProcess();

            string query = BuildQueryByCv();

            var queryResult = _dbHandler.RawSelectQuery(query);
            var jobList = new Job[queryResult.Rows.Count];

            for (int i = 0; i < queryResult.Rows.Count; i++)
                jobList[i] = jobProcess.ResultToObject(queryResult.Rows[i]);

            return jobList;
        }

        public Cv[] SearchByJob()
        {
            _dbHandler = DbHandler.Instance;
            var cvProcess = new CvProcess();

            string query = BuildQueryByCompany();

            var queryResult = _dbHandler.RawSelectQuery(query);
            var cvList = new Cv[queryResult.Rows.Count];

            for (int i = 0; i < queryResult.Rows.Count; i++)
                cvList[i] = cvProcess.ResultToObject(queryResult.Rows[i]);

            return cvList;
        }

        private string BuildQueryByCv()
        {
            var result = string.Empty;
            var isFirstOption = true;

            string where = "WHERE ";

            if (_search.SearchTerm != null)
            {
                where += String.Format(BaseWhereLikeJob, String.Format("\"%{0}%\"",_search.SearchTerm));
                isFirstOption = false;
            }
                

            if (_search.Education != null)
            {
                where += (isFirstOption)
                             ? String.Format("j.education_ID = {0} ", _search.Education.EducationId)
                             : String.Format("AND j.education_ID = {0} ", _search.Education.EducationId);
                isFirstOption = false;
            }

            if (_search.City != null)
            {
               where += (isFirstOption)
                             ? String.Format("c.city LIKE \"%{0}%\" ", _search.City)
                             : String.Format("AND c.city LIKE \"%{0}%\" ", _search.City);
                isFirstOption = false;
            }

            if (_search.Hours != null)
            {
                if (_search.Hours.Equals("Vast"))
                    where += (isFirstOption)
                             ? String.Format("j.employment LIKE \"%{0}%\" ", 40)
                             : String.Format("AND j.employment LIKE \"%{0}%\" ", 40);
                else if (_search.Hours.Equals("Parttime"))
                    where += (isFirstOption)
                             ? String.Format("j.employment NOT LIKE \"%{0}%\" ", 40 )
                             : String.Format("AND j.employment NOT LIKE \"%{0}%\" ", 40);

                isFirstOption = false;
            }

            if (_search.Branche != null)
            {
                BaseInnerJoinsJob += "INNER JOIN Branche_jobs brJobs ON brJobs.job_ID = j.job_id";

                where += (isFirstOption)
                              ? String.Format("brJobs.branche_ID = {0} ", _search.Branche.branche_ID)
                              : String.Format("AND brJobs.branche_ID = {0} ", _search.Branche.branche_ID);
            }

            result = String.Format("SELECT {0} FROM Jobs j {1} {2} ", String.Join(",", _defaultSelectJob.ToArray()), BaseInnerJoinsJob, where);


            return result;
        }

        private string BuildQueryByCompany()
        {
            var result = string.Empty;
            var isFirstOption = true;

            string where = "WHERE ";

            if (_search.SearchTerm != null)
            {
                where += String.Format(BaseWhereLikeCv, String.Format("\"%{0}%\" ", _search.SearchTerm));
                isFirstOption = false;
            }


            if (_search.Education != null)
            {
                where += (isFirstOption)
                             ? String.Format("c.education_ID = {0} ", _search.Education.EducationId)
                             : String.Format("AND c.education_ID = {0} ", _search.Education.EducationId);
                isFirstOption = false;
            }

            if (_search.City != null)
            {
                where += (isFirstOption)
                              ? String.Format("c.city LIKE \"%{0}%\" ", _search.City)
                              : String.Format("AND c.city LIKE \"%{0}%\" ", _search.City);
                isFirstOption = false;
            }

            if (_search.Hours != null)
            {
                where += (isFirstOption)
                            ? String.Format("c.hours LIKE \"%{0}%\" ", _search.Hours)
                            : String.Format("AND c.hours LIKE \"%{0}%\" ", _search.Hours);
                isFirstOption = false;
            }

            if (_search.Branche != null)
            {
                BaseInnerJoinsCv += "INNER JOIN Branche_cvs brCvs ON brCvs.cv_ID = c.cv_id ";

                where += (isFirstOption)
                              ? String.Format("brCvs.branche_ID = {0} ", _search.Branche.branche_ID)
                              : String.Format("AND brCvs.branche_ID = {0} ", _search.Branche.branche_ID);
            }

            result = String.Format("SELECT {0} FROM Cv c {1} {2}", String.Join(",", _defaultSelectCv.ToArray()), BaseInnerJoinsCv, where);

            return result;
        }
    }
}
