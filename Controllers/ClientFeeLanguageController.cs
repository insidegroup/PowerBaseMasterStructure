using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;
using System;

namespace CWTDesktopDatabase.Controllers
{
    public class ClientFeeLanguageController : Controller
    {
        //main repositories
        ClientFeeLanguageRepository clientFeeLanguageRepository = new ClientFeeLanguageRepository();
        ClientFeeRepository clientFeeRepository = new ClientFeeRepository();

        //GET:List
        public ActionResult List(int id, int? page, string sortField, int? sortOrder)
        {
            //Get ClientFee
            ClientFee clientFee = new ClientFee();
            clientFee = clientFeeRepository.GetItem(id);

            //Check Exists
            if (clientFee == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //SortField+SortOrder settings
            if (sortField != "ClientFeeLanguageDescription")
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

            //Get data
            ClientFeeLanguagesVM clientFeeLanguagesVM = new ClientFeeLanguagesVM();
            clientFeeLanguagesVM.ClientFeeLanguages = clientFeeLanguageRepository.PageClientFeeLanguageDescription(id, page ?? 1, sortField, sortOrder ?? 0);
            clientFeeLanguagesVM.ClientFee = clientFee;

            return View(clientFeeLanguagesVM);
        }

        //GET: View
        public ActionResult View(int id, string languageCode)
        {
            ClientFeeLanguage clientFeeLanguage = new ClientFeeLanguage();
            clientFeeLanguage = clientFeeLanguageRepository.GetItem(id, languageCode);
            if (clientFeeLanguage == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }
            return View(clientFeeLanguage);
        }

        // GET: /Create
        public ActionResult Create(int id)
        {
            //Get PolicyCarVendorGroupItem
            ClientFee clientFee = new ClientFee();
            clientFee = clientFeeRepository.GetItem(id);

            //Check Exists
            if (clientFee == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //New PolicyCarVendorGroupItemLanguage
            ClientFeeLanguage clientFeeLanguage = new ClientFeeLanguage();
            clientFeeLanguage.ClientFee = clientFee;

            //Language SelectList
            SelectList languageList = new SelectList(clientFeeLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            //Show Create Form
            return View(clientFeeLanguage);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientFeeLanguage clientFeeLanguage)
        {
            ClientFee clientFee = new ClientFee();
            clientFee = clientFeeRepository.GetItem(clientFeeLanguage.ClientFeeId);

            //Check Exists
            if (clientFee == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }


            //Update  Model from Form
            try
            {
                UpdateModel(clientFeeLanguage);
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
                clientFeeLanguageRepository.Add(clientFeeLanguage);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = clientFeeLanguage.ClientFeeId });
        }

        // GET: /Edit
        public ActionResult Edit(int id, string languageCode)
        {
            //Get Item 
            ClientFeeLanguage clientFeeLanguage = new ClientFeeLanguage();
            clientFeeLanguage = clientFeeLanguageRepository.GetItem(id, languageCode);
            if (clientFeeLanguage == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Language SelectList
            SelectList languageList = new SelectList(clientFeeLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            return View(clientFeeLanguage);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int clientFeeId, string languageCode, string carAdvice)
        {
            //Get Item 
            ClientFeeLanguage clientFeeLanguage = new ClientFeeLanguage();
            clientFeeLanguage = clientFeeLanguageRepository.GetItem(clientFeeId, languageCode);

            //Check Exists
            if (clientFeeLanguage == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }


            //Update Item from Form
            try
            {
                UpdateModel(clientFeeLanguage);
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




            //Update CountryAdvice
            try
            {
                clientFeeLanguageRepository.Update(clientFeeLanguage);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/CarAdvice.mvc/Edit/" + clientFeeLanguage.ClientFeeId.ToString() + "/" + languageCode;
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = clientFeeLanguage.ClientFeeId });
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, string languageCode)
        {
            //Get Item 
            ClientFeeLanguage clientFeeLanguage = new ClientFeeLanguage();
            clientFeeLanguage = clientFeeLanguageRepository.GetItem(id, languageCode);
            if (clientFeeLanguage == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Return View
            return View(clientFeeLanguage);

        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, string languageCode, FormCollection collection)
        {
            //Get Item 
            ClientFeeLanguage clientFeeLanguage = new ClientFeeLanguage();
            clientFeeLanguage = clientFeeLanguageRepository.GetItem(id, languageCode);
            if (clientFeeLanguage == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }


            //Delete Item
            try
            {
                clientFeeLanguage.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                clientFeeLanguageRepository.Delete(clientFeeLanguage);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ClientFeeLanguage.mvc/Delete/" + clientFeeLanguage.ClientFeeId.ToString() + "/" + clientFeeLanguage.LanguageCode;
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            //Return
            return RedirectToAction("List", new { id = clientFeeLanguage.ClientFeeId });
        }
    }
}
