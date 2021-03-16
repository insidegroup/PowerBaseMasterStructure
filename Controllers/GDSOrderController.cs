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
using Persits.PDF;
using System.IO;
using System.Net.Mail;
using System.Web.Mail;
using System.Net;
using System.Text;
using System.Configuration;

namespace CWTDesktopDatabase.Controllers
{
    public class GDSOrderController : Controller
    {
		//Repositories
		GDSOrderRepository gdsOrderRepository = new GDSOrderRepository();		
		GDSRepository gdsRepository = new GDSRepository();
		CountryRepository countryRepository = new CountryRepository();
		GDSOrderTypeRepository gdsOrderTypeRepository = new GDSOrderTypeRepository();
		GDSOrderStatusRepository gdsOrderStatusRepository = new GDSOrderStatusRepository();
		PseudoCityOrOfficeAddressRepository pseudoCityOrOfficeAddressRepository = new PseudoCityOrOfficeAddressRepository();
		GDSThirdPartyVendorRepository gdsThirdPartyVendorRepository = new GDSThirdPartyVendorRepository();
		GDSOrderLineItemActionRepository gdsOrderLineItemActionRepository = new GDSOrderLineItemActionRepository();

		HierarchyRepository hierarchyRepository = new HierarchyRepository();

		private string groupName = "GDS Order Administrator";
		
		// GET: /List
        public ActionResult List(
			string filter, 
			int? page, 
			string sortField, 
			int? sortOrder,
			int? gdsOrderId = null,
			int? gdsOrderStatusId = null, 
			string ticketNumber = null,
			string pseudoCityOrOfficeId = "",
			int? gdsOrderTypeId = null,
			DateTime? gdsOrderDateTimeStart = null,
			DateTime? gdsOrderDateTimeEnd = null,
			string orderAnalyst = "",
			string internalSiteName = "",
			string gdsCode = "")
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
				sortField = "GDSOrderId";
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

			//Default Filters
			ViewData["GDSOrderId"] = gdsOrderId ?? null;
			ViewData["GDSOrderStatusId"] = gdsOrderStatusId ?? null;
			ViewData["TicketNumber"] = ticketNumber ?? "";
			ViewData["PseudoCityOrOfficeId"] = pseudoCityOrOfficeId ?? null;
			ViewData["GDSOrderTypeId"] = gdsOrderTypeId ?? null;
			ViewData["GDSOrderDateTimeStart"] = gdsOrderDateTimeStart ?? null;
			ViewData["GDSOrderDateTimeEnd"] = gdsOrderDateTimeEnd ?? null;
			ViewData["OrderAnalyst"] = orderAnalyst ?? "";
			ViewData["InternalSiteName"] = internalSiteName ?? "";
			ViewData["GDSCode"] = gdsCode ?? "";

			ViewData["Analysts"] = new SelectList(gdsOrderRepository.GetAllGDSOrderAnalysts().ToList().OrderBy(x => x.Value), "Key", "Value", orderAnalyst);

			ViewData["GDSs"] = new SelectList(gdsRepository.GetAllGDSs().ToDictionary(x => x.GDSCode, x => x.GDSName).OrderBy(x => x.Value), "Key", "Value", gdsCode);

			ViewData["OrderStatuses"] = new SelectList(gdsOrderStatusRepository.GetAllGDSOrderStatuses().ToDictionary(x => x.GDSOrderStatusId, x => x.GDSOrderStatusName).OrderBy(x => x.Value), "Key", "Value", gdsOrderStatusId);

			ViewData["OrderTypes"] = new SelectList(gdsOrderTypeRepository.GetAllGDSOrderTypes().ToDictionary(x => x.GDSOrderTypeId, x => x.GDSOrderTypeName).OrderBy(x => x.Value), "Key", "Value", gdsOrderTypeId);
			 
            //return items
			var cwtPaginatedList = gdsOrderRepository.PageGDSOrders(
				page ?? 1, 
				filter ?? "", 
				sortField, 
				sortOrder ?? 0,
				gdsOrderId ?? null,
				gdsOrderStatusId ?? null,
				ticketNumber ?? "",
				pseudoCityOrOfficeId ?? "",
				gdsOrderTypeId ?? null,
				gdsOrderDateTimeStart ?? null,
				gdsOrderDateTimeEnd ?? null,
				orderAnalyst ?? "",
				internalSiteName ?? "",
				gdsCode ?? ""
			);
            
			return View(cwtPaginatedList);
        }

        // GET: /View
        public ActionResult ViewItem(int id)
        {
            //Check Exists
            GDSOrder gdsOrder = new GDSOrder();
			gdsOrder = gdsOrderRepository.GetGDSOrder(id);
            if (gdsOrder == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

			gdsOrderRepository.EditForDisplay(gdsOrder);

			gdsOrder.GDSThirdPartyVendors = gdsOrderRepository.GetGDSOrderThirdPartyVendors(gdsOrder.GDSOrderId);

			PseudoCityOrOfficeMaintenanceRepository pseudoCityOrOfficeMaintenanceRepository = new PseudoCityOrOfficeMaintenanceRepository();
			pseudoCityOrOfficeMaintenanceRepository.EditForDisplay(gdsOrder.PseudoCityOrOfficeMaintenance);

            return View(gdsOrder);
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

			GDSOrderVM GDSOrderVM = new GDSOrderVM();

			//Empty list for dropdowns populated by Ajax
			List<SelectListItem> blankList = new List<SelectListItem>();

			//Create Item 
			GDSOrder gdsOrder = new GDSOrder();

			//Order Analyst
			GDSOrderAnalyst gdsOrderAnalyst = gdsOrderRepository.GetGDSOrderAnalyst();
			if (gdsOrderAnalyst != null)
			{
				gdsOrder.OrderAnalystName = gdsOrderAnalyst.FirstName + " " + gdsOrderAnalyst.LastName;
				gdsOrder.OrderAnalystEmail = gdsOrderAnalyst.Email;
				gdsOrder.OrderAnalystPhone = gdsOrderAnalyst.Phone;
				gdsOrder.OrderAnalystCountryCode = gdsOrderAnalyst.CountryCode;
			} 
					
			//GDS Order
			GDSOrderVM.OrderAnalystCountries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName", gdsOrder.OrderAnalystCountryCode);
			GDSOrderVM.RequesterCountries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName");
			GDSOrderVM.GDSs = new SelectList(gdsRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName");
			GDSOrderVM.GDSOrderTypes = new SelectList(gdsOrderTypeRepository.GetAllGDSOrderTypes().ToList(), "GDSOrderTypeId", "GDSOrderTypeName");
			GDSOrderVM.GDSOrderStatuses = new SelectList(gdsOrderStatusRepository.GetAllGDSOrderStatuses().ToList(), "GDSOrderStatusId", "GDSOrderStatusName");
			//GDSOrderVM.SelectedPseudoCityOrOfficeAddress = new SelectList(pseudoCityOrOfficeAddressRepository.GetSelectedPseudoCityOrOfficeAddress(gdsOrder.PseudoCityOrOfficeAddressId).ToList(), "PseudoCityOrOfficeAddressId", "FirstAddressLine", gdsOrder.PseudoCityOrOfficeAddressId);
			GDSOrderVM.PseudoCityOrOfficeAddresses = new SelectList(pseudoCityOrOfficeAddressRepository.GetUserPseudoCityOrOfficeAddresses().ToList(), "PseudoCityOrOfficeAddressId", "FirstAddressLine");
			GDSOrderVM.GDSOrderLineItemActions = new SelectList(gdsOrderLineItemActionRepository.GetAllGDSOrderLineItemActions().ToList(), "GDSOrderLineItemActionId", "GDSOrderLineItemActionName");

			//ThirdPartyVendors
			GDSThirdPartyVendorRepository gdsThirdPartyVendorRepository = new GDSThirdPartyVendorRepository();
			GDSOrderVM.GDSThirdPartyVendors = gdsThirdPartyVendorRepository.GetAllGDSThirdPartyVendors();

			List<SelectListItem> gdsThirdPartyVendors = new List<SelectListItem>();
			foreach (GDSThirdPartyVendor gdsThirdPartyVendor in GDSOrderVM.GDSThirdPartyVendors)
			{
				gdsThirdPartyVendors.Add(
					new SelectListItem
					{
						Value = gdsThirdPartyVendor.GDSThirdPartyVendorId.ToString(),
						Text = gdsThirdPartyVendor.GDSThirdPartyVendorName.ToString()
					}
				);
			}

			//GDS Order - ThirdPartyVendors
			GDSOrderVM.GDSThirdPartyVendorsList = new MultiSelectList(
				gdsThirdPartyVendors,
				"Value",
				"Text"
			);
			
			//Checkboxes
			gdsOrder.PseudoCityOrOfficeMaintenance_ActiveFlagNonNullable = true;
			gdsOrder.PseudoCityOrOfficeMaintenance_SharedPseudoCityOrOfficeFlagNonNullable = false;
			gdsOrder.PseudoCityOrOfficeMaintenance_CWTOwnedPseudoCityOrOfficeFlagNonNullable = false;
			gdsOrder.PseudoCityOrOfficeMaintenance_ClientDedicatedPseudoCityOrOfficeFlagNonNullable = false;
			gdsOrder.PseudoCityOrOfficeMaintenance_ClientGDSAccessFlagNonNullable = false;
			gdsOrder.PseudoCityOrOfficeMaintenance_DevelopmentOrInternalPseudoCityOrOfficeFlagNonNullable = false;
			gdsOrder.PseudoCityOrOfficeMaintenance_CubaPseudoCityOrOfficeFlagNonNullable = false;
			gdsOrder.PseudoCityOrOfficeMaintenance_GovernmentPseudoCityOrOfficeFlagNonNullable = false;
			gdsOrder.PseudoCityOrOfficeMaintenance_GDSThirdPartyVendorFlagNonNullable = false;

			//IATAs
			IATARepository iataRepository = new IATARepository();
			GDSOrderVM.IATAs = new SelectList(iataRepository.GetAllIATAs().ToList(), "IATAId", "IATANumber");

			//GDSs
			GDSOrderVM.GDSs = new SelectList(gdsRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName");

			//PseudoCityOrOfficeDefinedRegions
			PseudoCityOrOfficeDefinedRegionRepository pseudoCityOrOfficeDefinedRegionRepository = new PseudoCityOrOfficeDefinedRegionRepository();
			GDSOrderVM.PseudoCityOrOfficeDefinedRegions = new SelectList(blankList, "PseudoCityOrOfficeDefinedRegionId", "PseudoCityOrOfficeDefinedRegionName");

			//ExternalNames
			ExternalNameRepository externalNameRepository = new ExternalNameRepository();
			GDSOrderVM.ExternalNames = new SelectList(externalNameRepository.GetAllExternalNames().ToList(), "ExternalNameId", "ExternalName1");

			//PseudoCityOrOfficeTypes
			PseudoCityOrOfficeTypeRepository pseudoCityOrOfficeTypeRepository = new PseudoCityOrOfficeTypeRepository();
			GDSOrderVM.PseudoCityOrOfficeTypes = new SelectList(pseudoCityOrOfficeTypeRepository.GetAllPseudoCityOrOfficeTypes().ToList(), "PseudoCityOrOfficeTypeId", "PseudoCityOrOfficeTypeName");

			//PseudoCityOrOfficeLocationTypes
			PseudoCityOrOfficeLocationTypeRepository pseudoCityOrOfficeLocationTypeRepository = new PseudoCityOrOfficeLocationTypeRepository();
			GDSOrderVM.PseudoCityOrOfficeLocationTypes = new SelectList(pseudoCityOrOfficeLocationTypeRepository.GetAllPseudoCityOrOfficeLocationTypes().ToList(), "PseudoCityOrOfficeLocationTypeId", "PseudoCityOrOfficeLocationTypeName");

			//FareRedistributions
			FareRedistributionRepository fareRedistributionRepository = new FareRedistributionRepository();
			GDSOrderVM.FareRedistributions = new SelectList(blankList, "FareRedistributionId", "FareRedistributionName");

			//CubaPseudoCityOrOfficeFlagNonNullable
			//Only a user with the Compliance Administrator for All Clients Globally and a GDS Administrator role (Global or Global Region or Country) can check or uncheck this box
			ViewData["ComplianceAdministratorAccess"] = "";
			if (rolesRepository.PseudoCityOrOfficeMaintenance_CubaPseudoCityOrOfficeFlag())
			{
				ViewData["ComplianceAdministratorAccess"] = "WriteAccess";
			}

			//GovernmentPseudoCityOrOfficeFlagNonNullable
			//Only a user with the GDS Government Administrator role (Global or Global Region or Country) can check or uncheck this box
			ViewData["GDSGovernmentAdministratorAccess"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess("GDS Government Administrator"))
			{
				ViewData["GDSGovernmentAdministratorAccess"] = "WriteAccess";
			}
		
			//PseudoCityOrOfficeMaintenance - GDSThirdPartyVendors
			GDSOrderVM.PseudoCityOrOfficeMaintenanceGDSThirdPartyVendorsList = new MultiSelectList(
				gdsThirdPartyVendors,
				"Value",
				"Text"
			);
			
			GDSOrderVM.GDSOrder = gdsOrder;

			return View(GDSOrderVM);
		}

		// POST: //Create/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(GDSOrderVM gdsOrderVM, FormCollection formCollection)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Capture GDSOrderLineItems
			gdsOrderVM.GDSOrder.GDSOrderLineItemsXML = GetGDSOrderLineItems(formCollection);

			//Convert GDSThirdPartyVendorIds to GDSThirdPartyVendors
			gdsOrderVM.GDSOrder.GDSThirdPartyVendors = GetGDSThirdPartyVendors(gdsOrderVM.GDSThirdPartyVendorIds);
			gdsOrderVM.PseudoCityOrOfficeMaintenanceGDSThirdPartyVendors = GetGDSThirdPartyVendors(gdsOrderVM.PseudoCityOrOfficeMaintenanceGDSThirdPartyVendorIds);
			
			//Update  Model from Form
			try
			{
				UpdateModel(gdsOrderVM.GDSOrder);
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
				gdsOrderRepository.Add(gdsOrderVM);
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
			GDSOrder gdsOrder = new GDSOrder();
			gdsOrder = gdsOrderRepository.GetGDSOrder(id);

			//Check Exists
			if (gdsOrder == null)
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

			//Empty list for dropdowns populated by Ajax
			List<SelectListItem> blankList = new List<SelectListItem>();
			
			GDSOrderVM GDSOrderVM = new GDSOrderVM();

			//Order Analyst
			GDSOrderAnalyst gdsOrderAnalyst = gdsOrderRepository.GetGDSOrderAnalyst();
			if (gdsOrderAnalyst != null)
			{
				gdsOrder.OrderAnalystName = gdsOrderAnalyst.FirstName + " " + gdsOrderAnalyst.LastName;
				gdsOrder.OrderAnalystEmail = gdsOrderAnalyst.Email;
				gdsOrder.OrderAnalystPhone = gdsOrderAnalyst.Phone;
				gdsOrder.OrderAnalystCountryCode = gdsOrderAnalyst.CountryCode;
			}
			
			gdsOrderRepository.EditForDisplay(gdsOrder);

			PseudoCityOrOfficeMaintenanceRepository pseudoCityOrOfficeMaintenanceRepository = new PseudoCityOrOfficeMaintenanceRepository();
			pseudoCityOrOfficeMaintenanceRepository.EditForDisplay(gdsOrder.PseudoCityOrOfficeMaintenance);

			GDSOrderVM.GDSOrder = gdsOrder;

			//Select Lists
			GDSOrderVM.OrderAnalystCountries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName", gdsOrder.OrderAnalystCountryCode);
			GDSOrderVM.RequesterCountries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName", gdsOrder.RequesterCountryCode);
			GDSOrderVM.GDSs = new SelectList(gdsRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName", gdsOrder.GDSCode);
			GDSOrderVM.GDSOrderTypes = new SelectList(gdsOrderTypeRepository.GetAllGDSOrderTypes().ToList(), "GDSOrderTypeId", "GDSOrderTypeName", gdsOrder.GDSOrderTypeId);
			GDSOrderVM.GDSOrderStatuses = new SelectList(gdsOrderStatusRepository.GetAllGDSOrderStatuses().ToList(), "GDSOrderStatusId", "GDSOrderStatusName", gdsOrder.GDSOrderStatusId);
			GDSOrderVM.PseudoCityOrOfficeAddresses = new SelectList(pseudoCityOrOfficeAddressRepository.GetUserPseudoCityOrOfficeAddresses().ToList(), "PseudoCityOrOfficeAddressId", "FirstAddressLine");
			GDSOrderVM.GDSOrderLineItemActions = new SelectList(gdsOrderLineItemActionRepository.GetAllGDSOrderLineItemActions().ToList(), "GDSOrderLineItemActionId", "GDSOrderLineItemActionName");

			//ThirdPartyVendors
			GDSThirdPartyVendorRepository gdsThirdPartyVendorRepository = new GDSThirdPartyVendorRepository();
			GDSOrderVM.GDSThirdPartyVendors = gdsThirdPartyVendorRepository.GetAllGDSThirdPartyVendors();

			List<SelectListItem> gdsThirdPartyVendors = new List<SelectListItem>();
			foreach (GDSThirdPartyVendor gdsThirdPartyVendor in GDSOrderVM.GDSThirdPartyVendors)
			{
				gdsThirdPartyVendors.Add(
					new SelectListItem
					{
						Value = gdsThirdPartyVendor.GDSThirdPartyVendorId.ToString(),
						Text = gdsThirdPartyVendor.GDSThirdPartyVendorName.ToString()
					}
				);
			}

			//GDS Order - ThirdPartyVendors
			List<GDSThirdPartyVendor> selectedGDSThirdPartyVendors = gdsOrderRepository.GetGDSOrderThirdPartyVendors(gdsOrder.GDSOrderId);
			GDSOrderVM.GDSThirdPartyVendorsList = new MultiSelectList(
				gdsThirdPartyVendors,
				"Value",
				"Text",
				selectedGDSThirdPartyVendors.Select(x => x.GDSThirdPartyVendorId).ToArray()
			);

			//IATAs
			IATARepository iataRepository = new IATARepository();
			GDSOrderVM.IATAs = new SelectList(iataRepository.GetAllIATAs().ToList(), "IATAId", "IATANumber");

			//GDSs
			GDSRepository GDSRepository = new GDSRepository();
			GDSOrderVM.GDSs = new SelectList(GDSRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName");

			//PseudoCityOrOfficeDefinedRegions
			PseudoCityOrOfficeDefinedRegionRepository pseudoCityOrOfficeDefinedRegionRepository = new PseudoCityOrOfficeDefinedRegionRepository();
			GDSOrderVM.PseudoCityOrOfficeDefinedRegions = new SelectList(
				pseudoCityOrOfficeDefinedRegionRepository.GetPseudoCityOrOfficeDefinedRegionsForGlobalRegionCode(gdsOrder.PseudoCityOrOfficeMaintenance.GlobalRegionCode), 
				"PseudoCityOrOfficeDefinedRegionId", 
				"PseudoCityOrOfficeDefinedRegionName",
				gdsOrder.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeDefinedRegionId
			);

			//ExternalNames
			ExternalNameRepository externalNameRepository = new ExternalNameRepository();
			GDSOrderVM.ExternalNames = new SelectList(externalNameRepository.GetAllExternalNames().ToList(), "ExternalNameId", "ExternalName1");

			//PseudoCityOrOfficeTypes
			PseudoCityOrOfficeTypeRepository pseudoCityOrOfficeTypeRepository = new PseudoCityOrOfficeTypeRepository();
			GDSOrderVM.PseudoCityOrOfficeTypes = new SelectList(pseudoCityOrOfficeTypeRepository.GetAllPseudoCityOrOfficeTypes().ToList(), "PseudoCityOrOfficeTypeId", "PseudoCityOrOfficeTypeName");

			//PseudoCityOrOfficeLocationTypes
			PseudoCityOrOfficeLocationTypeRepository pseudoCityOrOfficeLocationTypeRepository = new PseudoCityOrOfficeLocationTypeRepository();
			GDSOrderVM.PseudoCityOrOfficeLocationTypes = new SelectList(pseudoCityOrOfficeLocationTypeRepository.GetAllPseudoCityOrOfficeLocationTypes().ToList(), "PseudoCityOrOfficeLocationTypeId", "PseudoCityOrOfficeLocationTypeName");

			//FareRedistributions
			FareRedistributionRepository fareRedistributionRepository = new FareRedistributionRepository();
			GDSOrderVM.FareRedistributions = new SelectList(
				fareRedistributionRepository.GetFareRedistributionsByGDSCode(gdsOrder.PseudoCityOrOfficeMaintenance.GDSCode),
				"FareRedistributionId", 
				"FareRedistributionName", 
				gdsOrder.PseudoCityOrOfficeMaintenance.FareRedistributionId
			);

			//CubaPseudoCityOrOfficeFlagNonNullable
			//Only a user with the Compliance Administrator for All Clients Globally and a GDS Administrator role (Global or Global Region or Country) can check or uncheck this box
			ViewData["ComplianceAdministratorAccess"] = "";
			if (rolesRepository.PseudoCityOrOfficeMaintenance_CubaPseudoCityOrOfficeFlag())
			{
				ViewData["ComplianceAdministratorAccess"] = "WriteAccess";
			}

			//GovernmentPseudoCityOrOfficeFlagNonNullable
			//Only a user with the GDS Government Administrator role (Global or Global Region or Country) can check or uncheck this box
			ViewData["GDSGovernmentAdministratorAccess"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess("GDS Government Administrator"))
			{
				ViewData["GDSGovernmentAdministratorAccess"] = "WriteAccess";
			}

			//PseudoCityOrOfficeMaintenance GDSThirdPartyVendors
			List<GDSThirdPartyVendor> selectedPseudoCityOrOfficeMaintenanceGDSThirdPartyVendors = gdsThirdPartyVendorRepository.GetGDSThirdPartyVendorsByPseudoCityOrOfficeMaintenanceId(
				gdsOrder.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeMaintenanceId
			);
			GDSOrderVM.PseudoCityOrOfficeMaintenanceGDSThirdPartyVendorsList = new MultiSelectList(
				gdsThirdPartyVendors,
				"Value",
				"Text",
				selectedPseudoCityOrOfficeMaintenanceGDSThirdPartyVendors.Select(x => x.GDSThirdPartyVendorId).ToArray()
			);

			return View(GDSOrderVM);
		}

		// POST: //Edit/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(GDSOrderVM gdsOrderVM, FormCollection formCollection)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Capture GDSOrderLineItems
			gdsOrderVM.GDSOrder.GDSOrderLineItemsXML = GetGDSOrderLineItems(formCollection);

			//Convert GDSThirdPartyVendorIds to GDSThirdPartyVendors
			gdsOrderVM.GDSOrder.GDSThirdPartyVendors = GetGDSThirdPartyVendors(gdsOrderVM.GDSThirdPartyVendorIds);
			gdsOrderVM.PseudoCityOrOfficeMaintenanceGDSThirdPartyVendors = GetGDSThirdPartyVendors(gdsOrderVM.PseudoCityOrOfficeMaintenanceGDSThirdPartyVendorIds);

			//Update  Model from Form
			try
			{
				UpdateModel(gdsOrderVM, "GDSOrderVM");
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
				gdsOrderRepository.Update(gdsOrderVM);
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

		// GET: /Email
		public ActionResult Email(int id)
		{
			//Check Exists
			GDSOrder gdsOrder = new GDSOrder();
			gdsOrder = gdsOrderRepository.GetGDSOrder(id);
			if (gdsOrder == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			GDSOrderVM gdsOrderVM = new GDSOrderVM();
			gdsOrderVM.GDSOrder = gdsOrder;

			gdsOrderRepository.EditForDisplay(gdsOrder);

			return View(gdsOrderVM);
		}

		// GET: /Email
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Email(GDSOrderVM gdsOrderVM)
		{
			//Check Exists
			GDSOrder gdsOrder = new GDSOrder();
			gdsOrder = gdsOrderRepository.GetGDSOrder(gdsOrderVM.GDSOrder.GDSOrderId);
			if (gdsOrder == null)
			{
				ViewData["ActionMethod"] = "EmailGet";
				return View("RecordDoesNotExistError");
			}

			gdsOrderRepository.EditForDisplay(gdsOrder);

			if(gdsOrderVM.GDSOrder.EmailToAddress == null || string.IsNullOrEmpty(gdsOrderVM.GDSOrder.EmailToAddress))
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError("Error sending GDSOrder email:" + gdsOrder.GDSOrderId.ToString());

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			try
			{
				gdsOrder.EmailToAddresses = gdsOrderVM.GDSOrder.EmailToAddress.Split(';').ToList();

				if (gdsOrderRepository.SendGDSOrderEmail(gdsOrder))
				{
					TempData["GDSOrder_Success"] = @"<div class=""GDSOrderMessage GDSOrderMessageSuccess"">Your order email has been sent.</div>";
				}
				else
				{
					LogRepository logRepository = new LogRepository();
					logRepository.LogError("Error sending GDSOrder email:" + gdsOrder.GDSOrderId.ToString());

					ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
					return View("Error");
				}
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
		
		private List<GDSOrderLineItem> GetGDSOrderLineItems(FormCollection formCollection)
		{
			List<GDSOrderLineItem> gdsOrderLineItems = new List<GDSOrderLineItem>();

			for (int counter = 0; counter < formCollection.Keys.Count; counter++)
			{
				string quantityKey = string.Format("GDSOrder.GDSOrderLineItem[{0}].Quantity", counter);
				string gdsOrderLineItemActionIdKey = string.Format("GDSOrder.GDSOrderLineItem[{0}].GDSOrderLineItemActionId", counter);
				string gdsOrderDetailId = string.Format("GDSOrder.GDSOrderLineItem[{0}].GDSOrderDetailId", counter);
				string commentKey = string.Format("GDSOrder.GDSOrderLineItem[{0}].Comment", counter);

				if (formCollection.AllKeys.Contains(quantityKey) && formCollection.AllKeys.Contains(gdsOrderLineItemActionIdKey) && formCollection.AllKeys.Contains(gdsOrderDetailId))
				{

					GDSOrderLineItem gdsOrderLineItem = new GDSOrderLineItem();

					//Quantity
					if (formCollection.AllKeys.Contains(quantityKey) && !string.IsNullOrEmpty(formCollection[quantityKey]))
					{
						gdsOrderLineItem.Quantity = Int32.Parse(formCollection[quantityKey]);
					}

					//GDSOrderLineItemActionId
					if (formCollection.AllKeys.Contains(gdsOrderLineItemActionIdKey) && !string.IsNullOrEmpty(formCollection[gdsOrderLineItemActionIdKey]))
					{
						gdsOrderLineItem.GDSOrderLineItemActionId = Int32.Parse(formCollection[gdsOrderLineItemActionIdKey]);
					}

					//GDSOrderDetailId
					if (formCollection.AllKeys.Contains(gdsOrderDetailId) && !string.IsNullOrEmpty(formCollection[gdsOrderDetailId]))
					{
						gdsOrderLineItem.GDSOrderDetailId = Int32.Parse(formCollection[gdsOrderDetailId]);
					}

					//Comment
					if (formCollection.AllKeys.Contains(commentKey) && !string.IsNullOrEmpty(formCollection[commentKey]))
					{
						gdsOrderLineItem.Comment = formCollection[commentKey];
					}

					gdsOrderLineItems.Add(gdsOrderLineItem);
				}
			}

			return gdsOrderLineItems;
		}

		//Get GDSThirdPartyVendors
		private List<GDSThirdPartyVendor> GetGDSThirdPartyVendors(int[] gdsThirdPartyVendorIds)
		{
			List<GDSThirdPartyVendor> gdsOrderRequestTypes = new List<GDSThirdPartyVendor>();
			if (gdsThirdPartyVendorIds != null)
			{
				foreach (int gdsOrderRequestTypeId in gdsThirdPartyVendorIds)
				{
					GDSThirdPartyVendorRepository gdsRequestTypeRepository = new GDSThirdPartyVendorRepository();
					GDSThirdPartyVendor gdsRequestType = gdsRequestTypeRepository.GetGDSThirdPartyVendor(gdsOrderRequestTypeId);
					if (gdsRequestType != null)
					{
						gdsOrderRequestTypes.Add(gdsRequestType);
					}
				}
			}
			return gdsOrderRequestTypes;
		}

		// GET: /Export
		public ActionResult Export(
			int? gdsOrderId = null,
			int? gdsOrderStatusId = null,
			string ticketNumber = null,
			string pseudoCityOrOfficeId = "",
			int? gdsOrderTypeId = null,
			DateTime? gdsOrderDateTimeStart = null,
			DateTime? gdsOrderDateTimeEnd = null,
			string orderAnalyst = "",
			string internalSiteName = "",
			string gdsCode = ""
		)
		{
			//Get CSV Data
			byte[] csvData = gdsOrderRepository.Export(
				gdsOrderId ?? null,
				gdsOrderStatusId ?? null,
				ticketNumber ?? "",
				pseudoCityOrOfficeId ?? "",
				gdsOrderTypeId ?? null,
				gdsOrderDateTimeStart ?? null,
				gdsOrderDateTimeEnd ?? null,
				orderAnalyst ?? "",
				internalSiteName ?? "",
				gdsCode ?? ""
			);

			return File(csvData, "text/csv", "GDS Order Export.csv");
		}

		//Get PseudoCityOrOfficeId
		[HttpPost]
		public JsonResult GetPseudoCityOrOfficeMaintenance(string searchText, string gdsCode)
		{
			return Json(gdsOrderRepository.GetPseudoCityOrOfficeMaintenance(searchText, gdsCode));
		}
	}
}
