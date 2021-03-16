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
	public class PolicyPriceTrackingOtherGroupItemDataTableItemController : Controller
	{
		// Main repository
		PolicyOtherGroupHeaderRepository policyOtherGroupHeaderRepository = new PolicyOtherGroupHeaderRepository();
		PolicyPriceTrackingOtherGroupItemDataTableRowRepository PolicyPriceTrackingOtherGroupItemDataTableRowRepository = new PolicyPriceTrackingOtherGroupItemDataTableRowRepository();
		PolicyPriceTrackingOtherGroupItemDataTableItemRepository PolicyPriceTrackingOtherGroupItemDataTableItemRepository = new PolicyPriceTrackingOtherGroupItemDataTableItemRepository();
		PolicyOtherGroupHeaderColumnNameRepository policyOtherGroupHeaderColumnNameRepository = new PolicyOtherGroupHeaderColumnNameRepository();

		PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		RolesRepository rolesRepository = new RolesRepository();

		//GET: /PolicyPriceTrackingOtherGroupItemDataTableItem/List
		public ActionResult List(int id, int policyGroupId, string filter, int? page, string sortField, int? sortOrder)
		{
			PolicyPriceTrackingOtherGroupItemDataTableItemsVM PolicyPriceTrackingOtherGroupItemDataTableItemsVM = new PolicyPriceTrackingOtherGroupItemDataTableItemsVM();

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("RecordDoesNotExistError");
			}

			PolicyPriceTrackingOtherGroupItemDataTableItemsVM.PolicyGroup = policyGroup;

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(id);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("RecordDoesNotExistError");
			}

			PolicyPriceTrackingOtherGroupItemDataTableItemsVM.PolicyOtherGroupHeader = policyOtherGroupHeader;

			//Set Access Rights
			PolicyPriceTrackingOtherGroupItemDataTableItemsVM.HasWriteAccess = false;
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToPolicyGroup(policyGroupId))
			{
				PolicyPriceTrackingOtherGroupItemDataTableItemsVM.HasWriteAccess = true;
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

			DataTable PolicyPriceTrackingOtherGroupItemDataTableItems = PolicyPriceTrackingOtherGroupItemDataTableItemRepository.GetPolicyPriceTrackingOtherGroupItemDataTableItems(
				id, policyGroup.PolicyGroupId, filter ?? "", sortField, sortOrder ?? 0, page ?? 1, ref PolicyPriceTrackingOtherGroupItemDataTableItemsVM);

			PolicyPriceTrackingOtherGroupItemDataTableItemsVM.PolicyPriceTrackingOtherGroupItemDataTableItems = PolicyPriceTrackingOtherGroupItemDataTableItems;

			return View(PolicyPriceTrackingOtherGroupItemDataTableItemsVM);
		}

		// GET: /PolicyPriceTrackingOtherGroupItemDataTableItem/Create
		public ActionResult Create(int id, int policyGroupId)
		{
			PolicyPriceTrackingOtherGroupItemDataTableItemVM PolicyPriceTrackingOtherGroupItemDataTableItemVM = new PolicyPriceTrackingOtherGroupItemDataTableItemVM();

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

			PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyGroup = policyGroup;

			List<PolicyPriceTrackingOtherGroupItemDataTableItem> PolicyPriceTrackingOtherGroupItemDataTableItems = new List<PolicyPriceTrackingOtherGroupItemDataTableItem>();
			PolicyPriceTrackingOtherGroupItemDataTableItems = PolicyPriceTrackingOtherGroupItemDataTableItemRepository.GetPolicyPriceTrackingOtherGroupItemDataTableItems(
				policyGroup.PolicyGroupId,	
				policyOtherGroupHeader.PolicyOtherGroupHeaderId
			);

			if (PolicyPriceTrackingOtherGroupItemDataTableItems != null)
			{
				PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyPriceTrackingOtherGroupItemDataTableItems = PolicyPriceTrackingOtherGroupItemDataTableItems;
			}

			return View(PolicyPriceTrackingOtherGroupItemDataTableItemVM);
		}

		// POST: /PolicyPriceTrackingOtherGroupItemDataTableItem/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(PolicyPriceTrackingOtherGroupItemDataTableItemVM PolicyPriceTrackingOtherGroupItemDataTableItemVM, FormCollection formCollection)
		{
			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//We need to extract group from groupVM
			List<PolicyPriceTrackingOtherGroupItemDataTableItem> PolicyPriceTrackingOtherGroupItemDataTableItems = new List<PolicyPriceTrackingOtherGroupItemDataTableItem>();
			foreach (string item in formCollection)
			{
				if (item.StartsWith("PolicyOtherGroupHeaderColumnNameId"))
				{
					PolicyPriceTrackingOtherGroupItemDataTableItem PolicyPriceTrackingOtherGroupItemDataTableItem = new PolicyPriceTrackingOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = Int32.Parse(item.Replace("PolicyOtherGroupHeaderColumnNameId_", "")),
						TableDataItem = formCollection[item]
					};
					PolicyPriceTrackingOtherGroupItemDataTableItems.Add(PolicyPriceTrackingOtherGroupItemDataTableItem);
				}
			}
			if (PolicyPriceTrackingOtherGroupItemDataTableItems.Count() <= 0)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyPriceTrackingOtherGroupItemDataTableItems = PolicyPriceTrackingOtherGroupItemDataTableItems;

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyPriceTrackingOtherGroupItemDataTableItemVM>(PolicyPriceTrackingOtherGroupItemDataTableItemVM, "PolicyPriceTrackingOtherGroupItemDataTableItemVM");
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
				PolicyPriceTrackingOtherGroupItemDataTableItemRepository.Add(PolicyPriceTrackingOtherGroupItemDataTableItemVM);
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
			return RedirectToAction("List", new { id = PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId });
		}

		// GET: /PolicyPriceTrackingOtherGroupItemDataTableItem/Edit
		public ActionResult Edit(int id, int policyGroupId, int policyOtherGroupHeaderId)
		{
			PolicyPriceTrackingOtherGroupItemDataTableItemVM PolicyPriceTrackingOtherGroupItemDataTableItemVM = new PolicyPriceTrackingOtherGroupItemDataTableItemVM();

			//Check PolicyPriceTrackingOtherGroupItemDataTableRow Exists
			PolicyPriceTrackingOtherGroupItemDataTableRow PolicyPriceTrackingOtherGroupItemDataTableRow = new PolicyPriceTrackingOtherGroupItemDataTableRow();
			PolicyPriceTrackingOtherGroupItemDataTableRow = PolicyPriceTrackingOtherGroupItemDataTableRowRepository.GetPolicyPriceTrackingOtherGroupItemDataTableRow(id);
			if (PolicyPriceTrackingOtherGroupItemDataTableRow == null)
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

			PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyGroup = policyGroup;

			PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyPriceTrackingOtherGroupItemDataTableRow = PolicyPriceTrackingOtherGroupItemDataTableRow;

			List<PolicyPriceTrackingOtherGroupItemDataTableItem> PolicyPriceTrackingOtherGroupItemDataTableItems = new List<PolicyPriceTrackingOtherGroupItemDataTableItem>();
			PolicyPriceTrackingOtherGroupItemDataTableItems = PolicyPriceTrackingOtherGroupItemDataTableRowRepository.GetPolicyPriceTrackingOtherGroupItemDataTableItems(id, policyOtherGroupHeaderId);
			if (PolicyPriceTrackingOtherGroupItemDataTableItems != null)
			{
				PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyPriceTrackingOtherGroupItemDataTableItems = PolicyPriceTrackingOtherGroupItemDataTableItems;
			}

			return View(PolicyPriceTrackingOtherGroupItemDataTableItemVM);
		}

		// POST: /PolicyPriceTrackingOtherGroupItemDataTableItem/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(PolicyPriceTrackingOtherGroupItemDataTableItemVM PolicyPriceTrackingOtherGroupItemDataTableItemVM, FormCollection formCollection)
		{
			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//We need to extract group from groupVM
			List<PolicyPriceTrackingOtherGroupItemDataTableItem> PolicyPriceTrackingOtherGroupItemDataTableItems = new List<PolicyPriceTrackingOtherGroupItemDataTableItem>();
			foreach (string item in formCollection)
			{
				if (item.StartsWith("PolicyOtherGroupHeaderColumnNameId"))
				{
					PolicyPriceTrackingOtherGroupItemDataTableItem PolicyPriceTrackingOtherGroupItemDataTableItem = new PolicyPriceTrackingOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = Int32.Parse(item.Replace("PolicyOtherGroupHeaderColumnNameId_", "")),
						TableDataItem = formCollection[item]
					};
					PolicyPriceTrackingOtherGroupItemDataTableItems.Add(PolicyPriceTrackingOtherGroupItemDataTableItem);
				}
			}
			if (PolicyPriceTrackingOtherGroupItemDataTableItems.Count() <= 0)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyPriceTrackingOtherGroupItemDataTableItems = PolicyPriceTrackingOtherGroupItemDataTableItems;

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyPriceTrackingOtherGroupItemDataTableItemVM>(PolicyPriceTrackingOtherGroupItemDataTableItemVM, "PolicyPriceTrackingOtherGroupItemDataTableItemVM");
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
				PolicyPriceTrackingOtherGroupItemDataTableItemRepository.Edit(PolicyPriceTrackingOtherGroupItemDataTableItemVM);
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
			return RedirectToAction("List", new { id = PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId });
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, int policyGroupId, int policyOtherGroupHeaderId)
		{
			PolicyPriceTrackingOtherGroupItemDataTableItemVM PolicyPriceTrackingOtherGroupItemDataTableItemVM = new PolicyPriceTrackingOtherGroupItemDataTableItemVM();

			//Check PolicyPriceTrackingOtherGroupItemDataTableRow Exists
			PolicyPriceTrackingOtherGroupItemDataTableRow PolicyPriceTrackingOtherGroupItemDataTableRow = new PolicyPriceTrackingOtherGroupItemDataTableRow();
			PolicyPriceTrackingOtherGroupItemDataTableRow = PolicyPriceTrackingOtherGroupItemDataTableRowRepository.GetPolicyPriceTrackingOtherGroupItemDataTableRow(id);
			if (PolicyPriceTrackingOtherGroupItemDataTableRow == null)
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

			PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyGroup = policyGroup;

			PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyPriceTrackingOtherGroupItemDataTableRow = PolicyPriceTrackingOtherGroupItemDataTableRow;

			List<PolicyPriceTrackingOtherGroupItemDataTableItem> PolicyPriceTrackingOtherGroupItemDataTableItems = new List<PolicyPriceTrackingOtherGroupItemDataTableItem>();
			PolicyPriceTrackingOtherGroupItemDataTableItems = PolicyPriceTrackingOtherGroupItemDataTableRowRepository.GetPolicyPriceTrackingOtherGroupItemDataTableItems(id, policyOtherGroupHeaderId);
			if (PolicyPriceTrackingOtherGroupItemDataTableItems != null)
			{
				PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyPriceTrackingOtherGroupItemDataTableItems = PolicyPriceTrackingOtherGroupItemDataTableItems;
			}

			return View(PolicyPriceTrackingOtherGroupItemDataTableItemVM);
		}

		// POST: /PolicyPriceTrackingOtherGroupItemDataTableItem/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(PolicyPriceTrackingOtherGroupItemDataTableItemVM PolicyPriceTrackingOtherGroupItemDataTableItemVM)
		{
			//Check PolicyPriceTrackingOtherGroupItemDataTableRow Exists
			PolicyPriceTrackingOtherGroupItemDataTableRow PolicyPriceTrackingOtherGroupItemDataTableRow = new PolicyPriceTrackingOtherGroupItemDataTableRow();
			PolicyPriceTrackingOtherGroupItemDataTableRow = PolicyPriceTrackingOtherGroupItemDataTableRowRepository.GetPolicyPriceTrackingOtherGroupItemDataTableRow(
				PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyPriceTrackingOtherGroupItemDataTableRow.PolicyPriceTrackingOtherGroupItemDataTableRowId
			);
			if (PolicyPriceTrackingOtherGroupItemDataTableRow == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(
				PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId
			);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Delete Form Item
			try
			{
				PolicyPriceTrackingOtherGroupItemDataTableItemRepository.Delete(PolicyPriceTrackingOtherGroupItemDataTableRow);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PolicyPriceTrackingOtherGroupItemDataTableItem.mvc/Delete/" + PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyPriceTrackingOtherGroupItemDataTableRow.PolicyPriceTrackingOtherGroupItemDataTableRowId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List", new { id = PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId });
		}
	}
}
