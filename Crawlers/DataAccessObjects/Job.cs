using System;
using DataAccessObjects;

namespace DataAccessObjects
{
    /// <summary>
    /// Job Properties
    /// </summary>
    public class Job
    {
        public int JobID { get; set; }

        public Company Company { get; set; }

        private String _jobTitle;

        public String JobTitle
        {
            get { return _jobTitle; }
            set { _jobTitle = value.Trim(); }
        }

        private String _jobDescription;

        public String JobDescription
        {
            get { return _jobDescription; }
            set { _jobDescription = value.Trim(); }
        }

        public DateTime JobPlaceDate { get; set; }

        public DateTime JobEndDate { get; set; }

        private String _jobHours;

        public String JobHours
        {
            get { return _jobHours; }
            set { _jobHours = value.Trim(); }
        }

        private String _jobEducation;

        public String JobEducation
        {
            get { return _jobEducation; }
            set { _jobEducation = value.Trim(); }
        }
    }
}
