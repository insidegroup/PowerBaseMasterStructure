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
    public class PolicyGroupController : Controller
    {
        //main repository
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
        HierarchyRepository hierarchyRepository = new HierarchyRepository();
        private string groupName = "Policy Hierarchy";

        // GET: /ListSubMenu
        public ActionResult ListSubMenu(int id)
        {
            PolicyGroupSubMenuVM policyGroupSubMenuVM = new PolicyGroupSubMenuVM();
            //policyGroupSubMenuVM = policyGroupRepository.CreateSubMenuViewModel(id);

			PolicyGroup group = new PolicyGroup();
			group = policyGroupRepository.GetGroup(id);

			//Check Exists
			if (group == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			} 
			
			policyGroupSubMenuVM.PolicyTypes = policyGroupRepository.GetPolicyTypes(id);
			policyGroupSubMenuVM.PolicyGroup = group;

            return View(policyGroupSubMenuVM);
        }

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
            if (sortField !="HierarchyType" && sortField !="EnabledDate" && sortField !="LinkedItemCount")
            {
                sortField = "PolicyGroupName";
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

			if (policyGroupRepository == null)
			{
				ViewData["ActionMethod"] = "ListUnDeletedGet";
				return View("Error");
			}

			var cwtPaginatedList = policyGroupRepository.PagePolicyGroups(false, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

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
            if (sortField != "HierarchyType" && sortField != "EnabledDate" && sortField !="LinkedItemCount")
            {
                sortField = "PolicyGroupName";
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
            var cwtPaginatedList = policyGroupRepository.PagePolicyGroups(true, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        // GET: /ListOrphaned
        public ActionResult ListOrphaned(string filter, int? page, string sortField, int? sortOrder)
        {
            //Set Access Rights
            ViewData["Access"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //SortField
            sortField = "PolicyGroupName";
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
            var cwtPaginatedList = policyGroupRepository.PageOrphanedPolicyGroups(  page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        // GET: /View
        public ActionResult View(int id)
        {
            PolicyGroup group = new PolicyGroup();
            group = policyGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            policyGroupRepository.EditGroupForDisplay(group);
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

			List<Meeting> meetings = new List<Meeting>();
			SelectList meetingsList = new SelectList(meetings.ToList(), "MeetingID", "MeetingName");
			ViewData["Meetings"] = meetingsList;
			
			PolicyGroup group = new PolicyGroup();
            return View(group);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicyGroup group)
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
                policyGroupRepository.Add(group);
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
            PolicyGroup group = new PolicyGroup();
            group = policyGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }
            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(id))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
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

			policyGroupRepository.EditGroupForDisplay(group);

			//Meetings
			MeetingRepository meetingRepository = new MeetingRepository();
			List<Meeting> meetings = meetingRepository.GetAvailableMeetings(group.HierarchyType, group.HierarchyCode, null, group.SourceSystemCode, group.TravelerTypeGuid);
			SelectList meetingsList = new SelectList(meetings.ToList(), "MeetingID", "MeetingDisplayName", group.MeetingID != null ? group.MeetingID.ToString() : "");
			ViewData["Meetings"] = meetingsList;

            return View(group);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            //Get Item From Database
            PolicyGroup group = new PolicyGroup();
            group = policyGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(id))
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
                policyGroupRepository.Edit(group);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyGroup.mvc/Edit/" + group.PolicyGroupId.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("ListUnDeleted");
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            //Get Item
            PolicyGroup group = new PolicyGroup();
            group = policyGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null || group.DeletedFlag == true)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(id))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            policyGroupRepository.EditGroupForDisplay(group);
            return View(group);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //Get Item From Database
            PolicyGroup group = new PolicyGroup();
            group = policyGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null || group.DeletedFlag == true)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }
            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(id))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                group.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                group.DeletedFlag = true;
                policyGroupRepository.UpdateGroupDeletedStatus(group);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyGroup.mvc/Delete/" + id.ToString();
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
            PolicyGroup group = new PolicyGroup();
            group = policyGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null || group.DeletedFlag == false)
            {
                ViewData["ActionMethod"] = "UnDeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(id))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            policyGroupRepository.EditGroupForDisplay(group);
            return View(group);
        }

        // POST: /UnDelete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnDelete(int id, FormCollection collection)
        {
            //Get Item From Database
            PolicyGroup group = new PolicyGroup();
            group = policyGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null || group.DeletedFlag == false)
            {
                ViewData["ActionMethod"] = "UnDeletePost";
                return View("RecordDoesNotExistError");
            }
            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(id))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                group.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                group.DeletedFlag = false;
                policyGroupRepository.UpdateGroupDeletedStatus(group);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyGroup.mvc/UnDelete/" + id.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            } 

            return RedirectToAction("ListDeleted");
        }

        // GET: /View
        /*public ActionResult EditSequence(string type, int? page)
        {
             PolicyGroupSequenceRepository  policyGroupSequenceRepository = new PolicyGroupSequenceRepository();
             var result = policyGroupSequenceRepository.GetPolicyGroupCountrySequences(page ?? 1);

             ViewData["Page"] = page ?? 1;
             ViewData["Type"] = type;
             return View(result);
        }*/
        // POST: /EditSequence
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSequence(int page, string type, FormCollection collection)
        {
            string[] sequences = collection["PolicySequence"].Split(new char [] {','});
            
            int sequence = (page - 1 * 50) - 2;
            if (sequence < 0)
            {
                sequence = 1;
            }

            string xml = "<SequenceXML>";
            foreach (string s in sequences)
            {
                string[] policyGroupCountryCodePK = s.Split(new char[] { '_' });

                int policyGroupId = Convert.ToInt32(policyGroupCountryCodePK[0]);
                string countryCode = policyGroupCountryCodePK[1];
                int versionNumber = Convert.ToInt32(policyGroupCountryCodePK[2]);

                xml = xml + "<Item>";
                xml = xml + "<Sequence>"+sequence+"</Sequence>";
                xml = xml + "<CountryCode>"+countryCode+"</CountryCode>";
                xml = xml + "<PolicyGroupId>" + policyGroupId + "</PolicyGroupId>";
                xml = xml + "<VersionNumber>" + versionNumber + "</VersionNumber>";
                xml = xml + "</Item>";

                sequence = sequence + 1;
            }
            xml = xml + "</SequenceXML>";

            try
            {
                PolicyGroupSequenceRepository policyGroupSequenceRepository = new PolicyGroupSequenceRepository();
                policyGroupSequenceRepository.UpdatePolicyCountrySequences(xml);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyGroup.mvc/EditSequence.mvc?type=" + type + "&page=" + page;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");

            }


            return RedirectToAction("EditSequence", new { type = type});
        }
        */

        [HttpPost]
        public JsonResult AutoCompleteClientSubUnitTravelerTypes(string searchText, string hierarchyItem, string filterText)
        {
            HierarchyRepository hierarchyRepository = new HierarchyRepository();

            int maxResults = 15;
            var result = hierarchyRepository.LookUpSystemUserClientSubUnitTravelerTypes(searchText, hierarchyItem, maxResults, groupName, filterText);
            return Json(result);
        }

        // GET: Linked ClientSubUnits
        public ActionResult LinkedClientSubUnits(int id)
        {
            //Get Group From Database
            PolicyGroup group = new PolicyGroup();
            group = policyGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "ClientSubUnitGet";
                return View("RecordDoesNotExistError");
            }

            PolicyGroupClientSubUnitCountriesVM policyGroupClientSubUnits = new PolicyGroupClientSubUnitCountriesVM();
            policyGroupClientSubUnits.PolicyGroupId = id;
            policyGroupClientSubUnits.PolicyGroupName = group.PolicyGroupName;

            List<ClientSubUnitCountryVM> clientSubUnitsAvailable = new List<ClientSubUnitCountryVM>();
            clientSubUnitsAvailable = policyGroupRepository.GetLinkedClientSubUnits(id, false);
            policyGroupClientSubUnits.ClientSubUnitsAvailable = clientSubUnitsAvailable;

            List<ClientSubUnitCountryVM> clientSubUnitsUnAvailable = new List<ClientSubUnitCountryVM>();
            clientSubUnitsUnAvailable = policyGroupRepository.GetLinkedClientSubUnits(id, true);
            policyGroupClientSubUnits.ClientSubUnitsUnAvailable = clientSubUnitsUnAvailable;


            return View(policyGroupClientSubUnits);
        }

        // POST: Linked ClientSubUnits
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkedClientSubUnits(int PolicyGroupId, string ClientSubUnitGuid)
        {
            //Get Item From Database
            PolicyGroup group = new PolicyGroup();
            group = policyGroupRepository.GetGroup(PolicyGroupId);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "ClientSubUnitPost";
                return View("RecordDoesNotExistError");
            }
            //Check Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Database Update
            try
            {
                policyGroupRepository.UpdateLinkedClientSubUnit(PolicyGroupId, ClientSubUnitGuid);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyGroup.mvc/ClientSubUnit/" + PolicyGroupId.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("LinkedClientSubUnits", new { id = PolicyGroupId });
        }
    }
}
