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
	public class PolicyOtherGroupItemDataTableItemController : Controller
	{
		// Main repository
		PolicyOtherGroupHeaderRepository policyOtherGroupHeaderRepository = new PolicyOtherGroupHeaderRepository();
		PolicyOtherGroupItemDataTableRowRepository policyOtherGroupItemDataTableRowRepository = new PolicyOtherGroupItemDataTableRowRepository();
		PolicyOtherGroupItemDataTableItemRepository policyOtherGroupItemDataTableItemRepository = new PolicyOtherGroupItemDataTableItemRepository();
		PolicyOtherGroupHeaderColumnNameRepository policyOtherGroupHeaderColumnNameRepository = new PolicyOtherGroupHeaderColumnNameRepository();

		PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		RolesRepository rolesRepository = new RolesRepository();

		//GET: /PolicyOtherGroupItemDataTableItem/List
		public ActionResult List(int id, int policyGroupId, string filter, int? page, string sortField, int? sortOrder)
		{
			PolicyOtherGroupItemDataTableItemsVM policyOtherGroupItemDataTableItemsVM = new PolicyOtherGroupItemDataTableItemsVM();

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("RecordDoesNotExistError");
			}

			policyOtherGroupItemDataTableItemsVM.PolicyGroup = policyGroup;

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(id);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("RecordDoesNotExistError");
			}

			policyOtherGroupItemDataTableItemsVM.PolicyOtherGroupHeader = policyOtherGroupHeader;

			//Set Access Rights
			policyOtherGroupItemDataTableItemsVM.HasWriteAccess = false;
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToPolicyGroup(policyGroupId))
			{
				policyOtherGroupItemDataTableItemsVM.HasWriteAccess = true;
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

			DataTable policyOtherGroupItemDataTableItems = policyOtherGroupItemDataTableItemRepository.GetPolicyOtherGroupItemDataTableItems(
				id, policyGroup.PolicyGroupId, filter ?? "", sortField, sortOrder ?? 0, page ?? 1, ref policyOtherGroupItemDataTableItemsVM);

			policyOtherGroupItemDataTableItemsVM.PolicyOtherGroupItemDataTableItems = policyOtherGroupItemDataTableItems;

			return View(policyOtherGroupItemDataTableItemsVM);
		}

		// GET: /PolicyOtherGroupItemDataTableItem/Create
		public ActionResult Create(int id, int policyGroupId)
		{
			PolicyOtherGroupItemDataTableItemVM policyOtherGroupItemDataTableItemVM = new PolicyOtherGroupItemDataTableItemVM();

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

			policyOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyOtherGroupItemDataTableItemVM.PolicyGroup = policyGroup;

			List<PolicyOtherGroupItemDataTableItem> policyOtherGroupItemDataTableItems = new List<PolicyOtherGroupItemDataTableItem>();
			policyOtherGroupItemDataTableItems = policyOtherGroupItemDataTableItemRepository.GetPolicyOtherGroupItemDataTableItems(
				policyGroup.PolicyGroupId,	
				policyOtherGroupHeader.PolicyOtherGroupHeaderId
			);

			if (policyOtherGroupItemDataTableItems != null)
			{
				policyOtherGroupItemDataTableItemVM.PolicyOtherGroupItemDataTableItems = policyOtherGroupItemDataTableItems;
			}

			return View(policyOtherGroupItemDataTableItemVM);
		}

		// POST: /PolicyOtherGroupItemDataTableItem/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(PolicyOtherGroupItemDataTableItemVM policyOtherGroupItemDataTableItemVM, FormCollection formCollection)
		{
			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//We need to extract group from groupVM
			List<PolicyOtherGroupItemDataTableItem> policyOtherGroupItemDataTableItems = new List<PolicyOtherGroupItemDataTableItem>();
			foreach (string item in formCollection)
			{
				if (item.StartsWith("PolicyOtherGroupHeaderColumnNameId"))
				{
					PolicyOtherGroupItemDataTableItem policyOtherGroupItemDataTableItem = new PolicyOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = Int32.Parse(item.Replace("PolicyOtherGroupHeaderColumnNameId_", "")),
						TableDataItem = formCollection[item]
					};
					policyOtherGroupItemDataTableItems.Add(policyOtherGroupItemDataTableItem);
				}
			}
			if (policyOtherGroupItemDataTableItems.Count() <= 0)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			policyOtherGroupItemDataTableItemVM.PolicyOtherGroupItemDataTableItems = policyOtherGroupItemDataTableItems;

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyOtherGroupItemDataTableItemVM>(policyOtherGroupItemDataTableItemVM, "PolicyOtherGroupItemDataTableItemVM");
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
				policyOtherGroupItemDataTableItemRepository.Add(policyOtherGroupItemDataTableItemVM);
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
			return RedirectToAction("List", new { id = policyOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policyOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId });
		}

		// GET: /PolicyOtherGroupItemDataTableItem/Edit
		public ActionResult Edit(int id, int policyGroupId, int policyOtherGroupHeaderId)
		{
			PolicyOtherGroupItemDataTableItemVM policyOtherGroupItemDataTableItemVM = new PolicyOtherGroupItemDataTableItemVM();

			//Check PolicyOtherGroupItemDataTableRow Exists
			PolicyOtherGroupItemDataTableRow policyOtherGroupItemDataTableRow = new PolicyOtherGroupItemDataTableRow();
			policyOtherGroupItemDataTableRow = policyOtherGroupItemDataTableRowRepository.GetPolicyOtherGroupItemDataTableRow(id);
			if (policyOtherGroupItemDataTableRow == null)
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

			policyOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyOtherGroupItemDataTableItemVM.PolicyGroup = policyGroup;

			policyOtherGroupItemDataTableItemVM.PolicyOtherGroupItemDataTableRow = policyOtherGroupItemDataTableRow;

			List<PolicyOtherGroupItemDataTableItem> policyOtherGroupItemDataTableItems = new List<PolicyOtherGroupItemDataTableItem>();
			policyOtherGroupItemDataTableItems = policyOtherGroupItemDataTableRowRepository.GetPolicyOtherGroupItemDataTableItems(id, policyOtherGroupHeaderId);
			if (policyOtherGroupItemDataTableItems != null)
			{
				policyOtherGroupItemDataTableItemVM.PolicyOtherGroupItemDataTableItems = policyOtherGroupItemDataTableItems;
			}

			return View(policyOtherGroupItemDataTableItemVM);
		}

		// POST: /PolicyOtherGroupItemDataTableItem/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(PolicyOtherGroupItemDataTableItemVM policyOtherGroupItemDataTableItemVM, FormCollection formCollection)
		{
			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//We need to extract group from groupVM
			List<PolicyOtherGroupItemDataTableItem> policyOtherGroupItemDataTableItems = new List<PolicyOtherGroupItemDataTableItem>();
			foreach (string item in formCollection)
			{
				if (item.StartsWith("PolicyOtherGroupHeaderColumnNameId"))
				{
					PolicyOtherGroupItemDataTableItem policyOtherGroupItemDataTableItem = new PolicyOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = Int32.Parse(item.Replace("PolicyOtherGroupHeaderColumnNameId_", "")),
						TableDataItem = formCollection[item]
					};
					policyOtherGroupItemDataTableItems.Add(policyOtherGroupItemDataTableItem);
				}
			}
			if (policyOtherGroupItemDataTableItems.Count() <= 0)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			policyOtherGroupItemDataTableItemVM.PolicyOtherGroupItemDataTableItems = policyOtherGroupItemDataTableItems;

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyOtherGroupItemDataTableItemVM>(policyOtherGroupItemDataTableItemVM, "PolicyOtherGroupItemDataTableItemVM");
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
				policyOtherGroupItemDataTableItemRepository.Edit(policyOtherGroupItemDataTableItemVM);
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
			return RedirectToAction("List", new { id = policyOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policyOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId });
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, int policyGroupId, int policyOtherGroupHeaderId)
		{
			PolicyOtherGroupItemDataTableItemVM policyOtherGroupItemDataTableItemVM = new PolicyOtherGroupItemDataTableItemVM();

			//Check PolicyOtherGroupItemDataTableRow Exists
			PolicyOtherGroupItemDataTableRow policyOtherGroupItemDataTableRow = new PolicyOtherGroupItemDataTableRow();
			policyOtherGroupItemDataTableRow = policyOtherGroupItemDataTableRowRepository.GetPolicyOtherGroupItemDataTableRow(id);
			if (policyOtherGroupItemDataTableRow == null)
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

			policyOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyOtherGroupItemDataTableItemVM.PolicyGroup = policyGroup;

			policyOtherGroupItemDataTableItemVM.PolicyOtherGroupItemDataTableRow = policyOtherGroupItemDataTableRow;

			List<PolicyOtherGroupItemDataTableItem> policyOtherGroupItemDataTableItems = new List<PolicyOtherGroupItemDataTableItem>();
			policyOtherGroupItemDataTableItems = policyOtherGroupItemDataTableRowRepository.GetPolicyOtherGroupItemDataTableItems(id, policyOtherGroupHeaderId);
			if (policyOtherGroupItemDataTableItems != null)
			{
				policyOtherGroupItemDataTableItemVM.PolicyOtherGroupItemDataTableItems = policyOtherGroupItemDataTableItems;
			}

			return View(policyOtherGroupItemDataTableItemVM);
		}

		// POST: /PolicyOtherGroupItemDataTableItem/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(PolicyOtherGroupItemDataTableItemVM policyOtherGroupItemDataTableItemVM)
		{
			//Check PolicyOtherGroupItemDataTableRow Exists
			PolicyOtherGroupItemDataTableRow policyOtherGroupItemDataTableRow = new PolicyOtherGroupItemDataTableRow();
			policyOtherGroupItemDataTableRow = policyOtherGroupItemDataTableRowRepository.GetPolicyOtherGroupItemDataTableRow(
				policyOtherGroupItemDataTableItemVM.PolicyOtherGroupItemDataTableRow.PolicyOtherGroupItemDataTableRowId
			);
			if (policyOtherGroupItemDataTableRow == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(
				policyOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId
			);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Delete Form Item
			try
			{
				policyOtherGroupItemDataTableItemRepository.Delete(policyOtherGroupItemDataTableRow);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PolicyOtherGroupItemDataTableItem.mvc/Delete/" + policyOtherGroupItemDataTableItemVM.PolicyOtherGroupItemDataTableRow.PolicyOtherGroupItemDataTableRowId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List", new { id = policyOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policyOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId });
		}
	}
}
