using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class SystemUserGDSController : Controller
    {
        //main repositories
        SystemUserRepository systemUserRepository = new SystemUserRepository();
        SystemUserGDSRepository systemUserGDSRepository = new SystemUserGDSRepository();

		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		private string groupName = "Role Based Access Team";

        // GET: /List
        public ActionResult List(int? page, string id)
        {
            SystemUser systemUser = new SystemUser();
            systemUser = systemUserRepository.GetUserBySystemUserGuid(id);
            ViewData["SystemUserName"] = (systemUser.LastName + ", " + systemUser.FirstName + " " + systemUser.MiddleName).Replace("  ", " ");
            ViewData["SystemUserGuid"] = systemUser.SystemUserGuid;

            //Check Exists
            if (systemUser == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

			//Set Access Rights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}   

            const int pageSize = 15;
            var items = systemUserGDSRepository.GetSystemUserGDSs(id);
            var paginatedView = new PaginatedList<fnDesktopDataAdmin_SelectSystemUserGDSs_v1Result>(items, page ?? 0, pageSize);
            return View(paginatedView);

        }

        // GET: /Create
        public ActionResult Create(string id)
        {
            //Get SystemUser
            SystemUser systemUser = new SystemUser();
            systemUser = systemUserRepository.GetUserBySystemUserGuid(id);

            //Check Exists
            if (systemUser == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

			//Set Access Rights
			ViewData["Access"] = "";
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}  

            SelectList gdsList = new SelectList(systemUserGDSRepository.GetUnUsedGDSs(id).ToList(), "GDSCode", "GDSName");
            ViewData["GDSs"] = gdsList;

            
            //Show Create Form
            SystemUserGDS systemUserGDS = new SystemUserGDS();
            systemUserGDS.SystemUserGuid = systemUser.SystemUserGuid;

            systemUserGDSRepository.EditForDisplay(systemUserGDS);
            return View(systemUserGDS);
        }


        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SystemUserGDS systemUserGDS)
        {
            //Get SystemUser
            SystemUser systemUser = new SystemUser();
            systemUser = systemUserRepository.GetUserBySystemUserGuid(systemUserGDS.SystemUserGuid);

            //Check Exists
            if (systemUser == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

			//Set Access Rights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}   

            //SystemUserGDS systemUserGDS = new SystemUserGDS();
            //systemUserGDS.SystemUserGuid = id;
			//systemUserGDS.GDSCode = gdsCode;

            //Database Update
            try
            {
                systemUserGDSRepository.Add(systemUserGDS);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            ViewData["NewSortOrder"] = 0;
            return RedirectToAction("List", new { id = systemUser.SystemUserGuid });

        }

        // GET: /Edit
        public ActionResult Edit(string id, string gdscode)
        {
            //Get SystemUser
            SystemUserGDS systemUserGDS = new SystemUserGDS();
            systemUserGDS = systemUserGDSRepository.GetSystemUserGDS(id, gdscode);

            //Check Exists
            if (systemUserGDS == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

			//Set Access Rights
			ViewData["Access"] = "";
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			} 

            systemUserGDSRepository.EditForDisplay(systemUserGDS);
            return View(systemUserGDS);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, string gdscode, FormCollection collection)
        {
            //Get SystemUser
            SystemUserGDS systemUserGDS = new SystemUserGDS();
            systemUserGDS = systemUserGDSRepository.GetSystemUserGDS(id, gdscode);

            //Check Exists
            if (systemUserGDS == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            try
            {
                UpdateModel(systemUserGDS);               
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
                //if (n == "Home Pseudo City or Office ID Required" && systemUserGDS.GDSCode != "ALL")
                //{
                //    ViewData["Message"] = "ValidationError : " + n;
                //    return View("Error");
                //}
            }

            //Database Update
            try
            {
                systemUserGDSRepository.Update(systemUserGDS);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/SystemUserGDS.mvc/Edit?gdscode=" + systemUserGDS.GDSCode.ToString() + "&id=" + systemUserGDS.SystemUserGuid.ToString();
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = systemUserGDS.SystemUserGuid });
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(string id, string gdscode)
        {
            //Get SystemUser
            SystemUserGDS systemUserGDS = new SystemUserGDS();
            systemUserGDS = systemUserGDSRepository.GetSystemUserGDS(id, gdscode);

            //Check Exists
            if (systemUserGDS == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

			//Set Access Rights
			ViewData["Access"] = "";
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			} 

            //Return View
            systemUserGDSRepository.EditForDisplay(systemUserGDS);
            return View(systemUserGDS);

        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, string gdscode, FormCollection collection)
        {
            //Get SystemUser
            SystemUserGDS systemUserGDS = new SystemUserGDS();
            systemUserGDS = systemUserGDSRepository.GetSystemUserGDS(id, gdscode);

            //Check Exists
            if (systemUserGDS == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Delete Item
            try
            {
                systemUserGDS.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                systemUserGDSRepository.Delete(systemUserGDS);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/SystemUserGDS.mvc/Delete?gdscode" + systemUserGDS.GDSCode + "&id=" + systemUserGDS.SystemUserGuid;
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            //Return
            return RedirectToAction("List", new { id = systemUserGDS.SystemUserGuid });
        }
    }
}
