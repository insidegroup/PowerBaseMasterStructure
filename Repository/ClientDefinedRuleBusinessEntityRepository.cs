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
	public class ClientDefinedRuleBusinessEntityRepository
	{
		private ClientDefinedRuleDC db = new ClientDefinedRuleDC(Settings.getConnectionString());

		//Get one ClientDefinedRuleBusinessEntity
		public ClientDefinedRuleBusinessEntity GetGroup(int id)
		{
			return db.ClientDefinedRuleBusinessEntities.SingleOrDefault(c => c.ClientDefinedRuleBusinessEntityId == id);
		}

		//GetClientDefinedRuleLogicBusinessEntities - Logic
		public List<ClientDefinedRuleBusinessEntity> GetClientDefinedRuleLogicBusinessEntities()
		{
			return db.ClientDefinedRuleBusinessEntities
				.Where(x => x.IsLogic == true && x.BusinessEntityDescription != "")
				.OrderBy(x => x.BusinessEntityDescription).ToList();
		}

		//AutoCompleteClientDefinedRuleLogicBusinessEntities - JSON
		public List<ClientDefinedRuleBusinessEntityJSON> AutoCompleteClientDefinedRuleLogicBusinessEntities(string searchText)
		{
			var result = from n in db.ClientDefinedRuleBusinessEntities
						 where n.IsLogic == true && n.BusinessEntityDescription != "" && n.BusinessEntityDescription.Contains(searchText)
						 orderby n.BusinessEntityDescription
						 select
							 new ClientDefinedRuleBusinessEntityJSON
							 {
								 ClientDefinedRuleBusinessEntityId = n.ClientDefinedRuleBusinessEntityId,
								 BusinessEntityDescription = n.BusinessEntityDescription.ToString().Trim()
							 };
			return result.ToList();
		}

		//GetClientDefinedRuleResultBusinessEntities - Result
		public List<ClientDefinedRuleBusinessEntity> GetClientDefinedRuleResultBusinessEntities()
		{
			return db.ClientDefinedRuleBusinessEntities
				.Where(x => x.IsResult == true && x.BusinessEntityDescription != "")
				.OrderBy(x => x.BusinessEntityDescription).ToList();
		}

		//AutoCompleteClientDefinedRuleLogicBusinessEntities - JSON
		public List<ClientDefinedRuleBusinessEntityJSON> AutoCompleteClientDefinedRuleResultBusinessEntities(string searchText)
		{
			var result = from n in db.ClientDefinedRuleBusinessEntities
						 where n.IsResult == true && n.BusinessEntityDescription != "" && n.BusinessEntityDescription.Contains(searchText)
						 orderby n.BusinessEntityDescription
						 select
							 new ClientDefinedRuleBusinessEntityJSON
							 {
								 ClientDefinedRuleBusinessEntityId = n.ClientDefinedRuleBusinessEntityId,
								 BusinessEntityDescription = n.BusinessEntityDescription.ToString().Trim()
							 };
			return result.ToList();
		}
	}
}


