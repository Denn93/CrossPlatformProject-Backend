using System;
using System.Collections.Generic;
using System.Data;
using DataAccessObjects;
using Database;


namespace Processes
{
    /// <summary>
    /// Summary description for BrancheProcess
    /// </summary>
    public class BrancheProcess : AProcess<Branche>
    {
        private readonly List<String> _defaultSelect = new List<string> { "brancheID", "crawlerID", "companyID", "source_ID", "education_ID", "title", "description", "place_date", "employment" }; 

        public override Branche[] Get(int id = 0, List<KeyValuePair<string, string>> @where = null)
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

        public override int Add(Branche obj)
        {
            throw new System.NotImplementedException();
        }

        public override int Delete(int id, List<KeyValuePair<string, string>> @where = null)
        {
            throw new System.NotImplementedException();
        }

        public override int Update(Branche obj)
        {
            throw new System.NotImplementedException();
        }

        protected override Branche ResultToObject(DataRow data)
        {
            throw new System.NotImplementedException();
        }
    }
}
