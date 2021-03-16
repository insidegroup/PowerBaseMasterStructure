using ClientProfileServiceBusiness;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;

namespace CWTDesktopDatabase.Controllers
{
	public class PolicyOtherGroupHeaderController : Controller
	{
		// Main repository
		PolicyOtherGroupHeaderRepository policyOtherGroupHeaderRepository = new PolicyOtherGroupHeaderRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		RolesRepository rolesRepository = new RolesRepository();

		private string groupName = "Policy Group Other Headers Administrator";

		//
		// GET: /PolicyOtherGroupHeader/List

		public ActionResult List(string filter, int? page, string sortField, int? sortOrder)
		{
			//SortField
			if (string.IsNullOrEmpty(sortField))
			{
				sortField = "Default";
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
				sortOrder = 0;
			}

			PolicyOtherGroupHeadersVM policyOtherGroupHeadersVM = new PolicyOtherGroupHeadersVM();

			//Set Access Rights
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				policyOtherGroupHeadersVM.HasWriteAccess = true;
			}

			bool canEditOrder = false;
			if (policyOtherGroupHeaderRepository != null)
			{
				var policyOtherGroupHeaders = policyOtherGroupHeaderRepository.PagePolicyOtherGroupHeaders(
					page ?? 1, 
					filter ?? "", 
					sortField, 
					sortOrder ?? 0,
					ref canEditOrder);

				if (policyOtherGroupHeaders != null)
				{
					policyOtherGroupHeadersVM.PolicyOtherGroupHeaders = policyOtherGroupHeaders;
				}
			}

			policyOtherGroupHeadersVM.CanEditOrder = canEditOrder;

			//return items
			return View(policyOtherGroupHeadersVM);
		}

		//
		// GET: /PolicyOtherGroupHeader/Create
		public ActionResult Create()
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			PolicyOtherGroupHeaderVM policyOtherGroupHeaderVM = new PolicyOtherGroupHeaderVM();

			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeaderVM.PolicyOtherGroupHeader = policyOtherGroupHeader;

			//Service Types
			PolicyOtherGroupHeaderServiceTypeRepository policyOtherGroupHeaderServiceTypeRepository = new PolicyOtherGroupHeaderServiceTypeRepository();
			SelectList policyOtherGroupHeaderServiceTypes = new SelectList(
				policyOtherGroupHeaderServiceTypeRepository.GetAllPolicyOtherGroupHeaderServiceTypes().ToList(),
				"PolicyOtherGroupHeaderServiceTypeId",
				"PolicyOtherGroupHeaderServiceTypeDescription"
			);
			policyOtherGroupHeaderVM.PolicyOtherGroupHeaderServiceTypes = policyOtherGroupHeaderServiceTypes;

			//Languages
			LanguageRepository languageRepository = new LanguageRepository();
			SelectList languages = new SelectList(languageRepository.GetAllLanguages().ToList(), "LanguageCode", "LanguageName", "en-gb");
			policyOtherGroupHeaderVM.Languages = languages;

			//Products
			ProductRepository productRepository = new ProductRepository();
			SelectList products = new SelectList(productRepository.GetPolicyOtherGroupHeaderProducts().ToList(), "ProductId", "ProductName");
			policyOtherGroupHeaderVM.Products = products;

			//Sub Products
			SubProductRepository subProductRepository = new SubProductRepository();
			SelectList subProducts = new SelectList(subProductRepository.GetPolicyOtherGroupHeaderSubProducts().ToList(), "SubProductId", "SubProductName");
			policyOtherGroupHeaderVM.SubProducts = subProducts;

			return View(policyOtherGroupHeaderVM);
		}

		// POST: /PolicyOtherGroupHeader/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(PolicyOtherGroupHeaderVM policyOtherGroupHeaderVM, FormCollection formCollection)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//We need to extract group from groupVM
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderVM.PolicyOtherGroupHeader;
			if (policyOtherGroupHeader == null)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyOtherGroupHeader>(policyOtherGroupHeader, "PolicyOtherGroupHeader");
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
				policyOtherGroupHeaderRepository.Add(policyOtherGroupHeader);
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

			ViewData["NewSortOrder"] = 0;
			return RedirectToAction("List");
		}

		// GET: /PolicyOtherGroupHeader/Edit
		public ActionResult Edit(int id)
		{
			//Get Item From Database
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(id);

			//Check Exists
			if (policyOtherGroupHeader == null)
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

			PolicyOtherGroupHeaderVM policyOtherGroupHeaderVM = new PolicyOtherGroupHeaderVM();
			policyOtherGroupHeaderVM.PolicyOtherGroupHeader = policyOtherGroupHeader;

			//Service Types
			PolicyOtherGroupHeaderServiceTypeRepository policyOtherGroupHeaderServiceTypeRepository = new PolicyOtherGroupHeaderServiceTypeRepository();
			SelectList policyOtherGroupHeaderServiceTypes = new SelectList(
					policyOtherGroupHeaderServiceTypeRepository.GetAllPolicyOtherGroupHeaderServiceTypes().ToList(), 
					"PolicyOtherGroupHeaderServiceTypeId", 
					"PolicyOtherGroupHeaderServiceTypeDescription",
					policyOtherGroupHeader.PolicyOtherGroupHeaderServiceTypeId
			);
			policyOtherGroupHeaderVM.PolicyOtherGroupHeaderServiceTypes = policyOtherGroupHeaderServiceTypes;

			//Languages
			LanguageRepository languageRepository = new LanguageRepository();
			SelectList languages = new SelectList(languageRepository.GetAllLanguages().ToList(), "LanguageCode", "LanguageName", "en-gb");
			policyOtherGroupHeaderVM.Languages = languages;

			//Products
			ProductRepository productRepository = new ProductRepository();
			SelectList products = new SelectList(productRepository.GetPolicyOtherGroupHeaderProducts().ToList(), "ProductId", "ProductName", policyOtherGroupHeader.ProductId);
			policyOtherGroupHeaderVM.Products = products;

			//Sub Products
			SubProductRepository subProductRepository = new SubProductRepository();
			SelectList subProducts = new SelectList(subProductRepository.GetPolicyOtherGroupHeaderSubProducts().ToList(), "SubProductId", "SubProductName", policyOtherGroupHeader.SubProductId);
			policyOtherGroupHeaderVM.SubProducts = subProducts;

			return View(policyOtherGroupHeaderVM);
		}

		// POST: /PolicyOtherGroupHeader/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(PolicyOtherGroupHeaderVM policyOtherGroupHeaderVM, FormCollection formCollection)
		{
			//Get Item
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyOtherGroupHeaderVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId);

			//Check Exists
			if (policyOtherGroupHeader == null)
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

			//Remove Table Name if no longer valid
			if (formCollection["PolicyOtherGroupHeader_TableName"] == null)
			{
				policyOtherGroupHeader.TableName = null;
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyOtherGroupHeader>(policyOtherGroupHeader, "PolicyOtherGroupHeader");
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
				policyOtherGroupHeaderRepository.Edit(policyOtherGroupHeader);
			}
			catch (SqlException ex)
			{
				//Non-Unique Name
				if (ex.Message == "NonUniqueName")
				{
					return View("NonUniqueNameError");
				}
				//Versioning Error
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PolicyOtherGroupHeader.mvc/Edit/" +policyOtherGroupHeader.PolicyOtherGroupHeaderId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List");
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
		{
			//Get Item From Database
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(id);

			//Check Exists
			if (policyOtherGroupHeader == null)
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

			PolicyOtherGroupHeaderVM policyOtherGroupHeaderVM = new PolicyOtherGroupHeaderVM();
			policyOtherGroupHeaderVM.PolicyOtherGroupHeader = policyOtherGroupHeader;

			//Service Types
			PolicyOtherGroupHeaderServiceTypeRepository policyOtherGroupHeaderServiceTypeRepository = new PolicyOtherGroupHeaderServiceTypeRepository();
			SelectList policyOtherGroupHeaderServiceTypes = new SelectList(
					policyOtherGroupHeaderServiceTypeRepository.GetAllPolicyOtherGroupHeaderServiceTypes().ToList(),
					"PolicyOtherGroupHeaderServiceTypeId",
					"PolicyOtherGroupHeaderServiceTypeDescription",
					policyOtherGroupHeader.PolicyOtherGroupHeaderServiceTypeId
			);
			policyOtherGroupHeaderVM.PolicyOtherGroupHeaderServiceTypes = policyOtherGroupHeaderServiceTypes;

			//Languages
			LanguageRepository languageRepository = new LanguageRepository();
			SelectList languages = new SelectList(languageRepository.GetAllLanguages().ToList(), "LanguageCode", "LanguageName", "en-gb");
			policyOtherGroupHeaderVM.Languages = languages;

			//Products
			ProductRepository productRepository = new ProductRepository();
			SelectList products = new SelectList(productRepository.GetPolicyOtherGroupHeaderProducts().ToList(), "ProductId", "ProductName", policyOtherGroupHeader.ProductId);
			policyOtherGroupHeaderVM.Products = products;

			//Sub Products
			SubProductRepository subProductRepository = new SubProductRepository();
			SelectList subProducts = new SelectList(subProductRepository.GetPolicyOtherGroupHeaderSubProducts().ToList(), "SubProductId", "SubProductName", policyOtherGroupHeader.SubProductId);
			policyOtherGroupHeaderVM.SubProducts = subProducts;

			return View(policyOtherGroupHeaderVM);
		}

		// POST: /PolicyOtherGroupHeader/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(PolicyOtherGroupHeaderVM policyOtherGroupHeaderVM)
		{
			//Check Valid Item passed in Form       
			if (policyOtherGroupHeaderVM.PolicyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Get Item From Database
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyOtherGroupHeaderVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId);

			//Check Exists
			if (policyOtherGroupHeader == null)
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

			//Delete Form Item
			try
			{
				policyOtherGroupHeaderRepository.Delete(policyOtherGroupHeaderVM.PolicyOtherGroupHeader);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PolicyOtherGroupHeader.mvc/Delete/" + policyOtherGroupHeaderVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List");
		}

		// GET: /SelectPolicyOtherGroupHeaderToOrder
		public ActionResult SelectPolicyOtherGroupHeaderToOrder()
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			PolicyOtherGroupHeaderSequenceVM policyOtherGroupHeaderSequenceVM = new PolicyOtherGroupHeaderSequenceVM();

			//Service Types
			PolicyOtherGroupHeaderServiceTypeRepository policyOtherGroupHeaderServiceTypeRepository = new PolicyOtherGroupHeaderServiceTypeRepository();
			SelectList policyOtherGroupHeaderServiceTypes = new SelectList(
					policyOtherGroupHeaderServiceTypeRepository.GetAllPolicyOtherGroupHeaderServiceTypes().ToList(),
					"PolicyOtherGroupHeaderServiceTypeId",
					"PolicyOtherGroupHeaderServiceTypeDescription"
			);
			policyOtherGroupHeaderSequenceVM.PolicyOtherGroupHeaderServiceTypes = policyOtherGroupHeaderServiceTypes;

			//Products
			ProductRepository productRepository = new ProductRepository();
			SelectList products = new SelectList(productRepository.GetPolicyOtherGroupHeaderProducts().ToList(), "ProductId", "ProductName");
			policyOtherGroupHeaderSequenceVM.Products = products;

			//Sub Products
			SubProductRepository subProductRepository = new SubProductRepository();
			SelectList subProducts = new SelectList(subProductRepository.GetPolicyOtherGroupHeaderSubProducts().ToList(), "SubProductId", "SubProductName");
			policyOtherGroupHeaderSequenceVM.SubProducts = subProducts;

			return View(policyOtherGroupHeaderSequenceVM);
		}

		// POST: /SelectPolicyOtherGroupHeaderToOrder
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult SelectPolicyOtherGroupHeaderToOrder(PolicyOtherGroupHeaderSequenceVM policyOtherGroupHeaderSequenceVM)
		{

			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}
			
			//Check Exists
			if(policyOtherGroupHeaderSequenceVM.ProductId == null || policyOtherGroupHeaderSequenceVM.PolicyOtherGroupHeaderServiceTypeId == null) 
			{
				ViewData["Message"] = "You have not provided a Product or a PolicyOtherGroupHeaderServiceTypeId";
				return View("Error");
			}

			//Optional
			if (policyOtherGroupHeaderSequenceVM.SubProductId <= 0)
			{
				policyOtherGroupHeaderSequenceVM.SubProductId = 0;
			}

			//Return
			return RedirectToAction("EditSequence", new {
				PolicyOtherGroupHeaderServiceTypeId = policyOtherGroupHeaderSequenceVM.PolicyOtherGroupHeaderServiceTypeId,
				ProductId = policyOtherGroupHeaderSequenceVM.ProductId,
				SubProductId = policyOtherGroupHeaderSequenceVM.SubProductId
 			});
		}

		// GET: /EditSequence
		public ActionResult EditSequence(int policyOtherGroupHeaderServiceTypeId, int productId, int subProductId, int? page)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}
			
			//Check Exists
			if (policyOtherGroupHeaderServiceTypeId <= 0 || productId <= 0)
			{
				ViewData["Message"] = "You have not provided a Product or a PolicyOtherGroupHeaderServiceTypeId";
				return View("Error");
			}

			PolicyOtherGroupHeaderSequenceVM policyOtherGroupHeaderSequenceVM = new PolicyOtherGroupHeaderSequenceVM();
			policyOtherGroupHeaderSequenceVM.PolicyOtherGroupHeaderServiceTypeId = policyOtherGroupHeaderServiceTypeId;
			policyOtherGroupHeaderSequenceVM.ProductId = productId;
			policyOtherGroupHeaderSequenceVM.SubProductId = subProductId;

			//Get Items
			PolicyOtherGroupHeaderRepository policyOtherGroupHeaderRepository = new PolicyOtherGroupHeaderRepository();
			policyOtherGroupHeaderSequenceVM.PolicyOtherGroupHeaderSequences = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeaderSequences(
				policyOtherGroupHeaderSequenceVM.PolicyOtherGroupHeaderServiceTypeId,
				policyOtherGroupHeaderSequenceVM.ProductId,
				policyOtherGroupHeaderSequenceVM.SubProductId,
				page ?? 1
			);

			//Service Types
			PolicyOtherGroupHeaderServiceTypeRepository policyOtherGroupHeaderServiceTypeRepository = new PolicyOtherGroupHeaderServiceTypeRepository();
			PolicyOtherGroupHeaderServiceType policyOtherGroupHeaderServiceType = policyOtherGroupHeaderServiceTypeRepository.GetPolicyOtherGroupHeaderServiceType(
				policyOtherGroupHeaderSequenceVM.PolicyOtherGroupHeaderServiceTypeId
			);
			policyOtherGroupHeaderSequenceVM.PolicyOtherGroupHeaderServiceType = policyOtherGroupHeaderServiceType;

			//Products
			ProductRepository productRepository = new ProductRepository();
			Product product = productRepository.GetProduct(policyOtherGroupHeaderSequenceVM.ProductId);
			policyOtherGroupHeaderSequenceVM.Product = product;

			//Sub Products
			if (policyOtherGroupHeaderSequenceVM.SubProductId > 0)
			{
				SubProductRepository subProductRepository = new SubProductRepository();
				SubProduct subProduct = subProductRepository.GetSubProduct(policyOtherGroupHeaderSequenceVM.SubProductId);
				policyOtherGroupHeaderSequenceVM.SubProduct = subProduct;
			}

			ViewData["Page"] = page ?? 1;

			return View(policyOtherGroupHeaderSequenceVM);
		}

		// POST: /EditSequence
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditSequence(int page, FormCollection collection)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}
			
			string[] sequences = collection["Sequence"].Split(new char[] { ',' });

			int sequence = (page - 1 * 5) - 2;
			if (sequence < 0)
			{
				sequence = 1;
			}

			XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
			doc.AppendChild(dec);
			XmlElement root = doc.CreateElement("SequenceXML");
			doc.AppendChild(root);

			foreach (string s in sequences)
			{
				string[] primaryKey = s.Split(new char[] { '_' });

				int policyOtherGroupHeaderId = Convert.ToInt32(primaryKey[0]);
				int versionNumber = Convert.ToInt32(primaryKey[1]);

				XmlElement xmlItem = doc.CreateElement("Item");
				root.AppendChild(xmlItem);

				XmlElement xmlSequence = doc.CreateElement("Sequence");
				xmlSequence.InnerText = sequence.ToString();
				xmlItem.AppendChild(xmlSequence);

				XmlElement xmlPolicyOtherGroupHeaderId = doc.CreateElement("PolicyOtherGroupHeaderId");
				xmlPolicyOtherGroupHeaderId.InnerText = policyOtherGroupHeaderId.ToString();
				xmlItem.AppendChild(xmlPolicyOtherGroupHeaderId);

				XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
				xmlVersionNumber.InnerText = versionNumber.ToString();
				xmlItem.AppendChild(xmlVersionNumber);

				sequence = sequence + 1;
			}

			try
			{
				PolicyOtherGroupHeaderRepository policyOtherGroupHeaderRepository = new PolicyOtherGroupHeaderRepository();
				policyOtherGroupHeaderRepository.UpdatePolicyOtherGroupHeaderSequences(System.Xml.Linq.XElement.Parse(doc.OuterXml));
			}
			catch (SqlException ex)
			{
				//Versioning Error
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PolicyOtherGroupHeader.mvc/EditSequence?page=" + page + "&id";
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List");
		}
	}
}
