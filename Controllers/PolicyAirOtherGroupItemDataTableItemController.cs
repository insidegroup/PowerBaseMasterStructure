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
	public class PolicyAirOtherGroupItemDataTableItemController : Controller
	{
		// Main repository
		PolicyOtherGroupHeaderRepository policyOtherGroupHeaderRepository = new PolicyOtherGroupHeaderRepository();
		PolicyAirOtherGroupItemDataTableRowRepository policyAirOtherGroupItemDataTableRowRepository = new PolicyAirOtherGroupItemDataTableRowRepository();
		PolicyAirOtherGroupItemDataTableItemRepository policyAirOtherGroupItemDataTableItemRepository = new PolicyAirOtherGroupItemDataTableItemRepository();
		PolicyOtherGroupHeaderColumnNameRepository policyOtherGroupHeaderColumnNameRepository = new PolicyOtherGroupHeaderColumnNameRepository();

		PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		RolesRepository rolesRepository = new RolesRepository();

		//GET: /PolicyAirOtherGroupItemDataTableItem/List
		public ActionResult List(int id, int policyGroupId, string filter, int? page, string sortField, int? sortOrder)
		{
			PolicyAirOtherGroupItemDataTableItemsVM policyAirOtherGroupItemDataTableItemsVM = new PolicyAirOtherGroupItemDataTableItemsVM();

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("RecordDoesNotExistError");
			}

			policyAirOtherGroupItemDataTableItemsVM.PolicyGroup = policyGroup;

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(id);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("RecordDoesNotExistError");
			}

			policyAirOtherGroupItemDataTableItemsVM.PolicyOtherGroupHeader = policyOtherGroupHeader;

			//Set Access Rights
			policyAirOtherGroupItemDataTableItemsVM.HasWriteAccess = false;
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToPolicyGroup(policyGroupId))
			{
				policyAirOtherGroupItemDataTableItemsVM.HasWriteAccess = true;
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

			DataTable policyAirOtherGroupItemDataTableItems = policyAirOtherGroupItemDataTableItemRepository.GetPolicyAirOtherGroupItemDataTableItems(
				id, policyGroup.PolicyGroupId, filter ?? "", sortField, sortOrder ?? 0, page ?? 1, ref policyAirOtherGroupItemDataTableItemsVM);

			policyAirOtherGroupItemDataTableItemsVM.PolicyAirOtherGroupItemDataTableItems = policyAirOtherGroupItemDataTableItems;

			return View(policyAirOtherGroupItemDataTableItemsVM);
		}

		// GET: /PolicyAirOtherGroupItemDataTableItem/Create
		public ActionResult Create(int id, int policyGroupId)
		{
			PolicyAirOtherGroupItemDataTableItemVM policyAirOtherGroupItemDataTableItemVM = new PolicyAirOtherGroupItemDataTableItemVM();

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

			policyAirOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyAirOtherGroupItemDataTableItemVM.PolicyGroup = policyGroup;

			List<PolicyAirOtherGroupItemDataTableItem> policyAirOtherGroupItemDataTableItems = new List<PolicyAirOtherGroupItemDataTableItem>();
			policyAirOtherGroupItemDataTableItems = policyAirOtherGroupItemDataTableItemRepository.GetPolicyAirOtherGroupItemDataTableItems(
				policyGroup.PolicyGroupId,	
				policyOtherGroupHeader.PolicyOtherGroupHeaderId
			);

			if (policyAirOtherGroupItemDataTableItems != null)
			{
				policyAirOtherGroupItemDataTableItemVM.PolicyAirOtherGroupItemDataTableItems = policyAirOtherGroupItemDataTableItems;
			}

			return View(policyAirOtherGroupItemDataTableItemVM);
		}

		// POST: /PolicyAirOtherGroupItemDataTableItem/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(PolicyAirOtherGroupItemDataTableItemVM policyAirOtherGroupItemDataTableItemVM, FormCollection formCollection)
		{
			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyAirOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyAirOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//We need to extract group from groupVM
			List<PolicyAirOtherGroupItemDataTableItem> policyAirOtherGroupItemDataTableItems = new List<PolicyAirOtherGroupItemDataTableItem>();
			foreach (string item in formCollection)
			{
				if (item.StartsWith("PolicyOtherGroupHeaderColumnNameId"))
				{
					PolicyAirOtherGroupItemDataTableItem policyAirOtherGroupItemDataTableItem = new PolicyAirOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = Int32.Parse(item.Replace("PolicyOtherGroupHeaderColumnNameId_", "")),
						TableDataItem = formCollection[item]
					};
					policyAirOtherGroupItemDataTableItems.Add(policyAirOtherGroupItemDataTableItem);
				}
			}
			if (policyAirOtherGroupItemDataTableItems.Count() <= 0)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			policyAirOtherGroupItemDataTableItemVM.PolicyAirOtherGroupItemDataTableItems = policyAirOtherGroupItemDataTableItems;

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyAirOtherGroupItemDataTableItemVM>(policyAirOtherGroupItemDataTableItemVM, "PolicyAirOtherGroupItemDataTableItemVM");
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
				policyAirOtherGroupItemDataTableItemRepository.Add(policyAirOtherGroupItemDataTableItemVM);
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
			return RedirectToAction("List", new { id = policyAirOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policyAirOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId });
		}

		// GET: /PolicyAirOtherGroupItemDataTableItem/Edit
		public ActionResult Edit(int id, int policyGroupId, int policyOtherGroupHeaderId)
		{
			PolicyAirOtherGroupItemDataTableItemVM policyAirOtherGroupItemDataTableItemVM = new PolicyAirOtherGroupItemDataTableItemVM();

			//Check PolicyAirOtherGroupItemDataTableRow Exists
			PolicyAirOtherGroupItemDataTableRow policyAirOtherGroupItemDataTableRow = new PolicyAirOtherGroupItemDataTableRow();
			policyAirOtherGroupItemDataTableRow = policyAirOtherGroupItemDataTableRowRepository.GetPolicyAirOtherGroupItemDataTableRow(id);
			if (policyAirOtherGroupItemDataTableRow == null)
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

			policyAirOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyAirOtherGroupItemDataTableItemVM.PolicyGroup = policyGroup;

			policyAirOtherGroupItemDataTableItemVM.PolicyAirOtherGroupItemDataTableRow = policyAirOtherGroupItemDataTableRow;

			List<PolicyAirOtherGroupItemDataTableItem> policyAirOtherGroupItemDataTableItems = new List<PolicyAirOtherGroupItemDataTableItem>();
			policyAirOtherGroupItemDataTableItems = policyAirOtherGroupItemDataTableRowRepository.GetPolicyAirOtherGroupItemDataTableItems(id, policyOtherGroupHeaderId);
			if (policyAirOtherGroupItemDataTableItems != null)
			{
				policyAirOtherGroupItemDataTableItemVM.PolicyAirOtherGroupItemDataTableItems = policyAirOtherGroupItemDataTableItems;
			}

			return View(policyAirOtherGroupItemDataTableItemVM);
		}

		// POST: /PolicyAirOtherGroupItemDataTableItem/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(PolicyAirOtherGroupItemDataTableItemVM policyAirOtherGroupItemDataTableItemVM, FormCollection formCollection)
		{
			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyAirOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyAirOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//We need to extract group from groupVM
			List<PolicyAirOtherGroupItemDataTableItem> policyAirOtherGroupItemDataTableItems = new List<PolicyAirOtherGroupItemDataTableItem>();
			foreach (string item in formCollection)
			{
				if (item.StartsWith("PolicyOtherGroupHeaderColumnNameId"))
				{
					PolicyAirOtherGroupItemDataTableItem policyAirOtherGroupItemDataTableItem = new PolicyAirOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = Int32.Parse(item.Replace("PolicyOtherGroupHeaderColumnNameId_", "")),
						TableDataItem = formCollection[item]
					};
					policyAirOtherGroupItemDataTableItems.Add(policyAirOtherGroupItemDataTableItem);
				}
			}
			if (policyAirOtherGroupItemDataTableItems.Count() <= 0)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			policyAirOtherGroupItemDataTableItemVM.PolicyAirOtherGroupItemDataTableItems = policyAirOtherGroupItemDataTableItems;

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyAirOtherGroupItemDataTableItemVM>(policyAirOtherGroupItemDataTableItemVM, "PolicyAirOtherGroupItemDataTableItemVM");
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
				policyAirOtherGroupItemDataTableItemRepository.Edit(policyAirOtherGroupItemDataTableItemVM);
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
			return RedirectToAction("List", new { id = policyAirOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policyAirOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId });
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, int policyGroupId, int policyOtherGroupHeaderId)
		{
			PolicyAirOtherGroupItemDataTableItemVM policyAirOtherGroupItemDataTableItemVM = new PolicyAirOtherGroupItemDataTableItemVM();

			//Check PolicyAirOtherGroupItemDataTableRow Exists
			PolicyAirOtherGroupItemDataTableRow policyAirOtherGroupItemDataTableRow = new PolicyAirOtherGroupItemDataTableRow();
			policyAirOtherGroupItemDataTableRow = policyAirOtherGroupItemDataTableRowRepository.GetPolicyAirOtherGroupItemDataTableRow(id);
			if (policyAirOtherGroupItemDataTableRow == null)
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

			policyAirOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyAirOtherGroupItemDataTableItemVM.PolicyGroup = policyGroup;

			policyAirOtherGroupItemDataTableItemVM.PolicyAirOtherGroupItemDataTableRow = policyAirOtherGroupItemDataTableRow;

			List<PolicyAirOtherGroupItemDataTableItem> policyAirOtherGroupItemDataTableItems = new List<PolicyAirOtherGroupItemDataTableItem>();
			policyAirOtherGroupItemDataTableItems = policyAirOtherGroupItemDataTableRowRepository.GetPolicyAirOtherGroupItemDataTableItems(id, policyOtherGroupHeaderId);
			if (policyAirOtherGroupItemDataTableItems != null)
			{
				policyAirOtherGroupItemDataTableItemVM.PolicyAirOtherGroupItemDataTableItems = policyAirOtherGroupItemDataTableItems;
			}

			return View(policyAirOtherGroupItemDataTableItemVM);
		}

		// POST: /PolicyAirOtherGroupItemDataTableItem/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(PolicyAirOtherGroupItemDataTableItemVM policyAirOtherGroupItemDataTableItemVM)
		{
			//Check PolicyAirOtherGroupItemDataTableRow Exists
			PolicyAirOtherGroupItemDataTableRow policyAirOtherGroupItemDataTableRow = new PolicyAirOtherGroupItemDataTableRow();
			policyAirOtherGroupItemDataTableRow = policyAirOtherGroupItemDataTableRowRepository.GetPolicyAirOtherGroupItemDataTableRow(
				policyAirOtherGroupItemDataTableItemVM.PolicyAirOtherGroupItemDataTableRow.PolicyAirOtherGroupItemDataTableRowId
			);
			if (policyAirOtherGroupItemDataTableRow == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyAirOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(
				policyAirOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId
			);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Delete Form Item
			try
			{
				policyAirOtherGroupItemDataTableItemRepository.Delete(policyAirOtherGroupItemDataTableRow);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PolicyAirOtherGroupItemDataTableItem.mvc/Delete/" + policyAirOtherGroupItemDataTableItemVM.PolicyAirOtherGroupItemDataTableRow.PolicyAirOtherGroupItemDataTableRowId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List", new { id = policyAirOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policyAirOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId });
		}
	}
}
