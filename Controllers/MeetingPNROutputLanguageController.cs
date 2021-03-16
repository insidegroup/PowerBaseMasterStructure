//using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;
using CWTDesktopDatabase.ViewModels;
using System.Xml;

namespace CWTDesktopDatabase.Controllers
{
	public class MeetingPNROutputLanguageController : Controller
	{
		MeetingRepository meetingRepository = new MeetingRepository();
		MeetingPNROutputRepository meetingPNROutputRepository = new MeetingPNROutputRepository();
		MeetingPNROutputLanguageRepository meetingPNROutputLanguageRepository = new MeetingPNROutputLanguageRepository();

		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		private string groupName = "Meeting Group Administrator";

		// GET: /List/
		public ActionResult List(int id, string filter, int? page, string sortField, int? sortOrder, string csu, string can, string ssc)
		{
			MeetingPNROutput meetingPNROutput = new MeetingPNROutput();
			meetingPNROutput = meetingPNROutputRepository.GetMeetingPNROutput(id);
			
			//Check Exists
			if (meetingPNROutput == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("RecordDoesNotExistError");
			}
			
			//Set Access Rights 
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//SortField
			if (string.IsNullOrEmpty(sortField))
			{
				sortField = "LanguageName";
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

			Meeting meeting = new Meeting();
			meeting = meetingRepository.GetGroup(meetingPNROutput.MeetingID);

			//Check Exists
			if (meeting == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("RecordDoesNotExistError");
			}

			meetingRepository.EditGroupForDisplay(meeting);

			ViewData["ClientTopUnitGuid"] = meeting.ClientTopUnitGuid;
			ViewData["ClientTopUnitName"] = meeting.ClientTopUnit.ClientTopUnitName;
			ViewData["MeetingID"] = meeting.MeetingID;
			ViewData["MeetingName"] = meeting.MeetingName;
			ViewData["MeetingPNROutputId"] = meetingPNROutput.MeetingPNROutputId;

			var items = meetingPNROutputLanguageRepository.PageMeetingPNROutputLanguages(id, filter ?? "", page ?? 1, sortField, sortOrder ?? 0);

			return View(items);
		}

		// GET: /Create
		public ActionResult Create(int id)
		{
			MeetingPNROutput meetingPNROutput = new MeetingPNROutput();
			meetingPNROutput = meetingPNROutputRepository.GetMeetingPNROutput(id);

			//Check Exists
			if (meetingPNROutput == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			MeetingPNROutputLanguageVM meetingPNROutputLanguageVM = new MeetingPNROutputLanguageVM();
			
			Meeting meeting = new Meeting();
			meeting = meetingRepository.GetGroup(meetingPNROutput.MeetingID);
			
			//Check Exists
			if (meeting == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			}

			meetingRepository.EditGroupForDisplay(meeting);

			ViewData["ClientTopUnitGuid"] = meeting.ClientTopUnitGuid;
			ViewData["ClientTopUnitName"] = meeting.ClientTopUnit.ClientTopUnitName;
			ViewData["MeetingID"] = meeting.MeetingID;
			ViewData["MeetingName"] = meeting.MeetingName;
			ViewData["MeetingPNROutputId"] = meetingPNROutput.MeetingPNROutputId;

			MeetingPNROutputLanguage meetingPNROutputLanguage = new MeetingPNROutputLanguage();
			meetingPNROutputLanguage.MeetingPNROutputId = meetingPNROutput.MeetingPNROutputId;
			
			meetingPNROutputLanguageVM.MeetingPNROutputLanguage = meetingPNROutputLanguage;
			meetingPNROutputLanguageVM.MeetingPNROutput = meetingPNROutput;

			//Languages
			LanguageRepository languageRepository = new LanguageRepository();
			meetingPNROutputLanguageVM.Languages = new SelectList(meetingPNROutputLanguageRepository.GetAvailableLanguages(meetingPNROutput.MeetingPNROutputId).ToList(), "LanguageCode", "LanguageName");

			return View(meetingPNROutputLanguageVM);
		}

		// POST: /Create/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(MeetingPNROutputLanguageVM meetingPNROutputLanguageVM)
		{
			MeetingPNROutput meetingPNROutput = new MeetingPNROutput();
			meetingPNROutput = meetingPNROutputRepository.GetMeetingPNROutput(meetingPNROutputLanguageVM.MeetingPNROutput.MeetingPNROutputId);

			//Check Exists
			if (meetingPNROutput == null)
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
				UpdateModel(meetingPNROutputLanguageVM);
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
				meetingPNROutputLanguageRepository.Add(meetingPNROutputLanguageVM.MeetingPNROutputLanguage);
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

			return RedirectToAction("List", new
			{
				id = meetingPNROutputLanguageVM.MeetingPNROutput.MeetingPNROutputId,
			});
		}

		// GET: /Edit
		public ActionResult Edit(int id, string languageCode)
		{
			MeetingPNROutput meetingPNROutput = new MeetingPNROutput();
			meetingPNROutput = meetingPNROutputRepository.GetMeetingPNROutput(id);

			//Check Exists
			if (meetingPNROutput == null)
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

			MeetingPNROutputLanguageVM meetingPNROutputLanguageVM = new MeetingPNROutputLanguageVM();

			Meeting meeting = new Meeting();
			meeting = meetingRepository.GetGroup(meetingPNROutput.MeetingID);

			//Check Exists
			if (meeting == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			meetingRepository.EditGroupForDisplay(meeting);

			ViewData["ClientTopUnitGuid"] = meeting.ClientTopUnitGuid;
			ViewData["ClientTopUnitName"] = meeting.ClientTopUnit.ClientTopUnitName;
			ViewData["MeetingID"] = meeting.MeetingID;
			ViewData["MeetingName"] = meeting.MeetingName;
			ViewData["MeetingPNROutputId"] = meetingPNROutput.MeetingPNROutputId;

			MeetingPNROutputLanguage meetingPNROutputLanguage = new MeetingPNROutputLanguage();
			meetingPNROutputLanguage = meetingPNROutputLanguageRepository.GetMeetingPNROutputLanguage(id, languageCode);

			//Check Exists
			if (meetingPNROutputLanguage == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Save Current Language
			meetingPNROutputLanguage.CurrentLanguageCode = meetingPNROutputLanguage.LanguageCode;

			meetingPNROutputLanguageVM.MeetingPNROutputLanguage = meetingPNROutputLanguage;
			meetingPNROutputLanguageVM.MeetingPNROutput = meetingPNROutput;

			//Languages
			LanguageRepository languageRepository = new LanguageRepository();
			List<Language> languages = meetingPNROutputLanguageRepository.GetAvailableLanguages(meetingPNROutput.MeetingPNROutputId).ToList();
			Language language = languageRepository.GetLanguage(meetingPNROutputLanguage.LanguageCode);
			if (language != null)
			{
				languages.Add(language);
			}

			meetingPNROutputLanguageVM.Languages = new SelectList(languages, "LanguageCode", "LanguageName", meetingPNROutputLanguage.LanguageCode);

			return View(meetingPNROutputLanguageVM);
		}

		// POST: /Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(MeetingPNROutputLanguageVM meetingPNROutputLanguageVM)
		{
			MeetingPNROutputLanguage meetingPNROutputLanguage = new MeetingPNROutputLanguage();
			meetingPNROutputLanguage = meetingPNROutputLanguageRepository.GetMeetingPNROutputLanguage(
				meetingPNROutputLanguageVM.MeetingPNROutputLanguage.MeetingPNROutputId,
				meetingPNROutputLanguageVM.MeetingPNROutputLanguage.CurrentLanguageCode
			);

			//Check Exists
			if (meetingPNROutputLanguage == null)
			{
				ViewData["ActionMethod"] = "EditPost";
				return View("RecordDoesNotExistError");
			}

			Meeting meeting = new Meeting();
			meeting = meetingRepository.GetGroup(meetingPNROutputLanguageVM.MeetingPNROutput.MeetingID);

			//Check Exists
			if (meeting == null)
			{
				ViewData["ActionMethod"] = "EditPost";
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
				UpdateModel(meetingPNROutputLanguageVM);
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
				meetingPNROutputLanguageRepository.Update(meetingPNROutputLanguageVM.MeetingPNROutputLanguage);
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

			return RedirectToAction("List", new
			{
				id = meetingPNROutputLanguageVM.MeetingPNROutput.MeetingPNROutputId,
			});
		}

		// GET: /Delete/5
		[HttpGet]
		public ActionResult Delete(int id, string languageCode)
		{
			MeetingPNROutputLanguage meetingPNROutputLanguage = new MeetingPNROutputLanguage();
			meetingPNROutputLanguage = meetingPNROutputLanguageRepository.GetMeetingPNROutputLanguage(id, languageCode);

			//Check Exists
			if (meetingPNROutputLanguage == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			MeetingPNROutputLanguageVM meetingPNROutputLanguageVM = new MeetingPNROutputLanguageVM();

			MeetingPNROutput meetingPNROutput = new MeetingPNROutput();
			meetingPNROutput = meetingPNROutputRepository.GetMeetingPNROutput(id);

			//Check Exists
			if (meetingPNROutput == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			Meeting meeting = new Meeting();
			meeting = meetingRepository.GetGroup(meetingPNROutput.MeetingID);

			//Check Exists
			if (meeting == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			meetingRepository.EditGroupForDisplay(meeting);

			ViewData["ClientTopUnitGuid"] = meeting.ClientTopUnitGuid;
			ViewData["ClientTopUnitName"] = meeting.ClientTopUnit.ClientTopUnitName;
			ViewData["MeetingID"] = meeting.MeetingID;
			ViewData["MeetingName"] = meeting.MeetingName;
			ViewData["MeetingPNROutputId"] = meetingPNROutput.MeetingPNROutputId;

			//Check Exists
			if (meetingPNROutputLanguage == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			meetingPNROutputLanguageVM.MeetingPNROutputLanguage = meetingPNROutputLanguage;
			meetingPNROutputLanguageVM.MeetingPNROutput = meetingPNROutput;

			//Languages
			LanguageRepository languageRepository = new LanguageRepository();
			List<Language> languages = meetingPNROutputLanguageRepository.GetAvailableLanguages(meetingPNROutput.MeetingPNROutputId).ToList();
			Language language = languageRepository.GetLanguage(meetingPNROutputLanguage.LanguageCode);
			if (language != null)
			{
				languages.Add(language);
			}

			meetingPNROutputLanguageVM.Languages = new SelectList(languages, "LanguageCode", "LanguageName", meetingPNROutputLanguage.LanguageCode);

			return View(meetingPNROutputLanguageVM);

		}

		// POST: //Delete/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(MeetingPNROutputLanguageVM meetingPNROutputLanguageVM)
		{
			MeetingPNROutputLanguage meetingPNROutputLanguage = new MeetingPNROutputLanguage();
			meetingPNROutputLanguage = meetingPNROutputLanguageRepository.GetMeetingPNROutputLanguage(
				meetingPNROutputLanguageVM.MeetingPNROutputLanguage.MeetingPNROutputId,
				meetingPNROutputLanguageVM.MeetingPNROutputLanguage.LanguageCode
			);

			//Check Exists
			if (meetingPNROutputLanguage == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			MeetingPNROutput meetingPNROutput = new MeetingPNROutput();
			meetingPNROutput = meetingPNROutputRepository.GetMeetingPNROutput(
				meetingPNROutputLanguage.MeetingPNROutputId
			);

			//Check Exists
			if (meetingPNROutput == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			Meeting meeting = new Meeting();
			meeting = meetingRepository.GetGroup(meetingPNROutput.MeetingID);

			//Check Exists
			if (meeting == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Delete item
			try
			{
				meetingPNROutputLanguageRepository.Delete(meetingPNROutputLanguage);
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

			return RedirectToAction("List", new
			{
				id = meetingPNROutputLanguageVM.MeetingPNROutput.MeetingPNROutputId
			});
		}
	}
}