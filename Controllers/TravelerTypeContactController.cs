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
    public class TravelerTypeContactController : Controller
    {
        ClientDetailRepository clientDetailRepository = new ClientDetailRepository();
        TravelerTypeRepository travelerTypeRepository = new TravelerTypeRepository();
        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
        ClientSubUnitTravelerTypeRepository clientSubUnitTravelerTypeRepository = new ClientSubUnitTravelerTypeRepository();
        ClientDetailTravelerTypeRepository clientDetailTravelerTypeRepository = new ClientDetailTravelerTypeRepository();
        ClientDetailContactRepository clientDetailContactRepository = new ClientDetailContactRepository();
        ContactRepository contactRepository = new ContactRepository();

        // GET: /List
        public ActionResult List(string csu, string tt, int id, int? page)
        {
            ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
            clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(id);

            //Check Exists
            if (clientDetailTravelerType == null)
            {
                ViewData["ActionMethod"] = "List";
                return View("RecordDoesNotExistError");
            }

            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu,tt);

            //Check Exists
            if (clientDetailTravelerType == null)
            {
                ViewData["ActionMethod"] = "List";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Populate View Model
            TravelerTypeContactsVM travelerTypeContactsVM = new TravelerTypeContactsVM();
            travelerTypeContactsVM.Contacts = clientDetailRepository.ListClientDetailContacts(id, page ?? 1);

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            travelerTypeContactsVM.ClientSubUnit = clientSubUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);
            travelerTypeContactsVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            travelerTypeContactsVM.TravelerType = travelerType;

            return View(travelerTypeContactsVM);
        }
        
        // GET: /Create
        public ActionResult Create(int id, string csu, string tt)
        {
            ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
            clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(id);

            //Check Exists
            if (clientDetailTravelerType == null)
            {
                ViewData["ActionMethod"] = "ListSubMenu";
                return View("RecordDoesNotExistError");
            }

            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

            //Check Exists
            if (clientDetailTravelerType == null)
            {
                ViewData["ActionMethod"] = "ListSubMenu";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Access"] = "WriteAccess";
            }

            TravelerTypeContactVM travelerTypeContactVM = new TravelerTypeContactVM();

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            travelerTypeContactVM.ClientSubUnit = clientSubUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);
            travelerTypeContactVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            travelerTypeContactVM.TravelerType = travelerType;

            Contact contact = new Contact();
            travelerTypeContactVM.Contact = contact;

            ContactTypeRepository contactTypeRepository = new ContactTypeRepository();
            travelerTypeContactVM.ContactTypes = new SelectList(contactTypeRepository.GetAllContactTypes().ToList(), "ContactTypeId", "ContactTypeName", contact.ContactTypeId);

            return View(travelerTypeContactVM);
        }
       
        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TravelerTypeContactVM travelerTypeContactVM)
        {
            int clientDetailId = travelerTypeContactVM.ClientDetail.ClientDetailId;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
            clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(clientDetailId);

            //Check Exists
            if (clientDetailTravelerType == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            string csu = travelerTypeContactVM.ClientSubUnit.ClientSubUnitGuid;
            string tt = travelerTypeContactVM.TravelerType.TravelerTypeGuid;

            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

            //Check Exists
            if (clientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "ListSubMenu";
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
                TryUpdateModel<Contact>(travelerTypeContactVM.Contact, "Contact");
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
                clientDetailContactRepository.Add(travelerTypeContactVM.ClientDetail, travelerTypeContactVM.Contact);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", new { id = clientDetailId, csu = csu, tt=tt });
        }
        
        // GET: /Edit
        public ActionResult Edit(string csu, int id)
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
           ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
           clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(clientDetailId);

           //Check Exists
           if (clientDetailTravelerType == null)
           {
               ViewData["ActionMethod"] = "EditGet";
               return View("RecordDoesNotExistError");
           }

           string tt = clientDetailTravelerType.TravelerTypeGuid;
           ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
           clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

           //Check Exists
           if (clientDetailTravelerType == null)
           {
               ViewData["ActionMethod"] = "EditGet";
               return View("RecordDoesNotExistError");
           }

           //Access Rights
           RolesRepository rolesRepository = new RolesRepository();
           if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
           {
               ViewData["Message"] = "You do not have access to this item";
               return View("Error");
           }           

           TravelerTypeContactVM travelerTypeContactVM = new TravelerTypeContactVM();

           ClientSubUnit clientSubUnit = new ClientSubUnit();
           clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu); 
           travelerTypeContactVM.ClientSubUnit = clientSubUnit;

           ClientDetail clientDetail = new ClientDetail();
           clientDetail = clientDetailRepository.GetGroup(clientDetailId);
           travelerTypeContactVM.ClientDetail = clientDetail;

           TravelerType travelerType = new TravelerType();
           travelerType = travelerTypeRepository.GetTravelerType(tt);
           travelerTypeContactVM.TravelerType = travelerType;

           Contact contact = new Contact();
           contact = contactRepository.GetContact(clientDetailContact.ContactId);
           travelerTypeContactVM.Contact = contact;

           ContactTypeRepository contactTypeRepository = new ContactTypeRepository();
           travelerTypeContactVM.ContactTypes = new SelectList(contactTypeRepository.GetAllContactTypes().ToList(), "ContactTypeId", "ContactTypeName", contact.ContactTypeId);

           return View(travelerTypeContactVM);
       }
       
        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TravelerTypeContactVM travelerTypeContactVM)
       {
           int clientDetailId = travelerTypeContactVM.ClientDetail.ClientDetailId;

           ClientDetail clientDetail = new ClientDetail();
           clientDetail = clientDetailRepository.GetGroup(clientDetailId);

           //Check Exists
           if (clientDetail == null)
           {
               ViewData["ActionMethod"] = "CreatePost";
               return View("RecordDoesNotExistError");
           }

           ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
           clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(clientDetailId);

           //Check Exists
           if (clientDetailTravelerType == null)
           {
               ViewData["ActionMethod"] = "CreatePost";
               return View("RecordDoesNotExistError");
           }

           string csu = travelerTypeContactVM.ClientSubUnit.ClientSubUnitGuid;
           string tt = travelerTypeContactVM.TravelerType.TravelerTypeGuid;

           ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
           clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

           //Check Exists
           if (clientSubUnitTravelerType == null)
           {
               ViewData["ActionMethod"] = "ListSubMenu";
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
               TryUpdateModel<Contact>(travelerTypeContactVM.Contact, "Contact");
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
               contactRepository.Edit(travelerTypeContactVM.Contact);
           }
           catch (SqlException ex)
           {
               LogRepository logRepository = new LogRepository();
               logRepository.LogError(ex.Message);

               ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
               return View("Error");
           }


           return RedirectToAction("List", new { id = clientDetailId, csu = csu, tt = tt });
       }
  
        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(string csu, int id)
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
          ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
          clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(clientDetailId);

          //Check Exists
          if (clientDetailTravelerType == null)
          {
              ViewData["ActionMethod"] = "DeleteGet";
              return View("RecordDoesNotExistError");
          }

          string tt = clientDetailTravelerType.TravelerTypeGuid;
          ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
          clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

          //Check Exists
          if (clientDetailTravelerType == null)
          {
              ViewData["ActionMethod"] = "DeleteGet";
              return View("RecordDoesNotExistError");
          }

          //Access Rights
          RolesRepository rolesRepository = new RolesRepository();
          if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
          {
              ViewData["Message"] = "You do not have access to this item";
              return View("Error");
          }

          TravelerTypeContactVM travelerTypeContactVM = new TravelerTypeContactVM();

          ClientSubUnit clientSubUnit = new ClientSubUnit();
          clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
          travelerTypeContactVM.ClientSubUnit = clientSubUnit;

          ClientDetail clientDetail = new ClientDetail();
          clientDetail = clientDetailRepository.GetGroup(clientDetailId);
          travelerTypeContactVM.ClientDetail = clientDetail;

          TravelerType travelerType = new TravelerType();
          travelerType = travelerTypeRepository.GetTravelerType(tt);
          travelerTypeContactVM.TravelerType = travelerType;

          Contact contact = new Contact();
          contact = contactRepository.GetContact(clientDetailContact.ContactId);
          contactRepository.EditForDisplay(contact);
          travelerTypeContactVM.Contact = contact;

          return View(travelerTypeContactVM);
      }
        
        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(TravelerTypeContactVM travelerTypeContactVM, FormCollection collection)
    {
        int contactId = travelerTypeContactVM.Contact.ContactId;

        Contact contact = new Contact();
        contact = contactRepository.GetContact(contactId);

        if (contact == null)
        {
            ViewData["ActionMethod"] = "DeletePost";
            return View("RecordDoesNotExistError");
        }

        int clientDetailId = travelerTypeContactVM.ClientDetail.ClientDetailId;

        ClientDetail clientDetail = new ClientDetail();
        clientDetail = clientDetailRepository.GetGroup(clientDetailId);

        //Check Exists
        if (clientDetail == null)
        {
            ViewData["ActionMethod"] = "DeletePost";
            return View("RecordDoesNotExistError");
        }

        ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
        clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(clientDetailId);

        //Check Exists
        if (clientDetailTravelerType == null)
        {
            ViewData["ActionMethod"] = "DeletePost";
            return View("RecordDoesNotExistError");
        }

        string csu = travelerTypeContactVM.ClientSubUnit.ClientSubUnitGuid;
        string tt = travelerTypeContactVM.TravelerType.TravelerTypeGuid;

        ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
        clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

        //Check Exists
        if (clientSubUnitTravelerType == null)
        {
            ViewData["ActionMethod"] = "DeletePost";
            return View("RecordDoesNotExistError");
        }

        //Access Rights
        RolesRepository rolesRepository = new RolesRepository();
        if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
        {
            ViewData["Message"] = "You do not have access to this item";
            return View("Error");
        }

       

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
                ViewData["ReturnURL"] = "/TravelerTypeContact.mvc/Delete/id=" + contactId.ToString() + "&csu=" + csu;
                return View("VersionError");
            }
            //Generic Error
            ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
            return View("Error");
        }


        return RedirectToAction("List", new { id = clientDetailId, csu = csu, tt = tt });
    }
        
        // GET: /View
        public ActionResult View(string csu, int id)
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
            ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
            clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(clientDetailId);

            //Check Exists
            if (clientDetailTravelerType == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            string tt = clientDetailTravelerType.TravelerTypeGuid;
            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

            //Check Exists
            if (clientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            TravelerTypeContactVM travelerTypeContactVM = new TravelerTypeContactVM();

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            travelerTypeContactVM.ClientSubUnit = clientSubUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            travelerTypeContactVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            travelerTypeContactVM.TravelerType = travelerType;

            Contact contact = new Contact();
            contact = contactRepository.GetContact(clientDetailContact.ContactId);
            contactRepository.EditForDisplay(contact);
            travelerTypeContactVM.Contact = contact;

            return View(travelerTypeContactVM);
        }
    }
}
