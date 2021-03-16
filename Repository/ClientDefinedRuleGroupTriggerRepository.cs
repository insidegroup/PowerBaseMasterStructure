using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;
using System.Text.RegularExpressions;
using System.Xml;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Repository
{
	public class ClientDefinedRuleGroupTriggerRepository
	{
		private ClientDefinedRuleDC db = new ClientDefinedRuleDC(Settings.getConnectionString());
		private ClientDefinedRuleWorkflowTriggerStateRepository clientDefinedRuleWorkflowTriggerStateRepository = new ClientDefinedRuleWorkflowTriggerStateRepository();
		private ClientDefinedRuleWorkflowTriggerApplicationModeRepository clientDefinedRuleWorkflowTriggerApplicationModeRepository = new ClientDefinedRuleWorkflowTriggerApplicationModeRepository();

		public List<ClientDefinedRuleGroupTrigger> GetClientDefinedRuleGroupTriggers(int clientDefinedRuleGroupId)
		{
			List<ClientDefinedRuleGroupTrigger> clientDefinedRuleGroupTriggers = db.ClientDefinedRuleGroupTriggers.Where(x => x.ClientDefinedRuleGroupId == clientDefinedRuleGroupId).ToList();

			foreach (ClientDefinedRuleGroupTrigger clientDefinedRuleGroupTrigger in clientDefinedRuleGroupTriggers)
			{
				EditForDisplay(clientDefinedRuleGroupTrigger);
			}

			return clientDefinedRuleGroupTriggers;
		}

		public ClientDefinedRuleGroupTrigger EditForDisplay(ClientDefinedRuleGroupTrigger clientDefinedRuleGroupTrigger)
		{
			if (clientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger == null)
			{
				clientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger = db.ClientDefinedRuleWorkflowTriggers.Where(x => x.ClientDefinedRuleWorkflowTriggerId == clientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTriggerId).FirstOrDefault();
			}
			
			//ClientDefinedRuleWorkflowTriggerStates
			clientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTriggerStates =
				new SelectList(clientDefinedRuleWorkflowTriggerStateRepository.GetClientDefinedRuleWorkflowTriggerStates().ToList(),
					"ClientDefinedRuleWorkflowTriggerStateId",
					"ClientDefinedRuleWorkflowTriggerStateName",
					clientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger != null ? clientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger.ClientDefinedRuleWorkflowTriggerStateId.ToString() : "");

			//ClientDefinedRuleWorkflowTriggerApplicationModes
			clientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTriggerApplicationModes =
				new SelectList(clientDefinedRuleWorkflowTriggerApplicationModeRepository.GetClientDefinedRuleWorkflowTriggerApplicationModes().ToList(),
					"ClientDefinedRuleWorkflowTriggerApplicationModeId",
					"ClientDefinedRuleWorkflowTriggerApplicationModeName",
					clientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger != null ? clientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger.ClientDefinedRuleWorkflowTriggerApplicationModeId.ToString() : "");

			return clientDefinedRuleGroupTrigger;
		}
	}
}