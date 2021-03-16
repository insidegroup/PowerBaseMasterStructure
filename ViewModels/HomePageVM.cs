using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class HomePageVM : CWTBaseViewModel
    {
        public IEnumerable<SelectListItem> MappingQualityCodes { get; set; }
        public GenericSelectListVM ConnectionSelectList { get; set; }
        public SelectList UserNewSelectList { get; set; }
        public List<spDesktopDataAdmin_SelectSystemUserUserProfiles_v1Result> UserProfileList { get; set; }
        public string UserProfileIdentifier { get; set; }
        public string SystemUserGuid { get; set; }
        
        
        public HomePageVM()
        {
        }
        public HomePageVM(
            IEnumerable<SelectListItem> mappingQualityCodes,
            GenericSelectListVM connectionSelectList,
            SelectList userSelectList,
            List<spDesktopDataAdmin_SelectSystemUserUserProfiles_v1Result> userProfileList,
            string userProfileIdentifier,
            string systemUserGuid           
            )
        {
            MappingQualityCodes = mappingQualityCodes;
            ConnectionSelectList = connectionSelectList;
            UserNewSelectList = userSelectList;
            UserProfileList = userProfileList;
            UserProfileIdentifier = userProfileIdentifier;
            SystemUserGuid = systemUserGuid;
        }

       /* private SelectListItem GetProfileSelectList(List<spDesktopDataAdmin_SelectSystemUserUserProfiles_v1Result> userProfileListVM)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];


            SelectListItem selectList =
            from c in userProfileListVM : CWTBaseViewModel
            select new SelectListItem
            {
                Selected = (c.SystemUserGuid == adminUserGuid),
                Text = c.UserProfileIdentifier,
                Value = c.UserProfileIdentifier,
            };

            return selectList;
        }*/
    }

}
