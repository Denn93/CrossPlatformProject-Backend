using System;
using System.Collections.Generic;
using System.Data;
using Database;
using log4net;


namespace Processes
{
    /// <summary>
    /// Dit zijn de basis elementen voor elke Process class. 
    /// </summary>
    public abstract class AProcess<TDataAccessObject>
    {
        protected readonly ILog _log = LogManager.GetLogger(typeof(TDataAccessObject));
        protected DbHandler _dbHandler;

        public abstract TDataAccessObject[] Get(int id = 0, List<KeyValuePair<String, String>> where = null, KeyValuePair<String, String> whereOperator = new KeyValuePair<String, String>(), String other = "");
        public abstract int Add(TDataAccessObject obj);
        public abstract int Delete(int id, List<KeyValuePair<String, String>> where = null);
        public abstract int Update(TDataAccessObject obj);

        protected abstract TDataAccessObject ResultToObject(DataRow data);
    }
}
