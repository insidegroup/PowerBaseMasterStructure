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

namespace CWTDesktopDatabase.Repository
{
	public class ClientDefinedBusinessRuleGroupRepository
    {
		private ClientDefinedRuleDC db = new ClientDefinedRuleDC(Settings.getConnectionString());

        //Get a Page of ClientDefinedRuleGroups - for Page Listings
		public CWTPaginatedList<spDesktopDataAdmin_SelectClientDefinedBusinessRuleGroups_v1Result> PageClientDefinedBusinessRuleGroups(
			bool deleted,
			string category, 
			string clientDefinedRuleGroupName,
			int page, 
			string filter, 
			string sortField, 
			int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectClientDefinedBusinessRuleGroups_v1(deleted, category, clientDefinedRuleGroupName, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientDefinedBusinessRuleGroups_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

		//Get Search List Filters
		public List<string> GetSearchListFilters()
		{
			List<string> searchFields = new List<string>() {
				 "Category", 
				 "Business Rules Group Name"
			};

			return searchFields.ToList();
		}

		public ClientDefinedRuleGroup GetGroup(int id)
		{
			return db.ClientDefinedRuleGroups.SingleOrDefault(c => c.ClientDefinedRuleGroupId == id);
		}

		//Add Group
		public void Add(ClientDefinedRuleGroupVM clientDefinedRuleGroupVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			//ClientDefinedRuleLogicItems to XML
			XmlDocument clientDefinedRuleLogicItemDoc = LoadClientDefinedRuleLogicItems(clientDefinedRuleGroupVM);

			//ClientDefinedRuleResultItem to XML
			XmlDocument clientDefinedRuleResultItemDoc = LoadClientDefinedRuleResultItems(clientDefinedRuleGroupVM);

			//ClientDefinedRuleWorkflowTriggers to XML
			XmlDocument clientDefinedRuleWorkflowTriggerDoc = LoadClientDefinedRuleWorkflowTriggers(clientDefinedRuleGroupVM);

			db.spDesktopDataAdmin_InsertClientDefinedBusinessRuleGroup_v1(
				clientDefinedRuleGroupVM.ClientDefinedRuleGroup.ClientDefinedRuleGroupName,
				clientDefinedRuleGroupVM.ClientDefinedRuleGroup.Category,
				clientDefinedRuleGroupVM.ClientDefinedRuleGroup.EnabledFlag,
				clientDefinedRuleGroupVM.ClientDefinedRuleGroup.EnabledDate,
				clientDefinedRuleGroupVM.ClientDefinedRuleGroup.ExpiryDate,
				clientDefinedRuleGroupVM.ClientDefinedRuleGroup.TripTypeId,
				System.Xml.Linq.XElement.Parse(clientDefinedRuleLogicItemDoc.OuterXml),
				System.Xml.Linq.XElement.Parse(clientDefinedRuleResultItemDoc.OuterXml),
				System.Xml.Linq.XElement.Parse(clientDefinedRuleWorkflowTriggerDoc.OuterXml),
				adminUserGuid
			);
		}

		//Edit Group
		public void Edit(ClientDefinedRuleGroupVM clientDefinedRuleGroupVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			//ClientDefinedRuleLogicItems to XML
			XmlDocument clientDefinedRuleLogicItemDoc = LoadClientDefinedRuleLogicItems(clientDefinedRuleGroupVM);

			//ClientDefinedRuleResultItem to XML
			XmlDocument clientDefinedRuleResultItemDoc = LoadClientDefinedRuleResultItems(clientDefinedRuleGroupVM);

			//ClientDefinedRuleWorkflowTriggers to XML
			XmlDocument clientDefinedRuleWorkflowTriggerDoc = LoadClientDefinedRuleWorkflowTriggers(clientDefinedRuleGroupVM);

			db.spDesktopDataAdmin_UpdateClientDefinedBusinessRuleGroup_v1(
				clientDefinedRuleGroupVM.ClientDefinedRuleGroup.ClientDefinedRuleGroupId,
				clientDefinedRuleGroupVM.ClientDefinedRuleGroup.ClientDefinedRuleGroupName,
				clientDefinedRuleGroupVM.ClientDefinedRuleGroup.Category,
				clientDefinedRuleGroupVM.ClientDefinedRuleGroup.EnabledFlag,
				clientDefinedRuleGroupVM.ClientDefinedRuleGroup.EnabledDate,
				clientDefinedRuleGroupVM.ClientDefinedRuleGroup.ExpiryDate,
				clientDefinedRuleGroupVM.ClientDefinedRuleGroup.TripTypeId,
				System.Xml.Linq.XElement.Parse(clientDefinedRuleLogicItemDoc.OuterXml),
				System.Xml.Linq.XElement.Parse(clientDefinedRuleResultItemDoc.OuterXml),
				System.Xml.Linq.XElement.Parse(clientDefinedRuleWorkflowTriggerDoc.OuterXml),
				adminUserGuid
			);
		}

		//Load ClientDefinedRuleLogicItems
		public XmlDocument LoadClientDefinedRuleLogicItems(ClientDefinedRuleGroupVM clientDefinedRuleGroupVM)
		{
			XmlDocument clientDefinedRuleLogicItemDoc = new XmlDocument();
			XmlDeclaration clientDefinedRuleLogicItemDec = clientDefinedRuleLogicItemDoc.CreateXmlDeclaration("1.0", null, null);
			clientDefinedRuleLogicItemDoc.AppendChild(clientDefinedRuleLogicItemDec);
			XmlElement clientDefinedRuleLogicItemRoot = clientDefinedRuleLogicItemDoc.CreateElement("ClientDefinedRuleLogicItems");
			clientDefinedRuleLogicItemDoc.AppendChild(clientDefinedRuleLogicItemRoot);

			if (clientDefinedRuleGroupVM.ClientDefinedRuleGroupLogics != null)
			{
				foreach (ClientDefinedRuleGroupLogic clientDefinedRuleGroupLogic in clientDefinedRuleGroupVM.ClientDefinedRuleGroupLogics)
				{
					if (clientDefinedRuleGroupLogic != null)
					{
						XmlElement xmlClientDefinedRuleLogicItem = clientDefinedRuleLogicItemDoc.CreateElement("ClientDefinedRuleLogicItem");

						XmlElement xmlClientDefinedRuleBusinessEntityId = clientDefinedRuleLogicItemDoc.CreateElement("ClientDefinedRuleBusinessEntityId");
						xmlClientDefinedRuleBusinessEntityId.InnerText = clientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleBusinessEntityId.ToString();
						xmlClientDefinedRuleLogicItem.AppendChild(xmlClientDefinedRuleBusinessEntityId);

						XmlElement xmlClientDefinedRuleRelationalOperatorId = clientDefinedRuleLogicItemDoc.CreateElement("ClientDefinedRuleRelationalOperatorId");
						xmlClientDefinedRuleRelationalOperatorId.InnerText = clientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleRelationalOperatorId.ToString();
						xmlClientDefinedRuleLogicItem.AppendChild(xmlClientDefinedRuleRelationalOperatorId);

						XmlElement xmlClientDefinedRuleLogicItemValue = clientDefinedRuleLogicItemDoc.CreateElement("ClientDefinedRuleLogicItemValue");
						xmlClientDefinedRuleLogicItemValue.InnerText = clientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleLogicItemValue != null ? clientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleLogicItemValue.ToString() : "";
						xmlClientDefinedRuleLogicItem.AppendChild(xmlClientDefinedRuleLogicItemValue);

						clientDefinedRuleLogicItemRoot.AppendChild(xmlClientDefinedRuleLogicItem);
					}
				}
			}

			return clientDefinedRuleLogicItemDoc;
		}

		//Load ClientDefinedRuleResultItems
		public XmlDocument LoadClientDefinedRuleResultItems(ClientDefinedRuleGroupVM clientDefinedRuleGroupVM)
		{
			XmlDocument clientDefinedRuleResultItemDoc = new XmlDocument();
			XmlDeclaration clientDefinedRuleResultItemDec = clientDefinedRuleResultItemDoc.CreateXmlDeclaration("1.0", null, null);
			clientDefinedRuleResultItemDoc.AppendChild(clientDefinedRuleResultItemDec);
			XmlElement clientDefinedRuleResultItemRoot = clientDefinedRuleResultItemDoc.CreateElement("ClientDefinedRuleResultItems");
			clientDefinedRuleResultItemDoc.AppendChild(clientDefinedRuleResultItemRoot);

			if (clientDefinedRuleGroupVM.ClientDefinedRuleGroupResults != null)
			{
				foreach (ClientDefinedRuleGroupResult clientDefinedRuleGroupResult in clientDefinedRuleGroupVM.ClientDefinedRuleGroupResults)
				{
					if (clientDefinedRuleGroupResult != null)
					{
						XmlElement xmlClientDefinedRuleResultItem = clientDefinedRuleResultItemDoc.CreateElement("ClientDefinedRuleResultItem");

						XmlElement xmlClientDefinedRuleBusinessEntityId = clientDefinedRuleResultItemDoc.CreateElement("ClientDefinedRuleBusinessEntityId");
						xmlClientDefinedRuleBusinessEntityId.InnerText = clientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleBusinessEntityId.ToString();
						xmlClientDefinedRuleResultItem.AppendChild(xmlClientDefinedRuleBusinessEntityId);

						XmlElement xmlClientDefinedRuleResultItemValue = clientDefinedRuleResultItemDoc.CreateElement("ClientDefinedRuleResultItemValue");
						xmlClientDefinedRuleResultItemValue.InnerText = clientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleResultItemValue.ToString();
						xmlClientDefinedRuleResultItem.AppendChild(xmlClientDefinedRuleResultItemValue);

						clientDefinedRuleResultItemRoot.AppendChild(xmlClientDefinedRuleResultItem);
					}
				}
			}

			return clientDefinedRuleResultItemDoc;
		}

		//Load ClientDefinedRuleWorkflowTriggers
		public XmlDocument LoadClientDefinedRuleWorkflowTriggers(ClientDefinedRuleGroupVM clientDefinedRuleGroupVM)
		{
			XmlDocument clientDefinedRuleWorkflowTriggerDoc = new XmlDocument();
			XmlDeclaration clientDefinedRuleWorkflowTriggerDec = clientDefinedRuleWorkflowTriggerDoc.CreateXmlDeclaration("1.0", null, null);
			clientDefinedRuleWorkflowTriggerDoc.AppendChild(clientDefinedRuleWorkflowTriggerDec);
			XmlElement clientDefinedRuleWorkflowTriggerRoot = clientDefinedRuleWorkflowTriggerDoc.CreateElement("ClientDefinedRuleWorkflowTriggers");
			clientDefinedRuleWorkflowTriggerDoc.AppendChild(clientDefinedRuleWorkflowTriggerRoot);

			if (clientDefinedRuleGroupVM.ClientDefinedRuleGroupTriggers != null)
			{
				foreach (ClientDefinedRuleGroupTrigger clientDefinedRuleGroupTrigger in clientDefinedRuleGroupVM.ClientDefinedRuleGroupTriggers)
				{
					if (clientDefinedRuleGroupTrigger != null)
					{
						XmlElement xmlClientDefinedRuleWorkflowTrigger = clientDefinedRuleWorkflowTriggerDoc.CreateElement("ClientDefinedRuleWorkflowTrigger");

						XmlElement xmlClientDefinedRuleWorkflowTriggerStateId = clientDefinedRuleWorkflowTriggerDoc.CreateElement("ClientDefinedRuleWorkflowTriggerStateId");
						xmlClientDefinedRuleWorkflowTriggerStateId.InnerText = clientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger.ClientDefinedRuleWorkflowTriggerStateId.ToString();
						xmlClientDefinedRuleWorkflowTrigger.AppendChild(xmlClientDefinedRuleWorkflowTriggerStateId);

						XmlElement xmlClientDefinedRuleWorkflowTriggerApplicationModeId = clientDefinedRuleWorkflowTriggerDoc.CreateElement("ClientDefinedRuleWorkflowTriggerApplicationModeId");
						xmlClientDefinedRuleWorkflowTriggerApplicationModeId.InnerText = clientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger.ClientDefinedRuleWorkflowTriggerApplicationModeId.ToString();
						xmlClientDefinedRuleWorkflowTrigger.AppendChild(xmlClientDefinedRuleWorkflowTriggerApplicationModeId);

						clientDefinedRuleWorkflowTriggerRoot.AppendChild(xmlClientDefinedRuleWorkflowTrigger);
					}
				}
			}

			return clientDefinedRuleWorkflowTriggerDoc;
		}

		//Add Data From Linked Tables for Display
		public void EditGroupForDisplay(ClientDefinedRuleGroup group)
		{
			HierarchyRepository hierarchyRepository = new HierarchyRepository();

			List<fnDesktopDataAdmin_SelectClientDefinedRuleGroupHierarchy_v1Result> hierarchy = new List<fnDesktopDataAdmin_SelectClientDefinedRuleGroupHierarchy_v1Result>();
			hierarchy = GetGroupHierarchy(group.ClientDefinedRuleGroupId);
			if (hierarchy.Count > 0)
			{
				if (hierarchy.Count == 1)
				{
					group.HierarchyCode = hierarchy[0].HierarchyCode.ToString();
					group.HierarchyItem = hierarchy[0].HierarchyName.Trim();
					group.HierarchyType = hierarchy[0].HierarchyType;

					if (hierarchy[0].HierarchyType == "ClientSubUnitTravelerType")
					{
						group.ClientSubUnitGuid = hierarchy[0].HierarchyCode.ToString();
						group.ClientSubUnitName = hierarchy[0].HierarchyName.Trim();
						group.TravelerTypeGuid = hierarchy[0].TravelerTypeGuid;
						group.TravelerTypeName = hierarchy[0].TravelerTypeName.Trim();
					}
					if (hierarchy[0].HierarchyType == "ClientAccount")
					{
						group.SourceSystemCode = hierarchy[0].SourceSystemCode;
					}
				}
				else
				{
					group.HierarchyCode = "Multiple"; //Placeholder
					group.HierarchyItem = "Multiple"; //Placeholder
					group.HierarchyType = "Multiple";
				}
			}
			group.IsMultipleHierarchy = (hierarchy.Count > 1);
		}

		//Get Hierarchy Details
		public List<fnDesktopDataAdmin_SelectClientDefinedRuleGroupHierarchy_v1Result> GetGroupHierarchy(int id)
		{
			var result = db.fnDesktopDataAdmin_SelectClientDefinedRuleGroupHierarchy_v1(id);
			return result.ToList();
		}

		//Change the deleted status on an item
		public void UpdateGroupDeletedStatus(ClientDefinedRuleGroup group)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateClientDefinedRuleGroupDeletedStatus_v1(
					group.ClientDefinedRuleGroupId,
					group.DeletedFlag,
					adminUserGuid,
					group.VersionNumber
				);

		}

		//Get Hierarchies linked to a ClientDefinedRuleGroup at one level or all levels
		public List<spDesktopDataAdmin_SelectClientDefinedBusinessRuleGroupLinkedHierarchies_v1Result> ClientDefinedBusinessRuleGroupLinkedHierarchies(int clientDefinedRuleGroupId, string filterHierarchyType)
		{
			return db.spDesktopDataAdmin_SelectClientDefinedBusinessRuleGroupLinkedHierarchies_v1(clientDefinedRuleGroupId, filterHierarchyType).ToList();
		}

		//Get A Count of all Hierarchies linked to a ClientDefinedRuleGroup
		public int CountClientDefinedRuleGroupLinkedHierarchies(int clientDefinedRuleGroupId)
		{
			return db.spDesktopDataAdmin_SelectClientDefinedRuleGroupLinkedHierarchies_v1(clientDefinedRuleGroupId, "Multiple").Count();
		}

		//Get Hierarchies linked to of ClientDefinedBusinessRuleGroups
		public List<spDesktopDataAdmin_SelectClientDefinedBusinessRuleGroupAvailableHierarchies_v1Result> ClientDefinedBusinessRuleGroupAvailableHierarchies(int clientDefinedRuleGroupId, string hierarchyType, string filter, string clientSubUnit = "", string travelerType = "")
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			return db.spDesktopDataAdmin_SelectClientDefinedBusinessRuleGroupAvailableHierarchies_v1(clientDefinedRuleGroupId, hierarchyType, filter, clientSubUnit, travelerType, adminUserGuid).ToList();
		}

		//Change the deleted status on an item
		public void UpdateLinkedHierarchy(GroupHierarchyVM groupHierarchyVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateClientDefinedBusinessRuleGroupLinkedHierarchy_v1(
					groupHierarchyVM.GroupId,
					groupHierarchyVM.HierarchyType,
					groupHierarchyVM.HierarchyCode,
					groupHierarchyVM.TravelerTypeGuid,
					groupHierarchyVM.ClientSubUnitGuid,
					groupHierarchyVM.SourceSystemCode,
					adminUserGuid
				);
		}

		public string getHierarchyType(string filterHierarchySearchProperty)
		{
			switch (filterHierarchySearchProperty)
			{
				case "ClientTopUnitName":
					return "ClientTopUnit";
				case "ClientTopUnitGuid":
					return "ClientTopUnit";
				case "ClientSubUnitName":
					return "ClientSubUnit";
				case "ClientSubUnitGuid":
					return "ClientSubUnit";
				case "ClientAccountName":
					return "ClientAccount";
				case "ClientAccountNumber":
					return "ClientAccount";
				case "CountryName":
					return "Country";
				case "CountryCode":
					return "Country";
				case "LocationName":
					return "Location";
				default:
					return "Multiple";
			}
		}

		public string GetAvailableHierarchyTypeDisplayName(string filterHierarchySearchProperty)
		{
			switch (filterHierarchySearchProperty)
			{
				case "ClientTopUnitName":
					return "Client TopUnits";
				case "ClientTopUnit":
					return "Client TopUnits";
				case "ClientSubUnitName":
					return "Client SubUnits";
				case "ClientAccountName":
					return "Client Accounts";
				case "ClientAccountNumber":
					return "Client Account";
				case "ClientSubUnitTravelerType":
					return "Client SubUnit Traveler Types";
				case "TravelerType":
					return "Traveler Types";
				default:
					return "Items";
			}
		}
    }
}


