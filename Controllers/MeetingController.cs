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
    public class MeetingController : Controller
    {
        //main repositories
        MeetingRepository meetingRepository = new MeetingRepository();
        HierarchyRepository hierarchyRepository = new HierarchyRepository();
		
		private string groupName = "Meeting Group Administrator";
       
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
                sortField = "Default";
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

			if (meetingRepository == null)
			{
				ViewData["ActionMethod"] = "ListUnDeletedGet";
				return View("Error");
			}

			var cwtPaginatedList = meetingRepository.PageMeetings(false, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

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
                sortField = "MeetingName";
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
			var cwtPaginatedList = meetingRepository.PageMeetings(true, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList); 

        }

        // GET: /View
        public ActionResult View(int id)
        {
            Meeting group = new Meeting();
			group = meetingRepository.GetGroup(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

			meetingRepository.EditGroupForDisplay(group);
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

            Meeting group = new Meeting();
			group.EnabledFlag = true;

			group.HierarchyType = "ClientTopUnit";

            return View(group);
        }

        // POST: /Create/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Meeting group, FormCollection formCollection)
        {
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

			//Combine MeetingArriveByDateTime Date and Time
			if (formCollection["MeetingArriveByTime"] != null && !string.IsNullOrEmpty(formCollection["MeetingArriveByTime"]) && group.MeetingArriveByDateTime != null)
			{
				DateTime meetingArriveByDateTime = (DateTime)group.MeetingArriveByDateTime;
				DateTime meetingArriveByTime = DateTime.Parse(formCollection["MeetingArriveByTime"].ToString());
				group.MeetingArriveByDateTime = meetingArriveByDateTime.AddHours(meetingArriveByTime.Hour).AddMinutes(meetingArriveByTime.Minute);
			}

			//Combine MeetingLeaveAfterDateTime Date and Time
			if (formCollection["MeetingLeaveAfterTime"] != null && !string.IsNullOrEmpty(formCollection["MeetingLeaveAfterTime"]) && group.MeetingLeaveAfterDateTime != null)
			{
				DateTime meetingLeaveAfterDateTime = (DateTime)group.MeetingLeaveAfterDateTime;
				DateTime meetingLeaveAfterTime = DateTime.Parse(formCollection["MeetingLeaveAfterTime"].ToString());
				group.MeetingLeaveAfterDateTime = meetingLeaveAfterDateTime.AddHours(meetingLeaveAfterTime.Hour).AddMinutes(meetingLeaveAfterTime.Minute);
			}

            //Database Update
            try
            {
				meetingRepository.Add(group);
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
            Meeting group = new Meeting();
			group = meetingRepository.GetGroup(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

			//Set Access Rights
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			meetingRepository.EditGroupForDisplay(group);

            return View(group);
        }


        // POST: /Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
		public ActionResult Edit(int id, FormCollection formCollection)
        {
            //Get Item From Database
            Meeting group = new Meeting();
			group = meetingRepository.GetGroup(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

			//Set Access Rights
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

			//Combine MeetingArriveByDateTime Date and Time
			if (formCollection["MeetingArriveByTime"] != null && !string.IsNullOrEmpty(formCollection["MeetingArriveByTime"]) && group.MeetingArriveByDateTime != null)
			{
				DateTime meetingArriveByDateTime = (DateTime)group.MeetingArriveByDateTime;
				DateTime meetingArriveByTime = DateTime.Parse(formCollection["MeetingArriveByTime"].ToString());
				group.MeetingArriveByDateTime = meetingArriveByDateTime.AddHours(meetingArriveByTime.Hour).AddMinutes(meetingArriveByTime.Minute);
			}

			//Combine MeetingLeaveAfterDateTime Date and Time
			if (formCollection["MeetingLeaveAfterTime"] != null && !string.IsNullOrEmpty(formCollection["MeetingLeaveAfterTime"]) && group.MeetingLeaveAfterDateTime != null)
			{
				DateTime meetingLeaveAfterDateTime = (DateTime)group.MeetingLeaveAfterDateTime;
				DateTime meetingLeaveAfterTime = DateTime.Parse(formCollection["MeetingLeaveAfterTime"].ToString());
				group.MeetingLeaveAfterDateTime = meetingLeaveAfterDateTime.AddHours(meetingLeaveAfterTime.Hour).AddMinutes(meetingLeaveAfterTime.Minute);
			}

            //Database Update
            try
            {
				meetingRepository.Edit(group);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/WorkFlowGroup.mvc/Edit/" + group.MeetingID.ToString();
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
           
            //Get Item From Database
            Meeting group = new Meeting();
			group = meetingRepository.GetGroup(id);

            //Check Exists
            if (group == null || group.DeletedFlag == true)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

			//Set Access Rights
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			meetingRepository.EditGroupForDisplay(group);

            return View(group);
        }

        // POST: /Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            
            //Get Item From Database
            Meeting group = new Meeting();
			group = meetingRepository.GetGroup(id);

            //Check Exists
            if (group == null || group.DeletedFlag == true)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

			//Set Access Rights
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

            //Delete Item
            try
            {
                group.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                group.DeletedFlag = true;
				meetingRepository.UpdateGroupDeletedStatus(group);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/Meeting.mvc/Delete/" + group.MeetingID;
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
            //Get Item From Database
            Meeting group = new Meeting();
			group = meetingRepository.GetGroup(id);

            //Check Exists
            if (group == null || group.DeletedFlag == false)
            {
                ViewData["ActionMethod"] = "UnDeleteGet";
                return View("RecordDoesNotExistError");
            }

			//Set Access Rights
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			meetingRepository.EditGroupForDisplay(group);
            
			return View(group);
        }

        // POST: /UnDelete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnDelete(int id, FormCollection collection)
        {
            //Get Item From Database
            Meeting group = new Meeting();
			group = meetingRepository.GetGroup(id);

            //Check Exists
            if (group == null || group.DeletedFlag == false)
            {
                ViewData["ActionMethod"] = "UnDeletePost";
                return View("RecordDoesNotExistError");
            }

			//Set Access Rights
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

            //Delete Item
            try
            {
                group.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                group.DeletedFlag = false;
				meetingRepository.UpdateGroupDeletedStatus(group);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/Meeting.mvc/UnDelete/" + group.MeetingID;
                    return View("VersionError");
                }

                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            return RedirectToAction("ListDeleted");
        }

        [HttpPost]
        public JsonResult AutoCompleteClientSubUnitTravelerTypes(string searchText, string hierarchyItem, string filterText)
        {
            HierarchyRepository hierarchyRepository = new HierarchyRepository();

            int maxResults = 100;
            var result = hierarchyRepository.LookUpSystemUserClientSubUnitTravelerTypes(searchText, hierarchyItem, maxResults, groupName, filterText);
            return Json(result);
        }

		// GET: Linked ClientSubUnits
		public ActionResult LinkedClientSubUnits(int id)
		{
			//Get Group From Database
			Meeting group = new Meeting();
			group = meetingRepository.GetGroup(id);

			//Check Exists
			if (group == null)
			{
				ViewData["ActionMethod"] = "ClientSubUnitGet";
				return View("RecordDoesNotExistError");
			}

			MeetingClientSubUnitCountriesVM MeetingClientSubUnits = new MeetingClientSubUnitCountriesVM();
			MeetingClientSubUnits.MeetingId = id;
			MeetingClientSubUnits.MeetingName = group.MeetingName;

			if (group.ClientTopUnitGuid != "")
			{
				ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
				ClientTopUnit clientTopUnit = clientTopUnitRepository.GetClientTopUnit(group.ClientTopUnitGuid);
				if (clientTopUnit != null)
				{
					MeetingClientSubUnits.ClientTopUnitName = clientTopUnit.ClientTopUnitName;
				}
			}

			List<ClientSubUnitCountryVM> clientSubUnitsAvailable = new List<ClientSubUnitCountryVM>();
			clientSubUnitsAvailable = meetingRepository.GetLinkedClientSubUnits(id, false);
			MeetingClientSubUnits.ClientSubUnitsAvailable = clientSubUnitsAvailable;

			List<ClientSubUnitCountryVM> clientSubUnitsUnAvailable = new List<ClientSubUnitCountryVM>();
			clientSubUnitsUnAvailable = meetingRepository.GetLinkedClientSubUnits(id, true);
			MeetingClientSubUnits.ClientSubUnitsUnAvailable = clientSubUnitsUnAvailable;

			//Set Access Rights
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			return View(MeetingClientSubUnits);
		}

		// POST: Linked ClientSubUnits
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LinkedClientSubUnits(int meetingId, string clientSubUnitGuid)
		{
			//Get Item From Database
			Meeting group = new Meeting();
			group = meetingRepository.GetGroup(meetingId);

			//Check Exists
			if (group == null)
			{
				ViewData["ActionMethod"] = "ClientSubUnitPost";
				return View("RecordDoesNotExistError");
			}

			//Set Access Rights
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Database Update
			try
			{
				meetingRepository.UpdateLinkedClientSubUnit(meetingId, clientSubUnitGuid);
			}
			catch (SqlException ex)
			{
				//Versioning Error
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/Meeting.mvc/ClientSubUnit/" + meetingId.ToString();
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}


			return RedirectToAction("LinkedClientSubUnits", new { id = meetingId });
		}

		// GET: Linked ClientSubUnit CreditCards
		public ActionResult LinkedClientSubUnitCreditCards(int id)
		{
			//Get Group From Database
			Meeting meeting = new Meeting();
			meeting = meetingRepository.GetGroup(id);

			//Check Exists
			if (meeting == null)
			{
				ViewData["ActionMethod"] = "ClientSubUnitGet";
				return View("RecordDoesNotExistError");
			}

			MeetingClientSubUnitCreditCardsVM meetingClientSubUnitCreditCardsVM = new MeetingClientSubUnitCreditCardsVM();
			meetingClientSubUnitCreditCardsVM.MeetingId = id;
			meetingClientSubUnitCreditCardsVM.MeetingName = meeting.MeetingName;

			if (meeting.ClientTopUnitGuid != "")
			{
				ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
				ClientTopUnit clientTopUnit = clientTopUnitRepository.GetClientTopUnit(meeting.ClientTopUnitGuid);
				if (clientTopUnit != null)
				{
					meetingClientSubUnitCreditCardsVM.ClientTopUnitName = clientTopUnit.ClientTopUnitName;
				}
			}

			List<MeetingLinkedCreditCardVM> creditCardsAvailable = new List<MeetingLinkedCreditCardVM>();
			creditCardsAvailable = meetingRepository.GetLinkedCreditCards(id, false);
			meetingClientSubUnitCreditCardsVM.CreditCardsAvailable = creditCardsAvailable;

			List<MeetingLinkedCreditCardVM> creditCardsUnAvailable = new List<MeetingLinkedCreditCardVM>();
			creditCardsUnAvailable = meetingRepository.GetLinkedCreditCards(id, true);
			meetingClientSubUnitCreditCardsVM.CreditCardsUnAvailable = creditCardsUnAvailable;

			//Set Access Rights
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			return View(meetingClientSubUnitCreditCardsVM);
		}

		// POST: Linked ClientSubUnit CreditCards
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LinkedClientSubUnitCreditCards(
			int meetingId, 
			int creditCardId, 
			string hierarchyType,
			string clientTopUnitGuid, 
			string clientSubUnitGuid,
			string sourceSystemCode,
			string clientAccountNumber,
			string travelerTypeGuid
		)
		{
			//Get Item From Database
			Meeting group = new Meeting();
			group = meetingRepository.GetGroup(meetingId);

			//Check Exists
			if (group == null)
			{
				ViewData["ActionMethod"] = "ClientSubUnitPost";
				return View("RecordDoesNotExistError");
			}

			//Set Access Rights
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Database Update
			try
			{
				meetingRepository.UpdateLinkedCreditCard(
					meetingId, 
					hierarchyType,
					clientTopUnitGuid,
					clientSubUnitGuid,
					sourceSystemCode,
					clientAccountNumber,
					travelerTypeGuid,
					creditCardId
				);
			}
			catch (SqlException ex)
			{
				//Versioning Error
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/Meeting.mvc/ClientSubUnit/" + meetingId.ToString();
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}


			return RedirectToAction("LinkedClientSubUnitCreditCards", new { id = meetingId });
		}
    }
}
