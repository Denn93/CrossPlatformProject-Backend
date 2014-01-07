using System;
using System.Collections.Generic;
using System.Data;
using DataAccessObjects;
using Database;

namespace Processes
{
    /// <summary>
    /// Summary description for CompanyProcess
    /// </summary>
    public class CompanyProcess : AProcess<Company>
    {
        private readonly List<String> _defaultSelect = new List<string> {"company_id", "name", "description", "date", "city", "email", "telnr"}; 

        public override Company[] Get(int id = 0, List<KeyValuePair<string, string>> where = null)
        {
            _dbHandler = DbHandler.Instance;

            var result = new DataTable();

            if (id >= 0)
                result = _dbHandler.Select(_defaultSelect, "Company", where);
            else if (id == -1)
                return new Company[] { new Company() };

            var companyList = new Company[result.Rows.Count];

            for (int i = 0; i < result.Rows.Count; i++)
                companyList[i] = ResultToObject(result.Rows[i]);

            return companyList;
        }

        public override int Add(Company obj)
        {
            var resultId = 0;

            _dbHandler = DbHandler.Instance;

            var where = new List<KeyValuePair<String, String>>();
            where.Add(new KeyValuePair<string, string>("name", obj.CompanyName));
            where.Add(new KeyValuePair<string, string>("description", obj.CompanyDescription));
            where.Add(new KeyValuePair<string, string>("city", obj.CompanyCity));
            where.Add(new KeyValuePair<string, string>("email", obj.CompanyEmail));
            where.Add(new KeyValuePair<string, string>("telnr", obj.CompanyTel));
            var companies = Get(0, where);

            var insertData = new List<KeyValuePair<String, String>>();
            insertData.Add(new KeyValuePair<string, string>("name", obj.CompanyName));
            insertData.Add(new KeyValuePair<string, string>("description", obj.CompanyDescription));
            insertData.Add(new KeyValuePair<string, string>("date", obj.CompanyDate)); 
            insertData.Add(new KeyValuePair<string, string>("city", obj.CompanyCity));
            insertData.Add(new KeyValuePair<string, string>("email", obj.CompanyEmail));
            insertData.Add(new KeyValuePair<string, string>("telnr", obj.CompanyTel));

            resultId = companies.Length == 0 ? _dbHandler.Insert("Company", insertData) : companies[0].CompanyID;

            return resultId;
        }

        public override int Delete(int id, List<KeyValuePair<string, string>> where = null)
        {
            throw new NotImplementedException();
        }

        public override int Update(Company obj)
        {
            throw new NotImplementedException();
        }

        protected override Company ResultToObject(DataRow data)
        {
            var company = new Company();

            company.CompanyDate = data["date"].ToString(); 
            company.CompanyDescription = data["description"].ToString();
            company.CompanyID = Convert.ToInt32(data["company_id"].ToString());
            company.CompanyName = data["name"].ToString();
            company.CompanyCity = data["city"].ToString();

            return company;
        }
    }
}