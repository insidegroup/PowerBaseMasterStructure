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

namespace CWTDesktopDatabase.Controllers
{
    public class PriceTrackingContactController : Controller
    {
		PriceTrackingContactRepository priceTrackingContactRepository = new PriceTrackingContactRepository();
        PriceTrackingSetupGroupRepository priceTrackingSetupGroupRepository = new PriceTrackingSetupGroupRepository();

        ContactTypeRepository contactTypeRepository = new ContactTypeRepository();
        PriceTrackingContactUserTypeRepository priceTrackingContactUserTypeRepository = new PriceTrackingContactUserTypeRepository();
        PriceTrackingEmailAlertTypeRepository priceTrackingEmailAlertTypeRepository = new PriceTrackingEmailAlertTypeRepository();
        CommonRepository commonRepository = new CommonRepository();

        HierarchyRepository hierarchyRepository = new HierarchyRepository();

		private string groupName = "Price Tracking Setup Administrator";
		
		// GET: /List
        public ActionResult List(int id, string filter, int? page, string sortField, int? sortOrder)
        {
			//Set Access Rights
			ViewData["Access"] = "";
			RolesRepository rolesRepository = new RolesRepository();
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}
			
			//SortField
            if (string.IsNullOrEmpty(sortField))
            {
				sortField = "ContactTypeName";
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

            //PriceTrackingSetupGroup
            PriceTrackingSetupGroup priceTrackingSetupGroup = new PriceTrackingSetupGroup();
            priceTrackingSetupGroup = priceTrackingSetupGroupRepository.GetPriceTrackingSetupGroup(id);
            if (priceTrackingSetupGroup != null)
            {
                ViewData["PriceTrackingSetupGroupId"] = priceTrackingSetupGroup.PriceTrackingSetupGroupId;
                ViewData["PriceTrackingSetupGroupName"] = priceTrackingSetupGroup.PriceTrackingSetupGroupName;
            }

            //return items
            var cwtPaginatedList = priceTrackingContactRepository.PagePriceTrackingContacts(id, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

		// GET: /Create
		[HttpGet]
		public ActionResult Create(int id)
		{
            //Check Parent Exists
            PriceTrackingSetupGroup priceTrackingSetupGroup = new PriceTrackingSetupGroup();
            priceTrackingSetupGroup = priceTrackingSetupGroupRepository.GetPriceTrackingSetupGroup(id);

            //Check Exists
            if (priceTrackingSetupGroup == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			PriceTrackingContactVM PriceTrackingContactVM = new PriceTrackingContactVM();

			//Create Item 
			PriceTrackingContact priceTrackingContact = new PriceTrackingContact();
            priceTrackingContact.PriceTrackingSetupGroupId = id;
            priceTrackingContact.PriceTrackingSetupGroup = priceTrackingSetupGroup;

            //ContactTypes
            PriceTrackingContactVM.ContactTypes = new SelectList(contactTypeRepository.GetAllContactTypes().ToList(), "ContactTypeId", "ContactTypeName");

            //PriceTrackingContactUserTypes
            PriceTrackingContactVM.PriceTrackingContactUserTypes = new SelectList(priceTrackingContactUserTypeRepository.GetAllPriceTrackingContactUserTypes().ToList(), "PriceTrackingContactUserTypeId", "PriceTrackingContactUserTypeName");

            //PriceTrackingDashboardAccessTypes
            PriceTrackingContactVM.PriceTrackingDashboardAccessTypes = new SelectList(commonRepository.GetTrueFalseList().ToList(), "Value", "Text");

            //PriceTrackingEmailAlertTypes
            PriceTrackingContactVM.PriceTrackingEmailAlertTypes = new SelectList(priceTrackingEmailAlertTypeRepository.GetAllPriceTrackingEmailAlertTypes().ToList(), "PriceTrackingEmailAlertTypeId", "PriceTrackingEmailAlertTypeName");

            PriceTrackingContactVM.PriceTrackingContact = priceTrackingContact;

			return View(PriceTrackingContactVM);
		}

		// POST: //Create/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(PriceTrackingContact priceTrackingContact, FormCollection formCollection)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update  Model from Form
			try
			{
				UpdateModel(priceTrackingContact);
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
				priceTrackingContactRepository.Add(priceTrackingContact);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { id = priceTrackingContact.PriceTrackingSetupGroupId });
		}

		// GET: /Edit
		public ActionResult Edit(int id)
		{
			//Get Item 
			PriceTrackingContact priceTrackingContact = new PriceTrackingContact();
			priceTrackingContact = priceTrackingContactRepository.GetPriceTrackingContact(id);

			//Check Exists
			if (priceTrackingContact == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			PriceTrackingContactVM PriceTrackingContactVM = new PriceTrackingContactVM();

			priceTrackingContactRepository.EditForDisplay(priceTrackingContact);

            //ContactTypes
            PriceTrackingContactVM.ContactTypes = new SelectList(contactTypeRepository.GetAllContactTypes().ToList(), "ContactTypeId", "ContactTypeName", priceTrackingContact.ContactTypeId);

            //PriceTrackingContactUserTypes
            PriceTrackingContactVM.PriceTrackingContactUserTypes = new SelectList(priceTrackingContactUserTypeRepository.GetAllPriceTrackingContactUserTypes().ToList(), "PriceTrackingContactUserTypeId", "PriceTrackingContactUserTypeName", priceTrackingContact.PriceTrackingContactUserTypeId);

            //PriceTrackingDashboardAccessTypes
            PriceTrackingContactVM.PriceTrackingDashboardAccessTypes = new SelectList(commonRepository.GetTrueFalseList().ToList(), "Value", "Text", priceTrackingContact.PriceTrackingDashboardAccessFlag);

            //PriceTrackingEmailAlertTypes
            PriceTrackingContactVM.PriceTrackingEmailAlertTypes = new SelectList(priceTrackingEmailAlertTypeRepository.GetAllPriceTrackingEmailAlertTypes().ToList(), "PriceTrackingEmailAlertTypeId", "PriceTrackingEmailAlertTypeName", priceTrackingContact.PriceTrackingEmailAlertTypeId);

            PriceTrackingContactVM.PriceTrackingContact = priceTrackingContact;

			return View(PriceTrackingContactVM);
		}

		// POST: //Edit/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(PriceTrackingContactVM priceTrackingContactVM, FormCollection formCollection)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update  Model from Form
			try
			{
				UpdateModel(priceTrackingContactVM);
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
				priceTrackingContactRepository.Update(priceTrackingContactVM.PriceTrackingContact);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { id = priceTrackingContactVM.PriceTrackingContact.PriceTrackingSetupGroupId });
		}

		// GET: Delete
		[HttpGet]
		public ActionResult Delete(int id)
		{
			PriceTrackingContactVM PriceTrackingContactVM = new PriceTrackingContactVM();

			PriceTrackingContact priceTrackingContact = new PriceTrackingContact();
			priceTrackingContact = priceTrackingContactRepository.GetPriceTrackingContact(id);

			//Check Exists
			if (priceTrackingContact == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			priceTrackingContactRepository.EditForDisplay(priceTrackingContact);
			
			PriceTrackingContactVM.PriceTrackingContact = priceTrackingContact;

			return View(PriceTrackingContactVM);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(PriceTrackingContactVM priceTrackingContactVM)
		{
			//Get Item 
			PriceTrackingContact priceTrackingContact = new PriceTrackingContact();
			priceTrackingContact = priceTrackingContactRepository.GetPriceTrackingContact(priceTrackingContactVM.PriceTrackingContact.PriceTrackingContactId);

			//Check Exists
			if (priceTrackingContactVM.PriceTrackingContact == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Delete Item
			try
			{
				priceTrackingContactRepository.Delete(priceTrackingContact);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PriceTrackingContact.mvc/Delete/" + priceTrackingContact.PriceTrackingContactId;
					return View("VersionError");
				}

				//Generic Error
				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			//Return
			return RedirectToAction("List", new { id = priceTrackingContact.PriceTrackingSetupGroupId });
		}
    }
}
