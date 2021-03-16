using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using System.Web.Security;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class TicketQueueItemRepository
    {
        private TicketQueueItemDC db = new TicketQueueItemDC(Settings.getConnectionString());

        //List of All Items - Sortable
		public CWTPaginatedList<spDesktopDataAdmin_SelectTicketQueueGroupTicketQueueItems_v1Result> GetTicketQueueGroupTicketQueueItems(int ticketQueueGroupId, int page, string sortField, int sortOrder)
        {
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectTicketQueueGroupTicketQueueItems_v1(ticketQueueGroupId, sortField, sortOrder, page, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectTicketQueueGroupTicketQueueItems_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
        }

        //Get one Item
        public TicketQueueItem GetItem(int ticketQueueItemId)
        {
            return db.TicketQueueItems.SingleOrDefault(c => c.TicketQueueItemId == ticketQueueItemId);
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(TicketQueueItem ticketQueueItem)
        {
            TicketTypeRepository ticketTypeRepository = new TicketTypeRepository();
            TicketType ticketType = new TicketType();
            ticketType = ticketTypeRepository.GetTicketType(ticketQueueItem.TicketTypeId);
            if (ticketType != null)
            {
                ticketQueueItem.TicketTypeDescription = ticketType.TicketTypeDescription;
            }

            TicketQueueGroupRepository ticketQueueGroupRepository = new TicketQueueGroupRepository();
            TicketQueueGroup ticketQueueGroup = new TicketQueueGroup();
            ticketQueueGroup = ticketQueueGroupRepository.GetGroup(ticketQueueItem.TicketQueueGroupId);
            if (ticketQueueGroup != null)
            {
                ticketQueueItem.TicketQueueGroupName = ticketQueueGroup.TicketQueueGroupName;
            }

            GDSRepository gdsRepository = new GDSRepository();
            GDS gds = new GDS();
            gds = gdsRepository.GetGDS(ticketQueueItem.GDSCode);
            if (gds != null)
            {
                ticketQueueItem.GDSName = gds.GDSName;
            }
        }

        //Add to DB
        public void Add(TicketQueueItem ticketQueueItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertTicketQueueItem_v1(
             ticketQueueItem.TicketQueueGroupId,
             ticketQueueItem.TicketQueueItemDescription,
                ticketQueueItem.PseudoCityOrOfficeId,
                ticketQueueItem.GDSCode,
                ticketQueueItem.QueueNumber,
                ticketQueueItem.QueueCategory,
                ticketQueueItem.TicketTypeId,
				ticketQueueItem.TicketingFieldRemark,
				ticketQueueItem.TicketingCommand,
				adminUserGuid
            );
        }

        //Edit in DB
		public void Edit(TicketQueueItem ticketQueueItem)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateTicketQueueItem_v1(
				ticketQueueItem.TicketQueueItemId,
				ticketQueueItem.TicketQueueItemDescription,
				ticketQueueItem.PseudoCityOrOfficeId,
				ticketQueueItem.QueueNumber,
				ticketQueueItem.QueueCategory,
				ticketQueueItem.TicketTypeId,
				ticketQueueItem.TicketingFieldRemark,
				ticketQueueItem.TicketingCommand,
				ticketQueueItem.GDSCode,
				adminUserGuid,
				ticketQueueItem.VersionNumber
			);
		}        

        //Delete From DB
        public void Delete(TicketQueueItem ticketQueueItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteTicketQueueItem_v1(
                ticketQueueItem.TicketQueueItemId,
                adminUserGuid,
                ticketQueueItem.VersionNumber);
        }
    
    }
}
