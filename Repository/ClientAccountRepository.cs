using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Web.Security;
using System.Web.Caching;

namespace CWTDesktopDatabase.Repository
{
    public class ClientAccountRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get one ClientAccount
        public ClientAccount GetClientAccount(string clientAccountNumber, string sourceSystemCode)
        {
            return db.ClientAccounts.SingleOrDefault(c => (c.ClientAccountNumber == clientAccountNumber && c.SourceSystemCode == sourceSystemCode));
        }

        //Get ClientAccount's ClientTopUnits
        public List<ClientTopUnit> GetClientAccountClientTopUnits(string clientAccountNumber, string sourceSystemCode)
        {
            var result = from n in db.spDesktopDataAdmin_SelectClientAccountClientTopUnits_v1(sourceSystemCode, clientAccountNumber).OrderBy(c => c.ClientTopUnitName)
                         select new ClientTopUnit
                         {
                             ClientTopUnitName = n.ClientTopUnitName.Trim(),
                             ClientTopUnitGuid = n.ClientTopUnitGuid.Trim()
                         };
            return result.ToList();
        }

        //Add Data From Linked Tables for Display of ClientAccount
        public void EditForDisplay(ClientAccount clientAccount)
        {

            CountryRepository countryRepository = new CountryRepository();
            Country country = new Country();
            country = countryRepository.GetCountry(clientAccount.CountryCode);
            if (country != null)
            {
                clientAccount.CountryName = country.CountryName;
            }
        }

        //Add ClientAccount to DB
        public void Add(ClientAccount clientAccount)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertClientAccount_v1(
                clientAccount.ClientAccountNumber,
                clientAccount.ClientAccountName,
                clientAccount.SourceSystemCode,
                clientAccount.GloryAccountName,
                clientAccount.ClientMasterCode,
                clientAccount.EffectiveDate,
                clientAccount.CountryCode,
                adminUserGuid
            );

        }

        //Update ClientAccount in DB
        public void Update(ClientAccount clientAccount)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateClientAccount_v1(
                clientAccount.SourceSystemCode,
                clientAccount.ClientAccountNumber,
                clientAccount.ClientAccountName,
                clientAccount.GloryAccountName,
                clientAccount.ClientMasterCode,
                clientAccount.EffectiveDate,
                clientAccount.CountryCode,
                adminUserGuid,
                clientAccount.VersionNumber
            );

        }

        //Delete ClientAccount From DB
        /*public void Delete(ClientAccount clientAccount)
        {
            db.spDesktopDataAdmin_DeleteClientAccount_v1(
                clientAccount.SourceSystemCode,
                clientAccount.ClientAccountNumber,
                clientAccount.VersionNumber
            );
        }*/

    }
}
