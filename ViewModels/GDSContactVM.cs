using System.Collections.Generic;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
	public class GDSContactVM : CWTBaseViewModel
	{
		public GDSContact GDSContact { get; set; }

		public IEnumerable<SelectListItem> Countries { get; set; }
		public IEnumerable<SelectListItem> GDSs { get; set; }
		public IEnumerable<SelectListItem> GlobalRegions { get; set; }
		public IEnumerable<SelectListItem> PseudoCityOrOfficeBusinesses { get; set; }
		public IEnumerable<SelectListItem> PseudoCityOrOfficeDefinedRegions { get; set; }
		public IEnumerable<SelectListItem> GDSRequestTypes { get; set; }
		
		public GDSContactVM()
		{
		}

		public GDSContactVM(GDSContact gdsContact)
		{
			GDSContact = gdsContact;
		}
	}
}