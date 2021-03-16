using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(ServiceFundValidation))]
	public partial class ServiceFund : CWTBaseModel
	{
		public string ClientTopUnitName { get; set; }

		public string SupplierName { get; set; }
		public string ProductName { get; set; }
		
		public string ClientAccountName { get; set; }
		
		public string ServiceFundStartTimeString { get; set; }
		public string ServiceFundEndTimeString { get; set; }
	}
}