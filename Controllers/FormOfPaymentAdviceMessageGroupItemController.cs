using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;
using System.Xml;
using CWTDesktopDatabase.ViewModels;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace CWTDesktopDatabase.Controllers
{
    public class FormOfPaymentAdviceMessageGroupItemController : Controller
    {
        //main repositories
        FormOfPaymentAdviceMessageGroupRepository formOfPaymentAdviceMessageGroupRepository = new FormOfPaymentAdviceMessageGroupRepository();
        FormOfPaymentAdviceMessageGroupItemRepository formOfPaymentAdviceMessageItemRepository = new FormOfPaymentAdviceMessageGroupItemRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();

        private string groupName = "Client Detail";

        // GET: /List
        public ActionResult List(int id, int? page, string filter, string sortField, int? sortOrder)
        {
            //Check Parent Exists
            FormOfPaymentAdviceMessageGroup formOfPaymentAdviceMessageGroup = new FormOfPaymentAdviceMessageGroup();
            formOfPaymentAdviceMessageGroup = formOfPaymentAdviceMessageGroupRepository.GetGroup(id);

            //Check Exists
            if (formOfPaymentAdviceMessageGroup == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

			//Set Access Rights
            ViewData["Access"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //SortField+SortOrder settings
            if (string.IsNullOrEmpty(sortField))
            {
				sortField = "FormOfPaymentAdviceMessageGroupName";
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

            ViewData["FormOfPaymentAdviceMessageGroupID"] = formOfPaymentAdviceMessageGroup.FormOfPaymentAdviceMessageGroupID;
            ViewData["FormOfPaymentAdviceMessageGroupName"] = formOfPaymentAdviceMessageGroup.FormOfPaymentAdviceMessageGroupName;

            //return items
            var cwtPaginatedList = formOfPaymentAdviceMessageItemRepository.PageFormOfPaymentAdviceMessageGroupItems(id, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

            return View(cwtPaginatedList);
        }

        // GET: /Create
        public ActionResult Create(int id)
        {
            //Check Parent Exists
            FormOfPaymentAdviceMessageGroup formOfPaymentAdviceMessageGroup = new FormOfPaymentAdviceMessageGroup();
            formOfPaymentAdviceMessageGroup = formOfPaymentAdviceMessageGroupRepository.GetGroup(id);

            //Check Exists
            if (formOfPaymentAdviceMessageGroup == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //Check Access
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

			FormOfPaymentAdviceMessageGroupItemVM formOfPaymentAdviceMessageGroupItemVM = new FormOfPaymentAdviceMessageGroupItemVM();

            FormOfPaymentAdviceMessageGroupItem formOfPaymentAdviceMessageItem = new FormOfPaymentAdviceMessageGroupItem();
            formOfPaymentAdviceMessageItem.FormOfPaymentAdviceMessageGroupName = formOfPaymentAdviceMessageGroup.FormOfPaymentAdviceMessageGroupName;
            formOfPaymentAdviceMessageItem.FormOfPaymentAdviceMessageGroupID = id;

			formOfPaymentAdviceMessageGroupItemVM.FormOfPaymentAdviceMessageGroup = formOfPaymentAdviceMessageGroup;

			//Language is a read only field defaulting to English (United Kingdom)
			LanguageRepository languageRepository = new LanguageRepository();
			Language language = languageRepository.GetLanguage("en-GB");
			if (language != null)
			{
				formOfPaymentAdviceMessageItem.LanguageCode = language.LanguageCode;
				formOfPaymentAdviceMessageItem.LanguageName = language.LanguageName;
			}

			formOfPaymentAdviceMessageGroupItemVM.FormOfPaymentAdviceMessageGroupItem = formOfPaymentAdviceMessageItem;

			CountryRepository countryRepository = new CountryRepository();
			formOfPaymentAdviceMessageGroupItemVM.Countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName");

			ProductRepository productRepository = new ProductRepository();
			formOfPaymentAdviceMessageGroupItemVM.Products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");

			TravelIndicatorRepository travelIndicatorRepository = new TravelIndicatorRepository();
			formOfPaymentAdviceMessageGroupItemVM.TravelIndicators = new SelectList(travelIndicatorRepository.GetAllTravelIndicators().OrderBy(x => x.TravelIndicatorDescription).ToList(), "TravelIndicator1", "TravelIndicatorDescription");

			FormOfPaymentTypeRepository formOfPaymentTypeRepository = new FormOfPaymentTypeRepository();
			formOfPaymentAdviceMessageGroupItemVM.FormOfPaymentTypes = new SelectList(formOfPaymentTypeRepository.GetAllFormOfPaymentTypes().ToList(), "FormOfPaymentTypeId", "FormOfPaymentTypeDescription");

            formOfPaymentAdviceMessageItemRepository.EditItemForDisplay(formOfPaymentAdviceMessageItem);

			return View(formOfPaymentAdviceMessageGroupItemVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
		public ActionResult Create(FormOfPaymentAdviceMessageGroupItemVM formOfPaymentAdviceMessageGroupItemVM)
        {

            //Get FormOfPaymentAdviceMessageGroup
            FormOfPaymentAdviceMessageGroup formOfPaymentAdviceMessageGroup = new FormOfPaymentAdviceMessageGroup();
			formOfPaymentAdviceMessageGroup = formOfPaymentAdviceMessageGroupRepository.GetGroup(formOfPaymentAdviceMessageGroupItemVM.FormOfPaymentAdviceMessageGroupItem.FormOfPaymentAdviceMessageGroupID);

            //Check Exists
            if (formOfPaymentAdviceMessageGroup == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }
            //Check Access
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            try
            {
				UpdateModel(formOfPaymentAdviceMessageGroupItemVM.FormOfPaymentAdviceMessageGroupItem);
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
				formOfPaymentAdviceMessageItemRepository.Add(formOfPaymentAdviceMessageGroupItemVM.FormOfPaymentAdviceMessageGroupItem);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

			return RedirectToAction("List", new { id = formOfPaymentAdviceMessageGroupItemVM.FormOfPaymentAdviceMessageGroupItem.FormOfPaymentAdviceMessageGroupID });
        }

        // GET: /Edit
        public ActionResult Edit(int id)
        {
            //Get Item
			FormOfPaymentAdviceMessageGroupItem formOfPaymentAdviceMessageGroupItem = new FormOfPaymentAdviceMessageGroupItem();
			formOfPaymentAdviceMessageGroupItem = formOfPaymentAdviceMessageItemRepository.GetItem(id);

            //Check Exists
			if (formOfPaymentAdviceMessageGroupItem == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check Parent Exists
            FormOfPaymentAdviceMessageGroup formOfPaymentAdviceMessageGroup = new FormOfPaymentAdviceMessageGroup();
			formOfPaymentAdviceMessageGroup = formOfPaymentAdviceMessageGroupRepository.GetGroup(formOfPaymentAdviceMessageGroupItem.FormOfPaymentAdviceMessageGroupID);

            //Check Exists
            if (formOfPaymentAdviceMessageGroup == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }
            
            //Check AccessRights
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


			FormOfPaymentAdviceMessageGroupItemVM formOfPaymentAdviceMessageGroupItemVM = new FormOfPaymentAdviceMessageGroupItemVM();

			formOfPaymentAdviceMessageGroupItemVM.FormOfPaymentAdviceMessageGroup = formOfPaymentAdviceMessageGroup;
			formOfPaymentAdviceMessageGroupItemVM.FormOfPaymentAdviceMessageGroupItem = formOfPaymentAdviceMessageGroupItem;

			CountryRepository countryRepository = new CountryRepository();
			formOfPaymentAdviceMessageGroupItemVM.Countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName", formOfPaymentAdviceMessageGroupItem.CountryCode);

			ProductRepository productRepository = new ProductRepository();
			formOfPaymentAdviceMessageGroupItemVM.Products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName", formOfPaymentAdviceMessageGroupItem.ProductId);

			TravelIndicatorRepository travelIndicatorRepository = new TravelIndicatorRepository();
			formOfPaymentAdviceMessageGroupItemVM.TravelIndicators = new SelectList(travelIndicatorRepository.GetAllTravelIndicators().OrderBy(x => x.TravelIndicatorDescription).ToList(), "TravelIndicator1", "TravelIndicatorDescription", formOfPaymentAdviceMessageGroupItem.TravelIndicator);

			FormOfPaymentTypeRepository formOfPaymentTypeRepository = new FormOfPaymentTypeRepository();
			formOfPaymentAdviceMessageGroupItemVM.FormOfPaymentTypes = new SelectList(formOfPaymentTypeRepository.GetAllFormOfPaymentTypes().ToList(), "FormOfPaymentTypeId", "FormOfPaymentTypeDescription", formOfPaymentAdviceMessageGroupItem.FormofPaymentTypeID);

			formOfPaymentAdviceMessageItemRepository.EditItemForDisplay(formOfPaymentAdviceMessageGroupItem);

			return View(formOfPaymentAdviceMessageGroupItemVM);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
		public ActionResult Edit(FormOfPaymentAdviceMessageGroupItemVM formOfPaymentAdviceMessageGroupItemVM)
        {
            //Get Item
			FormOfPaymentAdviceMessageGroupItem formOfPaymentAdviceMessageGroupItem = new FormOfPaymentAdviceMessageGroupItem();
			formOfPaymentAdviceMessageGroupItem = formOfPaymentAdviceMessageItemRepository.GetItem(formOfPaymentAdviceMessageGroupItemVM.FormOfPaymentAdviceMessageGroupItem.FormOfPaymentAdviceMessageGroupItemId);

            //Check Exists
			if (formOfPaymentAdviceMessageGroupItem == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            try
            {
				UpdateModel(formOfPaymentAdviceMessageGroupItem, "FormOfPaymentAdviceMessageGroupItem");
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
				formOfPaymentAdviceMessageItemRepository.Edit(formOfPaymentAdviceMessageGroupItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
					ViewData["ReturnURL"] = "/FormOfPaymentAdviceMessageGroupItem.mvc/Edit/" + formOfPaymentAdviceMessageGroupItem.FormOfPaymentAdviceMessageGroupItemId.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

			return RedirectToAction("List", new { id = formOfPaymentAdviceMessageGroupItem.FormOfPaymentAdviceMessageGroupID });
        }
        
        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            //Get Item
            FormOfPaymentAdviceMessageGroupItem formOfPaymentAdviceMessageItem = new FormOfPaymentAdviceMessageGroupItem();
            formOfPaymentAdviceMessageItem = formOfPaymentAdviceMessageItemRepository.GetItem(id);

            //Check Exists
            if (formOfPaymentAdviceMessageItem == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check Parent Exists
            FormOfPaymentAdviceMessageGroup formOfPaymentAdviceMessageGroup = new FormOfPaymentAdviceMessageGroup();
            formOfPaymentAdviceMessageGroup = formOfPaymentAdviceMessageGroupRepository.GetGroup(formOfPaymentAdviceMessageItem.FormOfPaymentAdviceMessageGroupID);

            //Check Exists
            if (formOfPaymentAdviceMessageGroup == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }
            
            //Check AccessRights
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Parent Information
            ViewData["FormOfPaymentAdviceMessageGroupID"] = formOfPaymentAdviceMessageGroup.FormOfPaymentAdviceMessageGroupID;
            ViewData["FormOfPaymentAdviceMessageGroupName"] = formOfPaymentAdviceMessageGroup.FormOfPaymentAdviceMessageGroupName;

            formOfPaymentAdviceMessageItemRepository.EditItemForDisplay(formOfPaymentAdviceMessageItem);

            return View(formOfPaymentAdviceMessageItem);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //Get Item
            FormOfPaymentAdviceMessageGroupItem formOfPaymentAdviceMessageItem = new FormOfPaymentAdviceMessageGroupItem();
            formOfPaymentAdviceMessageItem = formOfPaymentAdviceMessageItemRepository.GetItem(id);

            //Check Exists
            if (formOfPaymentAdviceMessageItem == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                formOfPaymentAdviceMessageItem.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                formOfPaymentAdviceMessageItemRepository.Delete(formOfPaymentAdviceMessageItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/FormOfPaymentAdviceMessageGroupItem.mvc/Delete/" + formOfPaymentAdviceMessageItem.FormOfPaymentAdviceMessageGroupItemId;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            //Return
            return RedirectToAction("List", new { id = formOfPaymentAdviceMessageItem.FormOfPaymentAdviceMessageGroupID });
        }
    }
}
