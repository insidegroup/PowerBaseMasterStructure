using ClientProfileServiceBusiness;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;

namespace CWTDesktopDatabase.Controllers
{
	public class PolicyOtherGroupHeaderColumnNameController : Controller
	{
		// Main repository
		PolicyOtherGroupHeaderRepository policyOtherGroupHeaderRepository = new PolicyOtherGroupHeaderRepository();
		PolicyOtherGroupHeaderColumnNameRepository policyOtherGroupHeaderColumnNameRepository = new PolicyOtherGroupHeaderColumnNameRepository();
		PolicyOtherGroupHeaderTableNameRepository policyOtherGroupHeaderTableNameRepository = new PolicyOtherGroupHeaderTableNameRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		RolesRepository rolesRepository = new RolesRepository();

		private string groupName = "Policy Group Other Headers Administrator";

		//
		// GET: /PolicyOtherGroupHeaderColumnName/List

		public ActionResult List(int id, string filter, int? page, string sortField, int? sortOrder)
		{
			//SortField
			if (string.IsNullOrEmpty(sortField))
			{
				sortField = "DisplayOrder";
			}
			ViewData["CurrentSortField"] = sortField;

			//SortOrder
			if (sortOrder == 1)
			{
				ViewData["NewSortOrder"] = 0;
				ViewData["CurrentSortOrder"] = 1;
			}
			else
			{
				ViewData["NewSortOrder"] = 1;
				ViewData["CurrentSortOrder"] = 0;
				sortOrder = 0;
			}

			PolicyOtherGroupHeaderColumnNamesVM policyOtherGroupHeaderColumnNamesVM = new PolicyOtherGroupHeaderColumnNamesVM();

			//Get Item From Database
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(id);

			//Check Exists
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			policyOtherGroupHeaderColumnNamesVM.PolicyOtherGroupHeader = policyOtherGroupHeader;

			//Set Access Rights
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				policyOtherGroupHeaderColumnNamesVM.HasWriteAccess = true;
			}

			if (policyOtherGroupHeaderColumnNameRepository != null)
			{
				var policyOtherGroupHeaderColumnNames = policyOtherGroupHeaderColumnNameRepository.PagePolicyOtherGroupHeaderColumnNames(id, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

				if (policyOtherGroupHeaderColumnNames != null)
				{
					policyOtherGroupHeaderColumnNamesVM.PolicyOtherGroupHeaderColumnNames = policyOtherGroupHeaderColumnNames;
				}
			}

			//return items
			return View(policyOtherGroupHeaderColumnNamesVM);
		}

		//
		// GET: /PolicyOtherGroupHeaderColumnName/Create
		public ActionResult Create(int id)
		{
			//Get Item From Database
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(id);

			//Check Exists
			if (policyOtherGroupHeader == null)
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

			PolicyOtherGroupHeaderColumnNameVM policyOtherGroupHeaderColumnNameVM = new PolicyOtherGroupHeaderColumnNameVM();

			PolicyOtherGroupHeaderColumnName policyOtherGroupHeaderColumnName = new PolicyOtherGroupHeaderColumnName();
			policyOtherGroupHeaderColumnNameVM.PolicyOtherGroupHeaderColumnName = policyOtherGroupHeaderColumnName;

			PolicyOtherGroupHeaderTableName policyOtherGroupHeaderTableName = new PolicyOtherGroupHeaderTableName();
			policyOtherGroupHeaderTableName = policyOtherGroupHeaderTableNameRepository.GetPolicyOtherGroupHeaderTableName(policyOtherGroupHeader.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeaderTableName == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			policyOtherGroupHeaderColumnNameVM.PolicyOtherGroupHeaderTableName = policyOtherGroupHeaderTableName;	
			
			policyOtherGroupHeaderColumnNameVM.PolicyOtherGroupHeader = policyOtherGroupHeader;

			return View(policyOtherGroupHeaderColumnNameVM);
		}

		// POST: /PolicyOtherGroupHeaderColumnName/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(PolicyOtherGroupHeaderColumnNameVM policyOtherGroupHeaderColumnNameVM, FormCollection formCollection)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//We need to extract group from groupVM
			PolicyOtherGroupHeaderColumnName policyOtherGroupHeaderColumnName = new PolicyOtherGroupHeaderColumnName();
			policyOtherGroupHeaderColumnName = policyOtherGroupHeaderColumnNameVM.PolicyOtherGroupHeaderColumnName;
			if (policyOtherGroupHeaderColumnName == null)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyOtherGroupHeaderColumnName>(policyOtherGroupHeaderColumnName, "PolicyOtherGroupHeaderColumnName");
			}
			catch
			{
				string n = "";
				foreach (ModelState modelState in ViewData.ModelState.Values)
				{
					foreach (ModelError error in modelState.Errors)
					{
						n += error.ErrorMessage;
					}
				}
				ViewData["Message"] = "ValidationError : " + n;
				return View("Error");
			}

			//Database Update
			try
			{
				policyOtherGroupHeaderColumnNameRepository.Add(policyOtherGroupHeaderColumnNameVM);
			}
			catch (SqlException ex)
			{
				//Non-Unique Name
				if (ex.Message == "NonUniqueName")
				{
					return View("NonUniqueNameError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			ViewData["NewSortOrder"] = 0;
			return RedirectToAction("List", new { id = policyOtherGroupHeaderColumnNameVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId });
		}

		// GET: /PolicyOtherGroupHeaderColumnName/Edit
		public ActionResult Edit(int id)
		{
			//Get Item From Database
			PolicyOtherGroupHeaderColumnName policyOtherGroupHeaderColumnName = new PolicyOtherGroupHeaderColumnName();
			policyOtherGroupHeaderColumnName = policyOtherGroupHeaderColumnNameRepository.GetPolicyOtherGroupHeaderColumnName(id);

			//Check Exists
			if (policyOtherGroupHeaderColumnName == null)
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

			//Get Table Name
			PolicyOtherGroupHeaderTableName policyOtherGroupHeaderTableName = new PolicyOtherGroupHeaderTableName();
			policyOtherGroupHeaderTableName = policyOtherGroupHeaderTableNameRepository.GetPolicyOtherGroupHeaderTableNameByPolicyOtherGroupHeaderTableNameId(policyOtherGroupHeaderColumnName.PolicyOtherGroupHeaderTableNameId);

			//Check Exists
			if (policyOtherGroupHeaderTableName == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}
			
			//Get Item From Database
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyOtherGroupHeaderTableName.PolicyOtherGroupHeaderId);

			//Check Exists
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			PolicyOtherGroupHeaderColumnNameVM policyOtherGroupHeaderColumnNameVM = new PolicyOtherGroupHeaderColumnNameVM();
			policyOtherGroupHeaderColumnNameVM.PolicyOtherGroupHeaderColumnName = policyOtherGroupHeaderColumnName;
			policyOtherGroupHeaderColumnNameVM.PolicyOtherGroupHeaderTableName = policyOtherGroupHeaderTableName; 
			policyOtherGroupHeaderColumnNameVM.PolicyOtherGroupHeader = policyOtherGroupHeader;

			return View(policyOtherGroupHeaderColumnNameVM);
		}

		// POST: /PolicyOtherGroupHeaderColumnName/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(PolicyOtherGroupHeaderColumnNameVM policyOtherGroupHeaderColumnNameVM, FormCollection formCollection)
		{
			//Get Item
			PolicyOtherGroupHeaderColumnName policyOtherGroupHeaderColumnName = new PolicyOtherGroupHeaderColumnName();
			policyOtherGroupHeaderColumnName = policyOtherGroupHeaderColumnNameRepository.GetPolicyOtherGroupHeaderColumnName(policyOtherGroupHeaderColumnNameVM.PolicyOtherGroupHeaderColumnName.PolicyOtherGroupHeaderColumnNameId);

			//Check Exists
			if (policyOtherGroupHeaderColumnName == null)
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

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyOtherGroupHeaderColumnName>(policyOtherGroupHeaderColumnName, "PolicyOtherGroupHeaderColumnName");
			}
			catch
			{
				string n = "";
				foreach (ModelState modelState in ViewData.ModelState.Values)
				{
					foreach (ModelError error in modelState.Errors)
					{
						n += error.ErrorMessage;
					}
				}
				ViewData["Message"] = "ValidationError : " + n;
				return View("Error");
			}

			//Database Update
			try
			{
				policyOtherGroupHeaderColumnNameRepository.Edit(policyOtherGroupHeaderColumnName);
			}
			catch (SqlException ex)
			{
				//Non-Unique Name
				if (ex.Message == "NonUniqueName")
				{
					return View("NonUniqueNameError");
				}
				//Versioning Error
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PolicyOtherGroupHeaderColumnName.mvc/Edit/" +policyOtherGroupHeaderColumnName.PolicyOtherGroupHeaderColumnNameId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { id = policyOtherGroupHeaderColumnNameVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId });
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
		{
			//Get Item From Database
			PolicyOtherGroupHeaderColumnName policyOtherGroupHeaderColumnName = new PolicyOtherGroupHeaderColumnName();
			policyOtherGroupHeaderColumnName = policyOtherGroupHeaderColumnNameRepository.GetPolicyOtherGroupHeaderColumnName(id);

			//Check Exists
			if (policyOtherGroupHeaderColumnName == null)
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

			//Get Table Name
			PolicyOtherGroupHeaderTableName policyOtherGroupHeaderTableName = new PolicyOtherGroupHeaderTableName();
			policyOtherGroupHeaderTableName = policyOtherGroupHeaderTableNameRepository.GetPolicyOtherGroupHeaderTableNameByPolicyOtherGroupHeaderTableNameId(policyOtherGroupHeaderColumnName.PolicyOtherGroupHeaderTableNameId);

			//Check Exists
			if (policyOtherGroupHeaderTableName == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Get Item From Database
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyOtherGroupHeaderTableName.PolicyOtherGroupHeaderId);

			//Check Exists
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			PolicyOtherGroupHeaderColumnNameVM policyOtherGroupHeaderColumnNameVM = new PolicyOtherGroupHeaderColumnNameVM();
			policyOtherGroupHeaderColumnNameVM.PolicyOtherGroupHeaderColumnName = policyOtherGroupHeaderColumnName;
			policyOtherGroupHeaderColumnNameVM.PolicyOtherGroupHeaderTableName = policyOtherGroupHeaderTableName;
			policyOtherGroupHeaderColumnNameVM.PolicyOtherGroupHeader = policyOtherGroupHeader;

			return View(policyOtherGroupHeaderColumnNameVM);
		}

		// POST: /PolicyOtherGroupHeaderColumnName/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(PolicyOtherGroupHeaderColumnNameVM policyOtherGroupHeaderColumnNameVM)
		{
			//Check Valid Item passed in Form       
			if (policyOtherGroupHeaderColumnNameVM.PolicyOtherGroupHeaderColumnName == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Get Item From Database
			PolicyOtherGroupHeaderColumnName policyOtherGroupHeaderColumnName = new PolicyOtherGroupHeaderColumnName();
			policyOtherGroupHeaderColumnName = policyOtherGroupHeaderColumnNameRepository.GetPolicyOtherGroupHeaderColumnName(policyOtherGroupHeaderColumnNameVM.PolicyOtherGroupHeaderColumnName.PolicyOtherGroupHeaderColumnNameId);

			//Check Exists
			if (policyOtherGroupHeaderColumnName == null)
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

			//Delete Form Item
			try
			{
				policyOtherGroupHeaderColumnNameRepository.Delete(policyOtherGroupHeaderColumnNameVM.PolicyOtherGroupHeaderColumnName);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PolicyOtherGroupHeaderColumnName.mvc/Delete/" + policyOtherGroupHeaderColumnNameVM.PolicyOtherGroupHeaderColumnName.PolicyOtherGroupHeaderColumnNameId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List", new { id = policyOtherGroupHeaderColumnNameVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId });
		}

		// GET: /EditSequence
		public ActionResult EditSequence(int id, int? page)
		{
			//Get Item From Database
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(id);

			//Check Exists
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			var items = policyOtherGroupHeaderColumnNameRepository.PagePolicyOtherGroupHeaderColumnNameSequences(page ?? 1, id);

			ViewData["Page"] = page ?? 1;
			ViewData["PolicyOtherGroupHeaderId"] = policyOtherGroupHeader.PolicyOtherGroupHeaderId;
			ViewData["Label"] = policyOtherGroupHeader.Label;

			return View(items);
		}

		// POST: /EditSequence
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditSequence(int page, int id, FormCollection collection)
		{

			//Get Item From Database
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(id);

			//Check Exists
			if (policyOtherGroupHeader == null)
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

				string policyOtherGroupHeaderColumnNameId = primaryKey[0];
				int versionNumber = Convert.ToInt32(primaryKey[1]);

				XmlElement xmlItem = doc.CreateElement("Item");
				root.AppendChild(xmlItem);

				XmlElement xmlSequence = doc.CreateElement("Sequence");
				xmlSequence.InnerText = sequence.ToString();
				xmlItem.AppendChild(xmlSequence);

				XmlElement xmlPolicyOtherGroupHeaderColumnNameId = doc.CreateElement("PolicyOtherGroupHeaderColumnNameId");
				xmlPolicyOtherGroupHeaderColumnNameId.InnerText = policyOtherGroupHeaderColumnNameId.ToString();
				xmlItem.AppendChild(xmlPolicyOtherGroupHeaderColumnNameId);

				XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
				xmlVersionNumber.InnerText = versionNumber.ToString();
				xmlItem.AppendChild(xmlVersionNumber);

				sequence = sequence + 1;
			}

			try
			{
				policyOtherGroupHeaderColumnNameRepository.UpdatePolicyOtherGroupHeaderColumnNameSequences(System.Xml.Linq.XElement.Parse(doc.OuterXml));
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PolicyOtherGroupHeaderColumnName.mvc/EditSequence/" + policyOtherGroupHeader.PolicyOtherGroupHeaderId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List", new { id = policyOtherGroupHeader.PolicyOtherGroupHeaderId });
		}
	}
}
