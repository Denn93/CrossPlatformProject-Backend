using System;
using System.Collections.Generic;
using System.Data;
using DataAccessObjects;
using Database;

namespace Processes
{
    /// <summary>
    /// Summary description for MatchesProcess
    /// </summary>
    public class MatchesProcess : AProcess<Match>
    {
        private readonly List<String> _defaultSelect = new List<string> { "cv_ID", "job_ID", "score", "date"};

        public override Match[] Get(int id = 0, List<KeyValuePair<string, string>> where = null, KeyValuePair<String, String> whereOperator = new KeyValuePair<String, String>(), String other = "")
        {
            _dbHandler = DbHandler.Instance;

            var result = new DataTable();

            if (id >= 0)
                result = _dbHandler.Select(_defaultSelect, "Matches", where, whereOperator, other);
            else if (id == -1)
                return new Match[] { new Match() };

            var matchList = new Match[result.Rows.Count];

            for (int i = 0; i < result.Rows.Count; i++)
                matchList[i] = ResultToObject(result.Rows[i]);

            return matchList;
        }

        public override int Add(Match obj)
        {
            _dbHandler = DbHandler.Instance;

            var where = new List<KeyValuePair<String, String>>();
            where.Add(new KeyValuePair<string, string>("job_ID", obj.Job.JobID.ToString()));
            where.Add(new KeyValuePair<string, string>("cv_ID", obj.Cv.CvID.ToString()));
            var matches = Get(0, where);

            var insertData = new List<KeyValuePair<String, String>>();
            insertData.Add(new KeyValuePair<string, string>("cv_ID", obj.Cv.CvID.ToString()));
            insertData.Add(new KeyValuePair<string, string>("job_ID", obj.Job.JobID.ToString()));
            insertData.Add(new KeyValuePair<string, string>("score", obj.Score.ToString()));
            insertData.Add(new KeyValuePair<string, string>("date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

            _dbHandler.Insert("Matches", insertData);

            return 0; /*Een match kan nooit 1 insertID terug returnen. 
                      Tabel bestaat uit een gecombineerde PK sleutel*/
        }

        public override int Delete(int id, List<KeyValuePair<string, string>> where = null)
        {
            throw new NotImplementedException();
        }

        public override int Update(Match obj)
        {
            throw new NotImplementedException();
        }

        protected override Match ResultToObject(DataRow data)
        {
            var match = new Match();
            var jobProcess = new JobProcess();
            var cvProcess = new CvProcess();

            match.Cv = cvProcess.Get(Convert.ToInt32(data["cv_ID"].ToString()))[0];
            match.Job = jobProcess.Get(Convert.ToInt32(data["job_ID"].ToString()))[0];
            match.Score = Convert.ToInt32(data["score"].ToString());
            match.Date = data["date"].ToString();

            return match;
        }
    }
}