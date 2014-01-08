using System;
using System.Collections.Generic;
using System.Data;
using DataAccessObjects;
using Database;

namespace Processes
{
    /// <summary>
    /// Summary description for EducationProcess
    /// </summary>
    public class EducationProcess : AProcess<Education>
    {
        private readonly List<String> _defaultSelect = new List<string> { "education_ID", "description"}; 

        public override Education[] Get(int id = 0, List<KeyValuePair<string, string>> @where = null)
        {
            _dbHandler = DbHandler.Instance;

            var result = new DataTable();

            if (id > 0)
            {
                where = new List<KeyValuePair<string, string>>();
                where.Add(new KeyValuePair<string, string>("education_ID", id.ToString()));
            }

            if (id >= 0)
                result = _dbHandler.Select(_defaultSelect, "Education", where);
            else if (id == -1)
                return new Education[] { new Education() };

            var educationList = new Education[result.Rows.Count];

            for (int i = 0; i < result.Rows.Count; i++)
                educationList[i] = ResultToObject(result.Rows[i]);

            return educationList;
        }

        public override int Add(Education obj)
        {
            var resultId = 0;

            _dbHandler = DbHandler.Instance;

            var where = new List<KeyValuePair<String, String>>();
            where.Add(new KeyValuePair<string, string>("description", obj.Name));
            var educations = Get(0, where);

            var insertData = new List<KeyValuePair<String, String>>();
            insertData.Add(new KeyValuePair<string, string>("description", obj.Name));

            resultId = educations.Length == 0 ? _dbHandler.Insert("Education", insertData) : educations[0].EducationId;

            return resultId;
        }

        public override int Delete(int id, List<KeyValuePair<string, string>> @where = null)
        {
            throw new NotImplementedException();
        }

        public override int Update(Education obj)
        {
            throw new NotImplementedException();
        }

        protected override Education ResultToObject(DataRow data)
        {
            var education = new Education();

            education.EducationId = Convert.ToInt32(data["education_ID"].ToString());
            education.Name = data["description"].ToString();

            return education;
        }
    }
}