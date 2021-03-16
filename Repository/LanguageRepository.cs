using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class LanguageRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public List<Language> GetAllLanguages()
        {
            var result = from n in db.spDesktopDataAdmin_SelectLanguages_v1()
                         select
                         new Language
                         {
                             LanguageCode = n.LanguageCode,
                             LanguageName = n.LanguageName
                         };
            return result.ToList();

        }

        public Language GetLanguage(string languageCode)
        {
            return (from n in db.spDesktopDataAdmin_SelectLanguage_v1(languageCode)
                    select
                    new Language
                    {
                        LanguageCode = n.LanguageCode,
                        LanguageName = n.LanguageName
                    }).FirstOrDefault();
        }
    }
}
