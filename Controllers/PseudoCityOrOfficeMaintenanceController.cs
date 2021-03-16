using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Controllers
{
	public class PseudoCityOrOfficeMaintenanceController : Controller
	{
		PseudoCityOrOfficeMaintenanceRepository pseudoCityOrOfficeMaintenanceRepository = new PseudoCityOrOfficeMaintenanceRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		CountryRepository countryRepository = new CountryRepository();
		StateProvinceRepository stateProvinceRepository = new StateProvinceRepository();
		RolesRepository rolesRepository = new RolesRepository();

		private string groupName = "GDS Administrator";

		// GET: /List
		public ActionResult ListUndeleted(int? page, string filter, string sortField, int? sortOrder)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//SortField
			if (string.IsNullOrEmpty(sortField))
			{
				sortField = "GlobalRegionName";
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
				sortOrder = 0;
			}

			//Populate View Model
			PseudoCityOrOfficeMaintenancesVM pseudoCityOrOfficeMaintenancesVM = new PseudoCityOrOfficeMaintenancesVM();

			var getPseudoCityOrOfficeMaintenances = pseudoCityOrOfficeMaintenanceRepository.GetPseudoCityOrOfficeMaintenances(sortField, sortOrder ?? 0, filter ?? "", page ?? 1, false);
			if (getPseudoCityOrOfficeMaintenances != null)
			{
				pseudoCityOrOfficeMaintenancesVM.PseudoCityOrOfficeMaintenances = getPseudoCityOrOfficeMaintenances;
			}

			return View(pseudoCityOrOfficeMaintenancesVM);
		}

		// GET: /List
		public ActionResult ListDeleted(int? page, string filter, string sortField, int? sortOrder)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//SortField
			if (string.IsNullOrEmpty(sortField))
			{
				sortField = "GlobalRegionName";
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
				sortOrder = 0;
			}

			//Populate View Model
			PseudoCityOrOfficeMaintenancesVM pseudoCityOrOfficeMaintenancesVM = new PseudoCityOrOfficeMaintenancesVM();

			var getPseudoCityOrOfficeMaintenances = pseudoCityOrOfficeMaintenanceRepository.GetPseudoCityOrOfficeMaintenances(sortField, sortOrder ?? 0, filter ?? "", page ?? 1, true);
			if (getPseudoCityOrOfficeMaintenances != null)
			{
				pseudoCityOrOfficeMaintenancesVM.PseudoCityOrOfficeMaintenances = getPseudoCityOrOfficeMaintenances;
			}

			return View(pseudoCityOrOfficeMaintenancesVM);
		}

		public ActionResult Create()
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			PseudoCityOrOfficeMaintenanceVM pseudoCityOrOfficeMaintenanceVM = new PseudoCityOrOfficeMaintenanceVM();
			PseudoCityOrOfficeMaintenance pseudoCityOrOfficeMaintenance = new PseudoCityOrOfficeMaintenance();

			//Checkboxes
			pseudoCityOrOfficeMaintenance.ActiveFlagNonNullable = true;
			pseudoCityOrOfficeMaintenance.SharedPseudoCityOrOfficeFlagNonNullable = false;
			pseudoCityOrOfficeMaintenance.CWTOwnedPseudoCityOrOfficeFlagNonNullable = false;
			pseudoCityOrOfficeMaintenance.ClientDedicatedPseudoCityOrOfficeFlagNonNullable = false;
			pseudoCityOrOfficeMaintenance.ClientGDSAccessFlagNonNullable = false;
			pseudoCityOrOfficeMaintenance.DevelopmentOrInternalPseudoCityOrOfficeFlagNonNullable = false;
			pseudoCityOrOfficeMaintenance.CubaPseudoCityOrOfficeFlagNonNullable = false;
			pseudoCityOrOfficeMaintenance.GovernmentPseudoCityOrOfficeFlagNonNullable = false;
			pseudoCityOrOfficeMaintenance.GDSThirdPartyVendorFlagNonNullable = false;

			pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance = pseudoCityOrOfficeMaintenance;

			//Created as dropdowns are using Ajax
			List<SelectListItem> blankList = new List<SelectListItem>();

			//IATAs
			IATARepository iataRepository = new IATARepository();
			pseudoCityOrOfficeMaintenanceVM.IATAs = new SelectList(iataRepository.GetAllIATAs().ToList(), "IATAId", "IATANumber");

			//GDSs
			GDSRepository GDSRepository = new GDSRepository();
			pseudoCityOrOfficeMaintenanceVM.GDSs = new SelectList(GDSRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName");

			//PseudoCityOrOfficeAddresses
			//Addresses will only display based upon the role of the system user and their Location. 
			PseudoCityOrOfficeAddressRepository pseudoCityOrOfficeAddressRepository = new PseudoCityOrOfficeAddressRepository();
			pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeAddresses = new SelectList(pseudoCityOrOfficeAddressRepository.GetUserPseudoCityOrOfficeAddresses().ToList(), "PseudoCityOrOfficeAddressId", "FirstAddressLine");

			//PseudoCityOrOfficeDefinedRegions
			PseudoCityOrOfficeDefinedRegionRepository pseudoCityOrOfficeDefinedRegionRepository = new PseudoCityOrOfficeDefinedRegionRepository();
			pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeDefinedRegions = new SelectList(blankList, "PseudoCityOrOfficeDefinedRegionId", "PseudoCityOrOfficeDefinedRegionName");

			//ExternalNames
			ExternalNameRepository externalNameRepository = new ExternalNameRepository();
			pseudoCityOrOfficeMaintenanceVM.ExternalNames = new SelectList(externalNameRepository.GetAllExternalNames().ToList(), "ExternalNameId", "ExternalName1");

			//PseudoCityOrOfficeTypes
			PseudoCityOrOfficeTypeRepository pseudoCityOrOfficeTypeRepository = new PseudoCityOrOfficeTypeRepository();
			pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeTypes = new SelectList(pseudoCityOrOfficeTypeRepository.GetAllPseudoCityOrOfficeTypes().ToList(), "PseudoCityOrOfficeTypeId", "PseudoCityOrOfficeTypeName");

			//PseudoCityOrOfficeLocationTypes
			PseudoCityOrOfficeLocationTypeRepository pseudoCityOrOfficeLocationTypeRepository = new PseudoCityOrOfficeLocationTypeRepository();
			pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeLocationTypes = new SelectList(pseudoCityOrOfficeLocationTypeRepository.GetAllPseudoCityOrOfficeLocationTypes().ToList(), "PseudoCityOrOfficeLocationTypeId", "PseudoCityOrOfficeLocationTypeName");

			//FareRedistributions
			FareRedistributionRepository fareRedistributionRepository = new FareRedistributionRepository();
			pseudoCityOrOfficeMaintenanceVM.FareRedistributions = new SelectList(blankList, "FareRedistributionId", "FareRedistributionName");

			//GDSThirdPartyVendors
			GDSThirdPartyVendorRepository gdsThirdPartyVendorRepository = new GDSThirdPartyVendorRepository();
			pseudoCityOrOfficeMaintenanceVM.GDSThirdPartyVendors = gdsThirdPartyVendorRepository.GetAllGDSThirdPartyVendors();

			List<SelectListItem> gdsThirdPartyVendors = new List<SelectListItem>();
			foreach (GDSThirdPartyVendor gdsThirdPartyVendor in pseudoCityOrOfficeMaintenanceVM.GDSThirdPartyVendors)
			{
				gdsThirdPartyVendors.Add(
					new SelectListItem
					{
						Value = gdsThirdPartyVendor.GDSThirdPartyVendorId.ToString(),
						Text = gdsThirdPartyVendor.GDSThirdPartyVendorName
					}
				);
			}

			List<GDSThirdPartyVendor> selectedGDSThirdPartyVendors = gdsThirdPartyVendorRepository.GetGDSThirdPartyVendorsByPseudoCityOrOfficeMaintenanceId(pseudoCityOrOfficeMaintenance.PseudoCityOrOfficeMaintenanceId);
			pseudoCityOrOfficeMaintenanceVM.GDSThirdPartyVendorsList = new MultiSelectList(
				gdsThirdPartyVendors,
				"Value",
				"Text"
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

			return View(pseudoCityOrOfficeMaintenanceVM);
		}

		// POST: /Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(PseudoCityOrOfficeMaintenanceVM pseudoCityOrOfficeMaintenanceVM, FormCollection formCollection)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update  Model from Form
			try
			{
				TryUpdateModel<PseudoCityOrOfficeMaintenanceVM>(pseudoCityOrOfficeMaintenanceVM, "GDSPseudoCityOrOfficeMaintenanceVM");
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

			//ClientSubUnitGuids
			List<string> clientSubUnitGuids = new List<string>();
			foreach (string key in formCollection)
			{
				if (key.StartsWith("PseudoCityOrOfficeMaintenance.ClientSubUnitGuid") && !string.IsNullOrEmpty(formCollection[key]))
				{
					string clientSubUnitGuid = formCollection[key];
					clientSubUnitGuids.Add(clientSubUnitGuid);
				}
			}
			pseudoCityOrOfficeMaintenanceVM.ClientSubUnitGuids = clientSubUnitGuids.Distinct().ToList();

			//GDSThirdPartyVendors
			List<GDSThirdPartyVendor> gdsThirdPartyVendors = new List<GDSThirdPartyVendor>();
			foreach (string key in formCollection)
			{
				if (key.StartsWith("GDSThirdPartyVendor") && !string.IsNullOrEmpty(formCollection[key]))
				{
					List<int> gdsThirdPartyVendorIds = formCollection[key].Split(',').Select(int.Parse).ToList();
					foreach (int gdsThirdPartyVendorId in gdsThirdPartyVendorIds)
					{
						GDSThirdPartyVendorRepository gdsThirdPartyVendorRepository = new GDSThirdPartyVendorRepository();
						GDSThirdPartyVendor gdsThirdPartyVendor = gdsThirdPartyVendorRepository.GetGDSThirdPartyVendor(gdsThirdPartyVendorId);
						if (gdsThirdPartyVendor != null)
						{
							gdsThirdPartyVendors.Add(gdsThirdPartyVendor);
						}
					}
				}
			}
			pseudoCityOrOfficeMaintenanceVM.GDSThirdPartyVendors = gdsThirdPartyVendors;

			try
			{
				pseudoCityOrOfficeMaintenanceRepository.Add(pseudoCityOrOfficeMaintenanceVM);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("ListUnDeleted");
		}

		// GET: /Edit
		public ActionResult Edit(int id)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			PseudoCityOrOfficeMaintenance pseudoCityOrOfficeMaintenance = pseudoCityOrOfficeMaintenanceRepository.GetPseudoCityOrOfficeMaintenance(id);

			//Check Exists
			if (pseudoCityOrOfficeMaintenance == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			PseudoCityOrOfficeMaintenanceVM pseudoCityOrOfficeMaintenanceVM = new PseudoCityOrOfficeMaintenanceVM();

			//Checkboxes
			pseudoCityOrOfficeMaintenance.ActiveFlagNonNullable = pseudoCityOrOfficeMaintenance.ActiveFlag.Value;
			pseudoCityOrOfficeMaintenance.SharedPseudoCityOrOfficeFlagNonNullable = pseudoCityOrOfficeMaintenance.SharedPseudoCityOrOfficeFlag.Value;
			pseudoCityOrOfficeMaintenance.CWTOwnedPseudoCityOrOfficeFlagNonNullable = pseudoCityOrOfficeMaintenance.CWTOwnedPseudoCityOrOfficeFlag.Value;
			pseudoCityOrOfficeMaintenance.ClientDedicatedPseudoCityOrOfficeFlagNonNullable = pseudoCityOrOfficeMaintenance.ClientDedicatedPseudoCityOrOfficeFlag.Value;
			pseudoCityOrOfficeMaintenance.ClientGDSAccessFlagNonNullable = pseudoCityOrOfficeMaintenance.ClientGDSAccessFlag.Value;
			pseudoCityOrOfficeMaintenance.DevelopmentOrInternalPseudoCityOrOfficeFlagNonNullable = pseudoCityOrOfficeMaintenance.DevelopmentOrInternalPseudoCityOrOfficeFlag.Value;
			pseudoCityOrOfficeMaintenance.CubaPseudoCityOrOfficeFlagNonNullable = pseudoCityOrOfficeMaintenance.CubaPseudoCityOrOfficeFlag.Value;
			pseudoCityOrOfficeMaintenance.GovernmentPseudoCityOrOfficeFlagNonNullable = pseudoCityOrOfficeMaintenance.GovernmentPseudoCityOrOfficeFlag.Value;
			pseudoCityOrOfficeMaintenance.GDSThirdPartyVendorFlagNonNullable = pseudoCityOrOfficeMaintenance.GDSThirdPartyVendorFlag.Value;

			pseudoCityOrOfficeMaintenanceRepository.EditForDisplay(pseudoCityOrOfficeMaintenance);

			pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance = pseudoCityOrOfficeMaintenance;

			//IATAs
			IATARepository iataRepository = new IATARepository();
			pseudoCityOrOfficeMaintenanceVM.IATAs = new SelectList(iataRepository.GetAllIATAs().ToList(), "IATAId", "IATANumber", pseudoCityOrOfficeMaintenance.IATAId);

			//GDSs
			GDSRepository GDSRepository = new GDSRepository();
			pseudoCityOrOfficeMaintenanceVM.GDSs = new SelectList(GDSRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName", pseudoCityOrOfficeMaintenance.GDSCode);

			//PseudoCityOrOfficeAddresses
			//Addresses will only display based upon the role of the system user and their Location. 
			PseudoCityOrOfficeAddressRepository pseudoCityOrOfficeAddressRepository = new PseudoCityOrOfficeAddressRepository();
			pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeAddresses = new SelectList(pseudoCityOrOfficeAddressRepository.GetUserPseudoCityOrOfficeAddresses().ToList(), "PseudoCityOrOfficeAddressId", "FirstAddressLine", pseudoCityOrOfficeMaintenance.PseudoCityOrOfficeAddressId);

			//PseudoCityOrOfficeDefinedRegions
			PseudoCityOrOfficeDefinedRegionRepository pseudoCityOrOfficeDefinedRegionRepository = new PseudoCityOrOfficeDefinedRegionRepository();
			pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeDefinedRegions = new SelectList(
				pseudoCityOrOfficeDefinedRegionRepository.GetPseudoCityOrOfficeDefinedRegionsForGlobalRegionCode(pseudoCityOrOfficeMaintenance.GlobalRegionCode).ToList(), 
				"PseudoCityOrOfficeDefinedRegionId", 
				"PseudoCityOrOfficeDefinedRegionName", 
				pseudoCityOrOfficeMaintenance.PseudoCityOrOfficeDefinedRegionId
			);

			//ExternalNames
			ExternalNameRepository externalNameRepository = new ExternalNameRepository();
			pseudoCityOrOfficeMaintenanceVM.ExternalNames = new SelectList(externalNameRepository.GetAllExternalNames().ToList(), "ExternalNameId", "ExternalName1", pseudoCityOrOfficeMaintenance.ExternalNameId);

			//PseudoCityOrOfficeTypes
			PseudoCityOrOfficeTypeRepository pseudoCityOrOfficeTypeRepository = new PseudoCityOrOfficeTypeRepository();
			pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeTypes = new SelectList(pseudoCityOrOfficeTypeRepository.GetAllPseudoCityOrOfficeTypes().ToList(), "PseudoCityOrOfficeTypeId", "PseudoCityOrOfficeTypeName", pseudoCityOrOfficeMaintenance.PseudoCityOrOfficeTypeId);

			//PseudoCityOrOfficeLocationTypes
			PseudoCityOrOfficeLocationTypeRepository pseudoCityOrOfficeLocationTypeRepository = new PseudoCityOrOfficeLocationTypeRepository();
			pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeLocationTypes = new SelectList(pseudoCityOrOfficeLocationTypeRepository.GetAllPseudoCityOrOfficeLocationTypes().ToList(), "PseudoCityOrOfficeLocationTypeId", "PseudoCityOrOfficeLocationTypeName", pseudoCityOrOfficeMaintenance.PseudoCityOrOfficeLocationTypeId);

			//FareRedistributions
			FareRedistributionRepository fareRedistributionRepository = new FareRedistributionRepository();
			pseudoCityOrOfficeMaintenanceVM.FareRedistributions = new SelectList(
				fareRedistributionRepository.GetFareRedistributionsByGDSCode(pseudoCityOrOfficeMaintenance.GDSCode).ToList(), 
				"FareRedistributionId", 
				"FareRedistributionName", 
				pseudoCityOrOfficeMaintenance.FareRedistributionId
			);

			//GDSThirdPartyVendors
			GDSThirdPartyVendorRepository gdsThirdPartyVendorRepository = new GDSThirdPartyVendorRepository();
			pseudoCityOrOfficeMaintenanceVM.GDSThirdPartyVendors = gdsThirdPartyVendorRepository.GetAllGDSThirdPartyVendors();

			List<SelectListItem> gdsThirdPartyVendors = new List<SelectListItem>();
			foreach (GDSThirdPartyVendor gdsThirdPartyVendor in pseudoCityOrOfficeMaintenanceVM.GDSThirdPartyVendors)
			{
				gdsThirdPartyVendors.Add(
					new SelectListItem
					{
						Value = gdsThirdPartyVendor.GDSThirdPartyVendorId.ToString(),
						Text = gdsThirdPartyVendor.GDSThirdPartyVendorName
					}
				);
			}

			List<GDSThirdPartyVendor> selectedGDSThirdPartyVendors = gdsThirdPartyVendorRepository.GetGDSThirdPartyVendorsByPseudoCityOrOfficeMaintenanceId(pseudoCityOrOfficeMaintenance.PseudoCityOrOfficeMaintenanceId);
			pseudoCityOrOfficeMaintenanceVM.GDSThirdPartyVendorsList = new MultiSelectList(
				gdsThirdPartyVendors,
				"Value",
				"Text",
				selectedGDSThirdPartyVendors.Select(x => x.GDSThirdPartyVendorId).ToArray()
			);

			//ClientSubUnits
			pseudoCityOrOfficeMaintenanceVM.ClientSubUnits = pseudoCityOrOfficeMaintenanceRepository.GetAllPseudoCityOrOfficeMaintenanceClientSubUnits(pseudoCityOrOfficeMaintenance.PseudoCityOrOfficeMaintenanceId);

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

			return View(pseudoCityOrOfficeMaintenanceVM);
		}

		// POST: /Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(PseudoCityOrOfficeMaintenanceVM pseudoCityOrOfficeMaintenanceVM, FormCollection formCollection)
		{
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update  Model from Form
			try
			{
				TryUpdateModel<PseudoCityOrOfficeMaintenance>(pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance, "PseudoCityOrOfficeMaintenance");
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

			//ClientSubUnitGuids
			List<string> clientSubUnitGuids = new List<string>();
			foreach (string key in formCollection)
			{
				if (key.StartsWith("ClientSubUnitGuids") && !string.IsNullOrEmpty(formCollection[key]))
				{
					clientSubUnitGuids = formCollection[key].Split(',').ToList();
				}
			}
			pseudoCityOrOfficeMaintenanceVM.ClientSubUnitGuids = clientSubUnitGuids.Distinct().ToList();

			//GDSThirdPartyVendors
			List<GDSThirdPartyVendor> gdsThirdPartyVendors = new List<GDSThirdPartyVendor>();
			foreach (string key in formCollection)
			{
				if (key.StartsWith("GDSThirdPartyVendor") && !string.IsNullOrEmpty(formCollection[key]))
				{
					List<int> gdsThirdPartyVendorIds = formCollection[key].Split(',').Select(int.Parse).ToList();
					foreach (int gdsThirdPartyVendorId in gdsThirdPartyVendorIds)
					{
						GDSThirdPartyVendorRepository gdsThirdPartyVendorRepository = new GDSThirdPartyVendorRepository();
						GDSThirdPartyVendor gdsThirdPartyVendor = gdsThirdPartyVendorRepository.GetGDSThirdPartyVendor(gdsThirdPartyVendorId);
						if (gdsThirdPartyVendor != null)
						{
							gdsThirdPartyVendors.Add(gdsThirdPartyVendor);
						}
					}
				}
			}
			pseudoCityOrOfficeMaintenanceVM.GDSThirdPartyVendors = gdsThirdPartyVendors;

			try
			{
				pseudoCityOrOfficeMaintenanceRepository.Update(pseudoCityOrOfficeMaintenanceVM);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			string returnAction = (pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.DeletedFlag == true) ? "ListDeleted" : "ListUnDeleted";
			return RedirectToAction(returnAction);
		}

		// GET: /View
		[HttpGet]
		public ActionResult View(int id)
		{
			PseudoCityOrOfficeMaintenance pseudoCityOrOfficeMaintenance = new PseudoCityOrOfficeMaintenance();
			pseudoCityOrOfficeMaintenance = pseudoCityOrOfficeMaintenanceRepository.GetPseudoCityOrOfficeMaintenance(id);

			//Check Exists
			if (pseudoCityOrOfficeMaintenance == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			PseudoCityOrOfficeMaintenanceVM pseudoCityOrOfficeMaintenanceVM = new PseudoCityOrOfficeMaintenanceVM();

			pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance = pseudoCityOrOfficeMaintenance;
			pseudoCityOrOfficeMaintenanceRepository.EditForDisplay(pseudoCityOrOfficeMaintenance);

			return View(pseudoCityOrOfficeMaintenanceVM);
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
		{
			PseudoCityOrOfficeMaintenance pseudoCityOrOfficeMaintenance = new PseudoCityOrOfficeMaintenance();
			pseudoCityOrOfficeMaintenance = pseudoCityOrOfficeMaintenanceRepository.GetPseudoCityOrOfficeMaintenance(id);

			//Check Exists
			if (pseudoCityOrOfficeMaintenance == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			//Check AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			PseudoCityOrOfficeMaintenanceVM pseudoCityOrOfficeMaintenanceVM = new PseudoCityOrOfficeMaintenanceVM();

			pseudoCityOrOfficeMaintenanceRepository.EditForDisplay(pseudoCityOrOfficeMaintenance);

			pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance = pseudoCityOrOfficeMaintenance;

			return View(pseudoCityOrOfficeMaintenanceVM);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(PseudoCityOrOfficeMaintenanceVM pseudoCityOrOfficeMaintenanceVM, FormCollection collection)
		{
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Get Item From Database
			PseudoCityOrOfficeMaintenance pseudoCityOrOfficeMaintenance = new PseudoCityOrOfficeMaintenance();
			pseudoCityOrOfficeMaintenance = pseudoCityOrOfficeMaintenanceRepository.GetPseudoCityOrOfficeMaintenance(pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeMaintenanceId);

			//Check Exists
			if (pseudoCityOrOfficeMaintenance == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Delete Item
			try
			{
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.DeletedFlag = true;
				pseudoCityOrOfficeMaintenanceRepository.UpdateDeletedStatus(pseudoCityOrOfficeMaintenanceVM);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PseudoCityOrOfficeMaintenance.mvc/Delete/" + pseudoCityOrOfficeMaintenance.PseudoCityOrOfficeMaintenanceId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("ListUnDeleted");
		}

		// GET: /UnDelete
		[HttpGet]
		public ActionResult UnDelete(int id)
		{
			PseudoCityOrOfficeMaintenance pseudoCityOrOfficeMaintenance = new PseudoCityOrOfficeMaintenance();
			pseudoCityOrOfficeMaintenance = pseudoCityOrOfficeMaintenanceRepository.GetPseudoCityOrOfficeMaintenance(id);

			//Check Exists
			if (pseudoCityOrOfficeMaintenance == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			//Check AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			PseudoCityOrOfficeMaintenanceVM pseudoCityOrOfficeMaintenanceVM = new PseudoCityOrOfficeMaintenanceVM();

			pseudoCityOrOfficeMaintenanceRepository.EditForDisplay(pseudoCityOrOfficeMaintenance);

			pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance = pseudoCityOrOfficeMaintenance;

			return View(pseudoCityOrOfficeMaintenanceVM);
		}

		// POST: /UnDelete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UnDelete(PseudoCityOrOfficeMaintenanceVM pseudoCityOrOfficeMaintenanceVM, FormCollection collection)
		{
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Get Item From Database
			PseudoCityOrOfficeMaintenance pseudoCityOrOfficeMaintenance = new PseudoCityOrOfficeMaintenance();
			pseudoCityOrOfficeMaintenance = pseudoCityOrOfficeMaintenanceRepository.GetPseudoCityOrOfficeMaintenance(pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeMaintenanceId);

			//Check Exists
			if (pseudoCityOrOfficeMaintenance == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//UnDelete Item
			try
			{
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.DeletedFlag = false;
				pseudoCityOrOfficeMaintenanceRepository.UpdateDeletedStatus(pseudoCityOrOfficeMaintenanceVM);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PseudoCityOrOfficeMaintenance.mvc/Delete/" + pseudoCityOrOfficeMaintenance.PseudoCityOrOfficeMaintenanceId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("ListDeleted");
		}

		// GET: /Export
		public ActionResult Export()
		{
			//Get CSV Data
			byte[] csvData = pseudoCityOrOfficeMaintenanceRepository.Export();
			return File(csvData, "text/csv", "Pseudo City/Office ID Maintenance Export.csv");
		}

		//Get PseudoCityOrOfficeDefinedRegions By GlobalRegionCode
		[HttpPost]
		public JsonResult GetPseudoCityOrOfficeDefinedRegionsByGlobalRegionCode(string globalRegionCode)
		{
			if (globalRegionCode == "")
			{
				return Json(false);
			}
			var result = pseudoCityOrOfficeMaintenanceRepository.GetPseudoCityOrOfficeDefinedRegionsByGlobalRegionCode(globalRegionCode);
			return Json(result);
		}
	}
}