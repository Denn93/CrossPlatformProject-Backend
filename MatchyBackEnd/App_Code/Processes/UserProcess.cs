using System;
using System.Collections.Generic;
using System.Data;
using DataAccessObjects;
using Database;

namespace Processes
{
    /// <summary>
    /// Deze Poccess class is specifiek voor User Data
    /// </summary>
    public class UserProcess : AProcess<User>
    {
        private readonly List<String> _defaultSelect = new List<String> { "profile_ID", "cv_ID", "company_ID", "password", "email", "date"};

        public override User[] Get(int id = 0, List<KeyValuePair<string, string>> where = null)
        {
            _dbHandler = DbHandler.Instance;

            if (id > 0)
            {
                where = new List<KeyValuePair<string, string>>();
                where.Add(new KeyValuePair<string, string>("profile_ID", id.ToString()));
            }

            var result = _dbHandler.Select(_defaultSelect, "Profile", where);
            var users = new User[result.Rows.Count];

            for (var i = 0; i < result.Rows.Count; i++)
                users[i] = ResultToObject(result.Rows[i]);

            return users;
        }

        public override int Add(User obj)
        {
            int resultId = 0;

            _dbHandler = DbHandler.Instance;

            //TODO Dennis Yet To be determined when Cv en Company Are inserted

            return resultId;
        }

        public override int Delete(int id, List<KeyValuePair<string, string>> where = null)
        {
            //TODO Dennis User deletion to be added

            throw new NotImplementedException();
        }

        public override int Update(User obj)
        {
            //TODO Dennis User Update yet to be added

            throw new NotImplementedException();
        }

        protected override User ResultToObject(DataRow data)
        {
            var user = new User();

            var companyId = (data["company_ID"].ToString().Equals("")) ? -1 : Convert.ToInt32(data["company_ID"].ToString());
            var cvId = (data["cv_ID"].ToString().Equals("")) ? -1 : Convert.ToInt32(data["cv_ID"].ToString());

            user.ProfileId = Convert.ToInt32(data["profile_ID"].ToString());
            user.UserCompany = new CompanyProcess().Get(companyId)[0];
            user.Pass = data["password"].ToString();
            user.UserCv = new CvProcess().Get(cvId)[0];
            user.Email = data["email"].ToString();
            user.BirthDay = data["date"].ToString(); //TODO Kan object niet hendelen. Moet nog Gefixed worden

            return user;
        }

        /// <summary>
        /// Deze methode kijkt of een user al bestaat en mag inloggen. Zo ja stuur alle data over hem terug. Naar website of App
        /// </summary>
        /// <param name="user">Het user object dat binnekomt in de webservice</param>
        /// <returns>Een user object met alle juiste data. Wanneer user niet bestaat stuur dezelfde user terug met Maylogin False</returns>
        public User Login(User user)
        {
            _dbHandler = DbHandler.Instance;

            var where = new List<KeyValuePair<string, string>>();
            where.Add(new KeyValuePair<String, String>("email", user.Email));

            var loginResult = _dbHandler.Select(_defaultSelect, "Profile", where);

            try
            {
                if (loginResult.Rows[0]["password"].ToString().Equals(user.Pass))
                {
                    user = Get(Convert.ToInt32(loginResult.Rows[0]["profile_ID"].ToString()))[0];
                    user.MayLogin = true;
                }

            }
            catch (NullReferenceException e)
            {
                _log.Error("No User was found by that email: " + e.Message);
            }
            catch(Exception e)
            {
                _log.Error("There was an unexpected error: " + e.Message);
            }
            finally
            {
                user.MayLogin = false;
            }
            
            return user;
        }
    }
}
