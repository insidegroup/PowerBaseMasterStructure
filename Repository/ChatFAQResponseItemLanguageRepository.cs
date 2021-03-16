using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class ChatFAQResponseItemLanguageRepository
    {
        private ChatFAQResponseGroupDC db = new ChatFAQResponseGroupDC(Settings.getConnectionString());

        //Get a Page of Cities
        public CWTPaginatedList<spDesktopDataAdmin_SelectChatFAQResponseItemLanguages_v1Result> PageChatFAQResponseItemLanguages(int chatFAQResponseItemId, int page, string sortField, int sortOrder)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectChatFAQResponseItemLanguages_v1(chatFAQResponseItemId, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectChatFAQResponseItemLanguages_v1Result>(result, page, Convert.ToInt32(totalRecords));
            return paginatedView;
        }

        //Get one Item
        public ChatFAQResponseItemLanguage GetItem(int chatFAQResponseItemId, string languageCode)
        {
            return db.ChatFAQResponseItemLanguages.SingleOrDefault(c => (c.ChatFAQResponseItemId == chatFAQResponseItemId) && (c.LanguageCode == languageCode));
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(ChatFAQResponseItemLanguage chatFAQResponseItemLanguage)
        {
            //Add LanguageName
            if (chatFAQResponseItemLanguage.LanguageCode != null)
            {
                LanguageRepository languageRepository = new LanguageRepository();
                Language language = new Language();
                language = languageRepository.GetLanguage(chatFAQResponseItemLanguage.LanguageCode);
                if (language != null)
                {
                    chatFAQResponseItemLanguage.LanguageName = language.LanguageName;
                }
            }
        }

        //Languages not used by this policyAirVendorGroupItem
        public List<Language> GetUnUsedLanguages(int chatFAQResponseItemId)
        {
            var result = from n in db.spDesktopDataAdmin_SelectChatFAQResponseItemAvailableLanguages_v1(chatFAQResponseItemId)
                         select new Language
                         {
                             LanguageName = n.LanguageName,
                             LanguageCode = n.LanguageCode
                         };
            return result.ToList();
        }

        //Add to DB
        public void Add(ChatFAQResponseItemLanguage chatFAQResponseItemLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertChatFAQResponseItemLanguage_v1(
                chatFAQResponseItemLanguage.ChatFAQResponseItemId,
                chatFAQResponseItemLanguage.LanguageCode,
                chatFAQResponseItemLanguage.ChatFAQResponseItemLanguageDescription,
                adminUserGuid
                );

        }

        //Change the deleted status on an item
        public void Update(ChatFAQResponseItemLanguage chatFAQResponseItemLanguage)
        {

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdateChatFAQResponseItemLanguage_v1(
                chatFAQResponseItemLanguage.ChatFAQResponseItemId,
                chatFAQResponseItemLanguage.LanguageCode,
                chatFAQResponseItemLanguage.ChatFAQResponseItemLanguageDescription,
                adminUserGuid,
                chatFAQResponseItemLanguage.VersionNumber
                );

        }

        //Delete From DB
        public void Delete(ChatFAQResponseItemLanguage chatFAQResponseItemLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteChatFAQResponseItemLanguage_v1(
               chatFAQResponseItemLanguage.ChatFAQResponseItemId,
               chatFAQResponseItemLanguage.LanguageCode,
               adminUserGuid,
               chatFAQResponseItemLanguage.VersionNumber
               );
        }

    }
}
