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
    public class ChatMessageFAQController : Controller
    {
		ChatMessageFAQRepository chatMessageFAQRepository = new ChatMessageFAQRepository();

		HierarchyRepository hierarchyRepository = new HierarchyRepository();

		private string groupName = "Reference Info";
		
		// GET: /List
        public ActionResult List(string filter, int? page, string sortField, int? sortOrder)
        {
            RolesRepository rolesRepository = new RolesRepository();

            //Set Access Rights
            ViewData["Access"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Access"] = "WriteAccess";
            }
    
            //SortField
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "ChatMessageFAQId";
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
            var cwtPaginatedList = chatMessageFAQRepository.PageChatMessageFAQs(page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

            return View(cwtPaginatedList);
        }

        // GET: /View
        public ActionResult ViewItem(int id)
        {
            //Check Exists
            ChatMessageFAQ chatMessageFAQ = new ChatMessageFAQ();
            chatMessageFAQ = chatMessageFAQRepository.GetChatMessageFAQ(id);
            if (chatMessageFAQ == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            chatMessageFAQRepository.EditForDisplay(chatMessageFAQ);
            return View(chatMessageFAQ);
        }

		// GET: /Create
		[HttpGet]
		public ActionResult Create()
		{
			//Create Item 
			ChatMessageFAQ chatMessageFAQ = new ChatMessageFAQ();

			//AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			return View(chatMessageFAQ);
		}

		// POST: //Create/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ChatMessageFAQ chatMessageFAQ)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update  Model from Form
			try
			{
				UpdateModel(chatMessageFAQ);
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
				chatMessageFAQRepository.Add(chatMessageFAQ);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List");
		}

		// GET: /Edit
		public ActionResult Edit(int id)
		{
			//Get Item 
			ChatMessageFAQ chatMessageFAQ = new ChatMessageFAQ();
			chatMessageFAQ = chatMessageFAQRepository.GetChatMessageFAQ(id);

			//Check Exists
			if (chatMessageFAQ == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			chatMessageFAQRepository.EditForDisplay(chatMessageFAQ);

			return View(chatMessageFAQ);
		}

		// POST: //Edit/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(ChatMessageFAQ chatMessageFAQ)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update  Model from Form
			try
			{
				UpdateModel(chatMessageFAQ);
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
				chatMessageFAQRepository.Update(chatMessageFAQ);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List");
		}

		// GET: Delete
		[HttpGet]
		public ActionResult Delete(int id)
		{
			ChatMessageFAQVM chatMessageFAQVM = new ChatMessageFAQVM();

			ChatMessageFAQ chatMessageFAQ = new ChatMessageFAQ();
			chatMessageFAQ = chatMessageFAQRepository.GetChatMessageFAQ(id);

			//Check Exists
			if (chatMessageFAQ == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			chatMessageFAQVM.ChatMessageFAQ = chatMessageFAQ;

            //Attached Items
            List<ChatMessageFAQReference> chatMessageFAQReferences = chatMessageFAQRepository.GetChatMessageFAQReferences(chatMessageFAQ.ChatMessageFAQId);
            if (chatMessageFAQReferences.Count > 0)
            {
                chatMessageFAQVM.AllowDelete = false;
                chatMessageFAQVM.ChatMessageFAQReferences = chatMessageFAQReferences;
            }

            chatMessageFAQRepository.EditForDisplay(chatMessageFAQ);

			return View(chatMessageFAQVM);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(ChatMessageFAQVM chatMessageFAQVM)
		{
			//Get Item 
			ChatMessageFAQ chatMessageFAQ = new ChatMessageFAQ();
			chatMessageFAQ = chatMessageFAQRepository.GetChatMessageFAQ(chatMessageFAQVM.ChatMessageFAQ.ChatMessageFAQId);

			//Check Exists
			if (chatMessageFAQVM.ChatMessageFAQ == null)
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

			//Delete Item
			try
			{
				chatMessageFAQRepository.Delete(chatMessageFAQ);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/ChatMessageFAQ.mvc/Delete/" + chatMessageFAQ.ChatMessageFAQId;
					return View("VersionError");
				}

				//Generic Error
				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			//Return
			return RedirectToAction("List");
		}
    }
}
