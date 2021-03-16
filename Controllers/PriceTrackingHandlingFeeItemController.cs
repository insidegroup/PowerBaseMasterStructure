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
    public class PriceTrackingHandlingFeeItemController : Controller
    {
        //main repository
		PriceTrackingHandlingFeeItemRepository priceTrackingHandlingFeeItemRepository = new PriceTrackingHandlingFeeItemRepository();
		PriceTrackingHandlingFeeGroupRepository priceTrackingHandlingFeeGroupRepository = new PriceTrackingHandlingFeeGroupRepository();
		
		HierarchyRepository hierarchyRepository = new HierarchyRepository();

		private string groupName = "Price Tracking Administrator";

        // GET: /List
        public ActionResult List(int id, string filter, int? page, string sortField, int? sortOrder)
        {
             RolesRepository rolesRepository = new RolesRepository();
			
			//Set Access Rights
            ViewData["Access"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName) && rolesRepository.HasWriteAccessToPriceTrackingHandlingFeeGroup(id))
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

			if (priceTrackingHandlingFeeItemRepository == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("Error");
			}

			PriceTrackingHandlingFeeGroup priceTrackingHandlingFeeGroup = new PriceTrackingHandlingFeeGroup();
			priceTrackingHandlingFeeGroup = priceTrackingHandlingFeeGroupRepository.GetGroup(id);
			if (priceTrackingHandlingFeeGroup != null)
			{
				ViewData["PriceTrackingHandlingFeeGroupId"] = priceTrackingHandlingFeeGroup.PriceTrackingHandlingFeeGroupId; 
				ViewData["PriceTrackingHandlingFeeGroupName"] = priceTrackingHandlingFeeGroup.PriceTrackingHandlingFeeGroupName;
			}

			var cwtPaginatedList = priceTrackingHandlingFeeItemRepository.PagePriceTrackingHandlingFeeItems(id, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

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
			PriceTrackingHandlingFeeItem priceTrackingHandlingFeeItem = new PriceTrackingHandlingFeeItem();
			priceTrackingHandlingFeeItem = priceTrackingHandlingFeeItemRepository.PriceTrackingHandlingFeeItem(id);

			//Check Exists
			if (priceTrackingHandlingFeeItem == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			priceTrackingHandlingFeeItemRepository.EditForDisplay(priceTrackingHandlingFeeItem);

			PriceTrackingHandlingFeeItemVM priceTrackingHandlingFeeItemVM = new PriceTrackingHandlingFeeItemVM();
			priceTrackingHandlingFeeItemVM.PriceTrackingHandlingFeeItem = priceTrackingHandlingFeeItem;

			PriceTrackingHandlingFeeGroup priceTrackingHandlingFeeGroup = new PriceTrackingHandlingFeeGroup();
			priceTrackingHandlingFeeGroup = priceTrackingHandlingFeeGroupRepository.GetGroup(priceTrackingHandlingFeeItem.PriceTrackingHandlingFeeGroupId);
			if (priceTrackingHandlingFeeGroup != null)
			{
				priceTrackingHandlingFeeItem.PriceTrackingHandlingFeeGroupId = priceTrackingHandlingFeeGroup.PriceTrackingHandlingFeeGroupId;
				priceTrackingHandlingFeeItemVM.PriceTrackingHandlingFeeItem = priceTrackingHandlingFeeItem;
				ViewData["PriceTrackingHandlingFeeGroupId"] = priceTrackingHandlingFeeGroup.PriceTrackingHandlingFeeGroupId;
				ViewData["PriceTrackingHandlingFeeGroupName"] = priceTrackingHandlingFeeGroup.PriceTrackingHandlingFeeGroupName;
			}

			return View(priceTrackingHandlingFeeItemVM);
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

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToPriceTrackingHandlingFeeGroup(id))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			} 
			
			PriceTrackingHandlingFeeItemVM priceTrackingHandlingFeeItemVM = new PriceTrackingHandlingFeeItemVM();

			PriceTrackingHandlingFeeGroup priceTrackingHandlingFeeGroup = new PriceTrackingHandlingFeeGroup();
			priceTrackingHandlingFeeGroup = priceTrackingHandlingFeeGroupRepository.GetGroup(id);
			if (priceTrackingHandlingFeeGroup != null)
			{
				PriceTrackingHandlingFeeItem priceTrackingHandlingFeeItem = new PriceTrackingHandlingFeeItem();
				priceTrackingHandlingFeeItem.PriceTrackingHandlingFeeGroupId = priceTrackingHandlingFeeGroup.PriceTrackingHandlingFeeGroupId;
				priceTrackingHandlingFeeItemVM.PriceTrackingHandlingFeeItem = priceTrackingHandlingFeeItem;
				ViewData["PriceTrackingHandlingFeeGroupId"] = priceTrackingHandlingFeeGroup.PriceTrackingHandlingFeeGroupId;
				ViewData["PriceTrackingHandlingFeeGroupName"] = priceTrackingHandlingFeeGroup.PriceTrackingHandlingFeeGroupName;
			}

			PriceTrackingSystemRepository priceTrackingSystemRepository = new PriceTrackingSystemRepository();
			SelectList priceTrackingSystems = new SelectList(priceTrackingSystemRepository.GetAllPriceTrackingSystems().ToList(), "PriceTrackingSystemId", "PriceTrackingSystemName");
			ViewData["PriceTrackingSystemList"] = priceTrackingSystems;

			ProductRepository productRepository = new ProductRepository();
			SelectList products = new SelectList(productRepository.GetPriceTrackerEligibleProducts().ToList(), "ProductId", "ProductName");
			ViewData["ProductList"] = products;

			return View(priceTrackingHandlingFeeItemVM);
		}

		// POST: /Create/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(PriceTrackingHandlingFeeItemVM priceTrackingHandlingFeeItemVM)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel(priceTrackingHandlingFeeItemVM);
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
				priceTrackingHandlingFeeItemRepository.Add(priceTrackingHandlingFeeItemVM.PriceTrackingHandlingFeeItem);
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
			return RedirectToAction("List", new { id = priceTrackingHandlingFeeItemVM.PriceTrackingHandlingFeeItem.PriceTrackingHandlingFeeGroupId});
		}

		// GET: /Edit
		public ActionResult Edit(int id)
		{
			//Get Item From Database
			PriceTrackingHandlingFeeItem priceTrackingHandlingFeeItem = new PriceTrackingHandlingFeeItem();
			priceTrackingHandlingFeeItem = priceTrackingHandlingFeeItemRepository.PriceTrackingHandlingFeeItem(id);

			//Check Exists
			if (priceTrackingHandlingFeeItem == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToPriceTrackingHandlingFeeGroup(priceTrackingHandlingFeeItem.PriceTrackingHandlingFeeGroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			PriceTrackingHandlingFeeItemVM priceTrackingHandlingFeeItemVM = new PriceTrackingHandlingFeeItemVM();
			priceTrackingHandlingFeeItemRepository.EditForDisplay(priceTrackingHandlingFeeItem);

			PriceTrackingHandlingFeeGroup priceTrackingHandlingFeeGroup = new PriceTrackingHandlingFeeGroup();
			priceTrackingHandlingFeeGroup = priceTrackingHandlingFeeGroupRepository.GetGroup(priceTrackingHandlingFeeItem.PriceTrackingHandlingFeeGroupId);
			if (priceTrackingHandlingFeeGroup != null)
			{
				priceTrackingHandlingFeeItem.PriceTrackingHandlingFeeGroupId = priceTrackingHandlingFeeGroup.PriceTrackingHandlingFeeGroupId;
				priceTrackingHandlingFeeItemVM.PriceTrackingHandlingFeeItem = priceTrackingHandlingFeeItem;
				ViewData["PriceTrackingHandlingFeeGroupId"] = priceTrackingHandlingFeeGroup.PriceTrackingHandlingFeeGroupId;
				ViewData["PriceTrackingHandlingFeeGroupName"] = priceTrackingHandlingFeeGroup.PriceTrackingHandlingFeeGroupName;
			}

			PriceTrackingSystemRepository priceTrackingSystemRepository = new PriceTrackingSystemRepository();
			SelectList priceTrackingSystems = new SelectList(priceTrackingSystemRepository.GetAllPriceTrackingSystems().ToList(), "PriceTrackingSystemId", "PriceTrackingSystemName", priceTrackingHandlingFeeItem.PriceTrackingSystemId);
			ViewData["PriceTrackingSystemList"] = priceTrackingSystems;

			ProductRepository productRepository = new ProductRepository();
			SelectList products = new SelectList(productRepository.GetPriceTrackerEligibleProducts().ToList(), "ProductId", "ProductName", priceTrackingHandlingFeeItem.ProductId);
			ViewData["ProductList"] = products;


			return View(priceTrackingHandlingFeeItemVM);
		}

		// POST: /Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(PriceTrackingHandlingFeeItemVM priceTrackingHandlingFeeItemVM)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel(priceTrackingHandlingFeeItemVM);
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
				priceTrackingHandlingFeeItemRepository.Update(priceTrackingHandlingFeeItemVM.PriceTrackingHandlingFeeItem);
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
			return RedirectToAction("List", new { id = priceTrackingHandlingFeeItemVM.PriceTrackingHandlingFeeItem.PriceTrackingHandlingFeeGroupId });
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
		{
			PriceTrackingHandlingFeeItem priceTrackingHandlingFeeItem = new PriceTrackingHandlingFeeItem();
			priceTrackingHandlingFeeItem = priceTrackingHandlingFeeItemRepository.PriceTrackingHandlingFeeItem(id);

			//Check Exists
			if (priceTrackingHandlingFeeItem == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			priceTrackingHandlingFeeItemRepository.EditForDisplay(priceTrackingHandlingFeeItem);

			PriceTrackingHandlingFeeItemVM priceTrackingHandlingFeeItemVM = new PriceTrackingHandlingFeeItemVM();
			priceTrackingHandlingFeeItemVM.PriceTrackingHandlingFeeItem = priceTrackingHandlingFeeItem;

			PriceTrackingHandlingFeeGroup priceTrackingHandlingFeeGroup = new PriceTrackingHandlingFeeGroup();
			priceTrackingHandlingFeeGroup = priceTrackingHandlingFeeGroupRepository.GetGroup(priceTrackingHandlingFeeItem.PriceTrackingHandlingFeeGroupId);
			if (priceTrackingHandlingFeeGroup != null)
			{
				priceTrackingHandlingFeeItem.PriceTrackingHandlingFeeGroupId = priceTrackingHandlingFeeGroup.PriceTrackingHandlingFeeGroupId;
				priceTrackingHandlingFeeItemVM.PriceTrackingHandlingFeeItem = priceTrackingHandlingFeeItem;
				ViewData["PriceTrackingHandlingFeeGroupId"] = priceTrackingHandlingFeeGroup.PriceTrackingHandlingFeeGroupId;
				ViewData["PriceTrackingHandlingFeeGroupName"] = priceTrackingHandlingFeeGroup.PriceTrackingHandlingFeeGroupName;
			}

			return View(priceTrackingHandlingFeeItemVM);
		}

		// POST: /Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(PriceTrackingHandlingFeeItemVM priceTrackingHandlingFeeItemVM)
		{
			//Get Item From Database
			PriceTrackingHandlingFeeItem priceTrackingHandlingFeeItem = new PriceTrackingHandlingFeeItem();
			priceTrackingHandlingFeeItem = priceTrackingHandlingFeeItemRepository.PriceTrackingHandlingFeeItem(priceTrackingHandlingFeeItemVM.PriceTrackingHandlingFeeItem.PriceTrackingHandlingFeeItemId);

			//Check Exists
			if (priceTrackingHandlingFeeItem == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToPriceTrackingHandlingFeeGroup(priceTrackingHandlingFeeItemVM.PriceTrackingHandlingFeeItem.PriceTrackingHandlingFeeGroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Delete Item
			try
			{
				priceTrackingHandlingFeeItemRepository.Delete(priceTrackingHandlingFeeItem);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PriceTrackingHandlingFeeItem.mvc/Delete/" + priceTrackingHandlingFeeItem.PriceTrackingHandlingFeeItemId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List", new { id = priceTrackingHandlingFeeItemVM.PriceTrackingHandlingFeeItem.PriceTrackingHandlingFeeGroupId });
		}
    }
}
