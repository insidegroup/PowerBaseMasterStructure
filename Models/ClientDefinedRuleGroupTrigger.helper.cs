using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	public partial class ClientDefinedRuleGroupTrigger : CWTBaseModel
	{
		public IEnumerable<SelectListItem> ClientDefinedRuleWorkflowTriggerStates { get; set; }
		public IEnumerable<SelectListItem> ClientDefinedRuleWorkflowTriggerApplicationModes { get; set; }

		public ClientDefinedRuleWorkflowTrigger ClientDefinedRuleWorkflowTrigger { get; set; }
	}
}

