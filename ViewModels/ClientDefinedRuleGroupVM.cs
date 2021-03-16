using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientDefinedRuleGroupVM : CWTBaseViewModel
    {
        public ClientDefinedRuleGroup ClientDefinedRuleGroup { get; set; }

		public IEnumerable<SelectListItem> TripTypes { get; set; }
		public IEnumerable<SelectListItem> HierarchyTypes { get; set; }
		public IEnumerable<SelectListItem> ClientDefinedRuleBusinessEntityCategories { get; set; }

		public List<ClientDefinedRuleGroupLogic> ClientDefinedRuleGroupLogics { get; set; }
		public List<ClientDefinedRuleGroupResult> ClientDefinedRuleGroupResults { get; set; }
		public List<ClientDefinedRuleGroupTrigger> ClientDefinedRuleGroupTriggers { get; set; }

		public IEnumerable<SelectListItem> ClientDefinedRuleBusinessEntities { get; set; }
		public IEnumerable<SelectListItem> ClientDefinedRuleLogicBusinessEntities { get; set; }
		public IEnumerable<SelectListItem> ClientDefinedRuleResultBusinessEntities { get; set; }

		public IEnumerable<SelectListItem> ClientDefinedRuleRelationalOperators { get; set; }
		public IEnumerable<SelectListItem> ClientDefinedRuleWorkflowTriggerStates { get; set; }
		public IEnumerable<SelectListItem> ClientDefinedRuleWorkflowTriggerApplicationModes { get; set; }


        public ClientDefinedRuleGroupVM()
        {
        }

        public ClientDefinedRuleGroupVM(
			ClientDefinedRuleGroup clientDefinedRuleGroup,
			IEnumerable<SelectListItem> clientDefinedRuleBusinessEntityCategories,
			IEnumerable<SelectListItem> hierarchyTypes,
			IEnumerable<SelectListItem> tripTypes)
        {
			ClientDefinedRuleBusinessEntityCategories = clientDefinedRuleBusinessEntityCategories;
			ClientDefinedRuleGroup = clientDefinedRuleGroup;
            HierarchyTypes = hierarchyTypes;
			TripTypes = tripTypes;
        }
    }
}