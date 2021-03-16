using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class QueueMinderItemRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of ControlValues - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectQueueMinderItems_v1Result> PageQueueMinderItems(int queueMinderGroupId, int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectQueueMinderItems_v1(queueMinderGroupId, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectQueueMinderItems_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one Item
        public QueueMinderItem GetItem(int queueMinderItemId)
        {
            return db.QueueMinderItems.SingleOrDefault(c => (c.QueueMinderItemId == queueMinderItemId));
        }

        //Add
        public void Add(QueueMinderItem queueMinderItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertQueueMinderItem_v1(
                queueMinderItem.QueueMinderGroupId,
                queueMinderItem.QueueMinderItemDescription,
                queueMinderItem.QueueMinderTypeId,
                queueMinderItem.PseudoCityOrOfficeId,
                queueMinderItem.QueueNumber,
                queueMinderItem.QueueCategory,
                queueMinderItem.GDSCode,
                queueMinderItem.ContextId,
                queueMinderItem.PrefactoryCode,
                adminUserGuid
            );
        }

        //Edit
        public void Edit(QueueMinderItem queueMinderItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateQueueMinderItem_v1(
                queueMinderItem.QueueMinderItemId,
                queueMinderItem.QueueMinderItemDescription,
                queueMinderItem.QueueMinderTypeId,
                queueMinderItem.PseudoCityOrOfficeId,
                queueMinderItem.QueueNumber,
                queueMinderItem.QueueCategory,
                queueMinderItem.GDSCode,
                queueMinderItem.ContextId,
                queueMinderItem.PrefactoryCode,
                adminUserGuid,
                queueMinderItem.VersionNumber
            );
        }

        //Delete
        public void Delete(QueueMinderItem queueMinderItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteQueueMinderItem_v1(
                queueMinderItem.QueueMinderItemId,
                adminUserGuid,
                queueMinderItem.VersionNumber
            );
        }

    }
}
