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
    public class ClientSubUnitESCInformationController : Controller
    {
        ClientDetailRepository clientDetailRepository = new ClientDetailRepository();
        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
        ClientDetailClientSubUnitRepository clientDetailClientSubUnitRepository = new ClientDetailClientSubUnitRepository();
        ClientDetailESCInformationRepository clientDetailESCInformationRepository = new ClientDetailESCInformationRepository();

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
            ClientSubUnitESCInformationVM clientSubUnitESCInformationVM = new ClientSubUnitESCInformationVM();
            clientSubUnitESCInformationVM.ClientDetailESCInformation = clientDetailRepository.GetClientDetailESCInformation(id);
            clientSubUnitESCInformationVM.ClientSubUnit = clientSubUnit;
            clientSubUnitESCInformationVM.ClientDetail = clientDetail;

            return View(clientSubUnitESCInformationVM);
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

            ClientSubUnitESCInformationVM clientSubUnitESCInformationVM = new ClientSubUnitESCInformationVM();
            clientSubUnitESCInformationVM.ClientSubUnit = clientSubUnit;
            clientSubUnitESCInformationVM.ClientDetail = clientDetail;

            ClientDetailESCInformation clientDetailESCInformation = new ClientDetailESCInformation();
            clientSubUnitESCInformationVM.ClientDetailESCInformation = clientDetailESCInformation;

            return View(clientSubUnitESCInformationVM);
        }
       
        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientSubUnitESCInformationVM clientSubUnitESCInformationVM)
        {
            int clientDetailId = clientSubUnitESCInformationVM.ClientDetail.ClientDetailId;

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

            string ctu = clientDetailClientSubUnit.ClientSubUnitGuid;


            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(ctu);

            //Check Exists
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(ctu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            //Update  Model from Form
            try
            {
                TryUpdateModel<ClientDetailESCInformation>(clientSubUnitESCInformationVM.ClientDetailESCInformation, "ESCInformation");
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
                clientDetailESCInformationRepository.Add(clientSubUnitESCInformationVM.ClientDetail, clientSubUnitESCInformationVM.ClientDetailESCInformation);
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
           ClientDetailESCInformation clientDetailESCInformation = new ClientDetailESCInformation();
           clientDetailESCInformation = clientDetailESCInformationRepository.GetClientDetailESCInformation(id);

           //Check Exists
           if (clientDetailESCInformation == null)
           {
               ViewData["ActionMethod"] = "EditGet";
               return View("RecordDoesNotExistError");
           }

           int clientDetailId = clientDetailESCInformation.ClientDetailId;
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

           ClientSubUnitESCInformationVM clientSubUnitESCInformationVM = new ClientSubUnitESCInformationVM();
           clientSubUnitESCInformationVM.ClientSubUnit = clientSubUnit;
           clientSubUnitESCInformationVM.ClientDetailESCInformation = clientDetailESCInformation;

           ClientDetail clientDetail = new ClientDetail();
           clientDetail = clientDetailRepository.GetGroup(clientDetailId);
           clientSubUnitESCInformationVM.ClientDetail = clientDetail;

        
           return View(clientSubUnitESCInformationVM);
       }
       
       // POST: /Edit
       [HttpPost]
       [ValidateAntiForgeryToken]
       public ActionResult Edit(ClientSubUnitESCInformationVM clientSubUnitESCInformationVM)
       {
           int clientDetailId = clientSubUnitESCInformationVM.ClientDetailESCInformation.ClientDetailId;

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
               TryUpdateModel<ClientDetailESCInformation>(clientSubUnitESCInformationVM.ClientDetailESCInformation, "ClientDetailESCInformation");
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
               clientDetailESCInformationRepository.Edit(clientSubUnitESCInformationVM.ClientDetailESCInformation);
           }
           catch (SqlException ex)
           {
               //Versioning Error
               if (ex.Message == "SQLVersioningError")
               {
                   ViewData["ReturnURL"] = "/ClientSubUnitESCInformation.mvc/Edit?id=" + clientDetailId;
                   return View("VersionError");
               }
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
          ClientDetailESCInformation clientDetailESCInformation = new ClientDetailESCInformation();
          clientDetailESCInformation = clientDetailESCInformationRepository.GetClientDetailESCInformation(id);

          //Check Exists
          if (clientDetailESCInformation == null)
          {
              ViewData["ActionMethod"] = "DeleteGet";
              return View("RecordDoesNotExistError");
          }

          int clientDetailId = clientDetailESCInformation.ClientDetailId;
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

          ClientSubUnitESCInformationVM clientSubUnitESCInformationVM = new ClientSubUnitESCInformationVM();
          clientSubUnitESCInformationVM.ClientSubUnit = clientSubUnit;
          clientSubUnitESCInformationVM.ClientDetailESCInformation = clientDetailESCInformation;

          ClientDetail clientDetail = new ClientDetail();
          clientDetail = clientDetailRepository.GetGroup(clientDetailId);
          clientSubUnitESCInformationVM.ClientDetail = clientDetail;

          return View(clientSubUnitESCInformationVM);
      }
      
     // POST: /Delete
     [HttpPost]
     [ValidateAntiForgeryToken]
     public ActionResult Delete(int id, FormCollection collection)
     {
         ClientDetailESCInformation clientDetailESCInformation = new ClientDetailESCInformation();
         clientDetailESCInformation = clientDetailESCInformationRepository.GetClientDetailESCInformation(id);

         //Check Exists
         if (clientDetailESCInformation == null)
         {
             ViewData["ActionMethod"] = "DeletePost";
             return View("RecordDoesNotExistError");
         }

         int clientDetailId = clientDetailESCInformation.ClientDetailId;
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
                 ViewData["ReturnURL"] = "/ClientSubUnitESCInformation.mvc/Delete/" + id.ToString();
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
       ClientDetailESCInformation clientDetailESCInformation = new ClientDetailESCInformation();
       clientDetailESCInformation = clientDetailESCInformationRepository.GetClientDetailESCInformation(id);

       //Check Exists
       if (clientDetailESCInformation == null)
       {
           ViewData["ActionMethod"] = "ViewGet";
           return View("RecordDoesNotExistError");
       }

       int clientDetailId = clientDetailESCInformation.ClientDetailId;
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

       ClientSubUnitESCInformationVM clientSubUnitESCInformationVM = new ClientSubUnitESCInformationVM();
       clientSubUnitESCInformationVM.ClientSubUnit = clientSubUnit;

       ClientDetail clientDetail = new ClientDetail();
       clientDetail = clientDetailRepository.GetGroup(clientDetailId);
       clientSubUnitESCInformationVM.ClientDetail = clientDetail;

       clientSubUnitESCInformationVM.ClientDetailESCInformation = clientDetailESCInformation;

       return View(clientSubUnitESCInformationVM);
   }
    
    }
}
