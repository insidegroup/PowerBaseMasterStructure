using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class ClientSubUnitTravelerTypeRepository
    {
        HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //List of ClientSubUnitTravelerTypes
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitTravelerTypes_v1Result> GetClientSubUnitTravelerTypes(string clientSubUnitGuid, int page)
        {
            //query db
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectClientSubUnitTravelerTypes_v1(clientSubUnitGuid, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return1
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitTravelerTypes_v1Result>(result, page, totalRecords);
            return paginatedView;

        }

        //Get one Item
        public ClientSubUnitTravelerType GetClientSubUnitTravelerType(string clientSubUnitGuid, string travelerTypeGuid)
        {
            return db.ClientSubUnitTravelerTypes.SingleOrDefault(c => (c.ClientSubUnitGuid == clientSubUnitGuid) && (c.TravelerTypeGuid == travelerTypeGuid));
        }

        //Get items by TravelerTypeGuid
        public List<ClientSubUnitTravelerType> GetClientSubUnitTravelerTypes(string travelerTypeGuid)
        {
            return db.ClientSubUnitTravelerTypes.Where(x => x.TravelerTypeGuid == travelerTypeGuid).ToList();
        }
    }
}