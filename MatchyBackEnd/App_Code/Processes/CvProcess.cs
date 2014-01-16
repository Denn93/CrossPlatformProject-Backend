using System;
using System.Collections.Generic;
using System.Data;
using DataAccessObjects;
using Database;

namespace Processes
{
    /// <summary>
    /// Summary description for CvProcess
    /// </summary>
    public class CvProcess : AProcess<Cv>
    {
        public static readonly List<String> _defaultSelectCv = new List<string> { "cv_id", "crawlerID", "education_id", "source_id", "name", "personal", "interests", "jobrequirements", 
                                                                          "email", "city", "place_date", "hours", "profession", "discipline", "province", "age", "experience", "education", "sex"};

        public override Cv[] Get(int id = 0, List<KeyValuePair<String, String>> where = null, KeyValuePair<String, String> whereOperator = new KeyValuePair<String, String>(), String other = "")
        {
            _dbHandler = DbHandler.Instance;

            var result = new DataTable();
            if (id > 0)
            {
                where = new List<KeyValuePair<string, string>>();
                where.Add(new KeyValuePair<string, string>("cv_ID", id.ToString()));
            }

            if (id >= 0)
                result = _dbHandler.Select(_defaultSelectCv, "Cv", where);                
            else if (id == -1)
                return new Cv[] { new Cv() };

            var cvList = new Cv[result.Rows.Count];

            for (int i = 0; i < result.Rows.Count; i++)
                cvList[i] = ResultToObject(result.Rows[i]);

            return cvList;
        }

        public override int Add(Cv obj)
        {
            var resultId = 0;

            _dbHandler = DbHandler.Instance;

            var where = new List<KeyValuePair<String, String>>();
            where.Add(new KeyValuePair<string, string>("crawlerID", obj.CrawlerId.ToString()));
            var cvs = Get(0, where);

            var insertData = new List<KeyValuePair<String, String>>();
            insertData.Add(new KeyValuePair<string, string>("crawlerID", obj.CrawlerId.ToString()));
            insertData.Add(new KeyValuePair<string, string>("education_ID", obj.EducationLevel.EducationId.ToString()));
            insertData.Add(new KeyValuePair<string, string>("source_ID", obj.Source.SourceId.ToString()));
            insertData.Add(new KeyValuePair<string, string>("name", "test"));
            insertData.Add(new KeyValuePair<string, string>("personal", obj.Personal));
            insertData.Add(new KeyValuePair<string, string>("interests", obj.Interests));
            insertData.Add(new KeyValuePair<string, string>("jobrequirements", obj.JobRequirements));
            insertData.Add(new KeyValuePair<string, string>("email", "test@test.com"));
            insertData.Add(new KeyValuePair<string, string>("city", obj.City));
            insertData.Add(new KeyValuePair<string, string>("place_date", obj.Date));
            insertData.Add(new KeyValuePair<string, string>("hours", obj.Hours));
            insertData.Add(new KeyValuePair<string, string>("profession", obj.Profession));
            insertData.Add(new KeyValuePair<string, string>("discipline", obj.Discipline));
            insertData.Add(new KeyValuePair<string, string>("province", obj.Province));
            insertData.Add(new KeyValuePair<string, string>("age", obj.Age.ToString()));
            insertData.Add(new KeyValuePair<string, string>("experience", obj.WorkExperience));
            insertData.Add(new KeyValuePair<string, string>("education", obj.EducationHistory));
            insertData.Add(new KeyValuePair<string, string>("sex", obj.Sex));

            resultId = cvs.Length == 0 ? _dbHandler.Insert("Cv", insertData) : cvs[0].CvID;
            
            AddBranches(resultId);

            return resultId;
        }

        public override Cv ResultToObject(DataRow data)
        {
            var result = new Cv();
            var educationProcess = new EducationProcess();
            var sourceProcess = new SourceProcess();

            result.CvID = Convert.ToInt32(data["cv_id"].ToString()); 
            result.CrawlerId = Convert.ToInt32(data["crawlerID"].ToString());
            result.EducationLevel = educationProcess.Get(Convert.ToInt32(data["education_ID"].ToString()))[0];
            result.Source = sourceProcess.Get(Convert.ToInt32(data["source_ID"].ToString()))[0];

            result.Name = data["name"].ToString();
            result.Personal = data["personal"].ToString();
            result.Interests = data["interests"].ToString();
            result.JobRequirements = data["jobrequirements"].ToString();
            result.Email = data["email"].ToString();
            result.City = data["city"].ToString();
            result.Date = data["place_date"].ToString();
            result.Hours = data["hours"].ToString();
            result.Profession = data["profession"].ToString();
            result.Discipline = data["discipline"].ToString();
            result.Province = data["province"].ToString();
            result.Age = Convert.ToInt32(data["age"].ToString());
            result.WorkExperience = data["experience"].ToString();
            result.EducationHistory = data["education"].ToString();
            result.Sex = data["sex"].ToString();

            return result;
        }

        private void AddBranches(int cv_Id)
        {
            var branches = new BrancheProcess().DetermineBranche(cv_Id, "Cv");

            foreach (var branch in branches)
            {
                var brancheJob = new BrancheCv { branche_ID = branch, cv_ID = cv_Id };
                new BrancheCvProcess().Add(brancheJob);
            }
        }
    }
}