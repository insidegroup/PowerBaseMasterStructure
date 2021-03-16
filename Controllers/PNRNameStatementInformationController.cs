using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
	public class PNRNameStatementInformationController : Controller
	{
		ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
		PNRNameStatementInformationRepository PNRNameStatementInformationRepository = new PNRNameStatementInformationRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();

		private string groupName = "Client Defined References";

		// GET: /List
		public ActionResult List(int? page, string id)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToClientSubUnit(id) && hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);

			//Check clientSubUnit
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "List";
				return View("RecordDoesNotExistError");
			}

			clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);

			ViewData["ClientSubUnitName"] = clientSubUnit.ClientSubUnitDisplayName;
			ViewData["ClientSubUnitGuid"] = clientSubUnit.ClientSubUnitGuid;
			ViewData["ClientTopUnitName"] = clientSubUnit.ClientTopUnitName;
			ViewData["ClientTopUnitGuid"] = clientSubUnit.ClientTopUnitGuid;

			var PNRNameStatementInformationitems = PNRNameStatementInformationRepository.PagePNRNameStatementInformationItems(page ?? 1, id, "GDSName", 0);

			//return items
			return View(PNRNameStatementInformationitems);
		}

		// GET: Create
		public ActionResult Create(string id)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToClientSubUnit(id) && hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);

			//Check clientSubUnit
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "List";
				return View("RecordDoesNotExistError");
			}

			clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);

			ViewData["ClientSubUnitName"] = clientSubUnit.ClientSubUnitDisplayName;
			ViewData["ClientSubUnitGuid"] = clientSubUnit.ClientSubUnitGuid;
			ViewData["ClientTopUnitName"] = clientSubUnit.ClientTopUnitName;
			ViewData["ClientTopUnitGuid"] = clientSubUnit.ClientTopUnitGuid;

			PNRNameStatementInformationVM PNRNameStatementInformationVM = new PNRNameStatementInformationVM();
			PNRNameStatementInformation PNRNameStatementInformation = new PNRNameStatementInformation();
			PNRNameStatementInformation.ClientSubUnit = clientSubUnit;
			PNRNameStatementInformationVM.PNRNameStatementInformation = PNRNameStatementInformation;
			PNRNameStatementInformationVM.ClientSubUnit = clientSubUnit;

			//GDS
			GDSRepository GDSRepository = new GDSRepository();
			PNRNameStatementInformationVM.GDSList = new SelectList(GDSRepository.GetAllGDSsExceptALL().OrderBy(x => x.GDSName).ToList(), "GDSCode", "GDSName");

			//Delimiters
			PNRNameStatementInformationVM.Delimiters = new SelectList(PNRNameStatementInformationRepository.GetPNRNameStatementInformationDelimiters().ToList(), "Value", "Text");

			//Statement Info
			PNRNameStatementInformationVM.StatementInformationItems = new SelectList(PNRNameStatementInformationRepository.GetPNRNameStatementInformationStatementInformation(id).ToList(), "Value", "Text");

			return View(PNRNameStatementInformationVM);
		}

		// POST: /Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(PNRNameStatementInformationVM PNRNameStatementInformationVM)
		{
			string clientSubUnitGuid = PNRNameStatementInformationVM.ClientSubUnit.ClientSubUnitGuid;

			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

			//Check Exists
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			PNRNameStatementInformation PNRNameStatementInformation = new PNRNameStatementInformation();
			PNRNameStatementInformation = PNRNameStatementInformationVM.PNRNameStatementInformation;
			if (PNRNameStatementInformation == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid) || !hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update  Model from Form
			try
			{
				TryUpdateModel<PNRNameStatementInformation>(PNRNameStatementInformation, "PNRNameStatementInformation");
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

			try
			{
				PNRNameStatementInformationRepository.Add(PNRNameStatementInformationVM);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { id = clientSubUnitGuid });
		}

		// GET: Edit
		public ActionResult Edit(string id, string csu)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToClientSubUnit(csu) && hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			PNRNameStatementInformationVM PNRNameStatementInformationVM = new PNRNameStatementInformationVM();

			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

			//Check clientSubUnit
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);
			PNRNameStatementInformationVM.ClientSubUnit = clientSubUnit;

			ViewData["ClientSubUnitName"] = clientSubUnit.ClientSubUnitDisplayName;
			ViewData["ClientSubUnitGuid"] = clientSubUnit.ClientSubUnitGuid;
			ViewData["ClientTopUnitName"] = clientSubUnit.ClientTopUnitName;
			ViewData["ClientTopUnitGuid"] = clientSubUnit.ClientTopUnitGuid;


			PNRNameStatementInformation PNRNameStatementInformation = new PNRNameStatementInformation();
			PNRNameStatementInformation = PNRNameStatementInformationRepository.GetPNRNameStatementInformation(id);

			//Check PNRNameStatementInformation
			if (PNRNameStatementInformation == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			PNRNameStatementInformation.ClientSubUnit = clientSubUnit;
			PNRNameStatementInformationVM.PNRNameStatementInformation = PNRNameStatementInformation;

			//GDS
			GDSRepository GDSRepository = new GDSRepository();
			PNRNameStatementInformationVM.GDSList = new SelectList(GDSRepository.GetAllGDSsExceptALL().OrderBy(x => x.GDSName).ToList(), "GDSCode", "GDSName", PNRNameStatementInformation.GDSCode);

			//Delimiters
			var delimiters = PNRNameStatementInformationRepository.GetPNRNameStatementInformationDelimiters().ToList();

			PNRNameStatementInformationVM.Delimiter1 = new SelectList(delimiters, "Value", "Text", PNRNameStatementInformation.Delimiter1);
			PNRNameStatementInformationVM.Delimiter2 = new SelectList(delimiters, "Value", "Text", PNRNameStatementInformation.Delimiter2);
			PNRNameStatementInformationVM.Delimiter3 = new SelectList(delimiters, "Value", "Text", PNRNameStatementInformation.Delimiter3);
			PNRNameStatementInformationVM.Delimiter4 = new SelectList(delimiters, "Value", "Text", PNRNameStatementInformation.Delimiter4);
			PNRNameStatementInformationVM.Delimiter5 = new SelectList(delimiters, "Value", "Text", PNRNameStatementInformation.Delimiter5);
			PNRNameStatementInformationVM.Delimiter6 = new SelectList(delimiters, "Value", "Text", PNRNameStatementInformation.Delimiter6);

			//Statement Info
			var PNRNameStatementInformationStatementInformation = PNRNameStatementInformationRepository.GetPNRNameStatementInformationStatementInformation(csu).ToList();

			PNRNameStatementInformationVM.StatementInformationItem1 = new SelectList(PNRNameStatementInformationStatementInformation, "Value", "Text", PNRNameStatementInformation.Field1_ReferToRecordIdentifier);
			PNRNameStatementInformationVM.StatementInformationItem2 = new SelectList(PNRNameStatementInformationStatementInformation, "Value", "Text", PNRNameStatementInformation.Field2_ReferToRecordIdentifier);
			PNRNameStatementInformationVM.StatementInformationItem3 = new SelectList(PNRNameStatementInformationStatementInformation, "Value", "Text", PNRNameStatementInformation.Field3_ReferToRecordIdentifier);
			PNRNameStatementInformationVM.StatementInformationItem4 = new SelectList(PNRNameStatementInformationStatementInformation, "Value", "Text", PNRNameStatementInformation.Field4_ReferToRecordIdentifier);
			PNRNameStatementInformationVM.StatementInformationItem5 = new SelectList(PNRNameStatementInformationStatementInformation, "Value", "Text", PNRNameStatementInformation.Field5_ReferToRecordIdentifier);

			return View(PNRNameStatementInformationVM);
		}

		// POST: /Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(PNRNameStatementInformationVM PNRNameStatementInformationVM)
		{
			string clientSubUnitGuid = PNRNameStatementInformationVM.ClientSubUnit.ClientSubUnitGuid;

			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

			//Check Exists
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "EditPost";
				return View("RecordDoesNotExistError");
			}

			PNRNameStatementInformation PNRNameStatementInformation = new PNRNameStatementInformation();
			PNRNameStatementInformation = PNRNameStatementInformationVM.PNRNameStatementInformation;
			if (PNRNameStatementInformation == null)
			{
				ViewData["ActionMethod"] = "EditPost";
				return View("RecordDoesNotExistError");
			}

			//Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid) || !hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update  Model from Form
			try
			{
				TryUpdateModel<PNRNameStatementInformation>(PNRNameStatementInformation, "PNRNameStatementInformation");
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

			try
			{
				PNRNameStatementInformationRepository.Update(PNRNameStatementInformationVM);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { id = clientSubUnitGuid });
		}

		// GET: Delete
		[HttpGet]
		public ActionResult Delete(string id, string csu)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToClientSubUnit(csu) && hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			PNRNameStatementInformationVM PNRNameStatementInformationVM = new PNRNameStatementInformationVM();

			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

			//Check clientSubUnit
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);
			PNRNameStatementInformationVM.ClientSubUnit = clientSubUnit;

			ViewData["ClientSubUnitName"] = clientSubUnit.ClientSubUnitDisplayName;
			ViewData["ClientSubUnitGuid"] = clientSubUnit.ClientSubUnitGuid;
			ViewData["ClientTopUnitName"] = clientSubUnit.ClientTopUnitName;
			ViewData["ClientTopUnitGuid"] = clientSubUnit.ClientTopUnitGuid;


			PNRNameStatementInformation PNRNameStatementInformation = new PNRNameStatementInformation();
			PNRNameStatementInformation = PNRNameStatementInformationRepository.GetPNRNameStatementInformation(id);

			//Check PNRNameStatementInformation
			if (PNRNameStatementInformation == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			PNRNameStatementInformation.ClientSubUnit = clientSubUnit;
			PNRNameStatementInformationVM.PNRNameStatementInformation = PNRNameStatementInformation;

			return View(PNRNameStatementInformationVM);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(PNRNameStatementInformationVM PNRNameStatementInformationVM)
		{
			string clientSubUnitGuid = PNRNameStatementInformationVM.ClientSubUnit.ClientSubUnitGuid;

			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

			//Check Exists
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			PNRNameStatementInformation PNRNameStatementInformation = new PNRNameStatementInformation();
			PNRNameStatementInformation = PNRNameStatementInformationVM.PNRNameStatementInformation;
			if (PNRNameStatementInformation == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid) || !hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			try
			{
				PNRNameStatementInformationRepository.Delete(PNRNameStatementInformationVM.PNRNameStatementInformation);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { id = clientSubUnitGuid });
		}
	}
}