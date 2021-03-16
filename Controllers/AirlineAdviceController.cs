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
    public class AirlineAdviceController : Controller
    {
        //main repositories
        PolicyAirVendorGroupItemLanguageRepository policyAirVendorGroupItemLanguageRepository = new PolicyAirVendorGroupItemLanguageRepository();
        PolicyAirVendorGroupItemRepository policyAirVendorGroupItemRepository = new PolicyAirVendorGroupItemRepository();
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();

        //GET:List
        public ActionResult List(int id, int? page, string sortField, int? sortOrder)
        {
            //Get PolicyAirVendorGroupItem
            PolicyAirVendorGroupItem policyAirVendorGroupItem = new PolicyAirVendorGroupItem();
            policyAirVendorGroupItem = policyAirVendorGroupItemRepository.GetPolicyAirVendorGroupItem(id);

            //Check Exists
            if (policyAirVendorGroupItem == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToPolicyGroup(policyAirVendorGroupItem.PolicyGroupId))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Parent Information
            ViewData["PolicyAirVendorGroupItemID"] = policyAirVendorGroupItem.PolicyAirVendorGroupItemId;
            ViewData["PolicyGroupID"] = policyAirVendorGroupItem.PolicyGroupId;
            ViewData["PolicyGroupName"] = policyGroupRepository.GetGroup(policyAirVendorGroupItem.PolicyGroupId).PolicyGroupName;


            //SortField+SortOrder settings
            if (sortField != "AirlineAdvice")
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
            var cwtPaginatedList = policyAirVendorGroupItemLanguageRepository.PagePolicyAirVendorGroupItemAirlineAdvice(id, page ?? 1, sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        //GET: View
        public ActionResult View(int id, string languageCode)
        {
            PolicyAirVendorGroupItemLanguage policyAirVendorGroupItemLanguage = new PolicyAirVendorGroupItemLanguage();
            policyAirVendorGroupItemLanguage = policyAirVendorGroupItemLanguageRepository.GetItem(id, languageCode);
            if (policyAirVendorGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            
            policyAirVendorGroupItemLanguageRepository.EditItemForDisplay(policyAirVendorGroupItemLanguage);
            return View(policyAirVendorGroupItemLanguage);
        }

        // GET: /Create
        public ActionResult Create(int id)
        {
            //Get PolicyAirVendorGroupItem
            PolicyAirVendorGroupItem policyAirVendorGroupItem = new PolicyAirVendorGroupItem();
            policyAirVendorGroupItem = policyAirVendorGroupItemRepository.GetPolicyAirVendorGroupItem(id);

            //Check Exists
            if (policyAirVendorGroupItem == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirVendorGroupItem.PolicyGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //New PolicyAirVendorGroupItemLanguage
            PolicyAirVendorGroupItemLanguage policyAirVendorGroupItemLanguage = new PolicyAirVendorGroupItemLanguage();
            policyAirVendorGroupItemLanguage.PolicyAirVendorGroupItemId = id;
            policyAirVendorGroupItemLanguageRepository.EditItemForDisplay(policyAirVendorGroupItemLanguage);

            //Language SelectList
            SelectList languageList = new SelectList(policyAirVendorGroupItemLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            //Show Create Form
            return View(policyAirVendorGroupItemLanguage);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicyAirVendorGroupItemLanguage policyAirVendorGroupItemLanguage)
        {
            PolicyAirVendorGroupItem policyAirVendorGroupItem = new PolicyAirVendorGroupItem();
            policyAirVendorGroupItem = policyAirVendorGroupItemRepository.GetPolicyAirVendorGroupItem(policyAirVendorGroupItemLanguage.PolicyAirVendorGroupItemId);
            
            //Check Exists
            if (policyAirVendorGroupItem == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirVendorGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(policyAirVendorGroupItemLanguage);
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


            policyAirVendorGroupItemLanguageRepository.Add(policyAirVendorGroupItemLanguage);

            return RedirectToAction("List", new { id = policyAirVendorGroupItem.PolicyAirVendorGroupItemId });
        }

        // GET: /Edit
        public ActionResult Edit(int id, string languageCode)
        {
            //Get Item 
            PolicyAirVendorGroupItemLanguage policyAirVendorGroupItemLanguage = new PolicyAirVendorGroupItemLanguage();
            policyAirVendorGroupItemLanguage = policyAirVendorGroupItemLanguageRepository.GetItem(id,languageCode);

            //Check Exists
            if (policyAirVendorGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyAirVendorGroupItem policyAirVendorGroupItem = new PolicyAirVendorGroupItem();
            policyAirVendorGroupItem = policyAirVendorGroupItemRepository.GetPolicyAirVendorGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirVendorGroupItem.PolicyGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Language SelectList
            SelectList languageList = new SelectList(policyAirVendorGroupItemLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            policyAirVendorGroupItemLanguageRepository.EditItemForDisplay(policyAirVendorGroupItemLanguage);
            return View(policyAirVendorGroupItemLanguage);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string languageCode, FormCollection formCollection)
        {
            //Get Item 
            PolicyAirVendorGroupItemLanguage policyAirVendorGroupItemLanguage = new PolicyAirVendorGroupItemLanguage();
            policyAirVendorGroupItemLanguage = policyAirVendorGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyAirVendorGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyAirVendorGroupItem policyAirVendorGroupItem = new PolicyAirVendorGroupItem();
            policyAirVendorGroupItem = policyAirVendorGroupItemRepository.GetPolicyAirVendorGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirVendorGroupItem.PolicyGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Item from Form
            try
            {
                UpdateModel(policyAirVendorGroupItemLanguage);
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



            //Update AirlineAdvice
            try
            {
                policyAirVendorGroupItemLanguageRepository.Update(policyAirVendorGroupItemLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/AirlineAdvice.mvc/Edit/" + policyAirVendorGroupItemLanguage.PolicyAirVendorGroupItemId.ToString();
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }

            return RedirectToAction("List", new { id = policyAirVendorGroupItemLanguage.PolicyAirVendorGroupItemId });
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, string languageCode)
        {
            //Get Item 
            PolicyAirVendorGroupItemLanguage policyAirVendorGroupItemLanguage = new PolicyAirVendorGroupItemLanguage();
            policyAirVendorGroupItemLanguage = policyAirVendorGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyAirVendorGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyAirVendorGroupItem policyAirVendorGroupItem = new PolicyAirVendorGroupItem();
            policyAirVendorGroupItem = policyAirVendorGroupItemRepository.GetPolicyAirVendorGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirVendorGroupItem.PolicyGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            
            //Add Linked Information
            policyAirVendorGroupItemLanguageRepository.EditItemForDisplay(policyAirVendorGroupItemLanguage);

            //Return View
            return View(policyAirVendorGroupItemLanguage);

            }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, string languageCode, FormCollection collection)
        {
            //Get Item 
            PolicyAirVendorGroupItemLanguage policyAirVendorGroupItemLanguage = new PolicyAirVendorGroupItemLanguage();
            policyAirVendorGroupItemLanguage = policyAirVendorGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyAirVendorGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyAirVendorGroupItem policyAirVendorGroupItem = new PolicyAirVendorGroupItem();
            policyAirVendorGroupItem = policyAirVendorGroupItemRepository.GetPolicyAirVendorGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirVendorGroupItem.PolicyGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                policyAirVendorGroupItemLanguage.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                policyAirVendorGroupItemLanguageRepository.Delete(policyAirVendorGroupItemLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/AirlineAdvice.mvc/Delete/" + policyAirVendorGroupItemLanguage.PolicyAirVendorGroupItemId.ToString() + "/" + policyAirVendorGroupItemLanguage.LanguageCode;
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }


            //Return
            return RedirectToAction("List", new { id = policyAirVendorGroupItemLanguage.PolicyAirVendorGroupItemId });
        }
    }
}
