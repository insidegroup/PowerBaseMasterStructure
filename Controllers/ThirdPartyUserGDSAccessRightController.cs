using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Controllers
{
	public class ThirdPartyUserGDSAccessRightController : Controller
	{
		ThirdPartyUserGDSAccessRightRepository thirdPartyUserGDSAccessRightRepository = new ThirdPartyUserGDSAccessRightRepository();
		GDSRepository gdsRepository = new GDSRepository();
		GDSAccessTypeRepository gdsAccessTypeRepository = new GDSAccessTypeRepository();
		ThirdPartyUserRepository thirdPartyUserRepository = new ThirdPartyUserRepository();

		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		RolesRepository rolesRepository = new RolesRepository();

		// GET: /List
		public ActionResult ListUnDeleted(int id, int? page, string filter, string sortField, int? sortOrder)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (rolesRepository.HasWriteAccessToThirdPartyGDSAccessRights(id))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//SortField
			if (string.IsNullOrEmpty(sortField))
			{
				sortField = "GDSName";
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

			//Populate View Model
			ThirdPartyUserGDSAccessRightsVM thirdPartyUserGDSAccessRightsVM = new ThirdPartyUserGDSAccessRightsVM();

			var getThirdPartyUserGDSAccessRights = thirdPartyUserGDSAccessRightRepository.PageThirdPartyUserGDSAccessRights(id, filter ?? "", sortField, sortOrder ?? 0, page ?? 1, false);
			if (getThirdPartyUserGDSAccessRights != null)
			{
				thirdPartyUserGDSAccessRightsVM.ThirdPartyUserGDSAccessRights = getThirdPartyUserGDSAccessRights;
			}

			//ThirdPartyUser
			ThirdPartyUser thirdPartyUser = new ThirdPartyUser();
			thirdPartyUser = thirdPartyUserRepository.GetThirdPartyUser(id);
			if (thirdPartyUser != null)
			{
				thirdPartyUserGDSAccessRightsVM.ThirdPartyUser = thirdPartyUser;
			}

			return View(thirdPartyUserGDSAccessRightsVM);
		}

		// GET: /List
		public ActionResult ListDeleted(int id, int? page, string filter, string sortField, int? sortOrder)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (rolesRepository.HasWriteAccessToThirdPartyGDSAccessRights(id))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//SortField
			if (string.IsNullOrEmpty(sortField))
			{
				sortField = "GDSName";
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

			//Populate View Model
			ThirdPartyUserGDSAccessRightsVM thirdPartyUserGDSAccessRightsVM = new ThirdPartyUserGDSAccessRightsVM();

			var getThirdPartyUserGDSAccessRights = thirdPartyUserGDSAccessRightRepository.PageThirdPartyUserGDSAccessRights(id, filter ?? "", sortField, sortOrder ?? 0, page ?? 1, true);
			if (getThirdPartyUserGDSAccessRights != null)
			{
				thirdPartyUserGDSAccessRightsVM.ThirdPartyUserGDSAccessRights = getThirdPartyUserGDSAccessRights;
			}

			//ThirdPartyUser
			ThirdPartyUser thirdPartyUser = new ThirdPartyUser();
			thirdPartyUser = thirdPartyUserRepository.GetThirdPartyUser(id);
			if (thirdPartyUser != null)
			{
				thirdPartyUserGDSAccessRightsVM.ThirdPartyUser = thirdPartyUser;
			}

			return View(thirdPartyUserGDSAccessRightsVM);
		}

		public ActionResult Create(int id)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (rolesRepository.HasWriteAccessToThirdPartyGDSAccessRights(id))
			{
				ViewData["Access"] = "WriteAccess";
			}

			ThirdPartyUserGDSAccessRightVM thirdPartyUserGDSAccessRightVM = new ThirdPartyUserGDSAccessRightVM();
			
			ThirdPartyUserGDSAccessRight thirdPartyUserGDSAccessRight = new ThirdPartyUserGDSAccessRight();
			thirdPartyUserGDSAccessRightVM.ThirdPartyUserGDSAccessRight = thirdPartyUserGDSAccessRight;

			//GDS
			thirdPartyUserGDSAccessRightVM.GDSs = new SelectList(gdsRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName");

			//GDSAccessTypes
			thirdPartyUserGDSAccessRightVM.GDSAccessTypes = new SelectList(gdsAccessTypeRepository.GetAllGDSAccessTypes().ToList(), "GDSAccessTypeId", "GDSAccessTypeName");

			//ThirdPartyUser
			ThirdPartyUser thirdPartyUser = new ThirdPartyUser();
			thirdPartyUser = thirdPartyUserRepository.GetThirdPartyUser(id);
			if (thirdPartyUser != null)
			{
				thirdPartyUserRepository.EditForDisplay(thirdPartyUser); 
				thirdPartyUserGDSAccessRightVM.ThirdPartyUser = thirdPartyUser;
				thirdPartyUserGDSAccessRightVM.ThirdPartyUserGDSAccessRight.ThirdPartyUserId = thirdPartyUser.ThirdPartyUserId;
			}

			return View(thirdPartyUserGDSAccessRightVM);
		}

		// POST: /Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ThirdPartyUserGDSAccessRightVM thirdPartyUserGDSAccessRightVM)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (rolesRepository.HasWriteAccessToThirdPartyGDSAccessRights(thirdPartyUserGDSAccessRightVM.ThirdPartyUserGDSAccessRight.ThirdPartyUserId))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//Check Access
			if (!rolesRepository.HasWriteAccessToThirdPartyGDSAccessRights(thirdPartyUserGDSAccessRightVM.ThirdPartyUserGDSAccessRight.ThirdPartyUserId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Add in ThirdPartyUser
			if(thirdPartyUserGDSAccessRightVM.ThirdPartyUserGDSAccessRight.ThirdPartyUserId > 0)
			{
				ThirdPartyUserRepository thirdPartyUserRepository = new ThirdPartyUserRepository();
				ThirdPartyUser thirdPartyUser = thirdPartyUserRepository.GetThirdPartyUser(thirdPartyUserGDSAccessRightVM.ThirdPartyUserGDSAccessRight.ThirdPartyUserId);
				if(thirdPartyUser != null)
				{
					thirdPartyUserGDSAccessRightVM.ThirdPartyUser = thirdPartyUser;
				}
			}

			//Update  Model from Form
			try
			{
				TryUpdateModel<ThirdPartyUserGDSAccessRightVM>(thirdPartyUserGDSAccessRightVM, "ThirdPartyUserGDSAccessRightVM");
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

			try
			{
				thirdPartyUserGDSAccessRightRepository.Add(thirdPartyUserGDSAccessRightVM);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("ListUnDeleted", new { id = thirdPartyUserGDSAccessRightVM.ThirdPartyUserGDSAccessRight.ThirdPartyUserId });
		}

		// GET: /Edit
		public ActionResult Edit(int id, int thirdPartyUserId)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (rolesRepository.HasWriteAccessToThirdPartyGDSAccessRights(thirdPartyUserId))
			{
				ViewData["Access"] = "WriteAccess";
			}

			ThirdPartyUserGDSAccessRight thirdPartyUserGDSAccessRight = thirdPartyUserGDSAccessRightRepository.GetThirdPartyUserGDSAccessRight(id);

			//Check Exists
			if (thirdPartyUserGDSAccessRight == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access
			if (!rolesRepository.HasWriteAccessToThirdPartyGDSAccessRights(thirdPartyUserId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			ThirdPartyUserGDSAccessRightVM thirdPartyUserGDSAccessRightVM = new ThirdPartyUserGDSAccessRightVM();
			thirdPartyUserGDSAccessRightVM.ThirdPartyUserGDSAccessRight = thirdPartyUserGDSAccessRight;

			//GDS
			thirdPartyUserGDSAccessRightVM.GDSs = new SelectList(gdsRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName", thirdPartyUserGDSAccessRight.GDSCode);

			//GDSAccessTypes
			thirdPartyUserGDSAccessRightVM.GDSAccessTypes = new SelectList(gdsAccessTypeRepository.GetGDSAccessTypesByGDSCode(thirdPartyUserGDSAccessRight.GDSCode).ToList(), "GDSAccessTypeId", "GDSAccessTypeName", thirdPartyUserGDSAccessRight.GDSAccessTypeId);

			//PseudoCityOrOfficeIds
			thirdPartyUserGDSAccessRightVM.PseudoCityOrOfficeIds = new SelectList(
				thirdPartyUserRepository.GetThirdPartyUserPseudoCityOrOfficeIdsByGDSCode(thirdPartyUserGDSAccessRight.GDSCode, thirdPartyUserGDSAccessRight.ThirdPartyUserId).ToList(), 
				"PseudoCityOrOfficeId", 
				"PseudoCityOrOfficeId", 
				thirdPartyUserGDSAccessRight.PseudoCityOrOfficeId
			);

			//ThirdPartyUser
			ThirdPartyUser thirdPartyUser = new ThirdPartyUser();
			thirdPartyUser = thirdPartyUserRepository.GetThirdPartyUser(thirdPartyUserGDSAccessRight.ThirdPartyUserId);
			if (thirdPartyUser != null)
			{
				thirdPartyUserRepository.EditForDisplay(thirdPartyUser); 
				thirdPartyUserGDSAccessRightVM.ThirdPartyUser = thirdPartyUser;
			}

			//Entitlements (All but this one)
			if (thirdPartyUser.ThirdPartyUserGDSAccessRights != null && thirdPartyUser.ThirdPartyUserGDSAccessRights.Count() > 0)
			{
				List<Entitlement> entitlements = new List<Entitlement>();
				foreach (ThirdPartyUserGDSAccessRight item in thirdPartyUser.ThirdPartyUserGDSAccessRights.Where(x => x.ThirdPartyUserGDSAccessRightId != thirdPartyUserGDSAccessRight.ThirdPartyUserGDSAccessRightId))
				{
					Entitlement entitlement = new Entitlement()
					{
						tpAgentID = item.GDSSignOnID,
						tpPCC = item.PseudoCityOrOfficeId,
						tpServiceID = item.GDS.GDSName,
						DeletedFlag = item.DeletedFlag == true ? true : false,
						DeletedTimestamp = item.DeletedDateTime
					};
					entitlements.Add(entitlement);
				}
				thirdPartyUserGDSAccessRightVM.Entitlements = entitlements;
			}

			return View(thirdPartyUserGDSAccessRightVM);
		}

		// POST: /Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(ThirdPartyUserGDSAccessRightVM thirdPartyUserGDSAccessRightVM)
		{
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToThirdPartyGDSAccessRights(thirdPartyUserGDSAccessRightVM.ThirdPartyUserGDSAccessRight.ThirdPartyUserId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Add in ThirdPartyUser
			if (thirdPartyUserGDSAccessRightVM.ThirdPartyUserGDSAccessRight.ThirdPartyUserId > 0)
			{
				ThirdPartyUserRepository thirdPartyUserRepository = new ThirdPartyUserRepository();
				ThirdPartyUser thirdPartyUser = thirdPartyUserRepository.GetThirdPartyUser(thirdPartyUserGDSAccessRightVM.ThirdPartyUserGDSAccessRight.ThirdPartyUserId);
				if (thirdPartyUser != null)
				{
					thirdPartyUserGDSAccessRightVM.ThirdPartyUser = thirdPartyUser;
				}
			}
			
			//Update  Model from Form
			try
			{
				TryUpdateModel<ThirdPartyUserGDSAccessRight>(thirdPartyUserGDSAccessRightVM.ThirdPartyUserGDSAccessRight, "ThirdPartyUserGDSAccessRight");
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

			try
			{
				thirdPartyUserGDSAccessRightRepository.Update(thirdPartyUserGDSAccessRightVM);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("ListUnDeleted", new { id = thirdPartyUserGDSAccessRightVM.ThirdPartyUserGDSAccessRight.ThirdPartyUserId });
		}

		// GET: /View
		[HttpGet]
		public ActionResult View(int id)
		{
			ThirdPartyUserGDSAccessRight thirdPartyUserGDSAccessRight = new ThirdPartyUserGDSAccessRight();
			thirdPartyUserGDSAccessRight = thirdPartyUserGDSAccessRightRepository.GetThirdPartyUserGDSAccessRight(id);

			//Check Exists
			if (thirdPartyUserGDSAccessRight == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			ThirdPartyUserGDSAccessRightVM thirdPartyUserGDSAccessRightVM = new ThirdPartyUserGDSAccessRightVM();

			//System User
			ThirdPartyUser thirdPartyUser = new ThirdPartyUser();
			thirdPartyUser = thirdPartyUserRepository.GetThirdPartyUser(thirdPartyUserGDSAccessRight.ThirdPartyUserId);
			if (thirdPartyUser != null)
			{
				thirdPartyUserGDSAccessRightVM.ThirdPartyUser = thirdPartyUser;
			}

			thirdPartyUserGDSAccessRightVM.ThirdPartyUserGDSAccessRight = thirdPartyUserGDSAccessRight;

			return View(thirdPartyUserGDSAccessRightVM);
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, int thirdPartyUserId)
		{
			ThirdPartyUserGDSAccessRight thirdPartyUserGDSAccessRight = new ThirdPartyUserGDSAccessRight();
			thirdPartyUserGDSAccessRight = thirdPartyUserGDSAccessRightRepository.GetThirdPartyUserGDSAccessRight(id);

			//Check Exists
			if (thirdPartyUserGDSAccessRight == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			//Check AccessRights
			if (!rolesRepository.HasWriteAccessToThirdPartyGDSAccessRights(thirdPartyUserId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			ThirdPartyUserGDSAccessRightVM thirdPartyUserGDSAccessRightVM = new ThirdPartyUserGDSAccessRightVM();

			//System User
			ThirdPartyUser thirdPartyUser = new ThirdPartyUser();
			thirdPartyUser = thirdPartyUserRepository.GetThirdPartyUser(thirdPartyUserGDSAccessRight.ThirdPartyUserId);
			if (thirdPartyUser != null)
			{
				thirdPartyUserRepository.EditForDisplay(thirdPartyUser);
				thirdPartyUserGDSAccessRightVM.ThirdPartyUser = thirdPartyUser;
			}

			//Entitlements (All but this one)
			if (thirdPartyUser.ThirdPartyUserGDSAccessRights != null && thirdPartyUser.ThirdPartyUserGDSAccessRights.Count() > 0)
			{
				List<Entitlement> entitlements = new List<Entitlement>();
				foreach (ThirdPartyUserGDSAccessRight item in thirdPartyUser.ThirdPartyUserGDSAccessRights.Where(x => x.ThirdPartyUserGDSAccessRightId != thirdPartyUserGDSAccessRight.ThirdPartyUserGDSAccessRightId))
				{
					Entitlement entitlement = new Entitlement()
					{
						tpAgentID = item.GDSSignOnID,
						tpPCC = item.PseudoCityOrOfficeId,
						tpServiceID = item.GDS.GDSName,
						DeletedFlag = item.DeletedFlag == true ? true : false,
						DeletedTimestamp = item.DeletedDateTime
					};
					entitlements.Add(entitlement);
				}
				thirdPartyUserGDSAccessRightVM.Entitlements = entitlements;
			}

			thirdPartyUserGDSAccessRightVM.ThirdPartyUserGDSAccessRight = thirdPartyUserGDSAccessRight;

			return View(thirdPartyUserGDSAccessRightVM);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(ThirdPartyUserGDSAccessRightVM thirdPartyUserGDSAccessRightVM, FormCollection collection)
		{
			//Check Access
			if (!rolesRepository.HasWriteAccessToThirdPartyGDSAccessRights(thirdPartyUserGDSAccessRightVM.ThirdPartyUserGDSAccessRight.ThirdPartyUserId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Get Item From Database
			ThirdPartyUserGDSAccessRight thirdPartyUserGDSAccessRight = new ThirdPartyUserGDSAccessRight();
			thirdPartyUserGDSAccessRight = thirdPartyUserGDSAccessRightRepository.GetThirdPartyUserGDSAccessRight(
				thirdPartyUserGDSAccessRightVM.ThirdPartyUserGDSAccessRight.ThirdPartyUserGDSAccessRightId
			);

			//Check Exists
			if (thirdPartyUserGDSAccessRight == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Delete Item
			try
			{
				thirdPartyUserGDSAccessRightVM.ThirdPartyUserGDSAccessRight.DeletedFlag = true;
				thirdPartyUserGDSAccessRightRepository.UpdateDeletedStatus(thirdPartyUserGDSAccessRightVM);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/ThirdPartyUserGDSAccessRight.mvc/Delete/" + thirdPartyUserGDSAccessRight.ThirdPartyUserGDSAccessRightId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("ListUnDeleted", new { id = thirdPartyUserGDSAccessRightVM.ThirdPartyUserGDSAccessRight.ThirdPartyUserId });
		}

		// GET: /UnDelete
		[HttpGet]
		public ActionResult UnDelete(int id, int thirdPartyUserId)
		{
			ThirdPartyUserGDSAccessRight thirdPartyUserGDSAccessRight = new ThirdPartyUserGDSAccessRight();
			thirdPartyUserGDSAccessRight = thirdPartyUserGDSAccessRightRepository.GetThirdPartyUserGDSAccessRight(id);

			//Check Exists
			if (thirdPartyUserGDSAccessRight == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			//Check AccessRights
			if (!rolesRepository.HasWriteAccessToThirdPartyGDSAccessRights(thirdPartyUserId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			ThirdPartyUserGDSAccessRightVM thirdPartyUserGDSAccessRightVM = new ThirdPartyUserGDSAccessRightVM();

			//System User
			ThirdPartyUser thirdPartyUser = new ThirdPartyUser();
			thirdPartyUser = thirdPartyUserRepository.GetThirdPartyUser(thirdPartyUserGDSAccessRight.ThirdPartyUserId);
			if (thirdPartyUser != null)
			{
				thirdPartyUserRepository.EditForDisplay(thirdPartyUser); 
				thirdPartyUserGDSAccessRightVM.ThirdPartyUser = thirdPartyUser;
			}

			//Entitlements (All but this one)
			if (thirdPartyUser.ThirdPartyUserGDSAccessRights != null && thirdPartyUser.ThirdPartyUserGDSAccessRights.Count() > 0)
			{
				List<Entitlement> entitlements = new List<Entitlement>();
				foreach (ThirdPartyUserGDSAccessRight item in thirdPartyUser.ThirdPartyUserGDSAccessRights.Where(x => x.ThirdPartyUserGDSAccessRightId != thirdPartyUserGDSAccessRight.ThirdPartyUserGDSAccessRightId))
				{
					Entitlement entitlement = new Entitlement()
					{
						tpAgentID = item.GDSSignOnID,
						tpPCC = item.PseudoCityOrOfficeId,
						tpServiceID = item.GDS.GDSName,
						DeletedFlag = item.DeletedFlag == true ? true : false,
						DeletedTimestamp = item.DeletedDateTime
					};
					entitlements.Add(entitlement);
				}
				thirdPartyUserGDSAccessRightVM.Entitlements = entitlements;
			}

			thirdPartyUserGDSAccessRightVM.ThirdPartyUserGDSAccessRight = thirdPartyUserGDSAccessRight;

			return View(thirdPartyUserGDSAccessRightVM);
		}

		// POST: /UnDelete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UnDelete(ThirdPartyUserGDSAccessRightVM thirdPartyUserGDSAccessRightVM, FormCollection collection)
		{
			//Check Access
			if (!rolesRepository.HasWriteAccessToThirdPartyGDSAccessRights(thirdPartyUserGDSAccessRightVM.ThirdPartyUserGDSAccessRight.ThirdPartyUserId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Get Item From Database
			ThirdPartyUserGDSAccessRight thirdPartyUserGDSAccessRight = new ThirdPartyUserGDSAccessRight();
			thirdPartyUserGDSAccessRight = thirdPartyUserGDSAccessRightRepository.GetThirdPartyUserGDSAccessRight(
				thirdPartyUserGDSAccessRightVM.ThirdPartyUserGDSAccessRight.ThirdPartyUserGDSAccessRightId
			);

			//Check Exists
			if (thirdPartyUserGDSAccessRight == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Delete Item
			try
			{
				thirdPartyUserGDSAccessRightVM.ThirdPartyUserGDSAccessRight.DeletedFlag = false;
				thirdPartyUserGDSAccessRightRepository.UpdateDeletedStatus(thirdPartyUserGDSAccessRightVM);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/ThirdPartyUserGDSAccessRight.mvc/Delete/" + thirdPartyUserGDSAccessRight.ThirdPartyUserGDSAccessRightId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("ListDeleted", new { id = thirdPartyUserGDSAccessRightVM.ThirdPartyUserGDSAccessRight.ThirdPartyUserId });
		}

		// GET: /Export
		public ActionResult Export(int id)
		{
			//Get CSV Data
			byte[] csvData = thirdPartyUserGDSAccessRightRepository.Export(id);
			return File(csvData, "text/csv", "GDS Access Right Export.csv");
		}
	}
}