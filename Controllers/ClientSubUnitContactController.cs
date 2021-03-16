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
    public class ClientSubUnitContactController : Controller
    {
        ClientDetailRepository clientDetailRepository = new ClientDetailRepository();
        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
        ClientDetailClientSubUnitRepository clientDetailClientSubUnitRepository = new ClientDetailClientSubUnitRepository();
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
                ViewData["ActionMethod"] = "ListSubMenu";
                return View("RecordDoesNotExistError");
            }

            ClientDetailClientSubUnit clientDetailClientSubUnit = new ClientDetailClientSubUnit();
            clientDetailClientSubUnit = clientDetailClientSubUnitRepository.GetClientDetailClientSubUnit(id);

            //Check Exists
            if (clientDetailClientSubUnit == null)
            {
                ViewData["ActionMethod"] = "ListSubMenu";
                return View("RecordDoesNotExistError");
            }

            string clientSubUnitGuid = clientDetailClientSubUnit.ClientSubUnitGuid;

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Populate View Model
            ClientSubUnitContactsVM clientDetailContactsVM = new ClientSubUnitContactsVM();
            clientDetailContactsVM.Contacts = clientDetailRepository.ListClientDetailContacts(id, page ?? 1);
            clientDetailContactsVM.ClientSubUnit = clientSubUnit;
            clientDetailContactsVM.ClientDetail = clientDetail;

            return View(clientDetailContactsVM);
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

            ClientDetailClientSubUnit clientDetailClientSubUnit = new ClientDetailClientSubUnit();
            clientDetailClientSubUnit = clientDetailClientSubUnitRepository.GetClientDetailClientSubUnit(id);

            //Check Exists
            if (clientDetailClientSubUnit == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            string clientSubUnitGuid = clientDetailClientSubUnit.ClientSubUnitGuid;
            

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid))
            {
                ViewData["Access"] = "WriteAccess";
            }

            ClientSubUnitContactVM clientSubUnitContactVM = new ClientSubUnitContactVM();
            clientSubUnitContactVM.ClientSubUnit = clientSubUnit;
            clientSubUnitContactVM.ClientDetail = clientDetail;

            Contact contact = new Contact();
            clientSubUnitContactVM.Contact = contact;

            ContactTypeRepository contactTypeRepository = new ContactTypeRepository();
            clientSubUnitContactVM.ContactTypes = new SelectList(contactTypeRepository.GetAllContactTypes().ToList(), "ContactTypeId", "ContactTypeName", contact.ContactTypeId);

            return View(clientSubUnitContactVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientSubUnitContactVM clientSubUnitContactVM)
        {
            int clientDetailId = clientSubUnitContactVM.ClientDetail.ClientDetailId;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            ClientDetailClientSubUnit clientDetailClientSubUnit = new ClientDetailClientSubUnit();
            clientDetailClientSubUnit = clientDetailClientSubUnitRepository.GetClientDetailClientSubUnit(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnit == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            string clientSubUnitGuid = clientDetailClientSubUnit.ClientSubUnitGuid;
            

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
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            //Update  Model from Form
            try
            {
                TryUpdateModel<Contact>(clientSubUnitContactVM.Contact, "Contact");
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
                clientDetailContactRepository.Add(clientSubUnitContactVM.ClientDetail, clientSubUnitContactVM.Contact);
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
            ClientDetailClientSubUnit clientDetailClientSubUnit = new ClientDetailClientSubUnit();
            clientDetailClientSubUnit = clientDetailClientSubUnitRepository.GetClientDetailClientSubUnit(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnit == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            string clientSubUnitGuid = clientDetailClientSubUnit.ClientSubUnitGuid;
            

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientSubUnitContactVM clientSubUnitContactVM = new ClientSubUnitContactVM();
            clientSubUnitContactVM.ClientSubUnit = clientSubUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            clientSubUnitContactVM.ClientDetail = clientDetail;

            Contact contact = new Contact();
            contact = contactRepository.GetContact(clientDetailContact.ContactId);
            clientSubUnitContactVM.Contact = contact;

            ContactTypeRepository contactTypeRepository = new ContactTypeRepository();
            clientSubUnitContactVM.ContactTypes = new SelectList(contactTypeRepository.GetAllContactTypes().ToList(), "ContactTypeId", "ContactTypeName", contact.ContactTypeId);

            return View(clientSubUnitContactVM);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientSubUnitContactVM clientSubUnitContactVM)
        {
            int clientDetailId = clientSubUnitContactVM.ClientDetail.ClientDetailId;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            ClientDetailClientSubUnit clientDetailClientSubUnit = new ClientDetailClientSubUnit();
            clientDetailClientSubUnit = clientDetailClientSubUnitRepository.GetClientDetailClientSubUnit(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnit == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            string clientSubUnitGuid = clientDetailClientSubUnit.ClientSubUnitGuid;
            

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

            //Check Exists
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            //Update  Model from Form
            try
            {
                TryUpdateModel<Contact>(clientSubUnitContactVM.Contact, "Contact");
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
                contactRepository.Edit(clientSubUnitContactVM.Contact);
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
            ClientDetailClientSubUnit clientDetailClientSubUnit = new ClientDetailClientSubUnit();
            clientDetailClientSubUnit = clientDetailClientSubUnitRepository.GetClientDetailClientSubUnit(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnit == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            string clientSubUnitGuid = clientDetailClientSubUnit.ClientSubUnitGuid;
            

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientSubUnitContactVM clientSubUnitContactVM = new ClientSubUnitContactVM();
            clientSubUnitContactVM.ClientSubUnit = clientSubUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            clientSubUnitContactVM.ClientDetail = clientDetail;

            Contact contact = new Contact();
            contact = contactRepository.GetContact(clientDetailContact.ContactId);
            contactRepository.EditForDisplay(contact);
            clientSubUnitContactVM.Contact = contact;
            
            return View(clientSubUnitContactVM);
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
            ClientDetailClientSubUnit clientDetailClientSubUnit = new ClientDetailClientSubUnit();
            clientDetailClientSubUnit = clientDetailClientSubUnitRepository.GetClientDetailClientSubUnit(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnit == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            string clientSubUnitGuid = clientDetailClientSubUnit.ClientSubUnitGuid;
            

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid))
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
            ClientDetailClientSubUnit clientDetailClientSubUnit = new ClientDetailClientSubUnit();
            clientDetailClientSubUnit = clientDetailClientSubUnitRepository.GetClientDetailClientSubUnit(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnit == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            string clientSubUnitGuid = clientDetailClientSubUnit.ClientSubUnitGuid;
            

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

            ClientSubUnitContactVM clientSubUnitContactVM = new ClientSubUnitContactVM();
            clientSubUnitContactVM.ClientSubUnit = clientSubUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            clientSubUnitContactVM.ClientDetail = clientDetail;

            Contact contact = new Contact();
            contact = contactRepository.GetContact(clientDetailContact.ContactId);
            contactRepository.EditForDisplay(contact);
            clientSubUnitContactVM.Contact = contact;

            return View(clientSubUnitContactVM);
        }
    }
}
