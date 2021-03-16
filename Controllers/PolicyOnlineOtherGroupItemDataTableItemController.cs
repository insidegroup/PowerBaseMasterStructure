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
	public class PolicyOnlineOtherGroupItemDataTableItemController : Controller
	{
		// Main repository
		PolicyOtherGroupHeaderRepository policyOtherGroupHeaderRepository = new PolicyOtherGroupHeaderRepository();
		PolicyOnlineOtherGroupItemDataTableRowRepository PolicyOnlineOtherGroupItemDataTableRowRepository = new PolicyOnlineOtherGroupItemDataTableRowRepository();
		PolicyOnlineOtherGroupItemDataTableItemRepository PolicyOnlineOtherGroupItemDataTableItemRepository = new PolicyOnlineOtherGroupItemDataTableItemRepository();
		PolicyOtherGroupHeaderColumnNameRepository policyOtherGroupHeaderColumnNameRepository = new PolicyOtherGroupHeaderColumnNameRepository();

		PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		RolesRepository rolesRepository = new RolesRepository();

		//GET: /PolicyOnlineOtherGroupItemDataTableItem/List
		public ActionResult List(int id, int policyGroupId, string filter, int? page, string sortField, int? sortOrder)
		{
			PolicyOnlineOtherGroupItemDataTableItemsVM PolicyOnlineOtherGroupItemDataTableItemsVM = new PolicyOnlineOtherGroupItemDataTableItemsVM();

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("RecordDoesNotExistError");
			}

			PolicyOnlineOtherGroupItemDataTableItemsVM.PolicyGroup = policyGroup;

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(id);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("RecordDoesNotExistError");
			}

			PolicyOnlineOtherGroupItemDataTableItemsVM.PolicyOtherGroupHeader = policyOtherGroupHeader;

			//Set Access Rights
			PolicyOnlineOtherGroupItemDataTableItemsVM.HasWriteAccess = false;
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToPolicyGroup(policyGroupId) && rolesRepository.HasWriteAccessToPolicyOnlineOtherGroupItemRepository())
			{
				PolicyOnlineOtherGroupItemDataTableItemsVM.HasWriteAccess = true;
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

			DataTable PolicyOnlineOtherGroupItemDataTableItems = PolicyOnlineOtherGroupItemDataTableItemRepository.GetPolicyOnlineOtherGroupItemDataTableItems(
				id, policyGroup.PolicyGroupId, filter ?? "", sortField, sortOrder ?? 0, page ?? 1, ref PolicyOnlineOtherGroupItemDataTableItemsVM);

			PolicyOnlineOtherGroupItemDataTableItemsVM.PolicyOnlineOtherGroupItemDataTableItems = PolicyOnlineOtherGroupItemDataTableItems;

			return View(PolicyOnlineOtherGroupItemDataTableItemsVM);
		}

		// GET: /PolicyOnlineOtherGroupItemDataTableItem/Create
		public ActionResult Create(int id, int policyGroupId)
		{
			PolicyOnlineOtherGroupItemDataTableItemVM PolicyOnlineOtherGroupItemDataTableItemVM = new PolicyOnlineOtherGroupItemDataTableItemVM();

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

			PolicyOnlineOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			PolicyOnlineOtherGroupItemDataTableItemVM.PolicyGroup = policyGroup;

			List<PolicyOnlineOtherGroupItemDataTableItem> PolicyOnlineOtherGroupItemDataTableItems = new List<PolicyOnlineOtherGroupItemDataTableItem>();
			PolicyOnlineOtherGroupItemDataTableItems = PolicyOnlineOtherGroupItemDataTableItemRepository.GetPolicyOnlineOtherGroupItemDataTableItems(
				policyGroup.PolicyGroupId,	
				policyOtherGroupHeader.PolicyOtherGroupHeaderId
			);

			if (PolicyOnlineOtherGroupItemDataTableItems != null)
			{
				PolicyOnlineOtherGroupItemDataTableItemVM.PolicyOnlineOtherGroupItemDataTableItems = PolicyOnlineOtherGroupItemDataTableItems;
			}

			return View(PolicyOnlineOtherGroupItemDataTableItemVM);
		}

		// POST: /PolicyOnlineOtherGroupItemDataTableItem/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(PolicyOnlineOtherGroupItemDataTableItemVM PolicyOnlineOtherGroupItemDataTableItemVM, FormCollection formCollection)
		{
			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(PolicyOnlineOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(PolicyOnlineOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//We need to extract group from groupVM
			List<PolicyOnlineOtherGroupItemDataTableItem> PolicyOnlineOtherGroupItemDataTableItems = new List<PolicyOnlineOtherGroupItemDataTableItem>();
			foreach (string item in formCollection)
			{
				if (item.StartsWith("PolicyOtherGroupHeaderColumnNameId"))
				{
					PolicyOnlineOtherGroupItemDataTableItem PolicyOnlineOtherGroupItemDataTableItem = new PolicyOnlineOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = Int32.Parse(item.Replace("PolicyOtherGroupHeaderColumnNameId_", "")),
						TableDataItem = formCollection[item]
					};
					PolicyOnlineOtherGroupItemDataTableItems.Add(PolicyOnlineOtherGroupItemDataTableItem);
				}
			}
			if (PolicyOnlineOtherGroupItemDataTableItems.Count() <= 0)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			PolicyOnlineOtherGroupItemDataTableItemVM.PolicyOnlineOtherGroupItemDataTableItems = PolicyOnlineOtherGroupItemDataTableItems;

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyOnlineOtherGroupItemDataTableItemVM>(PolicyOnlineOtherGroupItemDataTableItemVM, "PolicyOnlineOtherGroupItemDataTableItemVM");
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
				PolicyOnlineOtherGroupItemDataTableItemRepository.Add(PolicyOnlineOtherGroupItemDataTableItemVM);
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
			return RedirectToAction("List", new { id = PolicyOnlineOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = PolicyOnlineOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId });
		}

		// GET: /PolicyOnlineOtherGroupItemDataTableItem/Edit
		public ActionResult Edit(int id, int policyGroupId, int policyOtherGroupHeaderId)
		{
			PolicyOnlineOtherGroupItemDataTableItemVM PolicyOnlineOtherGroupItemDataTableItemVM = new PolicyOnlineOtherGroupItemDataTableItemVM();

			//Check PolicyOnlineOtherGroupItemDataTableRow Exists
			PolicyOnlineOtherGroupItemDataTableRow PolicyOnlineOtherGroupItemDataTableRow = new PolicyOnlineOtherGroupItemDataTableRow();
			PolicyOnlineOtherGroupItemDataTableRow = PolicyOnlineOtherGroupItemDataTableRowRepository.GetPolicyOnlineOtherGroupItemDataTableRow(id);
			if (PolicyOnlineOtherGroupItemDataTableRow == null)
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

			PolicyOnlineOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			PolicyOnlineOtherGroupItemDataTableItemVM.PolicyGroup = policyGroup;

			PolicyOnlineOtherGroupItemDataTableItemVM.PolicyOnlineOtherGroupItemDataTableRow = PolicyOnlineOtherGroupItemDataTableRow;

			List<PolicyOnlineOtherGroupItemDataTableItem> PolicyOnlineOtherGroupItemDataTableItems = new List<PolicyOnlineOtherGroupItemDataTableItem>();
			PolicyOnlineOtherGroupItemDataTableItems = PolicyOnlineOtherGroupItemDataTableRowRepository.GetPolicyOnlineOtherGroupItemDataTableItems(id, policyOtherGroupHeaderId);
			if (PolicyOnlineOtherGroupItemDataTableItems != null)
			{
				PolicyOnlineOtherGroupItemDataTableItemVM.PolicyOnlineOtherGroupItemDataTableItems = PolicyOnlineOtherGroupItemDataTableItems;
			}

			return View(PolicyOnlineOtherGroupItemDataTableItemVM);
		}

		// POST: /PolicyOnlineOtherGroupItemDataTableItem/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(PolicyOnlineOtherGroupItemDataTableItemVM PolicyOnlineOtherGroupItemDataTableItemVM, FormCollection formCollection)
		{
			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(PolicyOnlineOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(PolicyOnlineOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//We need to extract group from groupVM
			List<PolicyOnlineOtherGroupItemDataTableItem> PolicyOnlineOtherGroupItemDataTableItems = new List<PolicyOnlineOtherGroupItemDataTableItem>();
			foreach (string item in formCollection)
			{
				if (item.StartsWith("PolicyOtherGroupHeaderColumnNameId"))
				{
					PolicyOnlineOtherGroupItemDataTableItem PolicyOnlineOtherGroupItemDataTableItem = new PolicyOnlineOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = Int32.Parse(item.Replace("PolicyOtherGroupHeaderColumnNameId_", "")),
						TableDataItem = formCollection[item]
					};
					PolicyOnlineOtherGroupItemDataTableItems.Add(PolicyOnlineOtherGroupItemDataTableItem);
				}
			}
			if (PolicyOnlineOtherGroupItemDataTableItems.Count() <= 0)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			PolicyOnlineOtherGroupItemDataTableItemVM.PolicyOnlineOtherGroupItemDataTableItems = PolicyOnlineOtherGroupItemDataTableItems;

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyOnlineOtherGroupItemDataTableItemVM>(PolicyOnlineOtherGroupItemDataTableItemVM, "PolicyOnlineOtherGroupItemDataTableItemVM");
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
				PolicyOnlineOtherGroupItemDataTableItemRepository.Edit(PolicyOnlineOtherGroupItemDataTableItemVM);
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
			return RedirectToAction("List", new { id = PolicyOnlineOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = PolicyOnlineOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId });
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, int policyGroupId, int policyOtherGroupHeaderId)
		{
			PolicyOnlineOtherGroupItemDataTableItemVM PolicyOnlineOtherGroupItemDataTableItemVM = new PolicyOnlineOtherGroupItemDataTableItemVM();

			//Check PolicyOnlineOtherGroupItemDataTableRow Exists
			PolicyOnlineOtherGroupItemDataTableRow PolicyOnlineOtherGroupItemDataTableRow = new PolicyOnlineOtherGroupItemDataTableRow();
			PolicyOnlineOtherGroupItemDataTableRow = PolicyOnlineOtherGroupItemDataTableRowRepository.GetPolicyOnlineOtherGroupItemDataTableRow(id);
			if (PolicyOnlineOtherGroupItemDataTableRow == null)
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

			PolicyOnlineOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			PolicyOnlineOtherGroupItemDataTableItemVM.PolicyGroup = policyGroup;

			PolicyOnlineOtherGroupItemDataTableItemVM.PolicyOnlineOtherGroupItemDataTableRow = PolicyOnlineOtherGroupItemDataTableRow;

			List<PolicyOnlineOtherGroupItemDataTableItem> PolicyOnlineOtherGroupItemDataTableItems = new List<PolicyOnlineOtherGroupItemDataTableItem>();
			PolicyOnlineOtherGroupItemDataTableItems = PolicyOnlineOtherGroupItemDataTableRowRepository.GetPolicyOnlineOtherGroupItemDataTableItems(id, policyOtherGroupHeaderId);
			if (PolicyOnlineOtherGroupItemDataTableItems != null)
			{
				PolicyOnlineOtherGroupItemDataTableItemVM.PolicyOnlineOtherGroupItemDataTableItems = PolicyOnlineOtherGroupItemDataTableItems;
			}

			return View(PolicyOnlineOtherGroupItemDataTableItemVM);
		}

		// POST: /PolicyOnlineOtherGroupItemDataTableItem/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(PolicyOnlineOtherGroupItemDataTableItemVM PolicyOnlineOtherGroupItemDataTableItemVM)
		{
			//Check PolicyOnlineOtherGroupItemDataTableRow Exists
			PolicyOnlineOtherGroupItemDataTableRow PolicyOnlineOtherGroupItemDataTableRow = new PolicyOnlineOtherGroupItemDataTableRow();
			PolicyOnlineOtherGroupItemDataTableRow = PolicyOnlineOtherGroupItemDataTableRowRepository.GetPolicyOnlineOtherGroupItemDataTableRow(
				PolicyOnlineOtherGroupItemDataTableItemVM.PolicyOnlineOtherGroupItemDataTableRow.PolicyOnlineOtherGroupItemDataTableRowId
			);
			if (PolicyOnlineOtherGroupItemDataTableRow == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(PolicyOnlineOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(
				PolicyOnlineOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId
			);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Delete Form Item
			try
			{
				PolicyOnlineOtherGroupItemDataTableItemRepository.Delete(PolicyOnlineOtherGroupItemDataTableRow);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PolicyOnlineOtherGroupItemDataTableItem.mvc/Delete/" + PolicyOnlineOtherGroupItemDataTableItemVM.PolicyOnlineOtherGroupItemDataTableRow.PolicyOnlineOtherGroupItemDataTableRowId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List", new { id = PolicyOnlineOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = PolicyOnlineOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId });
		}
	}
}
