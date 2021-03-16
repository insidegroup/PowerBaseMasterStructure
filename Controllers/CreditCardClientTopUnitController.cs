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
    public class CreditCardClientTopUnitController : Controller
    {
        CreditCardClientTopUnitRepository creditCardClientTopUnitRepository = new CreditCardClientTopUnitRepository();
        ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
        CreditCardRepository creditCardRepository = new CreditCardRepository();

        // GET: /List/
        public ActionResult List(int? page, string filter, string id, string sortField, int? sortOrder)
        {
            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(id);

            //Check Exists
            if (clientTopUnit == null)
            {
                ViewData["ActionMethod"] = "ListGet";
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
            if (rolesRepository.HasWriteAccessToClientTopUnitCreditCards(id))
            {
                ViewData["Access"] = "WriteAccess";
            }

            ClientTopUnitCreditCardsVM clientTopUnitCreditCardsVM = new ClientTopUnitCreditCardsVM();
			clientTopUnitCreditCardsVM.CreditCards = creditCardClientTopUnitRepository.GetCreditCardsByTopUnit(id, filter ?? "", page ?? 1, sortField, sortOrder ?? 0);
            clientTopUnitCreditCardsVM.ClientTopUnit = clientTopUnit;
            clientTopUnitCreditCardsVM.CreditCardBehavior = creditCardRepository.GetCreditCardBehavior();

            return View(clientTopUnitCreditCardsVM);
        }

        // GET: /Create/
        public ActionResult Create(string ctu)
        {

            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(ctu);

            //Check Exists
            if (clientTopUnit == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientTopUnitCreditCards(ctu))
            {
                ViewData["Access"] = "WriteAccess";
            }

            ClientTopUnitCreditCardVM clientTopUnitCreditCardVM = new ClientTopUnitCreditCardVM();
            clientTopUnitCreditCardVM.ClientTopUnit = clientTopUnit;

            CreditCard creditCard = new CreditCard();
            creditCard.ClientTopUnitName = clientTopUnit.ClientTopUnitName;
            creditCard.CanHaveRealCreditCardsFlag = creditCardRepository.GetCreditCardBehavior().CanHaveRealCreditCardsFlag;
            creditCard.WarningShownFlag = !creditCard.CanHaveRealCreditCardsFlag;
            clientTopUnitCreditCardVM.CreditCard = creditCard;

            CreditCardTypeRepository creditCardTypeRepository = new CreditCardTypeRepository();
            clientTopUnitCreditCardVM.CreditCardTypes = new SelectList(creditCardTypeRepository.GetAllCreditCardTypes().ToList(), "CreditCardTypeId", "CreditCardTypeDescription");

            CreditCardVendorRepository creditCardVendorRepository = new CreditCardVendorRepository();
            clientTopUnitCreditCardVM.CreditCardVendors = new SelectList(creditCardVendorRepository.GetAllCreditCardVendors().ToList(), "CreditCardVendorCode", "CreditCardVendorName");

			//Populate List of Products
			ProductRepository productRepository = new ProductRepository();
			SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
			ViewData["ProductList"] = products;

            //Show Create Form
            return View(clientTopUnitCreditCardVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientTopUnitCreditCardVM clientTopUnitCreditCardVM)
        {

            string id = clientTopUnitCreditCardVM.ClientTopUnit.ClientTopUnitGuid;

            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(id);

            //Check Exists
            if (clientTopUnit == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientTopUnitCreditCards(id))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Must Add ClientTopUnitGuid to CreditCard
            clientTopUnitCreditCardVM.CreditCard.ClientTopUnitGuid = clientTopUnit.ClientTopUnitGuid;
            clientTopUnitCreditCardVM.CreditCard.ClientTopUnit = clientTopUnit;

            //Update  Model from Form
            try
            {   
                UpdateModel(clientTopUnitCreditCardVM.CreditCard, "CreditCard");
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

            //Add to Database
            try
            {
                creditCardClientTopUnitRepository.Add(clientTopUnitCreditCardVM.CreditCard, id);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = id });
        }

        // GET: /Edit
        public ActionResult Edit(int id, string ctu)
        {
            //Check if changes are allowed to Cards for this Database
            CreditCardBehavior creditCardBehavior = new CreditCardBehavior();
            creditCardBehavior = creditCardRepository.GetCreditCardBehavior();
            if(!creditCardBehavior.CanChangeCreditCardsFlag){
                ViewData["ActionMethod"] = "EditGet";
                return View("Error");
            }

            //Check Exists
            CreditCard creditCard = new CreditCard();
            creditCard = creditCardRepository.GetCreditCard(id, true);

            if (creditCard == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientTopUnitCreditCards(ctu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientTopUnitCreditCardVM clientTopUnitCreditCardVM = new ClientTopUnitCreditCardVM();
            clientTopUnitCreditCardVM.ClientTopUnit = clientTopUnitRepository.GetClientTopUnit(ctu);

            creditCardRepository.EditForDisplay(creditCard);
            clientTopUnitCreditCardVM.CreditCard = creditCard;

            CreditCardTypeRepository creditCardTypeRepository = new CreditCardTypeRepository();
            clientTopUnitCreditCardVM.CreditCardTypes = new SelectList(creditCardTypeRepository.GetAllCreditCardTypes().ToList(), "CreditCardTypeId", "CreditCardTypeDescription", creditCard.CreditCardTypeId);

            CreditCardVendorRepository creditCardVendorRepository = new CreditCardVendorRepository();
            clientTopUnitCreditCardVM.CreditCardVendors = new SelectList(creditCardVendorRepository.GetAllCreditCardVendors().ToList(), "CreditCardVendorCode", "CreditCardVendorName", creditCard.CreditCardVendorCode);

			//Populate List of Products
			ProductRepository productRepository = new ProductRepository();
			SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName", creditCard.ProductId);
			ViewData["ProductList"] = products;

            //Log View Of Credit Card
            LogRepository logRepository = new LogRepository();
            logRepository.LogApplicationUsage(8, "", "", "", creditCard.CreditCardToken, creditCard.CreditCardTypeId, true);

            //Show Edit Form
            return View(clientTopUnitCreditCardVM);

        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientTopUnitCreditCardEditableVM clientTopUnitCreditCardVM, FormCollection collection)
        {
            //Check if changes are allowed to Cards for this Database
            CreditCardBehavior creditCardBehavior = new CreditCardBehavior();
            creditCardBehavior = creditCardRepository.GetCreditCardBehavior();
            if(!creditCardBehavior.CanChangeCreditCardsFlag){
                ViewData["ActionMethod"] = "EditPost";
                return View("Error");
            }

            int creditCardId = clientTopUnitCreditCardVM.CreditCard.CreditCardId;

            //Check Exists
            CreditCardEditable creditCard = new CreditCardEditable();
            creditCard = creditCardRepository.GetCreditCardEditable(creditCardId, false);
            if (creditCard == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            string ctu = clientTopUnitCreditCardVM.ClientTopUnit.ClientTopUnitGuid;

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientTopUnitCreditCards(ctu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(creditCard,"CreditCard");
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
                creditCard.VersionNumber = Int32.Parse(collection["CreditCard.VersionNumber"]);
                creditCardRepository.Edit(creditCard);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/CreditCardClientTopUnit.mvc/Edit?id=" + creditCardId + "&ctu=" + ctu;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            return RedirectToAction("List", new { id = ctu });
        }

        // GET: /View
        public ActionResult View(int id, string ctu)
        {
            //Check Exists
            CreditCard creditCardClientTopUnit = new CreditCard();
            creditCardClientTopUnit = creditCardRepository.GetCreditCard(id, false);

            if (creditCardClientTopUnit == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            ClientTopUnitCreditCardVM clientTopUnitCreditCardVM = new ClientTopUnitCreditCardVM();
            clientTopUnitCreditCardVM.ClientTopUnit = clientTopUnitRepository.GetClientTopUnit(ctu);


            CreditCard creditCard = new CreditCard();
            creditCard = creditCardRepository.GetCreditCard(id, false);
            creditCardRepository.EditForDisplay(creditCard);
            clientTopUnitCreditCardVM.CreditCard = creditCard;

            //Log View Of Credit Card
            LogRepository logRepository = new LogRepository();
            logRepository.LogApplicationUsage(8, "", "", "", creditCard.CreditCardToken, creditCard.CreditCardTypeId, true);

            //Show View Form
            return View(clientTopUnitCreditCardVM);
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, string ctu)
        {
            //Check if changes are allowed to Cards for this Database
            CreditCardBehavior creditCardBehavior = new CreditCardBehavior();
            creditCardBehavior = creditCardRepository.GetCreditCardBehavior();
            if(!creditCardBehavior.CanChangeCreditCardsFlag){
                ViewData["ActionMethod"] = "DeleteGet";
                return View("Error");
            }

            //Check Exists
            CreditCard creditCard = new CreditCard();
            creditCard = creditCardRepository.GetCreditCard(id, false);
            creditCard.ValidateCreditCardNumber = true;
            if (creditCard == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientTopUnitCreditCards(ctu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientTopUnitCreditCardVM clientTopUnitCreditCardVM = new ClientTopUnitCreditCardVM();
            clientTopUnitCreditCardVM.ClientTopUnit = clientTopUnitRepository.GetClientTopUnit(ctu);

            creditCardRepository.EditForDisplay(creditCard);
            clientTopUnitCreditCardVM.CreditCard = creditCard;

            CreditCardTypeRepository creditCardTypeRepository = new CreditCardTypeRepository();
            clientTopUnitCreditCardVM.CreditCardTypes = new SelectList(creditCardTypeRepository.GetAllCreditCardTypes().ToList(), "CreditCardTypeId", "CreditCardTypeDescription", creditCard.CreditCardTypeId);

            CreditCardVendorRepository creditCardVendorRepository = new CreditCardVendorRepository();
            clientTopUnitCreditCardVM.CreditCardVendors = new SelectList(creditCardVendorRepository.GetAllCreditCardVendors().ToList(), "CreditCardVendorCode", "CreditCardVendorName", creditCard.CreditCardVendorCode);

            //Log View Of Credit Card
            LogRepository logRepository = new LogRepository();
            logRepository.LogApplicationUsage(8, "", "", "", creditCard.CreditCardToken, creditCard.CreditCardTypeId, true);

            //Show View Form
            return View(clientTopUnitCreditCardVM);

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
            CreditCard creditCard = new CreditCard();
            creditCard = creditCardRepository.GetCreditCard(id, false);
            if (creditCard == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            string ctu = creditCard.ClientTopUnitGuid;

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientTopUnitCreditCards(ctu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Database Update
            try
            {
                creditCard.VersionNumber = Int32.Parse(collection["CreditCard.VersionNumber"]);
                creditCardClientTopUnitRepository.Delete(creditCard);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/CreditCardClientTopUnit.mvc/Delete?id=" + id + "&ctu=" + ctu;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            return RedirectToAction("List", new { id = ctu });
        }
    }
}
