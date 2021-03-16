using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
	public class ThirdPartyUserGDSAccessRightVM : CWTBaseViewModel
    {
		public ThirdPartyUserGDSAccessRight ThirdPartyUserGDSAccessRight { get; set; }
		public IEnumerable<SelectListItem> GDSs { get; set; }
		public IEnumerable<SelectListItem> GDSAccessTypes { get; set; }
		public IEnumerable<SelectListItem> PseudoCityOrOfficeIds { get; set; }

		public List<Entitlement> Entitlements { get; set; }

		public ThirdPartyUser ThirdPartyUser { get; set; }

		public ThirdPartyUserGDSAccessRightVM()
		{
		}

		public ThirdPartyUserGDSAccessRightVM(ThirdPartyUserGDSAccessRight systemUserGDSAccessRight)
		{
			ThirdPartyUserGDSAccessRight = systemUserGDSAccessRight;
		}
	}
}