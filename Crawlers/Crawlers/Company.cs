using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawlers
{
    class Company
    {
        /// <summary>
        /// Properties for the Company class. All private variables with public getters and setters
        /// </summary>
        
        private int companyID;
        
        public int CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }

        private String companyName;

        public String CompanyName
        {
            get { return companyName; }
            set { companyName = value; }
        }

        private String companyDescription;

        public String CompanyDescription
        {
            get { return companyDescription; }
            set { companyDescription = value; }
        }

        private DateTime companyDate;

        public DateTime CompanyDate
        {
            get { return companyDate; }
            set { companyDate = value; }
        }

        private String companyEmail;

        public String CompanyEmail
        {
            get { return companyEmail; }
            set { companyEmail = value; }
        }
    }
}
