using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
	public class PseudoCityOrOfficeAddressVM : CWTBaseViewModel
    {
        public PseudoCityOrOfficeAddress PseudoCityOrOfficeAddress { get; set; }
		public IEnumerable<SelectListItem> Countries { get; set; }
		public IEnumerable<SelectListItem> StateProvinces { get; set; }

		public bool AllowDelete { get; set; }
		public List<PseudoCityOrOfficeAddressReference> PseudoCityOrOfficeAddressReferences { get; set; }

		public PseudoCityOrOfficeAddressVM()
		{
		}

		public PseudoCityOrOfficeAddressVM(PseudoCityOrOfficeAddress pseudoCityOrOfficeAddress, List<PseudoCityOrOfficeAddressReference> pseudoCityOrOfficeAddressReferences)
		{
			PseudoCityOrOfficeAddress = pseudoCityOrOfficeAddress;
			PseudoCityOrOfficeAddressReferences = pseudoCityOrOfficeAddressReferences;
		}
    }	
}