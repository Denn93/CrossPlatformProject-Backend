using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawlers
{
    class Job
    {
        /// <summary>
        /// Properties for the Job class. All private variables with public getters and setters
        /// </summary>

        private int jobID;

        public int JobID
        {
            get { return jobID; }
            set { jobID = value; }
        }

        private Company company;

        public Company Company
        {
            get { return company; }
            set { company = value; }
        }

        private String jobTitle;

        public String JobTitle
        {
            get { return jobTitle; }
            set { jobTitle = value; }
        }

        private String jobDescription;

        public String JobDescription
        {
            get { return jobDescription; }
            set { jobDescription = value; }
        }

        private DateTime jobPlaceDate;

        public DateTime JobPlaceDate
        {
            get { return jobPlaceDate; }
            set { jobPlaceDate = value; }
        }

        private DateTime jobEndDate;

        public DateTime JobEndDate
        {
            get { return jobEndDate; }
            set { jobEndDate = value; }
        }
    }
}
