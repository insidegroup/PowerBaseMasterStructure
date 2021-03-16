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
    public class HotelVendorAdviceController : Controller
    {
        //main repositories
        PolicyHotelVendorGroupItemLanguageRepository policyHotelVendorGroupItemLanguageRepository = new PolicyHotelVendorGroupItemLanguageRepository();
        PolicyHotelVendorGroupItemRepository policyHotelVendorGroupItemRepository = new PolicyHotelVendorGroupItemRepository();
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();

        //GET:List
        public ActionResult List(int id, int? page, string sortField, int? sortOrder)
        {
            //Get PolicyHotelVendorGroupItem
            PolicyHotelVendorGroupItem policyHotelVendorGroupItem = new PolicyHotelVendorGroupItem();
            policyHotelVendorGroupItem = policyHotelVendorGroupItemRepository.GetPolicyHotelVendorGroupItem(id);

            //Check Exists
            if (policyHotelVendorGroupItem == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToPolicyGroup(policyHotelVendorGroupItem.PolicyGroupId))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //SortField+SortOrder settings
            if (sortField != "Name")
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
                sortOrder = 0;
            }

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyHotelVendorGroupItem.PolicyGroupId);
            ViewData["PolicyGroupName"] = policyGroup.PolicyGroupName;
            ViewData["PolicyHotelVendorGroupItemId"] = id;

            //Get data
            var cwtPaginatedList = policyHotelVendorGroupItemLanguageRepository.PagePolicyHotelVendorGroupItemHotelAdvice(id, page ?? 1, sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        //GET: View
        public ActionResult View(int id, string languageCode)
        {
            PolicyHotelVendorGroupItemLanguage policyHotelVendorGroupItemLanguage = new PolicyHotelVendorGroupItemLanguage();
            policyHotelVendorGroupItemLanguage = policyHotelVendorGroupItemLanguageRepository.GetItem(id, languageCode);
            if (policyHotelVendorGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }
            policyHotelVendorGroupItemLanguageRepository.EditItemForDisplay(policyHotelVendorGroupItemLanguage);
            return View(policyHotelVendorGroupItemLanguage);
        }

        // GET: /Create
        public ActionResult Create(int id)
        {
            //Get PolicyHotelVendorGroupItem
            PolicyHotelVendorGroupItem policyHotelVendorGroupItem = new PolicyHotelVendorGroupItem();
            policyHotelVendorGroupItem = policyHotelVendorGroupItemRepository.GetPolicyHotelVendorGroupItem(id);

            //Check Exists
            if (policyHotelVendorGroupItem == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelVendorGroupItem.PolicyGroupId))
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("Error");
            }

            //New PolicyHotelVendorGroupItemLanguage
            PolicyHotelVendorGroupItemLanguage policyHotelVendorGroupItemLanguage = new PolicyHotelVendorGroupItemLanguage();
            policyHotelVendorGroupItemLanguage.PolicyHotelVendorGroupItemId = id;
            policyHotelVendorGroupItemLanguageRepository.EditItemForDisplay(policyHotelVendorGroupItemLanguage);

            //Language SelectList
            SelectList languageList = new SelectList(policyHotelVendorGroupItemLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            //Show Create Form
            return View(policyHotelVendorGroupItemLanguage);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicyHotelVendorGroupItemLanguage policyHotelVendorGroupItemLanguage)
        {
            PolicyHotelVendorGroupItem policyHotelVendorGroupItem = new PolicyHotelVendorGroupItem();
            policyHotelVendorGroupItem = policyHotelVendorGroupItemRepository.GetPolicyHotelVendorGroupItem(policyHotelVendorGroupItemLanguage.PolicyHotelVendorGroupItemId);

            //Check Exists
            if (policyHotelVendorGroupItem == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelVendorGroupItem.PolicyGroupId))
            {
                //AccessRights Error
                return View("Error");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(policyHotelVendorGroupItemLanguage);
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
                policyHotelVendorGroupItemLanguageRepository.Add(policyHotelVendorGroupItemLanguage);
            }
            catch
            {
                //Insert Error
                return View("Error");
            }

            return RedirectToAction("List", new { id = policyHotelVendorGroupItem.PolicyHotelVendorGroupItemId });
        }

        // GET: /Edit
        public ActionResult Edit(int id, string languageCode)
        {
            //Get Item 
            PolicyHotelVendorGroupItemLanguage policyHotelVendorGroupItemLanguage = new PolicyHotelVendorGroupItemLanguage();
            policyHotelVendorGroupItemLanguage = policyHotelVendorGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyHotelVendorGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyHotelVendorGroupItem policyHotelVendorGroupItem = new PolicyHotelVendorGroupItem();
            policyHotelVendorGroupItem = policyHotelVendorGroupItemRepository.GetPolicyHotelVendorGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelVendorGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Language SelectList
            SelectList languageList = new SelectList(policyHotelVendorGroupItemLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            policyHotelVendorGroupItemLanguageRepository.EditItemForDisplay(policyHotelVendorGroupItemLanguage);
            return View(policyHotelVendorGroupItemLanguage);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int policyHotelVendorGroupItemId, string languageCode, string hotelAdvice)
        {
            //Get Item 
            PolicyHotelVendorGroupItemLanguage policyHotelVendorGroupItemLanguage = new PolicyHotelVendorGroupItemLanguage();
            policyHotelVendorGroupItemLanguage = policyHotelVendorGroupItemLanguageRepository.GetItem(policyHotelVendorGroupItemId, languageCode);

            //Check Exists
            if (policyHotelVendorGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyHotelVendorGroupItem policyHotelVendorGroupItem = new PolicyHotelVendorGroupItem();
            policyHotelVendorGroupItem = policyHotelVendorGroupItemRepository.GetPolicyHotelVendorGroupItem(policyHotelVendorGroupItemId);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelVendorGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Item from Form
            try
            {
                UpdateModel(policyHotelVendorGroupItemLanguage);
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

            //Update CountryAdvice
            try
            {
                policyHotelVendorGroupItemLanguageRepository.Update(policyHotelVendorGroupItemLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/HotelAdvice.mvc/Edit/" + policyHotelVendorGroupItemLanguage.PolicyHotelVendorGroupItemId.ToString();
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }

            return RedirectToAction("List", new { id = policyHotelVendorGroupItemLanguage.PolicyHotelVendorGroupItemId });
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, string languageCode)
        {
            //Get Item 
            PolicyHotelVendorGroupItemLanguage policyHotelVendorGroupItemLanguage = new PolicyHotelVendorGroupItemLanguage();
            policyHotelVendorGroupItemLanguage = policyHotelVendorGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyHotelVendorGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyHotelVendorGroupItem policyHotelVendorGroupItem = new PolicyHotelVendorGroupItem();
            policyHotelVendorGroupItem = policyHotelVendorGroupItemRepository.GetPolicyHotelVendorGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelVendorGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Add Linked Information
            policyHotelVendorGroupItemLanguageRepository.EditItemForDisplay(policyHotelVendorGroupItemLanguage);

            //Return View
            return View(policyHotelVendorGroupItemLanguage);

        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, string languageCode, FormCollection collection)
        {
            //Get Item 
            PolicyHotelVendorGroupItemLanguage policyHotelVendorGroupItemLanguage = new PolicyHotelVendorGroupItemLanguage();
            policyHotelVendorGroupItemLanguage = policyHotelVendorGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyHotelVendorGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyHotelVendorGroupItem policyHotelVendorGroupItem = new PolicyHotelVendorGroupItem();
            policyHotelVendorGroupItem = policyHotelVendorGroupItemRepository.GetPolicyHotelVendorGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelVendorGroupItem.PolicyGroupId))
            {
               ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                policyHotelVendorGroupItemLanguage.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                policyHotelVendorGroupItemLanguageRepository.Delete(policyHotelVendorGroupItemLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/HotelAdvice.mvc/Delete/" + policyHotelVendorGroupItemLanguage.PolicyHotelVendorGroupItemId.ToString() + "/" + policyHotelVendorGroupItemLanguage.LanguageCode;
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }

            //Return
            return RedirectToAction("List", new { id = policyHotelVendorGroupItemLanguage.PolicyHotelVendorGroupItemId });
        }
    }
}
