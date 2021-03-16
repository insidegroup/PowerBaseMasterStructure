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
	public class PolicyCarOtherGroupItemDataTableItemController : Controller
	{
		// Main repository
		PolicyOtherGroupHeaderRepository policyOtherGroupHeaderRepository = new PolicyOtherGroupHeaderRepository();
		PolicyCarOtherGroupItemDataTableRowRepository policyCarOtherGroupItemDataTableRowRepository = new PolicyCarOtherGroupItemDataTableRowRepository();
		PolicyCarOtherGroupItemDataTableItemRepository policyCarOtherGroupItemDataTableItemRepository = new PolicyCarOtherGroupItemDataTableItemRepository();
		PolicyOtherGroupHeaderColumnNameRepository policyOtherGroupHeaderColumnNameRepository = new PolicyOtherGroupHeaderColumnNameRepository();

		PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		RolesRepository rolesRepository = new RolesRepository();
		
		//GET: /PolicyCarOtherGroupItemDataTableItem/List
		public ActionResult List(int id, int policyGroupId, string filter, int? page, string sortField, int? sortOrder)
		{
			PolicyCarOtherGroupItemDataTableItemsVM policyCarOtherGroupItemDataTableItemsVM = new PolicyCarOtherGroupItemDataTableItemsVM();

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("RecordDoesNotExistError");
			}

			policyCarOtherGroupItemDataTableItemsVM.PolicyGroup = policyGroup;

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(id);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("RecordDoesNotExistError");
			}

			policyCarOtherGroupItemDataTableItemsVM.PolicyOtherGroupHeader = policyOtherGroupHeader;

			//Set Access Rights
			policyCarOtherGroupItemDataTableItemsVM.HasWriteAccess = false;
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToPolicyGroup(policyGroupId))
			{
				policyCarOtherGroupItemDataTableItemsVM.HasWriteAccess = true;
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

			DataTable policyCarOtherGroupItemDataTableItems = policyCarOtherGroupItemDataTableItemRepository.GetPolicyCarOtherGroupItemDataTableItems(
				id, policyGroup.PolicyGroupId, filter ?? "", sortField, sortOrder ?? 0, page ?? 1, ref policyCarOtherGroupItemDataTableItemsVM);

			policyCarOtherGroupItemDataTableItemsVM.PolicyCarOtherGroupItemDataTableItems = policyCarOtherGroupItemDataTableItems;

			return View(policyCarOtherGroupItemDataTableItemsVM);
		}

		// GET: /PolicyCarOtherGroupItemDataTableItem/Create
		public ActionResult Create(int id, int policyGroupId)
		{
			PolicyCarOtherGroupItemDataTableItemVM policyCarOtherGroupItemDataTableItemVM = new PolicyCarOtherGroupItemDataTableItemVM();

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

			policyCarOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyCarOtherGroupItemDataTableItemVM.PolicyGroup = policyGroup;

			List<PolicyCarOtherGroupItemDataTableItem> policyCarOtherGroupItemDataTableItems = new List<PolicyCarOtherGroupItemDataTableItem>();
			policyCarOtherGroupItemDataTableItems = policyCarOtherGroupItemDataTableItemRepository.GetPolicyCarOtherGroupItemDataTableItems(
				policyGroup.PolicyGroupId,	
				policyOtherGroupHeader.PolicyOtherGroupHeaderId
			);

			if (policyCarOtherGroupItemDataTableItems != null)
			{
				policyCarOtherGroupItemDataTableItemVM.PolicyCarOtherGroupItemDataTableItems = policyCarOtherGroupItemDataTableItems;
			}

			return View(policyCarOtherGroupItemDataTableItemVM);
		}

		// POST: /PolicyCarOtherGroupItemDataTableItem/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(PolicyCarOtherGroupItemDataTableItemVM policyCarOtherGroupItemDataTableItemVM, FormCollection formCollection)
		{
			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyCarOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyCarOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//We need to extract group from groupVM
			List<PolicyCarOtherGroupItemDataTableItem> policyCarOtherGroupItemDataTableItems = new List<PolicyCarOtherGroupItemDataTableItem>();
			foreach (string item in formCollection)
			{
				if (item.StartsWith("PolicyOtherGroupHeaderColumnNameId"))
				{
					PolicyCarOtherGroupItemDataTableItem policyCarOtherGroupItemDataTableItem = new PolicyCarOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = Int32.Parse(item.Replace("PolicyOtherGroupHeaderColumnNameId_", "")),
						TableDataItem = formCollection[item]
					};
					policyCarOtherGroupItemDataTableItems.Add(policyCarOtherGroupItemDataTableItem);
				}
			}
			if (policyCarOtherGroupItemDataTableItems.Count() <= 0)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			policyCarOtherGroupItemDataTableItemVM.PolicyCarOtherGroupItemDataTableItems = policyCarOtherGroupItemDataTableItems;

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyCarOtherGroupItemDataTableItemVM>(policyCarOtherGroupItemDataTableItemVM, "PolicyCarOtherGroupItemDataTableItemVM");
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
				policyCarOtherGroupItemDataTableItemRepository.Add(policyCarOtherGroupItemDataTableItemVM);
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
			return RedirectToAction("List", new { id = policyCarOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policyCarOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId });
		}

		// GET: /PolicyCarOtherGroupItemDataTableItem/Edit
		public ActionResult Edit(int id, int policyGroupId, int policyOtherGroupHeaderId)
		{
			PolicyCarOtherGroupItemDataTableItemVM policyCarOtherGroupItemDataTableItemVM = new PolicyCarOtherGroupItemDataTableItemVM();

			//Check PolicyCarOtherGroupItemDataTableRow Exists
			PolicyCarOtherGroupItemDataTableRow policyCarOtherGroupItemDataTableRow = new PolicyCarOtherGroupItemDataTableRow();
			policyCarOtherGroupItemDataTableRow = policyCarOtherGroupItemDataTableRowRepository.GetPolicyCarOtherGroupItemDataTableRow(id);
			if (policyCarOtherGroupItemDataTableRow == null)
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

			policyCarOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyCarOtherGroupItemDataTableItemVM.PolicyGroup = policyGroup;

			policyCarOtherGroupItemDataTableItemVM.PolicyCarOtherGroupItemDataTableRow = policyCarOtherGroupItemDataTableRow;

			List<PolicyCarOtherGroupItemDataTableItem> policyCarOtherGroupItemDataTableItems = new List<PolicyCarOtherGroupItemDataTableItem>();
			policyCarOtherGroupItemDataTableItems = policyCarOtherGroupItemDataTableRowRepository.GetPolicyCarOtherGroupItemDataTableItems(id, policyOtherGroupHeaderId);
			if (policyCarOtherGroupItemDataTableItems != null)
			{
				policyCarOtherGroupItemDataTableItemVM.PolicyCarOtherGroupItemDataTableItems = policyCarOtherGroupItemDataTableItems;
			}

			return View(policyCarOtherGroupItemDataTableItemVM);
		}

		// POST: /PolicyCarOtherGroupItemDataTableItem/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(PolicyCarOtherGroupItemDataTableItemVM policyCarOtherGroupItemDataTableItemVM, FormCollection formCollection)
		{
			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyCarOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyCarOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//We need to extract group from groupVM
			List<PolicyCarOtherGroupItemDataTableItem> policyCarOtherGroupItemDataTableItems = new List<PolicyCarOtherGroupItemDataTableItem>();
			foreach (string item in formCollection)
			{
				if (item.StartsWith("PolicyOtherGroupHeaderColumnNameId"))
				{
					PolicyCarOtherGroupItemDataTableItem policyCarOtherGroupItemDataTableItem = new PolicyCarOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = Int32.Parse(item.Replace("PolicyOtherGroupHeaderColumnNameId_", "")),
						TableDataItem = formCollection[item]
					};
					policyCarOtherGroupItemDataTableItems.Add(policyCarOtherGroupItemDataTableItem);
				}
			}
			if (policyCarOtherGroupItemDataTableItems.Count() <= 0)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			policyCarOtherGroupItemDataTableItemVM.PolicyCarOtherGroupItemDataTableItems = policyCarOtherGroupItemDataTableItems;

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyCarOtherGroupItemDataTableItemVM>(policyCarOtherGroupItemDataTableItemVM, "PolicyCarOtherGroupItemDataTableItemVM");
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
				policyCarOtherGroupItemDataTableItemRepository.Edit(policyCarOtherGroupItemDataTableItemVM);
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
			return RedirectToAction("List", new { id = policyCarOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policyCarOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId });
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, int policyGroupId, int policyOtherGroupHeaderId)
		{
			PolicyCarOtherGroupItemDataTableItemVM policyCarOtherGroupItemDataTableItemVM = new PolicyCarOtherGroupItemDataTableItemVM();

			//Check PolicyCarOtherGroupItemDataTableRow Exists
			PolicyCarOtherGroupItemDataTableRow policyCarOtherGroupItemDataTableRow = new PolicyCarOtherGroupItemDataTableRow();
			policyCarOtherGroupItemDataTableRow = policyCarOtherGroupItemDataTableRowRepository.GetPolicyCarOtherGroupItemDataTableRow(id);
			if (policyCarOtherGroupItemDataTableRow == null)
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

			policyCarOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyCarOtherGroupItemDataTableItemVM.PolicyGroup = policyGroup;

			policyCarOtherGroupItemDataTableItemVM.PolicyCarOtherGroupItemDataTableRow = policyCarOtherGroupItemDataTableRow;

			List<PolicyCarOtherGroupItemDataTableItem> policyCarOtherGroupItemDataTableItems = new List<PolicyCarOtherGroupItemDataTableItem>();
			policyCarOtherGroupItemDataTableItems = policyCarOtherGroupItemDataTableRowRepository.GetPolicyCarOtherGroupItemDataTableItems(id, policyOtherGroupHeaderId);
			if (policyCarOtherGroupItemDataTableItems != null)
			{
				policyCarOtherGroupItemDataTableItemVM.PolicyCarOtherGroupItemDataTableItems = policyCarOtherGroupItemDataTableItems;
			}

			return View(policyCarOtherGroupItemDataTableItemVM);
		}

		// POST: /PolicyCarOtherGroupItemDataTableItem/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(PolicyCarOtherGroupItemDataTableItemVM policyCarOtherGroupItemDataTableItemVM)
		{
			//Check PolicyCarOtherGroupItemDataTableRow Exists
			PolicyCarOtherGroupItemDataTableRow policyCarOtherGroupItemDataTableRow = new PolicyCarOtherGroupItemDataTableRow();
			policyCarOtherGroupItemDataTableRow = policyCarOtherGroupItemDataTableRowRepository.GetPolicyCarOtherGroupItemDataTableRow(
				policyCarOtherGroupItemDataTableItemVM.PolicyCarOtherGroupItemDataTableRow.PolicyCarOtherGroupItemDataTableRowId
			);
			if (policyCarOtherGroupItemDataTableRow == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyCarOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(
				policyCarOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId
			);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Delete Form Item
			try
			{
				policyCarOtherGroupItemDataTableItemRepository.Delete(policyCarOtherGroupItemDataTableRow);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PolicyCarOtherGroupItemDataTableItem.mvc/Delete/" + policyCarOtherGroupItemDataTableItemVM.PolicyCarOtherGroupItemDataTableRow.PolicyCarOtherGroupItemDataTableRowId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List", new { id = policyCarOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policyCarOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId });
		}
	}
}
