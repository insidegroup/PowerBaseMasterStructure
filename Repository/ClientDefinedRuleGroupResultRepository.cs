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
	public class ClientDefinedRuleGroupResultRepository
	{
		private ClientDefinedRuleDC db = new ClientDefinedRuleDC(Settings.getConnectionString());
		private ClientDefinedRuleResultItemRepository clientDefinedRuleResultItemRepository = new ClientDefinedRuleResultItemRepository();
		private ClientDefinedRuleBusinessEntityRepository clientDefinedRuleBusinessEntityRepository = new ClientDefinedRuleBusinessEntityRepository();

		public List<ClientDefinedRuleGroupResult> GetClientDefinedRuleGroupResults(int clientDefinedRuleGroupId)
		{
			List<ClientDefinedRuleGroupResult> clientDefinedRuleGroupResults = db.ClientDefinedRuleGroupResults.Where(x => x.ClientDefinedRuleGroupId == clientDefinedRuleGroupId).ToList();

			foreach (ClientDefinedRuleGroupResult clientDefinedRuleGroupResult in clientDefinedRuleGroupResults)
			{
				EditForDisplay(clientDefinedRuleGroupResult);
			}

			return clientDefinedRuleGroupResults;
		}

		public ClientDefinedRuleGroupResult EditForDisplay(ClientDefinedRuleGroupResult clientDefinedRuleGroupResult)
		{
			//ClientDefinedRuleResultItem
			ClientDefinedRuleResultItem clientDefinedRuleResultItem = new ClientDefinedRuleResultItem();
			clientDefinedRuleResultItem = clientDefinedRuleResultItemRepository.GetItem(clientDefinedRuleGroupResult.ClientDefinedRuleResultItemId);
			if (clientDefinedRuleResultItem != null)
			{
				clientDefinedRuleGroupResult.ClientDefinedRuleResultItem = clientDefinedRuleResultItem;
			}

			//ClientDefinedRuleBusinessEntity
			if (clientDefinedRuleGroupResult.ClientDefinedRuleResultItem != null && clientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleBusinessEntity != null)
			{
				//Name
				clientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleBusinessEntityName = clientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleBusinessEntity.BusinessEntityName;
				
				//Description
				clientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleBusinessEntityDescription = clientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleBusinessEntity.BusinessEntityDescription;
			}

			return clientDefinedRuleGroupResult;
		}
	}
}