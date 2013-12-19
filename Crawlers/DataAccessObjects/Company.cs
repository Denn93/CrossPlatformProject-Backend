using System;
using System.Globalization;
using System.Threading;

namespace DataAccessObjects
{
    /// <summary>
    /// Company Properties
    /// </summary>
    public class Company
    {
        public int CompanyID { get; set; }

        private String _companyName;

        public String CompanyName
        {
            get { return _companyName; }
            set
            {
                TextInfo textInfo = Thread.CurrentThread.CurrentCulture.TextInfo;
                _companyName = (textInfo.ToTitleCase(value.Replace('-', ' '))).Trim();
            }
        }

        private String _companyDescription;

        public String CompanyDescription
        {
            get { return _companyDescription;  }
            set { _companyDescription = value.Trim(); }
        }

        public DateTime CompanyDate { get; set; }

        private String _companyEmail; 

        public String CompanyEmail 
        {
            get { return _companyEmail; }
            set { _companyEmail = value.Trim(); }
        }

        private String _companyCity;

        public String CompanyCity 
        {
            get { return _companyCity; }
            set { _companyCity = value.Trim(); }
        }
    }
}
