using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;
using System.Text;
using System.IO;
using System.Web.UI;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CWTDesktopDatabase.Repository
{
    public class QueueMinderItemLanguageRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of Hotel Advice - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectQueueMinderItemLanguage_v1Result> PageQueueMinderItemLanguages(int queueMinderItemId, int page, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectQueueMinderItemLanguage_v1(queueMinderItemId, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectQueueMinderItemLanguage_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one Item
        public QueueMinderItemLanguage GetItem(int queueMinderItemId, string languageCode)
        {
            return db.QueueMinderItemLanguages.SingleOrDefault(c => (c.QueueMinderItemId == queueMinderItemId)
                    && (c.LanguageCode == languageCode));
        }

        //Languages not used by this queueMinderItem
        public List<Language> GetUnUsedLanguages(int queueMinderItemId)
        {
            var result = from n in db.spDesktopDataAdmin_SelectQueueMinderItemAvailableLanguages_v1(queueMinderItemId)
                         select new Language
                         {
                             LanguageName = n.LanguageName,
                             LanguageCode = n.LanguageCode
                         };
            return result.ToList();
        }

        //Add to DB
        public void Add(QueueMinderItemLanguage queueMinderItemLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertQueueMinderItemLanguage_v1(
                queueMinderItemLanguage.QueueMinderItemId,
                queueMinderItemLanguage.LanguageCode,
                queueMinderItemLanguage.QueueMinderItemLanguageItineraryDescription,
                adminUserGuid
                );

        }

        //Delete From DB
        public void Delete(QueueMinderItemLanguage queueMinderItemLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteQueueMinderItemLanguage_v1(
               queueMinderItemLanguage.QueueMinderItemId,
               queueMinderItemLanguage.LanguageCode,
               adminUserGuid,
               queueMinderItemLanguage.VersionNumber
               );
        }

        //Change the deleted status on an item
        public void Update(QueueMinderItemLanguage queueMinderItemLanguage)
        {

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdateQueueMinderItemLanguage_v1(
                queueMinderItemLanguage.QueueMinderItemId,
                queueMinderItemLanguage.LanguageCode,
                queueMinderItemLanguage.QueueMinderItemLanguageItineraryDescription,
                queueMinderItemLanguage.QueueMinderItemLanguageItineraryDescription,
                adminUserGuid,
                queueMinderItemLanguage.VersionNumber
                );

        }

    }
}

       
 