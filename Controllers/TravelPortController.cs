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
    public class TravelPortController : Controller
    {
		TravelPortRepository travelPortRepository = new TravelPortRepository();
		TravelPortTypeRepository travelPortTypeRepository = new TravelPortTypeRepository();

		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		StateProvinceRepository stateProvinceRepository = new StateProvinceRepository();

		private string groupName = "System Data Administrator";
		
		// GET: /List
        public ActionResult List(string filter, int? page, string sortField, int? sortOrder)
        {
            RolesRepository rolesRepository = new RolesRepository();

            //Set Access Rights
            ViewData["Access"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Access"] = "WriteAccess";
            }
    
            //SortField
            if (sortField != "CountryName" && sortField != "TravelPortCode" && sortField != "TravelPortTypeDescription")
            {
                sortField = "TravelPortName";
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
            var cwtPaginatedList = travelPortRepository.PageTravelPorts(page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        // GET: /View
        public ActionResult ViewItem(string id)
        {
            //Check Exists
            TravelPort travelPort = new TravelPort();
            travelPort = travelPortRepository.GetTravelPort(id);
            if (travelPort == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            travelPortRepository.EditForDisplay(travelPort);
            return View(travelPort);
        }

		// GET: /Create
		[HttpGet]
		public ActionResult Create()
		{
			//Create Item 
			TravelPort travelPort = new TravelPort();

			//AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//StateProvince SelectList
			SelectList stateProvinceList = new SelectList(stateProvinceRepository.GetStateProvincesByCountryCode(travelPort.CountryCode).ToList(), "StateProvinceCode", "Name");
			ViewData["StateProvinceList"] = stateProvinceList;

			//TravelPort SelectList
			SelectList travelPortTypeList = new SelectList(travelPortTypeRepository.GetAllTravelPortTypes().ToList(), "TravelPortTypeId", "TravelPortTypeDescription");
			ViewData["TravelPortTypeList"] = travelPortTypeList;

			return View(travelPort);
		}

		// POST: //Create/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(TravelPort travelPort)
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
				UpdateModel(travelPort);
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
				travelPortRepository.Add(travelPort);
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
		public ActionResult Edit(string id)
		{
			//Get Item 
			TravelPort travelPort = new TravelPort();
			travelPort = travelPortRepository.GetTravelPort(id);

			//Check Exists
			if (travelPort == null)
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

			travelPortRepository.EditForDisplay(travelPort);
			
			//StateProvince SelectList
			SelectList stateProvinceList = new SelectList(stateProvinceRepository.GetStateProvincesByCountryCode(travelPort.CountryCode).ToList(), "StateProvinceCode", "Name", travelPort.StateProvinceCode);
			ViewData["StateProvinceList"] = stateProvinceList;

			//TravelPort SelectList
			SelectList travelPortTypeList = new SelectList(travelPortTypeRepository.GetAllTravelPortTypes().ToList(), "TravelPortTypeId", "TravelPortTypeDescription", travelPort.TravelPortTypeId);
			ViewData["TravelPortTypeList"] = travelPortTypeList;

			return View(travelPort);
		}

		// POST: //Edit/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(TravelPort travelPort)
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
				UpdateModel(travelPort);
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
				travelPortRepository.Update(travelPort);
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
		public ActionResult Delete(string id)
		{
			TravelPortVM travelPortVM = new TravelPortVM();
			travelPortVM.AllowDelete = true;

			TravelPort travelPort = new TravelPort();
			travelPort = travelPortRepository.GetTravelPort(id);

			//Check Exists
			if (travelPort == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			travelPortVM.TravelPort = travelPort;

			//Attached Items
			List<TravelPortReference> travelPortReferences = travelPortRepository.GetTravelPortReferences(travelPort.TravelPortCode);
			if (travelPortReferences.Count > 0)
			{
				travelPortVM.AllowDelete = false;
				travelPortVM.TravelPortReferences = travelPortReferences;
			}

			travelPortRepository.EditForDisplay(travelPort);

			return View(travelPortVM);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(TravelPortVM travelPortVM)
		{
			//Get Item 
			TravelPort travelPort = new TravelPort();
			travelPort = travelPortRepository.GetTravelPort(travelPortVM.TravelPort.TravelPortCode);

			//Check Exists
			if (travelPortVM.TravelPort == null)
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
				travelPortRepository.Delete(travelPort);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/TravelPort.mvc/Delete/" + travelPort.TravelPortCode;
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
