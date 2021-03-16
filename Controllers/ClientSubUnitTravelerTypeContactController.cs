using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.Helpers;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class ClientSubUnitTravelerTypeContactController : Controller
    {
        ClientDetailRepository clientDetailRepository = new ClientDetailRepository();
        TravelerTypeRepository travelerTypeRepository = new TravelerTypeRepository();
        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
        ClientDetailClientSubUnitTravelerTypeRepository clientDetailClientSubUnitTravelerTypeRepository = new ClientDetailClientSubUnitTravelerTypeRepository();
        ClientDetailContactRepository clientDetailContactRepository = new ClientDetailContactRepository();
        ContactRepository contactRepository = new ContactRepository();

        // GET: /List
        public ActionResult List(int id, int? page)
        {
            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "List";
                return View("RecordDoesNotExistError");
            }

            ClientDetailClientSubUnitTravelerType clientDetailClientSubUnitTravelerType = new ClientDetailClientSubUnitTravelerType();
            clientDetailClientSubUnitTravelerType = clientDetailClientSubUnitTravelerTypeRepository.GetClientDetailClientSubUnitTravelerType(id);

            //Check Exists
            if (clientDetailClientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "List";
                return View("RecordDoesNotExistError");
            }

            string csu = clientDetailClientSubUnitTravelerType.ClientSubUnitGuid;
            string tt = clientDetailClientSubUnitTravelerType.TravelerTypeGuid;

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Populate View Model
            ClientSubUnitTravelerTypeContactsVM clientSubUnitTravelerTypeContactsVM = new ClientSubUnitTravelerTypeContactsVM();
            clientSubUnitTravelerTypeContactsVM.Contacts = clientDetailRepository.ListClientDetailContacts(id, page ?? 1);
            clientSubUnitTravelerTypeContactsVM.ClientSubUnit = clientSubUnit;
            clientSubUnitTravelerTypeContactsVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            clientSubUnitTravelerTypeContactsVM.TravelerType = travelerType;

            return View(clientSubUnitTravelerTypeContactsVM);
        }

        // GET: /Create
        public ActionResult Create(int id)
        {
            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            ClientDetailClientSubUnitTravelerType clientDetailClientSubUnitTravelerType = new ClientDetailClientSubUnitTravelerType();
            clientDetailClientSubUnitTravelerType = clientDetailClientSubUnitTravelerTypeRepository.GetClientDetailClientSubUnitTravelerType(id);

            //Check Exists
            if (clientDetailClientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            string csu = clientDetailClientSubUnitTravelerType.ClientSubUnitGuid;
            string tt = clientDetailClientSubUnitTravelerType.TravelerTypeGuid;

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Access"] = "WriteAccess";
            }

            ClientSubUnitTravelerTypeContactVM clientSubUnitTravelerTypeContactVM = new ClientSubUnitTravelerTypeContactVM();
            clientSubUnitTravelerTypeContactVM.ClientSubUnit = clientSubUnit;
            clientSubUnitTravelerTypeContactVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            clientSubUnitTravelerTypeContactVM.TravelerType = travelerType;

            Contact contact = new Contact();
            clientSubUnitTravelerTypeContactVM.Contact = contact;

            ContactTypeRepository contactTypeRepository = new ContactTypeRepository();
            clientSubUnitTravelerTypeContactVM.ContactTypes = new SelectList(contactTypeRepository.GetAllContactTypes().ToList(), "ContactTypeId", "ContactTypeName", contact.ContactTypeId);

            return View(clientSubUnitTravelerTypeContactVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientSubUnitTravelerTypeContactVM clientSubUnitTravelerTypeContactVM)
        {
            int clientDetailId = clientSubUnitTravelerTypeContactVM.ClientDetail.ClientDetailId;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            ClientDetailClientSubUnitTravelerType clientDetailClientSubUnitTravelerType = new ClientDetailClientSubUnitTravelerType();
            clientDetailClientSubUnitTravelerType = clientDetailClientSubUnitTravelerTypeRepository.GetClientDetailClientSubUnitTravelerType(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            string csu = clientDetailClientSubUnitTravelerType.ClientSubUnitGuid;
            string tt = clientDetailClientSubUnitTravelerType.TravelerTypeGuid;


            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            //Check Exists
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            //Update  Model from Form
            try
            {
                TryUpdateModel<Contact>(clientSubUnitTravelerTypeContactVM.Contact, "Contact");
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
                clientDetailContactRepository.Add(clientSubUnitTravelerTypeContactVM.ClientDetail, clientSubUnitTravelerTypeContactVM.Contact);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", new { id = clientDetailId });
        }

        // GET: /Edit
        public ActionResult Edit(int id)
        {
            ClientDetailContact clientDetailContact = new ClientDetailContact();
            clientDetailContact = clientDetailContactRepository.GetContactClientDetail(id);

            //Check Exists
            if (clientDetailContact == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            int clientDetailId = clientDetailContact.ClientDetailId;
            ClientDetailClientSubUnitTravelerType clientDetailClientSubUnitTravelerType = new ClientDetailClientSubUnitTravelerType();
            clientDetailClientSubUnitTravelerType = clientDetailClientSubUnitTravelerTypeRepository.GetClientDetailClientSubUnitTravelerType(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            string csu = clientDetailClientSubUnitTravelerType.ClientSubUnitGuid;
            string tt = clientDetailClientSubUnitTravelerType.TravelerTypeGuid;


            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientSubUnitTravelerTypeContactVM clientSubUnitTravelerTypeContactVM = new ClientSubUnitTravelerTypeContactVM();
            clientSubUnitTravelerTypeContactVM.ClientSubUnit = clientSubUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            clientSubUnitTravelerTypeContactVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            clientSubUnitTravelerTypeContactVM.TravelerType = travelerType;

            Contact contact = new Contact();
            contact = contactRepository.GetContact(clientDetailContact.ContactId);
            clientSubUnitTravelerTypeContactVM.Contact = contact;

            ContactTypeRepository contactTypeRepository = new ContactTypeRepository();
            clientSubUnitTravelerTypeContactVM.ContactTypes = new SelectList(contactTypeRepository.GetAllContactTypes().ToList(), "ContactTypeId", "ContactTypeName", contact.ContactTypeId);

            return View(clientSubUnitTravelerTypeContactVM);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientSubUnitTravelerTypeContactVM clientSubUnitTravelerTypeContactVM)
        {
            int clientDetailId = clientSubUnitTravelerTypeContactVM.ClientDetail.ClientDetailId;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            ClientDetailClientSubUnitTravelerType clientDetailClientSubUnitTravelerType = new ClientDetailClientSubUnitTravelerType();
            clientDetailClientSubUnitTravelerType = clientDetailClientSubUnitTravelerTypeRepository.GetClientDetailClientSubUnitTravelerType(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            string csu = clientDetailClientSubUnitTravelerType.ClientSubUnitGuid;
            string tt = clientDetailClientSubUnitTravelerType.TravelerTypeGuid;


            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            //Check Exists
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            //Update  Model from Form
            try
            {
                TryUpdateModel<Contact>(clientSubUnitTravelerTypeContactVM.Contact, "Contact");
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
                contactRepository.Edit(clientSubUnitTravelerTypeContactVM.Contact);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", new { id = clientDetailId });
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            ClientDetailContact clientDetailContact = new ClientDetailContact();
            clientDetailContact = clientDetailContactRepository.GetContactClientDetail(id);

            //Check Exists
            if (clientDetailContact == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            int clientDetailId = clientDetailContact.ClientDetailId;
            ClientDetailClientSubUnitTravelerType clientDetailClientSubUnitTravelerType = new ClientDetailClientSubUnitTravelerType();
            clientDetailClientSubUnitTravelerType = clientDetailClientSubUnitTravelerTypeRepository.GetClientDetailClientSubUnitTravelerType(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            string csu = clientDetailClientSubUnitTravelerType.ClientSubUnitGuid;
            string tt = clientDetailClientSubUnitTravelerType.TravelerTypeGuid;

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientSubUnitTravelerTypeContactVM clientSubUnitTravelerTypeContactVM = new ClientSubUnitTravelerTypeContactVM();
            clientSubUnitTravelerTypeContactVM.ClientSubUnit = clientSubUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            clientSubUnitTravelerTypeContactVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            clientSubUnitTravelerTypeContactVM.TravelerType = travelerType;


            Contact contact = new Contact();
            contact = contactRepository.GetContact(clientDetailContact.ContactId);
            contactRepository.EditForDisplay(contact);
            clientSubUnitTravelerTypeContactVM.Contact = contact;

            return View(clientSubUnitTravelerTypeContactVM);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            ClientDetailContact clientDetailContact = new ClientDetailContact();
            clientDetailContact = clientDetailContactRepository.GetContactClientDetail(id);

            //Check Exists
            if (clientDetailContact == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            int clientDetailId = clientDetailContact.ClientDetailId;
            ClientDetailClientSubUnitTravelerType clientDetailClientSubUnitTravelerType = new ClientDetailClientSubUnitTravelerType();
            clientDetailClientSubUnitTravelerType = clientDetailClientSubUnitTravelerTypeRepository.GetClientDetailClientSubUnitTravelerType(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            string csu = clientDetailClientSubUnitTravelerType.ClientSubUnitGuid;
            string tt = clientDetailClientSubUnitTravelerType.TravelerTypeGuid;


            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            Contact contact = new Contact();
            contact = contactRepository.GetContact(id);

            //Delete Item
            try
            {
                contact.VersionNumber = Int32.Parse(collection["Contact.VersionNumber"]);
                contactRepository.Delete(contact);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ClientSubUnitContact.mvc/Delete/" + id.ToString();
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", new { id = clientDetailId });
        }

        // GET: /View
        public ActionResult View(int id)
        {
            ClientDetailContact clientDetailContact = new ClientDetailContact();
            clientDetailContact = clientDetailContactRepository.GetContactClientDetail(id);

            //Check Exists
            if (clientDetailContact == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            int clientDetailId = clientDetailContact.ClientDetailId;
            ClientDetailClientSubUnitTravelerType clientDetailClientSubUnitTravelerType = new ClientDetailClientSubUnitTravelerType();
            clientDetailClientSubUnitTravelerType = clientDetailClientSubUnitTravelerTypeRepository.GetClientDetailClientSubUnitTravelerType(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            string csu = clientDetailClientSubUnitTravelerType.ClientSubUnitGuid;
            string tt = clientDetailClientSubUnitTravelerType.TravelerTypeGuid;


            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            ClientSubUnitTravelerTypeContactVM clientSubUnitTravelerTypeContactVM = new ClientSubUnitTravelerTypeContactVM();
            clientSubUnitTravelerTypeContactVM.ClientSubUnit = clientSubUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            clientSubUnitTravelerTypeContactVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            clientSubUnitTravelerTypeContactVM.TravelerType = travelerType;

            Contact contact = new Contact();
            contact = contactRepository.GetContact(clientDetailContact.ContactId);
            contactRepository.EditForDisplay(contact);
            clientSubUnitTravelerTypeContactVM.Contact = contact;

            return View(clientSubUnitTravelerTypeContactVM);
        }
    }
}
