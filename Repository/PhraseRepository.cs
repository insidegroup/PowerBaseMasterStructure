using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class PhraseRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of Phrases
        public CWTPaginatedList<spDesktopDataAdmin_SelectPhrases_v1Result> PagePhrases(int page, string filter, string sortField, int sortOrder)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectPhrases_v1(filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPhrases_v1Result>(result, page, Convert.ToInt32(totalRecords));
            return paginatedView;
        }

        public IQueryable<Phrase> GetAllPhrases()
        {
            return db.Phrases.OrderBy(c => c.PhraseName);
        }

        public Phrase GetPhrase(int phraseId)
        {
            return db.Phrases.SingleOrDefault(c => c.PhraseId == phraseId);
        }

    }
}
