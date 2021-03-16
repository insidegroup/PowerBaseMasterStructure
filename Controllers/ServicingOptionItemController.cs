using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;
using System.Xml;


namespace CWTDesktopDatabase.Controllers
{
    public class ServicingOptionItemController : Controller
    {
        //main repositories
        ServicingOptionGroupRepository servicingOptionGroupRepository = new ServicingOptionGroupRepository();
        ServicingOptionItemRepository servicingOptionItemRepository = new ServicingOptionItemRepository();

        // GET: List/
        public ActionResult List(int id, int? page, string sortField, int? sortOrder)
        {

            //Get Group
            ServicingOptionGroup servicingOptionGroup = new ServicingOptionGroup();
            servicingOptionGroup = servicingOptionGroupRepository.GetGroup(id);
 
            //Check Exists
            if (servicingOptionGroup == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }
            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToServicingOptionGroup(id))
            {
                ViewData["Access"] = "WriteAccess";
            }

            if (sortField != "ServicingOptionItemSequence")
            {
                sortField = "ServicingOptionName";
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
            }

            ViewData["ServicingOptionGroupId"] = servicingOptionGroup.ServicingOptionGroupId;
            ViewData["ServicingOptionGroupName"] = servicingOptionGroup.ServicingOptionGroupName;

            //return items
            var cwtPaginatedList = servicingOptionItemRepository.PageServicingOptionItems(id, page ?? 1, sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        // GET: /Edit
        public ActionResult Edit(int id)
        {
            
            ServicingOptionItem servicingOptionItem = new ServicingOptionItem();
            servicingOptionItem = servicingOptionItemRepository.GetItem(id);

            //Check Exists
            if (servicingOptionItem == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }
            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToServicingOptionGroup(servicingOptionItem.ServicingOptionGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ServicingOptionRepository servicingOptionRepository = new ServicingOptionRepository();
            SelectList servicingOptionsList = new SelectList(servicingOptionRepository.GetAllServicingOptions().ToList(), "ServicingOptionId", "ServicingOptionName");
            ViewData["ServicingOptions"] = servicingOptionsList;

            GDSRepository gdsRepository = new GDSRepository();
            SelectList gDSList = new SelectList(gdsRepository.GetAllGDSsExceptALL().ToList(), "GDSCode", "GDSName");
            ViewData["GDSs"] = gDSList;

			ViewData["DepartureTimeWindowMinutesList"] = new SelectList(
				servicingOptionRepository.GetServicingOptionDepartureTimeWindows().Select(
					x => new { value = x, text = x }
				), "value", "text", servicingOptionItem.DepartureTimeWindowMinutes);

			ViewData["ArrivalTimeWindowMinutesList"] = new SelectList(
				servicingOptionRepository.GetServicingOptionArrivalTimeWindows().Select(
					x => new { value = x, text = x }
				), "value", "text", servicingOptionItem.ArrivalTimeWindowMinutes);

			ViewData["MaximumStopsList"] = new SelectList(
				servicingOptionRepository.GetServicingOptionMaximumStops().Select(
					x => new { value = x, text = x }
				), "value", "text", servicingOptionItem.MaximumStops);

            servicingOptionItemRepository.EditItemForDisplay(servicingOptionItem);
            return View(servicingOptionItem);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            ServicingOptionItem servicingOptionItem = new ServicingOptionItem();
            servicingOptionItem = servicingOptionItemRepository.GetItem(id);

            //Check Exists
            if (servicingOptionItem == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }
            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToServicingOptionGroup(servicingOptionItem.ServicingOptionGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            try
            {
                UpdateModel(servicingOptionItem);
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
                servicingOptionItemRepository.Edit(servicingOptionItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ServicingOptionItem.mvc/Edit/" + servicingOptionItem.ServicingOptionItemId;
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }


            return RedirectToAction("List", new { id = servicingOptionItem.ServicingOptionGroupId });
        }

        // GET: /Create
        public ActionResult Create(int id)
        {
            //Check Exists
            ServicingOptionGroup servicingOptionGroup = new ServicingOptionGroup();
            servicingOptionGroup = servicingOptionGroupRepository.GetGroup(id);
            if (servicingOptionGroup == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToServicingOptionGroup(id))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            ServicingOptionRepository servicingOptionRepository = new ServicingOptionRepository();
            SelectList servicingOptionsList = new SelectList(servicingOptionRepository.GetAvailableServicingOptions(id).ToList(), "ServicingOptionId", "ServicingOptionName");
            ViewData["ServicingOptions"] = servicingOptionsList;

            GDSRepository gdsRepository = new GDSRepository();
            SelectList gDSList = new SelectList(gdsRepository.GetAllGDSsExceptALL().ToList(), "GDSCode", "GDSName");
            ViewData["GDSs"] = gDSList;

			ViewData["DepartureTimeWindowMinutesList"] = new SelectList(servicingOptionRepository.GetServicingOptionDepartureTimeWindows().Select(x => new { value = x, text = x }), "value", "text");
			ViewData["ArrivalTimeWindowMinutesList"] = new SelectList(servicingOptionRepository.GetServicingOptionArrivalTimeWindows().Select(x => new { value = x, text = x }), "value", "text");
			ViewData["MaximumStopsList"] = new SelectList(servicingOptionRepository.GetServicingOptionMaximumStops().Select(x => new { value = x, text = x }), "value", "text");

            ServicingOptionItem servicingOptionItem = new ServicingOptionItem();
            servicingOptionItem.ServicingOptionGroupName = servicingOptionGroup.ServicingOptionGroupName;
            servicingOptionItem.ServicingOptionGroupId = servicingOptionGroup.ServicingOptionGroupId;

            return View(servicingOptionItem);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServicingOptionItem servicingOptionItem)
        {
            //Check Exists
            ServicingOptionGroup servicingOptionGroup = new ServicingOptionGroup();
            servicingOptionGroup = servicingOptionGroupRepository.GetGroup(servicingOptionItem.ServicingOptionGroupId);
            
            //Check Exists
            if (servicingOptionGroup == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToServicingOptionGroup(servicingOptionItem.ServicingOptionGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(servicingOptionItem);
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
                servicingOptionItemRepository.Add(servicingOptionItem);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = servicingOptionItem.ServicingOptionGroupId });
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            //AccessRights
            ServicingOptionItem servicingOptionItem = new ServicingOptionItem();
            servicingOptionItem = servicingOptionItemRepository.GetItem(id);

            //Check Exists
            if (servicingOptionItem == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }
            //Check Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToServicingOptionGroup(servicingOptionItem.ServicingOptionGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Add Linked Information
            servicingOptionItemRepository.EditItemForDisplay(servicingOptionItem);

            //Return
            return View(servicingOptionItem);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //Get Parent
            ServicingOptionItem servicingOptionItem = new ServicingOptionItem();
            servicingOptionItem = servicingOptionItemRepository.GetItem(id);

            //Check Exists
            if (servicingOptionItem == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }
            //Check Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToServicingOptionGroup(servicingOptionItem.ServicingOptionGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                servicingOptionItem.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                servicingOptionItemRepository.Delete(servicingOptionItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ServicingOptionItem.mvc/Delete/" + servicingOptionItem.ServicingOptionItemId;
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }

            //Return
            return RedirectToAction("List", new { id = servicingOptionItem.ServicingOptionGroupId });
        }

        // GET: /View
        public ActionResult View(int id)
        {
            ServicingOptionItem servicingOptionItem = new ServicingOptionItem();
            servicingOptionItem = servicingOptionItemRepository.GetItem(id);
            if (servicingOptionItem == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }
            servicingOptionItemRepository.EditItemForDisplay(servicingOptionItem);
            return View(servicingOptionItem);
        }

        // GET: /ServicingOptionSelect
        public ActionResult SelectServicingOptionToOrder(int id)
        {
            //Get Group
            ServicingOptionGroup servicingOptionGroup = new ServicingOptionGroup();
            servicingOptionGroup = servicingOptionGroupRepository.GetGroup(id);

            //Check Exists
            if (servicingOptionGroup == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }
            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToServicingOptionGroup(id))
            {
                ViewData["Access"] = "WriteAccess";
            }

            ServicingOptionHFLFVM servicingOptionHFLFVM = new ServicingOptionHFLFVM();
            ServicingOptionRepository servicingOptionRepository = new ServicingOptionRepository();
            servicingOptionHFLFVM.ServicingOptionGroup = servicingOptionGroup;
            servicingOptionHFLFVM.ServicingOptions = new SelectList(servicingOptionRepository.GetServicingOptionsHFLFSelectList(id).ToList(), "ServicingOptionId", "ServicingOptionName");

            return View(servicingOptionHFLFVM);
        }

        // POST: /SelectServicingOptionToOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectServicingOptionToOrder(ServicingOptionHFLFVM servicingOptionHFLFVM)
        {
            int groupId = servicingOptionHFLFVM.ServicingOptionGroup.ServicingOptionGroupId;
            int id = servicingOptionHFLFVM.ServicingOptionId;

            //Get Group
            ServicingOptionGroup servicingOptionGroup = new ServicingOptionGroup();
            servicingOptionGroup = servicingOptionGroupRepository.GetGroup(groupId);

            //Check Exists
            if (servicingOptionGroup == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //Return
            return RedirectToAction("EditSequence", new { groupid = groupId, id = id });
        }

        // GET: /EditSequence
        public ActionResult EditSequence(int groupid, int id, int? page)
        {
            //Check Exists
            //Get Group
            ServicingOptionGroup servicingOptionGroup = new ServicingOptionGroup();
            servicingOptionGroup = servicingOptionGroupRepository.GetGroup(groupid);

            //Check Exists
            if (servicingOptionGroup == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //Check Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToServicingOptionGroup(groupid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            ServicingOptionItemSequenceRepository servicingOptionItemSequenceRepository = new ServicingOptionItemSequenceRepository();
            ServicingOptionRepository servicingOptionRepository = new ServicingOptionRepository();

            ServicingOptionItemSequencesVM servicingOptionItemSequencesVM = new ServicingOptionItemSequencesVM();        
            servicingOptionItemSequencesVM.ServicingOptionItemSequences = servicingOptionItemSequenceRepository.GetServicingOptionItemSequences(groupid, id, page ?? 1);
            servicingOptionItemSequencesVM.ServicingOptionGroup = servicingOptionGroup;
            servicingOptionItemSequencesVM.ServicingOption = servicingOptionRepository.GetServicingOption(id);
            servicingOptionItemSequencesVM.ServicingOptionId = id;
            
            ViewData["Page"] = page ?? 1;


            return View(servicingOptionItemSequencesVM);
        }

        // POST: /EditSequence
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSequence(int groupid, int page, int id, FormCollection collection)
        {

            //Check Exists
            ServicingOptionGroup servicingOptionGroup = new ServicingOptionGroup();
            servicingOptionGroup = servicingOptionGroupRepository.GetGroup(groupid);
            if (servicingOptionGroup == null)
            {
                ViewData["ActionMethod"] = "EditSequencePost";
                return View("RecordDoesNotExistError");
            }
            //Check Exists
            ServicingOption servicingOption = new ServicingOption();
            ServicingOptionRepository servicingOptionRepository = new ServicingOptionRepository();
            servicingOption = servicingOptionRepository.GetServicingOption(id);
            if (servicingOption == null)
            {
                ViewData["ActionMethod"] = "EditSequencePost";
                return View("RecordDoesNotExistError");
            }

            //Check Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToServicingOptionGroup(groupid))
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

                int servicingOptionItemId = Convert.ToInt32(primaryKey[0]);
                int versionNumber = Convert.ToInt32(primaryKey[1]);

                XmlElement xmlItem = doc.CreateElement("Item");
                root.AppendChild(xmlItem);

                XmlElement xmlSequence = doc.CreateElement("Sequence");
                xmlSequence.InnerText = sequence.ToString();
                xmlItem.AppendChild(xmlSequence);

                XmlElement xmlServicingOptionItemId = doc.CreateElement("ServicingOptionItemId");
                xmlServicingOptionItemId.InnerText = servicingOptionItemId.ToString();
                xmlItem.AppendChild(xmlServicingOptionItemId);

                XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                xmlVersionNumber.InnerText = versionNumber.ToString();
                xmlItem.AppendChild(xmlVersionNumber);

                sequence = sequence + 1;
            }

            try
            {
                ServicingOptionItemSequenceRepository servicingOptionItemSequenceRepository = new ServicingOptionItemSequenceRepository();
                servicingOptionItemSequenceRepository.UpdateServicingOptionItemSequences(System.Xml.Linq.XElement.Parse(doc.OuterXml));
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/servicingOptionItem.mvc/EditSequence?page=" + page + "&id=" + id + "&groupid=" + groupid;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = groupid });
        }
    }
}
