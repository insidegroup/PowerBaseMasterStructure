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
    public class ClientTopUnitESCInformationController : Controller
    {
        ClientDetailRepository clientDetailRepository = new ClientDetailRepository();
        ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
        ClientDetailClientTopUnitRepository clientDetailClientTopUnitRepository = new ClientDetailClientTopUnitRepository();
        ClientDetailESCInformationRepository clientDetailESCInformationRepository = new ClientDetailESCInformationRepository();

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

            ClientDetailClientTopUnit clientDetailClientTopUnit = new ClientDetailClientTopUnit();
            clientDetailClientTopUnit = clientDetailClientTopUnitRepository.GetClientDetailClientTopUnit(id);

            //Check Exists
            if (clientDetailClientTopUnit == null)
            {
                ViewData["ActionMethod"] = "List";
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
            ClientTopUnitESCInformationVM clientTopUnitESCInformationVM = new ClientTopUnitESCInformationVM();
            clientTopUnitESCInformationVM.ClientDetailESCInformation = clientDetailRepository.GetClientDetailESCInformation(id);
            clientTopUnitESCInformationVM.ClientTopUnit = clientTopUnit;
            clientTopUnitESCInformationVM.ClientDetail = clientDetail;

            return View(clientTopUnitESCInformationVM);
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

            ClientTopUnitESCInformationVM clientTopUnitESCInformationVM = new ClientTopUnitESCInformationVM();
            clientTopUnitESCInformationVM.ClientTopUnit = clientTopUnit;
            clientTopUnitESCInformationVM.ClientDetail = clientDetail;

            ClientDetailESCInformation clientDetailESCInformation = new ClientDetailESCInformation();
            clientTopUnitESCInformationVM.ClientDetailESCInformation = clientDetailESCInformation;

            return View(clientTopUnitESCInformationVM);
        }
       
        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientTopUnitESCInformationVM clientTopUnitESCInformationVM)
        {
            int clientDetailId = clientTopUnitESCInformationVM.ClientDetail.ClientDetailId;

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

            string ctu = clientDetailClientTopUnit.ClientTopUnitGuid;


            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(ctu);

            //Check Exists
            if (clientTopUnit == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientTopUnit(ctu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            //Update  Model from Form
            try
            {
                TryUpdateModel<ClientDetailESCInformation>(clientTopUnitESCInformationVM.ClientDetailESCInformation, "ESCInformation");
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
                clientDetailESCInformationRepository.Add(clientTopUnitESCInformationVM.ClientDetail, clientTopUnitESCInformationVM.ClientDetailESCInformation);
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

           ClientTopUnitESCInformationVM clientTopUnitESCInformationVM = new ClientTopUnitESCInformationVM();
           clientTopUnitESCInformationVM.ClientTopUnit = clientTopUnit;
           clientTopUnitESCInformationVM.ClientDetailESCInformation = clientDetailESCInformation;

           ClientDetail clientDetail = new ClientDetail();
           clientDetail = clientDetailRepository.GetGroup(clientDetailId);
           clientTopUnitESCInformationVM.ClientDetail = clientDetail;

        
           return View(clientTopUnitESCInformationVM);
       }
       
       // POST: /Edit
       [HttpPost]
       [ValidateAntiForgeryToken]
       public ActionResult Edit(ClientTopUnitESCInformationVM clientTopUnitESCInformationVM)
       {
           int clientDetailId = clientTopUnitESCInformationVM.ClientDetailESCInformation.ClientDetailId;

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
               TryUpdateModel<ClientDetailESCInformation>(clientTopUnitESCInformationVM.ClientDetailESCInformation, "ClientDetailESCInformation");
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
               clientDetailESCInformationRepository.Edit(clientTopUnitESCInformationVM.ClientDetailESCInformation);
           }
           catch (SqlException ex)
           {
               //Versioning Error
               if (ex.Message == "SQLVersioningError")
               {
                   ViewData["ReturnURL"] = "/ClientTopUnitESCInformation.mvc/Edit?id=" + clientDetailId;
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

          ClientTopUnitESCInformationVM clientTopUnitESCInformationVM = new ClientTopUnitESCInformationVM();
          clientTopUnitESCInformationVM.ClientTopUnit = clientTopUnit;
          clientTopUnitESCInformationVM.ClientDetailESCInformation = clientDetailESCInformation;

          ClientDetail clientDetail = new ClientDetail();
          clientDetail = clientDetailRepository.GetGroup(clientDetailId);
          clientTopUnitESCInformationVM.ClientDetail = clientDetail;

          return View(clientTopUnitESCInformationVM);
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
                 ViewData["ReturnURL"] = "/ClientTopUnitESCInformation.mvc/Delete/" + id.ToString();
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

       ClientTopUnitESCInformationVM clientTopUnitESCInformationVM = new ClientTopUnitESCInformationVM();
       clientTopUnitESCInformationVM.ClientTopUnit = clientTopUnit;

       ClientDetail clientDetail = new ClientDetail();
       clientDetail = clientDetailRepository.GetGroup(clientDetailId);
       clientTopUnitESCInformationVM.ClientDetail = clientDetail;

       clientTopUnitESCInformationVM.ClientDetailESCInformation = clientDetailESCInformation;

       return View(clientTopUnitESCInformationVM);
   }
    
    }
}
