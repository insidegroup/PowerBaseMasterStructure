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
	public class ReasonCodeProductTypeTravelerDescriptionController : Controller
	{
		ReasonCodeProductTypeTravelerDescriptionRepository reasonCodeProductTypeTravelerDescriptionRepository = new ReasonCodeProductTypeTravelerDescriptionRepository();
		ReasonCodeProductTypeRepository reasonCodeProductTypeRepository = new ReasonCodeProductTypeRepository();
		ReasonCodeProductTypeDescriptionRepository reasonCodeProductTypeDescriptionRepository = new ReasonCodeProductTypeDescriptionRepository();

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
			if (sortField != "ReasonCodeProductTypeTravelerDescription")
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
			var cwtPaginatedList = reasonCodeProductTypeTravelerDescriptionRepository.PageReasonCodeProductTypeTravelerDescriptions(reasonCode, productId, reasonCodeTypeId, page ?? 1, sortField, sortOrder ?? 0);
			return View(cwtPaginatedList);
		}

		//GET: View
		public ActionResult View(string reasonCode, int productId, int reasonCodeTypeId, string languageCode)
		{
			ReasonCodeProductTypeTravelerDescription reasonCodeProductTypeTravelerDescription = new ReasonCodeProductTypeTravelerDescription();
			reasonCodeProductTypeTravelerDescription = reasonCodeProductTypeTravelerDescriptionRepository.GetItem(languageCode, reasonCode, productId, reasonCodeTypeId);
			if (reasonCodeProductTypeTravelerDescription == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}
			reasonCodeProductTypeTravelerDescriptionRepository.EditItemForDisplay(reasonCodeProductTypeTravelerDescription);
			return View(reasonCodeProductTypeTravelerDescription);
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

			//ReasonCodeProductTypeTravelerDescription
			ReasonCodeProductTypeTravelerDescriptionRepository reasonCodeProductTypeTravelerDescriptionRepository = new ReasonCodeProductTypeTravelerDescriptionRepository();
			ReasonCodeProductTypeTravelerDescription reasonCodeProductTypeTravelerDescription = new ReasonCodeProductTypeTravelerDescription();

			//ReasonCodeProductTypeTravelerDescription Label
			ReasonCodeProductTypeTravelerDescription reasonCodeProductTypeTravelerDescriptionLabel = new ReasonCodeProductTypeTravelerDescription();
			reasonCodeProductTypeTravelerDescriptionLabel = reasonCodeProductTypeTravelerDescriptionRepository.GetItem("en-GB", reasonCode, productId,	reasonCodeTypeId);
			ViewData["ReasonCodeProductTypeDescription"] = (reasonCodeProductTypeTravelerDescriptionLabel != null) ?
					reasonCodeProductTypeTravelerDescriptionLabel.ReasonCodeProductTypeTravelerDescription1 : String.Empty;

			//ProductRepository
			ProductRepository productRepository = new ProductRepository();
			Product product = new Product();
			product = productRepository.GetProduct(productId);

			//ReasonCodeType
			ReasonCodeTypeRepository reasonCodeTypeRepository = new ReasonCodeTypeRepository();
			ReasonCodeType reasonCodeType = new ReasonCodeType();
			reasonCodeType = reasonCodeTypeRepository.GetItem(reasonCodeTypeId);

			//ReasonCodeItem
			ReasonCodeItemRepository reasonCodeItemRepository = new ReasonCodeItemRepository();
			ReasonCodeItem reasonCodeItem = new ReasonCodeItem();
			reasonCodeItem = reasonCodeItemRepository.GetItem(1);
			ViewData["ReasonCodeItem"] = reasonCode + "/" + reasonCodeType.ReasonCodeTypeDescription + "/" + product.ProductName;
			
			//Language SelectList
			SelectList languageList = new SelectList(reasonCodeProductTypeTravelerDescriptionRepository.GetUnUsedLanguages(reasonCode, productId, reasonCodeTypeId).ToList(), "LanguageCode", "LanguageName");
			ViewData["Languages"] = languageList;

			//Show Create Form
			return View(reasonCodeProductTypeTravelerDescription);
		}

		// POST: /Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ReasonCodeProductTypeTravelerDescription reasonCodeProductTypeTravelerDescription)
		{
			//Get Item 
			ReasonCodeProductType reasonCodeProductType = new ReasonCodeProductType();
			reasonCodeProductType = reasonCodeProductTypeRepository.GetReasonCodeProductType(reasonCodeProductTypeTravelerDescription.ReasonCode, reasonCodeProductTypeTravelerDescription.ProductId, reasonCodeProductTypeTravelerDescription.ReasonCodeTypeId);

			//Check Exists
			if (reasonCodeProductType == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//Update  Model from Form
			try
			{
				UpdateModel(reasonCodeProductTypeTravelerDescription);
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


			reasonCodeProductTypeTravelerDescriptionRepository.Add(reasonCodeProductTypeTravelerDescription);

			return RedirectToAction("List", new { reasonCode = reasonCodeProductTypeTravelerDescription.ReasonCode, productId = reasonCodeProductTypeTravelerDescription.ProductId, reasonCodeTypeId = reasonCodeProductTypeTravelerDescription.ReasonCodeTypeId });
		}

		// GET: /Edit
		public ActionResult Edit(string reasonCode, int productId, int reasonCodeTypeId, string languageCode)
		{
			//Get Item 
			ReasonCodeProductTypeTravelerDescription reasonCodeProductTypeTravelerDescription = new ReasonCodeProductTypeTravelerDescription();
			reasonCodeProductTypeTravelerDescription = reasonCodeProductTypeTravelerDescriptionRepository.GetItem(languageCode, reasonCode, productId, reasonCodeTypeId);

			//Check Exists
			if (reasonCodeProductTypeTravelerDescription == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//ReasonCodeProductTypeTravelerDescription Label
			ReasonCodeProductTypeTravelerDescription reasonCodeProductTypeTravelerDescriptionLabel = new ReasonCodeProductTypeTravelerDescription();
			reasonCodeProductTypeTravelerDescriptionLabel = reasonCodeProductTypeTravelerDescriptionRepository.GetItem("en-GB", reasonCode, productId, reasonCodeTypeId);
			ViewData["ReasonCodeProductTypeDescription"] = (reasonCodeProductTypeTravelerDescriptionLabel != null) ?
					reasonCodeProductTypeTravelerDescriptionLabel.ReasonCodeProductTypeTravelerDescription1 : String.Empty;

			//Product
			ProductRepository productRepository = new ProductRepository();
			Product product = new Product();
			product = productRepository.GetProduct(productId);

			//ReasonCodeType
			ReasonCodeTypeRepository reasonCodeTypeRepository = new ReasonCodeTypeRepository();
			ReasonCodeType reasonCodeType = new ReasonCodeType();
			reasonCodeType = reasonCodeTypeRepository.GetItem(reasonCodeTypeId);

			//ReasonCodeItem
			ReasonCodeItemRepository reasonCodeItemRepository = new ReasonCodeItemRepository();
			ReasonCodeItem reasonCodeItem = new ReasonCodeItem();
			reasonCodeItem = reasonCodeItemRepository.GetItem(1);
			ViewData["ReasonCodeItem"] = reasonCode + "/" + reasonCodeType.ReasonCodeTypeDescription + "/" + product.ProductName;

			//Language SelectList
			SelectList languageList = new SelectList(reasonCodeProductTypeTravelerDescriptionRepository.GetUnUsedLanguages(reasonCode, productId, reasonCodeTypeId).ToList(), "LanguageCode", "LanguageName");
			ViewData["Languages"] = languageList;

			reasonCodeProductTypeTravelerDescriptionRepository.EditItemForDisplay(reasonCodeProductTypeTravelerDescription);
			return View(reasonCodeProductTypeTravelerDescription);
		}

		// POST: /Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(string reasonCode, int productId, int reasonCodeTypeId, string languageCode, FormCollection formCollection)
		{
			//Get Item 
			ReasonCodeProductTypeTravelerDescription reasonCodeProductTypeTravelerDescription = new ReasonCodeProductTypeTravelerDescription();
			reasonCodeProductTypeTravelerDescription = reasonCodeProductTypeTravelerDescriptionRepository.GetItem(languageCode, reasonCode, productId, reasonCodeTypeId);

			//Check Exists
			if (reasonCodeProductTypeTravelerDescription == null)
			{
				ViewData["ActionMethod"] = "EditPost";
				return View("RecordDoesNotExistError");
			}



			//Update Item from Form
			try
			{
				UpdateModel(reasonCodeProductTypeTravelerDescription);
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
				reasonCodeProductTypeTravelerDescriptionRepository.Update(reasonCodeProductTypeTravelerDescription);
			}
			catch (SqlException ex)
			{
				//Versioning Error
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/ReasonCodeProductTypeTravelerDescription.mvc/Edit?reasonCodeTypeId=" + reasonCodeTypeId.ToString()
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
			ReasonCodeProductTypeTravelerDescription reasonCodeProductTypeTravelerDescription = new ReasonCodeProductTypeTravelerDescription();
			reasonCodeProductTypeTravelerDescription = reasonCodeProductTypeTravelerDescriptionRepository.GetItem(languageCode, reasonCode, productId, reasonCodeTypeId);

			//Check Exists
			if (reasonCodeProductTypeTravelerDescription == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			//Add Linked Information
			reasonCodeProductTypeTravelerDescriptionRepository.EditItemForDisplay(reasonCodeProductTypeTravelerDescription);

			//ReasonCodeProductTypeTravelerDescription Label
			ReasonCodeProductTypeTravelerDescription reasonCodeProductTypeTravelerDescriptionLabel = new ReasonCodeProductTypeTravelerDescription();
			reasonCodeProductTypeTravelerDescriptionLabel = reasonCodeProductTypeTravelerDescriptionRepository.GetItem("en-GB", reasonCode, productId, reasonCodeTypeId);
			ViewData["ReasonCodeProductTypeDescription"] = (reasonCodeProductTypeTravelerDescriptionLabel != null) ?
					reasonCodeProductTypeTravelerDescriptionLabel.ReasonCodeProductTypeTravelerDescription1 : String.Empty;

			//Return View
			return View(reasonCodeProductTypeTravelerDescription);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(string reasonCode, int productId, int reasonCodeTypeId, string languageCode, FormCollection collection)
		{
			//Get Item 
			ReasonCodeProductTypeTravelerDescription reasonCodeProductTypeTravelerDescription = new ReasonCodeProductTypeTravelerDescription();
			reasonCodeProductTypeTravelerDescription = reasonCodeProductTypeTravelerDescriptionRepository.GetItem(languageCode, reasonCode, productId, reasonCodeTypeId);

			//Check Exists
			if (reasonCodeProductTypeTravelerDescription == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Delete Item
			try
			{
				reasonCodeProductTypeTravelerDescription.VersionNumber = Int32.Parse(collection["VersionNumber"]);
				reasonCodeProductTypeTravelerDescriptionRepository.Delete(reasonCodeProductTypeTravelerDescription);
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
