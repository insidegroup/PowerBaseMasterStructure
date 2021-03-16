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
    public class TravelerTypeESCInformationController : Controller
    {
        ClientDetailRepository clientDetailRepository = new ClientDetailRepository();
        TravelerTypeRepository travelerTypeRepository = new TravelerTypeRepository();
        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
        ClientSubUnitTravelerTypeRepository clientSubUnitTravelerTypeRepository = new ClientSubUnitTravelerTypeRepository();
        ClientDetailTravelerTypeRepository clientDetailTravelerTypeRepository = new ClientDetailTravelerTypeRepository();
        ClientDetailESCInformationRepository clientDetailESCInformationRepository = new ClientDetailESCInformationRepository();

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
            TravelerTypeESCInformationVM travelerTypeESCInformationVM = new TravelerTypeESCInformationVM();
            travelerTypeESCInformationVM.ClientDetailESCInformation = clientDetailRepository.GetClientDetailESCInformation(id);

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            travelerTypeESCInformationVM.ClientSubUnit = clientSubUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);
            travelerTypeESCInformationVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            travelerTypeESCInformationVM.TravelerType = travelerType;

            return View(travelerTypeESCInformationVM);
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

            TravelerTypeESCInformationVM travelerTypeESCInformationVM = new TravelerTypeESCInformationVM();

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            travelerTypeESCInformationVM.ClientSubUnit = clientSubUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);
            travelerTypeESCInformationVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            travelerTypeESCInformationVM.TravelerType = travelerType;

            ClientDetailESCInformation clientDetailESCInformation = new ClientDetailESCInformation();
            travelerTypeESCInformationVM.ClientDetailESCInformation = clientDetailESCInformation;

            return View(travelerTypeESCInformationVM);
        }
       
        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TravelerTypeESCInformationVM travelerTypeESCInformationVM)
        {
            int clientDetailId = travelerTypeESCInformationVM.ClientDetail.ClientDetailId;

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

            string csu = travelerTypeESCInformationVM.ClientSubUnit.ClientSubUnitGuid;
            string tt = travelerTypeESCInformationVM.TravelerType.TravelerTypeGuid;

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
                TryUpdateModel<ClientDetailESCInformation>(travelerTypeESCInformationVM.ClientDetailESCInformation, "ClientDetailESCInformation");
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
                clientDetailESCInformationRepository.Add(travelerTypeESCInformationVM.ClientDetail, travelerTypeESCInformationVM.ClientDetailESCInformation);
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
           ClientDetailESCInformation clientDetailESCInformation = new ClientDetailESCInformation();
           clientDetailESCInformation = clientDetailESCInformationRepository.GetClientDetailESCInformation(id);

           //Check Exists
           if (clientDetailESCInformation == null)
           {
               ViewData["ActionMethod"] = "EditGet";
               return View("RecordDoesNotExistError");
           }

           int clientDetailId = clientDetailESCInformation.ClientDetailId;
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

           TravelerTypeESCInformationVM travelerTypeESCInformationVM = new TravelerTypeESCInformationVM();

           ClientSubUnit clientSubUnit = new ClientSubUnit();
           clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu); 
           travelerTypeESCInformationVM.ClientSubUnit = clientSubUnit;
           travelerTypeESCInformationVM.ClientDetailESCInformation = clientDetailESCInformation;

           ClientDetail clientDetail = new ClientDetail();
           clientDetail = clientDetailRepository.GetGroup(clientDetailId);
           travelerTypeESCInformationVM.ClientDetail = clientDetail;

           TravelerType travelerType = new TravelerType();
           travelerType = travelerTypeRepository.GetTravelerType(tt);
           travelerTypeESCInformationVM.TravelerType = travelerType;

           

           return View(travelerTypeESCInformationVM);
       }
       
        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TravelerTypeESCInformationVM travelerTypeESCInformationVM)
       {
           int clientDetailId = travelerTypeESCInformationVM.ClientDetail.ClientDetailId;

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

           string csu = travelerTypeESCInformationVM.ClientSubUnit.ClientSubUnitGuid;
           string tt = travelerTypeESCInformationVM.TravelerType.TravelerTypeGuid;

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
               TryUpdateModel<ClientDetailESCInformation>(travelerTypeESCInformationVM.ClientDetailESCInformation, "ClientDetailESCInformation");
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
               clientDetailESCInformationRepository.Edit(travelerTypeESCInformationVM.ClientDetailESCInformation);
           }
           catch (SqlException ex)
           {
               //Versioning Error
               if (ex.Message == "SQLVersioningError")
               {
                   ViewData["ReturnURL"] = "/TravelerTypeESCINformation.mvc/Edit?id=" + clientDetailId + "&csu=" + csu;
                   return View("VersionError");
               }
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
           ClientDetailESCInformation clientDetailESCInformation = new ClientDetailESCInformation();
           clientDetailESCInformation = clientDetailESCInformationRepository.GetClientDetailESCInformation(id);

           //Check Exists
           if (clientDetailESCInformation == null)
           {
               ViewData["ActionMethod"] = "DeleteGet";
               return View("RecordDoesNotExistError");
           }

          int clientDetailId = clientDetailESCInformation.ClientDetailId;
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

          TravelerTypeESCInformationVM travelerTypeESCInformationVM = new TravelerTypeESCInformationVM();

          ClientSubUnit clientSubUnit = new ClientSubUnit();
          clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
          travelerTypeESCInformationVM.ClientSubUnit = clientSubUnit;
          travelerTypeESCInformationVM.ClientDetailESCInformation = clientDetailESCInformation;

          ClientDetail clientDetail = new ClientDetail();
          clientDetail = clientDetailRepository.GetGroup(clientDetailId);
          travelerTypeESCInformationVM.ClientDetail = clientDetail;

          TravelerType travelerType = new TravelerType();
          travelerType = travelerTypeRepository.GetTravelerType(tt);
          travelerTypeESCInformationVM.TravelerType = travelerType;

          return View(travelerTypeESCInformationVM);
      }
        
        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(TravelerTypeESCInformationVM travelerTypeESCInformationVM, FormCollection collection)
    {
        int clientDetailId = travelerTypeESCInformationVM.ClientDetailESCInformation.ClientDetailId;
        ClientDetailESCInformation clientDetailESCInformation = new ClientDetailESCInformation();
        clientDetailESCInformation = clientDetailESCInformationRepository.GetClientDetailESCInformation(clientDetailId);

        if (clientDetailESCInformation == null)
        {
            ViewData["ActionMethod"] = "DeletePost";
            return View("RecordDoesNotExistError");
        }


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

        string csu = travelerTypeESCInformationVM.ClientSubUnit.ClientSubUnitGuid;
        string tt = travelerTypeESCInformationVM.TravelerType.TravelerTypeGuid;

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
            clientDetailESCInformation.VersionNumber = Int32.Parse(collection["ClientDetailESCInformation.VersionNumber"]);
            clientDetailESCInformationRepository.Delete(clientDetailESCInformation);
        }
        catch (SqlException ex)
        {
            //Versioning Error - go to standard versionError page
            if (ex.Message == "SQLVersioningError")
            {
                ViewData["ReturnURL"] = "/TravelerTypeAddress.mvc/Delete/id=" + clientDetailId.ToString() + "&csu=" + csu;
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
            ClientDetailESCInformation clientDetailESCInformation = new ClientDetailESCInformation();
            clientDetailESCInformation = clientDetailESCInformationRepository.GetClientDetailESCInformation(id);

            //Check Exists
            if (clientDetailESCInformation == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            int clientDetailId = clientDetailESCInformation.ClientDetailId;
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

            TravelerTypeESCInformationVM travelerTypeESCInformationVM = new TravelerTypeESCInformationVM();

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            travelerTypeESCInformationVM.ClientSubUnit = clientSubUnit;
            travelerTypeESCInformationVM.ClientDetailESCInformation = clientDetailESCInformation;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            travelerTypeESCInformationVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            travelerTypeESCInformationVM.TravelerType = travelerType;

            return View(travelerTypeESCInformationVM);
        }
    }
}
