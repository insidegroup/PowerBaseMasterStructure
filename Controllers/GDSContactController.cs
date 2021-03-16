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
    public class GDSContactController : Controller
    {
		GDSContactRepository gdsContactRepository = new GDSContactRepository();
		GDSRepository gdsRepository = new GDSRepository();
		CountryRepository countryRepository = new CountryRepository();
		PseudoCityOrOfficeBusinessRepository pseudoCityOrOfficeBusinessRepository = new PseudoCityOrOfficeBusinessRepository();
		PseudoCityOrOfficeDefinedRegionRepository pseudoCityOrOfficeDefinedRegionRepository = new PseudoCityOrOfficeDefinedRegionRepository();
		GDSRequestTypeRepository gdsRequestTypeRepository = new GDSRequestTypeRepository();

		HierarchyRepository hierarchyRepository = new HierarchyRepository();

		private string groupName = "GDS Administrator";
		
		// GET: /List
        public ActionResult List(string filter, int? page, string sortField, int? sortOrder)
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
            }

            //return items
			var cwtPaginatedList = gdsContactRepository.PageGDSContacts(page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        // GET: /View
        public ActionResult ViewItem(int id)
        {
            //Check Exists
            GDSContact GDSContact = new GDSContact();
			GDSContact = gdsContactRepository.GetGDSContact(id);
            if (GDSContact == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

			gdsContactRepository.EditForDisplay(GDSContact);
            return View(GDSContact);
        }

		// GET: /Create
		[HttpGet]
		public ActionResult Create()
		{
			//AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			GDSContactVM GDSContactVM = new GDSContactVM();

			//Create Item 
			GDSContact gdsContact = new GDSContact();

			//Countries
			GDSContactVM.Countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName");

			//GDS
			GDSContactVM.GDSs = new SelectList(gdsRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName");

			//GDSRegions
			GDSContactVM.GlobalRegions = new SelectList(hierarchyRepository.GetAllGlobalRegions().ToList(), "GlobalRegionCode", "GlobalRegionName");

			//GDSRequestTypes
			GDSContactVM.GDSRequestTypes = new SelectList(gdsRequestTypeRepository.GetAllGDSRequestTypes().ToList(), "GDSRequestTypeId", "GDSRequestTypeName");

			//PseudoCityOrOfficeBusinesses
			GDSContactVM.PseudoCityOrOfficeBusinesses = new SelectList(pseudoCityOrOfficeBusinessRepository.GetAllPseudoCityOrOfficeBusinesses().ToList(), "PseudoCityOrOfficeBusinessId", "PseudoCityOrOfficeBusinessName");

			//PseudoCityOrOfficeDefinedRegions
			GDSContactVM.PseudoCityOrOfficeDefinedRegions = new SelectList(pseudoCityOrOfficeDefinedRegionRepository.GetAllPseudoCityOrOfficeDefinedRegions().ToList(), "PseudoCityOrOfficeDefinedRegionId", "PseudoCityOrOfficeDefinedRegionName");

			GDSContactVM.GDSContact = gdsContact;

			return View(GDSContactVM);
		}

		// POST: //Create/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(GDSContact gdsContact, FormCollection formCollection)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

            //Convert GDSRequestTypeIds to GDSRequestTypes
            List<GDSRequestType> gdsContactRequestTypes = new List<GDSRequestType>();
            string key = "GDSContact.GDSRequestTypeIds";
            if (formCollection[key] != null)
            {
                List<int> gdsContactRequestTypeIds = formCollection[key].Split(',').Select(Int32.Parse).ToList();
                foreach (int gdsContactRequestTypeId in gdsContactRequestTypeIds)
                {
                    GDSRequestTypeRepository gdsRequestTypeRepository = new GDSRequestTypeRepository();
                    GDSRequestType gdsRequestType = gdsRequestTypeRepository.GetGDSRequestType(gdsContactRequestTypeId);
                    if (gdsRequestType != null)
                    {
                        gdsContactRequestTypes.Add(gdsRequestType);
                    }
                }
            }
            gdsContact.GDSRequestTypes = gdsContactRequestTypes;

			//Update  Model from Form
			try
			{
				UpdateModel(gdsContact);
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
				gdsContactRepository.Add(gdsContact);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List");
		}

		// GET: /Edit
		public ActionResult Edit(int id)
		{
			//Get Item 
			GDSContact gdsContact = new GDSContact();
			gdsContact = gdsContactRepository.GetGDSContact(id);

			//Check Exists
			if (gdsContact == null)
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

			GDSContactVM GDSContactVM = new GDSContactVM();

			gdsContactRepository.EditForDisplay(gdsContact);

			//Countries
			GDSContactVM.Countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName", gdsContact.CountryCode);

			//GDS
			GDSContactVM.GDSs = new SelectList(gdsRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName", gdsContact.GDSCode);

			//GDSRegions
			GDSContactVM.GlobalRegions = new SelectList(hierarchyRepository.GetAllGlobalRegions().ToList(), "GlobalRegionCode", "GlobalRegionName", gdsContact.GlobalRegionCode);
			
			//GDSRequestTypes
			GDSContactVM.GDSRequestTypes = new SelectList(gdsRequestTypeRepository.GetAllGDSRequestTypes().ToList(), "GDSRequestTypeId", "GDSRequestTypeName", gdsContact.GDSRequestTypeIds);

			//PseudoCityOrOfficeBusinesses
			GDSContactVM.PseudoCityOrOfficeBusinesses = new SelectList(pseudoCityOrOfficeBusinessRepository.GetAllPseudoCityOrOfficeBusinesses().ToList(), "PseudoCityOrOfficeBusinessId", "PseudoCityOrOfficeBusinessName", gdsContact.PseudoCityOrOfficeBusinessId);

			//PseudoCityOrOfficeDefinedRegions
			GDSContactVM.PseudoCityOrOfficeDefinedRegions = new SelectList(pseudoCityOrOfficeDefinedRegionRepository.GetAllPseudoCityOrOfficeDefinedRegions().ToList(), "PseudoCityOrOfficeDefinedRegionId", "PseudoCityOrOfficeDefinedRegionName", gdsContact.PseudoCityOrOfficeDefinedRegionId);

			GDSContactVM.GDSContact = gdsContact;

			return View(GDSContactVM);
		}

		// POST: //Edit/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(GDSContactVM gdsContactVM, FormCollection formCollection)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Convert GDSRequestTypeIds to GDSRequestTypes
			List<GDSRequestType> gdsContactRequestTypes = new List<GDSRequestType>();
			string key = "GDSContact.GDSRequestTypeIds";
			if (formCollection[key] != null)
			{
				List<int> gdsContactRequestTypeIds = formCollection[key].Split(',').Select(Int32.Parse).ToList();
				foreach (int gdsContactRequestTypeId in gdsContactRequestTypeIds)
				{
					GDSRequestTypeRepository gdsRequestTypeRepository = new GDSRequestTypeRepository();
					GDSRequestType gdsRequestType = gdsRequestTypeRepository.GetGDSRequestType(gdsContactRequestTypeId);
					if (gdsRequestType != null)
					{
						gdsContactRequestTypes.Add(gdsRequestType);
					}
				}
			}
			gdsContactVM.GDSContact.GDSRequestTypes = gdsContactRequestTypes;

			//Update  Model from Form
			try
			{
				UpdateModel(gdsContactVM);
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
				gdsContactRepository.Update(gdsContactVM.GDSContact);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List");
		}

		// GET: Delete
		[HttpGet]
		public ActionResult Delete(int id)
		{
			GDSContactVM GDSContactVM = new GDSContactVM();

			GDSContact gdsContact = new GDSContact();
			gdsContact = gdsContactRepository.GetGDSContact(id);

			//Check Exists
			if (gdsContact == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			gdsContactRepository.EditForDisplay(gdsContact);
			
			GDSContactVM.GDSContact = gdsContact;

			return View(GDSContactVM);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(GDSContactVM gdsContactVM)
		{
			//Get Item 
			GDSContact gdsContact = new GDSContact();
			gdsContact = gdsContactRepository.GetGDSContact(gdsContactVM.GDSContact.GDSContactId);

			//Check Exists
			if (gdsContactVM.GDSContact == null)
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
				gdsContactRepository.Delete(gdsContact);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/GDSContact.mvc/Delete/" + gdsContact.GDSContactId;
					return View("VersionError");
				}

				//Generic Error
				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			//Return
			return RedirectToAction("List");
		}
    }
}
