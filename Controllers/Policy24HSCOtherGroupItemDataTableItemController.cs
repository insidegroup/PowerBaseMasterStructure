using ClientProfileServiceBusiness;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
	public class Policy24HSCOtherGroupItemDataTableItemController : Controller
	{
		// Main repository
		PolicyOtherGroupHeaderRepository policyOtherGroupHeaderRepository = new PolicyOtherGroupHeaderRepository();
		Policy24HSCOtherGroupItemDataTableRowRepository policy24HSCOtherGroupItemDataTableRowRepository = new Policy24HSCOtherGroupItemDataTableRowRepository();
		Policy24HSCOtherGroupItemDataTableItemRepository policy24HSCOtherGroupItemDataTableItemRepository = new Policy24HSCOtherGroupItemDataTableItemRepository();
		PolicyOtherGroupHeaderColumnNameRepository policyOtherGroupHeaderColumnNameRepository = new PolicyOtherGroupHeaderColumnNameRepository();

		PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		RolesRepository rolesRepository = new RolesRepository();

		//GET: /Policy24HSCOtherGroupItemDataTableItem/List
		public ActionResult List(int id, int policyGroupId, string filter, int? page, string sortField, int? sortOrder)
		{
			Policy24HSCOtherGroupItemDataTableItemsVM policy24HSCOtherGroupItemDataTableItemsVM = new Policy24HSCOtherGroupItemDataTableItemsVM();

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("RecordDoesNotExistError");
			}

			policy24HSCOtherGroupItemDataTableItemsVM.PolicyGroup = policyGroup;

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(id);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("RecordDoesNotExistError");
			}

			policy24HSCOtherGroupItemDataTableItemsVM.PolicyOtherGroupHeader = policyOtherGroupHeader;

			//Set Access Rights
			policy24HSCOtherGroupItemDataTableItemsVM.HasWriteAccess = false;
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToPolicyGroup(policyGroupId))
			{
				policy24HSCOtherGroupItemDataTableItemsVM.HasWriteAccess = true;
			}
			
			//SortField + SortOrder settings
			if (string.IsNullOrEmpty(sortField))
			{
				//Dynamically generated in proc
				sortField = string.Empty;
			}
			ViewData["CurrentSortField"] = sortField;

			if (sortOrder == 1)
			{
				ViewData["NewSortOrder"] = 0;
				ViewData["CurrentSortOrder"] = 1;
			}
			else
			{
				ViewData["NewSortOrder"] = 1;
				ViewData["CurrentSortOrder"] = 0;
			}

			DataTable policy24HSCOtherGroupItemDataTableItems = policy24HSCOtherGroupItemDataTableItemRepository.GetPolicy24HSCOtherGroupItemDataTableItems(
				id, policyGroup.PolicyGroupId, filter ?? "", sortField, sortOrder ?? 0, page ?? 1, ref policy24HSCOtherGroupItemDataTableItemsVM);

			policy24HSCOtherGroupItemDataTableItemsVM.Policy24HSCOtherGroupItemDataTableItems = policy24HSCOtherGroupItemDataTableItems;

			return View(policy24HSCOtherGroupItemDataTableItemsVM);
		}

		// GET: /Policy24HSCOtherGroupItemDataTableItem/Create
		public ActionResult Create(int id, int policyGroupId)
		{
			Policy24HSCOtherGroupItemDataTableItemVM policy24HSCOtherGroupItemDataTableItemVM = new Policy24HSCOtherGroupItemDataTableItemVM();

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(id);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			}

			policy24HSCOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policy24HSCOtherGroupItemDataTableItemVM.PolicyGroup = policyGroup;

			List<Policy24HSCOtherGroupItemDataTableItem> policy24HSCOtherGroupItemDataTableItems = new List<Policy24HSCOtherGroupItemDataTableItem>();
			policy24HSCOtherGroupItemDataTableItems = policy24HSCOtherGroupItemDataTableItemRepository.GetPolicy24HSCOtherGroupItemDataTableItems(
				policyGroup.PolicyGroupId,	
				policyOtherGroupHeader.PolicyOtherGroupHeaderId
			);

			if (policy24HSCOtherGroupItemDataTableItems != null)
			{
				policy24HSCOtherGroupItemDataTableItemVM.Policy24HSCOtherGroupItemDataTableItems = policy24HSCOtherGroupItemDataTableItems;
			}

			return View(policy24HSCOtherGroupItemDataTableItemVM);
		}

		// POST: /Policy24HSCOtherGroupItemDataTableItem/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Policy24HSCOtherGroupItemDataTableItemVM policy24HSCOtherGroupItemDataTableItemVM, FormCollection formCollection)
		{
			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policy24HSCOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policy24HSCOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//We need to extract group from groupVM
			List<Policy24HSCOtherGroupItemDataTableItem> policy24HSCOtherGroupItemDataTableItems = new List<Policy24HSCOtherGroupItemDataTableItem>();
			foreach (string item in formCollection)
			{
				if (item.StartsWith("PolicyOtherGroupHeaderColumnNameId"))
				{
					Policy24HSCOtherGroupItemDataTableItem policy24HSCOtherGroupItemDataTableItem = new Policy24HSCOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = Int32.Parse(item.Replace("PolicyOtherGroupHeaderColumnNameId_", "")),
						TableDataItem = formCollection[item]
					};
					policy24HSCOtherGroupItemDataTableItems.Add(policy24HSCOtherGroupItemDataTableItem);
				}
			}
			if (policy24HSCOtherGroupItemDataTableItems.Count() <= 0)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			policy24HSCOtherGroupItemDataTableItemVM.Policy24HSCOtherGroupItemDataTableItems = policy24HSCOtherGroupItemDataTableItems;

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<Policy24HSCOtherGroupItemDataTableItemVM>(policy24HSCOtherGroupItemDataTableItemVM, "Policy24HSCOtherGroupItemDataTableItemVM");
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
				policy24HSCOtherGroupItemDataTableItemRepository.Add(policy24HSCOtherGroupItemDataTableItemVM);
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
			return RedirectToAction("List", new { id = policy24HSCOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policy24HSCOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId });
		}

		// GET: /Policy24HSCOtherGroupItemDataTableItem/Edit
		public ActionResult Edit(int id, int policyGroupId, int policyOtherGroupHeaderId)
		{
			Policy24HSCOtherGroupItemDataTableItemVM policy24HSCOtherGroupItemDataTableItemVM = new Policy24HSCOtherGroupItemDataTableItemVM();

			//Check Policy24HSCOtherGroupItemDataTableRow Exists
			Policy24HSCOtherGroupItemDataTableRow policy24HSCOtherGroupItemDataTableRow = new Policy24HSCOtherGroupItemDataTableRow();
			policy24HSCOtherGroupItemDataTableRow = policy24HSCOtherGroupItemDataTableRowRepository.GetPolicy24HSCOtherGroupItemDataTableRow(id);
			if (policy24HSCOtherGroupItemDataTableRow == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyOtherGroupHeaderId);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			policy24HSCOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policy24HSCOtherGroupItemDataTableItemVM.PolicyGroup = policyGroup;

			policy24HSCOtherGroupItemDataTableItemVM.Policy24HSCOtherGroupItemDataTableRow = policy24HSCOtherGroupItemDataTableRow;

			List<Policy24HSCOtherGroupItemDataTableItem> policy24HSCOtherGroupItemDataTableItems = new List<Policy24HSCOtherGroupItemDataTableItem>();
			policy24HSCOtherGroupItemDataTableItems = policy24HSCOtherGroupItemDataTableRowRepository.GetPolicy24HSCOtherGroupItemDataTableItems(id, policyOtherGroupHeaderId);
			if (policy24HSCOtherGroupItemDataTableItems != null)
			{
				policy24HSCOtherGroupItemDataTableItemVM.Policy24HSCOtherGroupItemDataTableItems = policy24HSCOtherGroupItemDataTableItems;
			}

			return View(policy24HSCOtherGroupItemDataTableItemVM);
		}

		// POST: /Policy24HSCOtherGroupItemDataTableItem/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Policy24HSCOtherGroupItemDataTableItemVM policy24HSCOtherGroupItemDataTableItemVM, FormCollection formCollection)
		{
			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policy24HSCOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policy24HSCOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//We need to extract group from groupVM
			List<Policy24HSCOtherGroupItemDataTableItem> policy24HSCOtherGroupItemDataTableItems = new List<Policy24HSCOtherGroupItemDataTableItem>();
			foreach (string item in formCollection)
			{
				if (item.StartsWith("PolicyOtherGroupHeaderColumnNameId"))
				{
					Policy24HSCOtherGroupItemDataTableItem policy24HSCOtherGroupItemDataTableItem = new Policy24HSCOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = Int32.Parse(item.Replace("PolicyOtherGroupHeaderColumnNameId_", "")),
						TableDataItem = formCollection[item]
					};
					policy24HSCOtherGroupItemDataTableItems.Add(policy24HSCOtherGroupItemDataTableItem);
				}
			}
			if (policy24HSCOtherGroupItemDataTableItems.Count() <= 0)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			policy24HSCOtherGroupItemDataTableItemVM.Policy24HSCOtherGroupItemDataTableItems = policy24HSCOtherGroupItemDataTableItems;

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<Policy24HSCOtherGroupItemDataTableItemVM>(policy24HSCOtherGroupItemDataTableItemVM, "Policy24HSCOtherGroupItemDataTableItemVM");
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
				policy24HSCOtherGroupItemDataTableItemRepository.Edit(policy24HSCOtherGroupItemDataTableItemVM);
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
			return RedirectToAction("List", new { id = policy24HSCOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policy24HSCOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId });
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, int policyGroupId, int policyOtherGroupHeaderId)
		{
			Policy24HSCOtherGroupItemDataTableItemVM policy24HSCOtherGroupItemDataTableItemVM = new Policy24HSCOtherGroupItemDataTableItemVM();

			//Check Policy24HSCOtherGroupItemDataTableRow Exists
			Policy24HSCOtherGroupItemDataTableRow policy24HSCOtherGroupItemDataTableRow = new Policy24HSCOtherGroupItemDataTableRow();
			policy24HSCOtherGroupItemDataTableRow = policy24HSCOtherGroupItemDataTableRowRepository.GetPolicy24HSCOtherGroupItemDataTableRow(id);
			if (policy24HSCOtherGroupItemDataTableRow == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyOtherGroupHeaderId);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			policy24HSCOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policy24HSCOtherGroupItemDataTableItemVM.PolicyGroup = policyGroup;

			policy24HSCOtherGroupItemDataTableItemVM.Policy24HSCOtherGroupItemDataTableRow = policy24HSCOtherGroupItemDataTableRow;

			List<Policy24HSCOtherGroupItemDataTableItem> policy24HSCOtherGroupItemDataTableItems = new List<Policy24HSCOtherGroupItemDataTableItem>();
			policy24HSCOtherGroupItemDataTableItems = policy24HSCOtherGroupItemDataTableRowRepository.GetPolicy24HSCOtherGroupItemDataTableItems(id, policyOtherGroupHeaderId);
			if (policy24HSCOtherGroupItemDataTableItems != null)
			{
				policy24HSCOtherGroupItemDataTableItemVM.Policy24HSCOtherGroupItemDataTableItems = policy24HSCOtherGroupItemDataTableItems;
			}

			return View(policy24HSCOtherGroupItemDataTableItemVM);
		}

		// POST: /Policy24HSCOtherGroupItemDataTableItem/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(Policy24HSCOtherGroupItemDataTableItemVM policy24HSCOtherGroupItemDataTableItemVM)
		{
			//Check Policy24HSCOtherGroupItemDataTableRow Exists
			Policy24HSCOtherGroupItemDataTableRow policy24HSCOtherGroupItemDataTableRow = new Policy24HSCOtherGroupItemDataTableRow();
			policy24HSCOtherGroupItemDataTableRow = policy24HSCOtherGroupItemDataTableRowRepository.GetPolicy24HSCOtherGroupItemDataTableRow(
				policy24HSCOtherGroupItemDataTableItemVM.Policy24HSCOtherGroupItemDataTableRow.Policy24HSCOtherGroupItemDataTableRowId
			);
			if (policy24HSCOtherGroupItemDataTableRow == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policy24HSCOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(
				policy24HSCOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId
			);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Delete Form Item
			try
			{
				policy24HSCOtherGroupItemDataTableItemRepository.Delete(policy24HSCOtherGroupItemDataTableRow);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/Policy24HSCOtherGroupItemDataTableItem.mvc/Delete/" + policy24HSCOtherGroupItemDataTableItemVM.Policy24HSCOtherGroupItemDataTableRow.Policy24HSCOtherGroupItemDataTableRowId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List", new { id = policy24HSCOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policy24HSCOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId });
		}
	}
}
