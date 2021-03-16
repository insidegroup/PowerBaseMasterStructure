using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class TravelPortTypeRepository
    {
        private TravelPortTypeDC db = new TravelPortTypeDC(Settings.getConnectionString());
        public IQueryable<TravelPortType> GetAllTravelPortTypes()
        {
            return db.TravelPortTypes.OrderBy(t => t.TravelPortTypeDescription);
        }

        public TravelPortType GetTravelPortType(int travelPortTypeId)
        {
            return db.TravelPortTypes.SingleOrDefault(c => c.TravelPortTypeId == travelPortTypeId);
        }

        public TravelPortType GetTravelPortTypeByDescription(string travelPortTypeDescription)
        {
            return db.TravelPortTypes.SingleOrDefault(c => c.TravelPortTypeDescription == travelPortTypeDescription);
        }


        public List<Language> LookUpAvailableLanguages(int travelPortTypeId, string travelPortCode)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = from n in db.spDesktopDataAdmin_SelectTravelPortLanguageAvailableLanguages_v1(travelPortTypeId, travelPortCode)

                         select
                             new Language
                             {
                                 LanguageCode = n.LanguageCode,
                                 LanguageName = n.LanguageName
                             };
            return result.ToList();
        }
    }
}
