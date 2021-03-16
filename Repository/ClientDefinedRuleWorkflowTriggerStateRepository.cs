using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;
using System.Text.RegularExpressions;

namespace CWTDesktopDatabase.Repository
{
	public class ClientDefinedRuleWorkflowTriggerStateRepository
	{
		private ClientDefinedRuleDC db = new ClientDefinedRuleDC(Settings.getConnectionString());

		//Get one ClientDefinedRuleWorkflowTriggerState
		public ClientDefinedRuleWorkflowTriggerState GetGroup(int id)
		{
			return db.ClientDefinedRuleWorkflowTriggerStates.SingleOrDefault(c => c.ClientDefinedRuleWorkflowTriggerStateId == id);
		}

		//GetClientDefinedRuleWorkflowTriggerStates
		public List<ClientDefinedRuleWorkflowTriggerState> GetClientDefinedRuleWorkflowTriggerStates()
		{
			return db.ClientDefinedRuleWorkflowTriggerStates.OrderBy(x => x.ClientDefinedRuleWorkflowTriggerStateName).ToList();
		}
	}
}


