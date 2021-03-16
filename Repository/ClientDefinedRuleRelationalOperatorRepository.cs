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
	public class ClientDefinedRuleRelationalOperatorRepository
	{
		private ClientDefinedRuleDC db = new ClientDefinedRuleDC(Settings.getConnectionString());

		//Get one ClientDefinedRuleRelationalOperator
		public ClientDefinedRuleRelationalOperator GetGroup(int id)
		{
			return db.ClientDefinedRuleRelationalOperators.SingleOrDefault(c => c.ClientDefinedRuleRelationalOperatorId == id);
		}

		//GetClientDefinedRuleBusinessEntities
		public List<ClientDefinedRuleRelationalOperator> GetClientDefinedRuleRelationalOperators()
		{
			return db.ClientDefinedRuleRelationalOperators.OrderBy(x => x.RelationalOperatorName).ToList();
		}
	}
}


