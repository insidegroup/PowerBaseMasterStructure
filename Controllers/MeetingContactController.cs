using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.Helpers;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class MeetingContactController : Controller
    {
        ClientDetailRepository clientDetailRepository = new ClientDetailRepository();
        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
        ClientDetailClientSubUnitRepository clientDetailClientSubUnitRepository = new ClientDetailClientSubUnitRepository();
        ClientDetailContactRepository clientDetailContactRepository = new ClientDetailContactRepository();
		ClientSubUnitContactRepository clientSubUnitContactRepository = new ClientSubUnitContactRepository();
		
		MeetingRepository meetingRepository = new MeetingRepository();
		MeetingContactRepository meetingContactRepository = new MeetingContactRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();

		private string groupName = "Meeting Group Administrator";

        // GET: /List
		public ActionResult List(int id, string filter, int? page, string sortField, int? sortOrder)
        {
			Meeting meeting = new Meeting();
			meeting = meetingRepository.GetGroup(id);

            //Check Exists
			if (meeting == null)
            {
                ViewData["ActionMethod"] = "List";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//SortField
			if (sortField == string.Empty)
			{
				sortField = "ContactTypeName";
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
				sortOrder = 0;
			}

            //Populate View Model
			MeetingContactsVM meetingContactsVM = new MeetingContactsVM();

			meetingRepository.EditGroupForDisplay(meeting);
			meetingContactsVM.Meeting = meeting;

			var contacts = meetingContactRepository.PageMeetingContacts(id, filter ?? "", sortField, sortOrder ?? 0, page ?? 1);
			if (contacts != null)
			{
				meetingContactsVM.Contacts = contacts;
			}

			return View(meetingContactsVM);
        }

		// GET: /Create
		public ActionResult Create(int id)
		{
			Meeting meeting = new Meeting();
			meeting = meetingRepository.GetGroup(id);

			//Check Exists
			if (meeting == null)
			{
				ViewData["ActionMethod"] = "List";
				return View("RecordDoesNotExistError");
			}

			//Access Rights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			MeetingContactVM meetingContactVM = new MeetingContactVM();

			meetingRepository.EditGroupForDisplay(meeting);
			meetingContactVM.Meeting = meeting;

			MeetingContact meetingContact = new MeetingContact();
			meetingContactVM.MeetingContact = meetingContact;

			ContactTypeRepository contactTypeRepository = new ContactTypeRepository();
			meetingContactVM.ContactTypes = new SelectList(contactTypeRepository.GetAllContactTypes().ToList(), "ContactTypeId", "ContactTypeName");

			CountryRepository countryRepository = new CountryRepository();
			meetingContactVM.Countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName");

			return View(meetingContactVM);
		}

		// POST: /Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(MeetingContactVM meetingContactVM)
		{
			int id = meetingContactVM.Meeting.MeetingID;

			Meeting meeting = new Meeting();
			meeting = meetingRepository.GetGroup(id);

			//Check Exists
			if (meeting == null)
			{
				ViewData["ActionMethod"] = "Create";
				return View("RecordDoesNotExistError");
			}

			//Access Rights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//Update  Model from Form
			try
			{
				TryUpdateModel<MeetingContact>(meetingContactVM.MeetingContact, "MeetingContact");
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
				meetingContactRepository.Add(meetingContactVM);
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
		public ActionResult Edit(int id)
		{

			MeetingContact meetingContact = new MeetingContact();
			meetingContact = meetingContactRepository.GetContact(id);

			//Check Exists
			if (meetingContact == null)
			{
				ViewData["ActionMethod"] = "List";
				return View("RecordDoesNotExistError");
			}

			//Access Rights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			MeetingContactVM meetingContactVM = new MeetingContactVM();
			meetingContactVM.MeetingContact = meetingContact;

			Meeting meeting = new Meeting();
			meeting = meetingRepository.GetGroup(meetingContact.MeetingID);
			if (meeting != null)
			{
				meetingRepository.EditGroupForDisplay(meeting);
				meetingContactVM.Meeting = meeting;
			}

			ContactTypeRepository contactTypeRepository = new ContactTypeRepository();
			meetingContactVM.ContactTypes = new SelectList(contactTypeRepository.GetAllContactTypes().ToList(), "ContactTypeId", "ContactTypeName", meetingContact.ContactTypeId);

			CountryRepository countryRepository = new CountryRepository();
			meetingContactVM.Countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName", meetingContact.CountryCode);

			return View(meetingContactVM);
		}

		// POST: /Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(MeetingContactVM meetingContactVM)
		{
			int id = meetingContactVM.Meeting.MeetingID;

			Meeting meeting = new Meeting();
			meeting = meetingRepository.GetGroup(id);
			meetingContactVM.Meeting = meeting;

			//Check Exists
			if (meeting == null)
			{
				ViewData["ActionMethod"] = "List";
				return View("RecordDoesNotExistError");
			}

			//Access Rights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//Update  Model from Form
			try
			{
				TryUpdateModel<MeetingContact>(meetingContactVM.MeetingContact, "MeetingContact");
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
				meetingContactRepository.Edit(meetingContactVM);
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

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
		{
			MeetingContact meetingContact = new MeetingContact();
			meetingContact = meetingContactRepository.GetContact(id);

			//Check Exists
			if (meetingContact == null)
			{
				ViewData["ActionMethod"] = "List";
				return View("RecordDoesNotExistError");
			}

			//Access Rights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			MeetingContactVM meetingContactVM = new MeetingContactVM();
			meetingContactRepository.EditForDisplay(meetingContact);
			meetingContactVM.MeetingContact = meetingContact;

			Meeting meeting = new Meeting();
			meeting = meetingRepository.GetGroup(meetingContact.MeetingID);
			if (meeting != null)
			{
				meetingRepository.EditGroupForDisplay(meeting);
				meetingContactVM.Meeting = meeting;
			}

			return View(meetingContactVM);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, FormCollection collection)
		{
			MeetingContact meetingContact = new MeetingContact();
			meetingContact = meetingContactRepository.GetContact(id);

			//Check Exists
			if (meetingContact == null)
			{
				ViewData["ActionMethod"] = "List";
				return View("RecordDoesNotExistError");
			}

			//Access Rights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//Delete Item
			try
			{
				meetingContactRepository.Delete(meetingContact);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/MeetingContact.mvc/Delete/" + meetingContact.MeetingContactID.ToString();
					return View("VersionError");
				}
				//Generic Error
				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { id = meetingContact.MeetingID });
		}
    }
}
