using System;
using System.Collections.Generic;
using System.Data;
using DataAccessObjects;
using Database;

namespace Processes
{
    /// <summary>
    /// Summary description for BrancheJobProcess
    /// </summary>
    public class BrancheCvProcess : AProcess<BrancheCv>
    {
        private readonly List<String> _defaultSelect = new List<string> { "branche_ID", "cv_ID"};

        public override BrancheCv[] Get(int id = 0, List<KeyValuePair<string, string>> @where = null, KeyValuePair<string, string> whereOperator = new KeyValuePair<string, string>(), string other = "")
        {
            _dbHandler = DbHandler.Instance;

            var result = new DataTable();

            if (id >= 0)
                result = _dbHandler.Select(_defaultSelect, "Branche_cvs", where, whereOperator, other);
            else if (id == -1)
                return new BrancheCv[] { new BrancheCv() };

            var brancheCvList = new BrancheCv[result.Rows.Count];

            for (int i = 0; i < result.Rows.Count; i++)
                brancheCvList[i] = ResultToObject(result.Rows[i]);

            return brancheCvList;
        }

        public override int Add(BrancheCv obj)
        {
            _dbHandler = DbHandler.Instance;

            var where = new List<KeyValuePair<String, String>>();
            where.Add(new KeyValuePair<string, string>("branche_ID", obj.branche_ID.ToString()));
            where.Add(new KeyValuePair<string, string>("cv_ID", obj.cv_ID.ToString()));
            var brancheCvs = Get(0, where);

            if (brancheCvs.Length == 0)
            {
                var insertData = new List<KeyValuePair<String, String>>();
                insertData.Add(new KeyValuePair<string, string>("branche_ID", obj.branche_ID.ToString()));
                insertData.Add(new KeyValuePair<string, string>("cv_ID", obj.cv_ID.ToString()));

                _dbHandler.Insert("Branche_cvs", insertData);
            }

            return 0;
        }

        public override BrancheCv ResultToObject(DataRow data)
        {
            var brancheCv = new BrancheCv();

            brancheCv.branche_ID = Convert.ToInt32(data["branche_ID"].ToString());
            brancheCv.cv_ID = Convert.ToInt32(data["cv_ID"].ToString());

            return brancheCv;
        }
    }
}