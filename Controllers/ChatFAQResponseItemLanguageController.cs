using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class ChatFAQResponseItemLanguageController : Controller
    {
        ChatFAQResponseItemRepository chatFAQResponseItemRepository = new ChatFAQResponseItemRepository();
        ChatFAQResponseItemLanguageRepository chatFAQResponseItemLanguageRepository = new ChatFAQResponseItemLanguageRepository();

        private string groupName = "Chat FAQ Administrator";

        //GET:List
        public ActionResult List(int id, int? page, string sortField, int? sortOrder)
        {
            //Get ChatFAQResponseItem
            ChatFAQResponseItem chatFAQResponseItem = new ChatFAQResponseItem();
            chatFAQResponseItem = chatFAQResponseItemRepository.GetItem(id);

            //Check Exists
            if (chatFAQResponseItem == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //Set Access Rights
            HierarchyRepository hierarchyRepository = new HierarchyRepository();
            ViewData["Access"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //SortField+SortOrder settings
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "LanguageName";
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

            //ChatFAQResponseItem
            chatFAQResponseItemRepository.EditItemForDisplay(chatFAQResponseItem);

            //View data
            ViewData["ChatFAQResponseItemId"] = id;
            ViewData["ChatFAQResponseItemDescription"] = chatFAQResponseItem.ChatFAQResponseItemDescription;
            ViewData["ChatFAQResponseGroupId"] = chatFAQResponseItem.ChatFAQResponseGroup.ChatFAQResponseGroupId;
            ViewData["ChatFAQResponseGroupName"] = chatFAQResponseItem.ChatFAQResponseGroup.ChatFAQResponseGroupName;
            ViewData["ChatMessageFAQName"] = chatFAQResponseItem.ChatMessageFAQName;

            //Get data
            var cwtPaginatedList = chatFAQResponseItemLanguageRepository.PageChatFAQResponseItemLanguages(id, page ?? 1, sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        // GET: /Create
        public ActionResult Create(int id)
        {
            //Get ChatFAQResponseItem
            ChatFAQResponseItem chatFAQResponseItem = new ChatFAQResponseItem();
            chatFAQResponseItem = chatFAQResponseItemRepository.GetItem(id);

            //Check Exists
            if (chatFAQResponseItem == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            chatFAQResponseItemRepository.EditItemForDisplay(chatFAQResponseItem);

            //AccessRights
            HierarchyRepository hierarchyRepository = new HierarchyRepository();
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //ChatFAQResponseItem
            chatFAQResponseItemRepository.EditItemForDisplay(chatFAQResponseItem);

            //ChatFAQResponseItemLanguage
            ChatFAQResponseItemLanguage chatFAQResponseItemLanguage = new ChatFAQResponseItemLanguage();
            chatFAQResponseItemLanguage.ChatFAQResponseItemId = id;
            chatFAQResponseItemLanguage.ChatFAQResponseItem = chatFAQResponseItem;
            chatFAQResponseItemLanguageRepository.EditItemForDisplay(chatFAQResponseItemLanguage);

            //Language SelectList
            SelectList languageList = new SelectList(chatFAQResponseItemLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            //View data
            ViewData["ChatFAQResponseItemId"] = id;
            ViewData["ChatFAQResponseItemDescription"] = chatFAQResponseItem.ChatFAQResponseItemDescription;
            ViewData["ChatFAQResponseGroupId"] = chatFAQResponseItem.ChatFAQResponseGroup.ChatFAQResponseGroupId;
            ViewData["ChatFAQResponseGroupName"] = chatFAQResponseItem.ChatFAQResponseGroup.ChatFAQResponseGroupName;
            ViewData["ChatMessageFAQName"] = chatFAQResponseItem.ChatMessageFAQName;

            //Show Create Form
            return View(chatFAQResponseItemLanguage);


        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ChatFAQResponseItemLanguage chatFAQResponseItemLanguage)
        {
            //Get ChatFAQResponseItem
            ChatFAQResponseItem chatFAQResponseItem = new ChatFAQResponseItem();
            chatFAQResponseItem = chatFAQResponseItemRepository.GetItem(chatFAQResponseItemLanguage.ChatFAQResponseItemId);

            //Check Exists
            if (chatFAQResponseItem == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            HierarchyRepository hierarchyRepository = new HierarchyRepository();
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(chatFAQResponseItemLanguage);
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


            chatFAQResponseItemLanguageRepository.Add(chatFAQResponseItemLanguage);

            return RedirectToAction("List", new { id = chatFAQResponseItemLanguage.ChatFAQResponseItemId });
        }

        // GET: /Edit
        public ActionResult Edit(int id, string languageCode)
        {
            //Get Item 
            ChatFAQResponseItemLanguage chatFAQResponseItemLanguage = new ChatFAQResponseItemLanguage();
            chatFAQResponseItemLanguage = chatFAQResponseItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (chatFAQResponseItemLanguage == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            HierarchyRepository hierarchyRepository = new HierarchyRepository();
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Get ChatFAQResponseItem
            ChatFAQResponseItem chatFAQResponseItem = new ChatFAQResponseItem();
            chatFAQResponseItem = chatFAQResponseItemRepository.GetItem(chatFAQResponseItemLanguage.ChatFAQResponseItemId);

            //Check Exists
            if (chatFAQResponseItem == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //ChatFAQResponseItem
            chatFAQResponseItemRepository.EditItemForDisplay(chatFAQResponseItem);

            //ChatFAQResponseItemLanguage
            chatFAQResponseItemLanguage.ChatFAQResponseItemId = id;
            chatFAQResponseItemLanguage.ChatFAQResponseItem = chatFAQResponseItem;
            chatFAQResponseItemLanguageRepository.EditItemForDisplay(chatFAQResponseItemLanguage);

            //Language SelectList
            List<Language> availableLanguages = chatFAQResponseItemLanguageRepository.GetUnUsedLanguages(id).ToList();
            if (chatFAQResponseItemLanguage.LanguageCode != null)
            {
                LanguageRepository languageRepository = new LanguageRepository();
                Language langage = languageRepository.GetLanguage(chatFAQResponseItemLanguage.LanguageCode);
                if (langage != null)
                {
                    availableLanguages.Add(langage);
                }
            }

            SelectList languageList = new SelectList(availableLanguages, "LanguageCode", "LanguageName", chatFAQResponseItemLanguage.LanguageCode);
            ViewData["Languages"] = languageList;

            //View data
            ViewData["ChatFAQResponseItemId"] = id;
            ViewData["ChatFAQResponseItemDescription"] = chatFAQResponseItem.ChatFAQResponseItemDescription;
            ViewData["ChatFAQResponseGroupId"] = chatFAQResponseItem.ChatFAQResponseGroup.ChatFAQResponseGroupId;
            ViewData["ChatFAQResponseGroupName"] = chatFAQResponseItem.ChatFAQResponseGroup.ChatFAQResponseGroupName;
            ViewData["ChatMessageFAQName"] = chatFAQResponseItem.ChatMessageFAQName;

            return View(chatFAQResponseItemLanguage);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string languageCode, FormCollection formCollection)
        {
            //Get Item 
            ChatFAQResponseItemLanguage chatFAQResponseItemLanguage = new ChatFAQResponseItemLanguage();
            chatFAQResponseItemLanguage = chatFAQResponseItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (chatFAQResponseItemLanguage == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            HierarchyRepository hierarchyRepository = new HierarchyRepository();
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Item from Form
            try
            {
                UpdateModel(chatFAQResponseItemLanguage);

                if (!string.IsNullOrEmpty(formCollection["NewLanguageCode"]))
                {
                    chatFAQResponseItemLanguage.LanguageCode = formCollection["NewLanguageCode"];
                }
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

            //Update
            try
            {
                chatFAQResponseItemLanguageRepository.Update(chatFAQResponseItemLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ChatFAQResponseItemLanguage.mvc/Edit?id=" + chatFAQResponseItemLanguage.ChatFAQResponseItemId + "&languageCode=" + chatFAQResponseItemLanguage.LanguageCode;
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = chatFAQResponseItemLanguage.ChatFAQResponseItemId });
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, string languageCode)
        {
            //Get Item 
            ChatFAQResponseItemLanguage chatFAQResponseItemLanguage = new ChatFAQResponseItemLanguage();
            chatFAQResponseItemLanguage = chatFAQResponseItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (chatFAQResponseItemLanguage == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            HierarchyRepository hierarchyRepository = new HierarchyRepository();
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Get ChatFAQResponseItem
            ChatFAQResponseItem chatFAQResponseItem = new ChatFAQResponseItem();
            chatFAQResponseItem = chatFAQResponseItemRepository.GetItem(chatFAQResponseItemLanguage.ChatFAQResponseItemId);

            //Check Exists
            if (chatFAQResponseItem == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //ChatFAQResponseItem
            chatFAQResponseItemRepository.EditItemForDisplay(chatFAQResponseItem);

            //ChatFAQResponseItemLanguage
            chatFAQResponseItemLanguage.ChatFAQResponseItemId = id;
            chatFAQResponseItemLanguage.ChatFAQResponseItem = chatFAQResponseItem;
            chatFAQResponseItemLanguageRepository.EditItemForDisplay(chatFAQResponseItemLanguage);

            //Language SelectList
            SelectList languageList = new SelectList(chatFAQResponseItemLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            //View data
            ViewData["ChatFAQResponseItemId"] = id;
            ViewData["ChatFAQResponseItemDescription"] = chatFAQResponseItem.ChatFAQResponseItemDescription;
            ViewData["ChatFAQResponseGroupId"] = chatFAQResponseItem.ChatFAQResponseGroup.ChatFAQResponseGroupId;
            ViewData["ChatFAQResponseGroupName"] = chatFAQResponseItem.ChatFAQResponseGroup.ChatFAQResponseGroupName;
            ViewData["ChatMessageFAQName"] = chatFAQResponseItem.ChatMessageFAQName;

            return View(chatFAQResponseItemLanguage);

        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, string languageCode, FormCollection collection)
        {
            //Get Item 
            ChatFAQResponseItemLanguage chatFAQResponseItemLanguage = new ChatFAQResponseItemLanguage();
            chatFAQResponseItemLanguage = chatFAQResponseItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (chatFAQResponseItemLanguage == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            HierarchyRepository hierarchyRepository = new HierarchyRepository();
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                chatFAQResponseItemLanguage.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                chatFAQResponseItemLanguageRepository.Delete(chatFAQResponseItemLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ChatFAQResponseItemLanguage.mvc/Delete?id=" + chatFAQResponseItemLanguage.ChatFAQResponseItemId + "&languageCode=" + chatFAQResponseItemLanguage.LanguageCode;
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            //Return
            return RedirectToAction("List", new { id = chatFAQResponseItemLanguage.ChatFAQResponseItemId });
        }
    }
}
