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
    [HandleError]
    public class PriceTrackingSetupGroupItemAirController : Controller
    {
        //Repositories
        PriceTrackingSetupGroupItemAirRepository priceTrackingSetupGroupItemAirRepository = new PriceTrackingSetupGroupItemAirRepository();
        PriceTrackingSetupGroupRepository priceTrackingSetupGroupRepository = new PriceTrackingSetupGroupRepository();
        TimeZoneRuleRepository timeZoneRuleRepository = new TimeZoneRuleRepository();

        HierarchyRepository hierarchyRepository = new HierarchyRepository();
        CommonRepository commonRepository = new CommonRepository();

        private string groupName = "Price Tracking Setup Administrator";
        private string adminGroupName = "Price Tracking Admin Access Administrator";

        // GET: /Create
        public ActionResult Create(int id)
        {
            //Get Item From Database
            PriceTrackingSetupGroup group = new PriceTrackingSetupGroup();
            group = priceTrackingSetupGroupRepository.GetPriceTrackingSetupGroup(id);

            //Check Exists
            if (group == null)
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

            //Set Access Rights to Admin
            ViewData["AdminAccess"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(adminGroupName))
            {
                ViewData["AdminAccess"] = "WriteAccess";
            }

            //PriceTrackingSetupGroup
            ViewData["PriceTrackingSetupGroupId"] = group.PriceTrackingSetupGroupId;
            ViewData["PriceTrackingSetupGroupName"] = group.PriceTrackingSetupGroupName;

            //PriceTrackingSetupGroupItemAir
            PriceTrackingSetupGroupItemAir priceTrackingSetupGroupItemAir = new PriceTrackingSetupGroupItemAir();
            priceTrackingSetupGroupItemAir.PriceTrackingSetupGroupId = id;

            //Default Flags
            priceTrackingSetupGroupItemAir.SharedSavingsFlag = true;
            priceTrackingSetupGroupItemAir.TransactionFeeFlag = false;
            priceTrackingSetupGroupItemAir.RefundableToRefundableWithPenaltyForRefundAllowedFlag = true;
            priceTrackingSetupGroupItemAir.RefundableToNonRefundableAllowedFlag = true;
            priceTrackingSetupGroupItemAir.VoidWindowAllowedFlag = true;
            priceTrackingSetupGroupItemAir.RefundableToRefundableOutsideVoidWindowAllowedFlag = true;
            priceTrackingSetupGroupItemAir.NonRefundableToNonRefundableOutsideVoidWindowAllowedFlag = true;
            priceTrackingSetupGroupItemAir.ExchangesAllowedFlag = true;
            priceTrackingSetupGroupItemAir.VoidExchangeAllowedFlag = true;
            priceTrackingSetupGroupItemAir.NonRefundableToLowerNonRefundableWithDifferentChangeFeeAllowedFlag = true;
            priceTrackingSetupGroupItemAir.RefundableToLowerNonRefundableAllowedFlag = true;
            priceTrackingSetupGroupItemAir.RefundableToLowerRefundableAllowedFlag = true;
            priceTrackingSetupGroupItemAir.NonPenaltyRefundableToLowerPenaltyRefundableAllowedFlag = true;
            priceTrackingSetupGroupItemAir.ChargeChangeFeeUpFrontForSpecificCarriersFlag = true;
            priceTrackingSetupGroupItemAir.ChangeFeeMustBeUsedFromResidualValueFlag = true;
            priceTrackingSetupGroupItemAir.AutomaticReticketingFlag = true;

            //Pricing Model Defaults
            priceTrackingSetupGroupItemAir.SharedSavingsAmount = 30;

            //Switch Window Defaults
            priceTrackingSetupGroupItemAir.RefundableToRefundablePreDepartureDayAmount = 5;
            priceTrackingSetupGroupItemAir.RefundableToNonRefundablePreDepartureDayAmount = 5;

            //Reason Codes
            priceTrackingSetupGroupItemAir.RealisedSavingsCode = "XX";
            priceTrackingSetupGroupItemAir.MissedSavingsCode = "L";

            //Admin Only
            priceTrackingSetupGroupItemAir.AlphaCodeRemarkField = "Y";

            //Currency
            CurrencyRepository currencyRepository = new CurrencyRepository();
            SelectList currencysList = new SelectList(currencyRepository.GetAllCurrencies().ToList(), "CurrencyCode", "Name");
            ViewData["Currencies"] = currencysList;

            //Lists

            ViewData["ClientHasProvidedWrittenApprovalFlagList"] = new SelectList(commonRepository.GetTrueFalseList().ToList(), "Value", "Text", priceTrackingSetupGroupItemAir.ClientHasProvidedWrittenApprovalFlag);

            ViewData["SharedSavingsList"] = new SelectList(commonRepository.GetTrueFalseList().ToList(), "Value", "Text", priceTrackingSetupGroupItemAir.SharedSavingsFlag);

            ViewData["TransactionFeeList"] = new SelectList(commonRepository.GetTrueFalseList().ToList(), "Value", "Text", priceTrackingSetupGroupItemAir.TransactionFeeFlag);

            ViewData["CentralFulfillmentTimeZoneRuleCodes"] = new SelectList(timeZoneRuleRepository.GetAllTimeZoneRules().ToList(), "TimeZoneRuleCode", "TimeZoneRuleCodeDesc");

            ViewData["TimeZoneRules"] = new SelectList(timeZoneRuleRepository.GetAllTimeZoneRules().ToList(), "TimeZoneRuleCode", "TimeZoneRuleCodeDesc");

            ViewData["RefundableToRefundableWithPenaltyForRefundAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text", priceTrackingSetupGroupItemAir.RefundableToRefundableWithPenaltyForRefundAllowedFlag);

            ViewData["RefundableToNonRefundableAllowedFlag"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text", priceTrackingSetupGroupItemAir.RefundableToNonRefundableAllowedFlag);

            ViewData["VoidWindowAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text", priceTrackingSetupGroupItemAir.VoidWindowAllowedFlag);

            ViewData["RefundableToRefundableOutsideVoidWindowAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text", priceTrackingSetupGroupItemAir.RefundableToRefundableOutsideVoidWindowAllowedFlag);

            ViewData["NonRefundableToNonRefundableOutsideVoidWindowAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text", priceTrackingSetupGroupItemAir.NonRefundableToNonRefundableOutsideVoidWindowAllowedFlag);

            ViewData["ExchangesAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text", priceTrackingSetupGroupItemAir.ExchangesAllowedFlag);

            ViewData["VoidExchangeAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text", priceTrackingSetupGroupItemAir.VoidExchangeAllowedFlag);

            ViewData["ExchangePreviousExchangeAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text");

            ViewData["NonRefundableToLowerNonRefundableWithDifferentChangeFeeAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text",
                priceTrackingSetupGroupItemAir.NonRefundableToLowerNonRefundableWithDifferentChangeFeeAllowedFlag);

            ViewData["RefundableToLowerNonRefundableAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text",
                priceTrackingSetupGroupItemAir.RefundableToLowerNonRefundableAllowedFlag);

            ViewData["RefundableToLowerRefundableAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text",
    priceTrackingSetupGroupItemAir.RefundableToLowerRefundableAllowedFlag);

            ViewData["NonPenaltyRefundableToLowerPenaltyRefundableAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text",
    priceTrackingSetupGroupItemAir.NonPenaltyRefundableToLowerPenaltyRefundableAllowedFlag);

            ViewData["ChargeChangeFeeUpFrontForSpecificCarriersFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text",
    priceTrackingSetupGroupItemAir.ChargeChangeFeeUpFrontForSpecificCarriersFlag);

            ViewData["ChangeFeeMustBeUsedFromResidualValueFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text",
    priceTrackingSetupGroupItemAir.ChangeFeeMustBeUsedFromResidualValueFlag);

            ViewData["TrackPrivateNegotiatedFareFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text");

            ViewData["AutomaticReticketingFlagList"] = new SelectList(commonRepository.GetTrueFalseList().ToList(), "Value", "Text", priceTrackingSetupGroupItemAir.AutomaticReticketingFlag);

            //System User
            ViewData["SystemUser"] = "";
            string adminUserGuid = User.Identity.Name.Split(new[] { '|' })[0];
            SystemUserRepository systemUserRepository = new SystemUserRepository();
            SystemUser systemUser = systemUserRepository.GetUserBySystemUserGuid(adminUserGuid);
            if (systemUser != null)
            {
                string username = systemUser.FirstName + " " + systemUser.LastName;
                ViewData["SystemUser"] = string.Format("{0} - {1}", username, adminUserGuid);
            }

            return View(priceTrackingSetupGroupItemAir);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PriceTrackingSetupGroupItemAir group, FormCollection collection)
        {
            //Get Item From Database
            PriceTrackingSetupGroup priceTrackingSetupGroup = new PriceTrackingSetupGroup();
            priceTrackingSetupGroup = priceTrackingSetupGroupRepository.GetPriceTrackingSetupGroup(group.PriceTrackingSetupGroupId);

            //Check Exists
            if (priceTrackingSetupGroup == null)
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

            //Update Model From Form + Validate against DB
            try
            {
                UpdateModel(group);
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
                priceTrackingSetupGroupItemAirRepository.Add(group);
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

            return RedirectToAction(
                "ListUnDeleted", 
                new {
                    id = priceTrackingSetupGroup.PriceTrackingSetupGroupId,
                    controller = "PriceTrackingSetupGroup"
                }
            );
        }

        // GET: /Edit
        public ActionResult Edit(int id)
        {
            //Get Item From Database
            PriceTrackingSetupGroupItemAir group = new PriceTrackingSetupGroupItemAir();
            group = priceTrackingSetupGroupItemAirRepository.GetPriceTrackingSetupGroupItemAir(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Get Item From Database
            PriceTrackingSetupGroup priceTrackingSetupGroup = new PriceTrackingSetupGroup();
            priceTrackingSetupGroup = priceTrackingSetupGroupRepository.GetPriceTrackingSetupGroup(group.PriceTrackingSetupGroupId);

            //Check Exists
            if (priceTrackingSetupGroup == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check Access to PriceTrackingSetupGroup
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPriceTrackingSetupGroup(priceTrackingSetupGroup.PriceTrackingSetupGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            priceTrackingSetupGroupItemAirRepository.EditForDisplay(group);

            //Set Access Rights to Admin
            ViewData["AdminAccess"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(adminGroupName))
            {
                ViewData["AdminAccess"] = "WriteAccess";
            }
            
            //PriceTrackingSetupGroup
            ViewData["PriceTrackingSetupGroupId"] = priceTrackingSetupGroup.PriceTrackingSetupGroupId;
            ViewData["PriceTrackingSetupGroupName"] = priceTrackingSetupGroup.PriceTrackingSetupGroupName;

            //Currency
            CurrencyRepository currencyRepository = new CurrencyRepository();
            SelectList currencysList = new SelectList(currencyRepository.GetAllCurrencies().ToList(), "CurrencyCode", "Name", group.CurrencyCode);
            ViewData["Currencies"] = currencysList;

            //Lists

            ViewData["ClientHasProvidedWrittenApprovalFlagList"] = new SelectList(commonRepository.GetTrueFalseList().ToList(), "Value", "Text", group.ClientHasProvidedWrittenApprovalFlag);

            ViewData["SharedSavingsList"] = new SelectList(commonRepository.GetTrueFalseList().ToList(), "Value", "Text", group.SharedSavingsFlag);

            ViewData["TransactionFeeList"] = new SelectList(commonRepository.GetTrueFalseList().ToList(), "Value", "Text", group.TransactionFeeFlag);

            ViewData["CentralFulfillmentTimeZoneRuleCodes"] = new SelectList(timeZoneRuleRepository.GetAllTimeZoneRules().ToList(), "TimeZoneRuleCode", "TimeZoneRuleCodeDesc", group.CentralFulfillmentTimeZoneRuleCode);

            ViewData["TimeZoneRules"] = new SelectList(timeZoneRuleRepository.GetAllTimeZoneRules().ToList(), "TimeZoneRuleCode", "TimeZoneRuleCodeDesc", group.TimeZoneRuleCode);

            ViewData["RefundableToRefundableWithPenaltyForRefundAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text", group.RefundableToRefundableWithPenaltyForRefundAllowedFlag);

            ViewData["RefundableToNonRefundableAllowedFlag"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text", group.RefundableToNonRefundableAllowedFlag);

            ViewData["VoidWindowAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text", group.VoidWindowAllowedFlag);

            ViewData["RefundableToRefundableOutsideVoidWindowAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text", group.RefundableToRefundableOutsideVoidWindowAllowedFlag);

            ViewData["NonRefundableToNonRefundableOutsideVoidWindowAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text", group.NonRefundableToNonRefundableOutsideVoidWindowAllowedFlag);

            ViewData["ExchangesAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text", group.ExchangesAllowedFlag);

            ViewData["VoidExchangeAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text", group.VoidExchangeAllowedFlag);

			if (group.ExchangePreviousExchangeAllowedFlag == true || group.ExchangePreviousExchangeAllowedFlag == false)
			{
				ViewData["ExchangePreviousExchangeAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text", group.ExchangePreviousExchangeAllowedFlag);
			}
			else
			{
				ViewData["ExchangePreviousExchangeAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text");
			}

            ViewData["NonRefundableToLowerNonRefundableWithDifferentChangeFeeAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text",
                group.NonRefundableToLowerNonRefundableWithDifferentChangeFeeAllowedFlag);

            ViewData["RefundableToLowerNonRefundableAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text",
                group.RefundableToLowerNonRefundableAllowedFlag);

            ViewData["RefundableToLowerRefundableAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text",
    group.RefundableToLowerRefundableAllowedFlag);

            ViewData["NonPenaltyRefundableToLowerPenaltyRefundableAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text",
    group.NonPenaltyRefundableToLowerPenaltyRefundableAllowedFlag);

            ViewData["ChargeChangeFeeUpFrontForSpecificCarriersFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text",
    group.ChargeChangeFeeUpFrontForSpecificCarriersFlag);

            ViewData["ChangeFeeMustBeUsedFromResidualValueFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text",
    group.ChangeFeeMustBeUsedFromResidualValueFlag);

            ViewData["TrackPrivateNegotiatedFareFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text");

            ViewData["AutomaticReticketingFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text", group.AutomaticReticketingFlag);

            //System User
            ViewData["SystemUser"] = group.LastUpdateUserIdentifier != null ? group.LastUpdateUserIdentifier : group.CreationUserIdentifier;

            return View(group);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            //Get Item From Database
            PriceTrackingSetupGroupItemAir group = new PriceTrackingSetupGroupItemAir();
            group = priceTrackingSetupGroupItemAirRepository.GetPriceTrackingSetupGroupItemAir(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPriceTrackingSetupGroup(group.PriceTrackingSetupGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Model From Form + Validate against DB
            try
            {
                UpdateModel(group);
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
                priceTrackingSetupGroupItemAirRepository.Update(group);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PriceTrackingSetupGroupItemAir.mvc/Edit/" + group.PriceTrackingSetupGroupItemAirId.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction(
                "ListUnDeleted",
                new
                {
                    id = group.PriceTrackingSetupGroupId,
                    controller = "PriceTrackingSetupGroup"
                }
            );
        }
    }
}
