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
	public class PolicyHotelOtherGroupItemDataTableItemController : Controller
	{
		// Main repository
		PolicyOtherGroupHeaderRepository policyOtherGroupHeaderRepository = new PolicyOtherGroupHeaderRepository();
		PolicyHotelOtherGroupItemDataTableRowRepository policyHotelOtherGroupItemDataTableRowRepository = new PolicyHotelOtherGroupItemDataTableRowRepository();
		PolicyHotelOtherGroupItemDataTableItemRepository policyHotelOtherGroupItemDataTableItemRepository = new PolicyHotelOtherGroupItemDataTableItemRepository();
		PolicyOtherGroupHeaderColumnNameRepository policyOtherGroupHeaderColumnNameRepository = new PolicyOtherGroupHeaderColumnNameRepository();

		PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		RolesRepository rolesRepository = new RolesRepository();

		//GET: /PolicyHotelOtherGroupItemDataTableItem/List
		public ActionResult List(int id, int policyGroupId, string filter, int? page, string sortField, int? sortOrder)
		{
			PolicyHotelOtherGroupItemDataTableItemsVM policyHotelOtherGroupItemDataTableItemsVM = new PolicyHotelOtherGroupItemDataTableItemsVM();

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("RecordDoesNotExistError");
			}

			policyHotelOtherGroupItemDataTableItemsVM.PolicyGroup = policyGroup;

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(id);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("RecordDoesNotExistError");
			}

			policyHotelOtherGroupItemDataTableItemsVM.PolicyOtherGroupHeader = policyOtherGroupHeader;

			//Set Access Rights
			policyHotelOtherGroupItemDataTableItemsVM.HasWriteAccess = false;
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToPolicyGroup(policyGroupId))
			{
				policyHotelOtherGroupItemDataTableItemsVM.HasWriteAccess = true;
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

			DataTable policyHotelOtherGroupItemDataTableItems = policyHotelOtherGroupItemDataTableItemRepository.GetPolicyHotelOtherGroupItemDataTableItems(
				id, policyGroup.PolicyGroupId, filter ?? "", sortField, sortOrder ?? 0, page ?? 1, ref policyHotelOtherGroupItemDataTableItemsVM);

			policyHotelOtherGroupItemDataTableItemsVM.PolicyHotelOtherGroupItemDataTableItems = policyHotelOtherGroupItemDataTableItems;

			return View(policyHotelOtherGroupItemDataTableItemsVM);
		}

		// GET: /PolicyHotelOtherGroupItemDataTableItem/Create
		public ActionResult Create(int id, int policyGroupId)
		{
			PolicyHotelOtherGroupItemDataTableItemVM policyHotelOtherGroupItemDataTableItemVM = new PolicyHotelOtherGroupItemDataTableItemVM();

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

			policyHotelOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyHotelOtherGroupItemDataTableItemVM.PolicyGroup = policyGroup;

			List<PolicyHotelOtherGroupItemDataTableItem> policyHotelOtherGroupItemDataTableItems = new List<PolicyHotelOtherGroupItemDataTableItem>();
			policyHotelOtherGroupItemDataTableItems = policyHotelOtherGroupItemDataTableItemRepository.GetPolicyHotelOtherGroupItemDataTableItems(
				policyGroup.PolicyGroupId,	
				policyOtherGroupHeader.PolicyOtherGroupHeaderId
			);

			if (policyHotelOtherGroupItemDataTableItems != null)
			{
				policyHotelOtherGroupItemDataTableItemVM.PolicyHotelOtherGroupItemDataTableItems = policyHotelOtherGroupItemDataTableItems;
			}

			return View(policyHotelOtherGroupItemDataTableItemVM);
		}

		// POST: /PolicyHotelOtherGroupItemDataTableItem/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(PolicyHotelOtherGroupItemDataTableItemVM policyHotelOtherGroupItemDataTableItemVM, FormCollection formCollection)
		{
			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyHotelOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyHotelOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//We need to extract group from groupVM
			List<PolicyHotelOtherGroupItemDataTableItem> policyHotelOtherGroupItemDataTableItems = new List<PolicyHotelOtherGroupItemDataTableItem>();
			foreach (string item in formCollection)
			{
				if (item.StartsWith("PolicyOtherGroupHeaderColumnNameId"))
				{
					PolicyHotelOtherGroupItemDataTableItem policyHotelOtherGroupItemDataTableItem = new PolicyHotelOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = Int32.Parse(item.Replace("PolicyOtherGroupHeaderColumnNameId_", "")),
						TableDataItem = formCollection[item]
					};
					policyHotelOtherGroupItemDataTableItems.Add(policyHotelOtherGroupItemDataTableItem);
				}
			}
			if (policyHotelOtherGroupItemDataTableItems.Count() <= 0)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			policyHotelOtherGroupItemDataTableItemVM.PolicyHotelOtherGroupItemDataTableItems = policyHotelOtherGroupItemDataTableItems;

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyHotelOtherGroupItemDataTableItemVM>(policyHotelOtherGroupItemDataTableItemVM, "PolicyHotelOtherGroupItemDataTableItemVM");
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
				policyHotelOtherGroupItemDataTableItemRepository.Add(policyHotelOtherGroupItemDataTableItemVM);
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
			return RedirectToAction("List", new { id = policyHotelOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policyHotelOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId });
		}

		// GET: /PolicyHotelOtherGroupItemDataTableItem/Edit
		public ActionResult Edit(int id, int policyGroupId, int policyOtherGroupHeaderId)
		{
			PolicyHotelOtherGroupItemDataTableItemVM policyHotelOtherGroupItemDataTableItemVM = new PolicyHotelOtherGroupItemDataTableItemVM();

			//Check PolicyHotelOtherGroupItemDataTableRow Exists
			PolicyHotelOtherGroupItemDataTableRow policyHotelOtherGroupItemDataTableRow = new PolicyHotelOtherGroupItemDataTableRow();
			policyHotelOtherGroupItemDataTableRow = policyHotelOtherGroupItemDataTableRowRepository.GetPolicyHotelOtherGroupItemDataTableRow(id);
			if (policyHotelOtherGroupItemDataTableRow == null)
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

			policyHotelOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyHotelOtherGroupItemDataTableItemVM.PolicyGroup = policyGroup;

			policyHotelOtherGroupItemDataTableItemVM.PolicyHotelOtherGroupItemDataTableRow = policyHotelOtherGroupItemDataTableRow;

			List<PolicyHotelOtherGroupItemDataTableItem> policyHotelOtherGroupItemDataTableItems = new List<PolicyHotelOtherGroupItemDataTableItem>();
			policyHotelOtherGroupItemDataTableItems = policyHotelOtherGroupItemDataTableRowRepository.GetPolicyHotelOtherGroupItemDataTableItems(id, policyOtherGroupHeaderId);
			if (policyHotelOtherGroupItemDataTableItems != null)
			{
				policyHotelOtherGroupItemDataTableItemVM.PolicyHotelOtherGroupItemDataTableItems = policyHotelOtherGroupItemDataTableItems;
			}

			return View(policyHotelOtherGroupItemDataTableItemVM);
		}

		// POST: /PolicyHotelOtherGroupItemDataTableItem/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(PolicyHotelOtherGroupItemDataTableItemVM policyHotelOtherGroupItemDataTableItemVM, FormCollection formCollection)
		{
			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyHotelOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyHotelOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//We need to extract group from groupVM
			List<PolicyHotelOtherGroupItemDataTableItem> policyHotelOtherGroupItemDataTableItems = new List<PolicyHotelOtherGroupItemDataTableItem>();
			foreach (string item in formCollection)
			{
				if (item.StartsWith("PolicyOtherGroupHeaderColumnNameId"))
				{
					PolicyHotelOtherGroupItemDataTableItem policyHotelOtherGroupItemDataTableItem = new PolicyHotelOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = Int32.Parse(item.Replace("PolicyOtherGroupHeaderColumnNameId_", "")),
						TableDataItem = formCollection[item]
					};
					policyHotelOtherGroupItemDataTableItems.Add(policyHotelOtherGroupItemDataTableItem);
				}
			}
			if (policyHotelOtherGroupItemDataTableItems.Count() <= 0)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			policyHotelOtherGroupItemDataTableItemVM.PolicyHotelOtherGroupItemDataTableItems = policyHotelOtherGroupItemDataTableItems;

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyHotelOtherGroupItemDataTableItemVM>(policyHotelOtherGroupItemDataTableItemVM, "PolicyHotelOtherGroupItemDataTableItemVM");
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
				policyHotelOtherGroupItemDataTableItemRepository.Edit(policyHotelOtherGroupItemDataTableItemVM);
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
			return RedirectToAction("List", new { id = policyHotelOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policyHotelOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId });
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, int policyGroupId, int policyOtherGroupHeaderId)
		{
			PolicyHotelOtherGroupItemDataTableItemVM policyHotelOtherGroupItemDataTableItemVM = new PolicyHotelOtherGroupItemDataTableItemVM();

			//Check PolicyHotelOtherGroupItemDataTableRow Exists
			PolicyHotelOtherGroupItemDataTableRow policyHotelOtherGroupItemDataTableRow = new PolicyHotelOtherGroupItemDataTableRow();
			policyHotelOtherGroupItemDataTableRow = policyHotelOtherGroupItemDataTableRowRepository.GetPolicyHotelOtherGroupItemDataTableRow(id);
			if (policyHotelOtherGroupItemDataTableRow == null)
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

			policyHotelOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyHotelOtherGroupItemDataTableItemVM.PolicyGroup = policyGroup;

			policyHotelOtherGroupItemDataTableItemVM.PolicyHotelOtherGroupItemDataTableRow = policyHotelOtherGroupItemDataTableRow;

			List<PolicyHotelOtherGroupItemDataTableItem> policyHotelOtherGroupItemDataTableItems = new List<PolicyHotelOtherGroupItemDataTableItem>();
			policyHotelOtherGroupItemDataTableItems = policyHotelOtherGroupItemDataTableRowRepository.GetPolicyHotelOtherGroupItemDataTableItems(id, policyOtherGroupHeaderId);
			if (policyHotelOtherGroupItemDataTableItems != null)
			{
				policyHotelOtherGroupItemDataTableItemVM.PolicyHotelOtherGroupItemDataTableItems = policyHotelOtherGroupItemDataTableItems;
			}

			return View(policyHotelOtherGroupItemDataTableItemVM);
		}

		// POST: /PolicyHotelOtherGroupItemDataTableItem/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(PolicyHotelOtherGroupItemDataTableItemVM policyHotelOtherGroupItemDataTableItemVM)
		{
			//Check PolicyHotelOtherGroupItemDataTableRow Exists
			PolicyHotelOtherGroupItemDataTableRow policyHotelOtherGroupItemDataTableRow = new PolicyHotelOtherGroupItemDataTableRow();
			policyHotelOtherGroupItemDataTableRow = policyHotelOtherGroupItemDataTableRowRepository.GetPolicyHotelOtherGroupItemDataTableRow(
				policyHotelOtherGroupItemDataTableItemVM.PolicyHotelOtherGroupItemDataTableRow.PolicyHotelOtherGroupItemDataTableRowId
			);
			if (policyHotelOtherGroupItemDataTableRow == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyHotelOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(
				policyHotelOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId
			);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Delete Form Item
			try
			{
				policyHotelOtherGroupItemDataTableItemRepository.Delete(policyHotelOtherGroupItemDataTableRow);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PolicyHotelOtherGroupItemDataTableItem.mvc/Delete/" + policyHotelOtherGroupItemDataTableItemVM.PolicyHotelOtherGroupItemDataTableRow.PolicyHotelOtherGroupItemDataTableRowId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List", new { id = policyHotelOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policyHotelOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId });
		}
	}
}
