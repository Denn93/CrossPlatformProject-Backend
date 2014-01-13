using System;
using System.Collections.Generic;
using System.Data;
using DataAccessObjects;
using Database;

namespace Processes
{
    /// <summary>
    /// Summary description for SourceProcess
    /// </summary>
    public class SourceProcess : AProcess<Source>
    {
        private readonly List<String> _defaultSelect = new List<string> { "source_ID", "description" };

        public override Source[] Get(int id = 0, List<KeyValuePair<String, String>> where = null, KeyValuePair<String, String> whereOperator = new KeyValuePair<String, String>(), String other = "")
        {
            _dbHandler = DbHandler.Instance;

            var result = new DataTable();

            if (id > 0)
            {
                where = new List<KeyValuePair<string, string>>();
                where.Add(new KeyValuePair<string, string>("source_ID", id.ToString()));
            }

            if (id >= 0)
                result = _dbHandler.Select(_defaultSelect, "Source", where);
            else if (id == -1)
                return new Source[] { new Source() };

            var sourceList = new Source[result.Rows.Count];

            for (int i = 0; i < result.Rows.Count; i++)
                sourceList[i] = ResultToObject(result.Rows[i]);

            return sourceList;
        }

        public override int Add(Source obj)
        {
            var resultId = 0;

            _dbHandler = DbHandler.Instance;

            var where = new List<KeyValuePair<String, String>>();
            where.Add(new KeyValuePair<string, string>("description", obj.Description));
            var sources = Get(0, where);

            var insertData = new List<KeyValuePair<String, String>>();
            insertData.Add(new KeyValuePair<string, string>("description", obj.Description));

            resultId = sources.Length == 0 ? _dbHandler.Insert("Source", insertData) : sources[0].SourceId;

            return resultId;
        }

        public override int Delete(int id, List<KeyValuePair<string, string>> @where = null)
        {
            throw new NotImplementedException();
        }

        public override int Update(Source obj)
        {
            throw new NotImplementedException();
        }

        public override Source ResultToObject(DataRow data)
        {
            var source = new Source();

            source.SourceId = Convert.ToInt32(data["source_ID"].ToString());
            source.Description = data["description"].ToString();

            return source;
        }
    }
}