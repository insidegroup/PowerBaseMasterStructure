using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class ClientAccountAddressRepository
    {
        private ClientDetailDC db = new ClientDetailDC(Settings.getConnectionString());

        //Get a Page of ClientAccountAddresses
        /*public CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailAddresses_v1Result> ListClientAccountAddresses(string clientAccountNumber, string sourceSystemCode, int page)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectClientDetailAddresses_v1(sourceSystemCode, clientAccountNumber, adminUserGuid, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailAddresses_v1>(result, page, Convert.ToInt32(totalRecords));
            return paginatedView;
        }
         * */

        //Add ClientAccountAddress
        public void Add(ClientAccountAddressVM clientAccountAddressVM)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            /*db.spDesktopDataAdmin_InsertClientAccountAddress_v1(
                clientAccountAddressVM.ClientAccount.ClientAccountNumber,
                clientAccountAddressVM.ClientAccount.SourceSystemCode,
                clientAccountAddressVM.Address.FirstAddressLine,
                clientAccountAddressVM.Address.SecondAddressLine,
                clientAccountAddressVM.Address.CityName,
                clientAccountAddressVM.Address.CountyName,
                clientAccountAddressVM.Address.StateProvinceName,
                clientAccountAddressVM.Address.LatitudeDecimal,
                clientAccountAddressVM.Address.LongitudeDecimal,
                clientAccountAddressVM.Address.MappingQualityCode,
                clientAccountAddressVM.Address.PostalCode,
                clientAccountAddressVM.Address.ReplicatedFromClientMaintenanceFlag,
                clientAccountAddressVM.Address.CountryCode,
                adminUserGuid
            );
             * */
        }
    }
}