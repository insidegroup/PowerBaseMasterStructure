using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class ClientSubUnitTelephonyRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of ClientSubUnitTelephony Items - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitTelephony_v1Result> PageClientSubUnitTelephonies(int page, string id, string sortField, int? sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectClientSubUnitTelephony_v1(id, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitTelephony_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        
        //Get one Item
        public ClientSubUnitTelephony GetClientSubUnitTelephony(string cientSubUnitGuid, string dialedNumber)
        {
            return db.ClientSubUnitTelephonies.Where(c => (c.ClientSubUnitGuid == cientSubUnitGuid && c.DialedNumber == dialedNumber)).FirstOrDefault();
        }

        //Get all items for a ClientSubUnit
        public List<ClientSubUnitTelephony> GetClientSubUnitTelephonies(string cientSubUnitGuid)
        {
            return db.ClientSubUnitTelephonies.Where(c => (c.ClientSubUnitGuid == cientSubUnitGuid)).ToList();
        }
        //Add to DB
        public void Add(ClientSubUnitTelephony clientSubUnitTelephony)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertClientSubUnitTelephony_v1(
                clientSubUnitTelephony.ClientSubUnitGuid,
                clientSubUnitTelephony.DialedNumber,
                clientSubUnitTelephony.CallerEnteredDigitDefinitionTypeId,
                adminUserGuid
            );
        }

        //Update in DB
        public void Update(ClientSubUnitTelephony clientSubUnitTelephony, string originalDialledNumber)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateClientSubUnitTelephony_v1(
                clientSubUnitTelephony.ClientSubUnitGuid,
                clientSubUnitTelephony.DialedNumber,
                originalDialledNumber,
                clientSubUnitTelephony.CallerEnteredDigitDefinitionTypeId,
                adminUserGuid,
                clientSubUnitTelephony.VersionNumber
            );
        }

        //Delete From DB
        public void Delete(ClientSubUnitTelephony clientSubUnitTelephony)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteClientSubUnitTelephony_v1(
                clientSubUnitTelephony.ClientSubUnitGuid,
                clientSubUnitTelephony.DialedNumber,
                adminUserGuid,
                clientSubUnitTelephony.VersionNumber
            );
        }
    }
}