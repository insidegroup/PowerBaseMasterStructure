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
    public class CWTCreditCardController : Controller
    {
        //main repository
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
        HierarchyRepository hierarchyRepository = new HierarchyRepository();
        CreditCardRepository creditCardRepository = new CreditCardRepository();
        private string groupName = "CWT Credit Card";


        // GET: /List
		public ActionResult List(int? page, string sortField, int? sortOrder)
        {
			//Sorting
			if (sortField == null || sortField == "CreditCardId")
			{
				sortField = "CreditCardId";
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
			
			//Check Access Rights to Domain
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

            CWTCreditCardsVM cwtCreditCardsVM = new CWTCreditCardsVM();
            cwtCreditCardsVM.CreditCards = creditCardRepository.GetCWTCreditCards(page ?? 1);
            cwtCreditCardsVM.CreditCardBehavior = creditCardRepository.GetCreditCardBehavior();
            return View(cwtCreditCardsVM);
        }

        // GET: /View
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

            //Log View Of Credit Card
            LogRepository logRepository = new LogRepository();
            logRepository.LogApplicationUsage(8, "", "", "", creditCard.CreditCardToken, creditCard.CreditCardTypeId, true);

            creditCardRepository.EditForDisplay(creditCard);
            return View(creditCard);
        }

		//// GET: /Create
		//public ActionResult Create()
		//{
		//	//Check Access Rights to Domain
		//	if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
		//	{
		//		ViewData["Message"] = "You do not have access to this item";
		//		return View("Error");
		//	}

		//	CreditCardTypeRepository creditCardTypeRepository = new CreditCardTypeRepository();
		//	ViewData["CreditCardTypeList"] = new SelectList(creditCardTypeRepository.GetAllCreditCardTypes().ToList(), "CreditCardTypeId", "CreditCardTypeDescription");
		
		//	CreditCardVendorRepository creditCardVendorRepository = new CreditCardVendorRepository();
		//	SelectList creditCardVendorsList = new SelectList(creditCardVendorRepository.GetAllCreditCardVendors().ToList(), "CreditCardVendorCode", "CreditCardVendorName");
		//	ViewData["CreditCardVendors"] = creditCardVendorsList;

		//	TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
		//	SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchies(groupName).ToList(), "HierarchyLevelTableName", "HierarchyLevelTableName");
		//	ViewData["HierarchyTypes"] = hierarchyTypesList;

		//	CreditCard creditCard = new CreditCard();
		//	creditCard.CanHaveRealCreditCardsFlag = creditCardRepository.GetCreditCardBehavior().CanHaveRealCreditCardsFlag;
		//	creditCard.WarningShownFlag = !creditCard.CanHaveRealCreditCardsFlag;

		//	return View(creditCard);
		//}

		//// POST: /Create
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public ActionResult Create(CreditCard group)
		//{
		//	//Check Access Rights to Domain
		//	if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
		//	{
		//		ViewData["Message"] = "You do not have access to this item";
		//		return View("Error");
		//	}

		//	//Check Access Rights to Domain Hierarchy
		//	//if (!hierarchyRepository.AdminHasDomainHierarchyWriteAccess(group.HierarchyType, group.HierarchyCode, group.SourceSystemCode, groupName))
		//	//{
		//	//    ViewData["Message"] = "You cannot add to this hierarchy item";
		//	//    return View("Error");
		//	//}

		//	//Update Model From Form + Validate against DB
		//	try
		//	{
		//		UpdateModel(group);
		//	}
		//	catch
		//	{
		//		string n = "";
		//		foreach (ModelState modelState in ViewData.ModelState.Values)
		//		{
		//			foreach (ModelError error in modelState.Errors)
		//			{
		//				n += error.ErrorMessage;
		//			}
		//		}
		//		ViewData["Message"] = "ValidationError : " + n;
		//		return View("Error");
		//	}

		//	//ClientSubUnitTravelerType has extra field
		//	string hierarchyCode = group.HierarchyCode;
		//	if (group.HierarchyType == "ClientSubUnitTravelerType")
		//	{
		//		group.ClientSubUnitGuid = hierarchyCode;  //ClientSubUnitTravelerType has 2 primarykeys
		//	}

		//	//Database Update
		//	try
		//	{
		//		creditCardRepository.AddCWTCreditCard(group);
		//	}
		//	catch (SqlException ex)
		//	{
		//		LogRepository logRepository = new LogRepository();
		//		logRepository.LogError(ex.Message);

		//		ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
		//		return View("Error");
		//	}
		//	return RedirectToAction("List");
		//}

        //GET: /Edit
        public ActionResult Edit(int id)
        {
            //Check if changes are allowed to Cards for this Database
            CreditCardBehavior creditCardBehavior = new CreditCardBehavior();
            creditCardBehavior = creditCardRepository.GetCreditCardBehavior();
            if(!creditCardBehavior.CanChangeCreditCardsFlag){
                ViewData["ActionMethod"] = "EditGet";
                return View("Error");
            }

            //Get Item From Database
            CreditCard group = new CreditCard();
            group = creditCardRepository.GetCreditCard(id, true);
            
            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }
            //Check Access
            //RolesRepository rolesRepository = new RolesRepository();
            // (!rolesRepository.HasWriteAccessToPolicyGroup(id))
            //{
		    //    ViewData["Message"] = "You do not have access to this item";
            //    return View("Error");
            //}

            CreditCardVendorRepository creditCardVendorRepository = new CreditCardVendorRepository();
            SelectList creditCardVendorsList = new SelectList(creditCardVendorRepository.GetAllCreditCardVendors().ToList(), "CreditCardVendorCode", "CreditCardVendorName");
            ViewData["CreditCardVendors"] = creditCardVendorsList;

            TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
            SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchies(groupName).ToList(), "HierarchyLevelTableName", "HierarchyLevelTableName");
            ViewData["HierarchyTypes"] = hierarchyTypesList;

            //Log View Of Credit Card
            LogRepository logRepository = new LogRepository();
            logRepository.LogApplicationUsage(8, "","", "", group.CreditCardToken, group.CreditCardTypeId, true);

            creditCardRepository.EditForDisplay(group);
            return View(group);
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
            //RolesRepository rolesRepository = new RolesRepository();
            //if (!rolesRepository.HasWriteAccessToPolicyGroup(id))
            //{
             //   ViewData["Message"] = "You do not have access to this item";
             //   return View("Error");
            //}
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

            //ClientSubUnitTravelerType has extra field
            string hierarchyCode = creditCard.HierarchyCode;
            if (creditCard.HierarchyType == "ClientSubUnitTravelerType")
            {
                creditCard.ClientSubUnitGuid = hierarchyCode;  //ClientSubUnitTravelerType has 2 primarykeys
            }
            //Check Access Rights
            HierarchyRepository hierarchyRepository = new HierarchyRepository();
            if (!hierarchyRepository.AdminHasDomainHierarchyWriteAccess(creditCard.HierarchyType, hierarchyCode, creditCard.SourceSystemCode, groupName))
            {
                ViewData["Message"] = "You cannot add to this hierarchy item";
                return View("Error");
            }


            //Database Update
            try
            {
                creditCard.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                creditCardRepository.EditCWTCreditCard(creditCard, collection["OriginalCreditCardNumber"]);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/CWTCreditCard.mvc/Edit/" + creditCard.CreditCardId.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            return RedirectToAction("List");
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

            //Get Item
            CreditCard group = new CreditCard();
            group = creditCardRepository.GetCreditCard(id, false);

            //Check Exists
            if (group == null )
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check Access
           // RolesRepository rolesRepository = new RolesRepository();
            //if (!rolesRepository.HasWriteAccessToPolicyGroup(id))
            //{
            //    ViewData["Message"] = "You do not have access to this item";
             //   return View("Error");
            //}

            //Log View Of Credit Card
            LogRepository logRepository = new LogRepository();
            logRepository.LogApplicationUsage(8, "","", "", group.CreditCardToken, group.CreditCardTypeId, true);


            creditCardRepository.EditForDisplay(group);
            return View(group);
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

            //Get Item From Database
            CreditCard group = new CreditCard();
            group = creditCardRepository.GetCreditCard(id, false);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }
            //Check Access
            //RolesRepository rolesRepository = new RolesRepository();
            //if (!rolesRepository.HasWriteAccessToPolicyGroup(id))
           // {
            //    ViewData["Message"] = "You do not have access to this item";
            //    return View("Error");
            //}

            //Delete Item
            try
            {
                group.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                creditCardRepository.DeleteCWTCreditCard(group);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/CWTCreditCard.mvc/Delete/" + id.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List");
        }
    }
}
