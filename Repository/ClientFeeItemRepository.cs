using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class ClientFeeItemRepository
    {

        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of ClientFees - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientFeeGroupClientFeeItems_v1Result> PageClientFeeItems(int clientFeeGroupId, int page, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectClientFeeGroupClientFeeItems_v1(clientFeeGroupId, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientFeeGroupClientFeeItems_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one Item
        public ClientFeeItem GetItem(int clientFeeItemId)
        {
            return db.ClientFeeItems.SingleOrDefault(c => (c.ClientFeeItemId == clientFeeItemId));
        }

        //Add to DB
        public void Add(ClientFeeItem clientFeeItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertClientFeeItem_v1(
                clientFeeItem.ClientFeeGroupId,
                clientFeeItem.ClientFeeId,
                clientFeeItem.ValueAmount,
                clientFeeItem.ValuePercentage,
                adminUserGuid
            );


        }

        //Update in DB
         public void Update(ClientFeeItem clientFeeItem)
         {
             string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

             db.spDesktopDataAdmin_UpdateClientFeeItem_v1(
                 clientFeeItem.ClientFeeItemId,
                 clientFeeItem.ClientFeeId,
                 clientFeeItem.ValueAmount,
                 clientFeeItem.ValuePercentage,
                 adminUserGuid,
                 clientFeeItem.VersionNumber                
             );

         }

         //Delete From DB
         public void Delete(ClientFeeItem clientFeeItem)
         {
             string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

             db.spDesktopDataAdmin_DeleteClientFeeItem_v1(
                 clientFeeItem.ClientFeeItemId,
                 adminUserGuid,
                 clientFeeItem.VersionNumber
             );
         }
    }
}
        