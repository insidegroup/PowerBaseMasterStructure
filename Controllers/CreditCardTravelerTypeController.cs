using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
	public class CreditCardTravelerTypeController : Controller
	{
		CreditCardTravelerTypeRepository creditCardTravelerTypeRepository = new CreditCardTravelerTypeRepository();
		ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
		ClientSubUnitTravelerTypeRepository clientSubUnitTravelerTypeRepository = new ClientSubUnitTravelerTypeRepository();
		TravelerTypeRepository travelerTypeRepository = new TravelerTypeRepository();
		CreditCardRepository creditCardRepository = new CreditCardRepository();

		// GET: /List/
		public ActionResult List(int? page, string csu, string tt, string filter, string sortField, int? sortOrder)
		{

			ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
			clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

			//Check Exists - Although TT exists independently of CSU, Access Rights are dependent on CSU
			//User can only edit a TT Credit Card if it s linked to a CSU that the user has access to
			//Must check for existance to prevent user changing CSUid in URL to access other TTs
			if (clientSubUnitTravelerType == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("RecordDoesNotExistError");
			}

			//Set Access Rights
			ViewData["Access"] = "";
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToClientSubUnitCreditCards(csu))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//Sorting
			if (sortField == null || sortField == "CreditCardTypeDescription")
			{
				sortField = "CreditCardTypeDescription";
			}
			ViewData["CurrentSortField"] = sortField;

			//Ordering
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

			TravelerTypeCreditCardsVM travelerTypeCreditCardsVM = new TravelerTypeCreditCardsVM();
			travelerTypeCreditCardsVM.CreditCards = creditCardTravelerTypeRepository.GetCreditCards(tt, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

			TravelerType travelerType = new TravelerType();
			travelerType = travelerTypeRepository.GetTravelerType(tt);
			travelerTypeCreditCardsVM.TravelerType = travelerType;

			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
			travelerTypeCreditCardsVM.ClientSubUnit = clientSubUnit;

			ClientTopUnit clientTopUnit = clientSubUnit.ClientTopUnit;
			travelerTypeCreditCardsVM.ClientTopUnit = clientTopUnit;

			//Behavior
			travelerTypeCreditCardsVM.CreditCardBehavior = creditCardRepository.GetCreditCardBehavior();

			return View(travelerTypeCreditCardsVM);
		}

		// GET: /Create/
		public ActionResult Create(string csu, string tt)
		{

			ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
			clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

			//Check Exists - Although TT exists independently of CSU, Access Rights are dependent on CSU
			//User can only edit a TT Credit Card if it s linked to a CSU that the user has access to
			//Must check for existance to prevent user changing CSUid in URL to access other TTs
			if (clientSubUnitTravelerType == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("RecordDoesNotExistError");
			}


			//Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientSubUnitCreditCards(csu))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			TravelerTypeCreditCardVM travelerTypeCreditCardVM = new TravelerTypeCreditCardVM();

			TravelerType travelerType = new TravelerType();
			travelerType = travelerTypeRepository.GetTravelerType(tt);
			travelerTypeCreditCardVM.TravelerType = travelerTypeRepository.GetTravelerType(tt);

			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
			travelerTypeCreditCardVM.ClientSubUnit = clientSubUnit;
			CreditCard creditCard = new CreditCard();

			creditCard.ClientTopUnitName = clientSubUnit.ClientTopUnit.ClientTopUnitName;
			creditCard.ClientSubUnitGuid = clientSubUnit.ClientSubUnitGuid;
			creditCard.CanHaveRealCreditCardsFlag = creditCardRepository.GetCreditCardBehavior().CanHaveRealCreditCardsFlag;
			creditCard.WarningShownFlag = !creditCard.CanHaveRealCreditCardsFlag;
			travelerTypeCreditCardVM.CreditCard = creditCard;

			CreditCardTypeRepository creditCardTypeRepository = new CreditCardTypeRepository();
			travelerTypeCreditCardVM.CreditCardTypes = new SelectList(creditCardTypeRepository.GetAllCreditCardTypes().ToList(), "CreditCardTypeId", "CreditCardTypeDescription");

			CreditCardVendorRepository creditCardVendorRepository = new CreditCardVendorRepository();
			travelerTypeCreditCardVM.CreditCardVendors = new SelectList(creditCardVendorRepository.GetAllCreditCardVendors().ToList(), "CreditCardVendorCode", "CreditCardVendorName");

			//Populate List of Products
			ProductRepository productRepository = new ProductRepository();
			SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
			ViewData["ProductList"] = products;

			//Show Create Form
			return View(travelerTypeCreditCardVM);
		}

		// POST: /Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(TravelerTypeCreditCardVM travelerTypeCreditCardVM)
		{

			string csu = travelerTypeCreditCardVM.ClientSubUnit.ClientSubUnitGuid;
			string tt = travelerTypeCreditCardVM.TravelerType.TravelerTypeGuid;

			ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
			clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

			//Check Exists - Although TT exists independently of CSU, Access Rights are dependent on CSU
			//User can only edit a TT Credit Card if it s linked to a CSU that the user has access to
			//Must check for existance to prevent user changing CSUid in URL to access other TTs
			if (clientSubUnitTravelerType == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("RecordDoesNotExistError");
			}


			//Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientSubUnitCreditCards(csu))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//TravelerType travelerType = new TravelerType();
			//travelerType = travelerTypeRepository.GetTravelerType(tt);

			//Must Add ClientTopUnitGuid to CreditCard
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
			travelerTypeCreditCardVM.CreditCard.ClientTopUnitGuid = clientSubUnit.ClientTopUnitGuid;

			//Add to Database
			try
			{
				creditCardTravelerTypeRepository.Add(travelerTypeCreditCardVM.CreditCard, tt);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { csu = csu, tt = tt });
		}

		// GET: /Edit
		public ActionResult Edit(int id, string csu)
		{
			//Check if changes are allowed to Cards for this Database
			CreditCardBehavior creditCardBehavior = new CreditCardBehavior();
			creditCardBehavior = creditCardRepository.GetCreditCardBehavior();
			if (!creditCardBehavior.CanChangeCreditCardsFlag)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("Error");
			}

			//Check Exists
			CreditCardTravelerType creditCardTravelerType = new CreditCardTravelerType();
			creditCardTravelerType = creditCardTravelerTypeRepository.GetCreditCardTravelerType(id);

			if (creditCardTravelerType == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//string csu = creditCardTravelerType.ClientSubUnitGuid;
			string tt = creditCardTravelerType.TravelerTypeGuid;

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientSubUnitCreditCards(csu))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}


			TravelerTypeCreditCardVM travelerTypeCreditCardVM = new TravelerTypeCreditCardVM();
			travelerTypeCreditCardVM.TravelerType = travelerTypeRepository.GetTravelerType(tt);
			travelerTypeCreditCardVM.ClientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

			CreditCard creditCard = new CreditCard();
			creditCard = creditCardRepository.GetCreditCard(id, true);
			creditCardRepository.EditForDisplay(creditCard);
			travelerTypeCreditCardVM.CreditCard = creditCard;

			CreditCardTypeRepository creditCardTypeRepository = new CreditCardTypeRepository();
			travelerTypeCreditCardVM.CreditCardTypes = new SelectList(creditCardTypeRepository.GetAllCreditCardTypes().ToList(), "CreditCardTypeId", "CreditCardTypeDescription", creditCard.CreditCardTypeId);

			CreditCardVendorRepository creditCardVendorRepository = new CreditCardVendorRepository();
			travelerTypeCreditCardVM.CreditCardVendors = new SelectList(creditCardVendorRepository.GetAllCreditCardVendors().ToList(), "CreditCardVendorCode", "CreditCardVendorName", creditCard.CreditCardVendorCode);

			//Populate List of Products
			ProductRepository productRepository = new ProductRepository();
			SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
			ViewData["ProductList"] = products;

			//Log View Of Credit Card
			LogRepository logRepository = new LogRepository();
			logRepository.LogApplicationUsage(8, csu, tt, "", creditCard.CreditCardToken, creditCard.CreditCardTypeId, true);

			//Show Form

			return View(travelerTypeCreditCardVM);

		}

		// POST: /Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(TravelerTypeCreditCardEditableVM travelerTypeCreditCardVM, FormCollection collection)
		{
			//Check if changes are allowed to Cards for this Database
			CreditCardBehavior creditCardBehavior = new CreditCardBehavior();
			creditCardBehavior = creditCardRepository.GetCreditCardBehavior();
			if (!creditCardBehavior.CanChangeCreditCardsFlag)
			{
				ViewData["ActionMethod"] = "EditPost";
				return View("Error");
			}

			int creditCardId = travelerTypeCreditCardVM.CreditCard.CreditCardId;

			//Check Exists
			CreditCardTravelerType creditCardTravelerType = new CreditCardTravelerType();
			creditCardTravelerType = creditCardTravelerTypeRepository.GetCreditCardTravelerType(creditCardId);
			if (creditCardTravelerType == null)
			{
				ViewData["ActionMethod"] = "EditPost";
				return View("RecordDoesNotExistError");
			}

			string csu = travelerTypeCreditCardVM.ClientSubUnit.ClientSubUnitGuid;
			string tt = travelerTypeCreditCardVM.TravelerType.TravelerTypeGuid;

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientSubUnitCreditCards(csu))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Get Item From Database
			CreditCardEditable creditCard = new CreditCardEditable();
			creditCard = creditCardRepository.GetCreditCardEditable(creditCardId, false);

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<CreditCardEditable>(creditCard, "CreditCard");
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
				creditCard.ClientSubUnitGuid = csu;
				creditCard.TravelerTypeGuid = tt;
				creditCard.VersionNumber = Int32.Parse(collection["CreditCard.VersionNumber"]);
				creditCardRepository.Edit(creditCard);
			}
			catch (SqlException ex)
			{
				//Versioning Error
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/CreditCardTravelerType.mvc/Edit?id=" + creditCardId.ToString() + "&csu=" + csu;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List", new { csu = csu, tt = tt });
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, string csu)
		{
			//Check if changes are allowed to Cards for this Database
			CreditCardBehavior creditCardBehavior = new CreditCardBehavior();
			creditCardBehavior = creditCardRepository.GetCreditCardBehavior();
			if (!creditCardBehavior.CanChangeCreditCardsFlag)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("Error");
			}

			//Check Exists
			CreditCardTravelerType creditCardTravelerType = new CreditCardTravelerType();
			creditCardTravelerType = creditCardTravelerTypeRepository.GetCreditCardTravelerType(id);

			if (creditCardTravelerType == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			//string csu = creditCardTravelerType.ClientSubUnitGuid;
			string tt = creditCardTravelerType.TravelerTypeGuid;

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientSubUnitCreditCards(csu))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}


			TravelerTypeCreditCardVM travelerTypeCreditCardVM = new TravelerTypeCreditCardVM();
			travelerTypeCreditCardVM.TravelerType = travelerTypeRepository.GetTravelerType(tt);
			travelerTypeCreditCardVM.ClientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

			CreditCard creditCard = new CreditCard();
			creditCard = creditCardRepository.GetCreditCard(id, false);
			creditCardRepository.EditForDisplay(creditCard);
			travelerTypeCreditCardVM.CreditCard = creditCard;

			CreditCardTypeRepository creditCardTypeRepository = new CreditCardTypeRepository();
			travelerTypeCreditCardVM.CreditCardTypes = new SelectList(creditCardTypeRepository.GetAllCreditCardTypes().ToList(), "CreditCardTypeId", "CreditCardTypeDescription", creditCard.CreditCardTypeId);

			CreditCardVendorRepository creditCardVendorRepository = new CreditCardVendorRepository();
			travelerTypeCreditCardVM.CreditCardVendors = new SelectList(creditCardVendorRepository.GetAllCreditCardVendors().ToList(), "CreditCardVendorCode", "CreditCardVendorName", creditCard.CreditCardVendorCode);

			//Log View Of Credit Card
			LogRepository logRepository = new LogRepository();
			logRepository.LogApplicationUsage(8, csu, tt, "", creditCard.CreditCardToken, creditCard.CreditCardTypeId, true);

			//Show Form
			return View(travelerTypeCreditCardVM);
		}


		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(TravelerTypeCreditCardVM travelerTypeCreditCardVM, FormCollection collection)
		{
			//Check if changes are allowed to Cards for this Database
			CreditCardBehavior creditCardBehavior = new CreditCardBehavior();
			creditCardBehavior = creditCardRepository.GetCreditCardBehavior();
			if (!creditCardBehavior.CanChangeCreditCardsFlag)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("Error");
			}

			//Check Exists
			int creditCardId = travelerTypeCreditCardVM.CreditCard.CreditCardId;
			CreditCardTravelerType creditCardTravelerType = new CreditCardTravelerType();
			creditCardTravelerType = creditCardTravelerTypeRepository.GetCreditCardTravelerType(creditCardId);

			if (creditCardTravelerType == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			string csu = travelerTypeCreditCardVM.ClientSubUnit.ClientSubUnitGuid;
			string tt = travelerTypeCreditCardVM.TravelerType.TravelerTypeGuid;

			ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
			clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

			//Check Exists
			if (clientSubUnitTravelerType == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientSubUnitCreditCards(csu))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			CreditCard creditCard = new CreditCard();
			creditCard = creditCardRepository.GetCreditCard(creditCardId, false);

			//Delete Item
			try
			{
				creditCard.ClientSubUnitGuid = csu;
				creditCard.TravelerTypeGuid = tt;
				creditCard.VersionNumber = Int32.Parse(collection["CreditCard.VersionNumber"]);
				creditCardTravelerTypeRepository.Delete(creditCard);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/CreditCardTravelerType.mvc/Delete?id=" + creditCardId.ToString() + "&csu=" + csu;
					return View("VersionError");
				}
				//Generic Error
				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { csu = csu, tt = tt });
		}
		// GET: /View
		public ActionResult View(string csu, int id)
		{
			//Check Exists
			CreditCardTravelerType creditCardTravelerType = new CreditCardTravelerType();
			creditCardTravelerType = creditCardTravelerTypeRepository.GetCreditCardTravelerType(id);

			if (creditCardTravelerType == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			string tt = creditCardTravelerType.TravelerTypeGuid;
			ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
			clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

			//Check Exists
			if (clientSubUnitTravelerType == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			TravelerTypeCreditCardVM travelerTypeCreditCardVM = new TravelerTypeCreditCardVM();
			travelerTypeCreditCardVM.TravelerType = travelerTypeRepository.GetTravelerType(tt);
			travelerTypeCreditCardVM.ClientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);


			CreditCard creditCard = new CreditCard();
			creditCard = creditCardRepository.GetCreditCard(id, false);
			creditCardRepository.EditForDisplay(creditCard);
			travelerTypeCreditCardVM.CreditCard = creditCard;

			//Log View Of Credit Card
			LogRepository logRepository = new LogRepository();
			logRepository.LogApplicationUsage(8, csu, tt, "", creditCard.CreditCardToken, creditCard.CreditCardTypeId, true);

			return View(travelerTypeCreditCardVM);
		}
	}
}
