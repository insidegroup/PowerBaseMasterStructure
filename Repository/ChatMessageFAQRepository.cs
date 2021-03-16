using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class ChatMessageFAQRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of Country Regions - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectChatMessageFAQs_v1Result> PageChatMessageFAQs(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectChatMessageFAQs_v1(filter, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectChatMessageFAQs_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        public IQueryable<ChatMessageFAQ> GetAllChatMessageFAQs()
        {
            return db.ChatMessageFAQs.OrderBy(t => t.ChatMessageFAQName);
        }

        public ChatMessageFAQ GetChatMessageFAQ(int chatMessageFAQId)
        {
            return db.ChatMessageFAQs.SingleOrDefault(c => c.ChatMessageFAQId == chatMessageFAQId);
        }

        public List<ChatMessageFAQ> GetChatFAQResponseItemAvailableChatMessageFAQs(int? chatFAQResponseItemId, int chatFAQResponseGroupId)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = from n in db.spDesktopDataAdmin_SelectChatFAQResponseItemAvailableChatMessageFAQs_v1(chatFAQResponseItemId, chatFAQResponseGroupId)

                         select
                             new ChatMessageFAQ
                             {
                                 ChatMessageFAQId = n.ChatMessageFAQId,
                                 ChatMessageFAQName = n.ChatMessageFAQName
                             };
            return result.ToList();
        }

        //Add Data From Linked Tables for Display
        public void EditForDisplay(ChatMessageFAQ chatMessageFAQ)
        {
        }

		//Add to DB
		public void Add(ChatMessageFAQ chatMessageFAQ)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertChatMessageFAQ_v1(
				chatMessageFAQ.ChatMessageFAQName,
				adminUserGuid
			);
		}

		//Update in DB
		public void Update(ChatMessageFAQ chatMessageFAQ)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateChatMessageFAQ_v1(
				chatMessageFAQ.ChatMessageFAQId,
				chatMessageFAQ.ChatMessageFAQName,
				adminUserGuid,
				chatMessageFAQ.VersionNumber				
			);
		}

		//Delete in DB
		public void Delete(ChatMessageFAQ chatMessageFAQ)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteChatMessageFAQ_v1(
				chatMessageFAQ.ChatMessageFAQId,
				adminUserGuid,
				chatMessageFAQ.VersionNumber
			);
		}

        public List<ChatMessageFAQReference> GetChatMessageFAQReferences(int chatMessageFAQId)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            var result = from n in db.spDesktopDataAdmin_SelectChatMessageFAQReferences_v1(chatMessageFAQId, adminUserGuid)
                         select
                             new ChatMessageFAQReference
                             {
                                 TableName = n.TableName.ToString()
                             };

            return result.ToList();
        }
    }
}
