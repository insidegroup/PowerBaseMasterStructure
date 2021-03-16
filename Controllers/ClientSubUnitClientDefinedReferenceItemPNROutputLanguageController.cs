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
	public class ClientSubUnitClientDefinedReferenceItemPNROutputLanguageController : Controller
	{
		ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
		ClientSubUnitClientAccountRepository clientSubUnitClientAccountRepository = new ClientSubUnitClientAccountRepository();
		ClientDefinedReferenceItemRepository clientDefinedReferenceItemRepository = new ClientDefinedReferenceItemRepository();
		ClientDefinedReferenceItemPNROutputRepository clientDefinedReferenceItemPNROutputRepository = new ClientDefinedReferenceItemPNROutputRepository();
		ClientDefinedReferenceItemPNROutputLanguageRepository clientDefinedReferenceItemPNROutputLanguageRepository = new ClientDefinedReferenceItemPNROutputLanguageRepository();

		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		private string groupName = "Client Defined References";

		// GET: /List/
		public ActionResult List(int id, string filter, int? page, string sortField, int? sortOrder, string csu)
		{
			ClientDefinedReferenceItemPNROutput clientDefinedReferenceItemPNROutput = new ClientDefinedReferenceItemPNROutput();
			clientDefinedReferenceItemPNROutput = clientDefinedReferenceItemPNROutputRepository.GetClientDefinedReferenceItemPNROutput(id);
			
			//Check Exists
			if (clientDefinedReferenceItemPNROutput == null)
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

			//SortField
			if (string.IsNullOrEmpty(sortField))
			{
				sortField = "LanguageName";
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

			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

			//Check Exists
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("RecordDoesNotExistError");
			}

			ClientDefinedReferenceItem clientDefinedReferenceItem = new ClientDefinedReferenceItem();
			clientDefinedReferenceItem = clientDefinedReferenceItemRepository.GetClientDefinedReferenceItem(clientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemId);

			//Check Exists
			if (clientDefinedReferenceItem == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("RecordDoesNotExistError");
			}

			clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);
			ViewData["ClientTopUnitGuid"] = clientSubUnit.ClientTopUnitGuid;
			ViewData["ClientTopUnitName"] = clientSubUnit.ClientTopUnitName;
			ViewData["ClientSubUnitGuid"] = clientSubUnit.ClientSubUnitGuid;
			ViewData["ClientSubUnitName"] = clientSubUnit.ClientSubUnitName;
			ViewData["ClientDefinedReferenceItemId"] = clientDefinedReferenceItem.ClientDefinedReferenceItemId;
			ViewData["ClientDefinedReferenceItemPNROutputId"] = clientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemPNROutputId;
			ViewData["ClientDefinedReferenceItemPNROutputDefaultRemark"] = clientDefinedReferenceItemPNROutput.DefaultRemark;
			ViewData["DisplayNameAlias"] = clientDefinedReferenceItem.DisplayNameAlias != null ? clientDefinedReferenceItem.DisplayNameAlias : clientDefinedReferenceItem.DisplayName;

			var items = clientDefinedReferenceItemPNROutputLanguageRepository.PageClientDefinedReferenceItemPNROutputLanguages(id, filter ?? "", page ?? 1, sortField, sortOrder ?? 0);

			return View(items);
		}

		// GET: /Create
		public ActionResult Create(int id, string csu, string can, string ssc)
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

			ClientDefinedReferenceItemPNROutputLanguageVM clientDefinedReferenceItemPNROutputLanguageVM = new ClientDefinedReferenceItemPNROutputLanguageVM();

			ClientDefinedReferenceItemPNROutput clientDefinedReferenceItemPNROutput = new ClientDefinedReferenceItemPNROutput();
			clientDefinedReferenceItemPNROutput = clientDefinedReferenceItemPNROutputRepository.GetClientDefinedReferenceItemPNROutput(id);
			
			//Check Exists
			if (clientDefinedReferenceItemPNROutput == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			}
			
			ClientDefinedReferenceItem clientDefinedReferenceItem = new ClientDefinedReferenceItem();
			clientDefinedReferenceItem = clientDefinedReferenceItemRepository.GetClientDefinedReferenceItem(clientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemId);
			
			//Check Exists
			if (clientDefinedReferenceItem == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			}

			clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);
			ViewData["ClientTopUnitGuid"] = clientSubUnit.ClientTopUnitGuid;
			ViewData["ClientTopUnitName"] = clientSubUnit.ClientTopUnitName;
			ViewData["ClientSubUnitGuid"] = clientSubUnit.ClientSubUnitGuid;
			ViewData["ClientSubUnitName"] = clientSubUnit.ClientSubUnitName;
			ViewData["ClientDefinedReferenceItemId"] = clientDefinedReferenceItem.ClientDefinedReferenceItemId;
			ViewData["ClientDefinedReferenceItemPNROutputId"] = clientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemPNROutputId;
			ViewData["ClientDefinedReferenceItemPNROutputDefaultRemark"] = clientDefinedReferenceItemPNROutput.DefaultRemark;
			ViewData["DisplayNameAlias"] = clientDefinedReferenceItem.DisplayNameAlias != null ? clientDefinedReferenceItem.DisplayNameAlias : clientDefinedReferenceItem.DisplayName;

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

			ClientDefinedReferenceItemPNROutputLanguage clientDefinedReferenceItemPNROutputLanguage = new ClientDefinedReferenceItemPNROutputLanguage();
			clientDefinedReferenceItemPNROutputLanguage.ClientDefinedReferenceItemPNROutputId = clientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemPNROutputId;
			clientDefinedReferenceItemPNROutputLanguageVM.ClientDefinedReferenceItemPNROutputLanguage = clientDefinedReferenceItemPNROutputLanguage;
			clientDefinedReferenceItemPNROutputLanguageVM.ClientDefinedReferenceItemPNROutput = clientDefinedReferenceItemPNROutput;
			clientDefinedReferenceItemPNROutputLanguageVM.ClientSubUnit = clientSubUnit;
			clientDefinedReferenceItem.ClientSubUnitGuid = clientSubUnit.ClientSubUnitGuid;
			
			//Languages
			LanguageRepository languageRepository = new LanguageRepository();
			clientDefinedReferenceItemPNROutputLanguageVM.Languages = new SelectList(clientDefinedReferenceItemPNROutputLanguageRepository.GetAvailableLanguages(clientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemPNROutputId).ToList(), "LanguageCode", "LanguageName");

			return View(clientDefinedReferenceItemPNROutputLanguageVM);
		}

		// POST: /Create/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ClientDefinedReferenceItemPNROutputLanguageVM clientDefinedReferenceItemPNROutputLanguageVM)
		{
			ClientDefinedReferenceItem clientDefinedReferenceItem = new ClientDefinedReferenceItem();
			clientDefinedReferenceItem = clientDefinedReferenceItemRepository.GetClientDefinedReferenceItem(
				clientDefinedReferenceItemPNROutputLanguageVM.ClientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemId
			);

			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel(clientDefinedReferenceItemPNROutputLanguageVM);
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
				clientDefinedReferenceItemPNROutputLanguageRepository.Add(clientDefinedReferenceItemPNROutputLanguageVM.ClientDefinedReferenceItemPNROutputLanguage);
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
				id = clientDefinedReferenceItemPNROutputLanguageVM.ClientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemPNROutputId,
				csu = clientDefinedReferenceItemPNROutputLanguageVM.ClientSubUnit.ClientSubUnitGuid,
				can = clientDefinedReferenceItem.ClientAccountNumber,
				ssc = clientDefinedReferenceItem.SourceSystemCode
			});
		}

		// GET: /Edit
		public ActionResult Edit(int id, string languageCode, string csu, string can, string ssc)
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

			ClientDefinedReferenceItemPNROutputLanguageVM clientDefinedReferenceItemPNROutputLanguageVM = new ClientDefinedReferenceItemPNROutputLanguageVM();

			ClientDefinedReferenceItemPNROutput clientDefinedReferenceItemPNROutput = new ClientDefinedReferenceItemPNROutput();
			clientDefinedReferenceItemPNROutput = clientDefinedReferenceItemPNROutputRepository.GetClientDefinedReferenceItemPNROutput(id);

			//Check Exists
			if (clientDefinedReferenceItemPNROutput == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			ClientDefinedReferenceItem clientDefinedReferenceItem = new ClientDefinedReferenceItem();
			clientDefinedReferenceItem = clientDefinedReferenceItemRepository.GetClientDefinedReferenceItem(clientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemId);

			//Check Exists
			if (clientDefinedReferenceItem == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);
			ViewData["ClientTopUnitGuid"] = clientSubUnit.ClientTopUnitGuid;
			ViewData["ClientTopUnitName"] = clientSubUnit.ClientTopUnitName;
			ViewData["ClientSubUnitGuid"] = clientSubUnit.ClientSubUnitGuid;
			ViewData["ClientSubUnitName"] = clientSubUnit.ClientSubUnitName;
			ViewData["ClientDefinedReferenceItemId"] = clientDefinedReferenceItem.ClientDefinedReferenceItemId;
			ViewData["ClientDefinedReferenceItemPNROutputId"] = clientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemPNROutputId;
			ViewData["ClientDefinedReferenceItemPNROutputDefaultRemark"] = clientDefinedReferenceItemPNROutput.DefaultRemark;
			ViewData["DisplayNameAlias"] = clientDefinedReferenceItem.DisplayNameAlias != null ? clientDefinedReferenceItem.DisplayNameAlias : clientDefinedReferenceItem.DisplayName;

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

			ClientDefinedReferenceItemPNROutputLanguage clientDefinedReferenceItemPNROutputLanguage = new ClientDefinedReferenceItemPNROutputLanguage();
			clientDefinedReferenceItemPNROutputLanguage = clientDefinedReferenceItemPNROutputLanguageRepository.GetClientDefinedReferenceItemPNROutputLanguage(id, languageCode);
            clientDefinedReferenceItemPNROutputLanguage.CurrentLanguageCode = clientDefinedReferenceItemPNROutputLanguage.LanguageCode;

            //Check Exists
            if (clientDefinedReferenceItemPNROutputLanguage == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			clientDefinedReferenceItemPNROutputLanguageVM.ClientDefinedReferenceItemPNROutputLanguage = clientDefinedReferenceItemPNROutputLanguage;
			clientDefinedReferenceItemPNROutputLanguageVM.ClientDefinedReferenceItemPNROutput = clientDefinedReferenceItemPNROutput;
			clientDefinedReferenceItemPNROutputLanguageVM.ClientSubUnit = clientSubUnit;
			clientDefinedReferenceItem.ClientSubUnitGuid = clientSubUnit.ClientSubUnitGuid;

			//Languages
			LanguageRepository languageRepository = new LanguageRepository();
			List<Language> languages = clientDefinedReferenceItemPNROutputLanguageRepository.GetAvailableLanguages(clientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemPNROutputId).ToList();
			Language language = languageRepository.GetLanguage(clientDefinedReferenceItemPNROutputLanguage.LanguageCode);
			if (language != null)
			{
				languages.Add(language);
			}

			clientDefinedReferenceItemPNROutputLanguageVM.Languages = new SelectList(languages, "LanguageCode", "LanguageName", clientDefinedReferenceItemPNROutputLanguage.LanguageCode);

			return View(clientDefinedReferenceItemPNROutputLanguageVM);
		}

		// POST: /Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(ClientDefinedReferenceItemPNROutputLanguageVM clientDefinedReferenceItemPNROutputLanguageVM)
		{
			ClientDefinedReferenceItemPNROutputLanguage clientDefinedReferenceItemPNROutputLanguage = new ClientDefinedReferenceItemPNROutputLanguage();
			clientDefinedReferenceItemPNROutputLanguage = clientDefinedReferenceItemPNROutputLanguageRepository.GetClientDefinedReferenceItemPNROutputLanguage(
				clientDefinedReferenceItemPNROutputLanguageVM.ClientDefinedReferenceItemPNROutputLanguage.ClientDefinedReferenceItemPNROutputId,
				clientDefinedReferenceItemPNROutputLanguageVM.ClientDefinedReferenceItemPNROutputLanguage.CurrentLanguageCode
			);

			//Check Exists
			if (clientDefinedReferenceItemPNROutputLanguage == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			ClientDefinedReferenceItem clientDefinedReferenceItem = new ClientDefinedReferenceItem();
			clientDefinedReferenceItem = clientDefinedReferenceItemRepository.GetClientDefinedReferenceItem(
				clientDefinedReferenceItemPNROutputLanguageVM.ClientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemId
			);

			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel(clientDefinedReferenceItemPNROutputLanguageVM);
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
				clientDefinedReferenceItemPNROutputLanguageRepository.Update(clientDefinedReferenceItemPNROutputLanguageVM.ClientDefinedReferenceItemPNROutputLanguage);
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
				id = clientDefinedReferenceItemPNROutputLanguageVM.ClientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemPNROutputId,
				csu = clientDefinedReferenceItemPNROutputLanguageVM.ClientSubUnit.ClientSubUnitGuid,
				can = clientDefinedReferenceItem.ClientAccountNumber,
				ssc = clientDefinedReferenceItem.SourceSystemCode
			});
		}

		// GET: /Delete/5
		[HttpGet]
		public ActionResult Delete(int id, string languageCode, string csu, string can, string ssc)
		{
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

			//Check Exists
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			ClientDefinedReferenceItemPNROutputLanguageVM clientDefinedReferenceItemPNROutputLanguageVM = new ClientDefinedReferenceItemPNROutputLanguageVM();

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

			clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);
			ViewData["ClientTopUnitGuid"] = clientSubUnit.ClientTopUnitGuid;
			ViewData["ClientTopUnitName"] = clientSubUnit.ClientTopUnitName;
			ViewData["ClientSubUnitGuid"] = clientSubUnit.ClientSubUnitGuid;
			ViewData["ClientSubUnitName"] = clientSubUnit.ClientSubUnitName;
			ViewData["ClientDefinedReferenceItemId"] = clientDefinedReferenceItem.ClientDefinedReferenceItemId;
			ViewData["ClientDefinedReferenceItemPNROutputId"] = clientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemPNROutputId;
			ViewData["ClientDefinedReferenceItemPNROutputDefaultRemark"] = clientDefinedReferenceItemPNROutput.DefaultRemark;
			ViewData["DisplayNameAlias"] = clientDefinedReferenceItem.DisplayNameAlias != null ? clientDefinedReferenceItem.DisplayNameAlias : clientDefinedReferenceItem.DisplayName;

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

			ClientDefinedReferenceItemPNROutputLanguage clientDefinedReferenceItemPNROutputLanguage = new ClientDefinedReferenceItemPNROutputLanguage();
			clientDefinedReferenceItemPNROutputLanguage = clientDefinedReferenceItemPNROutputLanguageRepository.GetClientDefinedReferenceItemPNROutputLanguage(id, languageCode);

			//Check Exists
			if (clientDefinedReferenceItemPNROutputLanguage == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			clientDefinedReferenceItemPNROutputLanguageVM.ClientDefinedReferenceItemPNROutputLanguage = clientDefinedReferenceItemPNROutputLanguage;
			clientDefinedReferenceItemPNROutputLanguageVM.ClientDefinedReferenceItemPNROutput = clientDefinedReferenceItemPNROutput;
			clientDefinedReferenceItemPNROutputLanguageVM.ClientSubUnit = clientSubUnit;
			clientDefinedReferenceItem.ClientSubUnitGuid = clientSubUnit.ClientSubUnitGuid;

			//Languages
			LanguageRepository languageRepository = new LanguageRepository();
			List<Language> languages = clientDefinedReferenceItemPNROutputLanguageRepository.GetAvailableLanguages(clientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemPNROutputId).ToList();
			Language language = languageRepository.GetLanguage(clientDefinedReferenceItemPNROutputLanguage.LanguageCode);
			if (language != null)
			{
				languages.Add(language);
			}

			clientDefinedReferenceItemPNROutputLanguageVM.Languages = new SelectList(languages, "LanguageCode", "LanguageName", clientDefinedReferenceItemPNROutputLanguage.LanguageCode);

			return View(clientDefinedReferenceItemPNROutputLanguageVM);

		}

		// POST: //Delete/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(ClientDefinedReferenceItemPNROutputLanguageVM clientDefinedReferenceItemPNROutputLanguageVM)
		{
			ClientDefinedReferenceItemPNROutputLanguage clientDefinedReferenceItemPNROutputLanguage = new ClientDefinedReferenceItemPNROutputLanguage();
			clientDefinedReferenceItemPNROutputLanguage = clientDefinedReferenceItemPNROutputLanguageRepository.GetClientDefinedReferenceItemPNROutputLanguage(
				clientDefinedReferenceItemPNROutputLanguageVM.ClientDefinedReferenceItemPNROutputLanguage.ClientDefinedReferenceItemPNROutputId,
				clientDefinedReferenceItemPNROutputLanguageVM.ClientDefinedReferenceItemPNROutputLanguage.LanguageCode
			);

			//Check Exists
			if (clientDefinedReferenceItemPNROutputLanguage == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			ClientDefinedReferenceItemPNROutput clientDefinedReferenceItemPNROutput = new ClientDefinedReferenceItemPNROutput();
			clientDefinedReferenceItemPNROutput = clientDefinedReferenceItemPNROutputRepository.GetClientDefinedReferenceItemPNROutput(
				clientDefinedReferenceItemPNROutputLanguage.ClientDefinedReferenceItemPNROutputId
			);

			//Check Exists
			if (clientDefinedReferenceItemPNROutput == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			ClientDefinedReferenceItem clientDefinedReferenceItem = new ClientDefinedReferenceItem();
			clientDefinedReferenceItem = clientDefinedReferenceItemRepository.GetClientDefinedReferenceItem(
				clientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemId
			);

			//Check Exists
			if (clientDefinedReferenceItem == null)
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

			//Delete item
			try
			{
				clientDefinedReferenceItemPNROutputLanguageRepository.Delete(clientDefinedReferenceItemPNROutputLanguage);
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
				id = clientDefinedReferenceItemPNROutputLanguageVM.ClientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemPNROutputId,
				csu = clientDefinedReferenceItemPNROutputLanguageVM.ClientSubUnit.ClientSubUnitGuid,
				can = clientDefinedReferenceItem.ClientAccountNumber,
				ssc = clientDefinedReferenceItem.SourceSystemCode
			});
		}
	}
}