using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(ClientDefinedRuleGroupLogicValidation))]
	public partial class ClientDefinedRuleGroupLogic : CWTBaseModel
	{
		public IEnumerable<SelectListItem> ClientDefinedRuleBusinessEntities { get; set; }
		public IEnumerable<SelectListItem> ClientDefinedRuleLogicBusinessEntities { get; set; }
		public IEnumerable<SelectListItem> ClientDefinedRuleResultBusinessEntities { get; set; }
		
		public IEnumerable<SelectListItem> ClientDefinedRuleRelationalOperators { get; set; }
	}
}

