using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Collections.Generic;

namespace CWTDesktopDatabase.Repository
{
    public class ApprovalGroupApprovalTypeRepository
    {
        //DataContext
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get All ApprovalGroupApprovalTypes2 - for SelectLists
        public IQueryable<ApprovalGroupApprovalType> GetAllApprovalGroupApprovalTypes()
        {
			return db.ApprovalGroupApprovalTypes.OrderBy(c => c.ApprovalGroupApprovalTypeDescription);
        }

        //Get a Single ApprovalGroupApprovalType
        public ApprovalGroupApprovalType GetApprovalGroupApprovalType(int approvalGroupApprovalTypeId)
        {
            return db.ApprovalGroupApprovalTypes.SingleOrDefault(c => c.ApprovalGroupApprovalTypeId == approvalGroupApprovalTypeId);
        }

        //Get a Page of ApprovalGroupApprovalTypes2 - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectApprovalGroupApprovalTypes_v1Result> PageApprovalGroupApprovalTypes(int page)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectApprovalGroupApprovalTypes_v1(page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectApprovalGroupApprovalTypes_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get ApprovalGroupApprovalTypeReferences
        public List<ApprovalGroupApprovalTypeReference> GetApprovalGroupApprovalTypeReferences(int approvalGroupApprovalTypeId)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = from n in db.spDesktopDataAdmin_SelectApprovalGroupApprovalTypeReferences_v1(approvalGroupApprovalTypeId, adminUserGuid)
                         select
                             new ApprovalGroupApprovalTypeReference
                             {
                                 TableName = n.TableName.ToString()
                             };

            return result.ToList();
        }

        //Add to DB
        public void Add(ApprovalGroupApprovalType approvalGroupApprovalType)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertApprovalGroupApprovalType_v1(
               approvalGroupApprovalType.ApprovalGroupApprovalTypeId,
               approvalGroupApprovalType.ApprovalGroupApprovalTypeDescription,
                adminUserGuid
            );
        }

        //Update in DB
        public void Update(ApprovalGroupApprovalType approvalGroupApprovalType)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateApprovalGroupApprovalType_v1(
                approvalGroupApprovalType.ApprovalGroupApprovalTypeId,
                approvalGroupApprovalType.NewApprovalGroupApprovalTypeId,
                approvalGroupApprovalType.ApprovalGroupApprovalTypeDescription,
                adminUserGuid,
                approvalGroupApprovalType.VersionNumber
            );
        }

        //Delete From DB
        public void Delete(ApprovalGroupApprovalType approvalGroupApprovalType)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteApprovalGroupApprovalType_v1(
                approvalGroupApprovalType.ApprovalGroupApprovalTypeId,
                adminUserGuid,
                approvalGroupApprovalType.VersionNumber
            );
        }
    }
}