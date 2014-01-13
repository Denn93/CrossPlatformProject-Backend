using System;
using System.Collections.Generic;
using System.Data;
using DataAccessObjects;
using Database;

namespace Processes
{
    /// <summary>
    /// Summary description for DetailJobProcess
    /// </summary>
    public class DetailJobProcess : AProcess<DetailJob>
    {
        private readonly List<String> _defaultSelect = new List<string> { "detailjob_ID", "job_ID", "data"};

        public override DetailJob[] Get(int id = 0, List<KeyValuePair<String, String>> where = null, KeyValuePair<String, String> whereOperator = new KeyValuePair<String, String>(), String other = "")
        {
            _dbHandler = DbHandler.Instance;

            var result = new DataTable();

            if (id > 0)
            {
                where = new List<KeyValuePair<string, string>>();
                where.Add(new KeyValuePair<string, string>("detailjob_ID", id.ToString()));
            }

            if (id >= 0)
                result = _dbHandler.Select(_defaultSelect, "Detailjob", where);
            else if (id == -1)
                return new DetailJob[] { new DetailJob()};

            if (result.Rows.Count == 0 && where == null)
                return new DetailJob[] { new DetailJob() };

            var detailJobList = new DetailJob[result.Rows.Count];

            for (int i = 0; i < result.Rows.Count; i++)
                detailJobList[i] = ResultToObject(result.Rows[i]);

            return detailJobList;
        }

        public override int Add(DetailJob obj)
        {
            var resultId = 0;

            _dbHandler = DbHandler.Instance;

            var where = new List<KeyValuePair<String, String>>();
            where.Add(new KeyValuePair<string, string>("job_ID", obj.JobId.ToString()));
            var detailjobs = Get(0, where);

            var insertData = new List<KeyValuePair<String, String>>();
            insertData.Add(new KeyValuePair<string, string>("job_ID", obj.JobId.ToString()));
            insertData.Add(new KeyValuePair<string, string>("data", obj.Data));

            resultId = detailjobs.Length == 0 ? _dbHandler.Insert("Detailjob", insertData) : detailjobs[0].DetailJobId;

            return resultId;
        }

        public override int Delete(int id, List<KeyValuePair<string, string>> where = null)
        {
            throw new NotImplementedException();
        }

        public override int Update(DetailJob obj)
        {
            throw new NotImplementedException();
        }

        public override DetailJob ResultToObject(DataRow data)
        {
            var detailJob = new DetailJob();

            detailJob.DetailJobId = Convert.ToInt32(data["detailjob_ID"].ToString()); 
            detailJob.JobId = Convert.ToInt32(data["job_ID"].ToString());
            detailJob.Data = data["data"].ToString();

            return detailJob;
        }
    }
}