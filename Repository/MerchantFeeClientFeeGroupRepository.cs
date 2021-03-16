using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq;
using System.Collections.Generic;
using System.Web;

namespace CWTDesktopDatabase.Repository
{
    public class MerchantFeeClientFeeGroupRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of MerchantFeeClientFeeGroups - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientFeeGroupMerchantFees_v1Result> PageMerchantFeeClientFeeGroups(int clientFeeGroupId, int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectClientFeeGroupMerchantFees_v1(clientFeeGroupId, filter, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientFeeGroupMerchantFees_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //MerchantFees available for attaching to a ClientFeeGroup
        public List<MerchantFee> GetUnUsedMerchantFees(int clientFeeGroupId, int? merchantFeeId)
         {

             var result = from n in db.spDesktopDataAdmin_SelectClientFeeGroupAvailableMerchantFees_v1(clientFeeGroupId, merchantFeeId)
                          select new MerchantFee
                          {
                              MerchantFeeId = n.MerchantFeeId,
                              MerchantFeeDescription = CWTStringHelpers.TrimString(n.MerchantFeeDescription,40)
                          };
             return result.ToList();
         }

        //Get a single MerchantFeeClientFeeGroup 
        public MerchantFeeClientFeeGroup GetItem(int clientFeeGroupId, int merchantFeeId)
        {
            return db.MerchantFeeClientFeeGroups.SingleOrDefault(c =>
                (c.ClientFeeGroupId == clientFeeGroupId && c.MerchantFeeId == merchantFeeId)
                );
        }
        
        //Add MerchantFeeClientFeeGroup
        public void Add(MerchantFeeClientFeeGroup merchantFeeClientFeeGroup)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertMerchantFeeClientFeeGroup_v1(
                merchantFeeClientFeeGroup.ClientFeeGroupId,
                merchantFeeClientFeeGroup.MerchantFeeId,
                adminUserGuid
            );
        }

        //Update MerchantFeeClientFeeGroup
        public void Update(MerchantFeeClientFeeGroup merchantFeeClientFeeGroup, int originalMerchantFeeId)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateMerchantFeeClientFeeGroup_v1(
                merchantFeeClientFeeGroup.ClientFeeGroupId,
                merchantFeeClientFeeGroup.MerchantFeeId, 
                originalMerchantFeeId,
                adminUserGuid,
                merchantFeeClientFeeGroup.VersionNumber
            );
        }

        //Delete MerchantFeeClientFeeGroup From DB
        public void Delete(MerchantFeeClientFeeGroup merchantFeeClientFeeGroup)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteMerchantFeeClientFeeGroup_v1(
                merchantFeeClientFeeGroup.ClientFeeGroupId,
                merchantFeeClientFeeGroup.MerchantFeeId,
                merchantFeeClientFeeGroup.VersionNumber,
                adminUserGuid
            );
        }
    }
}