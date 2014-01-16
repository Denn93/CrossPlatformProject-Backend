using System;
using System.Collections.Generic;
using System.Data;
using DataAccessObjects;
using Database;
using Features;


namespace Processes
{
    /// <summary>
    /// Summary description for BrancheProcess
    /// </summary>
    public class BrancheProcess : AProcess<Branche>
    {
        private readonly List<String> _defaultSelect = new List<string> { "branche_ID", "description"};

        public override Branche[] Get(int id = 0, List<KeyValuePair<String, String>> where = null, KeyValuePair<String, String> whereOperator = new KeyValuePair<String, String>(), String other = "")
        {
            _dbHandler = DbHandler.Instance;

            var result = new DataTable();

            if (id > 0)
            {
                where = new List<KeyValuePair<string, string>>();
                where.Add(new KeyValuePair<string, string>("branche_ID", id.ToString()));
            }

            if (id >= 0)
                result = _dbHandler.Select(_defaultSelect, "Branche", where);
            else if (id == -1)
                return new Branche[] { new Branche() };

            var brancheList = new Branche[result.Rows.Count];

            for (int i = 0; i < result.Rows.Count; i++)
                brancheList[i] = ResultToObject(result.Rows[i]);

            return brancheList;
        }

        public override int Add(Branche branche)
        {
            var resultId = 0;

            _dbHandler = DbHandler.Instance;

            var where = new List<KeyValuePair<String, String>>();
            where.Add(new KeyValuePair<string, string>("description", branche.Description));
            var branches = Get(0, where);

            var insertData = new List<KeyValuePair<String, String>>();
            insertData.Add(new KeyValuePair<string, string>("description", branche.Description));

            resultId = branches.Length == 0 ? _dbHandler.Insert("Branche", insertData) : branches[0].branche_ID;

            return resultId;
        }

        public override Branche ResultToObject(DataRow data)
        {
            var branche = new Branche();

            branche.branche_ID = Convert.ToInt32(data["branche_ID"].ToString());
            branche.Description = data["description"].ToString();

            return branche;
        }

        public void InsertFromFile()
        {
            String[] branches = System.IO.File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "brancheList.txt");

            foreach (var branch in branches)
                Add(new Branche() {Description = branch});
        }

        public List<int> DetermineBranche(int id, String objectType)
        {
            _dbHandler = DbHandler.Instance;
            var results = new List<int>();

            foreach(var branche in Get())
            {
                String where = "WHERE ";

                where += (objectType.Equals("Job"))
                                   ? String.Format(Searching.BaseWhereLikeJob, String.Format("\"%{0}%\"", branche.Description))
                                   : String.Format(Searching.BaseWhereLikeCv, String.Format("\"%{0}%\"", branche.Description));

                String query = (objectType.Equals("Job"))
                                   ? String.Format("Select * FROM Jobs j {0} {1} AND j.job_ID = {2} ",
                                                   Searching.BaseInnerJoinsJob, where, id)
                                   : String.Format("Select * FROM Cv c {0} {1} AND c.cv_ID = {2} ",
                                                   Searching.BaseInnerJoinsCv, where, id);

                DataTable result = new DataTable();
                result = _dbHandler.RawSelectQuery(query);

                if (result.Rows.Count != 0)
                    results.Add(branche.branche_ID);
            }

            return results;
        }
    }
}
