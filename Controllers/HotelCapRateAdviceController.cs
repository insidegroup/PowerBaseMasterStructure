using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class HotelCapRateAdviceController : Controller
    {
        //main repositories
        PolicyHotelCapRateGroupItemLanguageRepository policyHotelCapRateGroupItemLanguageRepository = new PolicyHotelCapRateGroupItemLanguageRepository();
        PolicyHotelCapRateGroupItemRepository policyHotelCapRateGroupItemRepository = new PolicyHotelCapRateGroupItemRepository();
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();

        //GET:List
        public ActionResult List(int id, int? page, string sortField, int? sortOrder)
        {
            //Get PolicyHotelCapRateGroupItem
            PolicyHotelCapRateGroupItem policyHotelCapRateGroupItem = new PolicyHotelCapRateGroupItem();
            policyHotelCapRateGroupItem = policyHotelCapRateGroupItemRepository.GetPolicyHotelCapRateGroupItem(id);

            //Check Exists
            if (policyHotelCapRateGroupItem == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToPolicyGroup(policyHotelCapRateGroupItem.PolicyGroupId))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Parent Information
            ViewData["PolicyHotelCapRateGroupItemId"] = policyHotelCapRateGroupItem.PolicyHotelCapRateItemId;
            ViewData["PolicyGroupID"] = policyHotelCapRateGroupItem.PolicyGroupId;
            ViewData["PolicyGroupName"] = policyGroupRepository.GetGroup(policyHotelCapRateGroupItem.PolicyGroupId).PolicyGroupName;


            //SortField+SortOrder settings
            if (sortField !="HotelCapRateAdvice")
            {
                sortField = "LanguageName";
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

            //Get data
            var cwtPaginatedList = policyHotelCapRateGroupItemLanguageRepository.PagePolicyHotelCapRateGroupItemHotelCapRateAdvice(id, page ?? 1, sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        //GET: View
        public ActionResult View(int id, string languageCode)
        {
            PolicyHotelCapRateGroupItemLanguage policyHotelCapRateGroupItemLanguage = new PolicyHotelCapRateGroupItemLanguage();
            policyHotelCapRateGroupItemLanguage = policyHotelCapRateGroupItemLanguageRepository.GetItem(id, languageCode);
            if (policyHotelCapRateGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }
            policyHotelCapRateGroupItemLanguageRepository.EditItemForDisplay(policyHotelCapRateGroupItemLanguage);
            return View(policyHotelCapRateGroupItemLanguage);
        }

        // GET: /Create
        public ActionResult Create(int id)
        {
            //Get PolicyHotelCapRateGroupItem
            PolicyHotelCapRateGroupItem policyHotelCapRateGroupItem = new PolicyHotelCapRateGroupItem();
            policyHotelCapRateGroupItem = policyHotelCapRateGroupItemRepository.GetPolicyHotelCapRateGroupItem(id);

            //Check Exists
            if (policyHotelCapRateGroupItem == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelCapRateGroupItem.PolicyGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //New PolicyHotelCapRateGroupItemLanguage
            PolicyHotelCapRateGroupItemLanguage policyHotelCapRateGroupItemLanguage = new PolicyHotelCapRateGroupItemLanguage();
            policyHotelCapRateGroupItemLanguage.PolicyHotelCapRateItemId = id;
            policyHotelCapRateGroupItemLanguageRepository.EditItemForDisplay(policyHotelCapRateGroupItemLanguage);

            //Language SelectList
            SelectList languageList = new SelectList(policyHotelCapRateGroupItemLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            //Show Create Form
            return View(policyHotelCapRateGroupItemLanguage);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicyHotelCapRateGroupItemLanguage policyHotelCapRateGroupItemLanguage)
        {
            PolicyHotelCapRateGroupItem policyHotelCapRateGroupItem = new PolicyHotelCapRateGroupItem();
            policyHotelCapRateGroupItem = policyHotelCapRateGroupItemRepository.GetPolicyHotelCapRateGroupItem(policyHotelCapRateGroupItemLanguage.PolicyHotelCapRateItemId);

            //Check Exists
            if (policyHotelCapRateGroupItem == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelCapRateGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(policyHotelCapRateGroupItemLanguage);
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
                policyHotelCapRateGroupItemLanguageRepository.Add(policyHotelCapRateGroupItemLanguage);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = policyHotelCapRateGroupItem.PolicyHotelCapRateItemId });
        }

        // GET: /Edit
        public ActionResult Edit(int id, string languageCode)
        {
            //Get Item 
            PolicyHotelCapRateGroupItemLanguage policyHotelCapRateGroupItemLanguage = new PolicyHotelCapRateGroupItemLanguage();
            policyHotelCapRateGroupItemLanguage = policyHotelCapRateGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyHotelCapRateGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyHotelCapRateGroupItem policyHotelCapRateGroupItem = new PolicyHotelCapRateGroupItem();
            policyHotelCapRateGroupItem = policyHotelCapRateGroupItemRepository.GetPolicyHotelCapRateGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelCapRateGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Language SelectList
            SelectList languageList = new SelectList(policyHotelCapRateGroupItemLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            policyHotelCapRateGroupItemLanguageRepository.EditItemForDisplay(policyHotelCapRateGroupItemLanguage);
            return View(policyHotelCapRateGroupItemLanguage);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int policyHotelCapRateItemId, string languageCode, string hotelCapRateAdvice)
        {
            //Get Item 
            PolicyHotelCapRateGroupItemLanguage policyHotelCapRateGroupItemLanguage = new PolicyHotelCapRateGroupItemLanguage();
            policyHotelCapRateGroupItemLanguage = policyHotelCapRateGroupItemLanguageRepository.GetItem(policyHotelCapRateItemId, languageCode);

            //Check Exists
            if (policyHotelCapRateGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyHotelCapRateGroupItem policyHotelCapRateGroupItem = new PolicyHotelCapRateGroupItem();
            policyHotelCapRateGroupItem = policyHotelCapRateGroupItemRepository.GetPolicyHotelCapRateGroupItem(policyHotelCapRateItemId);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelCapRateGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            //Update Item from Form
            try
            {
                UpdateModel(policyHotelCapRateGroupItemLanguage);
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



            //Update Advice
            try
            {
                policyHotelCapRateGroupItemLanguageRepository.Update(policyHotelCapRateGroupItemLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/HotelCapRateAdviceAdvice.mvc/Edit/" + policyHotelCapRateGroupItemLanguage.PolicyHotelCapRateItemId.ToString();
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }

            return RedirectToAction("List", new { id = policyHotelCapRateGroupItemLanguage.PolicyHotelCapRateItemId });
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, string languageCode)
        {
            //Get Item 
            PolicyHotelCapRateGroupItemLanguage policyHotelCapRateGroupItemLanguage = new PolicyHotelCapRateGroupItemLanguage();
            policyHotelCapRateGroupItemLanguage = policyHotelCapRateGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyHotelCapRateGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyHotelCapRateGroupItem policyHotelCapRateGroupItem = new PolicyHotelCapRateGroupItem();
            policyHotelCapRateGroupItem = policyHotelCapRateGroupItemRepository.GetPolicyHotelCapRateGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelCapRateGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Add Linked Information
            policyHotelCapRateGroupItemLanguageRepository.EditItemForDisplay(policyHotelCapRateGroupItemLanguage);

            //Return View
            return View(policyHotelCapRateGroupItemLanguage);

        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, string languageCode, FormCollection collection)
        {
            //Get Item 
            PolicyHotelCapRateGroupItemLanguage policyHotelCapRateGroupItemLanguage = new PolicyHotelCapRateGroupItemLanguage();
            policyHotelCapRateGroupItemLanguage = policyHotelCapRateGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyHotelCapRateGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyHotelCapRateGroupItem policyHotelCapRateGroupItem = new PolicyHotelCapRateGroupItem();
            policyHotelCapRateGroupItem = policyHotelCapRateGroupItemRepository.GetPolicyHotelCapRateGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelCapRateGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                policyHotelCapRateGroupItemLanguage.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                policyHotelCapRateGroupItemLanguageRepository.Delete(policyHotelCapRateGroupItemLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/HOtelCapRateAdvice.mvc/Delete/" + policyHotelCapRateGroupItemLanguage.PolicyHotelCapRateItemId.ToString() + "/" + policyHotelCapRateGroupItemLanguage.LanguageCode;
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            
            }

            //Return
            return RedirectToAction("List", new { id = policyHotelCapRateGroupItemLanguage.PolicyHotelCapRateItemId });
        }
    }
}
