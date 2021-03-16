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
    public class ClientAccountESCInformationController : Controller
    {
        ClientDetailRepository clientDetailRepository = new ClientDetailRepository();
        ClientAccountRepository clientAccountRepository = new ClientAccountRepository();
        ClientDetailClientAccountRepository clientDetailClientAccountRepository = new ClientDetailClientAccountRepository();
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
            if (rolesRepository.HasWriteAccessToClientAccount(can, ssc))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Populate View Model
            ClientAccountESCInformationVM clientAccountESCInformationVM = new ClientAccountESCInformationVM();
            clientAccountESCInformationVM.ClientDetailESCInformation = clientDetailRepository.GetClientDetailESCInformation(id);
            clientAccountESCInformationVM.ClientAccount = clientAccount;
            clientAccountESCInformationVM.ClientDetail = clientDetail;

            return View(clientAccountESCInformationVM);
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

            ClientAccountESCInformationVM clientAccountESCInformationVM = new ClientAccountESCInformationVM();
            clientAccountESCInformationVM.ClientAccount = clientAccount;
            clientAccountESCInformationVM.ClientDetail = clientDetail;

            ClientDetailESCInformation clientDetailESCInformation = new ClientDetailESCInformation();
            clientAccountESCInformationVM.ClientDetailESCInformation = clientDetailESCInformation;

            return View(clientAccountESCInformationVM);
        }
       
        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientAccountESCInformationVM clientAccountESCInformationVM)
        {
            int clientDetailId = clientAccountESCInformationVM.ClientDetail.ClientDetailId;

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
                TryUpdateModel<ClientDetailESCInformation>(clientAccountESCInformationVM.ClientDetailESCInformation, "ESCInformation");
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
                clientDetailESCInformationRepository.Add(clientAccountESCInformationVM.ClientDetail, clientAccountESCInformationVM.ClientDetailESCInformation);
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
           ClientDetailClientAccount clientDetailClientAccount = new ClientDetailClientAccount();
           clientDetailClientAccount = clientDetailClientAccountRepository.GetClientDetailClientAccount(id);

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

           //Check Exists
           if (clientAccount == null)
           {
               ViewData["ActionMethod"] = "EditGet";
               return View("RecordDoesNotExistError");
           }

           //Access Rights
           RolesRepository rolesRepository = new RolesRepository();
           if (!rolesRepository.HasWriteAccessToClientAccount(can, ssc))
           {
               ViewData["Message"] = "You do not have access to this item";
               return View("Error");
           }

           ClientAccountESCInformationVM clientAccountESCInformationVM = new ClientAccountESCInformationVM();
           clientAccountESCInformationVM.ClientAccount = clientAccount;
           clientAccountESCInformationVM.ClientDetailESCInformation = clientDetailESCInformation;

           ClientDetail clientDetail = new ClientDetail();
           clientDetail = clientDetailRepository.GetGroup(clientDetailId);
           clientAccountESCInformationVM.ClientDetail = clientDetail;

        
           return View(clientAccountESCInformationVM);
       }
       
       // POST: /Edit
       [HttpPost]
       [ValidateAntiForgeryToken]
       public ActionResult Edit(ClientAccountESCInformationVM clientAccountESCInformationVM)
       {
           int clientDetailId = clientAccountESCInformationVM.ClientDetailESCInformation.ClientDetailId;

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
               ViewData["ActionMethod"] = "DeleteGet";
               return View("RecordDoesNotExistError");
           }

           //Access Rights
           RolesRepository rolesRepository = new RolesRepository();
           if (!rolesRepository.HasWriteAccessToClientAccount(can,ssc))
           {
               ViewData["Message"] = "You do not have access to this item";
               return View("Error");
           }


           //Update  Model from Form
           try
           {
               TryUpdateModel<ClientDetailESCInformation>(clientAccountESCInformationVM.ClientDetailESCInformation, "ClientDetailESCInformation");
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
               clientDetailESCInformationRepository.Edit(clientAccountESCInformationVM.ClientDetailESCInformation);
           }
           catch (SqlException ex)
           {
               //Versioning Error
               if (ex.Message == "SQLVersioningError")
               {
                   ViewData["ReturnURL"] = "/ClientAccountESCInformation.mvc/Edit?id=" + clientDetailId;
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
          ClientDetailClientAccount clientDetailClientAccount = new ClientDetailClientAccount();
          clientDetailClientAccount = clientDetailClientAccountRepository.GetClientDetailClientAccount(id);

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

          //Check Exists
          if (clientAccount == null)
          {
              ViewData["ActionMethod"] = "DeleteGet";
              return View("RecordDoesNotExistError");
          }

          //Access Rights
          RolesRepository rolesRepository = new RolesRepository();
          if (!rolesRepository.HasWriteAccessToClientAccount(can, ssc))
          {
              ViewData["Message"] = "You do not have access to this item";
              return View("Error");
          }

          ClientAccountESCInformationVM clientAccountESCInformationVM = new ClientAccountESCInformationVM();
          clientAccountESCInformationVM.ClientAccount = clientAccount;
          clientAccountESCInformationVM.ClientDetailESCInformation = clientDetailESCInformation;

          ClientDetail clientDetail = new ClientDetail();
          clientDetail = clientDetailRepository.GetGroup(clientDetailId);
          clientAccountESCInformationVM.ClientDetail = clientDetail;

          return View(clientAccountESCInformationVM);
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
         ClientDetailClientAccount clientDetailClientAccount = new ClientDetailClientAccount();
         clientDetailClientAccount = clientDetailClientAccountRepository.GetClientDetailClientAccount(id);

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
                 ViewData["ReturnURL"] = "/ClientAccountESCInformation.mvc/Delete/" + id.ToString();
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
       ClientDetailClientAccount clientDetailClientAccount = new ClientDetailClientAccount();
       clientDetailClientAccount = clientDetailClientAccountRepository.GetClientDetailClientAccount(id);

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

       //Check Exists
       if (clientAccount == null)
       {
           ViewData["ActionMethod"] = "ViewGet";
           return View("RecordDoesNotExistError");
       }

       ClientAccountESCInformationVM clientAccountESCInformationVM = new ClientAccountESCInformationVM();
       clientAccountESCInformationVM.ClientAccount = clientAccount;

       ClientDetail clientDetail = new ClientDetail();
       clientDetail = clientDetailRepository.GetGroup(clientDetailId);
       clientAccountESCInformationVM.ClientDetail = clientDetail;

       clientAccountESCInformationVM.ClientDetailESCInformation = clientDetailESCInformation;

       return View(clientAccountESCInformationVM);
   }
    
    }
}
