using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Models
{
	public partial class ClientSubUnit : CWTBaseModel
    {
        public string ClientTopUnitName { get; set; }
        public string CountryName { get; set; }

		public LineOfBusiness LineOfBusiness { get; set; }
		public int LineOfBusinessId { get; set; }

		//ClientSubUnit Attribute Types
		public bool RestrictedClient { get; set; }
		public bool PrivateClient { get; set; }
		public bool CubaBookingAllowed { get; set; }
		public bool InCountryServiceOnly { get; set; }
		public string DialledNumber24HSC { get; set; }
		public string BranchContactNumber { get; set; }
		public string BranchFaxNumber { get; set; }
		public string BranchEmail { get; set; }
		public string ClientIATA { get; set; }
        public string PortraitStatusDescription { get; set; }
        public string ClientBusinessDescription { get; set; }
   }
}
