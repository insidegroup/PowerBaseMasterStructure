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
	public class ClientDefinedRuleGroupLogicRepository
	{
		private ClientDefinedRuleDC db = new ClientDefinedRuleDC(Settings.getConnectionString());
		private ClientDefinedRuleBusinessEntityRepository clientDefinedRuleBusinessEntityRepository = new ClientDefinedRuleBusinessEntityRepository();
		private ClientDefinedRuleRelationalOperatorRepository clientDefinedRuleRelationalOperatorRepository = new ClientDefinedRuleRelationalOperatorRepository();

		public List<ClientDefinedRuleGroupLogic> GetClientDefinedRuleGroupLogics(int clientDefinedRuleGroupId)
		{
			List<ClientDefinedRuleGroupLogic> clientDefinedRuleGroupLogics = db.ClientDefinedRuleGroupLogics
				.Where(x => x.ClientDefinedRuleGroupId == clientDefinedRuleGroupId)
				.OrderBy(x => x.LogicSequenceNumber)
				.ToList();

			foreach (ClientDefinedRuleGroupLogic clientDefinedRuleGroupLogic in clientDefinedRuleGroupLogics)
			{
				EditForDisplay(clientDefinedRuleGroupLogic);
			}

			return clientDefinedRuleGroupLogics;
		}

		public void EditForDisplay(ClientDefinedRuleGroupLogic clientDefinedRuleGroupLogic)
		{
			//ClientDefinedRuleBusinessEntity
			if(clientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem != null && clientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleBusinessEntity != null) {

				//Name
				clientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleBusinessEntityName = clientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleBusinessEntity.BusinessEntityName;
				
				//Description
				clientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleBusinessEntityDescription = clientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleBusinessEntity.BusinessEntityDescription;
			}

			//ClientDefinedRuleRelationalOperators
			IEnumerable<SelectListItem> clientDefinedRuleRelationalOperators = new SelectList(clientDefinedRuleRelationalOperatorRepository.GetClientDefinedRuleRelationalOperators().ToList(),
				"ClientDefinedRuleRelationalOperatorId",
				"RelationalOperatorName",
				clientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem != null ? clientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleRelationalOperatorId.ToString() : "");

			clientDefinedRuleGroupLogic.ClientDefinedRuleRelationalOperators = clientDefinedRuleRelationalOperators;
		}

		//List of ClientDefinedRuleGroupLogic Sequences
		public CWTPaginatedList<spDesktopDataAdmin_SelectClientDefinedRuleLogicSequences_v1Result> PageClientDefinedRuleGroupLogicSequences(int clientDefinedRuleGroupId, int? page)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			int pageSize = 50;
			var result = db.spDesktopDataAdmin_SelectClientDefinedRuleLogicSequences_v1(
				clientDefinedRuleGroupId,
				page,
				pageSize
			).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientDefinedRuleLogicSequences_v1Result>(result, page ?? 1, Convert.ToInt32(totalRecords));
			return paginatedView;

		}

		//Update Sequences of ClientDefinedRuleGroupLogic
		public void UpdateClientDefinedRuleGroupLogicSequences(System.Xml.Linq.XElement xmlElement)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			db.spDesktopDataAdmin_UpdateClientDefinedRuleGroupLogicSequences_v1(xmlElement, adminUserGuid);
		}


	}
}