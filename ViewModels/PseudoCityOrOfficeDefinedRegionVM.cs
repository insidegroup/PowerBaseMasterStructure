using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
	public class PseudoCityOrOfficeDefinedRegionVM : CWTBaseViewModel
    {
        public PseudoCityOrOfficeDefinedRegion PseudoCityOrOfficeDefinedRegion { get; set; }
		public IEnumerable<SelectListItem> GlobalRegions { get; set; }

		public bool AllowDelete { get; set; }
		public List<PseudoCityOrOfficeDefinedRegionReference> PseudoCityOrOfficeDefinedRegionReferences { get; set; }

		public PseudoCityOrOfficeDefinedRegionVM()
		{
		}

		public PseudoCityOrOfficeDefinedRegionVM(PseudoCityOrOfficeDefinedRegion pseudoCityOrOfficeDefinedRegion, List<PseudoCityOrOfficeDefinedRegionReference> pseudoCityOrOfficeDefinedRegionReferences)
		{
			PseudoCityOrOfficeDefinedRegion = pseudoCityOrOfficeDefinedRegion;
			PseudoCityOrOfficeDefinedRegionReferences = pseudoCityOrOfficeDefinedRegionReferences;
		}
    }	
}