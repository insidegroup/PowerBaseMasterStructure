using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Controllers
{
    public class SupplierController : Controller
    {
		SupplierRepository supplierRepository = new SupplierRepository();
		ProductRepository productRepository = new ProductRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();

		private string groupName = "System Data Administrator";

		//GET: List
		[HttpGet]
		public ActionResult List(string filter, int? page, string sortField, int? sortOrder)
		{

			//Set Access Rights
			ViewData["Access"] = "";
			RolesRepository rolesRepository = new RolesRepository();
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//SortField + SortOrder settings
			if (string.IsNullOrEmpty(sortField))
			{
				sortField = "Default";
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
			}

			var items = supplierRepository.PageSuppliers(page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
			return View(items);
		}

		// GET: /Create
		[HttpGet]
		public ActionResult Create()
		{
			//Create Item 
			Supplier supplier = new Supplier();

			//AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Products 
			SelectList productList = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
			ViewData["ProductList"] = productList;

			return View(supplier);
		}

		// POST: //Create/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Supplier supplier)
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
				UpdateModel(supplier);
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
				supplierRepository.AddSupplier(supplier);
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
		public ActionResult Edit(string id, int productId)
		{
			//Get Item 
			Supplier supplier = new Supplier();
			supplier = supplierRepository.GetSupplier(id, productId);

			//Check Exists
			if (supplier == null)
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

			//Products 
			SelectList productList = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName", supplier.ProductId);
			ViewData["ProductList"] = productList;

			supplierRepository.EditItemForDisplay(supplier);

			return View(supplier);
		}

		// POST: //Edit/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Supplier supplier)
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
				UpdateModel(supplier);
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
				supplierRepository.UpdateSupplier(supplier);
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
		public ActionResult Delete(string id, int productId)
		{
			SupplierVM supplierVM = new SupplierVM();
			supplierVM.AllowDelete = true;

			Supplier supplier = new Supplier();
			supplier = supplierRepository.GetSupplier(id, productId);

			//Check Exists
			if (supplier == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			supplierVM.Supplier = supplier;

			//Attached Items
			List<SupplierReference> supplierReferences = supplierRepository.GetSupplierReferences(supplier.SupplierCode, supplier.ProductId);
			if (supplierReferences.Count > 0)
			{
				supplierVM.AllowDelete = false;
				supplierVM.SupplierReferences = supplierReferences;
			}

			supplierRepository.EditItemForDisplay(supplier);

			return View(supplierVM);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(SupplierVM supplierVM)
		{
			//Get Item 
			Supplier supplier = new Supplier();
			supplier = supplierRepository.GetSupplier(supplierVM.Supplier.SupplierCode, supplierVM.Supplier.ProductId);

			//Check Exists
			if (supplierVM.Supplier == null)
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
				supplierRepository.DeleteSupplier(supplier);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/Supplier.mvc/Delete/" + supplier.SupplierCode;
					return View("VersionError");
				}

				//Generic Error
				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			//Return
			return RedirectToAction("List");
		}
		
		// POST: AutoComplete Suppliers
        [HttpPost]
        public JsonResult AutoCompleteSuppliers(string searchText, int maxResults)
        {
            SupplierRepository supplierRepository = new SupplierRepository();
            var result = supplierRepository.LookUpSuppliers(searchText, maxResults);
            return Json(result);
        }

        // POST: AutoComplete Products of a Supplier
        [HttpPost]
        public JsonResult AutoCompleteSupplierProducts(string searchText, string supplierCode, int maxResults)
        {
            SupplierRepository supplierRepository = new SupplierRepository();
            var result = supplierRepository.LookUpSupplierProducts(searchText, supplierCode, maxResults);
            return Json(result);
        }

        // POST: AutoComplete Suppliers
        [HttpPost]
        public JsonResult IsValidSupplier(string supplierName)
        {
            SupplierRepository supplierRepository = new SupplierRepository();
            var result = supplierRepository.GetSupplierByName(supplierName);
            return Json(result);
        }

    }
}
