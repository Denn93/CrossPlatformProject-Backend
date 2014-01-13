using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using log4net;

namespace Database
{
        /// <summary>
    /// Summary description for DBHandler
    /// </summary>
    public class DbHandler
    {

        private const string ServerAddress = "127.0.0.1";
        private const uint ServerPort = 3306;
        private const string DatabaseName = "matchybackend";
        private const string DatebaseUsername = "root";
        private const string DatebasePassword = "";

        private static DbHandler _instance;
        private static MySqlConnection _connection;

        private readonly ILog _log = LogManager.GetLogger(typeof(DbHandler));

        private DbHandler()
        {
            log4net.Config.XmlConfigurator.Configure();
            _log.Info("DbHandler initialized!!");
	        MakeConnection();
	    }

        public static DbHandler Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DbHandler();

                return _instance;
            }
        }

        private void MakeConnection()
        {
            try
            {
                var connectionBuilder = new MySqlConnectionStringBuilder();
                connectionBuilder.Server = ServerAddress;
                connectionBuilder.Port = ServerPort;
                connectionBuilder.Database = DatabaseName;
                connectionBuilder.UserID = DatebaseUsername;
                connectionBuilder.Password = DatebasePassword;
                connectionBuilder.AllowZeroDateTime = true;
                connectionBuilder.ConvertZeroDateTime = true;

                _connection = new MySqlConnection(connectionBuilder.ToString());
                _connection.Open();
            }
            catch (MySqlException e )
            {
                _log.Fatal("Create Connection with Database failed " + e.Message);
            }
        }

        private void CloseConnection()
        {
            try
            {
                _connection.Clone();
            }
            catch (MySqlException e)
            {
                _log.Fatal("Close Connection with the database has failed: " + e.Message);
            }
        }

        /// <summary>
        /// Deze methode kan voor de 2 verschillende strings zorgen. Namelijk een Select string en een parameter String 
        /// </summary>
        /// <param name="insert">De data die gebruikt moet worden</param>
        /// <param name="isSelect">Wanneer True maakt de methode een select String. Bijv veld 1, veld2, veld3 etc. Anders wordt het een parameter String. Bijv @veld1, @veld2 etc.</param>
        /// <returns>Returns result String</returns>
        private String createStringsForInsert(List<KeyValuePair<String, String>> insert, Boolean isSelect)
        {
            String result = "";

            int i = 1;
            foreach (var value in insert)
            {
                if (i == insert.Count)
                    if (isSelect)
                        result += value.Key;
                    else
                        result += "@" + value.Key;
                else
                    if (isSelect)
                        result += value.Key + ",";
                    else
                        result += "@" + value.Key + ", ";

                i++;
            }

            return result;
        }

        /// <summary>
        /// Deze methode maakt een where query string aan met de aangeleverde velden en waardes. 
        /// </summary>
        /// <param name="where">Alle waardes die in de where verwerkt moeten worden. Bijvoorbeeld dus veld = waarde </param>
        /// <returns>De volledige query String</returns>
        private String createWhereString(List<KeyValuePair<String , String>> where, KeyValuePair<String, String> whereOperator)
        {
            String result = "Where ";

            foreach (var pair in where)
            {
                var sqloperator = "="; 

                if (whereOperator.Key.Equals(pair.Key))
                    sqloperator = whereOperator.Value;

                if (where.IndexOf(pair).Equals(where.Count - 1))
                    result += String.Format("{0} {1} '{2}'", pair.Key, sqloperator, pair.Value);
                else
                {
                    result += String.Format("{0} {1} '{2}' AND ", pair.Key, sqloperator, pair.Value);
                }
            }

            return result;
        }

        public DataTable Select(List<String> select, String tableName, List<KeyValuePair<String, String>> where = null, KeyValuePair<String, String> whereOperator = new KeyValuePair<string, string>(), String other = "")
        {
            var result = new DataTable();
            MySqlCommand dbCom = _connection.CreateCommand();

            String whereString = "";

            if (whereOperator.Key == null)
                whereOperator = new KeyValuePair<string, string>("default", "=");

            if (where != null)
                whereString = createWhereString(where, whereOperator);

            try
            {
                dbCom.CommandText = String.Format("Select {0} FROM {1} {2} {3}", String.Join(",", select.ToArray()), tableName, whereString, other);

                MySqlDataReader dbReader = dbCom.ExecuteReader();

                DataSet ds = new DataSet();
                ds.Tables.Add(result);
                ds.EnforceConstraints = false;
                result.Load(dbReader);
                dbReader.Close();
            }
            catch (MySqlException e)
            {
                _log.Error("There was a error executing the query: " + e.Message);
            }

            return result;
        }

        public int Insert(String tableName, List<KeyValuePair<String, String>> data)
        {
            var resultId = 0;

            string selectString = createStringsForInsert(data, true);
            string valueString = createStringsForInsert(data, false);

            try
            {
                MySqlCommand dbCom = _connection.CreateCommand();

                dbCom.CommandText = String.Format("insert into {0} ({1}) values({2}); SELECT LAST_INSERT_ID();", tableName, selectString, valueString);

                foreach (var value in data)
                {
                    dbCom.Parameters.AddWithValue("@" + value.Key, value.Value);
                    // createTempFile(values[i]);  TODO Dennis Only for Debug purposes
                }

                resultId = Convert.ToInt32(dbCom.ExecuteScalar());

            }
            catch (MySqlException e)
            {
                _log.Error("There was a error executing the query: " + e.Message);
            }

            return resultId;
        }

        public int Update()
        {
            var resultId = 0;

            return resultId;
        }

        public int Delete()
        {
            var resultId = 0;

            return resultId;
        }
    }
}

