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
	public class ClientDefinedRuleWorkflowTriggerApplicationModeRepository
	{
		private ClientDefinedRuleDC db = new ClientDefinedRuleDC(Settings.getConnectionString());

		//Get one ClientDefinedRuleWorkflowTriggerApplicationMode
		public ClientDefinedRuleWorkflowTriggerApplicationMode GetGroup(int id)
		{
			return db.ClientDefinedRuleWorkflowTriggerApplicationModes.SingleOrDefault(c => c.ClientDefinedRuleWorkflowTriggerApplicationModeId == id);
		}

		//GetClientDefinedRuleWorkflowTriggerStates
		public List<ClientDefinedRuleWorkflowTriggerApplicationMode> GetClientDefinedRuleWorkflowTriggerApplicationModes()
		{
			return db.ClientDefinedRuleWorkflowTriggerApplicationModes.OrderBy(x => x.ClientDefinedRuleWorkflowTriggerApplicationModeName).ToList();
		}
	}
}


