using System;
using DataAccessObjects;

namespace CrawlerBatch.Mappers
{
    public class CompanyMapper : AMapper<Company, MatchyBackEnd.Company>
    {
        public override int Insert(Company obj)
        {
            int insertId = MatchyBackend.AddCompany(MapToService(obj));

            if (insertId == 0)
                Console.WriteLine("Company not inserted. Error, Error");

            return insertId;
        }

        public override Company[] Get(int id = 0)
        {
            throw new NotImplementedException();
        }

        public override MatchyBackEnd.Company MapToService(Company company)
        {
            return new MatchyBackEnd.Company()
                       {
                           CompanyCity = company.CompanyCity,
                           CompanyDate = company.CompanyDate,
                           CompanyDescription = company.CompanyDescription,
                           CompanyEmail = company.CompanyEmail,
                           CompanyID = company.CompanyID,
                           CompanyName = company.CompanyName,
                           CompanyTel = company.CompanyTel
                       };
        }

        public override Company MapFromService(MatchyBackEnd.Company company)
        {
            return new Company
                {
                    CompanyCity = company.CompanyCity,
                    CompanyDate = company.CompanyDate,
                    CompanyDescription = company.CompanyDescription,
                    CompanyEmail = company.CompanyEmail,
                    CompanyID = company.CompanyID,
                    CompanyName = company.CompanyName,
                    CompanyTel = company.CompanyTel
                };
        }
    }
}
