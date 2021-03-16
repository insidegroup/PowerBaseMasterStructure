using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
	public class ThirdPartyUserVM : CWTBaseViewModel
    {
        public ThirdPartyUser ThirdPartyUser { get; set; }
		public IEnumerable<SelectListItem> ThirdPartyUserTypes { get; set; }
		public IEnumerable<SelectListItem> GDSThirdPartyVendors { get; set; }
		public IEnumerable<SelectListItem> StateProvinces { get; set; }

		public ThirdPartyUserVM()
		{
		}

		public ThirdPartyUserVM(ThirdPartyUser thirdPartyUser)
		{
			ThirdPartyUser = thirdPartyUser;
		}
	}
}