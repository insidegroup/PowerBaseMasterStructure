using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
	public class SystemUserGDSAccessRightVM : CWTBaseViewModel
    {
		public SystemUserGDSAccessRight SystemUserGDSAccessRight { get; set; }
		public IEnumerable<SelectListItem> GDSs { get; set; }
		public IEnumerable<SelectListItem> GDSAccessTypes { get; set; }
		public IEnumerable<SelectListItem> PseudoCityOrOfficeIds { get; set; }

		public SystemUser SystemUser { get; set; }

		public SystemUserGDSAccessRightVM()
		{
		}

		public SystemUserGDSAccessRightVM(SystemUserGDSAccessRight systemUserGDSAccessRight)
		{
			SystemUserGDSAccessRight = systemUserGDSAccessRight;
		}
	}
}