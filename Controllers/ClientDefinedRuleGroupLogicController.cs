using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;
using System.Xml;

namespace CWTDesktopDatabase.Controllers
{
	public class ClientDefinedRuleGroupLogicController : Controller
    {
        //main repository
		ClientDefinedRuleGroupRepository clientDefinedRuleGroupRepository = new ClientDefinedRuleGroupRepository();
		ClientDefinedRuleGroupLogicRepository clientDefinedRuleGroupLogicRepository = new ClientDefinedRuleGroupLogicRepository();
        HierarchyRepository hierarchyRepository = new HierarchyRepository();
		
		private string groupName = "Client Rules Group Administrator";

		// GET: /EditSequence for Logic Items
		public ActionResult EditSequence(int id, string filter, int? page)
		{
			//Get Item From Database
			ClientDefinedRuleGroup clientDefinedRuleGroup = new ClientDefinedRuleGroup();
			clientDefinedRuleGroup = clientDefinedRuleGroupRepository.GetGroup(id);

			//Check Exists
			if (clientDefinedRuleGroup == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}
			
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			var items = clientDefinedRuleGroupLogicRepository.PageClientDefinedRuleGroupLogicSequences(id, page ?? 1);

			ViewData["ClientDefinedRuleGroupId"] = clientDefinedRuleGroup.ClientDefinedRuleGroupId;
			ViewData["ClientDefinedRuleGroupName"] = clientDefinedRuleGroup.ClientDefinedRuleGroupName;
			ViewData["ClientDefinedRuleGroupType"] = clientDefinedRuleGroup.IsBusinessGroupFlag ? "Business" : "Client";
			ViewData["ClientDefinedRuleGroupController"] = clientDefinedRuleGroup.IsBusinessGroupFlag ? "ClientDefinedBusinessRuleGroup" : "ClientDefinedRuleGroup";
			ViewData["Filter"] = filter ?? "";
			ViewData["Page"] = page ?? 1;

			return View(items);
		}
		
		// POST: /EditSequence
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditSequence(string clientDefinedRuleGroupId, int page, FormCollection collection)
		{

			//Get Item From Database
			ClientDefinedRuleGroup clientDefinedRuleGroup = new ClientDefinedRuleGroup();
			clientDefinedRuleGroup = clientDefinedRuleGroupRepository.GetGroup(Int32.Parse(clientDefinedRuleGroupId));

			//Check Exists
			if (clientDefinedRuleGroup == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}
			
			//Check Access Rights
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			string[] sequences = collection["Sequence"].Split(new char[] { ',' });

			int sequence = (page - 1 * 5) - 2;
			if (sequence < 0)
			{
				sequence = 1;
			}

			XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
			doc.AppendChild(dec);
			XmlElement root = doc.CreateElement("SequenceXML");
			doc.AppendChild(root);

			foreach (string s in sequences)
			{
				string[] primaryKey = s.Split(new char[] { '_' });

				string clientDefinedRuleLogicItemId = primaryKey[0];

				if (!string.IsNullOrEmpty(clientDefinedRuleLogicItemId))
				{
					int versionNumber = Convert.ToInt32(primaryKey[1]);

					XmlElement xmlItem = doc.CreateElement("Item");
					root.AppendChild(xmlItem);

					XmlElement xmlSequence = doc.CreateElement("LogicSequenceNumber");
					xmlSequence.InnerText = sequence.ToString();
					xmlItem.AppendChild(xmlSequence);

					XmlElement xmlClientDefinedRuleGroupId = doc.CreateElement("ClientDefinedRuleGroupId");
					xmlClientDefinedRuleGroupId.InnerText = clientDefinedRuleGroupId.ToString();
					xmlItem.AppendChild(xmlClientDefinedRuleGroupId);

					XmlElement xmlClientDefinedRuleLogicItemId = doc.CreateElement("ClientDefinedRuleLogicItemId");
					xmlClientDefinedRuleLogicItemId.InnerText = clientDefinedRuleLogicItemId.ToString();
					xmlItem.AppendChild(xmlClientDefinedRuleLogicItemId);

					XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
					xmlVersionNumber.InnerText = versionNumber.ToString();
					xmlItem.AppendChild(xmlVersionNumber);

					sequence = sequence + 1;
				}
			}

			try
			{
				clientDefinedRuleGroupLogicRepository.UpdateClientDefinedRuleGroupLogicSequences(System.Xml.Linq.XElement.Parse(doc.OuterXml));
			}
			catch (SqlException ex)
			{
				//Versioning Error
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/ClientDefinedRuleGroupLogic.mvc/EditSequence?clientDefinedRuleGroupId=" + clientDefinedRuleGroupId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("ListUndeleted", clientDefinedRuleGroup.IsBusinessGroupFlag ? "ClientDefinedBusinessRuleGroup" : "ClientDefinedRuleGroup");
		}
    }
}
