using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;
using CWTDesktopDatabase.ViewModels;
using System.Xml;

namespace CWTDesktopDatabase.Controllers
{
    public class ClientSubUnitClientAccountController : Controller
    {
        ClientSubUnitClientAccountRepository clientSubUnitClientAccountRepository = new ClientSubUnitClientAccountRepository();
        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
        ClientAccountRepository clientAccountRepository = new ClientAccountRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
		private string groupName = "Client Detail";
		
		// GET: /ListBySubUnit/
		public ActionResult ListBySubUnit(int? page, string id, string sortField, int? sortOrder)
		{
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);

			//Check Exists
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "ListBySubUnitGet";
				return View("RecordDoesNotExistError");
			}

			//Set Access Rights
			ViewData["Access"] = "";
			RolesRepository rolesRepository = new RolesRepository();
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);
			ViewData["ClientTopUnitGuid"] = clientSubUnit.ClientTopUnitGuid;
			ViewData["ClientTopUnitName"] = clientSubUnit.ClientTopUnitName;
			ViewData["ClientSubUnitGuid"] = clientSubUnit.ClientSubUnitGuid;
			ViewData["ClientSubUnitName"] = clientSubUnit.ClientSubUnitName;

			var items = clientSubUnitClientAccountRepository.PageClientAccounts(id, page ?? 1);
			return View(items);
		}

		// GET: /CreateAccount/
		public ActionResult CreateAccount(string id)
		{
			//Check Exists
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			}

			//Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientSubUnit(id))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			ClientWizardRepository clientWizardRepository = new ClientWizardRepository();
			List<spDDAWizard_SelectClientSubUnitClientAccounts_v1Result> clientSubUnitClientAccounts = new List<spDDAWizard_SelectClientSubUnitClientAccounts_v1Result>();
			clientSubUnitClientAccounts = clientWizardRepository.GetClientSubUnitClientAccounts(clientSubUnit.ClientSubUnitGuid);

			ClientSubUnitClientAccountsVM clientSubUnitClientAccountsVM = new ClientSubUnitClientAccountsVM();
			clientSubUnitClientAccountsVM.ClientAccounts = clientSubUnitClientAccounts;
			clientSubUnitClientAccountsVM.ClientSubUnit = clientSubUnit;

			return View(clientSubUnitClientAccountsVM);

		}

		// POST: /CreateAccount/
		[HttpPost]
		public JsonResult CreateAccount(string id, ClientWizardVM updatedClient)
		{
			//Check Exists
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return Json("RecordDoesNotExistError");
			}

			//Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientSubUnit(id))
			{
				ViewData["Message"] = "You do not have access to this item";
				return Json("Error");
			}

			// Create the xml document container
			XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
			doc.AppendChild(dec);
			XmlElement root = doc.CreateElement("ClientSubUnitClientAccounts");
			doc.AppendChild(root);

			bool changesExist = false;
			ClientAccountRepository clientAccountRepository = new ClientAccountRepository();

			if (updatedClient.ClientAccountsAdded != null)
			{
				if (updatedClient.ClientAccountsAdded.Count > 0)
				{
					changesExist = true;
					XmlElement xmlClientAccountsAdded = doc.CreateElement("ClientAccountsAdded");

					foreach (ClientSubUnitClientAccount item in updatedClient.ClientAccountsAdded)
					{
						ClientAccount clientAccount = new ClientAccount();
						clientAccount = clientAccountRepository.GetClientAccount(item.ClientAccountNumber, item.SourceSystemCode);

						XmlElement xmlClientAccount = doc.CreateElement("ClientAccount");
						xmlClientAccountsAdded.AppendChild(xmlClientAccount);

						XmlElement xmlSourceSystemCode = doc.CreateElement("SourceSystemCode");
						xmlSourceSystemCode.InnerText = item.SourceSystemCode;
						xmlClientAccount.AppendChild(xmlSourceSystemCode);

						XmlElement xmlClientAccountNumber = doc.CreateElement("ClientAccountNumber");
						xmlClientAccountNumber.InnerText = item.ClientAccountNumber;
						xmlClientAccount.AppendChild(xmlClientAccountNumber);

						XmlElement xmlClientAccountName = doc.CreateElement("ClientAccountName");
						xmlClientAccountName.InnerText = clientAccount.ClientAccountName;
						xmlClientAccount.AppendChild(xmlClientAccountName);

                        XmlElement xmlConfidenceLevelForLoadId = doc.CreateElement("ConfidenceLevelForLoadId");
                        xmlConfidenceLevelForLoadId.InnerText = item.ConfidenceLevelForLoadId.ToString();
                        xmlClientAccount.AppendChild(xmlConfidenceLevelForLoadId);

                    }
                    root.AppendChild(xmlClientAccountsAdded);
				}
			}
			if (updatedClient.ClientAccountsRemoved != null)
			{
				if (updatedClient.ClientAccountsRemoved.Count > 0)
				{
					changesExist = true;
					XmlElement xmlClientAccountsRemoved = doc.CreateElement("ClientAccountsRemoved");

					foreach (ClientSubUnitClientAccount item in updatedClient.ClientAccountsRemoved)
					{
						ClientAccount clientAccount = new ClientAccount();
						clientAccount = clientAccountRepository.GetClientAccount(item.ClientAccountNumber, item.SourceSystemCode);

						XmlElement xmlClientAccount = doc.CreateElement("ClientAccount");
						xmlClientAccountsRemoved.AppendChild(xmlClientAccount);

						XmlElement xmlSourceSystemCode = doc.CreateElement("SourceSystemCode");
						xmlSourceSystemCode.InnerText = item.SourceSystemCode;
						xmlClientAccount.AppendChild(xmlSourceSystemCode);

						XmlElement xmlClientAccountNumber = doc.CreateElement("ClientAccountNumber");
						xmlClientAccountNumber.InnerText = item.ClientAccountNumber;
						xmlClientAccount.AppendChild(xmlClientAccountNumber);

						XmlElement xmlClientAccountName = doc.CreateElement("ClientAccountName");
						xmlClientAccountName.InnerText = clientAccount.ClientAccountName;
						xmlClientAccount.AppendChild(xmlClientAccountName);

						XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
						xmlVersionNumber.InnerText = item.VersionNumber.ToString();
						xmlClientAccount.AppendChild(xmlVersionNumber);
					}
					root.AppendChild(xmlClientAccountsRemoved);
				}
			}

			string response = string.Empty;

			if (changesExist)
			{
				string adminUserGuid = User.Identity.Name.Split(new[] { '|' })[0];

				var output = (from n in db.spDDAWizard_UpdateClientSubUnitClientAccounts_v1(
					clientSubUnit.ClientSubUnitGuid,
					System.Xml.Linq.XElement.Parse(doc.OuterXml),
					adminUserGuid)
							  select n).ToList();

				foreach (spDDAWizard_UpdateClientSubUnitClientAccounts_v1Result message in output)
				{
					response += string.Format("{0} {0}", message.MessageText.ToString(), (bool)message.Success);
				}

			}
			return Json(new
             {
				 html = response,
                 message = "Success",
                 success = true
             });
		}

        // GET: /Add/
        public ActionResult Create(string id)
        {
            //Check Exists
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //LineOfBusiness
            LineOfBusinessRepository lineOfBusinessRepository = new LineOfBusinessRepository();
            SelectList lineOfBusinessList = new SelectList(lineOfBusinessRepository.GetAllLineOfBusinesses().ToList(), "LineOfBusinessId", "LineOfBusinessDescription");
            ViewData["LineOfBusinessses"] = lineOfBusinessList;


            ClientSubUnitClientAccount clientSubUnitClientAccount = new ClientSubUnitClientAccount();
            clientSubUnitClientAccount.ClientSubUnitGuid = id;

            ClientSubUnitClientAccountRepository clientSubUnitClientAccountRepository = new ClientSubUnitClientAccountRepository();
            clientSubUnitClientAccountRepository.EditForDisplay(clientSubUnitClientAccount);
            return View(clientSubUnitClientAccount);

        }

        // POST: /Create/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientSubUnitClientAccount clientSubUnitClientAccount)
        {
            //Check Exists
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitClientAccount.ClientSubUnitGuid);
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitClientAccount.ClientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            if (!rolesRepository.HasWriteAccessToClientAccount(clientSubUnitClientAccount.ClientAccountNumber, clientSubUnitClientAccount.SourceSystemCode))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            //Update  Model from Form
            try
            {
                UpdateModel(clientSubUnitClientAccount);
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
                clientSubUnitClientAccountRepository.Add(clientSubUnitClientAccount);
            }
            catch
            {
                //Validation Errors on Table
               ViewData["Message"] = "Validation Error";
                return View("Error");
		
            }


            return RedirectToAction("ListBySubUnit", new { id = clientSubUnitClientAccount.ClientSubUnitGuid });
        }

        // GET: /Delete/
		[HttpGet]
		public ActionResult Delete(string can, string ssc, string clientSubUnitId)
        {
            //Check Exists
            ClientSubUnitClientAccount clientSubUnitClientAccount = new ClientSubUnitClientAccount();
            clientSubUnitClientAccount = clientSubUnitClientAccountRepository.GetClientSubUnitClientAccount(can, ssc, clientSubUnitId);
            if (clientSubUnitClientAccount == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitClientAccount.ClientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            if (!rolesRepository.HasWriteAccessToClientAccount(clientSubUnitClientAccount.ClientAccountNumber, clientSubUnitClientAccount.SourceSystemCode))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            clientSubUnitClientAccountRepository.EditForDisplay(clientSubUnitClientAccount);
            return View(clientSubUnitClientAccount);

        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string can, string ssc, string clientSubUnitId, FormCollection collection)
        {
            //Check Exists
            ClientSubUnitClientAccount clientSubUnitClientAccount = new ClientSubUnitClientAccount();
            clientSubUnitClientAccount = clientSubUnitClientAccountRepository.GetClientSubUnitClientAccount(can, ssc, clientSubUnitId);
            if (clientSubUnitClientAccount == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitClientAccount.ClientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            if (!rolesRepository.HasWriteAccessToClientAccount(clientSubUnitClientAccount.ClientAccountNumber, clientSubUnitClientAccount.SourceSystemCode))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                clientSubUnitClientAccount.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                clientSubUnitClientAccountRepository.Delete(clientSubUnitClientAccount);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ClientSubUnitClientAccount.mvc/Delete?id=" + clientSubUnitClientAccount.ClientAccountNumber.ToString() + "&ssc=" + clientSubUnitClientAccount.SourceSystemCode +"&clientSubUnitGuid=" + clientSubUnitClientAccount.ClientSubUnitGuid;
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }

            //Return
            return RedirectToAction("ListBySubUnit", new { id = clientSubUnitClientAccount.ClientSubUnitGuid });
        }


        /* GET: /View
        public ActionResult View(int id, string clientSubUnitId)
        {
            //Check Exists
            ClientSubUnitClientAccount clientSubUnitClientAccount = new ClientSubUnitClientAccount();
            clientSubUnitClientAccount = clientSubUnitClientAccountRepository.GetClientSubUnitClientAccount(id, clientSubUnitId);
            if (clientSubUnitClientAccount == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            clientSubUnitClientAccountRepository.EditForDisplay(clientSubUnitClientAccount);
            return View(clientSubUnitClientAccount);

        }
        */

        // GET: /Edit/
        public ActionResult Edit(string can, string ssc, string csu)
        {
            //Check Exists
            ClientSubUnitClientAccount clientSubUnitClientAccount = new ClientSubUnitClientAccount();
            clientSubUnitClientAccount = clientSubUnitClientAccountRepository.GetClientSubUnitClientAccount(can, ssc, csu);
            if (clientSubUnitClientAccount == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }
            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitClientAccount.ClientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            if (!rolesRepository.HasWriteAccessToClientAccount(clientSubUnitClientAccount.ClientAccountNumber, clientSubUnitClientAccount.SourceSystemCode))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //LineOfBusiness
            LineOfBusinessRepository lineOfBusinessRepository = new LineOfBusinessRepository();
            SelectList lineOfBusinessList = new SelectList(lineOfBusinessRepository.GetAllLineOfBusinesses().ToList(), "LineOfBusinessId", "LineOfBusinessDescription");
            ViewData["LineOfBusinessses"] = lineOfBusinessList;

            clientSubUnitClientAccountRepository.EditForDisplay(clientSubUnitClientAccount);
            return View(clientSubUnitClientAccount);

        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string can, string ssc, string csu, FormCollection formCollection)
        {
            //Check Exists
            ClientSubUnitClientAccount clientSubUnitClientAccount = new ClientSubUnitClientAccount();
            clientSubUnitClientAccount = clientSubUnitClientAccountRepository.GetClientSubUnitClientAccount(can, ssc, csu);
            if (clientSubUnitClientAccount == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }
            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitClientAccount.ClientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            if (!rolesRepository.HasWriteAccessToClientAccount(clientSubUnitClientAccount.ClientAccountNumber, clientSubUnitClientAccount.SourceSystemCode))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Item from Form
            try
            {
                UpdateModel(clientSubUnitClientAccount);
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

            //Update Item
            try
            {
                clientSubUnitClientAccountRepository.Update(clientSubUnitClientAccount);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ClientSubUnitClientAccount.mvc/Edit?clientAccountNumber=" + can + "&ssc="+ ssc + "&csu=" + csu;
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }

            return RedirectToAction("ListBySubUnit", new { id = clientSubUnitClientAccount.ClientSubUnitGuid });
        }

    }
}
