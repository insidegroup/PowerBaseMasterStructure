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
using System.Data;
using ClosedXML.Excel;
using System.IO;

namespace CWTDesktopDatabase.Controllers
{
    [HandleError]
    public class PriceTrackingSetupGroupController : Controller
    {
        //main repository
        PriceTrackingSetupGroupRepository priceTrackingSetupGroupRepository = new PriceTrackingSetupGroupRepository();
        HierarchyRepository hierarchyRepository = new HierarchyRepository();
        CommonRepository commonRepository = new CommonRepository();

        private string groupName = "Price Tracking Setup Administrator";
        private string fIQIDGroupName = "Price Tracking FIQID Override Administrator";

        // GET: /ListUnDeleted
        public ActionResult ListUnDeleted(string filter, int? page, string sortField, int? sortOrder)
        {
            //Set Access Rights
            ViewData["Access"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //SortField
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "PriceTrackingSetupGroupName";
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

            if (priceTrackingSetupGroupRepository == null)
            {
                ViewData["ActionMethod"] = "ListUnDeletedGet";
                return View("Error");
            }

            var cwtPaginatedList = priceTrackingSetupGroupRepository.PagePriceTrackingSetupGroups(false, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

            if (cwtPaginatedList == null)
            {
                ViewData["ActionMethod"] = "ListUnDeletedGet";
                return View("Error");
            }

            //return items
            return View(cwtPaginatedList);
        }

        // GET: /ListDeleted
        public ActionResult ListDeleted(string filter, int? page, string sortField, int? sortOrder)
        {
            //Set Access Rights
            ViewData["Access"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //SortField
            if (sortField != "HierarchyType" && sortField != "EnabledDate" && sortField != "LinkedItemCount")
            {
                sortField = "PriceTrackingSetupGroupName";
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

            //return items
            var cwtPaginatedList = priceTrackingSetupGroupRepository.PagePriceTrackingSetupGroups(true, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        // GET: /View
        public ActionResult View(int id)
        {
            PriceTrackingSetupGroup group = new PriceTrackingSetupGroup();
            group = priceTrackingSetupGroupRepository.GetPriceTrackingSetupGroup(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }
            
            priceTrackingSetupGroupRepository.EditForDisplay(group);

            return View(group);
        }

        // GET: /Create
        public ActionResult Create()
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            PriceTrackingSetupGroup priceTrackingSetupGroup = new PriceTrackingSetupGroup();
            priceTrackingSetupGroup.MidOfficeUsedForQCTicketingFlag = true;

            TripTypeRepository tripTypeRepository = new TripTypeRepository();
            SelectList tripTypesList = new SelectList(tripTypeRepository.GetAllTripTypes().ToList(), "TripTypeId", "TripTypeDescription");
            ViewData["TripTypes"] = tripTypesList;

            GDSRepository gdsRepository = new GDSRepository();
            SelectList gDSList = new SelectList(gdsRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName");
            ViewData["GDSs"] = gDSList;

            PNROutputTypeRepository pNROutputTypeRepository = new PNROutputTypeRepository();
            SelectList pNROutputTypeList = new SelectList(pNROutputTypeRepository.GetAllPNROutputTypes().ToList(), "PNROutputTypeId", "PNROutputTypeName");
            ViewData["PNROutputTypes"] = pNROutputTypeList;

            TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
            SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchies(groupName).ToList(), "HierarchyLevelTableName", "HierarchyLevelTableName");
            ViewData["HierarchyTypes"] = hierarchyTypesList;

            PriceTrackingSetupTypeRepository priceTrackingSetupTypeRepository = new PriceTrackingSetupTypeRepository();
            SelectList priceTrackingSetupTypes = new SelectList(priceTrackingSetupTypeRepository.GetAllPriceTrackingSetupTypes().ToList(), "PriceTrackingSetupTypeId", "PriceTrackingSetupTypeName");
            ViewData["PriceTrackingSetupTypes"] = priceTrackingSetupTypes;

            DesktopUsedTypeRepository desktopUsedTypeRepository = new DesktopUsedTypeRepository();
            SelectList desktopUsedTypes = new SelectList(desktopUsedTypeRepository.GetAllDesktopUsedTypes().ToList(), "DesktopUsedTypeId", "DesktopUsedTypeDescription");
            ViewData["DesktopUsedTypes"] = desktopUsedTypes;

            PriceTrackingMidOfficePlatformRepository priceTrackingMidOfficePlatformRepository = new PriceTrackingMidOfficePlatformRepository();
            SelectList priceTrackingMidOfficePlatforms = new SelectList(priceTrackingMidOfficePlatformRepository.GetAllPriceTrackingMidOfficePlatforms().ToList(), "PriceTrackingMidOfficePlatformId", "PriceTrackingMidOfficePlatformName");
            ViewData["PriceTrackingMidOfficePlatforms"] = priceTrackingMidOfficePlatforms;

            ViewData["SharedPseudoCityOrOfficeList"] = new SelectList(commonRepository.GetTrueFalseList().ToList(), "Value", "Text");

            ViewData["MidOfficeUsedForQCTicketingList"] = new SelectList(commonRepository.GetTrueFalseList().ToList(), "Value", "Text", priceTrackingSetupGroup.MidOfficeUsedForQCTicketingFlag);

            ViewData["USGovernmentContractorList"] = new SelectList(commonRepository.GetTrueFalseList().ToList(), "Value", "Text");

            ViewData["PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeIdList"] = new SelectList(commonRepository.GetTrueFalseList().ToList(), "Value", "Text", "false");

            PriceTrackingItinerarySolutionRepository priceTrackingItinerarySolutionRepository = new PriceTrackingItinerarySolutionRepository();
            SelectList priceTrackingItinerarySolutions = new SelectList(priceTrackingItinerarySolutionRepository.GetAllPriceTrackingItinerarySolutions().ToList(), "PriceTrackingItinerarySolutionId", "PriceTrackingItinerarySolutionName");
            ViewData["PriceTrackingItinerarySolutions"] = priceTrackingItinerarySolutions;

            PriceTrackingSystemRuleRepository priceTrackingSystemRuleRepository = new PriceTrackingSystemRuleRepository();
            SelectList priceTrackingSystemRules = new SelectList(priceTrackingSystemRuleRepository.GetAllPriceTrackingSystemRules().ToList(), "PriceTrackingSystemRuleId", "PriceTrackingSystemRuleName");
            ViewData["PriceTrackingSystemRules"] = priceTrackingSystemRules;

            BackOfficeSystemRepository backOfficeSystemRepository = new BackOfficeSystemRepository();
            SelectList backOfficeSystems = new SelectList(backOfficeSystemRepository.GetAllBackOfficeSystems().ToList(), "BackOfficeSytemId", "BackOfficeSystemDescription");
            ViewData["BackOfficeSystems"] = backOfficeSystems;

            PriceTrackingBillingModelRepository priceTrackingBillingModelRepository = new PriceTrackingBillingModelRepository();
            List<PriceTrackingBillingModel> priceTrackingBillingModels = priceTrackingBillingModelRepository.GetAllPriceTrackingBillingModels().ToList();
            SelectList priceTrackingBillingModelsList = new SelectList(priceTrackingBillingModels, "PriceTrackingBillingModelId", "PriceTrackingBillingModelName");
            ViewData["AirPriceTrackingBillingModels"] = priceTrackingBillingModelsList;
            ViewData["HotelPriceTrackingBillingModels"] = priceTrackingBillingModelsList;
            ViewData["PreTicketPriceTrackingBillingModels"] = priceTrackingBillingModelsList;

            return View(priceTrackingSetupGroup);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PriceTrackingSetupGroup group, FormCollection collection)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Check Access Rights to Domain Hierarchy
            if (!hierarchyRepository.AdminHasDomainHierarchyWriteAccess(group.HierarchyType, group.HierarchyCode, group.SourceSystemCode, groupName))
            {
                ViewData["Message"] = "You cannot add to this hierarchy item";
                return View("Error");
            }

            //Capture XML from multiple fields
            group.PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeIdsXML = GetAdditionalPseudoCityOrOfficeIdsXML(collection);
            group.PriceTrackingSetupGroupExcludedTravelerTypesXML = GetPriceTrackingSetupGroupExcludedTravelerTypesXML(collection);

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

            //ClientSubUnitTravelerType has extra field
            string hierarchyCode = group.HierarchyCode;
            if (group.HierarchyType == "ClientSubUnitTravelerType")
            {
                group.ClientSubUnitGuid = hierarchyCode;  //ClientSubUnitTravelerType has 2 primarykeys
            }

            //Database Update
            try
            {
                priceTrackingSetupGroupRepository.Add(group);
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
            return RedirectToAction("ListUnDeleted");
        }

        // GET: /Edit
        public ActionResult Edit(int id)
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

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPriceTrackingSetupGroup(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Set Access Rights
            ViewData["FIQIDAccess"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(fIQIDGroupName))
            {
                ViewData["FIQIDAccess"] = "WriteAccess";
            }

            priceTrackingSetupGroupRepository.EditForDisplay(group);

            TripTypeRepository tripTypeRepository = new TripTypeRepository();
            SelectList tripTypesList = new SelectList(tripTypeRepository.GetAllTripTypes().ToList(), "TripTypeId", "TripTypeDescription");
            ViewData["tripTypes"] = tripTypesList;

            GDSRepository gdsRepository = new GDSRepository();
            SelectList gDSList = new SelectList(gdsRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName");
            ViewData["GDSs"] = gDSList;

            PNROutputTypeRepository pNROutputTypeRepository = new PNROutputTypeRepository();
            SelectList pNROutputTypeList = new SelectList(pNROutputTypeRepository.GetAllPNROutputTypes().ToList(), "PNROutputTypeId", "PNROutputTypeName");
            ViewData["PNROutputTypes"] = pNROutputTypeList;

            TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
            SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchies(groupName).ToList(), "HierarchyLevelTableName", "HierarchyLevelTableName");
            ViewData["HierarchyTypes"] = hierarchyTypesList;

            PriceTrackingSetupTypeRepository priceTrackingSetupTypeRepository = new PriceTrackingSetupTypeRepository();
            SelectList priceTrackingSetupTypes = new SelectList(priceTrackingSetupTypeRepository.GetAllPriceTrackingSetupTypes().ToList(), "PriceTrackingSetupTypeId", "PriceTrackingSetupTypeName", group.PriceTrackingSetupTypeId);
            ViewData["PriceTrackingSetupTypes"] = priceTrackingSetupTypes;

            DesktopUsedTypeRepository desktopUsedTypeRepository = new DesktopUsedTypeRepository();
            SelectList desktopUsedTypes = new SelectList(desktopUsedTypeRepository.GetAllDesktopUsedTypes().ToList(), "DesktopUsedTypeId", "DesktopUsedTypeDescription", group.DesktopUsedTypeId);
            ViewData["DesktopUsedTypes"] = desktopUsedTypes;

            PriceTrackingMidOfficePlatformRepository priceTrackingMidOfficePlatformRepository = new PriceTrackingMidOfficePlatformRepository();
            SelectList priceTrackingMidOfficePlatforms = new SelectList(priceTrackingMidOfficePlatformRepository.GetAllPriceTrackingMidOfficePlatforms().ToList(), "PriceTrackingMidOfficePlatformId", "PriceTrackingMidOfficePlatformName", group.PriceTrackingMidOfficePlatformId);
            ViewData["PriceTrackingMidOfficePlatforms"] = priceTrackingMidOfficePlatforms;

            ViewData["SharedPseudoCityOrOfficeList"] = new SelectList(commonRepository.GetTrueFalseList().ToList(), "Value", "Text", group.SharedPseudoCityOrOfficeIdFlag);

            ViewData["MidOfficeUsedForQCTicketingList"] = new SelectList(commonRepository.GetTrueFalseList().ToList(), "Value", "Text", group.MidOfficeUsedForQCTicketingFlag);

            ViewData["USGovernmentContractorList"] = new SelectList(commonRepository.GetTrueFalseList().ToList(), "Value", "Text", group.USGovernmentContractorFlag);

            ViewData["PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeIdList"] = new SelectList(commonRepository.GetTrueFalseList().ToList(), "Value", "Text", "false");

            PriceTrackingItinerarySolutionRepository priceTrackingItinerarySolutionRepository = new PriceTrackingItinerarySolutionRepository();
            SelectList priceTrackingItinerarySolutions = new SelectList(priceTrackingItinerarySolutionRepository.GetAllPriceTrackingItinerarySolutions().ToList(), "PriceTrackingItinerarySolutionId", "PriceTrackingItinerarySolutionName", group.PriceTrackingItinerarySolutionId);
            ViewData["PriceTrackingItinerarySolutions"] = priceTrackingItinerarySolutions;

            PriceTrackingSystemRuleRepository priceTrackingSystemRuleRepository = new PriceTrackingSystemRuleRepository();
            SelectList priceTrackingSystemRules = new SelectList(priceTrackingSystemRuleRepository.GetAllPriceTrackingSystemRules().ToList(), "PriceTrackingSystemRuleId", "PriceTrackingSystemRuleName", group.PriceTrackingSystemRuleId);
            ViewData["PriceTrackingSystemRules"] = priceTrackingSystemRules;

            BackOfficeSystemRepository backOfficeSystemRepository = new BackOfficeSystemRepository();
            SelectList backOfficeSystems = new SelectList(backOfficeSystemRepository.GetAllBackOfficeSystems().ToList(), "BackOfficeSytemId", "BackOfficeSystemDescription", group.BackOfficeSystemId);
            ViewData["BackOfficeSystems"] = backOfficeSystems;

            PriceTrackingBillingModelRepository priceTrackingBillingModelRepository = new PriceTrackingBillingModelRepository();
            List<PriceTrackingBillingModel> priceTrackingBillingModels = priceTrackingBillingModelRepository.GetAllPriceTrackingBillingModels().ToList();

            SelectList airPriceTrackingBillingModels = new SelectList(priceTrackingBillingModels, "PriceTrackingBillingModelId", "PriceTrackingBillingModelName", group.AirPriceTrackingBillingModelId);
            ViewData["AirPriceTrackingBillingModels"] = airPriceTrackingBillingModels;

            SelectList hotelPriceTrackingBillingModels = new SelectList(priceTrackingBillingModels, "PriceTrackingBillingModelId", "PriceTrackingBillingModelName", group.HotelPriceTrackingBillingModelId);
            ViewData["HotelPriceTrackingBillingModels"] = hotelPriceTrackingBillingModels;

            SelectList preTicketPriceTrackingBillingModels = new SelectList(priceTrackingBillingModels, "PriceTrackingBillingModelId", "PriceTrackingBillingModelName", group.PreTicketPriceTrackingBillingModelId);
            ViewData["PreTicketPriceTrackingBillingModels"] = preTicketPriceTrackingBillingModels;

            return View(group);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
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

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPriceTrackingSetupGroup(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Capture XML from multiple fields
            group.PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeIdsXML = GetAdditionalPseudoCityOrOfficeIdsXML(collection);
            group.PriceTrackingSetupGroupExcludedTravelerTypesXML = GetPriceTrackingSetupGroupExcludedTravelerTypesXML(collection);

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

            //Dont check for multiple as We are not editing Hierarchy, we have alrady checked access to the item itself
            if (group.HierarchyType != "Multiple")
            {
                //ClientSubUnitTravelerType has extra field
                string hierarchyCode = group.HierarchyCode;
                if (group.HierarchyType == "ClientSubUnitTravelerType")
                {
                    group.ClientSubUnitGuid = hierarchyCode;  //ClientSubUnitTravelerType has 2 primarykeys
                }

                //Check Access Rights
                HierarchyRepository hierarchyRepository = new HierarchyRepository();
                if (!hierarchyRepository.AdminHasDomainHierarchyWriteAccess(group.HierarchyType, hierarchyCode, group.SourceSystemCode, groupName))
                {
                    ViewData["Message"] = "You cannot add to this hierarchy item";
                    return View("Error");
                }
            }

            //Database Update
            try
            {
                priceTrackingSetupGroupRepository.Update(group);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PriceTrackingSetupGroup.mvc/Edit/" + group.PriceTrackingSetupGroupId.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("ListUnDeleted");
        }

        private List<PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId> GetAdditionalPseudoCityOrOfficeIdsXML(FormCollection formCollection)
        {
            List<PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId> items = new List<PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId>();

            for (int counter = 0; counter < formCollection.Keys.Count; counter++)
            {
                string pseudoCityOrOfficeId = string.Format("PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId[{0}].PseudoCityOrOfficeId", counter);
                string sharedPseudoCityOrOfficeIdFlag = string.Format("PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId[{0}].SharedPseudoCityOrOfficeIdFlag", counter);

                if (formCollection.AllKeys.Contains(pseudoCityOrOfficeId) && formCollection.AllKeys.Contains(sharedPseudoCityOrOfficeIdFlag))
                {

                    PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId item = new PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId();

                    //PseudoCityOrOfficeId
                    if (formCollection.AllKeys.Contains(pseudoCityOrOfficeId) && !string.IsNullOrEmpty(formCollection[pseudoCityOrOfficeId]))
                    {
                        item.PseudoCityOrOfficeId = formCollection[pseudoCityOrOfficeId];
                    }

                    //SharedPseudoCityOrOfficeIdFlag
                    if (formCollection.AllKeys.Contains(sharedPseudoCityOrOfficeIdFlag) && !string.IsNullOrEmpty(formCollection[sharedPseudoCityOrOfficeIdFlag]))
                    {
                        item.SharedPseudoCityOrOfficeIdFlag = formCollection[sharedPseudoCityOrOfficeIdFlag] == "true";
                    }

                    items.Add(item);
                }
            }

            return items;
        }

        private List<PriceTrackingSetupGroupExcludedTravelerType> GetPriceTrackingSetupGroupExcludedTravelerTypesXML(FormCollection formCollection)
        {
            List<PriceTrackingSetupGroupExcludedTravelerType> items = new List<PriceTrackingSetupGroupExcludedTravelerType>();

            for (int counter = 0; counter < formCollection.Keys.Count; counter++)
            {
                string travelerTypeGuid = string.Format("PriceTrackingSetupGroupExcludedTravelerType[{0}].TravelerTypeGuid", counter);

                if (formCollection.AllKeys.Contains(travelerTypeGuid))
                {

                    PriceTrackingSetupGroupExcludedTravelerType item = new PriceTrackingSetupGroupExcludedTravelerType();

                    //TravelerTypeGuid
                    if (formCollection.AllKeys.Contains(travelerTypeGuid) && !string.IsNullOrEmpty(formCollection[travelerTypeGuid]))
                    {
                        item.TravelerTypeGuid = formCollection[travelerTypeGuid];
                    }

                    items.Add(item);
                }
            }

            return items;
        }

        // GET: /Delete
        [HttpGet]
        public ActionResult Delete(int id)
        {
            //Get Item
            PriceTrackingSetupGroup group = new PriceTrackingSetupGroup();
            group = priceTrackingSetupGroupRepository.GetPriceTrackingSetupGroup(id);

            //Check Exists
            if (group == null || group.DeletedFlag == true)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPriceTrackingSetupGroup(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            priceTrackingSetupGroupRepository.EditForDisplay(group);
            return View(group);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //Get Item From Database
            PriceTrackingSetupGroup group = new PriceTrackingSetupGroup();
            group = priceTrackingSetupGroupRepository.GetPriceTrackingSetupGroup(id);

            //Check Exists
            if (group == null || group.DeletedFlag == true)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }
            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPriceTrackingSetupGroup(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                group.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                group.DeletedFlag = true;
                priceTrackingSetupGroupRepository.UpdateGroupDeletedStatus(group);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PriceTrackingSetupGroup.mvc/Delete/" + id.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("ListUnDeleted");
        }

        // GET: /UnDelete
        public ActionResult UnDelete(int id)
        {
            //Get Item
            PriceTrackingSetupGroup group = new PriceTrackingSetupGroup();
            group = priceTrackingSetupGroupRepository.GetPriceTrackingSetupGroup(id);

            //Check Exists
            if (group == null || group.DeletedFlag == false)
            {
                ViewData["ActionMethod"] = "UnDeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPriceTrackingSetupGroup(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            priceTrackingSetupGroupRepository.EditForDisplay(group);
            return View(group);
        }

        // POST: /UnDelete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnDelete(int id, FormCollection collection)
        {
            //Get Item From Database
            PriceTrackingSetupGroup group = new PriceTrackingSetupGroup();
            group = priceTrackingSetupGroupRepository.GetPriceTrackingSetupGroup(id);

            //Check Exists
            if (group == null || group.DeletedFlag == false)
            {
                ViewData["ActionMethod"] = "UnDeletePost";
                return View("RecordDoesNotExistError");
            }
            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPriceTrackingSetupGroup(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                group.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                group.DeletedFlag = false;
                priceTrackingSetupGroupRepository.UpdateGroupDeletedStatus(group);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PriceTrackingSetupGroup.mvc/UnDelete/" + id.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("ListDeleted");
        }

        // GET: Linked ClientSubUnits
        public ActionResult LinkedClientSubUnits(int id)
        {
            //Get Group From Database
            PriceTrackingSetupGroup group = new PriceTrackingSetupGroup();
            group = priceTrackingSetupGroupRepository.GetPriceTrackingSetupGroup(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "ClientSubUnitGet";
                return View("RecordDoesNotExistError");
            }

            PriceTrackingSetupGroupClientSubUnitCountriesVM priceTrackingSetupGroupClientSubUnits = new PriceTrackingSetupGroupClientSubUnitCountriesVM();
            priceTrackingSetupGroupClientSubUnits.PriceTrackingSetupGroupId = id;
            priceTrackingSetupGroupClientSubUnits.PriceTrackingSetupGroupName = group.PriceTrackingSetupGroupName;

            List<ClientSubUnitCountryVM> clientSubUnitsAvailable = new List<ClientSubUnitCountryVM>();
            clientSubUnitsAvailable = priceTrackingSetupGroupRepository.GetLinkedClientSubUnits(id, false);
            priceTrackingSetupGroupClientSubUnits.ClientSubUnitsAvailable = clientSubUnitsAvailable;

            List<ClientSubUnitCountryVM> clientSubUnitsUnAvailable = new List<ClientSubUnitCountryVM>();
            clientSubUnitsUnAvailable = priceTrackingSetupGroupRepository.GetLinkedClientSubUnits(id, true);
            priceTrackingSetupGroupClientSubUnits.ClientSubUnitsUnAvailable = clientSubUnitsUnAvailable;


            return View(priceTrackingSetupGroupClientSubUnits);
        }

        // POST: Linked ClientSubUnits
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkedClientSubUnits(int PriceTrackingSetupGroupId, string ClientSubUnitGuid)
        {
            //Get Item From Database
            PriceTrackingSetupGroup group = new PriceTrackingSetupGroup();
            group = priceTrackingSetupGroupRepository.GetPriceTrackingSetupGroup(PriceTrackingSetupGroupId);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "ClientSubUnitPost";
                return View("RecordDoesNotExistError");
            }
            //Check Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPriceTrackingSetupGroup(PriceTrackingSetupGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Database Update
            try
            {
                priceTrackingSetupGroupRepository.UpdateLinkedClientSubUnit(PriceTrackingSetupGroupId, ClientSubUnitGuid);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PriceTrackingSetupGroup.mvc/ClientSubUnit/" + PriceTrackingSetupGroupId.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("LinkedClientSubUnits", new { id = PriceTrackingSetupGroupId });
        }

        // POST: AutoComplete ClientAccounts
        [HttpPost]
        public JsonResult AutoCompleteClientAccounts(string searchText, string hierarchyType, string hierarchyItem, int priceTrackingSetupGroupId = 0)
        {
            return Json(priceTrackingSetupGroupRepository.AutoCompleteClientAccounts(searchText, hierarchyType, hierarchyItem, priceTrackingSetupGroupId));
        }

        // POST: AutoComplete TravelerTypes
        [HttpPost]
        public JsonResult AutoCompleteTravelerTypes(string searchText, string hierarchyType, string hierarchyItem, int priceTrackingSetupGroupId = 0)
        {
            return Json(priceTrackingSetupGroupRepository.AutoCompleteTravelerTypes(searchText, hierarchyType, hierarchyItem, priceTrackingSetupGroupId));
        }

        // POST: AutoComplete ClientSubUnitTravelerTypes
        [HttpPost]
        public JsonResult AutoCompleteClientSubUnitTravelerTypes(string searchText, string hierarchyItem, string filterText)
        {
            HierarchyRepository hierarchyRepository = new HierarchyRepository();

            int maxResults = 15;
            var result = hierarchyRepository.LookUpSystemUserClientSubUnitTravelerTypes(searchText, hierarchyItem, maxResults, groupName, filterText);
            return Json(result);
        }

        // GET: /Export
        public ActionResult Export(int id)
        {
            //Get Item From Database
            PriceTrackingSetupGroup priceTrackingSetupGroup = new PriceTrackingSetupGroup();
            priceTrackingSetupGroup = priceTrackingSetupGroupRepository.GetPriceTrackingSetupGroup(id);

            //Check Exists
            if (priceTrackingSetupGroup == null)
            {
                ViewData["ActionMethod"] = "ExportGet";
                return View("RecordDoesNotExistError");
            }

            priceTrackingSetupGroupRepository.EditForDisplay(priceTrackingSetupGroup);

            //Export to Excel File
            DataSet priceTrackingSetupGroupExcelData = priceTrackingSetupGroupRepository.GetPriceTrackingSetupGroupExcelData(priceTrackingSetupGroup.PriceTrackingSetupGroupId);
            using (XLWorkbook workbook = new XLWorkbook())
            {
                workbook.Worksheets.Add(priceTrackingSetupGroupExcelData);
                workbook.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                workbook.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", string.Format("attachment;filename= {0}.xlsx", priceTrackingSetupGroup.PriceTrackingSetupGroupName));
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    workbook.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                }
            }

            Response.End();
            return View();
        }
    }
}
