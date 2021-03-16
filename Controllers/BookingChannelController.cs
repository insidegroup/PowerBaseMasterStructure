using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using CWTDesktopDatabase.Helpers;
using Newtonsoft.Json;

namespace CWTDesktopDatabase.Controllers
{
	public class BookingChannelController : Controller
	{
		ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
		BookingChannelRepository bookingChannelRepository = new BookingChannelRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();

		private string groupName = "Client Detail";

		// GET: /List
		public ActionResult List(int? page, string id, string sortField, int? sortOrder, string filter)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToClientSubUnit(id) && hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//Sorting
			if (sortField == null || sortField == "BookingChannelTypeDescription")
			{
				sortField = "BookingChannelTypeDescription";
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

			BookingChannelsVM bookingChannelsVM = new BookingChannelsVM();
			bookingChannelsVM.BookingChannels = bookingChannelRepository.PageBookingChannel(page ?? 1, id, sortField, sortOrder, filter ?? "");

			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);

			//Check clientSubUnit
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "List";
				return View("RecordDoesNotExistError");
			}

			bookingChannelsVM.ClientSubUnit = clientSubUnit;

			//return items
			return View(bookingChannelsVM);
		}

		// GET: /Create
		public ActionResult Create(string id)
		{
			//Get ClientSubUnit
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);

			//Check Exists
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			}

			//Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientSubUnit(id) || !hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			BookingChannelVM bookingChannelVM = new BookingChannelVM();
			bookingChannelVM.ClientSubUnit = clientSubUnit;

			BookingChannel bookingChannel = new BookingChannel();
			bookingChannel.ClientSubUnit = clientSubUnit;
			bookingChannel.ClientSubUnitGuid = clientSubUnit.ClientSubUnitGuid;
			bookingChannelVM.BookingChannel = bookingChannel;

			//Usage Types
			UsageTypeRepository usageTypeRepository = new UsageTypeRepository();
			bookingChannelVM.UsageTypes = new SelectList(usageTypeRepository.GetAvailableUsageTypes(id).ToList(), "UsageTypeId", "UsageTypeDescription");

			//Booking Channel Types
			BookingChannelTypeRepository bookingChannelTypeRepository = new BookingChannelTypeRepository();
			bookingChannelVM.BookingChannelTypes = new SelectList(bookingChannelTypeRepository.GetAllBookingChannelTypes().ToList(), "BookingChannelTypeId", "BookingChannelTypeDescription");

			//Channel Products
			ProductChannelTypeRepository productChannelTypeRepository = new ProductChannelTypeRepository();
			bookingChannelVM.ProductChannelTypes = new SelectList(productChannelTypeRepository.GetAllProductChannelTypes().ToList(), "ProductChannelTypeId", "ProductChannelTypeDescription");

			//Desktop Used Types
			DesktopUsedTypeRepository desktopUsedTypeRepository = new DesktopUsedTypeRepository();
			bookingChannelVM.DesktopUsedTypes = new SelectList(desktopUsedTypeRepository.GetAllDesktopUsedTypes().ToList(), "DesktopUsedTypeId", "DesktopUsedTypeDescription");

			//Content Booked Items
			ProductRepository productRepository = new ProductRepository();
			bookingChannelVM.Products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");

			GDSRepository GDSRepository = new GDSRepository();
			bookingChannelVM.GDSList = new SelectList(GDSRepository.GetAllGDSsExceptALL().OrderBy(x => x.GDSName).ToList(), "GDSCode", "GDSName");

			//Show Create Form
			return View(bookingChannelVM);
		}

		// POST: /Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(BookingChannelVM bookingChannelVM, FormCollection formCollection)
		{
			BookingChannel bookingChannel = new BookingChannel();
			bookingChannel = bookingChannelVM.BookingChannel;
			if (bookingChannel == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			string clientSubUnitGuid = formCollection["BookingChannel.ClientSubUnit.ClientSubUnitGuid"];

			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

			//Check Exists
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			bookingChannel.ClientSubUnitGuid = clientSubUnitGuid;

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
				TryUpdateModel<BookingChannel>(bookingChannel, "BookingChannel");
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
				bookingChannelRepository.Add(bookingChannelVM);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { id = clientSubUnitGuid });
		}

		// GET: /Edit
		public ActionResult Edit(int id)
		{
			//Get BookingChannel
			BookingChannel bookingChannel = new BookingChannel();
			bookingChannel = bookingChannelRepository.BookingChannel(id);

			//Check Exists
			if (bookingChannel == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientSubUnit(bookingChannel.ClientSubUnitGuid) || !hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			BookingChannelVM bookingChannelVM = new BookingChannelVM();
			bookingChannelVM.BookingChannel = bookingChannel;

			//Get ClientSubUnit
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(bookingChannel.ClientSubUnitGuid);
			bookingChannelVM.ClientSubUnit = clientSubUnit;


			//Booking Channel Types
			BookingChannelTypeRepository bookingChannelTypeRepository = new BookingChannelTypeRepository();
			if (bookingChannelVM.BookingChannel.BookingChannelTypeId != null)
			{
				bookingChannelVM.BookingChannelTypes = new SelectList(
					bookingChannelTypeRepository.GetAllBookingChannelTypes().ToList(),
					"BookingChannelTypeId",
					"BookingChannelTypeDescription",
					bookingChannelVM.BookingChannel.BookingChannelTypeId
				);
			}
			else
			{
				bookingChannelVM.BookingChannelTypes = new SelectList(
					bookingChannelTypeRepository.GetAllBookingChannelTypes().ToList(),
					"BookingChannelTypeId",
					"BookingChannelTypeDescription"
				);
			}

			//Channel Products			
			ProductChannelTypeRepository productChannelTypeRepository = new ProductChannelTypeRepository();
			int bookingChannelTypeId = (bookingChannelVM.BookingChannel.BookingChannelTypeId != null) ? bookingChannelVM.BookingChannel.BookingChannelTypeId.Value : 1;
			bookingChannelVM.ProductChannelTypes = new SelectList(
				productChannelTypeRepository.GetProductChannelTypesForBookingChannel(bookingChannelTypeId).ToList(),
				"ProductChannelTypeId",
				"ProductChannelTypeDescription",
				bookingChannelVM.BookingChannel.ProductChannelTypeId
			);

			//Desktop Used Types
			DesktopUsedTypeRepository desktopUsedTypeRepository = new DesktopUsedTypeRepository();
			if (bookingChannelVM.BookingChannel.DesktopUsedTypeId != null)
			{
				bookingChannelVM.DesktopUsedTypes = new SelectList(
					desktopUsedTypeRepository.GetAllDesktopUsedTypes().ToList(),
					"DesktopUsedTypeId",
					"DesktopUsedTypeDescription",
					bookingChannelVM.BookingChannel.DesktopUsedTypeId
				);
			}
			else
			{
				bookingChannelVM.DesktopUsedTypes = new SelectList(
					desktopUsedTypeRepository.GetAllDesktopUsedTypes().ToList(),
					"DesktopUsedTypeId",
					"DesktopUsedTypeDescription"
				);
			}

			//Content Booked Items
			ContentBookedItemRepository contentBookedItemRepository = new ContentBookedItemRepository();
			List<ContentBookedItem> contentBookedItems = contentBookedItemRepository.GetBookingChannelContentBookedItems(bookingChannelVM.BookingChannel.BookingChannelId).ToList();

			ProductRepository productRepository = new ProductRepository();
			IEnumerable<SelectListItem> defaultProducts = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");

			List<SelectListItem> contentBookedItemsSelected = new List<SelectListItem>();

			foreach (SelectListItem item in defaultProducts)
			{
				bool selected = false; 
				
				foreach (ContentBookedItem contentBookedItem in contentBookedItems)
				{
					if(item.Value == contentBookedItem.Product.ProductId.ToString()) {
						selected = true;
					}
				}

				contentBookedItemsSelected.Add(
					new SelectListItem()
					{
						Text = item.Text,
						Value = item.Value,
						Selected = selected
					}
				);
			}

			bookingChannelVM.ContentBookedItemsSelected = contentBookedItemsSelected;

			//GDS
			GDSRepository GDSRepository = new GDSRepository();
			bookingChannelVM.GDSList = new SelectList(GDSRepository.GetAllGDSsExceptALL().OrderBy(x => x.GDSName).ToList(), "GDSCode", "GDSName");

			//Show Edit Form
			return View(bookingChannelVM);
		}

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BookingChannelVM bookingChannelVM)
        {
            //Get BookingChannel
			BookingChannel bookingChannel = new BookingChannel();
			bookingChannel = bookingChannelRepository.BookingChannel(
				bookingChannelVM.BookingChannel.BookingChannelId
			);

			//Check Exists
			if (bookingChannel == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

			string clientSubUnitGuid = bookingChannelVM.BookingChannel.ClientSubUnitGuid;

			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

			//Check Exists
			if (clientSubUnit == null)
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
				TryUpdateModel<BookingChannel>(bookingChannel, "BookingChannel");
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
                bookingChannelRepository.Update(bookingChannelVM);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", new { id = clientSubUnitGuid });
        }

		// GET: /View
		public ActionResult View(int id)
		{
			//Get BookingChannel
			BookingChannel bookingChannel = new BookingChannel();
			bookingChannel = bookingChannelRepository.BookingChannel(id);

			//Check Exists
			if (bookingChannel == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			BookingChannelVM bookingChannelVM = new BookingChannelVM();
			bookingChannelVM.BookingChannel = bookingChannel;

			//Get ClientSubUnit
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(bookingChannel.ClientSubUnitGuid);
			bookingChannelVM.ClientSubUnit = clientSubUnit;

			//Get GDS
			GDSRepository gdsRepository = new GDSRepository();
			GDS gds = gdsRepository.GetGDS(bookingChannel.GDSCode);
			bookingChannelVM.GDS = gds;

			//Channel Products
			if (bookingChannel.ProductChannelTypeId != null)
			{
				ProductChannelTypeRepository productChannelTypeRepository = new ProductChannelTypeRepository();
				ProductChannelType productChannelType = productChannelTypeRepository.GetProductChannelType((int)bookingChannel.ProductChannelTypeId);
				if (productChannelType != null)
				{
					bookingChannelVM.BookingChannel.ProductChannelType = productChannelType;
				}
			}

			//Desktop Used Types
			if (bookingChannel.DesktopUsedTypeId != null)
			{
				DesktopUsedTypeRepository desktopUsedTypeRepository = new DesktopUsedTypeRepository();
				DesktopUsedType desktopUsedType = desktopUsedTypeRepository.GetDesktopUsedType((int)bookingChannel.DesktopUsedTypeId);
				if (desktopUsedType != null)
				{
					bookingChannelVM.BookingChannel.DesktopUsedType = desktopUsedType;
				}
			}

			//Content Booked Items
			ContentBookedItemRepository contentBookedItemRepository = new ContentBookedItemRepository();
			List<ContentBookedItem> contentBookedItems = contentBookedItemRepository.GetBookingChannelContentBookedItems(bookingChannel.BookingChannelId).ToList();
			if (contentBookedItems != null)
			{
				bookingChannelVM.ContentBookedItemsList = String.Join(", ", contentBookedItems.Select(x => x.Product.ProductName.ToString()).ToArray());
			}

			//Show Form
			return View(bookingChannelVM);
		}
		
		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
		{
			//Get BookingChannel
			BookingChannel bookingChannel = new BookingChannel();
			bookingChannel = bookingChannelRepository.BookingChannel(id);

			//Check Exists
			if (bookingChannel == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			//Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientSubUnit(bookingChannel.ClientSubUnitGuid) || !hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			BookingChannelVM bookingChannelVM = new BookingChannelVM();
			bookingChannelVM.BookingChannel = bookingChannel;

			//Get ClientSubUnit
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(bookingChannel.ClientSubUnitGuid);
			bookingChannelVM.ClientSubUnit = clientSubUnit;

			//Get GDS
			GDSRepository gdsRepository = new GDSRepository();
			GDS gds = gdsRepository.GetGDS(bookingChannel.GDSCode);
			bookingChannelVM.GDS = gds;

			//Channel Products
			if (bookingChannel.ProductChannelTypeId != null)
			{
				ProductChannelTypeRepository productChannelTypeRepository = new ProductChannelTypeRepository();
				ProductChannelType productChannelType = productChannelTypeRepository.GetProductChannelType((int)bookingChannel.ProductChannelTypeId);
				if(productChannelType != null) {
					bookingChannelVM.BookingChannel.ProductChannelType = productChannelType;
				}
			}

			//Desktop Used Types
			if (bookingChannel.DesktopUsedTypeId != null)
			{
				DesktopUsedTypeRepository desktopUsedTypeRepository = new DesktopUsedTypeRepository();
				DesktopUsedType desktopUsedType = desktopUsedTypeRepository.GetDesktopUsedType((int)bookingChannel.DesktopUsedTypeId);
				if (desktopUsedType != null)
				{
					bookingChannelVM.BookingChannel.DesktopUsedType = desktopUsedType;
				}
			}

			//Content Booked Items
			ContentBookedItemRepository contentBookedItemRepository = new ContentBookedItemRepository();
			List<ContentBookedItem> contentBookedItems = contentBookedItemRepository.GetBookingChannelContentBookedItems(bookingChannel.BookingChannelId).ToList();
			if (contentBookedItems != null)
			{
				bookingChannelVM.ContentBookedItemsList = String.Join(", ", contentBookedItems.Select(x => x.Product.ProductName.ToString()).ToArray());
			}

			//Show Form
			return View(bookingChannelVM);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(BookingChannelVM bookingChannelVM)
		{
			//Get BookingChannel
			BookingChannel bookingChannel = new BookingChannel();
			bookingChannel = bookingChannelRepository.BookingChannel(
				bookingChannelVM.BookingChannel.BookingChannelId
			);

			//Check Exists
			if (bookingChannel == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientSubUnit(bookingChannel.ClientSubUnitGuid) || !hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Delete Item
			try
			{
				bookingChannelRepository.Delete(bookingChannelVM.BookingChannel);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = String.Format("/BookingChannel.mvc/Delete/id={0}", bookingChannelVM.BookingChannel.BookingChannelId);
					return View("VersionError");
				}
				//Generic Error
				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			//Return
			return RedirectToAction("List", new { id = bookingChannel.ClientSubUnitGuid });
		}

		//Update Select List
		[HttpPost]
		public JsonResult GetProductChannelTypes(int bookingChannelTypeId)
		{
			ProductChannelTypeRepository productChannelTypeRepository = new ProductChannelTypeRepository();
			var result = productChannelTypeRepository.GetProductChannelTypesForBookingChannel(bookingChannelTypeId);
			return Json(result);
		}

        // GET: /Export
        public ActionResult Export(string id)
        {
            //Get ClientSubUnit
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);

            //Check Exists
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(id) || !hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            string filename = string.Format("{0}-{1}-BookingChannels Export.csv", CWTStringHelpers.AlphaNumericOnly(clientSubUnit.ClientTopUnit.ClientTopUnitName), CWTStringHelpers.AlphaNumericOnly(clientSubUnit.ClientSubUnitName));

            //Get CSV Data
            byte[] csvData = bookingChannelRepository.Export(id);
            return File(csvData, "text/csv", filename);
        }

        // GET: /ExportErrors
        public ActionResult ExportErrors()
        {
            var preImportCheckResultVM = (BookingChannelImportStep1VM)TempData["ErrorMessages"];

            if (preImportCheckResultVM == null)
            {
                ViewData["ActionMethod"] = "ExportGet";
                return View("RecordDoesNotExistError");
            }

            if (preImportCheckResultVM.ClientSubUnit == null)
            {
                ViewData["ActionMethod"] = "ExportGet";
                return View("RecordDoesNotExistError");
            }

            //Check Exists
            ClientSubUnit clientSubUnit = clientSubUnitRepository.GetClientSubUnit(preImportCheckResultVM.ClientSubUnit.ClientSubUnitGuid);
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "ExportGet";
                return View("RecordDoesNotExistError");
            }

            //Get CSV Data
            var errors = preImportCheckResultVM.ImportStep2VM.ReturnMessages;
            var combinedErrors = errors.Aggregate((current, next) => current + "\r\n" + next);
            byte[] csvData = Encoding.ASCII.GetBytes(combinedErrors);

            return File(csvData, "text/plain", string.Format("{0}-{1}-BookingChannels Error Summary.txt", CWTStringHelpers.AlphaNumericOnly(clientSubUnit.ClientTopUnit.ClientTopUnitName), CWTStringHelpers.AlphaNumericOnly(clientSubUnit.ClientSubUnitName)));
        }

        public ActionResult ImportStep1(string id)
        {
            BookingChannelImportStep1WithFileVM clientSubUnitImportStep1WithFileVM = new BookingChannelImportStep1WithFileVM();

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);

            //Check Exists
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "ListSubMenu";
                return View("RecordDoesNotExistError");
            }

            clientSubUnitImportStep1WithFileVM.ClientSubUnit = clientSubUnit;
            clientSubUnitImportStep1WithFileVM.ClientSubUnitGuid = id;

            return View(clientSubUnitImportStep1WithFileVM);
        }

        [HttpPost]
        public ActionResult ImportStep1(BookingChannelImportStep1WithFileVM csvfile)
        {
            //used for return only
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csvfile.ClientSubUnitGuid);
            csvfile.ClientSubUnit = clientSubUnit;

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
                BookingChannelImportStep2VM preImportCheckResult = new BookingChannelImportStep2VM();
                List<string> returnMessages = new List<string>();

                preImportCheckResult = bookingChannelRepository.PreImportCheck(csvfile.File, csvfile.ClientSubUnitGuid);

                BookingChannelImportStep1VM preImportCheckResultVM = new BookingChannelImportStep1VM();
                preImportCheckResultVM.ClientSubUnit = clientSubUnit;
                preImportCheckResultVM.ImportStep2VM = preImportCheckResult;
                preImportCheckResultVM.ClientSubUnitGuid = csvfile.ClientSubUnitGuid;

                TempData["PreImportCheckResultVM"] = preImportCheckResultVM;
                return RedirectToAction("ImportStep2");
            }

            return View();
        }

        public ActionResult ImportStep2()
        {
            BookingChannelImportStep1VM preImportCheckResultVM = new BookingChannelImportStep1VM();
            preImportCheckResultVM = (BookingChannelImportStep1VM)TempData["PreImportCheckResultVM"];
            if (preImportCheckResultVM != null)
            {
                ClientSubUnit clientSubUnit = new ClientSubUnit();
                clientSubUnit = clientSubUnitRepository.GetClientSubUnit(preImportCheckResultVM.ClientSubUnitGuid);
                preImportCheckResultVM.ClientSubUnit = clientSubUnit;
            }
            else
            {
                return View("Error");
            }

            return View(preImportCheckResultVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImportStep2(BookingChannelImportStep1VM preImportCheckResultVM)
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
            BookingChannelImportStep2VM preImportCheckResult = new BookingChannelImportStep2VM();
            preImportCheckResult = preImportCheckResultVM.ImportStep2VM;

            //Do the Import, return results
            BookingChannelImportStep3VM cdrPostImportResult = new BookingChannelImportStep3VM();
            cdrPostImportResult = bookingChannelRepository.Import(
                preImportCheckResult.FileBytes,
                preImportCheckResultVM.ClientSubUnit.ClientSubUnitGuid
            );

            cdrPostImportResult.ClientSubUnitGuid = preImportCheckResultVM.ClientSubUnit.ClientSubUnitGuid;
            TempData["CdrPostImportResult"] = cdrPostImportResult;

            //Pass Results to Next Page
            return RedirectToAction("ImportStep3");

        }

        public ActionResult ImportStep3()
        {
            //Display Results of Import
            BookingChannelImportStep3VM cdrPostImportResult = new BookingChannelImportStep3VM();
            cdrPostImportResult = (BookingChannelImportStep3VM)TempData["CdrPostImportResult"];
            if (cdrPostImportResult != null)
            {
                ClientSubUnit clientSubUnit = new ClientSubUnit();

                if (cdrPostImportResult.ClientSubUnitGuid != null)
                {
                    clientSubUnit = clientSubUnitRepository.GetClientSubUnit(cdrPostImportResult.ClientSubUnitGuid);
                    cdrPostImportResult.ClientSubUnit = clientSubUnit;
                }
            }
            else
            {
                return View("Error");
            }

            return View(cdrPostImportResult);
        }
    }
}
 