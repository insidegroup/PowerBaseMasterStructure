using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System.Linq.Dynamic;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class ClientDetailAddressRepository
    {
        private ClientDetailDC db = new ClientDetailDC(Settings.getConnectionString());
       
        //Get a Page of Addresses
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailAddresses_v1Result> ListClientDetailAddresses(int clientDetailId, int page)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectClientDetailAddresses_v1(clientDetailId, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailAddresses_v1Result>(result, page, Convert.ToInt32(totalRecords));
            return paginatedView;
        }

        //Add ClientAccountAddress
        public void Add(ClientDetail clientDetail, Address address)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertClientDetailAddress_v1(
                clientDetail.ClientDetailId,
                address.FirstAddressLine,
                address.SecondAddressLine,
                address.CityName,
                address.CountyName,
                address.StateProvinceName,
                address.LatitudeDecimal,
                address.LongitudeDecimal,
                address.MappingQualityCode,
                address.PostalCode,
                address.ReplicatedFromClientMaintenanceFlag,
                address.CountryCode,
                adminUserGuid
            );

        }

      
        //Get one Item from Address
        public ClientDetailAddress GetAddressClientDetail(int addressId)
        {
            return db.ClientDetailAddresses.SingleOrDefault(c => c.AddressId == addressId);
        }

        //Get one Item from ClientDetail
        public ClientDetailAddress GetClientDetailAddress(int clientDetailId)
        {
            return db.ClientDetailAddresses.SingleOrDefault(c => c.ClientDetailId == clientDetailId);
        }
    }
}
