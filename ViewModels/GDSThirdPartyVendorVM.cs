using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
	public class GDSThirdPartyVendorVM : CWTBaseViewModel
    {
        public GDSThirdPartyVendor GDSThirdPartyVendor { get; set; }

		public bool AllowDelete { get; set; }
		public List<GDSThirdPartyVendorReference> GDSThirdPartyVendorReferences { get; set; }

		public GDSThirdPartyVendorVM()
		{
		}

		public GDSThirdPartyVendorVM(GDSThirdPartyVendor thirdPartyVendor, List<GDSThirdPartyVendorReference> thirdPartyVendorReferences)
		{
			GDSThirdPartyVendor = thirdPartyVendor;
			GDSThirdPartyVendorReferences = thirdPartyVendorReferences;
		}
    }	
}