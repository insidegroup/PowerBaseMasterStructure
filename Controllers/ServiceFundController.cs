using ClientProfileServiceBusiness;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;

namespace CWTDesktopDatabase.Controllers
{
	public class ServiceFundController : Controller
	{
		// Main repository
		ServiceFundRepository serviceFundRepository = new ServiceFundRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		RolesRepository rolesRepository = new RolesRepository();

		private string groupName = "Service Funds Administrator";

		// GET: /ServiceFund/List
		public ActionResult List(string filter, int? page, string sortField, int? sortOrder)
		{
			//SortField
			if (string.IsNullOrEmpty(sortField))
			{
				sortField = "Default";
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

			ServiceFundsVM serviceFundsVM = new ServiceFundsVM();

			//Set Access Rights
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				serviceFundsVM.HasWriteAccess = true;
			}

			if (serviceFundRepository != null)
			{
				var serviceFunds = serviceFundRepository.PageServiceFunds(
					page ?? 1, 
					filter ?? "", 
					sortField, 
					sortOrder ?? 0);

				if (serviceFunds != null)
				{
					serviceFundsVM.ServiceFunds = serviceFunds;
				}
			}

			//return items
			return View(serviceFundsVM);
		}

		//
		// GET: /ServiceFund/Create
		public ActionResult Create()
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			ServiceFundVM serviceFundVM = new ServiceFundVM();

			ServiceFund serviceFund = new ServiceFund();
			serviceFundVM.ServiceFund = serviceFund;

			//GDSs
			GDSRepository gdsRepository = new GDSRepository();
			SelectList gdsList = new SelectList(gdsRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName");
			serviceFundVM.GDSs = gdsList;

			//Countries
			CountryRepository countryRepository = new CountryRepository();
			SelectList countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName");
			serviceFundVM.Countries = countries;

			//Fund Use Statuses
			FundUseStatusRepository fundUseStatusRepository = new FundUseStatusRepository();
			SelectList fundUseStatusList = new SelectList(fundUseStatusRepository.GetAllFundUseStatuses().ToList(), "Key", "Value");
			serviceFundVM.FundUseStatuses = fundUseStatusList;

			//TimeZoneRules
			TimeZoneRuleRepository timeZoneRuleRepository = new TimeZoneRuleRepository();
			SelectList timeZoneRules = new SelectList(timeZoneRuleRepository.GetAllTimeZoneRules().ToList(), "TimeZoneRuleCode", "TimeZoneRuleCodeDesc", "CST/CDT");
			serviceFundVM.TimeZoneRules = timeZoneRules;

			//Products
			ProductRepository productRepository = new ProductRepository();
			SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
			serviceFundVM.Products = products;

			//Suppliers
			SupplierRepository supplierRepository = new SupplierRepository();
			SelectList suppliers = new SelectList(supplierRepository.GetAllSuppliers().ToList(), "SupplierCode", "SupplierName");
			serviceFundVM.Suppliers = suppliers;

			//Currencies
			CurrencyRepository currencyRepository = new CurrencyRepository();
			SelectList currencies = new SelectList(currencyRepository.GetAllCurrencies().ToList(), "CurrencyCode", "Name");
			serviceFundVM.Currencies = currencies;

			//Service Fund Routings
			ServiceFundRoutingRepository serviceFundRoutingRepository = new ServiceFundRoutingRepository();
			SelectList serviceFundRoutingsList = new SelectList(serviceFundRoutingRepository.GetAllServiceFundRoutings().ToList(), "Key", "Value");
			serviceFundVM.ServiceFundRoutings = serviceFundRoutingsList;

			//ServiceFundChannelTypes
			ServiceFundChannelTypeRepository serviceFundChannelTypeRepository = new ServiceFundChannelTypeRepository();
			SelectList serviceFundChannelTypesList = new SelectList(serviceFundChannelTypeRepository.GetAllServiceFundChannelTypes().ToList(), "ServiceFundChannelTypeId", "ServiceFundChannelTypeName");
			serviceFundVM.ServiceFundChannelTypes = serviceFundChannelTypesList;

			return View(serviceFundVM);
		}

		// POST: /ServiceFund/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ServiceFundVM serviceFundVM, FormCollection formCollection)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//We need to extract group from groupVM
			ServiceFund serviceFund = new ServiceFund();
			serviceFund = serviceFundVM.ServiceFund;
			if (serviceFund == null)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			//Convert Times to DateTime for DB
			serviceFund.ServiceFundStartTime = CWTStringHelpers.BuildDateTime(serviceFund.ServiceFundStartTimeString);
			serviceFund.ServiceFundEndTime = CWTStringHelpers.BuildDateTime(serviceFund.ServiceFundEndTimeString);

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<ServiceFund>(serviceFund, "ServiceFund");
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
				serviceFundRepository.Add(serviceFund);
			}
			catch (SqlException ex)
			{
				//Non-Unique Name
				if (ex.Message == "NonUniqueName")
				{
					return View("NonUniqueNameError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			ViewData["NewSortOrder"] = 0;
			return RedirectToAction("List");
		}

		// GET: /ServiceFund/Edit
		public ActionResult Edit(int id)
		{
			//Get Item From Database
			ServiceFund serviceFund = new ServiceFund();
			serviceFund = serviceFundRepository.GetServiceFund(id);

			//Check Exists
			if (serviceFund == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			ServiceFundVM serviceFundVM = new ServiceFundVM();
			serviceFundVM.ServiceFund = serviceFund;

			//GDSs
			GDSRepository gdsRepository = new GDSRepository();
			SelectList gdsList = new SelectList(gdsRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName", serviceFund.GDSCode);
			serviceFundVM.GDSs = gdsList;

			//Countries
			CountryRepository countryRepository = new CountryRepository();
			SelectList countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName", serviceFund.PCCCountryCode);
			serviceFundVM.Countries = countries;

			//Fund Use Statuses
			FundUseStatusRepository fundUseStatusRepository = new FundUseStatusRepository();
			SelectList fundUseStatusList = new SelectList(fundUseStatusRepository.GetAllFundUseStatuses().ToList(), "Key", "Value", serviceFund.FundUseStatus);
			serviceFundVM.FundUseStatuses = fundUseStatusList;

			//TimeZoneRules
			TimeZoneRuleRepository timeZoneRuleRepository = new TimeZoneRuleRepository();
			SelectList timeZoneRules = new SelectList(timeZoneRuleRepository.GetAllTimeZoneRules().ToList(), "TimeZoneRuleCode", "TimeZoneRuleCodeDesc", serviceFund.TimeZoneRuleCode);
			serviceFundVM.TimeZoneRules = timeZoneRules;

			//Products
			ProductRepository productRepository = new ProductRepository();
			SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName", serviceFund.ProductId);
			serviceFundVM.Products = products;

			//Suppliers
			SupplierRepository supplierRepository = new SupplierRepository();
			SelectList suppliers = new SelectList(supplierRepository.GetAllSuppliers().ToList(), "SupplierCode", "SupplierName", serviceFund.SupplierName);
			serviceFundVM.Suppliers = suppliers;

			//Currencies
			CurrencyRepository currencyRepository = new CurrencyRepository();
			SelectList currencies = new SelectList(currencyRepository.GetAllCurrencies().ToList(), "CurrencyCode", "Name", serviceFund.ServiceFundCurrencyCode);
			serviceFundVM.Currencies = currencies;

			//Service Fund Routings
			ServiceFundRoutingRepository serviceFundRoutingRepository = new ServiceFundRoutingRepository();
			SelectList serviceFundRoutingsList = new SelectList(serviceFundRoutingRepository.GetAllServiceFundRoutings().ToList(), "Key", "Value", serviceFund.ServiceFundRouting);
			serviceFundVM.ServiceFundRoutings = serviceFundRoutingsList;

			//ServiceFundChannelTypes
			ServiceFundChannelTypeRepository serviceFundChannelTypeRepository = new ServiceFundChannelTypeRepository();
			SelectList serviceFundChannelTypesList = new SelectList(serviceFundChannelTypeRepository.GetAllServiceFundChannelTypes().ToList(), "ServiceFundChannelTypeId", "ServiceFundChannelTypeName", serviceFund.ServiceFundChannelTypeId);
			serviceFundVM.ServiceFundChannelTypes = serviceFundChannelTypesList;

			serviceFundRepository.EditGroupForDisplay(serviceFund);

			return View(serviceFundVM);
		}

		// POST: /ServiceFund/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(ServiceFundVM serviceFundVM, FormCollection formCollection)
		{
			//Get Item
			ServiceFund serviceFund = new ServiceFund();
			serviceFund = serviceFundRepository.GetServiceFund(serviceFundVM.ServiceFund.ServiceFundId);

			//Check Exists
			if (serviceFund == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Convert Times to DateTime for DB
			serviceFund.ServiceFundStartTime = CWTStringHelpers.BuildDateTime(serviceFundVM.ServiceFund.ServiceFundStartTimeString);
			serviceFund.ServiceFundEndTime = CWTStringHelpers.BuildDateTime(serviceFundVM.ServiceFund.ServiceFundEndTimeString);
			
			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<ServiceFund>(serviceFund, "ServiceFund");
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
				serviceFundRepository.Edit(serviceFund);
			}
			catch (SqlException ex)
			{
				//Non-Unique Name
				if (ex.Message == "NonUniqueName")
				{
					return View("NonUniqueNameError");
				}
				//Versioning Error
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/ServiceFund.mvc/Edit/" +serviceFund.ServiceFundId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List");
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
		{
			//Get Item From Database
			ServiceFund serviceFund = new ServiceFund();
			serviceFund = serviceFundRepository.GetServiceFund(id);

			//Check Exists
			if (serviceFund == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			ServiceFundVM serviceFundVM = new ServiceFundVM();
			serviceFundVM.ServiceFund = serviceFund;

			serviceFundRepository.EditGroupForDisplay(serviceFund);

			return View(serviceFundVM);
		}

		// POST: /ServiceFund/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(ServiceFundVM serviceFundVM)
		{
			//Check Valid Item passed in Form       
			if (serviceFundVM.ServiceFund == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Get Item From Database
			ServiceFund serviceFund = new ServiceFund();
			serviceFund = serviceFundRepository.GetServiceFund(serviceFundVM.ServiceFund.ServiceFundId);

			//Check Exists
			if (serviceFund == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Delete Form Item
			try
			{
				serviceFundRepository.Delete(serviceFundVM.ServiceFund);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/ServiceFund.mvc/Delete/" + serviceFundVM.ServiceFund.ServiceFundId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List");
		}

		// GET: /View
		[HttpGet]
		public ActionResult View(int id)
		{
			//Get Item From Database
			ServiceFund serviceFund = new ServiceFund();
			serviceFund = serviceFundRepository.GetServiceFund(id);

			//Check Exists
			if (serviceFund == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			ServiceFundVM serviceFundVM = new ServiceFundVM();
			serviceFundVM.ServiceFund = serviceFund;

			serviceFundRepository.EditGroupForDisplay(serviceFund);

			return View(serviceFundVM);
		}

		//Validation
		[HttpPost]
		public JsonResult IsAvailableServiceFund(string clientTopUnitGUID, string clientAccountNumber, int productId, string supplierCode, string serviceFundChannelTypeId, int? serviceFundId)
		{
			var result = serviceFundRepository.IsAvailableServiceFund(clientTopUnitGUID, clientAccountNumber, productId, supplierCode, serviceFundChannelTypeId, serviceFundId);
			return Json(result);
		}
	}
}
