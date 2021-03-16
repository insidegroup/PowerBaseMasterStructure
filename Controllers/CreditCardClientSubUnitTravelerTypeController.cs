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
    public class CreditCardClientSubUnitTravelerTypeController : Controller
    {
        CreditCardClientSubUnitTravelerTypeRepository creditCardClientSubUnitTravelerTypeRepository = new CreditCardClientSubUnitTravelerTypeRepository();
        ClientSubUnitTravelerTypeRepository clientSubUnitTravelerTypeRepository = new ClientSubUnitTravelerTypeRepository();
        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
        TravelerTypeRepository travelerTypeRepository = new TravelerTypeRepository();
        CreditCardRepository creditCardRepository = new CreditCardRepository();

        // GET: /ListBySubUnit/
		public ActionResult List(int? page, string csu, string tt, string filter, string sortField, int? sortOrder)
        {
            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu,tt);

            //Check Exists
            if (clientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "ListBySubUnitGet";
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

            ClientSubUnitTravelerTypeCreditCardsVM clientSubUnitTravelerTypeCreditCardsVM = new ClientSubUnitTravelerTypeCreditCardsVM();
			clientSubUnitTravelerTypeCreditCardsVM.CreditCards = creditCardClientSubUnitTravelerTypeRepository.GetCreditCardsBySubUnit(csu, tt, page ?? 1, sortField, sortOrder ?? 0);

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            clientSubUnitTravelerTypeCreditCardsVM.ClientSubUnit = clientSubUnit;

			ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
			ClientTopUnit clientTopUnit = clientSubUnit.ClientTopUnit;
			clientSubUnitTravelerTypeCreditCardsVM.ClientTopUnit = clientTopUnit;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            clientSubUnitTravelerTypeCreditCardsVM.TravelerType = travelerType;

            //Behavior
            clientSubUnitTravelerTypeCreditCardsVM.CreditCardBehavior = creditCardRepository.GetCreditCardBehavior();

            return View(clientSubUnitTravelerTypeCreditCardsVM);
        }
        
        // GET: /Create/
        public ActionResult Create(string csu, string tt)
        {
            //Check Exists
            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu,tt);
            if (clientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnitCreditCards(csu))
            {
                 ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientSubUnitTravelerTypeCreditCardVM clientSubUnitTravelerTypeCreditCardVM = new ClientSubUnitTravelerTypeCreditCardVM();
            clientSubUnitTravelerTypeCreditCardVM.TravelerType = travelerTypeRepository.GetTravelerType(tt);
            clientSubUnitTravelerTypeCreditCardVM.ClientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            CreditCard creditCard = new CreditCard();

            creditCard.ClientTopUnitName = clientSubUnit.ClientTopUnit.ClientTopUnitName;
            creditCard.CanHaveRealCreditCardsFlag = creditCardRepository.GetCreditCardBehavior().CanHaveRealCreditCardsFlag;
            creditCard.WarningShownFlag = !creditCard.CanHaveRealCreditCardsFlag;
            clientSubUnitTravelerTypeCreditCardVM.CreditCard = creditCard;

            CreditCardTypeRepository creditCardTypeRepository = new CreditCardTypeRepository();
            clientSubUnitTravelerTypeCreditCardVM.CreditCardTypes = new SelectList(creditCardTypeRepository.GetAllCreditCardTypes().ToList(), "CreditCardTypeId", "CreditCardTypeDescription");

            CreditCardVendorRepository creditCardVendorRepository = new CreditCardVendorRepository();
            clientSubUnitTravelerTypeCreditCardVM.CreditCardVendors = new SelectList(creditCardVendorRepository.GetAllCreditCardVendors().ToList(), "CreditCardVendorCode", "CreditCardVendorName");

			//Populate List of Products
			ProductRepository productRepository = new ProductRepository();
			SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
			ViewData["ProductList"] = products;

			//Show Create Form
            return View(clientSubUnitTravelerTypeCreditCardVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientSubUnitTravelerTypeCreditCardVM clientSubUnitTravelerTypeCreditCardVM)
        {

            string csu = clientSubUnitTravelerTypeCreditCardVM.ClientSubUnit.ClientSubUnitGuid;
            string tt = clientSubUnitTravelerTypeCreditCardVM.TravelerType.TravelerTypeGuid;

            //Check Exists
            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);
            if (clientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnitCreditCards(csu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Must Add ClientTopUnitGuid to CreditCard
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            clientSubUnitTravelerTypeCreditCardVM.CreditCard.ClientTopUnitGuid = clientSubUnit.ClientTopUnitGuid;

            //Add to Database
            try
            {
                creditCardClientSubUnitTravelerTypeRepository.Add(clientSubUnitTravelerTypeCreditCardVM.CreditCard, csu, tt);
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
        public ActionResult Edit(int id)
        {     
            //Check if changes are allowed to Cards for this Database
            CreditCardBehavior creditCardBehavior = new CreditCardBehavior();
            creditCardBehavior = creditCardRepository.GetCreditCardBehavior();
            if(!creditCardBehavior.CanChangeCreditCardsFlag){
                ViewData["ActionMethod"] = "EditGet";
                return View("Error");
            }

            //Check Exists
            CreditCardClientSubUnitTravelerType creditCardClientSubUnitTravelerType = new CreditCardClientSubUnitTravelerType();
            creditCardClientSubUnitTravelerType = creditCardClientSubUnitTravelerTypeRepository.GetCreditCardClientSubUnitTravelerType(id);

            if (creditCardClientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            string csu = creditCardClientSubUnitTravelerType.ClientSubUnitGuid;
            string tt = creditCardClientSubUnitTravelerType.TravelerTypeGuid;

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnitCreditCards(csu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            ClientSubUnitTravelerTypeCreditCardVM clientSubUnitTravelerTypeCreditCardVM = new ClientSubUnitTravelerTypeCreditCardVM();
            clientSubUnitTravelerTypeCreditCardVM.TravelerType = travelerTypeRepository.GetTravelerType(tt);
            clientSubUnitTravelerTypeCreditCardVM.ClientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            CreditCard creditCard = new CreditCard();
            creditCard = creditCardRepository.GetCreditCard(id, true);
            creditCardRepository.EditForDisplay(creditCard);        
            clientSubUnitTravelerTypeCreditCardVM.CreditCard = creditCard;

            CreditCardTypeRepository creditCardTypeRepository = new CreditCardTypeRepository();
            clientSubUnitTravelerTypeCreditCardVM.CreditCardTypes = new SelectList(creditCardTypeRepository.GetAllCreditCardTypes().ToList(), "CreditCardTypeId", "CreditCardTypeDescription",creditCard.CreditCardTypeId);

            CreditCardVendorRepository creditCardVendorRepository = new CreditCardVendorRepository();
            clientSubUnitTravelerTypeCreditCardVM.CreditCardVendors = new SelectList(creditCardVendorRepository.GetAllCreditCardVendors().ToList(), "CreditCardVendorCode", "CreditCardVendorName", creditCard.CreditCardVendorCode);

			//Populate List of Products
			ProductRepository productRepository = new ProductRepository();
			SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
			ViewData["ProductList"] = products;

			//Log View Of Credit Card
            LogRepository logRepository = new LogRepository();
            logRepository.LogApplicationUsage(8, csu, tt, "", creditCard.CreditCardToken, creditCard.CreditCardTypeId, true);

            //Show Form
            return View(clientSubUnitTravelerTypeCreditCardVM);

        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientSubUnitTravelerTypeCreditCardEditableVM clientSubUnitTravelerTypeCreditCardVM, FormCollection collection)
        {
            //Check if changes are allowed to Cards for this Database
            CreditCardBehavior creditCardBehavior = new CreditCardBehavior();
            creditCardBehavior = creditCardRepository.GetCreditCardBehavior();
            if(!creditCardBehavior.CanChangeCreditCardsFlag){
                ViewData["ActionMethod"] = "EditPost";
                return View("Error");
            }

            int creditCardId = clientSubUnitTravelerTypeCreditCardVM.CreditCard.CreditCardId;

            //Check Exists
            CreditCardClientSubUnitTravelerType creditCardClientSubUnitTravelerType = new CreditCardClientSubUnitTravelerType();
            creditCardClientSubUnitTravelerType = creditCardClientSubUnitTravelerTypeRepository.GetCreditCardClientSubUnitTravelerType(creditCardId);
            if (creditCardClientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            string csu = clientSubUnitTravelerTypeCreditCardVM.ClientSubUnit.ClientSubUnitGuid;
            string tt = clientSubUnitTravelerTypeCreditCardVM.TravelerType.TravelerTypeGuid;

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnitCreditCards(csu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Get Item From Database
            CreditCardEditable creditCard = new CreditCardEditable();
            creditCard = creditCardRepository.GetCreditCardEditable(creditCardId, true);

            //Update Model From Form + Validate against DB
            try
            {
                UpdateModel<CreditCardEditable>(creditCard,"CreditCard");
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
                    ViewData["ReturnURL"] = "/CreditCardClientSubUnitTravelerType.mvc/Edit?id=" + creditCardId;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            return RedirectToAction("List", new { csu = csu, tt=tt });
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

            //Check Exists
            CreditCardClientSubUnitTravelerType creditCardClientSubUnitTravelerType = new CreditCardClientSubUnitTravelerType();
            creditCardClientSubUnitTravelerType = creditCardClientSubUnitTravelerTypeRepository.GetCreditCardClientSubUnitTravelerType(id);

            if (creditCardClientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            string csu = creditCardClientSubUnitTravelerType.ClientSubUnitGuid;
            string tt = creditCardClientSubUnitTravelerType.TravelerTypeGuid;

            ClientSubUnitTravelerTypeCreditCardVM clientSubUnitTravelerTypeCreditCardVM = new ClientSubUnitTravelerTypeCreditCardVM();
            clientSubUnitTravelerTypeCreditCardVM.TravelerType = travelerTypeRepository.GetTravelerType(tt);
            clientSubUnitTravelerTypeCreditCardVM.ClientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            CreditCard creditCard = new CreditCard();
            creditCard = creditCardRepository.GetCreditCard(id, false);
            creditCardRepository.EditForDisplay(creditCard);
            clientSubUnitTravelerTypeCreditCardVM.CreditCard = creditCard;

            //Log View Of Credit Card
            LogRepository logRepository = new LogRepository();
            logRepository.LogApplicationUsage(8, csu, tt, "", creditCard.CreditCardToken, creditCard.CreditCardTypeId, true);


            return View(clientSubUnitTravelerTypeCreditCardVM);
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
            CreditCardClientSubUnitTravelerType creditCardClientSubUnitTravelerType = new CreditCardClientSubUnitTravelerType();
            creditCardClientSubUnitTravelerType = creditCardClientSubUnitTravelerTypeRepository.GetCreditCardClientSubUnitTravelerType(id);

            if (creditCardClientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            string csu = creditCardClientSubUnitTravelerType.ClientSubUnitGuid;
            string tt = creditCardClientSubUnitTravelerType.TravelerTypeGuid;

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnitCreditCards(csu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            CreditCard creditCard = new CreditCard();
            creditCard = creditCardRepository.GetCreditCard(id, false);

            //Delete Item
            try
            {
                creditCard.VersionNumber = Int32.Parse(collection["CreditCard.VersionNumber"]);
                creditCardClientSubUnitTravelerTypeRepository.Delete(creditCard);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/CreditCardClientSubUnitTravelerType.mvc/Delete/" + id.ToString();
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", new { csu = csu, tt = tt });
        }
        // GET: /View
        public ActionResult View(int id)
        {
            //Check Exists
            CreditCardClientSubUnitTravelerType creditCardClientSubUnitTravelerType = new CreditCardClientSubUnitTravelerType();
            creditCardClientSubUnitTravelerType = creditCardClientSubUnitTravelerTypeRepository.GetCreditCardClientSubUnitTravelerType(id);

            if (creditCardClientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            string csu = creditCardClientSubUnitTravelerType.ClientSubUnitGuid;
            string tt = creditCardClientSubUnitTravelerType.TravelerTypeGuid;

            ClientSubUnitTravelerTypeCreditCardVM clientSubUnitTravelerTypeCreditCardVM = new ClientSubUnitTravelerTypeCreditCardVM();
            clientSubUnitTravelerTypeCreditCardVM.TravelerType = travelerTypeRepository.GetTravelerType(tt);
            clientSubUnitTravelerTypeCreditCardVM.ClientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);


            CreditCard creditCard = new CreditCard();
            creditCard = creditCardRepository.GetCreditCard(id, false);
            creditCardRepository.EditForDisplay(creditCard);
            clientSubUnitTravelerTypeCreditCardVM.CreditCard = creditCard;

            //Log View Of Credit Card
            LogRepository logRepository = new LogRepository();
            logRepository.LogApplicationUsage(8, csu, tt, "", creditCard.CreditCardToken, creditCard.CreditCardTypeId, true);


            return View(clientSubUnitTravelerTypeCreditCardVM);
        }

      
    }
}
