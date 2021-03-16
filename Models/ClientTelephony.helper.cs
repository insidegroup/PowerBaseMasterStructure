using CWTDesktopDatabase.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(ClientTelephonyValidation))]
	public partial class ClientTelephony : CWTBaseModel
    {
        public string HierarchyName { get; set; }
		public bool MainNumberFlagNullable { get; set; }
	}

	public class ClientTelephonyJSON{
		public string ClientSubUnitName { get; set; }
		public string ClientSubUnitGuid { get; set; }
		public string ClientTopUnitName { get; set; }
		public string ClientTopUnitGuid { get; set; }
	}
}
