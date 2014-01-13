using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DataAccessObjects;
using Database;

namespace Processes
{
    /// <summary>
    /// Summary description for BrancheJobProcess
    /// </summary>
    public class BrancheJobProcess : AProcess<BrancheJob>
    {
        private readonly List<String> _defaultSelect = new List<string> { "branche_ID", "job_ID" };

        public override BrancheJob[] Get(int id = 0, List<KeyValuePair<string, string>> where = null, KeyValuePair<string, string> whereOperator = new KeyValuePair<string, string>(), string other = "")
        {
            _dbHandler = DbHandler.Instance;

            var result = new DataTable();

            if (id >= 0)
                result = _dbHandler.Select(_defaultSelect, "Branche_jobs", where, whereOperator, other);
            else if (id == -1)
                return new BrancheJob[] { new BrancheJob() };

            var brancheJobList = new BrancheJob[result.Rows.Count];

            for (int i = 0; i < result.Rows.Count; i++)
                brancheJobList[i] = ResultToObject(result.Rows[i]);

            return brancheJobList;
        }

        public override int Add(BrancheJob obj)
        {
            _dbHandler = DbHandler.Instance;

            var where = new List<KeyValuePair<String, String>>();
            where.Add(new KeyValuePair<string, string>("branche_ID", obj.branche_ID.ToString()));
            where.Add(new KeyValuePair<string, string>("job_ID", obj.job_ID.ToString()));
            var brancheJobs = Get(0, where);

            if (brancheJobs.Length == 0)
            {
                var insertData = new List<KeyValuePair<String, String>>();
                insertData.Add(new KeyValuePair<string, string>("branche_ID", obj.branche_ID.ToString()));
                insertData.Add(new KeyValuePair<string, string>("job_ID", obj.job_ID.ToString()));

                _dbHandler.Insert("Branche_jobs", insertData);
            }

            return 0;
        }

        public override int Delete(int id, List<KeyValuePair<string, string>> @where = null)
        {
            throw new NotImplementedException();
        }

        public override int Update(BrancheJob obj)
        {
            throw new NotImplementedException();
        }

        public override BrancheJob ResultToObject(DataRow data)
        {
            var brancheJob = new BrancheJob();

            brancheJob.branche_ID = Convert.ToInt32(data["branche_ID"].ToString());
            brancheJob.job_ID = Convert.ToInt32(data["job_ID"].ToString());

            return brancheJob;
        }
    }
}
