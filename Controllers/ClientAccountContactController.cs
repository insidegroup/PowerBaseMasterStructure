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
    public class ClientAccountContactController : Controller
    {
        ClientDetailRepository clientDetailRepository = new ClientDetailRepository();
        ClientAccountRepository clientAccountRepository = new ClientAccountRepository();
        ClientDetailClientAccountRepository clientDetailClientAccountRepository = new ClientDetailClientAccountRepository();
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

            ClientDetailClientAccount clientDetailClientAccount = new ClientDetailClientAccount();
            clientDetailClientAccount = clientDetailClientAccountRepository.GetClientDetailClientAccount(id);

            //Check Exists
            if (clientDetailClientAccount == null)
            {
                ViewData["ActionMethod"] = "ListSubMenu";
                return View("RecordDoesNotExistError");
            }

            string can = clientDetailClientAccount.ClientAccountNumber;
            string ssc = clientDetailClientAccount.SourceSystemCode;

            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientAccount(can,ssc))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Populate View Model
            ClientAccountContactsVM clientDetailContactsVM = new ClientAccountContactsVM();
            clientDetailContactsVM.Contacts = clientDetailRepository.ListClientDetailContacts(id, page ?? 1);
            clientDetailContactsVM.ClientAccount = clientAccount;
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

            ClientDetailClientAccount clientDetailClientAccount = new ClientDetailClientAccount();
            clientDetailClientAccount = clientDetailClientAccountRepository.GetClientDetailClientAccount(id);

            //Check Exists
            if (clientDetailClientAccount == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            string can = clientDetailClientAccount.ClientAccountNumber;
            string ssc = clientDetailClientAccount.SourceSystemCode;

            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientAccount(can, ssc))
            {
                ViewData["Access"] = "WriteAccess";
            }

            ClientAccountContactVM clientAccountContactVM = new ClientAccountContactVM();
            clientAccountContactVM.ClientAccount = clientAccount;
            clientAccountContactVM.ClientDetail = clientDetail;

            Contact contact = new Contact();
            clientAccountContactVM.Contact = contact;

            ContactTypeRepository contactTypeRepository = new ContactTypeRepository();
            clientAccountContactVM.ContactTypes = new SelectList(contactTypeRepository.GetAllContactTypes().ToList(), "ContactTypeId", "ContactTypeName", contact.ContactTypeId);

            return View(clientAccountContactVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientAccountContactVM clientAccountContactVM)
        {
            int clientDetailId = clientAccountContactVM.ClientDetail.ClientDetailId;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            ClientDetailClientAccount clientDetailClientAccount = new ClientDetailClientAccount();
            clientDetailClientAccount = clientDetailClientAccountRepository.GetClientDetailClientAccount(clientDetailId);

            //Check Exists
            if (clientDetailClientAccount == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            string can = clientDetailClientAccount.ClientAccountNumber;
            string ssc = clientDetailClientAccount.SourceSystemCode;

            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Check Exists
            if (clientAccount == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientAccount(can, ssc))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            //Update  Model from Form
            try
            {
                TryUpdateModel<Contact>(clientAccountContactVM.Contact, "Contact");
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
                clientDetailContactRepository.Add(clientAccountContactVM.ClientDetail, clientAccountContactVM.Contact);
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
            ClientDetailClientAccount clientDetailClientAccount = new ClientDetailClientAccount();
            clientDetailClientAccount = clientDetailClientAccountRepository.GetClientDetailClientAccount(clientDetailId);

            //Check Exists
            if (clientDetailClientAccount == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            string can = clientDetailClientAccount.ClientAccountNumber;
            string ssc = clientDetailClientAccount.SourceSystemCode;

            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientAccount(can, ssc))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientAccountContactVM clientAccountContactVM = new ClientAccountContactVM();
            clientAccountContactVM.ClientAccount = clientAccount;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            clientAccountContactVM.ClientDetail = clientDetail;

            Contact contact = new Contact();
            contact = contactRepository.GetContact(clientDetailContact.ContactId);
            clientAccountContactVM.Contact = contact;

            ContactTypeRepository contactTypeRepository = new ContactTypeRepository();
            clientAccountContactVM.ContactTypes = new SelectList(contactTypeRepository.GetAllContactTypes().ToList(), "ContactTypeId", "ContactTypeName", contact.ContactTypeId);

            return View(clientAccountContactVM);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientAccountContactVM clientAccountContactVM)
        {
            int clientDetailId = clientAccountContactVM.ClientDetail.ClientDetailId;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            ClientDetailClientAccount clientDetailClientAccount = new ClientDetailClientAccount();
            clientDetailClientAccount = clientDetailClientAccountRepository.GetClientDetailClientAccount(clientDetailId);

            //Check Exists
            if (clientDetailClientAccount == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            string can = clientDetailClientAccount.ClientAccountNumber;
            string ssc = clientDetailClientAccount.SourceSystemCode;

            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Check Exists
            if (clientAccount == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientAccount(can, ssc))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            //Update  Model from Form
            try
            {
                TryUpdateModel<Contact>(clientAccountContactVM.Contact, "Contact");
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
                contactRepository.Edit(clientAccountContactVM.Contact);
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
            ClientDetailClientAccount clientDetailClientAccount = new ClientDetailClientAccount();
            clientDetailClientAccount = clientDetailClientAccountRepository.GetClientDetailClientAccount(clientDetailId);

            //Check Exists
            if (clientDetailClientAccount == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            string can = clientDetailClientAccount.ClientAccountNumber;
            string ssc = clientDetailClientAccount.SourceSystemCode;

            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientAccount(can, ssc))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientAccountContactVM clientAccountContactVM = new ClientAccountContactVM();
            clientAccountContactVM.ClientAccount = clientAccount;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            clientAccountContactVM.ClientDetail = clientDetail;

            Contact contact = new Contact();
            contact = contactRepository.GetContact(clientDetailContact.ContactId);
            contactRepository.EditForDisplay(contact);
            clientAccountContactVM.Contact = contact;
            
            return View(clientAccountContactVM);
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
            ClientDetailClientAccount clientDetailClientAccount = new ClientDetailClientAccount();
            clientDetailClientAccount = clientDetailClientAccountRepository.GetClientDetailClientAccount(clientDetailId);

            //Check Exists
            if (clientDetailClientAccount == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            string can = clientDetailClientAccount.ClientAccountNumber;
            string ssc = clientDetailClientAccount.SourceSystemCode;

            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientAccount(can, ssc))
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
                    ViewData["ReturnURL"] = "/ClientAccountContact.mvc/Delete/" + id.ToString();
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
            ClientDetailClientAccount clientDetailClientAccount = new ClientDetailClientAccount();
            clientDetailClientAccount = clientDetailClientAccountRepository.GetClientDetailClientAccount(clientDetailId);

            //Check Exists
            if (clientDetailClientAccount == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            string can = clientDetailClientAccount.ClientAccountNumber;
            string ssc = clientDetailClientAccount.SourceSystemCode;

            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            ClientAccountContactVM clientAccountContactVM = new ClientAccountContactVM();
            clientAccountContactVM.ClientAccount = clientAccount;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            clientAccountContactVM.ClientDetail = clientDetail;

            Contact contact = new Contact();
            contact = contactRepository.GetContact(clientDetailContact.ContactId);
            contactRepository.EditForDisplay(contact);
            clientAccountContactVM.Contact = contact;

            return View(clientAccountContactVM);
        }
    }
}
