using System;

namespace DataAccessObjects
{
    public class Cv
    {     
        public int CvID { get; set; }

        private String _title;
        public String Title
        {
            get { return _title; }
            set { _title = value.Trim(); }
        }

        private String _date;
        public String Date
        {
            get { return _date; }
            set { _date = value.Trim(); }
        }

        private String _profession;
        public String Profession
        {
            get { return _profession; }
            set { _profession = value.Trim(); }
        }

        private String _discipline;
        public String Discipline
        {
            get { return _discipline; }
            set { _discipline = value.Trim(); }
        }

        public Education EducationLevel { get; set; }

        private String _province;
        public String Province
        {
            get { return _province; }
            set { _province = value.Trim(); }
        }

        private String _hours;
        public String Hours
        {
            get { return _hours; }
            set { _hours = value.Trim(); }
        }

        private String _city;
        public String City
        {
            get { return _city; }
            set { _city = value.Trim(); }
        }

        private String _sex;
        public String Sex
        {
            get { return _sex; }
            set { _sex = value.Trim(); }
        }

        public int Age { get; set; }

        private String _education;
        public String Education
        {
            get { return _education; }
            set { _education = value.Trim(); }
        }

        private String _workExperience;
        public String WorkExperience
        {
            get { return _workExperience; }
            set { _workExperience = value.Trim(); }
        }
    }
}
