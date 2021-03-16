using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class ClientAccountController : Controller
    {
        //main repository
        ClientAccountRepository clientAccountRepository = new ClientAccountRepository();

        // GET: /ListSubMenu
        public ActionResult ListSubMenu(string can, string ssc)
        {
            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Check Exists
            if (clientAccount == null)
            {
                ViewData["ActionMethod"] = "ListSubMenu";
                return View("RecordDoesNotExistError");
            }

            return View(clientAccount);
        }

        // GET: /View
        public ActionResult ViewItem(string can, string ssc)
        {
            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Check Exists
            if (clientAccount == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            clientAccountRepository.EditForDisplay(clientAccount);
            return View(clientAccount);
        }


        //REMOVED: CANNOT DELETE CLIENTACCOUNT
        // GET: /Delete
        /*public ActionResult Delete(string clientAccountNumber,  string sourceSystemCode, string clientSubUnitId)
        {
            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(clientAccountNumber, sourceSystemCode);

            //Check Exists
            if (clientAccount == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientAccount(clientAccountNumber, sourceSystemCode))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ViewData["ClientSubUnitGuid"] = clientSubUnitId;
            clientAccountRepository.EditForDisplay(clientAccount);
            return View(clientAccount);
        }*/

        //REMOVED: CANNOT DELETE CLIENTACCOUNT
        // POST: /Delete/5
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string clientAccountNumber, string sourceSystemCode, FormCollection collection)
        {
            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(clientAccountNumber, sourceSystemCode);

            //Check Exists
            if (clientAccount == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }
            //Check Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientAccount(clientAccountNumber, sourceSystemCode))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                clientAccount.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                clientAccountRepository.Delete(clientAccount);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ClientAccount.mvc/Delete?clientAccountNumber=" + clientAccount.ClientAccountNumber.ToString() + "&clientSubUnitId=" + collection["ClientSubUnitGuid"];
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List");
        }
        */

        //REMOVED: CANNOT EDIT CLIENTACCOUNT
        // GET: /Edit
        /*public ActionResult Edit(string can, string ssc)
        {
            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Check Exists
            if (clientAccount == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }
            //Check Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientAccount(can, ssc))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            clientAccountRepository.EditForDisplay(clientAccount);
            return View(clientAccount);
        }*/

        //REMOVED: CANNOT EDIT CLIENTACCOUNT
        // POST: /Edit/5
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string can, string ssc, FormCollection collection)
        {

            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Check Exists
            if (clientAccount == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }
            //Check Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientAccount(can, ssc))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Item from Form
            try
            {
                UpdateModel(clientAccount);
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
                clientAccountRepository.Update(clientAccount);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ClientAccount.mvc/Edit?can=" + clientAccount.ClientAccountNumber + "&ssc=" + clientAccount.SourceSystemCode;
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }

            //Success
            return RedirectToAction("ListBySubUnit", new { id = collection["ClientSubUnitGuid"] });

        }
        */

        //REMOVED: NOT HERE
        // GET: /Create - Add a ClientAccount to a sub unit
        /*public ActionResult Create(string id)
        {
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);

            //Check Exists
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "ListBySubUnitGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(id))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Show Create Form
            return View();
        }
         * */
    }
}

        