using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(ClientDefinedRuleResultItemValidation))]
	public partial class ClientDefinedRuleResultItem : CWTBaseModel
	{
		public string ClientDefinedRuleBusinessEntityName { get; set; }
		public string ClientDefinedRuleBusinessEntityDescription { get; set; }
	}
}

