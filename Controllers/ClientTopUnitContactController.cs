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
    public class ClientTopUnitContactController : Controller
    {
        ClientDetailRepository clientDetailRepository = new ClientDetailRepository();
        ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
        ClientDetailClientTopUnitRepository clientDetailClientTopUnitRepository = new ClientDetailClientTopUnitRepository();
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

            ClientDetailClientTopUnit clientDetailClientTopUnit = new ClientDetailClientTopUnit();
            clientDetailClientTopUnit = clientDetailClientTopUnitRepository.GetClientDetailClientTopUnit(id);

            //Check Exists
            if (clientDetailClientTopUnit == null)
            {
                ViewData["ActionMethod"] = "ListSubMenu";
                return View("RecordDoesNotExistError");
            }

            string clientTopUnitGuid = clientDetailClientTopUnit.ClientTopUnitGuid;

            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientTopUnitGuid);

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientTopUnit(clientTopUnitGuid))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Populate View Model
            ClientTopUnitContactsVM clientDetailContactsVM = new ClientTopUnitContactsVM();
            clientDetailContactsVM.Contacts = clientDetailRepository.ListClientDetailContacts(id, page ?? 1);
            clientDetailContactsVM.ClientTopUnit = clientTopUnit;
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

            ClientDetailClientTopUnit clientDetailClientTopUnit = new ClientDetailClientTopUnit();
            clientDetailClientTopUnit = clientDetailClientTopUnitRepository.GetClientDetailClientTopUnit(id);

            //Check Exists
            if (clientDetailClientTopUnit == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            string clientTopUnitGuid = clientDetailClientTopUnit.ClientTopUnitGuid;
            

            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientTopUnitGuid);

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientTopUnit(clientTopUnitGuid))
            {
                ViewData["Access"] = "WriteAccess";
            }

            ClientTopUnitContactVM clientTopUnitContactVM = new ClientTopUnitContactVM();
            clientTopUnitContactVM.ClientTopUnit = clientTopUnit;
            clientTopUnitContactVM.ClientDetail = clientDetail;

            Contact contact = new Contact();
            clientTopUnitContactVM.Contact = contact;

            ContactTypeRepository contactTypeRepository = new ContactTypeRepository();
			clientTopUnitContactVM.ContactTypes = new SelectList(contactTypeRepository.GetAllContactTypes().ToList(), "ContactTypeId", "ContactTypeName", contact.ContactTypeId);

            return View(clientTopUnitContactVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientTopUnitContactVM clientTopUnitContactVM)
        {
            int clientDetailId = clientTopUnitContactVM.ClientDetail.ClientDetailId;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            ClientDetailClientTopUnit clientDetailClientTopUnit = new ClientDetailClientTopUnit();
            clientDetailClientTopUnit = clientDetailClientTopUnitRepository.GetClientDetailClientTopUnit(clientDetailId);

            //Check Exists
            if (clientDetailClientTopUnit == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            string clientTopUnitGuid = clientDetailClientTopUnit.ClientTopUnitGuid;
            

            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientTopUnitGuid);

            //Check Exists
            if (clientTopUnit == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientTopUnit(clientTopUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            //Update  Model from Form
            try
            {
                TryUpdateModel<Contact>(clientTopUnitContactVM.Contact, "Contact");
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
                clientDetailContactRepository.Add(clientTopUnitContactVM.ClientDetail, clientTopUnitContactVM.Contact);
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
            ClientDetailClientTopUnit clientDetailClientTopUnit = new ClientDetailClientTopUnit();
            clientDetailClientTopUnit = clientDetailClientTopUnitRepository.GetClientDetailClientTopUnit(clientDetailId);

            //Check Exists
            if (clientDetailClientTopUnit == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            string clientTopUnitGuid = clientDetailClientTopUnit.ClientTopUnitGuid;
            

            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientTopUnitGuid);

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientTopUnit(clientTopUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientTopUnitContactVM clientTopUnitContactVM = new ClientTopUnitContactVM();
            clientTopUnitContactVM.ClientTopUnit = clientTopUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            clientTopUnitContactVM.ClientDetail = clientDetail;

            Contact contact = new Contact();
            contact = contactRepository.GetContact(clientDetailContact.ContactId);
            clientTopUnitContactVM.Contact = contact;

            ContactTypeRepository contactTypeRepository = new ContactTypeRepository();
			clientTopUnitContactVM.ContactTypes = new SelectList(contactTypeRepository.GetAllContactTypes().ToList(), "ContactTypeId", "ContactTypeName", contact.ContactTypeId);

            return View(clientTopUnitContactVM);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientTopUnitContactVM clientTopUnitContactVM)
        {
            int clientDetailId = clientTopUnitContactVM.ClientDetail.ClientDetailId;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            ClientDetailClientTopUnit clientDetailClientTopUnit = new ClientDetailClientTopUnit();
            clientDetailClientTopUnit = clientDetailClientTopUnitRepository.GetClientDetailClientTopUnit(clientDetailId);

            //Check Exists
            if (clientDetailClientTopUnit == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            string clientTopUnitGuid = clientDetailClientTopUnit.ClientTopUnitGuid;
            

            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientTopUnitGuid);

            //Check Exists
            if (clientTopUnit == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientTopUnit(clientTopUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            //Update  Model from Form
            try
            {
                TryUpdateModel<Contact>(clientTopUnitContactVM.Contact, "Contact");
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
                contactRepository.Edit(clientTopUnitContactVM.Contact);
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
            ClientDetailClientTopUnit clientDetailClientTopUnit = new ClientDetailClientTopUnit();
            clientDetailClientTopUnit = clientDetailClientTopUnitRepository.GetClientDetailClientTopUnit(clientDetailId);

            //Check Exists
            if (clientDetailClientTopUnit == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            string clientTopUnitGuid = clientDetailClientTopUnit.ClientTopUnitGuid;
            

            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientTopUnitGuid);

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientTopUnit(clientTopUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientTopUnitContactVM clientTopUnitContactVM = new ClientTopUnitContactVM();
            clientTopUnitContactVM.ClientTopUnit = clientTopUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            clientTopUnitContactVM.ClientDetail = clientDetail;

            Contact contact = new Contact();
            contact = contactRepository.GetContact(clientDetailContact.ContactId);
            contactRepository.EditForDisplay(contact);
            clientTopUnitContactVM.Contact = contact;
            
            return View(clientTopUnitContactVM);
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
            ClientDetailClientTopUnit clientDetailClientTopUnit = new ClientDetailClientTopUnit();
            clientDetailClientTopUnit = clientDetailClientTopUnitRepository.GetClientDetailClientTopUnit(clientDetailId);

            //Check Exists
            if (clientDetailClientTopUnit == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            string clientTopUnitGuid = clientDetailClientTopUnit.ClientTopUnitGuid;
            

            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientTopUnitGuid);

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientTopUnit(clientTopUnitGuid))
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
                    ViewData["ReturnURL"] = "/ClientTopUnitContact.mvc/Delete/" + id.ToString();
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
            ClientDetailClientTopUnit clientDetailClientTopUnit = new ClientDetailClientTopUnit();
            clientDetailClientTopUnit = clientDetailClientTopUnitRepository.GetClientDetailClientTopUnit(clientDetailId);

            //Check Exists
            if (clientDetailClientTopUnit == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            string clientTopUnitGuid = clientDetailClientTopUnit.ClientTopUnitGuid;
            

            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientTopUnitGuid);

            ClientTopUnitContactVM clientTopUnitContactVM = new ClientTopUnitContactVM();
            clientTopUnitContactVM.ClientTopUnit = clientTopUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            clientTopUnitContactVM.ClientDetail = clientDetail;

            Contact contact = new Contact();
            contact = contactRepository.GetContact(clientDetailContact.ContactId);
            contactRepository.EditForDisplay(contact);
            clientTopUnitContactVM.Contact = contact;

            return View(clientTopUnitContactVM);
        }
    }
}
