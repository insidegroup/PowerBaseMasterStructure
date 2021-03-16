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

namespace CWTDesktopDatabase.Controllers
{
    public class ReasonCodeItemController : Controller
    {
        //main repositories
        ReasonCodeGroupRepository reasonCodeGroupRepository = new ReasonCodeGroupRepository();
        ReasonCodeItemRepository reasonCodeItemRepository = new ReasonCodeItemRepository();

        // GET: /List
        public ActionResult List(int id, int? page, string filter, string sortField, int? sortOrder)
        {
            //Check Parent Exists
            ReasonCodeGroup reasonCodeGroup = new ReasonCodeGroup();
            reasonCodeGroup = reasonCodeGroupRepository.GetGroup(id);

            //Check Exists
            if (reasonCodeGroup == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToReasonCodeGroup(id))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //SortField+SortOrder settings
            if (sortField != "DisplayOrder" && sortField != "ReasonCode" && sortField != "ReasonCodeTypeDescription" && sortField != "ProductName")
            {
                sortField = "DisplayOrder";
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

            ViewData["ReasonCodeGroupId"] = reasonCodeGroup.ReasonCodeGroupId;
            ViewData["ReasonCodeGroupName"] = reasonCodeGroup.ReasonCodeGroupName;

            //not used in this case
            //ViewData["CurrentSortField"] = "DisplayOrder";
            //ViewData["CurrentSortOrder"] = "0";


            //return items
            var cwtPaginatedList = reasonCodeItemRepository.PageReasonCodeItems(id, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        // GET: /Create
        public ActionResult Create(int id)
        {
            //Check Parent Exists
            ReasonCodeGroup reasonCodeGroup = new ReasonCodeGroup();
            reasonCodeGroup = reasonCodeGroupRepository.GetGroup(id);

            //Check Exists
            if (reasonCodeGroup == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }
            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReasonCodeGroup(id))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ReasonCodeProductTypeRepository reasonCodeProductTypeRepository = new ReasonCodeProductTypeRepository();
            SelectList reasonCodeList = new SelectList(reasonCodeProductTypeRepository.GetAllReasonCodeProductTypes().ToList(), "ReasonCodeValue", "ReasonCodeValue");
            ViewData["ReasonCodes"] = reasonCodeList;

            ReasonCodeItem reasonCodeItem = new ReasonCodeItem();
            reasonCodeItem.ReasonCodeGroupName = reasonCodeGroup.ReasonCodeGroupName;
            reasonCodeItem.ReasonCodeGroupId = id;
			reasonCodeItem.TravelerFacingFlag = true;

            return View(reasonCodeItem);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReasonCodeItem reasonCodeItem)
        {

            //Get ReasonCodeGroup
            ReasonCodeGroup reasonCodeGroup = new ReasonCodeGroup();
            reasonCodeGroup = reasonCodeGroupRepository.GetGroup(reasonCodeItem.ReasonCodeGroupId);

            //Check Exists
            if (reasonCodeGroup == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }
            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReasonCodeGroup(reasonCodeItem.ReasonCodeGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            try
            {
                UpdateModel(reasonCodeItem);
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
                reasonCodeItemRepository.Add(reasonCodeItem);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = reasonCodeItem.ReasonCodeGroupId });
        }

        // GET: /Edit
        public ActionResult Edit(int id)
        {
            //Get Item
            ReasonCodeItem reasonCodeItem = new ReasonCodeItem();
            reasonCodeItem = reasonCodeItemRepository.GetItem(id);

            //Check Exists
            if (reasonCodeItem == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReasonCodeGroup(reasonCodeItem.ReasonCodeGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ReasonCodeProductTypeRepository reasonCodeProductTypeRepository = new ReasonCodeProductTypeRepository();
            SelectList reasonCodeList = new SelectList(reasonCodeProductTypeRepository.GetAllReasonCodeProductTypes().ToList(), "ReasonCodeValue", "ReasonCodeValue");
            ViewData["ReasonCodes"] = reasonCodeList;

            SelectList productList = new SelectList(reasonCodeProductTypeRepository.LookUpReasonCodeProducts(reasonCodeItem.ReasonCode).ToList(), "ProductId", "ProductName");
            ViewData["Products"] = productList;

            SelectList reasonCodeTypeList = new SelectList(reasonCodeProductTypeRepository.LookUpAvailableReasonCodeProductReasonCodeTypes(
                                reasonCodeItem.ReasonCodeItemId,
                                reasonCodeItem.ReasonCodeGroupId,
                                reasonCodeItem.ReasonCode,
                                reasonCodeItem.ProductId).ToList(), "ReasonCodeTypeId", "ReasonCodeTypeDescription");
            ViewData["ReasonCodeTypes"] = reasonCodeTypeList;

            //Parent Information
            reasonCodeItemRepository.EditItemForDisplay(reasonCodeItem);
            ViewData["ReasonCodeItem"] = reasonCodeItem.ReasonCode + "/" + reasonCodeItem.ReasonCodeTypeDescription + "/" + reasonCodeItem.ProductName;
            ViewData["ReasonCodeItemId"] = reasonCodeItem.ReasonCodeItemId;
            ViewData["ReasonCodeGroupId"] = reasonCodeItem.ReasonCodeGroupId;
            ViewData["ReasonCodeGroupName"] = reasonCodeGroupRepository.GetGroup(reasonCodeItem.ReasonCodeGroupId).ReasonCodeGroupName;

            reasonCodeItemRepository.EditItemForDisplay(reasonCodeItem);
            return View(reasonCodeItem);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            //Get Item
            ReasonCodeItem reasonCodeItem = new ReasonCodeItem();
            reasonCodeItem = reasonCodeItemRepository.GetItem(id);

            //Check Exists
            if (reasonCodeItem == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReasonCodeGroup(reasonCodeItem.ReasonCodeGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            try
            {
                UpdateModel(reasonCodeItem);
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
                reasonCodeItemRepository.Edit(reasonCodeItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ReasonCodeItem.mvc/Edit/" + reasonCodeItem.ReasonCodeItemId.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = reasonCodeItem.ReasonCodeGroupId });
        }

        public ActionResult EditSequenceTypeSelection(int id){

            ReasonCodeGroup reasonCodeGroup = new ReasonCodeGroup();
            reasonCodeGroup = reasonCodeGroupRepository.GetGroup(id);

            ProductReasonItemOrderTypeSelectionVM productReasonItemOrderTypeSelectionVM = new ProductReasonItemOrderTypeSelectionVM();
            productReasonItemOrderTypeSelectionVM.ReasonCodeTypes = new SelectList(reasonCodeItemRepository.GetReasonCodeItemReasonCodeTypes(id), "ReasonCodeTypeId", "ReasonCodeTypeDescription");
            productReasonItemOrderTypeSelectionVM.ReasonCodeGroup = reasonCodeGroup;

            return View(productReasonItemOrderTypeSelectionVM);
        }

        public ActionResult EditSequence(int id, int reasonCodeTypeId, int? page){

             //Get Item From Database
            ReasonCodeGroup group = new ReasonCodeGroup();
            group = reasonCodeGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "EditSequenceGet";
                return View("RecordDoesNotExistError");
            }
            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReasonCodeGroup(id))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ReasonCodeItemSequenceRepository reasonCodeItemSequenceRepository = new ReasonCodeItemSequenceRepository();

            ReasonCodeGroupReasonCodeTypeSequencingVM reasonCodeGroupReasonCodeTypeSequencingVM = new ReasonCodeGroupReasonCodeTypeSequencingVM();
            reasonCodeGroupReasonCodeTypeSequencingVM.SequenceItems = reasonCodeItemSequenceRepository.GetReasonCodeItemSequences(id, reasonCodeTypeId, page ?? 1);
            reasonCodeGroupReasonCodeTypeSequencingVM.ReasonCodeGroup = group;
            reasonCodeGroupReasonCodeTypeSequencingVM.ReasonCodeTypeId = reasonCodeTypeId;
            reasonCodeGroupReasonCodeTypeSequencingVM.Page = page ?? 1;

            return View(reasonCodeGroupReasonCodeTypeSequencingVM);

        }
        
          // POST: /EditSequence
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSequence(ReasonCodeGroupReasonCodeTypeSequencingVM reasonCodeGroupReasonCodeTypeSequencingVM, FormCollection collection)
        {

            int reasonCodeGroupId = reasonCodeGroupReasonCodeTypeSequencingVM.ReasonCodeGroup.ReasonCodeGroupId;
            int reasonCodeTypeId = reasonCodeGroupReasonCodeTypeSequencingVM.ReasonCodeTypeId;
            int page = reasonCodeGroupReasonCodeTypeSequencingVM.Page;

            //Check Exists
            ReasonCodeGroup reasonCodeGroup = new ReasonCodeGroup();
            reasonCodeGroup = reasonCodeGroupRepository.GetGroup(reasonCodeGroupId);
            if (reasonCodeGroup == null)
            {
                return View("Error");
            }

            //Check Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReasonCodeGroup(reasonCodeGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            string[] sequences = collection["RCSequence"].Split(new char[] { ',' });

            int sequence = (page - 1 * 5) - 2;
            if (sequence < 0)
            {
                sequence = 1;
            }

			XmlDocument doc = new XmlDocument();
			XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
			XmlElement root = doc.DocumentElement;
			doc.InsertBefore(xmlDeclaration, root);
			XmlElement rootElement = doc.CreateElement(string.Empty, "SequenceXML", string.Empty);
			doc.AppendChild(rootElement);

			foreach (string s in sequences)
			{
				string[] reasonCodeItemPK = s.Split(new char[] { '_' });

				int reasonCodeItemId = Convert.ToInt32(reasonCodeItemPK[0]);
				int versionNumber = Convert.ToInt32(reasonCodeItemPK[1]);

				XmlElement itemElement = doc.CreateElement(string.Empty, "Item", string.Empty);

				XmlElement sequenceElement = doc.CreateElement(string.Empty, "Sequence", string.Empty);
				XmlCDataSection sequenceText = doc.CreateCDataSection(HttpUtility.HtmlEncode(sequence.ToString()));
				sequenceElement.AppendChild(sequenceText);
				itemElement.AppendChild(sequenceElement);

				XmlElement reasonCodeItemIdElement = doc.CreateElement(string.Empty, "ReasonCodeItemId", string.Empty);
				XmlCDataSection reasonCodeItemIdText = doc.CreateCDataSection(HttpUtility.HtmlEncode(reasonCodeItemId.ToString()));
				reasonCodeItemIdElement.AppendChild(reasonCodeItemIdText);
				itemElement.AppendChild(reasonCodeItemIdElement);

				XmlElement versionNumberElement = doc.CreateElement(string.Empty, "VersionNumber", string.Empty);
				XmlCDataSection versionNumberText = doc.CreateCDataSection(HttpUtility.HtmlEncode(versionNumber.ToString()));
				versionNumberElement.AppendChild(versionNumberText);
				itemElement.AppendChild(versionNumberElement);

				rootElement.AppendChild(itemElement);

				sequence = sequence + 1;
			}

            try
            {
                ReasonCodeItemSequenceRepository reasonCodeItemSequenceRepository = new ReasonCodeItemSequenceRepository();
                reasonCodeItemSequenceRepository.UpdateReasonCodeItemSequences(doc.OuterXml);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ReasonCodeItem.mvc/EditSequence?ReasonCodeTypeId="+ reasonCodeTypeId +"&id="+ reasonCodeGroupId +"?page=" + page;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");

            }

            return RedirectToAction("List", new { id = reasonCodeGroupId });
        }
        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            //Get Item
            ReasonCodeItem reasonCodeItem = new ReasonCodeItem();
            reasonCodeItem = reasonCodeItemRepository.GetItem(id);

            //Check Exists
            if (reasonCodeItem == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReasonCodeGroup(reasonCodeItem.ReasonCodeGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Parent Information
            reasonCodeItemRepository.EditItemForDisplay(reasonCodeItem);
            ViewData["ReasonCodeItem"] = reasonCodeItem.ReasonCode + "/" + reasonCodeItem.ReasonCodeTypeDescription + "/" + reasonCodeItem.ProductName;
            ViewData["ReasonCodeItemId"] = reasonCodeItem.ReasonCodeItemId;
            ViewData["ReasonCodeGroupId"] = reasonCodeItem.ReasonCodeGroupId;
            ViewData["ReasonCodeGroupName"] = reasonCodeGroupRepository.GetGroup(reasonCodeItem.ReasonCodeGroupId).ReasonCodeGroupName;


            reasonCodeItemRepository.EditItemForDisplay(reasonCodeItem);
            return View(reasonCodeItem);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //Get Item
            ReasonCodeItem reasonCodeItem = new ReasonCodeItem();
            reasonCodeItem = reasonCodeItemRepository.GetItem(id);

            //Check Exists
            if (reasonCodeItem == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReasonCodeGroup(reasonCodeItem.ReasonCodeGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                reasonCodeItem.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                reasonCodeItemRepository.Delete(reasonCodeItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ReasonCodeItem.mvc/Delete/" + reasonCodeItem.ReasonCodeItemId;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            //Return
            return RedirectToAction("List", new { id = reasonCodeItem.ReasonCodeGroupId });
        }


        //Update Select List
        [HttpPost]
        public JsonResult GetReasonCodeProducts(string reasonCode)
        {
            ReasonCodeProductTypeRepository reasonCodeProductTypeRepository = new ReasonCodeProductTypeRepository();
            var result = reasonCodeProductTypeRepository.LookUpReasonCodeProducts(reasonCode);
            return Json(result);
        }

        //Update Select List
        [HttpPost]
        public JsonResult GetReasonCodeProductReasonCodeTypes(int? reasonCodeId, int groupId, string reasonCode, int productId)
        {
            ReasonCodeProductTypeRepository reasonCodeProductTypeRepository = new ReasonCodeProductTypeRepository();
            var result = reasonCodeProductTypeRepository.LookUpAvailableReasonCodeProductReasonCodeTypes(reasonCodeId, groupId, reasonCode, productId);
            return Json(result);
        }

        //Validation
        [HttpPost]
        public JsonResult IsValidReasonCodeProductType(string reasonCode, int? productId, int? reasonCodeTypeId)
        {

            ReasonCodeProductTypeRepository reasonCodeProductTypeRepository = new ReasonCodeProductTypeRepository();
            var result = reasonCodeProductTypeRepository.GetReasonCodeProductType(reasonCode, productId, reasonCodeTypeId);
            return Json(result);
        }

    }

}
