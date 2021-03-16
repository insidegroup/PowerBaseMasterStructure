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
    public class PriceTrackingSetupGroupItemHotelController : Controller
    {
        //Repositories
        PriceTrackingSetupGroupItemHotelRepository priceTrackingSetupGroupItemHotelRepository = new PriceTrackingSetupGroupItemHotelRepository();
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

            //PriceTrackingSetupGroupItemHotel
            PriceTrackingSetupGroupItemHotel priceTrackingSetupGroupItemHotel = new PriceTrackingSetupGroupItemHotel();
            priceTrackingSetupGroupItemHotel.PriceTrackingSetupGroupId = id;

            //Default Flags
            priceTrackingSetupGroupItemHotel.SharedSavingsFlag = true;
            priceTrackingSetupGroupItemHotel.TransactionFeeFlag = false;

            //Pricing Model Defaults
            priceTrackingSetupGroupItemHotel.SharedSavingsAmount = 30;

            //Threshold Defaults
            priceTrackingSetupGroupItemHotel.EstimatedCWTRebookingFeeAmount = decimal.Parse("0.00"); ;
            priceTrackingSetupGroupItemHotel.CWTVoidRefundFeeAmount = decimal.Parse("0.00");
            priceTrackingSetupGroupItemHotel.ThresholdAmount = decimal.Parse("0.00");

            priceTrackingSetupGroupItemHotel.CalculatedTotalThresholdAmount =
                    priceTrackingSetupGroupItemHotel.EstimatedCWTRebookingFeeAmount +
                    priceTrackingSetupGroupItemHotel.CWTVoidRefundFeeAmount +
                    priceTrackingSetupGroupItemHotel.ThresholdAmount;

            //Hotel Amenity Values Defaults
            priceTrackingSetupGroupItemHotel.RoomTypeUpgradeAllowedFlag = true;
            priceTrackingSetupGroupItemHotel.BeddingTypeUpgradeAllowedFlag = true;
            priceTrackingSetupGroupItemHotel.HotelRateCodeUpgradeAllowedFlag = true;
            priceTrackingSetupGroupItemHotel.NegotiatedUpgradeAllowedFlag = true;
            priceTrackingSetupGroupItemHotel.CancellationPolicyUpgradeAllowedFlag = true;
            priceTrackingSetupGroupItemHotel.KingQueenUpgradeAllowedFlag = true;

            //Rate Codes
            priceTrackingSetupGroupItemHotel.CWTRateTrackingCode1 = "CWT";
            priceTrackingSetupGroupItemHotel.CWTRateTrackingCode2 = "CWV";

            //Admin Only
            priceTrackingSetupGroupItemHotel.AlphaCodeRemarkField = "Y";

            //Currency
            CurrencyRepository currencyRepository = new CurrencyRepository();
            SelectList currencysList = new SelectList(currencyRepository.GetAllCurrencies().ToList(), "CurrencyCode", "Name");
            ViewData["Currencies"] = currencysList;

            //Lists

            ViewData["ClientHasProvidedWrittenApprovalFlagList"] = new SelectList(commonRepository.GetTrueFalseList().ToList(), "Value", "Text", priceTrackingSetupGroupItemHotel.ClientHasProvidedWrittenApprovalFlag);

            ViewData["ClientHasHotelFeesInMSAFlagList"] = new SelectList(commonRepository.GetTrueFalseList().ToList(), "Value", "Text", priceTrackingSetupGroupItemHotel.ClientHasHotelFeesInMSAFlag);

            ViewData["ClientUsesConfermaVirtualCardsFlagList"] = new SelectList(commonRepository.GetTrueFalseList().ToList(), "Value", "Text", priceTrackingSetupGroupItemHotel.ClientUsesConfermaVirtualCardsFlag);

            ViewData["CentralFulfillmentTimeZoneRuleCodes"] = new SelectList(timeZoneRuleRepository.GetAllTimeZoneRules().ToList(), "TimeZoneRuleCode", "TimeZoneRuleCodeDesc");

            ViewData["TimeZoneRules"] = new SelectList(timeZoneRuleRepository.GetAllTimeZoneRules().ToList(), "TimeZoneRuleCode", "TimeZoneRuleCodeDesc");

            ViewData["SharedSavingsList"] = new SelectList(commonRepository.GetTrueFalseList().ToList(), "Value", "Text", priceTrackingSetupGroupItemHotel.SharedSavingsFlag);

            ViewData["TransactionFeeList"] = new SelectList(commonRepository.GetTrueFalseList().ToList(), "Value", "Text", priceTrackingSetupGroupItemHotel.TransactionFeeFlag);

            ViewData["RoomTypeUpgradeAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text", priceTrackingSetupGroupItemHotel.RoomTypeUpgradeAllowedFlag);

            ViewData["BeddingTypeUpgradeAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text", priceTrackingSetupGroupItemHotel.BeddingTypeUpgradeAllowedFlag);

            ViewData["HotelRateCodeUpgradeAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text", priceTrackingSetupGroupItemHotel.HotelRateCodeUpgradeAllowedFlag);

            ViewData["CancellationPolicyUpgradeAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text", priceTrackingSetupGroupItemHotel.CancellationPolicyUpgradeAllowedFlag);

            ViewData["KingQueenUpgradeAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text", priceTrackingSetupGroupItemHotel.KingQueenUpgradeAllowedFlag);

            ViewData["BreakfastChangesAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text", priceTrackingSetupGroupItemHotel.BreakfastChangesAllowedFlag);

            ViewData["EnableValueTrackingFlagList"] = new SelectList(commonRepository.GetTrueFalseList().ToList(), "Value", "Text");

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

            return View(priceTrackingSetupGroupItemHotel);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PriceTrackingSetupGroupItemHotel group, FormCollection collection)
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

            //Capture XML from multiple fields
            group.PriceTrackingSetupGroupItemHotelTrackingAlertEmailAddressesXML = GetPriceTrackingSetupGroupItemHotelTrackingAlertEmailAddressesXML(collection);

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
                priceTrackingSetupGroupItemHotelRepository.Add(group);
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
            PriceTrackingSetupGroupItemHotel group = new PriceTrackingSetupGroupItemHotel();
            group = priceTrackingSetupGroupItemHotelRepository.GetPriceTrackingSetupGroupItemHotel(id);

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

            priceTrackingSetupGroupItemHotelRepository.EditForDisplay(group);

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

            ViewData["ClientHasHotelFeesInMSAFlagList"] = new SelectList(commonRepository.GetTrueFalseList().ToList(), "Value", "Text", group.ClientHasHotelFeesInMSAFlag);

            ViewData["ClientUsesConfermaVirtualCardsFlagList"] = new SelectList(commonRepository.GetTrueFalseList().ToList(), "Value", "Text", group.ClientUsesConfermaVirtualCardsFlag);

            ViewData["CentralFulfillmentTimeZoneRuleCodes"] = new SelectList(timeZoneRuleRepository.GetAllTimeZoneRules().ToList(), "TimeZoneRuleCode", "TimeZoneRuleCodeDesc");

            ViewData["ClientHasHotelFeesInMSAFlagList"] = new SelectList(commonRepository.GetTrueFalseList().ToList(), "Value", "Text", group.ClientHasHotelFeesInMSAFlag);

            ViewData["SharedSavingsList"] = new SelectList(commonRepository.GetTrueFalseList().ToList(), "Value", "Text", group.SharedSavingsFlag);

            ViewData["TransactionFeeList"] = new SelectList(commonRepository.GetTrueFalseList().ToList(), "Value", "Text", group.TransactionFeeFlag);

            ViewData["RoomTypeUpgradeAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text", group.RoomTypeUpgradeAllowedFlag);

            ViewData["BeddingTypeUpgradeAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text", group.BeddingTypeUpgradeAllowedFlag);

            ViewData["HotelRateCodeUpgradeAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text", group.HotelRateCodeUpgradeAllowedFlag);

            ViewData["CancellationPolicyUpgradeAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text", group.CancellationPolicyUpgradeAllowedFlag);

            ViewData["KingQueenUpgradeAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text", group.KingQueenUpgradeAllowedFlag);

            ViewData["BreakfastChangesAllowedFlagList"] = new SelectList(commonRepository.GetAllowedNotAllowedList().ToList(), "Value", "Text", group.BreakfastChangesAllowedFlag);

            ViewData["EnableValueTrackingFlagList"] = new SelectList(commonRepository.GetTrueFalseList().ToList(), "Value", "Text");

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
            PriceTrackingSetupGroupItemHotel group = new PriceTrackingSetupGroupItemHotel();
            group = priceTrackingSetupGroupItemHotelRepository.GetPriceTrackingSetupGroupItemHotel(id);

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

            //Capture XML from multiple fields
            group.PriceTrackingSetupGroupItemHotelTrackingAlertEmailAddressesXML = GetPriceTrackingSetupGroupItemHotelTrackingAlertEmailAddressesXML(collection);

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
                priceTrackingSetupGroupItemHotelRepository.Update(group);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PriceTrackingSetupGroupItemHotel.mvc/Edit/" + group.PriceTrackingSetupGroupItemHotelId.ToString();
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

        private List<PriceTrackingSetupGroupItemHotelTrackingAlertEmailAddress> GetPriceTrackingSetupGroupItemHotelTrackingAlertEmailAddressesXML(FormCollection formCollection)
        {
            List<PriceTrackingSetupGroupItemHotelTrackingAlertEmailAddress> items = new List<PriceTrackingSetupGroupItemHotelTrackingAlertEmailAddress>();

            for (int counter = 0; counter < formCollection.Keys.Count; counter++)
            {
                string emailAddress = string.Format("PriceTrackingSetupGroupItemHotelTrackingAlertEmailAddress[{0}].EmailAddress", counter);

                if (formCollection.AllKeys.Contains(emailAddress))
                {
                    PriceTrackingSetupGroupItemHotelTrackingAlertEmailAddress item = new PriceTrackingSetupGroupItemHotelTrackingAlertEmailAddress();

                    //EmailAddress
                    if (formCollection.AllKeys.Contains(emailAddress) && !string.IsNullOrEmpty(formCollection[emailAddress]))
                    {
                        item.EmailAddress = formCollection[emailAddress];
                    }

                    items.Add(item);
                }
            }

            return items;
        }

    }
}
