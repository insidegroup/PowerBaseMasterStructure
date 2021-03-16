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
    public class CityAdviceController : Controller
    {
        //main repositories
        PolicyCityGroupItemLanguageRepository policyCityGroupItemLanguageRepository = new PolicyCityGroupItemLanguageRepository();
        PolicyCityGroupItemRepository policyCityGroupItemRepository = new PolicyCityGroupItemRepository();
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();

        //GET:List
        public ActionResult List(int id, int? page, string sortField, int? sortOrder)
        {
            //Get PolicyCityGroupItem
            PolicyCityGroupItem policyCityGroupItem = new PolicyCityGroupItem();
            policyCityGroupItem = policyCityGroupItemRepository.GetPolicyCityGroupItem(id);

            //Check Exists
            if (policyCityGroupItem == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToPolicyGroup(policyCityGroupItem.PolicyGroupId))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Parent Information
            ViewData["PolicyCityGroupItemId"] = policyCityGroupItem.PolicyCityGroupItemId;
            ViewData["PolicyGroupID"] = policyCityGroupItem.PolicyGroupId;
            ViewData["PolicyGroupName"] = policyGroupRepository.GetGroup(policyCityGroupItem.PolicyGroupId).PolicyGroupName;


            //SortField+SortOrder settings
            if (sortField != "CityAdvice")
            {
                sortField = "LanguageName";
            }
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

            //Get data
            var cwtPaginatedList = policyCityGroupItemLanguageRepository.PagePolicyCityGroupItemCityAdvice(id, page ?? 1, sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        // GET: /Create
        public ActionResult Create(int id)
        {
            //Get PolicyCityGroupItem
            PolicyCityGroupItem policyCityGroupItem = new PolicyCityGroupItem();
            policyCityGroupItem = policyCityGroupItemRepository.GetPolicyCityGroupItem(id);

            //Check Exists
            if (policyCityGroupItem == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCityGroupItem.PolicyGroupId))
            {
                return View("Error");
            }

            //New PolicyCityGroupItemLanguage
            PolicyCityGroupItemLanguage policyCityGroupItemLanguage = new PolicyCityGroupItemLanguage();
            policyCityGroupItemLanguage.PolicyCityGroupItemId = id;
            policyCityGroupItemLanguageRepository.EditItemForDisplay(policyCityGroupItemLanguage);

            //Language SelectList
            SelectList languageList = new SelectList(policyCityGroupItemLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            //Show Create Form
            return View(policyCityGroupItemLanguage);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicyCityGroupItemLanguage policyCityGroupItemLanguage)
        {
            PolicyCityGroupItem policyCityGroupItem = new PolicyCityGroupItem();
            policyCityGroupItem = policyCityGroupItemRepository.GetPolicyCityGroupItem(policyCityGroupItemLanguage.PolicyCityGroupItemId);

            //Check Exists
            if (policyCityGroupItem == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCityGroupItem.PolicyGroupId))
            {
                //AccessRights Error
                return View("Error");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(policyCityGroupItemLanguage);
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
                policyCityGroupItemLanguageRepository.Add(policyCityGroupItemLanguage);
            }
            catch
            {
                //Insert Error
                return View("Error");
            }


            return RedirectToAction("List", new { id = policyCityGroupItem.PolicyCityGroupItemId });
        }

        //GET: View
        public ActionResult View(int id, string languageCode)
        {
            PolicyCityGroupItemLanguage policyCityGroupItemLanguage = new PolicyCityGroupItemLanguage();
            policyCityGroupItemLanguage = policyCityGroupItemLanguageRepository.GetItem(id, languageCode);
            if (policyCityGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }
            policyCityGroupItemLanguageRepository.EditItemForDisplay(policyCityGroupItemLanguage);
            return View(policyCityGroupItemLanguage);
        }

        // GET: /Edit
        public ActionResult Edit(int id, string languageCode)
        {
            //Get Item 
            PolicyCityGroupItemLanguage policyCityGroupItemLanguage = new PolicyCityGroupItemLanguage();
            policyCityGroupItemLanguage = policyCityGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyCityGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyCityGroupItem policyCityGroupItem = new PolicyCityGroupItem();
            policyCityGroupItem = policyCityGroupItemRepository.GetPolicyCityGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCityGroupItem.PolicyGroupId))
            {
                return View("Error");
            }

            //Language SelectList
            SelectList languageList = new SelectList(policyCityGroupItemLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            policyCityGroupItemLanguageRepository.EditItemForDisplay(policyCityGroupItemLanguage);
            return View(policyCityGroupItemLanguage);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string languageCode, FormCollection formCollection)
        {
            //Get Item 
            PolicyCityGroupItemLanguage policyCityGroupItemLanguage = new PolicyCityGroupItemLanguage();
            policyCityGroupItemLanguage = policyCityGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyCityGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyCityGroupItem policyCityGroupItem = new PolicyCityGroupItem();
            policyCityGroupItem = policyCityGroupItemRepository.GetPolicyCityGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCityGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Item from Form
            try
            {
                UpdateModel(policyCityGroupItemLanguage);
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

            //Update CityAdvice
            try
            {
                policyCityGroupItemLanguageRepository.Update(policyCityGroupItemLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/CityAdvice.mvc/Edit/" + policyCityGroupItemLanguage.PolicyCityGroupItemId.ToString();
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = policyCityGroupItemLanguage.PolicyCityGroupItemId });
        }


        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, string languageCode)
        {
            //Get Item 
            PolicyCityGroupItemLanguage policyCityGroupItemLanguage = new PolicyCityGroupItemLanguage();
            policyCityGroupItemLanguage = policyCityGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyCityGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyCityGroupItem policyCityGroupItem = new PolicyCityGroupItem();
            policyCityGroupItem = policyCityGroupItemRepository.GetPolicyCityGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCityGroupItem.PolicyGroupId))
            {
                return View("Error");
            }

            //Add Linked Information
            policyCityGroupItemLanguageRepository.EditItemForDisplay(policyCityGroupItemLanguage);

            //Return View
            return View(policyCityGroupItemLanguage);

        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, string languageCode, FormCollection collection)
        {
            //Get Item 
            PolicyCityGroupItemLanguage policyCityGroupItemLanguage = new PolicyCityGroupItemLanguage();
            policyCityGroupItemLanguage = policyCityGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyCityGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyCityGroupItem policyCityGroupItem = new PolicyCityGroupItem();
            policyCityGroupItem = policyCityGroupItemRepository.GetPolicyCityGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCityGroupItem.PolicyGroupId))
            {
                return View("Error");
            }

            //Delete Item
            try
            {
                policyCityGroupItemLanguage.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                policyCityGroupItemLanguageRepository.Delete(policyCityGroupItemLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/CityAdvice.mvc/Delete/" + policyCityGroupItemLanguage.PolicyCityGroupItemId.ToString() + "/" + policyCityGroupItemLanguage.LanguageCode;
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            //Return
            return RedirectToAction("List", new { id = policyCityGroupItemLanguage.PolicyCityGroupItemId });
        }
    }
}
