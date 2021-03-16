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
    public class CommissionableRouteItemController : Controller
    {
        //main repository
		CommissionableRouteItemRepository commissionableRouteItemRepository = new CommissionableRouteItemRepository();
		CommissionableRouteGroupRepository commissionableRouteGroupRepository = new CommissionableRouteGroupRepository();
		PolicyRoutingRepository policyRoutingRepository = new PolicyRoutingRepository();
		
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
	
		private string groupName = "Commissionable Route";

        // GET: /List
        public ActionResult List(int id, string filter, int? page, string sortField, int? sortOrder)
        {
             RolesRepository rolesRepository = new RolesRepository();
			
			//Set Access Rights
            ViewData["Access"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName) && rolesRepository.HasWriteAccessToCommissionableRouteGroup(id))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //SortField
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "ProductName";
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

			if (commissionableRouteItemRepository == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("Error");
			}

			CommissionableRouteGroup commissionableRouteGroup = new CommissionableRouteGroup();
			commissionableRouteGroup = commissionableRouteGroupRepository.GetGroup(id);
			if (commissionableRouteGroup != null)
			{
				ViewData["CommissionableRouteGroupId"] = commissionableRouteGroup.CommissionableRouteGroupId; 
				ViewData["CommissionableRouteGroupName"] = commissionableRouteGroup.CommissionableRouteGroupName;
			}

			var cwtPaginatedList = commissionableRouteItemRepository.PageCommissionableRouteItems(id, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

			if (cwtPaginatedList == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("Error");
			}
			
			//return items
            return View(cwtPaginatedList);  
        }

		// GET: /View
		public ActionResult View(int id)
		{
			CommissionableRouteItem commissionableRouteItem = new CommissionableRouteItem();
			commissionableRouteItem = commissionableRouteItemRepository.CommissionableRouteItem(id);

			//Check Exists
			if (commissionableRouteItem == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			CommissionableRouteItemVM commissionableRouteItemVM = new CommissionableRouteItemVM();
			commissionableRouteItemVM.CommissionableRouteItem = commissionableRouteItem;

			//Product
			ProductRepository productRepository = new ProductRepository();
			Product product = productRepository.GetProduct(commissionableRouteItem.ProductId);
			if (product != null)
			{
				commissionableRouteItemVM.Product = product;
			}

			//Supplier
			SupplierRepository supplierRepository = new SupplierRepository();
			Supplier supplier = supplierRepository.GetSupplier(commissionableRouteItem.SupplierCode, commissionableRouteItem.ProductId);
			if (supplier != null)
			{
				commissionableRouteItemVM.Supplier = supplier;
			}

			//Policy Routing
			PolicyRouting policyRouting = new PolicyRouting();
			if (commissionableRouteItem.PolicyRoutingId != null)
			{
				int policyRoutingId = (int)commissionableRouteItem.PolicyRoutingId;
				policyRouting = policyRoutingRepository.GetPolicyRouting(policyRoutingId);
			}
			commissionableRouteItemVM.PolicyRouting = policyRouting;

			CommissionableRouteGroup commissionableRouteGroup = new CommissionableRouteGroup();
			commissionableRouteGroup = commissionableRouteGroupRepository.GetGroup(commissionableRouteItem.CommissionableRouteGroupId);
			if (commissionableRouteGroup != null)
			{
				commissionableRouteItem.CommissionableRouteGroupId = commissionableRouteGroup.CommissionableRouteGroupId;
				commissionableRouteItemVM.CommissionableRouteItem = commissionableRouteItem;
				ViewData["CommissionableRouteGroupId"] = commissionableRouteGroup.CommissionableRouteGroupId;
				ViewData["CommissionableRouteGroupName"] = commissionableRouteGroup.CommissionableRouteGroupName;
			}

			return View(commissionableRouteItemVM);
		}

		// GET: /Create
		public ActionResult Create(int id)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Populate List of Products
			ProductRepository productRepository = new ProductRepository();
			SelectList products = new SelectList(productRepository.GetCommissionableRouteItemProducts().ToList(), "ProductId", "ProductName");
			ViewData["ProductList"] = products;

			//Populate List of Travel Indicators
			TravelIndicatorRepository TravelIndicatorRepository = new TravelIndicatorRepository();
			SelectList travelIndicators = new SelectList(TravelIndicatorRepository.GetAllTravelIndicators().ToList(), "TravelIndicator1", "TravelIndicatorDescription");
			ViewData["TravelIndicatorList"] = travelIndicators;

			//Populate List of CommissionAmountCurrencyCodes
			CurrencyRepository currencyRepository = new CurrencyRepository();
			SelectList commissionAmountCurrencyCodes = new SelectList(currencyRepository.GetAllCurrencies().ToList(), "CurrencyCode", "Name");
			ViewData["CommissionAmountCurrencyCodeList"] = commissionAmountCurrencyCodes;

			PolicyRouting policyRouting = new PolicyRouting();
			policyRouting.FromGlobalFlag = false;
			policyRouting.ToGlobalFlag = false;

			CommissionableRouteItemVM commissionableRouteItemVM = new CommissionableRouteItemVM();
			commissionableRouteItemVM.PolicyRouting = policyRouting;

			CommissionableRouteGroup commissionableRouteGroup = new CommissionableRouteGroup();
			commissionableRouteGroup = commissionableRouteGroupRepository.GetGroup(id);
			if (commissionableRouteGroup != null)
			{
				CommissionableRouteItem commissionableRouteItem = new CommissionableRouteItem();
				commissionableRouteItem.CommissionableRouteGroupId = commissionableRouteGroup.CommissionableRouteGroupId;
				commissionableRouteItemVM.CommissionableRouteItem = commissionableRouteItem;
				ViewData["CommissionableRouteGroupId"] = commissionableRouteGroup.CommissionableRouteGroupId;
				ViewData["CommissionableRouteGroupName"] = commissionableRouteGroup.CommissionableRouteGroupName;
			}

			return View(commissionableRouteItemVM);
		}

		// POST: /Create/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(CommissionableRouteItemVM commissionableRouteItemVM)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToCommissionableRouteGroup(commissionableRouteItemVM.CommissionableRouteItem.CommissionableRouteGroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel(commissionableRouteItemVM);
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

			//Get PolicyRouting Info
			PolicyRouting policyRouting = new PolicyRouting();
			policyRouting = commissionableRouteItemVM.PolicyRouting;

			//Edit Routing
			policyRoutingRepository.EditPolicyRouting(policyRouting);

			//Database Update
			try
			{
				commissionableRouteItemRepository.Add(commissionableRouteItemVM.CommissionableRouteItem, policyRouting);
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
			return RedirectToAction("List", new { id = commissionableRouteItemVM.CommissionableRouteItem.CommissionableRouteGroupId});
		}

		// GET: /Edit
		public ActionResult Edit(int id)
		{
			//Get Item From Database
			CommissionableRouteItem commissionableRouteItem = new CommissionableRouteItem();
			commissionableRouteItem = commissionableRouteItemRepository.CommissionableRouteItem(id);

			//Check Exists
			if (commissionableRouteItem == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToCommissionableRouteGroup(commissionableRouteItem.CommissionableRouteGroup.CommissionableRouteGroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Populate List of Products
			ProductRepository productRepository = new ProductRepository();
			SelectList products = new SelectList(productRepository.GetCommissionableRouteItemProducts().ToList(), "ProductId", "ProductName");
			ViewData["ProductList"] = products;

			//Populate List of Travel Indicators
			TravelIndicatorRepository TravelIndicatorRepository = new TravelIndicatorRepository();
			SelectList travelIndicators = new SelectList(TravelIndicatorRepository.GetAllTravelIndicators().ToList(), "TravelIndicator1", "TravelIndicatorDescription");
			ViewData["TravelIndicatorList"] = travelIndicators;

			//Populate List of CommissionAmountCurrencyCodes
			CurrencyRepository currencyRepository = new CurrencyRepository();
			SelectList commissionAmountCurrencyCodes = new SelectList(currencyRepository.GetAllCurrencies().ToList(), "CurrencyCode", "Name", commissionableRouteItem.CommissionAmountCurrencyCode);
			ViewData["CommissionAmountCurrencyCodeList"] = commissionAmountCurrencyCodes;

			CommissionableRouteItemVM commissionableRouteItemVM = new CommissionableRouteItemVM();
			commissionableRouteItemRepository.EditForDisplay(commissionableRouteItem);

			//Policy Routing
			PolicyRouting policyRouting = new PolicyRouting();
			if (commissionableRouteItem.PolicyRoutingId != null)
			{
				int policyRoutingId = (int)commissionableRouteItem.PolicyRoutingId;
				policyRouting = policyRoutingRepository.GetPolicyRouting(policyRoutingId);
			}
			commissionableRouteItemVM.PolicyRouting = policyRouting; 

			CommissionableRouteGroup commissionableRouteGroup = new CommissionableRouteGroup();
			commissionableRouteGroup = commissionableRouteGroupRepository.GetGroup(commissionableRouteItem.CommissionableRouteGroupId);
			if (commissionableRouteGroup != null)
			{
				commissionableRouteItem.CommissionableRouteGroupId = commissionableRouteGroup.CommissionableRouteGroupId;
				commissionableRouteItemVM.CommissionableRouteItem = commissionableRouteItem;
				ViewData["CommissionableRouteGroupId"] = commissionableRouteGroup.CommissionableRouteGroupId;
				ViewData["CommissionableRouteGroupName"] = commissionableRouteGroup.CommissionableRouteGroupName;
			}

			return View(commissionableRouteItemVM);
		}

		// POST: /Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(CommissionableRouteItemVM commissionableRouteItemVM)
		{
			//Get Item From Database
			CommissionableRouteItem commissionableRouteItem = new CommissionableRouteItem();
			commissionableRouteItem = commissionableRouteItemRepository.CommissionableRouteItem(commissionableRouteItemVM.CommissionableRouteItem.CommissionableRouteItemId);

			//Check Exists
			if (commissionableRouteItem == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToCommissionableRouteGroup(commissionableRouteItemVM.CommissionableRouteItem.CommissionableRouteGroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel(commissionableRouteItemVM);
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

			//Get PolicyRouting Info
			PolicyRouting policyRouting = new PolicyRouting();
			policyRouting = commissionableRouteItemVM.PolicyRouting;

			//Edit Routing
			policyRoutingRepository.EditPolicyRouting(policyRouting);

			//Database Update
			try
			{
				commissionableRouteItemRepository.Update(commissionableRouteItemVM.CommissionableRouteItem, policyRouting);
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
			return RedirectToAction("List", new { id = commissionableRouteItemVM.CommissionableRouteItem.CommissionableRouteGroupId });
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
		{
			CommissionableRouteItem commissionableRouteItem = new CommissionableRouteItem();
			commissionableRouteItem = commissionableRouteItemRepository.CommissionableRouteItem(id);

			//Check Exists
			if (commissionableRouteItem == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToCommissionableRouteGroup(commissionableRouteItem.CommissionableRouteGroup.CommissionableRouteGroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			} 
			
			CommissionableRouteItemVM commissionableRouteItemVM = new CommissionableRouteItemVM();
			commissionableRouteItemVM.CommissionableRouteItem = commissionableRouteItem;

			//Product
			ProductRepository productRepository = new ProductRepository();
			Product product = productRepository.GetProduct(commissionableRouteItem.ProductId);
			if (product != null)
			{
				commissionableRouteItemVM.Product = product;
			}

			//Supplier
			SupplierRepository supplierRepository = new SupplierRepository();
			Supplier supplier = supplierRepository.GetSupplier(commissionableRouteItem.SupplierCode, commissionableRouteItem.ProductId);
			if (supplier != null)
			{
				commissionableRouteItemVM.Supplier = supplier;
			}

			//Policy Routing
			PolicyRouting policyRouting = new PolicyRouting();
			if (commissionableRouteItem.PolicyRoutingId != null)
			{
				int policyRoutingId = (int)commissionableRouteItem.PolicyRoutingId;
				policyRouting = policyRoutingRepository.GetPolicyRouting(policyRoutingId);
			}
			commissionableRouteItemVM.PolicyRouting = policyRouting;

			CommissionableRouteGroup commissionableRouteGroup = new CommissionableRouteGroup();
			commissionableRouteGroup = commissionableRouteGroupRepository.GetGroup(commissionableRouteItem.CommissionableRouteGroupId);
			if (commissionableRouteGroup != null)
			{
				commissionableRouteItem.CommissionableRouteGroupId = commissionableRouteGroup.CommissionableRouteGroupId;
				commissionableRouteItemVM.CommissionableRouteItem = commissionableRouteItem;
				ViewData["CommissionableRouteGroupId"] = commissionableRouteGroup.CommissionableRouteGroupId;
				ViewData["CommissionableRouteGroupName"] = commissionableRouteGroup.CommissionableRouteGroupName;
			}

			return View(commissionableRouteItemVM);
		}

		// POST: /Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(CommissionableRouteItemVM commissionableRouteItemVM)
		{
			//Get Item From Database
			CommissionableRouteItem commissionableRouteItem = new CommissionableRouteItem();
			commissionableRouteItem = commissionableRouteItemRepository.CommissionableRouteItem(commissionableRouteItemVM.CommissionableRouteItem.CommissionableRouteItemId);

			//Check Exists
			if (commissionableRouteItem == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToCommissionableRouteGroup(commissionableRouteItemVM.CommissionableRouteItem.CommissionableRouteGroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Delete Item
			try
			{
				commissionableRouteItemRepository.Delete(commissionableRouteItem);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/CommissionableRouteItem.mvc/Delete/" + commissionableRouteItem.CommissionableRouteItemId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List", new { id = commissionableRouteItemVM.CommissionableRouteItem.CommissionableRouteGroupId });
		}
    }
}
