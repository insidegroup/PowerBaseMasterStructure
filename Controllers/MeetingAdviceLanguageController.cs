using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Controllers
{
    public class MeetingAdviceLanguageController : Controller
    {
        //main repositories
		MeetingAdviceLanguageRepository meetingAdviceLanguageRepository = new MeetingAdviceLanguageRepository();
		MeetingRepository meetingRepository = new MeetingRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();

		private string groupName = "Meeting Group Administrator";

        //GET:List
		public ActionResult List(int id, int meetingAdviceTypeId, int? page, string sortField, int? sortOrder)
        {
			//Get Meeting
			Meeting meeting = new Meeting();
			meeting = meetingRepository.GetGroup(id);

            //Check Exists
			if (meeting == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Access"] = "WriteAccess";
            }

			MeetingAdviceLanguagesVM meetingAdviceLanguagesVM = new MeetingAdviceLanguagesVM();
			meetingRepository.EditGroupForDisplay(meeting);
			meetingAdviceLanguagesVM.Meeting = meeting;

			ViewData["MeetingAdviceTypeId"] = meetingAdviceTypeId;
			ViewData["MeetingAdviceTypeName"] = meetingAdviceLanguageRepository.GetMeetingAdviceTypeName(meetingAdviceTypeId);
			ViewData["MeetingAdviceTypeLabelName"] = meetingAdviceLanguageRepository.GetMeetingAdviceTypeLabelName(meetingAdviceTypeId);

            //SortField+SortOrder settings
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "LanguageName";
            }
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

            //Get data
			meetingAdviceLanguagesVM.MeetingAdviceLanguages = meetingAdviceLanguageRepository.PageMeetingAdviceLanguages(id, meetingAdviceTypeId, page ?? 1, sortField, sortOrder ?? 0);
			
			return View(meetingAdviceLanguagesVM);
        }

        // GET: /Create
		public ActionResult Create(int id, int meetingAdviceTypeId)
        {
			//Get Meeting
			Meeting meeting = new Meeting();
			meeting = meetingRepository.GetGroup(id);

            //Check Exists
			if (meeting == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
			ViewData["Access"] = "";
			RolesRepository rolesRepository = new RolesRepository();
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			ViewData["MeetingAdviceTypeName"] = meetingAdviceLanguageRepository.GetMeetingAdviceTypeName(meetingAdviceTypeId);
			ViewData["MeetingAdviceTypeLabelName"] = meetingAdviceLanguageRepository.GetMeetingAdviceTypeLabelName(meetingAdviceTypeId);

			MeetingAdviceLanguageVM meetingAdviceLanguageVM = new MeetingAdviceLanguageVM();
			meetingRepository.EditGroupForDisplay(meeting); 
			meetingAdviceLanguageVM.Meeting = meeting;
			
			//New MeetingAdviceLanguage
            MeetingAdviceLanguage meetingAdviceLanguage = new MeetingAdviceLanguage();
			meetingAdviceLanguage.MeetingID = id;
			meetingAdviceLanguage.MeetingAdviceTypeId = meetingAdviceTypeId;
			meetingAdviceLanguageRepository.EditItemForDisplay(meetingAdviceLanguage); 
			meetingAdviceLanguageVM.MeetingAdviceLanguage = meetingAdviceLanguage;
            
            //Language SelectList
			SelectList languageList = new SelectList(meetingAdviceLanguageRepository.GetAllAvailableLanguages(id, meetingAdviceTypeId).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;
			if (languageList != null)
			{
				meetingAdviceLanguageVM.MeetingAdviceLanguages = languageList;
			}

            //Show Create Form
			return View(meetingAdviceLanguageVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MeetingAdviceLanguageVM meetingAdviceLanguageVM)
        {
			//Get Meeting
			Meeting meeting = new Meeting();
			meeting = meetingRepository.GetGroup(meetingAdviceLanguageVM.MeetingAdviceLanguage.MeetingID);

			//Check Exists
			if (meeting == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			}

			//AccessRights
			ViewData["Access"] = "";
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				return View("Error");
            }

            //Update  Model from Form
            try
            {
				UpdateModel(meetingAdviceLanguageVM);
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
				meetingAdviceLanguageRepository.Add(meetingAdviceLanguageVM);
            }
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);
               
				//Insert Error
                return View("Error");
            }

			return RedirectToAction("List", new { 
				id = meetingAdviceLanguageVM.MeetingAdviceLanguage.MeetingID,
				meetingAdviceTypeId = meetingAdviceLanguageVM.MeetingAdviceLanguage.MeetingAdviceTypeId
			 });
        }

        // GET: /Edit
		public ActionResult Edit(int id, int meetingAdviceTypeId, string languageCode)
        {
            //Get Item 
            MeetingAdviceLanguage meetingAdviceLanguage = new MeetingAdviceLanguage();
			meetingAdviceLanguage = meetingAdviceLanguageRepository.GetMeetingAdviceLanguage(id, meetingAdviceTypeId, languageCode);

            //Check Exists
            if (meetingAdviceLanguage == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

			//Get Meeting
			Meeting meeting = new Meeting();
			meeting = meetingRepository.GetGroup(id);

			//Check Exists
			if (meeting == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			}

			//AccessRights
			ViewData["Access"] = "";
			RolesRepository rolesRepository = new RolesRepository();
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			ViewData["MeetingAdviceTypeName"] = meetingAdviceLanguageRepository.GetMeetingAdviceTypeName(meetingAdviceTypeId);
			ViewData["MeetingAdviceTypeLabelName"] = meetingAdviceLanguageRepository.GetMeetingAdviceTypeLabelName(meetingAdviceTypeId);

			//New MeetingAdviceLanguage
			MeetingAdviceLanguageVM meetingAdviceLanguageVM = new MeetingAdviceLanguageVM();

			meetingRepository.EditGroupForDisplay(meeting);
			meetingAdviceLanguageVM.Meeting = meeting;

			meetingAdviceLanguageVM.MeetingAdviceLanguage = meetingAdviceLanguage;
			meetingAdviceLanguageRepository.EditItemForDisplay(meetingAdviceLanguage);

			//Language SelectList
			SelectList languageList = new SelectList(meetingAdviceLanguageRepository.GetAllAvailableLanguages(id, meetingAdviceTypeId).ToList(), "LanguageCode", "LanguageName", meetingAdviceLanguage.LanguageCode);
			ViewData["Languages"] = languageList;
			if (languageList != null)
			{
				meetingAdviceLanguageVM.MeetingAdviceLanguages = languageList;
			}

			//Show Create Form
			return View(meetingAdviceLanguageVM);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
		public ActionResult Edit(MeetingAdviceLanguageVM meetingAdviceLanguageVM)
        {
			int id = meetingAdviceLanguageVM.MeetingAdviceLanguage.MeetingID;

			Meeting meeting = new Meeting();
			meeting = meetingRepository.GetGroup(id);
			meetingAdviceLanguageVM.Meeting = meeting;

			//Check Exists
			if (meeting == null)
			{
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

			//AccessRights
			ViewData["Access"] = "";
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				return View("Error");
			}

            //Update Item from Form
            try
            {
				UpdateModel(meetingAdviceLanguageVM.MeetingAdviceLanguage);
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

            //Update MeetingAdviceLanguage
            try
            {
				meetingAdviceLanguageRepository.Update(meetingAdviceLanguageVM.MeetingAdviceLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
					ViewData["ReturnURL"] = "/MeetingAdviceLanguage.mvc/Edit/" + meetingAdviceLanguageVM.MeetingAdviceLanguage.MeetingID.ToString();
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }

			return RedirectToAction("List", new
			{
				id = meetingAdviceLanguageVM.MeetingAdviceLanguage.MeetingID,
				meetingAdviceTypeId = meetingAdviceLanguageVM.MeetingAdviceLanguage.MeetingAdviceTypeId
			});
        }


        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, int meetingAdviceTypeId, string languageCode)
        {
			//Get Item 
			MeetingAdviceLanguage meetingAdviceLanguage = new MeetingAdviceLanguage();
			meetingAdviceLanguage = meetingAdviceLanguageRepository.GetMeetingAdviceLanguage(id, meetingAdviceTypeId, languageCode);

			//Check Exists
			if (meetingAdviceLanguage == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Get Meeting
			Meeting meeting = new Meeting();
			meeting = meetingRepository.GetGroup(id);

			//Check Exists
			if (meeting == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			}

			//AccessRights
			ViewData["Access"] = "";
			RolesRepository rolesRepository = new RolesRepository();
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			ViewData["MeetingAdviceTypeName"] = meetingAdviceLanguageRepository.GetMeetingAdviceTypeName(meetingAdviceTypeId);
			ViewData["MeetingAdviceTypeLabelName"] = meetingAdviceLanguageRepository.GetMeetingAdviceTypeLabelName(meetingAdviceTypeId);
			
			//MeetingAdviceLanguage
			MeetingAdviceLanguageVM meetingAdviceLanguageVM = new MeetingAdviceLanguageVM();

			meetingRepository.EditGroupForDisplay(meeting);
			meetingAdviceLanguageVM.Meeting = meeting; 
			
			meetingAdviceLanguageVM.MeetingAdviceLanguage = meetingAdviceLanguage;
			meetingAdviceLanguageRepository.EditItemForDisplay(meetingAdviceLanguage);

			//Show Create Form
			return View(meetingAdviceLanguageVM);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
		public ActionResult Delete(int id, int meetingAdviceTypeId, string languageCode, FormCollection collection)
        {
			//Get Item 
			MeetingAdviceLanguage meetingAdviceLanguage = new MeetingAdviceLanguage();
			meetingAdviceLanguage = meetingAdviceLanguageRepository.GetMeetingAdviceLanguage(id, meetingAdviceTypeId, languageCode);

			//Check Exists
			if (meetingAdviceLanguage == null)
			{
				ViewData["ActionMethod"] = "EditPost";
				return View("RecordDoesNotExistError");
			}

			//AccessRights
			ViewData["Access"] = "";
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				return View("Error");
			}

            //Delete Item
            try
            {
                meetingAdviceLanguageRepository.Delete(meetingAdviceLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/MeetingAdviceLanguage.mvc/Delete/" + meetingAdviceLanguage.MeetingID.ToString() + "/" + meetingAdviceLanguage.LanguageCode;
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }


            //Return
			return RedirectToAction("List", new
			{
				id = meetingAdviceLanguage.MeetingID,
				meetingAdviceTypeId = meetingAdviceLanguage.MeetingAdviceTypeId
			});
        }
    }
}
