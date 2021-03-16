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
using System.Xml;

namespace CWTDesktopDatabase.Controllers
{
	public class MeetingPNROutputController : Controller
	{
		MeetingRepository meetingRepository = new MeetingRepository();
		MeetingPNROutputRepository meetingPNROutputRepository = new MeetingPNROutputRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		private string groupName = "Meeting Group Administrator";

		// GET: /List/
		public ActionResult List(int id, string filter, int? page, string sortField, int? sortOrder)
		{			
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
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//SortField
			if (string.IsNullOrEmpty(sortField))
			{
				sortField = "GDSName";
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

			meetingRepository.EditGroupForDisplay(meeting);

			ViewData["ClientTopUnitGuid"] = meeting.ClientTopUnitGuid;
			ViewData["ClientTopUnitName"] = meeting.ClientTopUnit.ClientTopUnitName;
			ViewData["MeetingID"] = meeting.MeetingID;
			ViewData["MeetingName"] = meeting.MeetingName;

			var items = meetingPNROutputRepository.PageMeetingPNROutputItems(filter ?? "", id, page ?? 1, sortField, sortOrder ?? 0);

			return View(items);
		}

		// GET: /Create
		public ActionResult Create(int id)
		{
			Meeting meeting = new Meeting();
			meeting = meetingRepository.GetGroup(id);

			//Check Exists
			if (meeting == null)
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

			MeetingPNROutputVM meetingPNROutputVM = new MeetingPNROutputVM();
			
			MeetingPNROutput meetingPNROutput = new MeetingPNROutput();
			meetingPNROutputVM.MeetingPNROutput = meetingPNROutput;
			
			meetingRepository.EditGroupForDisplay(meeting);

			ViewData["ClientTopUnitGuid"] = meeting.ClientTopUnitGuid;
			ViewData["ClientTopUnitName"] = meeting.ClientTopUnit.ClientTopUnitName;
			ViewData["MeetingID"] = meeting.MeetingID;
			ViewData["MeetingName"] = meeting.MeetingName;

			meetingPNROutputVM.Meeting = meeting;
			meetingPNROutput.MeetingID = meeting.MeetingID;

			//GDS
			GDSRepository GDSRepository = new GDSRepository();
			meetingPNROutputVM.GDSList = new SelectList(GDSRepository.GetAllGDSsExceptALL().OrderBy(x => x.GDSName).ToList(), "GDSCode", "GDSName");

			//PNR Output Remark Types
			PNROutputRemarkTypeRepository PNROutputRemarkTypeRepository = new PNROutputRemarkTypeRepository();
			meetingPNROutputVM.PNROutputRemarkTypeCodes = new SelectList(PNROutputRemarkTypeRepository.GetMeetingPNROutputRemarkTypes(), "PNROutputRemarkTypeCode", "PNROutputRemarkTypeName", "");

			//Languages
			LanguageRepository languageRepository = new LanguageRepository();
			meetingPNROutputVM.Languages = new SelectList(languageRepository.GetAllLanguages().ToList(), "LanguageCode", "LanguageName");

			//Countries
			CountryRepository countryRepository = new CountryRepository();
			meetingPNROutputVM.Countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName");

			return View(meetingPNROutputVM);
		}

		// POST: /Create/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(MeetingPNROutputVM meetingPNROutputVM)
		{
			MeetingPNROutput meetingPNROutput = new MeetingPNROutput();
			meetingPNROutput = meetingPNROutputRepository.GetMeetingPNROutput(meetingPNROutputVM.MeetingPNROutput.MeetingPNROutputId);

			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel(meetingPNROutputVM);
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
				meetingPNROutputRepository.Add(meetingPNROutputVM.MeetingPNROutput);
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
				id = meetingPNROutputVM.MeetingPNROutput.MeetingID
			});
		}

		// GET: /Edit
		public ActionResult Edit(int id)
		{
			MeetingPNROutputVM meetingPNROutputVM = new MeetingPNROutputVM();

			MeetingPNROutput meetingPNROutput = new MeetingPNROutput();
			meetingPNROutput = meetingPNROutputRepository.GetMeetingPNROutput(id);
			if (meetingPNROutput == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			meetingPNROutputVM.MeetingPNROutput = meetingPNROutput;

			//Check Exists Meeting for VM
			Meeting meeting = new Meeting();
			meeting = meetingRepository.GetGroup(meetingPNROutput.MeetingID);
			if (meeting != null)
			{
				meetingRepository.EditGroupForDisplay(meeting);
				meetingPNROutputVM.Meeting = meeting;
			}

			meetingRepository.EditGroupForDisplay(meeting);

			ViewData["ClientTopUnitGuid"] = meeting.ClientTopUnitGuid;
			ViewData["ClientTopUnitName"] = meeting.ClientTopUnit.ClientTopUnitName;
			ViewData["MeetingID"] = meeting.MeetingID;
			ViewData["MeetingName"] = meeting.MeetingName;

			meetingPNROutputVM.Meeting = meeting;
			meetingPNROutput.MeetingID = meeting.MeetingID;

			//GDS
			GDSRepository GDSRepository = new GDSRepository();
			meetingPNROutputVM.GDSList = new SelectList(GDSRepository.GetAllGDSsExceptALL().OrderBy(x => x.GDSName).ToList(), "GDSCode", "GDSName", meetingPNROutput.GDSCode);

			//PNR Output Remark Types
			PNROutputRemarkTypeRepository PNROutputRemarkTypeRepository = new PNROutputRemarkTypeRepository();
			meetingPNROutputVM.PNROutputRemarkTypeCodes = new SelectList(PNROutputRemarkTypeRepository.GetMeetingPNROutputRemarkTypes(), "PNROutputRemarkTypeCode", "PNROutputRemarkTypeName", meetingPNROutput.PNROutputRemarkTypeCode);

			//Languages
			LanguageRepository languageRepository = new LanguageRepository();
			meetingPNROutputVM.Languages = new SelectList(languageRepository.GetAllLanguages().ToList(), "LanguageCode", "LanguageName", meetingPNROutput.DefaultLanguageCode);

			//Countries
			CountryRepository countryRepository = new CountryRepository();
			meetingPNROutputVM.Countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName", meetingPNROutput.CountryCode);

			return View(meetingPNROutputVM);
		}

		// POST: /Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(MeetingPNROutputVM meetingPNROutputVM)
		{
			MeetingPNROutput meetingPNROutput = new MeetingPNROutput();
			meetingPNROutput = meetingPNROutputRepository.GetMeetingPNROutput(meetingPNROutputVM.MeetingPNROutput.MeetingPNROutputId);

			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel(meetingPNROutputVM);
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
				meetingPNROutputRepository.Update(meetingPNROutputVM.MeetingPNROutput);
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
				id = meetingPNROutputVM.MeetingPNROutput.MeetingID
			});
		}

		// GET: /Delete/5
		[HttpGet]
		public ActionResult Delete(int id)
		{
			MeetingPNROutput meetingPNROutput = new MeetingPNROutput();
			meetingPNROutput = meetingPNROutputRepository.GetMeetingPNROutput(id);

			//Check Exists
			if (meetingPNROutput == null)
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

			MeetingPNROutputVM meetingPNROutputVM = new MeetingPNROutputVM();

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

			meetingPNROutputVM.Meeting = meeting;
			meetingPNROutput.MeetingID = meeting.MeetingID;

			meetingPNROutputRepository.EditGroupForDisplay(meetingPNROutput);
			meetingPNROutputVM.MeetingPNROutput = meetingPNROutput;

			return View(meetingPNROutputVM);

		}

		// POST: //Delete/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(MeetingPNROutputVM meetingPNROutputVM)
		{
			MeetingPNROutput meetingPNROutput = new MeetingPNROutput();
			meetingPNROutput = meetingPNROutputRepository.GetMeetingPNROutput(meetingPNROutputVM.MeetingPNROutput.MeetingPNROutputId);

			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Delete item
			try
			{
				meetingPNROutputRepository.Delete(meetingPNROutputVM.MeetingPNROutput);
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
				id = meetingPNROutputVM.MeetingPNROutput.MeetingID
			});
		}
	}
}