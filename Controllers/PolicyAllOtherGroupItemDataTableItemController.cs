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
	public class PolicyAllOtherGroupItemDataTableItemController : Controller
	{
		// Main repository
		PolicyOtherGroupHeaderRepository policyOtherGroupHeaderRepository = new PolicyOtherGroupHeaderRepository();
		PolicyAllOtherGroupItemDataTableRowRepository policyAllOtherGroupItemDataTableRowRepository = new PolicyAllOtherGroupItemDataTableRowRepository();
		PolicyAllOtherGroupItemDataTableItemRepository policyAllOtherGroupItemDataTableItemRepository = new PolicyAllOtherGroupItemDataTableItemRepository();
		PolicyOtherGroupHeaderColumnNameRepository policyOtherGroupHeaderColumnNameRepository = new PolicyOtherGroupHeaderColumnNameRepository();

		PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		RolesRepository rolesRepository = new RolesRepository();

		//GET: /PolicyAllOtherGroupItemDataTableItem/List
		public ActionResult List(int id, int policyGroupId, string filter, int? page, string sortField, int? sortOrder)
		{
			PolicyAllOtherGroupItemDataTableItemsVM policyAllOtherGroupItemDataTableItemsVM = new PolicyAllOtherGroupItemDataTableItemsVM();

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("RecordDoesNotExistError");
			}

			policyAllOtherGroupItemDataTableItemsVM.PolicyGroup = policyGroup;

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(id);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("RecordDoesNotExistError");
			}

			policyAllOtherGroupItemDataTableItemsVM.PolicyOtherGroupHeader = policyOtherGroupHeader;

			//Set Access Rights
			policyAllOtherGroupItemDataTableItemsVM.HasWriteAccess = false;
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToPolicyGroup(policyGroupId))
			{
				policyAllOtherGroupItemDataTableItemsVM.HasWriteAccess = true;
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

			DataTable policyAllOtherGroupItemDataTableItems = policyAllOtherGroupItemDataTableItemRepository.GetPolicyAllOtherGroupItemDataTableItems(
				id, policyGroup.PolicyGroupId, filter ?? "", sortField, sortOrder ?? 0, page ?? 1, ref policyAllOtherGroupItemDataTableItemsVM);

			policyAllOtherGroupItemDataTableItemsVM.PolicyAllOtherGroupItemDataTableItems = policyAllOtherGroupItemDataTableItems;

			return View(policyAllOtherGroupItemDataTableItemsVM);
		}

		// GET: /PolicyAllOtherGroupItemDataTableItem/Create
		public ActionResult Create(int id, int policyGroupId)
		{
			PolicyAllOtherGroupItemDataTableItemVM policyAllOtherGroupItemDataTableItemVM = new PolicyAllOtherGroupItemDataTableItemVM();

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

			policyAllOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyAllOtherGroupItemDataTableItemVM.PolicyGroup = policyGroup;

			List<PolicyAllOtherGroupItemDataTableItem> policyAllOtherGroupItemDataTableItems = new List<PolicyAllOtherGroupItemDataTableItem>();
			policyAllOtherGroupItemDataTableItems = policyAllOtherGroupItemDataTableItemRepository.GetPolicyAllOtherGroupItemDataTableItems(
				policyGroup.PolicyGroupId,	
				policyOtherGroupHeader.PolicyOtherGroupHeaderId
			);

			if (policyAllOtherGroupItemDataTableItems != null)
			{
				policyAllOtherGroupItemDataTableItemVM.PolicyAllOtherGroupItemDataTableItems = policyAllOtherGroupItemDataTableItems;
			}

			return View(policyAllOtherGroupItemDataTableItemVM);
		}

		// POST: /PolicyAllOtherGroupItemDataTableItem/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(PolicyAllOtherGroupItemDataTableItemVM policyAllOtherGroupItemDataTableItemVM, FormCollection formCollection)
		{
			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyAllOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyAllOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//We need to extract group from groupVM
			List<PolicyAllOtherGroupItemDataTableItem> policyAllOtherGroupItemDataTableItems = new List<PolicyAllOtherGroupItemDataTableItem>();
			foreach (string item in formCollection)
			{
				if (item.StartsWith("PolicyOtherGroupHeaderColumnNameId"))
				{
					PolicyAllOtherGroupItemDataTableItem policyAllOtherGroupItemDataTableItem = new PolicyAllOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = Int32.Parse(item.Replace("PolicyOtherGroupHeaderColumnNameId_", "")),
						TableDataItem = formCollection[item]
					};
					policyAllOtherGroupItemDataTableItems.Add(policyAllOtherGroupItemDataTableItem);
				}
			}
			if (policyAllOtherGroupItemDataTableItems.Count() <= 0)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			policyAllOtherGroupItemDataTableItemVM.PolicyAllOtherGroupItemDataTableItems = policyAllOtherGroupItemDataTableItems;

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyAllOtherGroupItemDataTableItemVM>(policyAllOtherGroupItemDataTableItemVM, "PolicyAllOtherGroupItemDataTableItemVM");
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
				policyAllOtherGroupItemDataTableItemRepository.Add(policyAllOtherGroupItemDataTableItemVM);
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
			return RedirectToAction("List", new { id = policyAllOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policyAllOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId });
		}

		// GET: /PolicyAllOtherGroupItemDataTableItem/Edit
		public ActionResult Edit(int id, int policyGroupId, int policyOtherGroupHeaderId)
		{
			PolicyAllOtherGroupItemDataTableItemVM policyAllOtherGroupItemDataTableItemVM = new PolicyAllOtherGroupItemDataTableItemVM();

			//Check PolicyAllOtherGroupItemDataTableRow Exists
			PolicyAllOtherGroupItemDataTableRow policyAllOtherGroupItemDataTableRow = new PolicyAllOtherGroupItemDataTableRow();
			policyAllOtherGroupItemDataTableRow = policyAllOtherGroupItemDataTableRowRepository.GetPolicyAllOtherGroupItemDataTableRow(id);
			if (policyAllOtherGroupItemDataTableRow == null)
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

			policyAllOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyAllOtherGroupItemDataTableItemVM.PolicyGroup = policyGroup;

			policyAllOtherGroupItemDataTableItemVM.PolicyAllOtherGroupItemDataTableRow = policyAllOtherGroupItemDataTableRow;

			List<PolicyAllOtherGroupItemDataTableItem> policyAllOtherGroupItemDataTableItems = new List<PolicyAllOtherGroupItemDataTableItem>();
			policyAllOtherGroupItemDataTableItems = policyAllOtherGroupItemDataTableRowRepository.GetPolicyAllOtherGroupItemDataTableItems(id, policyOtherGroupHeaderId);
			if (policyAllOtherGroupItemDataTableItems != null)
			{
				policyAllOtherGroupItemDataTableItemVM.PolicyAllOtherGroupItemDataTableItems = policyAllOtherGroupItemDataTableItems;
			}

			return View(policyAllOtherGroupItemDataTableItemVM);
		}

		// POST: /PolicyAllOtherGroupItemDataTableItem/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(PolicyAllOtherGroupItemDataTableItemVM policyAllOtherGroupItemDataTableItemVM, FormCollection formCollection)
		{
			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyAllOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyAllOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//We need to extract group from groupVM
			List<PolicyAllOtherGroupItemDataTableItem> policyAllOtherGroupItemDataTableItems = new List<PolicyAllOtherGroupItemDataTableItem>();
			foreach (string item in formCollection)
			{
				if (item.StartsWith("PolicyOtherGroupHeaderColumnNameId"))
				{
					PolicyAllOtherGroupItemDataTableItem policyAllOtherGroupItemDataTableItem = new PolicyAllOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = Int32.Parse(item.Replace("PolicyOtherGroupHeaderColumnNameId_", "")),
						TableDataItem = formCollection[item]
					};
					policyAllOtherGroupItemDataTableItems.Add(policyAllOtherGroupItemDataTableItem);
				}
			}
			if (policyAllOtherGroupItemDataTableItems.Count() <= 0)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			policyAllOtherGroupItemDataTableItemVM.PolicyAllOtherGroupItemDataTableItems = policyAllOtherGroupItemDataTableItems;

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyAllOtherGroupItemDataTableItemVM>(policyAllOtherGroupItemDataTableItemVM, "PolicyAllOtherGroupItemDataTableItemVM");
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
				policyAllOtherGroupItemDataTableItemRepository.Edit(policyAllOtherGroupItemDataTableItemVM);
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
			return RedirectToAction("List", new { id = policyAllOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policyAllOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId });
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, int policyGroupId, int policyOtherGroupHeaderId)
		{
			PolicyAllOtherGroupItemDataTableItemVM policyAllOtherGroupItemDataTableItemVM = new PolicyAllOtherGroupItemDataTableItemVM();

			//Check PolicyAllOtherGroupItemDataTableRow Exists
			PolicyAllOtherGroupItemDataTableRow policyAllOtherGroupItemDataTableRow = new PolicyAllOtherGroupItemDataTableRow();
			policyAllOtherGroupItemDataTableRow = policyAllOtherGroupItemDataTableRowRepository.GetPolicyAllOtherGroupItemDataTableRow(id);
			if (policyAllOtherGroupItemDataTableRow == null)
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

			policyAllOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyAllOtherGroupItemDataTableItemVM.PolicyGroup = policyGroup;

			policyAllOtherGroupItemDataTableItemVM.PolicyAllOtherGroupItemDataTableRow = policyAllOtherGroupItemDataTableRow;

			List<PolicyAllOtherGroupItemDataTableItem> policyAllOtherGroupItemDataTableItems = new List<PolicyAllOtherGroupItemDataTableItem>();
			policyAllOtherGroupItemDataTableItems = policyAllOtherGroupItemDataTableRowRepository.GetPolicyAllOtherGroupItemDataTableItems(id, policyOtherGroupHeaderId);
			if (policyAllOtherGroupItemDataTableItems != null)
			{
				policyAllOtherGroupItemDataTableItemVM.PolicyAllOtherGroupItemDataTableItems = policyAllOtherGroupItemDataTableItems;
			}

			return View(policyAllOtherGroupItemDataTableItemVM);
		}

		// POST: /PolicyAllOtherGroupItemDataTableItem/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(PolicyAllOtherGroupItemDataTableItemVM policyAllOtherGroupItemDataTableItemVM)
		{
			//Check PolicyAllOtherGroupItemDataTableRow Exists
			PolicyAllOtherGroupItemDataTableRow policyAllOtherGroupItemDataTableRow = new PolicyAllOtherGroupItemDataTableRow();
			policyAllOtherGroupItemDataTableRow = policyAllOtherGroupItemDataTableRowRepository.GetPolicyAllOtherGroupItemDataTableRow(
				policyAllOtherGroupItemDataTableItemVM.PolicyAllOtherGroupItemDataTableRow.PolicyAllOtherGroupItemDataTableRowId
			);
			if (policyAllOtherGroupItemDataTableRow == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyAllOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(
				policyAllOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId
			);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Delete Form Item
			try
			{
				policyAllOtherGroupItemDataTableItemRepository.Delete(policyAllOtherGroupItemDataTableRow);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PolicyAllOtherGroupItemDataTableItem.mvc/Delete/" + policyAllOtherGroupItemDataTableItemVM.PolicyAllOtherGroupItemDataTableRow.PolicyAllOtherGroupItemDataTableRowId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List", new { id = policyAllOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policyAllOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId });
		}
	}
}
