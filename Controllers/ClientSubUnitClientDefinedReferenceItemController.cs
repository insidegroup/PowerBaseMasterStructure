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
using System.Xml;

namespace CWTDesktopDatabase.Controllers
{
	public class ClientSubUnitClientDefinedReferenceItemController : Controller
	{
		ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
		ClientSubUnitClientAccountRepository clientSubUnitClientAccountRepository = new ClientSubUnitClientAccountRepository();
		ClientDefinedReferenceItemRepository clientDefinedReferenceItemRepository = new ClientDefinedReferenceItemRepository();
		ClientSubUnitClientDefinedReferenceItemRepository clientSubUnitClientDefinedReferenceItemRepository = new ClientSubUnitClientDefinedReferenceItemRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		private string groupName = "Client Defined References";

		// GET: /ListBySubUnit/
		public ActionResult ListBySubUnit(string filter, int? page, string id, string sortField, int? sortOrder, string can, string ssc)
		{
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);

			//Check Exists
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "ListBySubUnitGet";
				return View("RecordDoesNotExistError");
			}

			//Set Access Rights 
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//Set Create/Order Rights 
			ViewData["CreateAccess"] = "";
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToClientSubUnitClientDefinedReferenceItem(id))
			{
				ViewData["CreateAccess"] = "CreateAccess";
			}

			//SortField
			if (string.IsNullOrEmpty(sortField))
			{
				sortField = "SequenceNumber";
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

			clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);
			ViewData["ClientTopUnitGuid"] = clientSubUnit.ClientTopUnitGuid;
			ViewData["ClientTopUnitName"] = clientSubUnit.ClientTopUnitName;
			ViewData["ClientSubUnitGuid"] = clientSubUnit.ClientSubUnitGuid;
			ViewData["ClientSubUnitName"] = clientSubUnit.ClientSubUnitName;

			var items = clientSubUnitClientDefinedReferenceItemRepository.PageClientSubUnitClientDefinedReferenceItems(filter ?? "", id, page ?? 1, sortField, sortOrder ?? 0);

			return View(items);
		}

		// GET: /Create
		public ActionResult Create(string id, string can, string ssc)
		{
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);

			//Check Exists
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "ListBySubUnitGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);
			
			ClientDefinedReferenceItemVM clientDefinedReferenceItemVM = new ClientDefinedReferenceItemVM();
			clientDefinedReferenceItemVM.ClientSubUnit = clientSubUnit;

			ClientDefinedReferenceItem clientDefinedReferenceItem = new ClientDefinedReferenceItem();
			clientDefinedReferenceItem.ClientSubUnitGuid = clientSubUnit.ClientSubUnitGuid;
			clientDefinedReferenceItemVM.ClientDefinedReferenceItem = clientDefinedReferenceItem;

			ViewData["ClientTopUnitGuid"] = clientSubUnit.ClientTopUnitGuid;
			ViewData["ClientTopUnitName"] = clientSubUnit.ClientTopUnitName;
			ViewData["ClientSubUnitGuid"] = clientSubUnit.ClientSubUnitGuid;
			ViewData["ClientSubUnitName"] = clientSubUnit.ClientSubUnitName;

			//Products
			ProductRepository productRepository = new ProductRepository();
			clientDefinedReferenceItemVM.Products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");

			//Contexts
			ContextRepository contextRepository = new ContextRepository();
			clientDefinedReferenceItemVM.Contexts = new SelectList(contextRepository.GetAllContexts().ToList(), "ContextId", "ContextName");

			return View(clientDefinedReferenceItemVM);
		}

		// POST: /Create/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ClientDefinedReferenceItemVM clientDefinedReferenceItemVM, FormCollection formCollection)
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
				UpdateModel(clientDefinedReferenceItemVM);
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

			//ClientDefinedReferenceItemProducts
			string clientDefinedReferenceItemProducts = formCollection["ClientDefinedReferenceItem.ClientDefinedReferenceItemProductIds"];
			if (!string.IsNullOrEmpty(clientDefinedReferenceItemProducts))
			{
				clientDefinedReferenceItemVM.ClientDefinedReferenceItem.ClientDefinedReferenceItemProductIds = clientDefinedReferenceItemProducts;
			}

			//ClientDefinedReferenceItemContexts
			string clientDefinedReferenceItemContexts = formCollection["ClientDefinedReferenceItem.ClientDefinedReferenceItemContextIds"];
			if (!string.IsNullOrEmpty(clientDefinedReferenceItemContexts))
			{
				clientDefinedReferenceItemVM.ClientDefinedReferenceItem.ClientDefinedReferenceItemContextIds = clientDefinedReferenceItemContexts;
			}

			//Database Update
			try
			{
				clientSubUnitClientDefinedReferenceItemRepository.Add(clientDefinedReferenceItemVM.ClientDefinedReferenceItem);
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

			return RedirectToAction("ListBySubUnit", new
			{
				id = clientDefinedReferenceItemVM.ClientDefinedReferenceItem.ClientSubUnitGuid
			});
		}

		// GET: /Edit
		public ActionResult Edit(string id, string csu)
		{
			//Check Exists
			ClientDefinedReferenceItem clientDefinedReferenceItem = new ClientDefinedReferenceItem();
			clientDefinedReferenceItem = clientDefinedReferenceItemRepository.GetClientDefinedReferenceItem(id);
			if (clientDefinedReferenceItem == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			ClientDefinedReferenceItemVM clientDefinedReferenceItemVM = new ClientDefinedReferenceItemVM();
			clientDefinedReferenceItemRepository.EditForDisplay(clientDefinedReferenceItem);
			clientDefinedReferenceItemVM.ClientDefinedReferenceItem = clientDefinedReferenceItem;

			//Check Exists CSU for VM
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
			if (clientSubUnit != null)
			{
				clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);
				clientDefinedReferenceItemVM.ClientSubUnit = clientSubUnit;
				ViewData["ClientTopUnitGuid"] = clientSubUnit.ClientTopUnitGuid;
				ViewData["ClientTopUnitName"] = clientSubUnit.ClientTopUnitName;
				ViewData["ClientSubUnitGuid"] = clientSubUnit.ClientSubUnitGuid;
				ViewData["ClientSubUnitName"] = clientSubUnit.ClientSubUnitName;
			}

			clientDefinedReferenceItemVM.Products = GetelectedProducts(clientDefinedReferenceItem);
			clientDefinedReferenceItemVM.Contexts = GetSelectedContexts(clientDefinedReferenceItem);
			
			return View(clientDefinedReferenceItemVM);
		}

		// POST: /Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(ClientDefinedReferenceItemVM clientDefinedReferenceItemVM, FormCollection formCollection)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//ClientDefinedReferenceItemProducts
			clientDefinedReferenceItemVM.ClientDefinedReferenceItem.ClientDefinedReferenceItemProductIds = formCollection["ProductList"];
			
			//ClientDefinedReferenceItemContexts
			clientDefinedReferenceItemVM.ClientDefinedReferenceItem.ClientDefinedReferenceItemContextIds = formCollection["ContextList"];
			
			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel(clientDefinedReferenceItemVM);
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
				clientSubUnitClientDefinedReferenceItemRepository.Update(clientDefinedReferenceItemVM.ClientDefinedReferenceItem);
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

			return RedirectToAction("ListBySubUnit", new
			{
				id = clientDefinedReferenceItemVM.ClientSubUnit.ClientSubUnitGuid
			});
		}

		// GET: /View/5
		public ActionResult View(string id, string csu)
		{
			//Check Exists
			ClientDefinedReferenceItem clientDefinedReferenceItem = new ClientDefinedReferenceItem();
			clientDefinedReferenceItem = clientDefinedReferenceItemRepository.GetClientDefinedReferenceItem(id);
			if (clientDefinedReferenceItem == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			ClientDefinedReferenceItemVM clientDefinedReferenceItemVM = new ClientDefinedReferenceItemVM();

			clientDefinedReferenceItemRepository.EditForDisplay(clientDefinedReferenceItem);
			clientDefinedReferenceItemVM.ClientDefinedReferenceItem = clientDefinedReferenceItem;

			//Check Exists CSU for VM
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
			if (clientSubUnit != null)
			{
				clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);
				clientDefinedReferenceItemVM.ClientSubUnit = clientSubUnit;
				ViewData["ClientTopUnitGuid"] = clientSubUnit.ClientTopUnitGuid;
				ViewData["ClientTopUnitName"] = clientSubUnit.ClientTopUnitName;
				ViewData["ClientSubUnitGuid"] = clientSubUnit.ClientSubUnitGuid;
				ViewData["ClientSubUnitName"] = clientSubUnit.ClientSubUnitName;
			}

			return View(clientDefinedReferenceItemVM);
		}

		// GET: /Delete/5
		[HttpGet]
		public ActionResult Delete(string id, string csu)
		{
			//Check Exists
			ClientDefinedReferenceItem clientDefinedReferenceItem = new ClientDefinedReferenceItem();
			clientDefinedReferenceItem = clientDefinedReferenceItemRepository.GetClientDefinedReferenceItem(id);
			if (clientDefinedReferenceItem == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			ClientDefinedReferenceItemVM clientDefinedReferenceItemVM = new ClientDefinedReferenceItemVM();
			clientDefinedReferenceItemRepository.EditForDisplay(clientDefinedReferenceItem);
			clientDefinedReferenceItemVM.ClientDefinedReferenceItem = clientDefinedReferenceItem;

			//Check Exists CSU for VM
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
			if (clientSubUnit != null)
			{
				clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);
				clientDefinedReferenceItemVM.ClientSubUnit = clientSubUnit;
				ViewData["ClientTopUnitGuid"] = clientSubUnit.ClientTopUnitGuid;
				ViewData["ClientTopUnitName"] = clientSubUnit.ClientTopUnitName;
				ViewData["ClientSubUnitGuid"] = clientSubUnit.ClientSubUnitGuid;
				ViewData["ClientSubUnitName"] = clientSubUnit.ClientSubUnitName;
			}

			return View(clientDefinedReferenceItemVM);
		}

		// POST: //Delete/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(string id, FormCollection formCollection)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Check Exists
			ClientDefinedReferenceItem clientDefinedReferenceItem = new ClientDefinedReferenceItem();
			clientDefinedReferenceItem = clientDefinedReferenceItemRepository.GetClientDefinedReferenceItem(id);
			if (clientDefinedReferenceItem == null)
			{
				ViewData["ActionMethod"] = "ViewBySubUnitGet";
				return View("RecordDoesNotExistError");
			}

			//Check Exists
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			if (clientDefinedReferenceItem.ClientSubUnitGuid != null)
			{
				clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientDefinedReferenceItem.ClientSubUnitGuid);

				if (clientSubUnit == null)
				{
					ViewData["ActionMethod"] = "ViewBySubUnitGet";
					return View("RecordDoesNotExistError");
				}
			}

			//Delete Item
			try
			{
				clientSubUnitClientDefinedReferenceItemRepository.Delete(clientDefinedReferenceItem);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/ClientDefinedReferenceItem.mvc/Delete/" + clientDefinedReferenceItem.ClientDefinedReferenceItemId.ToString();
					return View("VersionError");
				}
				//Restraint Error - go to standard DeleteError page
				if (ex.Message == "SQLDeleteError")
				{
					ViewData["ReturnURL"] = "/ClientDefinedReferenceItem.mvc/Delete/" + clientDefinedReferenceItem.ClientDefinedReferenceItemId.ToString();
					return View("DeleteError");
				}
				//Generic Error
				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			//Return
			return RedirectToAction("ListBySubUnit", new
			{
				id = formCollection["ClientSubUnit.ClientSubUnitGuid"]
			});
		}

		//Products
		private IEnumerable<SelectListItem> GetelectedProducts(ClientDefinedReferenceItem clientDefinedReferenceItem)
		{
			ProductRepository productRepository = new ProductRepository();
			IEnumerable<SelectListItem> defaultProducts = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
			List<SelectListItem> selectedProducts = new List<SelectListItem>();

			foreach (SelectListItem item in defaultProducts)
			{
				bool selected = false;

				if (clientDefinedReferenceItem.ClientDefinedReferenceItemProductIds != null && clientDefinedReferenceItem.ClientDefinedReferenceItemProductIds.Length > 0)
				{
					foreach (string productId in clientDefinedReferenceItem.ClientDefinedReferenceItemProductIds.Split(','))
					{
						if (item.Value == productId)
						{
							selected = true;
						}
					}
				}

				selectedProducts.Add(
					new SelectListItem()
					{
						Text = item.Text,
						Value = item.Value,
						Selected = selected
					}
				);
			}

			return selectedProducts;
		}

		//Contexts
		private IEnumerable<SelectListItem> GetSelectedContexts(ClientDefinedReferenceItem clientDefinedReferenceItem)
		{
			ContextRepository contextRepository = new ContextRepository();
			IEnumerable<SelectListItem> defaultContexts = new SelectList(contextRepository.GetAllContexts().ToList(), "ContextId", "ContextName");
			List<SelectListItem> selectedContexts = new List<SelectListItem>();

			foreach (SelectListItem item in defaultContexts)
			{
				bool selected = false;

				if (clientDefinedReferenceItem.ClientDefinedReferenceItemContextIds != null && clientDefinedReferenceItem.ClientDefinedReferenceItemContextIds.Length > 0)
				{
					foreach (string contextId in clientDefinedReferenceItem.ClientDefinedReferenceItemContextIds.Split(','))
					{
						if (item.Value == contextId)
						{
							selected = true;
						}
					}
				}

				selectedContexts.Add(
					new SelectListItem()
					{
						Text = item.Text,
						Value = item.Value,
						Selected = selected
					}
				);
			}

			return selectedContexts;
		}
	}
}