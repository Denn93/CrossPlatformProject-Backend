using System;
using DataAccessObjects;

namespace CrawlerBatch.Mappers
{
    /// <summary>
    /// CompanyMapper class. Inherits van AMapper class. Met Generic Company.cs en MatchyBackEnd.Company.cs
    /// </summary>
    public class CompanyMapper : AMapper<Company, MatchyBackEnd.Company>
    {
        /// <summary>
        /// Insert Methode. Deze methode zorgt ervoor dat de gecrawlde data naar de webservice gestuurd kan worden voor insert
        /// </summary>
        /// <param name="company"></param>
        /// <returns>Inserted id</returns>
        public override int Insert(Company company)
        {
            int insertId = MatchyBackend.AddCompany(MapToService(company));

            if (insertId == 0)
                Console.WriteLine("Company not inserted. Error, Error");

            return insertId;
        }

        /// <summary>
        /// Deze methode zorgt ervoor dat de data vanuit deze crawler omgezet kan worden naar de datatypes van de backEnd. 
        /// Dus een handmatige serializer. Omdat we een ingebouwde serializer niet werkend kregen. Terwijl de datatypes exact 
        /// overeenkwamen vanwege import library met gebruikte dataobjecten
        /// </summary>
        /// <param name="company">Het data object dat omgezet moet worden</param>
        /// <returns>Het data object van de backEnd</returns>
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

        /// <summary>
        /// Deze methode zorgt ervoor dat de data vanuit de backend omgezet kan worden naar de datatypes van deze crawler. 
        /// Dus een handmatige serializer. Omdat we een ingebouwde serializer niet werkend kregen. Terwijl de datatypes exact 
        /// overeenkwamen vanwege import library met gebruikte dataobjecten
        /// </summary>
        /// <param name="company">Het data object dat omgezet moet worden</param>
        /// <returns>Het data object van deze crawler</returns>
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
