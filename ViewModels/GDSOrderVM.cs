using System.Collections.Generic;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
	public class GDSOrderVM : CWTBaseViewModel
	{
		public GDSOrder GDSOrder { get; set; }

		public IEnumerable<SelectListItem> OrderAnalystCountries { get; set; }
		public IEnumerable<SelectListItem> RequesterCountries { get; set; }

		public IEnumerable<SelectListItem> GDSs { get; set; }
		public IEnumerable<SelectListItem> GDSOrderTypes { get; set; }
		public IEnumerable<SelectListItem> GDSOrderStatuses { get; set; }
		public IEnumerable<SelectListItem> PseudoCityOrOfficeAddresses { get; set; }
		public IEnumerable<SelectListItem> SelectedPseudoCityOrOfficeAddress { get; set; }

		public int[] GDSThirdPartyVendorIds { get; set; }
		public MultiSelectList GDSThirdPartyVendorsList { get; set; }
		public List<GDSThirdPartyVendor> GDSThirdPartyVendors { get; set; }

		public int[] PseudoCityOrOfficeMaintenanceGDSThirdPartyVendorIds { get; set; }
		public MultiSelectList PseudoCityOrOfficeMaintenanceGDSThirdPartyVendorsList { get; set; }
		public List<GDSThirdPartyVendor> PseudoCityOrOfficeMaintenanceGDSThirdPartyVendors { get; set; }

		public IEnumerable<SelectListItem> GDSOrderLineItemActions { get; set; }

		//PseudoCityOrOfficeMaintenance
		public IEnumerable<SelectListItem> IATAs { get; set; }
		public IEnumerable<SelectListItem> PseudoCityOrOfficeDefinedRegions { get; set; }
		public IEnumerable<SelectListItem> ExternalNames { get; set; }
		public IEnumerable<SelectListItem> PseudoCityOrOfficeTypes { get; set; }
		public IEnumerable<SelectListItem> PseudoCityOrOfficeLocationTypes { get; set; }
		public IEnumerable<SelectListItem> FareRedistributions { get; set; }


		public GDSOrderVM()
		{
		}

		public GDSOrderVM(GDSOrder gdsOrder)
		{
			GDSOrder = gdsOrder;
		}
	}
}