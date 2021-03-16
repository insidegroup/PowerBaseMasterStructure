using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
	public class PseudoCityOrOfficeMaintenanceVM : CWTBaseViewModel
    {
        public PseudoCityOrOfficeMaintenance PseudoCityOrOfficeMaintenance { get; set; }

		public List<string> ClientSubUnitGuids { get; set; }
		public List<ClientSubUnit> ClientSubUnits { get; set; }

		public int[] GDSThirdPartyVendorIds { get; set; }
		public MultiSelectList GDSThirdPartyVendorsList { get; set; }
		public List<GDSThirdPartyVendor> GDSThirdPartyVendors { get; set; }


		//Lists
		public IEnumerable<SelectListItem> IATAs { get; set; }
		public IEnumerable<SelectListItem> GDSs { get; set; }
		public IEnumerable<SelectListItem> PseudoCityOrOfficeAddresses { get; set; }
		public IEnumerable<SelectListItem> PseudoCityOrOfficeDefinedRegions { get; set; }
		public IEnumerable<SelectListItem> ExternalNames { get; set; }
		public IEnumerable<SelectListItem> PseudoCityOrOfficeTypes { get; set; }
		public IEnumerable<SelectListItem> PseudoCityOrOfficeLocationTypes { get; set; }
		public IEnumerable<SelectListItem> FareRedistributions { get; set; }

		public PseudoCityOrOfficeMaintenanceVM()
		{
		}

		public PseudoCityOrOfficeMaintenanceVM(PseudoCityOrOfficeMaintenance pseudoCityOrOfficeMaintenance)
		{
			PseudoCityOrOfficeMaintenance = pseudoCityOrOfficeMaintenance;	
		}
    }	
}