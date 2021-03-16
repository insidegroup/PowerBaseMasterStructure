using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class ReasonCodeProductTypeDescriptionController : Controller
    {
        ReasonCodeProductTypeDescriptionRepository reasonCodeProductTypeDescriptionRepository = new ReasonCodeProductTypeDescriptionRepository();
        ReasonCodeProductTypeRepository reasonCodeProductTypeRepository = new ReasonCodeProductTypeRepository();

        public ActionResult List(string reasonCode, int productId, int reasonCodeTypeId, int? page, string sortField, int? sortOrder)
        {
            //Get ReasonCodeProductType
            ReasonCodeProductType reasonCodeProductType = new ReasonCodeProductType();
            reasonCodeProductType = reasonCodeProductTypeRepository.GetReasonCodeProductType(reasonCode, productId, reasonCodeTypeId);

            //Check Exists
            if (reasonCodeProductType == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //Set Access Rights
            ViewData["Access"] = "WriteAccess";

            //Parent Information
            ViewData["ReasonCode"] = reasonCode;
            ViewData["ReasonCodeTypeId"] = reasonCodeTypeId;
            ViewData["ProductId"] = productId;

            ProductRepository productRepository = new ProductRepository();
            Product product = new Product();
            product = productRepository.GetProduct(productId);
            ReasonCodeTypeRepository reasonCodeTypeRepository = new ReasonCodeTypeRepository();
            ReasonCodeType reasonCodeType = new ReasonCodeType();
            reasonCodeType = reasonCodeTypeRepository.GetItem(reasonCodeTypeId);
            ReasonCodeItemRepository reasonCodeItemRepository = new ReasonCodeItemRepository();
            ReasonCodeItem reasonCodeItem = new ReasonCodeItem();
            reasonCodeItem = reasonCodeItemRepository.GetItem(1);
            ViewData["ReasonCodeItem"] = reasonCode + "/" + reasonCodeType.ReasonCodeTypeDescription + "/" + product.ProductName;


            //SortField+SortOrder settings
            if (sortField != "ReasonCodeProductTypeDescription")
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
            var cwtPaginatedList = reasonCodeProductTypeDescriptionRepository.PageReasonCodeProductTypeDescriptions(reasonCode, productId, reasonCodeTypeId, page ?? 1, sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        //GET: View
        public ActionResult View(string reasonCode, int productId, int reasonCodeTypeId, string languageCode)
        {
            ReasonCodeProductTypeDescription reasonCodeProductTypeDescription = new ReasonCodeProductTypeDescription();
            reasonCodeProductTypeDescription = reasonCodeProductTypeDescriptionRepository.GetItem(languageCode, reasonCode, productId, reasonCodeTypeId);
            if (reasonCodeProductTypeDescription == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }
            reasonCodeProductTypeDescriptionRepository.EditItemForDisplay(reasonCodeProductTypeDescription);
            return View(reasonCodeProductTypeDescription);
        }

        // GET: /Create
        public ActionResult Create(string reasonCode, int productId, int reasonCodeTypeId)
        {
            //Get Item 
            ReasonCodeProductType reasonCodeProductType = new ReasonCodeProductType();
            reasonCodeProductType = reasonCodeProductTypeRepository.GetReasonCodeProductType(reasonCode, productId, reasonCodeTypeId);

            //Check Exists
            if (reasonCodeProductType == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //New ReasonCodeProductTypeDescription
            ReasonCodeProductTypeDescription reasonCodeProductTypeDescription = new ReasonCodeProductTypeDescription();
            reasonCodeProductTypeDescription.ReasonCode = reasonCode;
            reasonCodeProductTypeDescription.ProductId = productId;
            reasonCodeProductTypeDescription.ReasonCodeTypeId = reasonCodeTypeId;
            reasonCodeProductTypeDescriptionRepository.EditItemForDisplay(reasonCodeProductTypeDescription);

            ProductRepository productRepository = new ProductRepository();
            Product product = new Product();
            product = productRepository.GetProduct(productId);
            ReasonCodeTypeRepository reasonCodeTypeRepository = new ReasonCodeTypeRepository();
            ReasonCodeType reasonCodeType = new ReasonCodeType();
            reasonCodeType = reasonCodeTypeRepository.GetItem(reasonCodeTypeId);
            ReasonCodeItemRepository reasonCodeItemRepository = new ReasonCodeItemRepository();
            ReasonCodeItem reasonCodeItem = new ReasonCodeItem();
            reasonCodeItem = reasonCodeItemRepository.GetItem(1);
            ViewData["ReasonCodeItem"] = reasonCode + "/" + reasonCodeType.ReasonCodeTypeDescription + "/" + product.ProductName;
            //Language SelectList
            SelectList languageList = new SelectList(reasonCodeProductTypeDescriptionRepository.GetUnUsedLanguages(reasonCode, productId, reasonCodeTypeId).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            //Show Create Form
            return View(reasonCodeProductTypeDescription);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReasonCodeProductTypeDescription reasonCodeProductTypeDescription)
        {
            //Get Item 
            ReasonCodeProductType reasonCodeProductType = new ReasonCodeProductType();
            reasonCodeProductType = reasonCodeProductTypeRepository.GetReasonCodeProductType(reasonCodeProductTypeDescription.ReasonCode, reasonCodeProductTypeDescription.ProductId, reasonCodeProductTypeDescription.ReasonCodeTypeId);

            //Check Exists
            if (reasonCodeProductType == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(reasonCodeProductTypeDescription);
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


            reasonCodeProductTypeDescriptionRepository.Add(reasonCodeProductTypeDescription);

            return RedirectToAction("List", new { reasonCode = reasonCodeProductTypeDescription.ReasonCode, productId = reasonCodeProductTypeDescription.ProductId, reasonCodeTypeId = reasonCodeProductTypeDescription.ReasonCodeTypeId });
        }

        // GET: /Edit
        public ActionResult Edit(string reasonCode, int productId, int reasonCodeTypeId, string languageCode)
        {
            //Get Item 
            ReasonCodeProductTypeDescription reasonCodeProductTypeDescription = new ReasonCodeProductTypeDescription();
            reasonCodeProductTypeDescription = reasonCodeProductTypeDescriptionRepository.GetItem(languageCode, reasonCode, productId, reasonCodeTypeId);
            
            //Check Exists
            if (reasonCodeProductTypeDescription == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            ProductRepository productRepository = new ProductRepository();
            Product product = new Product();
            product = productRepository.GetProduct(productId);
            ReasonCodeTypeRepository reasonCodeTypeRepository = new ReasonCodeTypeRepository();
            ReasonCodeType reasonCodeType = new ReasonCodeType();
            reasonCodeType = reasonCodeTypeRepository.GetItem(reasonCodeTypeId);
            ReasonCodeItemRepository reasonCodeItemRepository = new ReasonCodeItemRepository();
            ReasonCodeItem reasonCodeItem = new ReasonCodeItem();
            reasonCodeItem = reasonCodeItemRepository.GetItem(1);
            ViewData["ReasonCodeItem"] = reasonCode + "/" + reasonCodeType.ReasonCodeTypeDescription + "/" + product.ProductName;

            //Language SelectList
            SelectList languageList = new SelectList(reasonCodeProductTypeDescriptionRepository.GetUnUsedLanguages(reasonCode, productId, reasonCodeTypeId).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            reasonCodeProductTypeDescriptionRepository.EditItemForDisplay(reasonCodeProductTypeDescription);
            return View(reasonCodeProductTypeDescription);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string reasonCode, int productId, int reasonCodeTypeId, string languageCode, FormCollection formCollection)
        {
            //Get Item 
            ReasonCodeProductTypeDescription reasonCodeProductTypeDescription = new ReasonCodeProductTypeDescription();
            reasonCodeProductTypeDescription = reasonCodeProductTypeDescriptionRepository.GetItem(languageCode, reasonCode, productId, reasonCodeTypeId);

            //Check Exists
            if (reasonCodeProductTypeDescription == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }



            //Update Item from Form
            try
            {
                UpdateModel(reasonCodeProductTypeDescription);
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
                reasonCodeProductTypeDescriptionRepository.Update(reasonCodeProductTypeDescription);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ReasonCodeProductTypeDescription.mvc/Edit?reasonCodeTypeId=" + reasonCodeTypeId.ToString()
                        + "&languagCode=" + languageCode
                        + "&reasonCode=" + reasonCode.ToString()
                        + "&productId=" + productId.ToString()
                        + "&reasonCodeTypeId=" + reasonCodeTypeId.ToString();
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { reasonCode = reasonCode, productId = productId, reasonCodeTypeId = reasonCodeTypeId });
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(string reasonCode, int productId, int reasonCodeTypeId, string languageCode)
        {
            //Get Item 
            ReasonCodeProductTypeDescription reasonCodeProductTypeDescription = new ReasonCodeProductTypeDescription();
            reasonCodeProductTypeDescription = reasonCodeProductTypeDescriptionRepository.GetItem(languageCode, reasonCode, productId, reasonCodeTypeId);

            //Check Exists
            if (reasonCodeProductTypeDescription == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Add Linked Information
            reasonCodeProductTypeDescriptionRepository.EditItemForDisplay(reasonCodeProductTypeDescription);

            //Return View
            return View(reasonCodeProductTypeDescription);

        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string reasonCode, int productId, int reasonCodeTypeId, string languageCode, FormCollection collection)
        {
            //Get Item 
            ReasonCodeProductTypeDescription reasonCodeProductTypeDescription = new ReasonCodeProductTypeDescription();
            reasonCodeProductTypeDescription = reasonCodeProductTypeDescriptionRepository.GetItem(languageCode, reasonCode, productId, reasonCodeTypeId);

            //Check Exists
            if (reasonCodeProductTypeDescription == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Delete Item
            try
            {
                reasonCodeProductTypeDescription.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                reasonCodeProductTypeDescriptionRepository.Delete(reasonCodeProductTypeDescription);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/AirlineAdvice.mvc/Delete?reasonCodeTypeId=" + reasonCodeTypeId.ToString()
                        + "&languagCode=" + languageCode
                        + "&reasonCode=" + reasonCode.ToString()
                        + "&productId=" + productId.ToString()
                        + "&reasonCodeTypeId=" + reasonCodeTypeId.ToString();
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            //Return
            return RedirectToAction("List", new { reasonCode = reasonCode, productId = productId, reasonCodeTypeId = reasonCodeTypeId });
        }
    }
}
