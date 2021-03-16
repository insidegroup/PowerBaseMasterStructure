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
    public class CreditCardClientSubUnitController : Controller
    {
        CreditCardClientSubUnitRepository creditCardClientSubUnitRepository = new CreditCardClientSubUnitRepository();
        ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
        CreditCardRepository creditCardRepository = new CreditCardRepository();

        // GET: /ListBySubUnit/
        public ActionResult List(int? page, string filter, string id, string sortField, int? sortOrder)
        {
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);
			
			//Check Exists
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "ListBySubUnitGet";
                return View("RecordDoesNotExistError");
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

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientSubUnitCreditCards(id))
            {
                ViewData["Access"] = "WriteAccess";
            }

            ClientSubUnitCreditCardsVM clientSubUnitCreditCardsVM = new ClientSubUnitCreditCardsVM();
			clientSubUnitCreditCardsVM.CreditCards = creditCardClientSubUnitRepository.GetCreditCardsBySubUnit(id, filter ?? "", page ?? 1, sortField, sortOrder ?? 0);

            clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);
            clientSubUnitCreditCardsVM.ClientSubUnit = clientSubUnit;
            clientSubUnitCreditCardsVM.CreditCardBehavior = creditCardRepository.GetCreditCardBehavior();

            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientSubUnit.ClientTopUnitGuid);
            clientSubUnitCreditCardsVM.ClientTopUnit = clientTopUnit;

            return View(clientSubUnitCreditCardsVM);
        }

        // GET: /Create/
        public ActionResult Create(string id)
        {
            //Check Exists
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnitCreditCards(id))
            {
                 ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            CreditCard creditCard = new CreditCard();
            creditCard.ClientSubUnitGuid = clientSubUnit.ClientSubUnitGuid;
            creditCard.ClientSubUnitName = clientSubUnit.ClientSubUnitName;
            creditCard.ClientTopUnitName = clientSubUnit.ClientTopUnit.ClientTopUnitName;
            creditCard.ClientTopUnitGuid = clientSubUnit.ClientTopUnit.ClientTopUnitGuid;
            creditCard.CanHaveRealCreditCardsFlag = creditCardRepository.GetCreditCardBehavior().CanHaveRealCreditCardsFlag;
            creditCard.WarningShownFlag = !creditCard.CanHaveRealCreditCardsFlag;

            CreditCardVendorRepository creditCardVendorRepository = new CreditCardVendorRepository();
            SelectList creditCardVendorsList = new SelectList(creditCardVendorRepository.GetAllCreditCardVendors().ToList(), "CreditCardVendorCode", "CreditCardVendorName", creditCard.CreditCardVendorCode);
            ViewData["CreditCardVendorsList"] = creditCardVendorsList;

            CreditCardTypeRepository creditCardTypeRepository = new CreditCardTypeRepository();
            ViewData["CreditCardTypeList"] = new SelectList(creditCardTypeRepository.GetAllCreditCardTypes().ToList(), "CreditCardTypeId", "CreditCardTypeDescription", creditCard.CreditCardTypeId); ;

			//Populate List of Products
			ProductRepository productRepository = new ProductRepository();
			SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
			ViewData["ProductList"] = products;

            //Show Create Form
            return View(creditCard);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreditCard creditCard, FormCollection collection)
        {

            string clientSubUnitGuid = creditCard.ClientSubUnitGuid;
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

            //Check Exists
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnitCreditCards(clientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(creditCard);
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
                creditCardClientSubUnitRepository.Add(creditCard, clientSubUnitGuid);

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

        public ActionResult View(int id)
        {
            CreditCard creditCard = new CreditCard();
            creditCard = creditCardRepository.GetCreditCard(id, false);

            //Check Exists
            if (creditCard == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            //Check Exists
            CreditCardClientSubUnit creditCardClientSubUnit = new CreditCardClientSubUnit();
            creditCardClientSubUnit = creditCardClientSubUnitRepository.GetCreditCardClientSubUnit(id);

            if (creditCardClientSubUnit == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            //Log View Of Credit Card
            LogRepository logRepository = new LogRepository();
            logRepository.LogApplicationUsage(8, creditCardClientSubUnit.ClientSubUnitGuid, "", "", creditCard.CreditCardToken, creditCard.CreditCardTypeId, true);

            creditCardRepository.EditForDisplay(creditCard);
            return View(creditCard);

        }


        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            //Check if changes are allowed to Cards for this Database
            CreditCardBehavior creditCardBehavior = new CreditCardBehavior();
            creditCardBehavior = creditCardRepository.GetCreditCardBehavior();
            if(!creditCardBehavior.CanChangeCreditCardsFlag){
                ViewData["ActionMethod"] = "DeleteGet";
                return View("Error");
            }

            CreditCard creditCard = new CreditCard();
            creditCard = creditCardRepository.GetCreditCard(id, false);

            //Check Exists
            if (creditCard == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check Exists
            CreditCardClientSubUnit creditCardClientSubUnit = new CreditCardClientSubUnit();
            creditCardClientSubUnit = creditCardClientSubUnitRepository.GetCreditCardClientSubUnit(id);

            if (creditCardClientSubUnit == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Log View Of Credit Card
            LogRepository logRepository = new LogRepository();
            logRepository.LogApplicationUsage(8, creditCardClientSubUnit.ClientSubUnitGuid, "", "", creditCard.CreditCardToken, creditCard.CreditCardTypeId, true);

            creditCardRepository.EditForDisplay(creditCard);
            return View(creditCard);

        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //Check if changes are allowed to Cards for this Database
            CreditCardBehavior creditCardBehavior = new CreditCardBehavior();
            creditCardBehavior = creditCardRepository.GetCreditCardBehavior();
            if(!creditCardBehavior.CanChangeCreditCardsFlag){
                ViewData["ActionMethod"] = "DeletePost";
                return View("Error");
            }

            //Check Exists
            CreditCardClientSubUnit creditCardClientSubUnit = new CreditCardClientSubUnit();
            creditCardClientSubUnit = creditCardClientSubUnitRepository.GetCreditCardClientSubUnit(id);
            if (creditCardClientSubUnit == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnitCreditCards(creditCardClientSubUnit.ClientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            CreditCard creditCard = new CreditCard();
            creditCard = creditCardRepository.GetCreditCard(id, false);

            //Delete Item
            try
            {
                creditCardClientSubUnit.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                creditCardClientSubUnitRepository.Delete(creditCard, creditCardClientSubUnit);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/CreditCardClientSubUnit.mvc/Delete?id=" + id.ToString();
                    return View("VersionError");
                }
				
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);
				
				//Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            
            }

            //Return
            return RedirectToAction("List", new { id = creditCardClientSubUnit.ClientSubUnitGuid });
        }

        // GET: /Edit
        public ActionResult Edit(int id)
        {
            //Check if changes are allowed to Cards for this Database
            CreditCardBehavior creditCardBehavior = new CreditCardBehavior();
            creditCardBehavior = creditCardRepository.GetCreditCardBehavior();
            if(!creditCardBehavior.CanChangeCreditCardsFlag){
                ViewData["ActionMethod"] = "EditGet";
                return View("Error");
            }

            CreditCard creditCard = new CreditCard();
            creditCard = creditCardRepository.GetCreditCard(id, true);

            //Check Exists
            if (creditCard == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check Exists
            CreditCardClientSubUnit creditCardClientSubUnit = new CreditCardClientSubUnit();
            creditCardClientSubUnit = creditCardClientSubUnitRepository.GetCreditCardClientSubUnit(id);

            if (creditCardClientSubUnit == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnitCreditCards(creditCardClientSubUnit.ClientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            CreditCardVendorRepository creditCardVendorRepository = new CreditCardVendorRepository();
            SelectList creditCardVendorsList = new SelectList(creditCardVendorRepository.GetAllCreditCardVendors().ToList(), "CreditCardVendorCode", "CreditCardVendorName", creditCard.CreditCardVendorCode);
            ViewData["CreditCardVendorsList"] = creditCardVendorsList;

            CreditCardTypeRepository creditCardTypeRepository = new CreditCardTypeRepository();
            ViewData["CreditCardTypeList"] = new SelectList(creditCardTypeRepository.GetAllCreditCardTypes().ToList(), "CreditCardTypeId", "CreditCardTypeDescription", creditCard.CreditCardTypeId); ;

			//Populate List of Products
			ProductRepository productRepository = new ProductRepository();
			SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName", creditCard.ProductId);
			ViewData["ProductList"] = products;
			
			//Log View Of Credit Card
            LogRepository logRepository = new LogRepository();
            logRepository.LogApplicationUsage(8, creditCardClientSubUnit.ClientSubUnitGuid, "", "", creditCard.CreditCardToken, creditCard.CreditCardTypeId, true);

            //Show Create Form
            creditCardRepository.EditForDisplay(creditCard);
            return View(creditCard);

        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            //Check if changes are allowed to Cards for this Database
            CreditCardBehavior creditCardBehavior = new CreditCardBehavior();
            creditCardBehavior = creditCardRepository.GetCreditCardBehavior();
            if(!creditCardBehavior.CanChangeCreditCardsFlag){
                ViewData["ActionMethod"] = "EditPost";
                return View("Error");
            }

            //Check Exists
            CreditCardClientSubUnit creditCardClientSubUnit = new CreditCardClientSubUnit();
            creditCardClientSubUnit = creditCardClientSubUnitRepository.GetCreditCardClientSubUnit(id);
            if (creditCardClientSubUnit == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Get Item From Database
            CreditCardEditable creditCard = new CreditCardEditable();
            creditCard = creditCardRepository.GetCreditCardEditable(id, false);

            //Check Exists
            if (creditCard == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnitCreditCards(creditCardClientSubUnit.ClientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            //Update Model From Form + Validate against DB
            try
            {
                UpdateModel(creditCard);
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
                creditCard.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                creditCardRepository.Edit(creditCard);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/CreditCardClientSubUnit.mvc/Edit?id=" + id;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            return RedirectToAction("List", new { id = creditCardClientSubUnit.ClientSubUnitGuid });
        }
    }
}
