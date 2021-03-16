using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(ClientSubUnitClientAccountValidation))]
	public partial class ClientSubUnitClientAccount : CWTBaseModel
	{
		public string ClientSubUnitName { get; set; }
		public string ClientAccountName { get; set; }
		public string ConfidenceLevelForLoadDescription { get; set; }
	}

	public class ClientSubUnitClientAccountImport : CWTBaseModel
	{
		public string ClientSubUnitGuid { get; set; }
		public string SourceSystemCode { get; set; }
		public string ClientAccountNumber { get; set; }
	}
}
