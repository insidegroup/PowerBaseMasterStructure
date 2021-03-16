using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
	public class ServiceAccountGDSAccessRightVM : CWTBaseViewModel
    {
		public ServiceAccountGDSAccessRight ServiceAccountGDSAccessRight { get; set; }
		public IEnumerable<SelectListItem> GDSs { get; set; }
		public IEnumerable<SelectListItem> GDSAccessTypes { get; set; }
		public IEnumerable<SelectListItem> PseudoCityOrOfficeIds { get; set; }

		public ServiceAccount ServiceAccount { get; set; }

		public ServiceAccountGDSAccessRightVM()
		{
		}

		public ServiceAccountGDSAccessRightVM(ServiceAccountGDSAccessRight systemUserGDSAccessRight)
		{
            ServiceAccountGDSAccessRight = systemUserGDSAccessRight;
		}
	}
}