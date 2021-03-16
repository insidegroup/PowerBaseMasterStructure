using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Models;
using System.Data.SqlClient;
using System.IO;

namespace CWTDesktopDatabase.Controllers
{
	public class ClientSubUnitCDRItemController : Controller
	{
		RolesRepository rolesRepository = new RolesRepository();
		ClientSubUnitCDRItemRepository clientSubUnitCDRItemRepository = new ClientSubUnitCDRItemRepository();
		ClientSubUnitCDRRepository clientSubUnitCDRRepository = new ClientSubUnitCDRRepository();
		ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();

		public ActionResult List(int id, string csu, string filter, int? page, string sortField, int? sortOrder)
		{
			ClientSubUnitCDRItemsVM clientSubUnitCDRItemsVM = new ClientSubUnitCDRItemsVM();

			ClientSubUnitClientDefinedReference clientSubUnitClientDefinedReference = new ClientSubUnitClientDefinedReference();
			clientSubUnitClientDefinedReference = clientSubUnitCDRRepository.GetClientSubUnitCDR(id);

			//ClientSubUnitClientDefinedReference
			if (clientSubUnitClientDefinedReference == null)
			{
				ViewData["ActionMethod"] = "List";
				return View("RecordDoesNotExistError");
			}

			clientSubUnitCDRItemsVM.ClientSubUnitClientDefinedReference = clientSubUnitClientDefinedReference;

			//SortField
			if (string.IsNullOrEmpty(sortField))
			{
				sortField = "RelatedToValue";
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

			//ClientSubUnit
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "List";
				return View("RecordDoesNotExistError");
			}

			clientSubUnitCDRItemsVM.ClientSubUnit = clientSubUnit;
			clientSubUnitCDRItemsVM.HasWriteAccess = rolesRepository.HasWriteAccessToClientSubUnit(csu);

			//Return items
			clientSubUnitCDRItemsVM.ClientSubUnitCDRItems = clientSubUnitCDRItemRepository.PageClientSubUnitCDRItems(id, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

			return View(clientSubUnitCDRItemsVM);
		}

		// GET: /Create
		[HttpGet]
		public ActionResult Create(int id, string csu, string relatedToDisplayName)
		{
			//AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			ClientSubUnitCDRItemVM clientSubUnitCDRItemVM = new ClientSubUnitCDRItemVM();

			//ClientSubUnit
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "List";
				return View("RecordDoesNotExistError");
			}

			clientSubUnitCDRItemVM.ClientSubUnit = clientSubUnit;

			ClientSubUnitClientDefinedReferenceItem clientSubUnitClientDefinedReferenceItem = new ClientSubUnitClientDefinedReferenceItem();
			clientSubUnitClientDefinedReferenceItem.ClientSubUnitClientDefinedReferenceId = id;
			clientSubUnitCDRItemVM.ClientSubUnitClientDefinedReferenceItem = clientSubUnitClientDefinedReferenceItem;

			return View(clientSubUnitCDRItemVM);
		}

		// POST: //Create/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ClientSubUnitCDRItemVM clientSubUnitCDRItemVM)
		{
			//AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitCDRItemVM.ClientSubUnit.ClientSubUnitGuid))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update  Model from Form
			try
			{
				UpdateModel(clientSubUnitCDRItemVM.ClientSubUnitClientDefinedReferenceItem);
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
				clientSubUnitCDRItemRepository.Add(clientSubUnitCDRItemVM.ClientSubUnitClientDefinedReferenceItem);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction(
				"List", 
				new { 
					id = clientSubUnitCDRItemVM.ClientSubUnitClientDefinedReferenceItem.ClientSubUnitClientDefinedReferenceId,
					csu = clientSubUnitCDRItemVM.ClientSubUnitClientDefinedReferenceItem.ClientSubUnit.ClientSubUnitGuid,
					relatedToDisplayName = clientSubUnitCDRItemVM.ClientSubUnitClientDefinedReferenceItem.RelatedToDisplayName
				}
			);
		}

		// GET: /Edit
		public ActionResult Edit(int id, string csu)
		{
			//Get Item 
			ClientSubUnitClientDefinedReferenceItem clientSubUnitClientDefinedReferenceItem = new ClientSubUnitClientDefinedReferenceItem();
			clientSubUnitClientDefinedReferenceItem = clientSubUnitCDRItemRepository.GetClientSubUnitCDRItem(id);

			//Check Exists
			if (clientSubUnitClientDefinedReferenceItem == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			ClientSubUnitCDRItemVM clientSubUnitCDRItemVM = new ClientSubUnitCDRItemVM();
			clientSubUnitCDRItemVM.ClientSubUnitClientDefinedReferenceItem = clientSubUnitClientDefinedReferenceItem;

			//AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//ClientSubUnit
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "Edit";
				return View("RecordDoesNotExistError");
			}

			clientSubUnitCDRItemVM.ClientSubUnit = clientSubUnit;

			return View(clientSubUnitCDRItemVM);
		}

		// POST: //Edit/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(ClientSubUnitCDRItemVM clientSubUnitCDRItemVM)
		{
			//AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitCDRItemVM.ClientSubUnit.ClientSubUnitGuid))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update  Model from Form
			try
			{
				UpdateModel(clientSubUnitCDRItemVM.ClientSubUnitClientDefinedReferenceItem);
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
				clientSubUnitCDRItemRepository.Edit(clientSubUnitCDRItemVM.ClientSubUnitClientDefinedReferenceItem);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction(
				"List",
				new
				{
					id = clientSubUnitCDRItemVM.ClientSubUnitClientDefinedReferenceItem.ClientSubUnitClientDefinedReferenceId,
					csu = clientSubUnitCDRItemVM.ClientSubUnitClientDefinedReferenceItem.ClientSubUnit.ClientSubUnitGuid,
					relatedToDisplayName = clientSubUnitCDRItemVM.ClientSubUnitClientDefinedReferenceItem.RelatedToDisplayName
				}
			);
		}

		// GET: Delete
		[HttpGet]
		public ActionResult Delete(int id, string csu)
		{
			//Get Item 
			ClientSubUnitClientDefinedReferenceItem clientSubUnitClientDefinedReferenceItem = new ClientSubUnitClientDefinedReferenceItem();
			clientSubUnitClientDefinedReferenceItem = clientSubUnitCDRItemRepository.GetClientSubUnitCDRItem(id);

			//Check Exists
			if (clientSubUnitClientDefinedReferenceItem == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			ClientSubUnitCDRItemVM clientSubUnitCDRItemVM = new ClientSubUnitCDRItemVM();
			clientSubUnitCDRItemVM.ClientSubUnitClientDefinedReferenceItem = clientSubUnitClientDefinedReferenceItem;

			//AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//ClientSubUnit
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "Delete";
				return View("RecordDoesNotExistError");
			}

			clientSubUnitCDRItemVM.ClientSubUnit = clientSubUnit;

			return View(clientSubUnitCDRItemVM);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(ClientSubUnitCDRItemVM clientSubUnitCDRItemVM)
		{
			//AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitCDRItemVM.ClientSubUnit.ClientSubUnitGuid))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update  Model from Form
			try
			{
				UpdateModel(clientSubUnitCDRItemVM.ClientSubUnitClientDefinedReferenceItem);
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

			//Delete Item
			try
			{
				clientSubUnitCDRItemRepository.Delete(clientSubUnitCDRItemVM.ClientSubUnitClientDefinedReferenceItem);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/ClientSubUnitCDRItem.mvc/Delete/" + clientSubUnitCDRItemVM.ClientSubUnitClientDefinedReferenceItem.ClientSubUnitClientDefinedReferenceItemId;
					return View("VersionError");
				}

				//Generic Error
				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			//Return
			return RedirectToAction(
				"List",
				new
				{
					id = clientSubUnitCDRItemVM.ClientSubUnitClientDefinedReferenceItem.ClientSubUnitClientDefinedReferenceId,
					csu = clientSubUnitCDRItemVM.ClientSubUnitClientDefinedReferenceItem.ClientSubUnit.ClientSubUnitGuid,
					relatedToDisplayName = clientSubUnitCDRItemVM.ClientSubUnitClientDefinedReferenceItem.RelatedToDisplayName
				}
			);
		}
	}
}