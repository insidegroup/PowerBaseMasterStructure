using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;
using System.Xml;
using CWTDesktopDatabase.ViewModels;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace CWTDesktopDatabase.Controllers
{
    public class ChatFAQResponseItemController : Controller
    {
        //main repositories
        ChatFAQResponseGroupRepository chatFAQResponseGroupRepository = new ChatFAQResponseGroupRepository();
        ChatFAQResponseItemRepository chatFAQResponseItemRepository = new ChatFAQResponseItemRepository();

        private string groupName = "Chat FAQ Administrator";

        // GET: /List
        public ActionResult List(int id, int? page, string filter, string sortField, int? sortOrder)
        {
            //Check Parent Exists
            ChatFAQResponseGroup chatFAQResponseGroup = new ChatFAQResponseGroup();
            chatFAQResponseGroup = chatFAQResponseGroupRepository.GetGroup(id);

            //Check Exists
            if (chatFAQResponseGroup == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

			//Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToChatFAQResponseGroup(id))
            {
                ViewData["Access"] = "WriteAccess";
            }

			//Set Import Access Rights
			ViewData["ImportAccess"] = "";
			if (rolesRepository.HasWriteAccessToChatFAQResponseItemImport())
			{
				ViewData["ImportAccess"] = "WriteAccess";
			}

            //SortField+SortOrder settings
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "ChatMessageFAQName";
            }
            ViewData["CurrentSortField"] = sortField;

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

            ViewData["ChatFAQResponseGroupId"] = chatFAQResponseGroup.ChatFAQResponseGroupId;
            ViewData["ChatFAQResponseGroupName"] = chatFAQResponseGroup.ChatFAQResponseGroupName;

            //return items
            var cwtPaginatedList = chatFAQResponseItemRepository.PageChatFAQResponseItems(id, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

            return View(cwtPaginatedList);
        }

        // GET: /Create
        public ActionResult Create(int id)
        {
            //Check Parent Exists
            ChatFAQResponseGroup chatFAQResponseGroup = new ChatFAQResponseGroup();
            chatFAQResponseGroup = chatFAQResponseGroupRepository.GetGroup(id);

            //Check Exists
            if (chatFAQResponseGroup == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToChatFAQResponseGroup(id))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ChatFAQResponseItem chatFAQResponseItem = new ChatFAQResponseItem();
            chatFAQResponseItem.ChatFAQResponseGroupName = chatFAQResponseGroup.ChatFAQResponseGroupName;
            chatFAQResponseItem.ChatFAQResponseGroupId = id;

            //Get Available ChatMessageFAQs
            ChatMessageFAQRepository chatMessageFAQRepository = new ChatMessageFAQRepository();
            SelectList chatMessageFAQs = new SelectList(chatMessageFAQRepository.GetChatFAQResponseItemAvailableChatMessageFAQs(null, chatFAQResponseItem.ChatFAQResponseGroupId).ToList(), "ChatMessageFAQId", "ChatMessageFAQName");
            ViewData["ChatMessageFAQs"] = chatMessageFAQs;

            ViewData["ChatFAQResponseGroupId"] = chatFAQResponseGroup.ChatFAQResponseGroupId;
            ViewData["ChatFAQResponseGroupName"] = chatFAQResponseGroup.ChatFAQResponseGroupName;

            chatFAQResponseItemRepository.EditItemForDisplay(chatFAQResponseItem);

            return View(chatFAQResponseItem);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ChatFAQResponseItem chatFAQResponseItem)
        {

            //Get ChatFAQResponseGroup
            ChatFAQResponseGroup chatFAQResponseGroup = new ChatFAQResponseGroup();
            chatFAQResponseGroup = chatFAQResponseGroupRepository.GetGroup(chatFAQResponseItem.ChatFAQResponseGroupId);

            //Check Exists
            if (chatFAQResponseGroup == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }
            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToChatFAQResponseGroup(chatFAQResponseItem.ChatFAQResponseGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            try
            {
                UpdateModel(chatFAQResponseItem);
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
                chatFAQResponseItemRepository.Add(chatFAQResponseItem);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = chatFAQResponseItem.ChatFAQResponseGroupId });
        }

        // GET: /Edit
        public ActionResult Edit(int id)
        {
            //Get Item
            ChatFAQResponseItem chatFAQResponseItem = new ChatFAQResponseItem();
            chatFAQResponseItem = chatFAQResponseItemRepository.GetItem(id);

            //Check Exists
            if (chatFAQResponseItem == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check Parent Exists
            ChatFAQResponseGroup chatFAQResponseGroup = new ChatFAQResponseGroup();
            chatFAQResponseGroup = chatFAQResponseGroupRepository.GetGroup(chatFAQResponseItem.ChatFAQResponseGroupId);

            //Check Exists
            if (chatFAQResponseGroup == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }
            
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToChatFAQResponseGroup(chatFAQResponseItem.ChatFAQResponseGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Get Available ChatMessageFAQs
            ChatMessageFAQRepository chatMessageFAQRepository = new ChatMessageFAQRepository();
            SelectList chatMessageFAQs = new SelectList(
                chatMessageFAQRepository.GetChatFAQResponseItemAvailableChatMessageFAQs(chatFAQResponseItem.ChatFAQResponseItemId, chatFAQResponseItem.ChatFAQResponseGroupId).ToList(), 
                "ChatMessageFAQId", 
                "ChatMessageFAQName",
                chatFAQResponseItem.ChatMessageFAQId
            );
            ViewData["ChatMessageFAQs"] = chatMessageFAQs;

            //Parent Information
            ViewData["ChatFAQResponseGroupId"] = chatFAQResponseGroup.ChatFAQResponseGroupId;
            ViewData["ChatFAQResponseGroupName"] = chatFAQResponseGroup.ChatFAQResponseGroupName;

            chatFAQResponseItemRepository.EditItemForDisplay(chatFAQResponseItem);

            return View(chatFAQResponseItem);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            //Get Item
            ChatFAQResponseItem chatFAQResponseItem = new ChatFAQResponseItem();
            chatFAQResponseItem = chatFAQResponseItemRepository.GetItem(id);

            //Check Exists
            if (chatFAQResponseItem == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToChatFAQResponseGroup(chatFAQResponseItem.ChatFAQResponseGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            try
            {
                UpdateModel(chatFAQResponseItem);
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
                chatFAQResponseItemRepository.Edit(chatFAQResponseItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ChatFAQResponseItem.mvc/Edit/" + chatFAQResponseItem.ChatFAQResponseItemId.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = chatFAQResponseItem.ChatFAQResponseGroupId });
        }
        
        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            //Get Item
            ChatFAQResponseItem chatFAQResponseItem = new ChatFAQResponseItem();
            chatFAQResponseItem = chatFAQResponseItemRepository.GetItem(id);

            //Check Exists
            if (chatFAQResponseItem == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check Parent Exists
            ChatFAQResponseGroup chatFAQResponseGroup = new ChatFAQResponseGroup();
            chatFAQResponseGroup = chatFAQResponseGroupRepository.GetGroup(chatFAQResponseItem.ChatFAQResponseGroupId);

            //Check Exists
            if (chatFAQResponseGroup == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }
            
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToChatFAQResponseGroup(chatFAQResponseItem.ChatFAQResponseGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Parent Information
            ViewData["ChatFAQResponseGroupId"] = chatFAQResponseGroup.ChatFAQResponseGroupId;
            ViewData["ChatFAQResponseGroupName"] = chatFAQResponseGroup.ChatFAQResponseGroupName;

            chatFAQResponseItemRepository.EditItemForDisplay(chatFAQResponseItem);

            return View(chatFAQResponseItem);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //Get Item
            ChatFAQResponseItem chatFAQResponseItem = new ChatFAQResponseItem();
            chatFAQResponseItem = chatFAQResponseItemRepository.GetItem(id);

            //Check Exists
            if (chatFAQResponseItem == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToChatFAQResponseGroup(chatFAQResponseItem.ChatFAQResponseGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                chatFAQResponseItem.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                chatFAQResponseItemRepository.Delete(chatFAQResponseItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ChatFAQResponseItem.mvc/Delete/" + chatFAQResponseItem.ChatFAQResponseItemId;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            //Return
            return RedirectToAction("List", new { id = chatFAQResponseItem.ChatFAQResponseGroupId });
        }
		
		// GET: /Export
		public ActionResult Export(int id)
		{
			//Get Item
			ChatFAQResponseGroup chatFAQResponseGroup = new ChatFAQResponseGroup();
			chatFAQResponseGroup = chatFAQResponseGroupRepository.GetGroup(id);

			//Check Exists
			if (chatFAQResponseGroup == null)
			{
				ViewData["ActionMethod"] = "ExportGet";
				return View("RecordDoesNotExistError");
			}

			string filename = string.Format("{0}-Export.csv", chatFAQResponseGroup.ChatFAQResponseGroupName);

			//Get CSV Data
			byte[] csvData = chatFAQResponseItemRepository.Export(id);
			return File(csvData, "text/csv", filename);
		}


		// GET: /ExportErrors
		public ActionResult ExportErrors()
		{
			var preImportCheckResultVM = (ChatFAQResponseItemImportStep1VM)TempData["ErrorMessages"];

			if (preImportCheckResultVM == null)
			{
				ViewData["ActionMethod"] = "ExportGet";
				return View("RecordDoesNotExistError");
			}

			ChatFAQResponseGroup chatFAQResponseGroup = new ChatFAQResponseGroup();
			chatFAQResponseGroup = chatFAQResponseGroupRepository.GetGroup(preImportCheckResultVM.ChatFAQResponseGroupId);

			//Check Exists
			if (chatFAQResponseGroup == null)
			{
				ViewData["ActionMethod"] = "ExportGet";
				return View("RecordDoesNotExistError");
			} 

			//Get CSV Data
			var errors = preImportCheckResultVM.ImportStep2VM.ReturnMessages;
			var combinedErrors = errors.Aggregate((current, next) => current + "\r\n" + next);
			byte[] csvData = Encoding.ASCII.GetBytes(combinedErrors);

			return File(csvData, "text/plain", string.Format("{0}-ErrorExport.txt", chatFAQResponseGroup.ChatFAQResponseGroupName));
		}

		public ActionResult ImportStep1(int id)
		{
			ChatFAQResponseGroup chatFAQResponseGroup = new ChatFAQResponseGroup();
			chatFAQResponseGroup = chatFAQResponseGroupRepository.GetGroup(id);

			//Check Exists
			if (chatFAQResponseGroup == null)
			{
				ViewData["ActionMethod"] = "ExportGet";
				return View("RecordDoesNotExistError");
			} 
			
			ChatFAQResponseItemImportStep1WithFileVM clientSubUnitImportStep1WithFileVM = new ChatFAQResponseItemImportStep1WithFileVM();
			clientSubUnitImportStep1WithFileVM.ChatFAQResponseGroupId = id;
			clientSubUnitImportStep1WithFileVM.ChatFAQResponseGroup = chatFAQResponseGroup;

			return View(clientSubUnitImportStep1WithFileVM);
		}

		[HttpPost]
		public ActionResult ImportStep1(ChatFAQResponseItemImportStep1WithFileVM csvfile)
		{
			//used for return only
			ChatFAQResponseGroup chatFAQResponseGroup = new ChatFAQResponseGroup();
			chatFAQResponseGroup = chatFAQResponseGroupRepository.GetGroup(csvfile.ChatFAQResponseGroupId); 
			
			if (!ModelState.IsValid)
			{
				return View(csvfile);
			}
			string fileExtension = Path.GetExtension(csvfile.File.FileName);
			if (fileExtension != ".csv")
			{
				ModelState.AddModelError("file", "This is not a valid entry");
				return View(csvfile);
			}

			if (csvfile.File.ContentLength > 0)
			{
				ChatFAQResponseItemImportStep2VM preImportCheckResult = new ChatFAQResponseItemImportStep2VM();
				List<string> returnMessages = new List<string>();

				preImportCheckResult = chatFAQResponseItemRepository.PreImportCheck(csvfile.File, csvfile.ChatFAQResponseGroupId);

				ChatFAQResponseItemImportStep1VM preImportCheckResultVM = new ChatFAQResponseItemImportStep1VM();
				preImportCheckResultVM.ChatFAQResponseGroup = chatFAQResponseGroup;
				preImportCheckResultVM.ImportStep2VM = preImportCheckResult;
				preImportCheckResultVM.ChatFAQResponseGroupId = csvfile.ChatFAQResponseGroupId;

				TempData["PreImportCheckResultVM"] = preImportCheckResultVM;
				return RedirectToAction("ImportStep2");
			}

			return View();
		}

		public ActionResult ImportStep2()
		{
			ChatFAQResponseItemImportStep1VM preImportCheckResultVM = new ChatFAQResponseItemImportStep1VM();
			preImportCheckResultVM = (ChatFAQResponseItemImportStep1VM)TempData["PreImportCheckResultVM"];

			ChatFAQResponseGroup chatFAQResponseGroup = new ChatFAQResponseGroup();
			chatFAQResponseGroup = chatFAQResponseGroupRepository.GetGroup(preImportCheckResultVM.ChatFAQResponseGroupId);
			preImportCheckResultVM.ChatFAQResponseGroup = chatFAQResponseGroup; 
			
			return View(preImportCheckResultVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ImportStep2(ChatFAQResponseItemImportStep1VM preImportCheckResultVM)
		{
			if (preImportCheckResultVM.ImportStep2VM.IsValidData == false)
			{
				//Check JSON for valid messages
				if (preImportCheckResultVM.ImportStep2VM.ReturnMessages[0] != null)
				{
					List<string> returnMessages = new List<string>();

					var settings = new JsonSerializerSettings
					{
						StringEscapeHandling = StringEscapeHandling.EscapeHtml,
					};

					List<string> returnMessagesJSON = JsonConvert.DeserializeObject<List<string>>(preImportCheckResultVM.ImportStep2VM.ReturnMessages[0], settings);

					foreach (string message in returnMessagesJSON)
					{
						string validMessage = Regex.Replace(message, @"[^À-ÿ\w\s&:._()\-]", "");

						if (!string.IsNullOrEmpty(validMessage))
						{
							returnMessages.Add(validMessage);
						}
					}

					preImportCheckResultVM.ImportStep2VM.ReturnMessages = returnMessages;
				}

				TempData["ErrorMessages"] = preImportCheckResultVM;

				return RedirectToAction("ExportErrors");
			}

			//PreImport Check Results (check has passed)
			ChatFAQResponseItemImportStep2VM preImportCheckResult = new ChatFAQResponseItemImportStep2VM();
			preImportCheckResult = preImportCheckResultVM.ImportStep2VM;

			//Do the Import, return results
			ChatFAQResponseItemImportStep3VM postImportResult = new ChatFAQResponseItemImportStep3VM();
			postImportResult = chatFAQResponseItemRepository.Import(
				preImportCheckResult.FileBytes,
				preImportCheckResultVM.ChatFAQResponseGroupId
			);

			postImportResult.ChatFAQResponseGroupId = preImportCheckResultVM.ChatFAQResponseGroupId;
			TempData["PostImportResult"] = postImportResult;

			//Pass Results to Next Page
			return RedirectToAction("ImportStep3");

		}

		public ActionResult ImportStep3()
		{
			//Display Results of Import
			ChatFAQResponseItemImportStep3VM cdrPostImportResult = new ChatFAQResponseItemImportStep3VM();
			cdrPostImportResult = (ChatFAQResponseItemImportStep3VM)TempData["PostImportResult"];

			ChatFAQResponseGroup chatFAQResponseGroup = new ChatFAQResponseGroup();
			chatFAQResponseGroup = chatFAQResponseGroupRepository.GetGroup(cdrPostImportResult.ChatFAQResponseGroupId);
			cdrPostImportResult.ChatFAQResponseGroup = chatFAQResponseGroup;
			
			return View(cdrPostImportResult);
		}
    }
}
