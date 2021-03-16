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
    public class CreditCardClientAccountController : Controller
    {
        CreditCardClientAccountRepository creditCardClientAccountRepository = new CreditCardClientAccountRepository();
        ClientAccountRepository clientAccountRepository = new ClientAccountRepository();
        CreditCardRepository creditCardRepository = new CreditCardRepository();
        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();

        // GET: /ListByClientAccount/
        public ActionResult List(int? page, string can, string ssc, string csu, string sortField, int? sortOrder)
        {
            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Check Exists
            if (clientAccount == null)
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

            //Check AccessRights
            ViewData["Access"] = "";    
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientAccountCreditCards(can, ssc))
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

			ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
			ClientTopUnit clientTopUnit = clientSubUnit.ClientTopUnit;

            ClientAccountCreditCardsVM clientAccountCreditCardsVM = new ClientAccountCreditCardsVM();
			clientAccountCreditCardsVM.CreditCards = creditCardClientAccountRepository.GetCreditCardsByClientAccount(can, ssc, page ?? 1, sortField, sortOrder ?? 0);
            clientAccountCreditCardsVM.ClientAccount = clientAccount;
			clientAccountCreditCardsVM.ClientTopUnit = clientTopUnit;
			clientAccountCreditCardsVM.ClientSubUnit = clientSubUnit;
            clientAccountCreditCardsVM.CreditCardBehavior = creditCardRepository.GetCreditCardBehavior();

            return View(clientAccountCreditCardsVM);
        }

        // GET: /Create
        public ActionResult Create(string can, string ssc, string csu)
        {
            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Check Exists
            if (clientAccount == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            //Check Exists
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientSubUnit.ClientTopUnit;


            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientAccountCreditCards(can, ssc))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            CreditCard creditCard = new CreditCard();
            creditCard.ClientAccountNumber = can;
            creditCard.SourceSystemCode = ssc;
            creditCard.ClientTopUnitGuid = clientTopUnit.ClientTopUnitGuid;
            creditCard.ClientSubUnitGuid = clientSubUnit.ClientSubUnitGuid;
            creditCard.ClientSubUnitName = clientSubUnit.ClientSubUnitName;
            creditCard.ClientTopUnitName = clientTopUnit.ClientTopUnitName;
            creditCard.ClientAccountName = clientAccount.ClientAccountName;
            creditCard.CanHaveRealCreditCardsFlag = creditCardRepository.GetCreditCardBehavior().CanHaveRealCreditCardsFlag;
            creditCard.WarningShownFlag = !creditCard.CanHaveRealCreditCardsFlag;

            CreditCardVendorRepository creditCardVendorRepository = new CreditCardVendorRepository();
            SelectList creditCardVendorsList = new SelectList(creditCardVendorRepository.GetAllCreditCardVendors().ToList(), "CreditCardVendorCode", "CreditCardVendorName", creditCard.CreditCardVendorCode);
            ViewData["CreditCardVendorsList"] = creditCardVendorsList;

            SelectList clientTopUnitsList = new SelectList(clientAccountRepository.GetClientAccountClientTopUnits(can, ssc).ToList(), "ClientTopUnitGuid", "ClientTopUnitName", creditCard.ClientTopUnitGuid);
            ViewData["ClientTopUnitList"] = clientTopUnitsList;

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

            string can = creditCard.ClientAccountNumber;
            string ssc = creditCard.SourceSystemCode;
            string csu = creditCard.ClientSubUnitGuid;

            
            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);


            //Check Exists
            if (clientAccount == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            //Check Exists
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientAccountCreditCards(can, ssc))
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
                creditCardClientAccountRepository.Add(creditCard, can, ssc);

            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { can = can, ssc = ssc, csu = csu });
        }

        // GET: /View
        public ActionResult View(int id, string can, string ssc, string csu)
        {
            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Check Exists
            if (clientAccount == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            CreditCard creditCard = new CreditCard();
            creditCard = creditCardRepository.GetCreditCard(id, false);

            //Check Exists
            if (creditCard == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            //Check Exists
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            creditCard.ClientAccountName = clientAccount.ClientAccountName;
            creditCard.ClientAccountNumber = clientAccount.ClientAccountNumber;
            creditCard.SourceSystemCode = clientAccount.SourceSystemCode;
            creditCard.ClientSubUnitName = clientSubUnit.ClientSubUnitName;

            //Log View Of Credit Card
            LogRepository logRepository = new LogRepository();
            logRepository.LogApplicationUsage(8, clientSubUnit.ClientSubUnitGuid, "", "", creditCard.CreditCardToken, creditCard.CreditCardTypeId, true);


            creditCardRepository.EditForDisplay(creditCard);
            return View(creditCard);

        }

        // GET: /Edit
        public ActionResult Edit(int id, string can, string ssc, string csu)
        {
            //Check if changes are allowed to Cards for this Database
            CreditCardBehavior creditCardBehavior = new CreditCardBehavior();
            creditCardBehavior = creditCardRepository.GetCreditCardBehavior();
            if(!creditCardBehavior.CanChangeCreditCardsFlag){
                ViewData["ActionMethod"] = "EditGet";
                return View("Error");
            }

            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Check Exists
            if (clientAccount == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            CreditCard creditCard = new CreditCard();
            creditCard = creditCardRepository.GetCreditCard(id, true);

            //Check Exists
            if (creditCard == null)
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
            creditCard.ClientAccountName = clientAccount.ClientAccountName;
            creditCard.ClientAccountNumber = clientAccount.ClientAccountNumber;
            creditCard.SourceSystemCode = clientAccount.SourceSystemCode;
            creditCard.ClientSubUnitName = clientSubUnit.ClientSubUnitName;

            CreditCardVendorRepository creditCardVendorRepository = new CreditCardVendorRepository();
            SelectList creditCardVendorsList = new SelectList(creditCardVendorRepository.GetAllCreditCardVendors().ToList(), "CreditCardVendorCode", "CreditCardVendorName", creditCard.CreditCardVendorCode);
            ViewData["CreditCardVendorsList"] = creditCardVendorsList;

            SelectList clientTopUnitsList = new SelectList(clientAccountRepository.GetClientAccountClientTopUnits(can, ssc).ToList(), "ClientTopUnitGuid", "ClientTopUnitName", creditCard.ClientTopUnitGuid);
            ViewData["ClientTopUnitList"] = clientTopUnitsList;

            CreditCardTypeRepository creditCardTypeRepository = new CreditCardTypeRepository();
            SelectList creditCardTypeList = new SelectList(creditCardTypeRepository.GetAllCreditCardTypes().ToList(), "CreditCardTypeId", "CreditCardTypeDescription", creditCard.CreditCardTypeId); ;
            ViewData["CreditCardTypeList"] = creditCardTypeList;

			//Populate List of Products
			ProductRepository productRepository = new ProductRepository();
			SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
			ViewData["ProductList"] = products;
			
			//Log View Of Credit Card
            LogRepository logRepository = new LogRepository();
            logRepository.LogApplicationUsage(8, clientSubUnit.ClientSubUnitGuid, "", "", creditCard.CreditCardToken, creditCard.CreditCardTypeId, true);

            creditCardRepository.EditForDisplay(creditCard);
            return View(creditCard);

        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string can, string ssc, string csu,  FormCollection collection)
        {
             //Check if changes are allowed to Cards for this Database
            CreditCardBehavior creditCardBehavior = new CreditCardBehavior();
            creditCardBehavior = creditCardRepository.GetCreditCardBehavior();
            if(!creditCardBehavior.CanChangeCreditCardsFlag){
                ViewData["ActionMethod"] = "EditPost";
                return View("Error");
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

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            //Check Exists
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientAccountCreditCards(can,ssc))
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
                creditCard.ClientSubUnitGuid = csu;
                creditCard.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                creditCardRepository.Edit(creditCard);

            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/CreditCardClientAccount.mvc/Edit?id=" + id + "&can=" + can + "&ssc=" + ssc ;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            return RedirectToAction("List", new { can = can, ssc = ssc, csu = csu });
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, string can, string ssc, string csu)
        {
             //Check if changes are allowed to Cards for this Database
            CreditCardBehavior creditCardBehavior = new CreditCardBehavior();
            creditCardBehavior = creditCardRepository.GetCreditCardBehavior();
            if(!creditCardBehavior.CanChangeCreditCardsFlag){
                ViewData["ActionMethod"] = "DeleteGet";
                return View("Error");
            }

            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Check Exists
            if (clientAccount == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            CreditCard creditCard = new CreditCard();
            creditCard = creditCardRepository.GetCreditCard(id, false);

            //Check Exists
            if (creditCard == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            //Check Exists
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }
            creditCard.ClientAccountName = clientAccount.ClientAccountName;
            creditCard.ClientAccountNumber = clientAccount.ClientAccountNumber;
            creditCard.SourceSystemCode = clientAccount.SourceSystemCode;
            creditCard.ClientSubUnitName = clientSubUnit.ClientSubUnitName;

            //Log View Of Credit Card
            LogRepository logRepository = new LogRepository();
            logRepository.LogApplicationUsage(8, clientSubUnit.ClientSubUnitGuid, "", "", creditCard.CreditCardToken, creditCard.CreditCardTypeId, true);


            creditCardRepository.EditForDisplay(creditCard);
            return View(creditCard);

        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, string can, string ssc, string csu, FormCollection collection)
        {
            //Check if changes are allowed to Cards for this Database
            CreditCardBehavior creditCardBehavior = new CreditCardBehavior();
            creditCardBehavior = creditCardRepository.GetCreditCardBehavior();
            if(!creditCardBehavior.CanChangeCreditCardsFlag){
                ViewData["ActionMethod"] = "DeletePost";
                return View("Error");
            }

            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Check Exists
            if (clientAccount == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            //Check Exists
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Get Item 
            CreditCard creditCard = new CreditCard();
            creditCard = creditCardRepository.GetCreditCard(id, false);

            //Check Exists
            if (creditCard == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientAccountCreditCards(can, ssc))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                creditCard.ClientSubUnitGuid = csu;
                creditCard.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                creditCardClientAccountRepository.Delete(creditCard);

            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/CreditCardClientAccount.mvc/Delete?id=" + id.ToString() + "&can=" + can + "&ssc=" + ssc;
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            //Return
            return RedirectToAction("List", new { can = can, ssc = ssc, csu = csu });
        }
    }
}
