using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawlers
{
    class Job
    {
        public int JobID { get; set; }

        public Company Company { get; set; }

        public string JobTitle { get; set; }

        public string JobDescription { get; set; }

        public DateTime JobPlaceDate { get; set; }

        public DateTime JobEndDate { get; set; }

        public String JobHours { get; set; }

        public String JobEducation { get; set; }
    }
}
