using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;
using CWTDesktopDatabase.ViewModels;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace CWTDesktopDatabase.Controllers
{
	public class TeamOutOfOfficeGroupController : Controller
	{
		//main repository
		TeamOutOfOfficeGroupRepository teamOutOfOfficeGroupRepository = new TeamOutOfOfficeGroupRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		RolesRepository rolesRepository = new RolesRepository();

		private string groupName = "Client Detail";

		// GET: /ListUnDeleted
		public ActionResult ListUnDeleted(string filter, int? page, string sortField, int? sortOrder)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//Set Import Access Rights
			ViewData["ImportAccess"] = "";
			if (rolesRepository.HasWriteAccessToTeamOutOfOfficeGroup())
			{
				ViewData["ImportAccess"] = "WriteAccess";
			}

			//SortField
			if (string.IsNullOrEmpty(sortField))
			{
				sortField = "TeamOutOfOfficeGroupName";
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
			}

			if (teamOutOfOfficeGroupRepository == null)
			{
				ViewData["ActionMethod"] = "ListUnDeletedGet";
				return View("Error");
			}

			var cwtPaginatedList = teamOutOfOfficeGroupRepository.PageTeamOutOfOfficeGroups(false, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

			if (cwtPaginatedList == null)
			{
				ViewData["ActionMethod"] = "ListUnDeletedGet";
				return View("Error");
			}

			//return items
			return View(cwtPaginatedList);
		}

		// GET: /ListDeleted
		public ActionResult ListDeleted(string filter, int? page, string sortField, int? sortOrder)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//SortField
			if (sortField != "HierarchyType" && sortField != "EnabledDate" && sortField != "LinkedItemCount")
			{
				sortField = "TeamOutOfOfficeGroupName";
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
			}


			//return items
			var cwtPaginatedList = teamOutOfOfficeGroupRepository.PageTeamOutOfOfficeGroups(true, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
			return View(cwtPaginatedList);
		}

		// GET: /View
		public ActionResult View(int id)
		{
			TeamOutOfOfficeGroup group = new TeamOutOfOfficeGroup();
			group = teamOutOfOfficeGroupRepository.GetGroup(id);

			//Check Exists
			if (group == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			teamOutOfOfficeGroupRepository.EditGroupForDisplay(group);

			return View(group);
		}

		// GET: /Create
		public ActionResult Create()
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			SelectList hierarchyTypesList = new SelectList(teamOutOfOfficeGroupRepository.GetHierarchyTypes().ToList(), "Key", "Value");
			ViewData["HierarchyTypes"] = hierarchyTypesList;

			TeamOutOfOfficeGroup group = new TeamOutOfOfficeGroup();

			return View(group);
		}

		// POST: /Create/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(TeamOutOfOfficeGroup group)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Check Access Rights to Domain Hierarchy
			if (!hierarchyRepository.AdminHasDomainHierarchyWriteAccess(group.HierarchyType, group.HierarchyCode, "", groupName))
			{
				ViewData["Message"] = "You cannot add to this hierarchy item";
				return View("Error");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel(group);
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
				teamOutOfOfficeGroupRepository.Add(group);
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
			return RedirectToAction("ListUnDeleted");
		}

		// GET: /Edit
		public ActionResult Edit(int id)
		{
			//Get Item From Database
			TeamOutOfOfficeGroup group = new TeamOutOfOfficeGroup();
			group = teamOutOfOfficeGroupRepository.GetGroup(id);

			//Check Exists
			if (group == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToTeamOutOfOfficeGroup(id))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}


			SelectList hierarchyTypesList = new SelectList(teamOutOfOfficeGroupRepository.GetHierarchyTypes().ToList(), "Key", "Value", group.HierarchyType);
			ViewData["HierarchyTypes"] = hierarchyTypesList;

			teamOutOfOfficeGroupRepository.EditGroupForDisplay(group);

			return View(group);
		}

		// POST: /Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, FormCollection collection)
		{
			//Get Item From Database
			TeamOutOfOfficeGroup group = new TeamOutOfOfficeGroup();
			group = teamOutOfOfficeGroupRepository.GetGroup(id);

			//Check Exists
			if (group == null)
			{
				ViewData["ActionMethod"] = "EditPost";
				return View("RecordDoesNotExistError");
			}
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToTeamOutOfOfficeGroup(group.TeamOutOfOfficeGroupId))
			{
				return View("Error");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel(group);
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

			//ClientSubUnitTravelerType has extra field
			string hierarchyCode = group.HierarchyCode;
			if (group.HierarchyType == "ClientSubUnitTravelerType")
			{
				group.ClientSubUnitGuid = hierarchyCode;  //ClientSubUnitTravelerType has 2 primarykeys
			}

			//Check Access Rights to PolicyGroup
			HierarchyRepository hierarchyRepository = new HierarchyRepository();
			if (!hierarchyRepository.AdminHasDomainHierarchyWriteAccess(group.HierarchyType, hierarchyCode, "", groupName))
			{
				ViewData["Message"] = "You cannot add to this hierarchy item";
				return View("Error");
			}

			//Database Update
			try
			{
				teamOutOfOfficeGroupRepository.Edit(group);
			}
			catch (SqlException ex)
			{
				//Versioning Error
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/TeamOutOfOfficeGroup.mvc/Edit/" + group.TeamOutOfOfficeGroupId.ToString();
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("ListUnDeleted");
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
		{
			//Get Item From Database
			TeamOutOfOfficeGroup group = new TeamOutOfOfficeGroup();
			group = teamOutOfOfficeGroupRepository.GetGroup(id);

			//Check Exists
			if (group == null || group.DeletedFlag == true)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			//Check AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToTeamOutOfOfficeGroup(id))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			teamOutOfOfficeGroupRepository.EditGroupForDisplay(group);

			return View(group);
		}

		// POST: /Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, FormCollection collection)
		{
			//Get Item From Database
			TeamOutOfOfficeGroup group = new TeamOutOfOfficeGroup();
			group = teamOutOfOfficeGroupRepository.GetGroup(id);

			//Check Exists
			if (group == null || group.DeletedFlag == true)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToTeamOutOfOfficeGroup(id))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Delete Item
			try
			{
				group.VersionNumber = Int32.Parse(collection["VersionNumber"]);
				group.DeletedFlag = true;
				teamOutOfOfficeGroupRepository.UpdateGroupDeletedStatus(group);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/TeamOutOfOfficeGroup.mvc/Delete/" + group.TeamOutOfOfficeGroupId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("ListUnDeleted");
		}

		// GET: /UnDelete
		public ActionResult UnDelete(int id)
		{
			//Get Item From Database
			TeamOutOfOfficeGroup group = new TeamOutOfOfficeGroup();
			group = teamOutOfOfficeGroupRepository.GetGroup(id);

			//Check Exists
			if (group == null || group.DeletedFlag == false)
			{
				ViewData["ActionMethod"] = "UnDeleteGet";
				return View("RecordDoesNotExistError");
			}

			//Check AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToTeamOutOfOfficeGroup(id))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			teamOutOfOfficeGroupRepository.EditGroupForDisplay(group);

			return View(group);
		}

		// POST: /UnDelete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UnDelete(int id, FormCollection collection)
		{
			//Get Item From Database
			TeamOutOfOfficeGroup group = new TeamOutOfOfficeGroup();
			group = teamOutOfOfficeGroupRepository.GetGroup(id);

			//Check Exists
			if (group == null || group.DeletedFlag == false)
			{
				ViewData["ActionMethod"] = "UnDeletePost";
				return View("RecordDoesNotExistError");
			}
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToTeamOutOfOfficeGroup(id))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}
			//Delete Item
			try
			{
				group.VersionNumber = Int32.Parse(collection["VersionNumber"]);
				group.DeletedFlag = false;
				teamOutOfOfficeGroupRepository.UpdateGroupDeletedStatus(group);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/TeamOutOfOfficeGroup.mvc/UnDelete/" + group.TeamOutOfOfficeGroupId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("ListDeleted");
		}

		[HttpPost]
		public JsonResult AutoCompleteClientSubUnitTravelerTypes(string searchText, string hierarchyItem, string filterText)
		{
			HierarchyRepository hierarchyRepository = new HierarchyRepository();

			int maxResults = 15;
			var result = hierarchyRepository.LookUpSystemUserClientSubUnitTravelerTypes(searchText, hierarchyItem, maxResults, groupName, filterText);
			return Json(result);
		}

		//There can only be one active group created for each SubUnit (whether in UnDeleted or Deleted).
		//When considering an active group, the Enabled flag, Enabled Date and Expiry date of the existing group should be evaluated
		//returns boolean as Json
		[HttpPost]
		public JsonResult CanTeamOutOfOfficeGroupBeActive(int id, string clientSubUnitGuid)
		{
			var result = teamOutOfOfficeGroupRepository.CanTeamOutOfOfficeGroupBeActive(id, clientSubUnitGuid);
			return Json(result);
		}

		// GET: /Export
		public ActionResult Export()
		{
			string filename = string.Format("TeamOOOExport-{0}.csv", DateTime.Now.ToString("ddMMMyyyy-HHmm").ToUpper());

			//Get CSV Data
			byte[] csvData = teamOutOfOfficeGroupRepository.Export();
			return File(csvData, "text/csv", filename);
		}


		// GET: /ExportErrors
		public ActionResult ExportErrors()
		{
			var preImportCheckResultVM = (TeamOutOfOfficeGroupImportStep1VM)TempData["ErrorMessages"];

			if (preImportCheckResultVM == null)
			{
				ViewData["ActionMethod"] = "ExportGet";
				return View("RecordDoesNotExistError");
			}

			//Get CSV Data
			var errors = preImportCheckResultVM.ImportStep2VM.ReturnMessages;
			var combinedErrors = errors.Aggregate((current, next) => current + "\r\n" + next);
			byte[] csvData = Encoding.ASCII.GetBytes(combinedErrors);

			return File(csvData, "text/plain", "TeamOOOImportFailures.txt");
		}

		public ActionResult ImportStep1()
		{
			TeamOutOfOfficeGroupImportStep1WithFileVM clientSubUnitImportStep1WithFileVM = new TeamOutOfOfficeGroupImportStep1WithFileVM();

			return View(clientSubUnitImportStep1WithFileVM);
		}

		[HttpPost]
		public ActionResult ImportStep1(TeamOutOfOfficeGroupImportStep1WithFileVM csvfile)
		{
			if (!ModelState.IsValid)
			{

				return View(csvfile);
			}
			string fileExtension = Path.GetExtension(csvfile.File.FileName);
			if (fileExtension != ".csv")
			{
				ModelState.AddModelError("file", "This is not a valid entry");
				return View(csvfile);
			}

			if (csvfile.File.ContentLength > 0)
			{
				TeamOutOfOfficeGroupImportStep2VM preImportCheckResult = new TeamOutOfOfficeGroupImportStep2VM();
				List<string> returnMessages = new List<string>();

				preImportCheckResult = teamOutOfOfficeGroupRepository.PreImportCheck(csvfile.File, csvfile.ClientSubUnitGuid);

				TeamOutOfOfficeGroupImportStep1VM preImportCheckResultVM = new TeamOutOfOfficeGroupImportStep1VM();
				preImportCheckResultVM.ImportStep2VM = preImportCheckResult;
				preImportCheckResultVM.ClientSubUnitGuid = csvfile.ClientSubUnitGuid;

				TempData["PreImportCheckResultVM"] = preImportCheckResultVM;
				return RedirectToAction("ImportStep2");
			}

			return View();
		}

		public ActionResult ImportStep2()
		{
			TeamOutOfOfficeGroupImportStep1VM preImportCheckResultVM = new TeamOutOfOfficeGroupImportStep1VM();
			preImportCheckResultVM = (TeamOutOfOfficeGroupImportStep1VM)TempData["PreImportCheckResultVM"];
			if (preImportCheckResultVM == null)
			{
				return View("Error");
			}

			return View(preImportCheckResultVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ImportStep2(TeamOutOfOfficeGroupImportStep1VM preImportCheckResultVM)
		{
			if (preImportCheckResultVM.ImportStep2VM.IsValidData == false)
			{
				//Check JSON for valid messages
				if (preImportCheckResultVM.ImportStep2VM.ReturnMessages[0] != null)
				{
					List<string> returnMessages = new List<string>();

					var settings = new JsonSerializerSettings
					{
						StringEscapeHandling = StringEscapeHandling.EscapeHtml,
					};

					List<string> returnMessagesJSON = JsonConvert.DeserializeObject<List<string>>(preImportCheckResultVM.ImportStep2VM.ReturnMessages[0], settings);

					foreach (string message in returnMessagesJSON)
					{
						string validMessage = Regex.Replace(message, @"[^À-ÿ\w\s&:._()\-]", "");

						if (!string.IsNullOrEmpty(validMessage))
						{
							returnMessages.Add(validMessage);
						}
					}

					preImportCheckResultVM.ImportStep2VM.ReturnMessages = returnMessages;
				}

				TempData["ErrorMessages"] = preImportCheckResultVM;

				return RedirectToAction("ExportErrors");
			}

			//PreImport Check Results (check has passed)
			TeamOutOfOfficeGroupImportStep2VM preImportCheckResult = new TeamOutOfOfficeGroupImportStep2VM();
			preImportCheckResult = preImportCheckResultVM.ImportStep2VM;

			//Do the Import, return results
			TeamOutOfOfficeGroupImportStep3VM postImportResult = new TeamOutOfOfficeGroupImportStep3VM();
			postImportResult = teamOutOfOfficeGroupRepository.Import(
				preImportCheckResult.FileBytes
			);

			TempData["PostImportResult"] = postImportResult;

			//Pass Results to Next Page
			return RedirectToAction("ImportStep3");

		}

		public ActionResult ImportStep3()
		{
			//Display Results of Import
			TeamOutOfOfficeGroupImportStep3VM cdrPostImportResult = new TeamOutOfOfficeGroupImportStep3VM();
			cdrPostImportResult = (TeamOutOfOfficeGroupImportStep3VM)TempData["PostImportResult"];
			return View(cdrPostImportResult);
		}
	}
}