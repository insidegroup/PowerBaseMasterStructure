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
	public class ClientDefinedReferenceItemPNROutputController : Controller
	{
		ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
		ClientSubUnitClientAccountRepository clientSubUnitClientAccountRepository = new ClientSubUnitClientAccountRepository();
		ClientDefinedReferenceItemRepository clientDefinedReferenceItemRepository = new ClientDefinedReferenceItemRepository();
		ClientDefinedReferenceItemPNROutputRepository clientDefinedReferenceItemPNROutputRepository = new ClientDefinedReferenceItemPNROutputRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		private string groupName = "Client Defined References";

		// GET: /List/
		public ActionResult List(string filter, int? page, string id, string sortField, int? sortOrder, string csu, string can, string ssc)
		{
			ClientDefinedReferenceItem clientDefinedReferenceItem = new ClientDefinedReferenceItem();
			clientDefinedReferenceItem = clientDefinedReferenceItemRepository.GetClientDefinedReferenceItem(id);
			
			//Check Exists
			if (clientDefinedReferenceItem == null)
			{
				ViewData["ActionMethod"] = "ListGet";
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
			if (rolesRepository.HasWriteAccessToClientDefinedReferenceItem(id, can, ssc) && hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["CreateAccess"] = "CreateAccess";
			}

			//SortField
			if (string.IsNullOrEmpty(sortField))
			{
				sortField = "GDSName";
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
			ViewData["ClientDefinedReferenceItemId"] = clientDefinedReferenceItem.ClientDefinedReferenceItemId;
			ViewData["DisplayNameAlias"] = clientDefinedReferenceItem.DisplayNameAlias;

			if (can != null && ssc != null)
			{
				ClientAccount clientAccount = new ClientAccount();
				ClientAccountRepository clientAccountRepository = new ClientAccountRepository();
				clientAccount = clientAccountRepository.GetClientAccount(can, ssc);
				if (clientAccount != null)
				{
					ViewData["ClientAccountName"] = clientAccount.ClientAccountName;
				}
				
				ViewData["ClientAccountNumber"] = can;
				ViewData["SourceSystemCode"] = ssc;
			}

			var items = clientDefinedReferenceItemPNROutputRepository.PageClientDefinedReferenceItemPNROutputItems(filter ?? "", id, page ?? 1, sortField, sortOrder ?? 0);

			return View(items);
		}

		// GET: /Create
		public ActionResult Create(string id, string csu, string can, string ssc)
		{
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

			//Check Exists
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			ClientDefinedReferenceItemPNROutputVM clientDefinedReferenceItemPNROutputVM = new ClientDefinedReferenceItemPNROutputVM();
			
			ClientDefinedReferenceItem clientDefinedReferenceItem = new ClientDefinedReferenceItem();
			clientDefinedReferenceItem = clientDefinedReferenceItemRepository.GetClientDefinedReferenceItem(id);

            ClientDefinedReferenceItemPNROutput clientDefinedReferenceItemPNROutput = new ClientDefinedReferenceItemPNROutput();

            //ClientDefinedReferenceItem
            if (clientDefinedReferenceItem == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			}

            clientDefinedReferenceItem.ClientSubUnitGuid = clientSubUnit.ClientSubUnitGuid;
            clientDefinedReferenceItemPNROutput.ClientDefinedReferenceItem = clientDefinedReferenceItem;
            clientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemId = clientDefinedReferenceItem.ClientDefinedReferenceItemId;

            clientDefinedReferenceItemPNROutputVM.ClientDefinedReferenceItemPNROutput = clientDefinedReferenceItemPNROutput;

            ViewData["DsplayName"] = !string.IsNullOrEmpty(clientDefinedReferenceItem.DisplayNameAlias) ? clientDefinedReferenceItem.DisplayNameAlias : clientDefinedReferenceItem.DisplayName; 

            if (can != null && ssc != null)
			{
				ClientSubUnitClientAccount clientSubUnitClientAccount = new ClientSubUnitClientAccount();
				clientSubUnitClientAccount = clientSubUnitClientAccountRepository.GetClientSubUnitClientAccount(can, ssc, clientSubUnit.ClientSubUnitGuid);
				if (clientSubUnitClientAccount != null)
				{
					ViewData["ClientSubUnitClientAccountClientAccountName"] = clientSubUnitClientAccount.ClientAccount.ClientAccountName;
				}

				clientDefinedReferenceItem.ClientAccountNumber = can;
				clientDefinedReferenceItem.SourceSystemCode = ssc;
				ViewData["ClientDefinedReferenceItemId"] = clientDefinedReferenceItem.ClientDefinedReferenceItemId;
				ViewData["ClientAccountNumber"] = can;
				ViewData["SourceSystemCode"] = ssc;
			}

            //ClientSubUnit
            clientDefinedReferenceItemPNROutputVM.ClientSubUnit = clientSubUnit;

            //GDS
            GDSRepository GDSRepository = new GDSRepository();
			clientDefinedReferenceItemPNROutputVM.GDSList = new SelectList(GDSRepository.GetAllGDSsExceptALL().OrderBy(x => x.GDSName).ToList(), "GDSCode", "GDSName");

			//PNR Output Remark Types
			PNROutputRemarkTypeRepository PNROutputRemarkTypeRepository = new PNROutputRemarkTypeRepository();
			clientDefinedReferenceItemPNROutputVM.PNROutputRemarkTypeCodes = new SelectList(PNROutputRemarkTypeRepository.GetPNROutputRemarkTypes(), "PNROutputRemarkTypeCode", "PNROutputRemarkTypeName", "");

			//Languages
			LanguageRepository languageRepository = new LanguageRepository();
			clientDefinedReferenceItemPNROutputVM.Languages = new SelectList(languageRepository.GetAllLanguages().ToList(), "LanguageCode", "LanguageName");

            return View(clientDefinedReferenceItemPNROutputVM);
		}

		// POST: /Create/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ClientDefinedReferenceItemPNROutputVM clientDefinedReferenceItemPNROutputVM)
		{
			ClientDefinedReferenceItem clientDefinedReferenceItem = new ClientDefinedReferenceItem();
			clientDefinedReferenceItem = clientDefinedReferenceItemRepository.GetClientDefinedReferenceItem(clientDefinedReferenceItemPNROutputVM.ClientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemId);

			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel(clientDefinedReferenceItemPNROutputVM);
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
				clientDefinedReferenceItemPNROutputRepository.Add(clientDefinedReferenceItemPNROutputVM.ClientDefinedReferenceItemPNROutput);
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
				id = clientDefinedReferenceItem.ClientDefinedReferenceItemId,
				csu = clientDefinedReferenceItemPNROutputVM.ClientSubUnit.ClientSubUnitGuid,
				can = clientDefinedReferenceItem.ClientAccountNumber,
				ssc = clientDefinedReferenceItem.SourceSystemCode
			});
		}

		// GET: /Edit
		public ActionResult Edit(int id, string csu, string can, string ssc)
		{
			ClientDefinedReferenceItemPNROutputVM clientDefinedReferenceItemPNROutputVM = new ClientDefinedReferenceItemPNROutputVM();

			ClientDefinedReferenceItemPNROutput clientDefinedReferenceItemPNROutput = new ClientDefinedReferenceItemPNROutput();
			clientDefinedReferenceItemPNROutput = clientDefinedReferenceItemPNROutputRepository.GetClientDefinedReferenceItemPNROutput(id);
			if (clientDefinedReferenceItemPNROutput == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			clientDefinedReferenceItemPNROutputVM.ClientDefinedReferenceItemPNROutput = clientDefinedReferenceItemPNROutput;

			//Check Exists
			ClientDefinedReferenceItem clientDefinedReferenceItem = new ClientDefinedReferenceItem();
			clientDefinedReferenceItem = clientDefinedReferenceItemRepository.GetClientDefinedReferenceItem(clientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemId);
			if (clientDefinedReferenceItem == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

            ViewData["DsplayName"] = !string.IsNullOrEmpty(clientDefinedReferenceItem.DisplayNameAlias) ? clientDefinedReferenceItem.DisplayNameAlias : clientDefinedReferenceItem.DisplayName;

            //Check Exists CSU for VM
            ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
			if (clientSubUnit != null)
			{
				clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);
				clientDefinedReferenceItemPNROutputVM.ClientSubUnit = clientSubUnit;
			}

			// Check Exists CDR CSU (can be null)
			ClientSubUnit clientSubUnitCDR = new ClientSubUnit();
			if (clientDefinedReferenceItem.ClientSubUnitGuid != null)
			{
				clientSubUnitCDR = clientSubUnitRepository.GetClientSubUnit(clientDefinedReferenceItem.ClientSubUnitGuid);
				if (clientSubUnit != null)
				{
					clientSubUnitRepository.EditGroupForDisplay(clientSubUnitCDR);
					clientDefinedReferenceItem.ClientSubUnitGuid = clientSubUnitCDR.ClientSubUnitGuid;
				}
			}

			if (can != null && ssc != null)
			{
				ClientSubUnitClientAccount clientSubUnitClientAccount = new ClientSubUnitClientAccount();
				clientSubUnitClientAccount = clientSubUnitClientAccountRepository.GetClientSubUnitClientAccount(can, ssc, csu);
				if (clientSubUnitClientAccount != null)
				{
					ViewData["ClientSubUnitClientAccountClientAccountName"] = clientSubUnitClientAccount.ClientAccount.ClientAccountName;
				}

				clientDefinedReferenceItem.ClientAccountNumber = clientDefinedReferenceItem.ClientAccountNumber;
				clientDefinedReferenceItem.SourceSystemCode = clientDefinedReferenceItem.SourceSystemCode;
			}

			if (can != null && ssc != null)
			{
				ClientAccount clientAccount = new ClientAccount();
				ClientAccountRepository clientAccountRepository = new ClientAccountRepository();
				clientAccount = clientAccountRepository.GetClientAccount(can, ssc);
				if (clientAccount != null)
				{
					ViewData["ClientAccountName"] = clientAccount.ClientAccountName;
				}

				clientDefinedReferenceItem.ClientAccountNumber = can;
				clientDefinedReferenceItem.SourceSystemCode = ssc;
				ViewData["ClientDefinedReferenceItemId"] = clientDefinedReferenceItem.ClientDefinedReferenceItemId;
				ViewData["ClientAccountNumber"] = can;
				ViewData["SourceSystemCode"] = ssc;
			}

            //ClientDefinedReferenceItem
            clientDefinedReferenceItemPNROutput.ClientDefinedReferenceItem = clientDefinedReferenceItem;
            clientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemId = clientDefinedReferenceItem.ClientDefinedReferenceItemId;

            //GDS
            GDSRepository GDSRepository = new GDSRepository();
			clientDefinedReferenceItemPNROutputVM.GDSList = new SelectList(GDSRepository.GetAllGDSsExceptALL().OrderBy(x => x.GDSName).ToList(), "GDSCode", "GDSName");

			//PNR Output Remark Types
			PNROutputRemarkTypeRepository PNROutputRemarkTypeRepository = new PNROutputRemarkTypeRepository();
			clientDefinedReferenceItemPNROutputVM.PNROutputRemarkTypeCodes = new SelectList(PNROutputRemarkTypeRepository.GetPNROutputRemarkTypes(), "PNROutputRemarkTypeCode", "PNROutputRemarkTypeName");

			//Languages
			LanguageRepository languageRepository = new LanguageRepository();
			clientDefinedReferenceItemPNROutputVM.Languages = new SelectList(languageRepository.GetAllLanguages().ToList(), "LanguageCode", "LanguageName");

			return View(clientDefinedReferenceItemPNROutputVM);
		}

		// POST: /Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(ClientDefinedReferenceItemPNROutputVM clientDefinedReferenceItemPNROutputVM)
		{
			ClientDefinedReferenceItem clientDefinedReferenceItem = new ClientDefinedReferenceItem();
			clientDefinedReferenceItem = clientDefinedReferenceItemRepository.GetClientDefinedReferenceItem(clientDefinedReferenceItemPNROutputVM.ClientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemId);

			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel(clientDefinedReferenceItemPNROutputVM);
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
				clientDefinedReferenceItemPNROutputRepository.Update(clientDefinedReferenceItemPNROutputVM.ClientDefinedReferenceItemPNROutput);
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
				id = clientDefinedReferenceItem.ClientDefinedReferenceItemId,
				csu = clientDefinedReferenceItemPNROutputVM.ClientSubUnit.ClientSubUnitGuid,
				can = clientDefinedReferenceItem.ClientAccountNumber,
				ssc = clientDefinedReferenceItem.SourceSystemCode
			});
		}

		// GET: /Delete/5
		[HttpGet]
		public ActionResult Delete(int id, string csu, string can, string ssc)
		{
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

			//Check Exists
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			ClientDefinedReferenceItemPNROutputVM clientDefinedReferenceItemPNROutputVM = new ClientDefinedReferenceItemPNROutputVM();

			ClientDefinedReferenceItemPNROutput clientDefinedReferenceItemPNROutput = new ClientDefinedReferenceItemPNROutput();
			clientDefinedReferenceItemPNROutput = clientDefinedReferenceItemPNROutputRepository.GetClientDefinedReferenceItemPNROutput(id);

			//Check Exists
			if (clientDefinedReferenceItemPNROutput == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			ClientDefinedReferenceItem clientDefinedReferenceItem = new ClientDefinedReferenceItem();
			clientDefinedReferenceItem = clientDefinedReferenceItemRepository.GetClientDefinedReferenceItem(clientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemId);

			//Check Exists
			if (clientDefinedReferenceItem == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

            ViewData["DsplayName"] = !string.IsNullOrEmpty(clientDefinedReferenceItem.DisplayNameAlias) ? clientDefinedReferenceItem.DisplayNameAlias : clientDefinedReferenceItem.DisplayName;

            if (can != null && ssc != null)
			{
				ClientSubUnitClientAccount clientSubUnitClientAccount = new ClientSubUnitClientAccount();
				clientSubUnitClientAccount = clientSubUnitClientAccountRepository.GetClientSubUnitClientAccount(can, ssc, clientSubUnit.ClientSubUnitGuid);
				if (clientSubUnitClientAccount != null)
				{
					ViewData["ClientSubUnitClientAccountClientAccountName"] = clientSubUnitClientAccount.ClientAccount.ClientAccountName;
				}

				clientDefinedReferenceItem.ClientAccountNumber = can;
				clientDefinedReferenceItem.SourceSystemCode = ssc;
				ViewData["ClientDefinedReferenceItemId"] = clientDefinedReferenceItem.ClientDefinedReferenceItemId;
				ViewData["ClientAccountNumber"] = can;
				ViewData["SourceSystemCode"] = ssc;

			}

			clientDefinedReferenceItemPNROutputVM.ClientDefinedReferenceItemPNROutput = clientDefinedReferenceItemPNROutput;
			clientDefinedReferenceItemPNROutputVM.ClientSubUnit = clientSubUnit;
			clientDefinedReferenceItem.ClientSubUnitGuid = clientSubUnit.ClientSubUnitGuid;
			clientDefinedReferenceItemPNROutputVM.ClientSubUnit = clientSubUnit;

            clientDefinedReferenceItemPNROutput.ClientDefinedReferenceItem = clientDefinedReferenceItem;
            clientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemId = clientDefinedReferenceItem.ClientDefinedReferenceItemId;

            return View(clientDefinedReferenceItemPNROutputVM);

		}

		// POST: //Delete/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(ClientDefinedReferenceItemPNROutputVM clientDefinedReferenceItemPNROutputVM)
		{
			ClientDefinedReferenceItem clientDefinedReferenceItem = new ClientDefinedReferenceItem();
			clientDefinedReferenceItem = clientDefinedReferenceItemRepository.GetClientDefinedReferenceItem(clientDefinedReferenceItemPNROutputVM.ClientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemId);

			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Delete item
			try
			{
				clientDefinedReferenceItemPNROutputRepository.Delete(clientDefinedReferenceItemPNROutputVM.ClientDefinedReferenceItemPNROutput);
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
				id = clientDefinedReferenceItem.ClientDefinedReferenceItemId,
				csu = clientDefinedReferenceItemPNROutputVM.ClientSubUnit.ClientSubUnitGuid,
				can = clientDefinedReferenceItem.ClientAccountNumber,
				ssc = clientDefinedReferenceItem.SourceSystemCode
			});
		}
	}
}