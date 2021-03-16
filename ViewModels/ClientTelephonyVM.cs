using System.Collections.Generic;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
	public class ClientTelephonyVM : CWTBaseViewModel
	{
		public ClientTelephony ClientTelephony { get; set; }

		public IEnumerable<SelectListItem> Countries { get; set; }
		public IEnumerable<SelectListItem> HierarchyTypes { get; set; }
		public IEnumerable<SelectListItem> TelephoneTypes { get; set; }
		public IEnumerable<SelectListItem> TravelerBackOfficeTypes { get; set; }
		public IEnumerable<SelectListItem> CallerEnteredDigitDefinitionTypes { get; set; }

		public ClientTelephonyVM()
		{
		}

		public ClientTelephonyVM(ClientTelephony clientTelephony)
		{
			ClientTelephony = clientTelephony;
		}
	}
}