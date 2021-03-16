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
	public class ClientSubUnitClientDefinedReferenceItemValueController : Controller
	{
		ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
		ClientDefinedReferenceItemRepository clientDefinedReferenceItemRepository = new ClientDefinedReferenceItemRepository();
		ClientSubUnitClientDefinedReferenceItemValueRepository clientSubUnitClientDefinedReferenceItemValueRepository = new ClientSubUnitClientDefinedReferenceItemValueRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		private string groupName = "Client Defined References";

		// GET: /List/
		public ActionResult List(string filter, int? page, string id, string csu, string sortField, int? sortOrder)
		{
			//Check Exists
			ClientDefinedReferenceItem clientDefinedReferenceItem = new ClientDefinedReferenceItem();
			clientDefinedReferenceItem = clientDefinedReferenceItemRepository.GetClientDefinedReferenceItem(id);
			if (clientDefinedReferenceItem == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			} 
			
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

			//Check Exists
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "ListGet";
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
			if (rolesRepository.HasWriteAccessToClientSubUnitClientDefinedReferenceItemValue(id))
			{
				ViewData["CreateAccess"] = "CreateAccess";
			}

			//SortField
			if (string.IsNullOrEmpty(sortField))
			{
				sortField = "Value";
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
			ViewData["ClientDefinedReferenceItemId"] = clientDefinedReferenceItem.ClientDefinedReferenceItemId ?? "";
			ViewData["ClientDefinedReferenceItemDisplayNameAlias"] = clientDefinedReferenceItem.DisplayNameAlias ?? "";

			var items = clientSubUnitClientDefinedReferenceItemValueRepository.PageClientDefinedReferenceItemValues(filter ?? "", id, page ?? 1, sortField, sortOrder ?? 0);
			
			return View(items);
		}

		// GET: /Create
		public ActionResult Create(string id, string csu)
		{
			//Check Exists
			ClientDefinedReferenceItem clientDefinedReferenceItem = new ClientDefinedReferenceItem();
			clientDefinedReferenceItem = clientDefinedReferenceItemRepository.GetClientDefinedReferenceItem(id);
			if (clientDefinedReferenceItem == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

			//Check Exists
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			ClientDefinedReferenceItemValueVM clientDefinedReferenceItemValueVM = new ClientDefinedReferenceItemValueVM();

			ClientDefinedReferenceItemValue clientDefinedReferenceItemValue = new ClientDefinedReferenceItemValue();
			clientDefinedReferenceItemValue.ClientDefinedReferenceItemId = clientDefinedReferenceItem.ClientDefinedReferenceItemId;
			clientDefinedReferenceItemValueVM.ClientDefinedReferenceItemValue = clientDefinedReferenceItemValue;
			clientDefinedReferenceItemValueVM.ClientDefinedReferenceItem = clientDefinedReferenceItem;
			clientDefinedReferenceItemValueVM.ClientSubUnit = clientSubUnit;

			clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);
			ViewData["ClientTopUnitGuid"] = clientSubUnit.ClientTopUnitGuid;
			ViewData["ClientTopUnitName"] = clientSubUnit.ClientTopUnitName;
			ViewData["ClientSubUnitGuid"] = clientSubUnit.ClientSubUnitGuid;
			ViewData["ClientSubUnitName"] = clientSubUnit.ClientSubUnitName;
			ViewData["ClientDefinedReferenceItemId"] = clientDefinedReferenceItem.ClientDefinedReferenceItemId ?? "";
			ViewData["ClientDefinedReferenceItemDisplayNameAlias"] = clientDefinedReferenceItem.DisplayNameAlias ?? "";

			return View(clientDefinedReferenceItemValueVM);
		}

		//// POST: /Create/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ClientDefinedReferenceItemValueVM clientDefinedReferenceItemValueVM)
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
				UpdateModel(clientDefinedReferenceItemValueVM);
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
				clientSubUnitClientDefinedReferenceItemValueRepository.Add(clientDefinedReferenceItemValueVM.ClientDefinedReferenceItemValue);
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

			return RedirectToAction("List", new
			{
				id = clientDefinedReferenceItemValueVM.ClientDefinedReferenceItemValue.ClientDefinedReferenceItemId,
				csu = clientDefinedReferenceItemValueVM.ClientSubUnit.ClientSubUnitGuid
			});
		}

		// GET: /Edit
		public ActionResult Edit(string id, string csu)
		{
			//Check Exists
			ClientDefinedReferenceItemValue clientDefinedReferenceItemValue = new ClientDefinedReferenceItemValue();
			clientDefinedReferenceItemValue = clientSubUnitClientDefinedReferenceItemValueRepository.GetClientDefinedReferenceItemValue(id);
			if (clientDefinedReferenceItemValue == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check Exists
			ClientDefinedReferenceItem clientDefinedReferenceItem = new ClientDefinedReferenceItem();
			clientDefinedReferenceItem = clientDefinedReferenceItemRepository.GetClientDefinedReferenceItem(clientDefinedReferenceItemValue.ClientDefinedReferenceItemId);
			if (clientDefinedReferenceItem == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

			//Check Exists
			if (clientSubUnit == null)
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

			ClientDefinedReferenceItemValueVM clientDefinedReferenceItemValueVM = new ClientDefinedReferenceItemValueVM();
			clientDefinedReferenceItemValueVM.ClientDefinedReferenceItemValue = clientDefinedReferenceItemValue;
			clientDefinedReferenceItemValueVM.ClientDefinedReferenceItem = clientDefinedReferenceItem;
			clientDefinedReferenceItemValueVM.ClientSubUnit = clientSubUnit;
			clientDefinedReferenceItemValueVM.ClientDefinedReferenceItemValue = clientDefinedReferenceItemValue;

			clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);
			ViewData["ClientTopUnitGuid"] = clientSubUnit.ClientTopUnitGuid;
			ViewData["ClientTopUnitName"] = clientSubUnit.ClientTopUnitName;
			ViewData["ClientSubUnitGuid"] = clientSubUnit.ClientSubUnitGuid;
			ViewData["ClientSubUnitName"] = clientSubUnit.ClientSubUnitName;
			ViewData["ClientDefinedReferenceItemId"] = clientDefinedReferenceItem.ClientDefinedReferenceItemId ?? "";
			ViewData["ClientDefinedReferenceItemDisplayNameAlias"] = clientDefinedReferenceItem.DisplayNameAlias ?? "";

			return View(clientDefinedReferenceItemValueVM);
		}

		//// POST: /Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(ClientDefinedReferenceItemValueVM clientDefinedReferenceItemValueVM)
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
				UpdateModel(clientDefinedReferenceItemValueVM);
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
				clientSubUnitClientDefinedReferenceItemValueRepository.Update(clientDefinedReferenceItemValueVM.ClientDefinedReferenceItemValue);
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

			return RedirectToAction("List", new
			{
				id = clientDefinedReferenceItemValueVM.ClientDefinedReferenceItemValue.ClientDefinedReferenceItemId,
				csu = clientDefinedReferenceItemValueVM.ClientSubUnit.ClientSubUnitGuid
			});
		}

		// GET: /Delete/5
		[HttpGet]
		public ActionResult Delete(string id, string csu)
		{
			//Check Exists
			ClientDefinedReferenceItemValue clientDefinedReferenceItemValue = new ClientDefinedReferenceItemValue();
			clientDefinedReferenceItemValue = clientSubUnitClientDefinedReferenceItemValueRepository.GetClientDefinedReferenceItemValue(id);
			if (clientDefinedReferenceItemValue == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check Exists
			ClientDefinedReferenceItem clientDefinedReferenceItem = new ClientDefinedReferenceItem();
			clientDefinedReferenceItem = clientDefinedReferenceItemRepository.GetClientDefinedReferenceItem(clientDefinedReferenceItemValue.ClientDefinedReferenceItemId);
			if (clientDefinedReferenceItem == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

			//Check Exists
			if (clientSubUnit == null)
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

			ClientDefinedReferenceItemValueVM clientDefinedReferenceItemValueVM = new ClientDefinedReferenceItemValueVM();
			clientDefinedReferenceItemValueVM.ClientDefinedReferenceItemValue = clientDefinedReferenceItemValue;
			clientDefinedReferenceItemValueVM.ClientDefinedReferenceItem = clientDefinedReferenceItem;
			clientDefinedReferenceItemValueVM.ClientSubUnit = clientSubUnit;
			clientDefinedReferenceItemValueVM.ClientDefinedReferenceItemValue = clientDefinedReferenceItemValue;

			clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);
			ViewData["ClientTopUnitGuid"] = clientSubUnit.ClientTopUnitGuid;
			ViewData["ClientTopUnitName"] = clientSubUnit.ClientTopUnitName;
			ViewData["ClientSubUnitGuid"] = clientSubUnit.ClientSubUnitGuid;
			ViewData["ClientSubUnitName"] = clientSubUnit.ClientSubUnitName;
			ViewData["ClientDefinedReferenceItemId"] = clientDefinedReferenceItem.ClientDefinedReferenceItemId ?? "";
			ViewData["ClientDefinedReferenceItemDisplayNameAlias"] = clientDefinedReferenceItem.DisplayNameAlias ?? "";

			return View(clientDefinedReferenceItemValueVM);
		}

		// POST: //Delete/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(ClientDefinedReferenceItemValueVM clientDefinedReferenceItemValueVM)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Check Exists
			ClientDefinedReferenceItemValue clientDefinedReferenceItemValue = new ClientDefinedReferenceItemValue();
			clientDefinedReferenceItemValue = clientSubUnitClientDefinedReferenceItemValueRepository.GetClientDefinedReferenceItemValue(
					clientDefinedReferenceItemValueVM.ClientDefinedReferenceItemValue.ClientDefinedReferenceItemValueId
			);
			if (clientDefinedReferenceItemValue == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			//Delete Item
			try
			{
				clientSubUnitClientDefinedReferenceItemValueRepository.Delete(clientDefinedReferenceItemValue);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/ClientDefinedReferenceItem.mvc/Delete/" + clientDefinedReferenceItemValue.ClientDefinedReferenceItemValueId.ToString();
					return View("VersionError");
				}
				//Restraint Error - go to standard DeleteError page
				if (ex.Message == "SQLDeleteError")
				{
					ViewData["ReturnURL"] = "/ClientDefinedReferenceItem.mvc/Delete/" + clientDefinedReferenceItemValue.ClientDefinedReferenceItemValueId.ToString();
					return View("DeleteError");
				}
				//Generic Error
				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new
			{
				id = clientDefinedReferenceItemValueVM.ClientDefinedReferenceItemValue.ClientDefinedReferenceItemId,
				csu = clientDefinedReferenceItemValueVM.ClientSubUnit.ClientSubUnitGuid
			});
		}

	}
}