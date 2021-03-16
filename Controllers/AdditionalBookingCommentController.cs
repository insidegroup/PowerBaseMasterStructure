using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
	public class AdditionalBookingCommentController : Controller
	{
		ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
		BookingChannelRepository BookingChannelRepository = new BookingChannelRepository();
		AdditionalBookingCommentRepository additionalBookingCommentRepository = new AdditionalBookingCommentRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		
		private string groupName = "Client Detail";

		// GET: /List
		public ActionResult List(int? page, string csu, int id, string sortField, int? sortOrder, string filter)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToClientSubUnit(csu) && hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//Sorting
			if (sortField == null || sortField == "UsageTypeDescription")
			{
				sortField = "UsageTypeDescription";
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
			};

			AdditionalBookingCommentsVM additionalBookingCommentsVM = new AdditionalBookingCommentsVM();
			additionalBookingCommentsVM.AdditionalBookingComments = additionalBookingCommentRepository.PageAdditionalBookingComments(page ?? 1, id, sortField, sortOrder, filter ?? "");

			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

			//Check clientSubUnit
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "List";
				return View("RecordDoesNotExistError");
			}

			additionalBookingCommentsVM.ClientSubUnit = clientSubUnit;
			
			//Get BookingChannel
			BookingChannel BookingChannel = new BookingChannel();
			BookingChannel = BookingChannelRepository.BookingChannel(id);

			additionalBookingCommentsVM.BookingChannel = BookingChannel;

			//return items
			return View(additionalBookingCommentsVM);
		}

		// GET: /Create
		public ActionResult Create(int id, string csu)
		{
			//Get ClientSubUnit
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

			//Check Exists
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			}

			//Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//VM
			AdditionalBookingCommentVM additionalBookingCommentVM = new AdditionalBookingCommentVM();

			// Additional Booking Comment
			AdditionalBookingComment additionalBookingComment = new AdditionalBookingComment();
			additionalBookingComment.BookingChannelId = id;
			additionalBookingCommentVM.AdditionalBookingComment = additionalBookingComment;
		
			// CSU
			additionalBookingCommentVM.ClientSubUnit = clientSubUnit;

			// Languages
			LanguageRepository languageRepository = new LanguageRepository();
			additionalBookingCommentVM.Languages = new SelectList(additionalBookingCommentRepository.GetAvailableLanguages(id).ToList(), "LanguageCode", "LanguageName");

			// Show Create Form
			return View(additionalBookingCommentVM);
		}

		// POST: /Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(AdditionalBookingCommentVM additionalBookingCommentVM, FormCollection formCollection)
		{
			string clientSubUnitGuid = formCollection["ClientSubUnit.ClientSubUnitGuid"];

			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

			//Check Exists
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			AdditionalBookingComment additionalBookingComment = new AdditionalBookingComment();
			additionalBookingComment = additionalBookingCommentVM.AdditionalBookingComment;
			if (additionalBookingComment == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid) || !hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update  Model from Form
			try
			{
				TryUpdateModel<AdditionalBookingComment>(additionalBookingComment, "AdditionalBookingComment");
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
				additionalBookingCommentRepository.Add(additionalBookingCommentVM);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { id = additionalBookingCommentVM.AdditionalBookingComment.BookingChannelId, csu = clientSubUnitGuid });
		}

		// GET: /Edit
		public ActionResult Edit(int id, string csu)
		{
			//Get ClientSubUnit
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

			//Check Exists
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			}

			//Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientSubUnit(csu) || !hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//VM
			AdditionalBookingCommentVM additionalBookingCommentVM = new AdditionalBookingCommentVM();

			// Additional Booking Comment
			AdditionalBookingComment additionalBookingComment = new AdditionalBookingComment();
			additionalBookingComment = additionalBookingCommentRepository.GetAdditionalBookingComment(id);

			//Check Exists
			if (additionalBookingComment == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			} 
			
			additionalBookingCommentVM.AdditionalBookingComment = additionalBookingComment;
			
			// CSU
			additionalBookingCommentVM.ClientSubUnit = clientSubUnit;

			// Languages
			LanguageRepository languageRepository = new LanguageRepository();
			additionalBookingCommentVM.Languages = new SelectList(additionalBookingCommentRepository.GetAvailableLanguagesEdit(id).ToList(), "LanguageCode", "LanguageName", additionalBookingComment.LanguageCode);

			//Show Edit Form
			return View(additionalBookingCommentVM);
		}

		// POST: Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(AdditionalBookingCommentVM additionalBookingCommentVM)
		{
			string clientSubUnitGuid = additionalBookingCommentVM.ClientSubUnit.ClientSubUnitGuid;

			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

			//Check Exists
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			AdditionalBookingComment additionalBookingComment = new AdditionalBookingComment();
			additionalBookingComment = additionalBookingCommentVM.AdditionalBookingComment;
			if (additionalBookingComment == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid) || !hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update  Model from Form
			try
			{
				TryUpdateModel<AdditionalBookingComment>(additionalBookingComment, "AdditionalBookingComment");
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
				additionalBookingCommentRepository.Update(additionalBookingCommentVM);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { id = additionalBookingCommentVM.AdditionalBookingComment.BookingChannelId, csu = clientSubUnitGuid });

		}
		
		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, string csu)
		{
			//Get ClientSubUnit
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

			//Check Exists
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			}

			//Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientSubUnit(csu) || !hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//VM
			AdditionalBookingCommentVM additionalBookingCommentVM = new AdditionalBookingCommentVM();

			// Additional Booking Comment
			AdditionalBookingComment additionalBookingComment = new AdditionalBookingComment();
			additionalBookingComment = additionalBookingCommentRepository.GetAdditionalBookingComment(id);

			//Check Exists
			if (additionalBookingComment == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			}

			additionalBookingCommentVM.AdditionalBookingComment = additionalBookingComment;

			// CSU
			additionalBookingCommentVM.ClientSubUnit = clientSubUnit;

			// Languages
			LanguageRepository languageRepository = new LanguageRepository();
			additionalBookingCommentVM.Languages = new SelectList(additionalBookingCommentRepository.GetAvailableLanguages(id).ToList(), "LanguageCode", "LanguageName", additionalBookingComment.LanguageCode);

			//Show Form
			return View(additionalBookingCommentVM);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(AdditionalBookingCommentVM additionalBookingCommentVM)
		{
			string clientSubUnitGuid = additionalBookingCommentVM.ClientSubUnit.ClientSubUnitGuid;

			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

			//Check Exists
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			AdditionalBookingComment additionalBookingComment = new AdditionalBookingComment();
			additionalBookingComment = additionalBookingCommentVM.AdditionalBookingComment;
			if (additionalBookingComment == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid) || !hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Delete Item
			try
			{
				additionalBookingCommentRepository.Delete(additionalBookingComment);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = String.Format("/AdditionalBookingComment.mvc/Delete/id={0}", additionalBookingCommentVM.AdditionalBookingComment.AdditionalBookingCommentId);
					return View("VersionError");
				}
				//Generic Error
				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			//Return
			return RedirectToAction("List", new { id = additionalBookingCommentVM.AdditionalBookingComment.BookingChannelId, csu = clientSubUnitGuid });
		}

    }
}