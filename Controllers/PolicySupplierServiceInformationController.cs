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
    public class policySupplierServiceInformationController : Controller
    {
        //main repositories
        PolicySupplierServiceInformationRepository policySupplierServiceInformationRepository = new PolicySupplierServiceInformationRepository();
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();

        // GET: /ClientDetailAddress/
        public ActionResult List(string filter, int id, int? page, string sortField, int? sortOrder)
        {

            //Check Exists
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(id);
            if (policyGroup == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToPolicyGroup(id))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Parent Information
            ViewData["PolicyGroupID"] = id;
            ViewData["PolicyGroupName"] = policyGroupRepository.GetGroup(id).PolicyGroupName;

            if (sortField != "PolicySupplierServiceInformationTypeDescription")
            {
                sortField = "policySupplierServiceInformationValue";
            }

            if (sortOrder == 1)
            {
                ViewData["NewSortOrder"] = 0;
                ViewData["CurrentSortOrder"] = 1;
            }
            else
            {
                ViewData["NewSortOrder"] = 1;
                ViewData["CurrentSortOrder"] = 0;
                sortOrder = 0;
            }

            var items = policySupplierServiceInformationRepository.PagePolicySupplierServiceInformations(id, page ?? 1, sortField, sortOrder ?? 0);
            return View(items);
        }

        // GET: /Create
        public ActionResult Create(int id)
        {

            PolicyGroup policyGroup = policyGroupRepository.GetGroup(id);

            //Check Exists
            if (policyGroup == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(id))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            //Populate List of policySupplierServiceInformationTypeRepositorys
            PolicySupplierServiceInformationTypeRepository policySupplierServiceInformationTypeRepository = new PolicySupplierServiceInformationTypeRepository();
            SelectList policySupplierServiceInformations = new SelectList(policySupplierServiceInformationTypeRepository.GetAllPolicySupplierServiceInformationTypes().ToList(), "policySupplierServiceInformationTypeId", "policySupplierServiceInformationTypeDescription");
            ViewData["PolicySupplierServiceInformationList"] = policySupplierServiceInformations;

            //Populate List of Products
            ProductRepository productRepository = new ProductRepository();
            SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
            ViewData["ProductList"] = products;

            //populate new item with known PolicyGroup Information           
            PolicySupplierServiceInformation policySupplierServiceInformation = new PolicySupplierServiceInformation();
            policySupplierServiceInformation.PolicyGroupId = id;
            policySupplierServiceInformation.PolicyGroupName = policyGroup.PolicyGroupName;
            policySupplierServiceInformation.EnabledFlagNonNullable = true;

            //Show 'Create' Form
            return View(policySupplierServiceInformation);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicySupplierServiceInformation policySupplierServiceInformation)
        {

            PolicyGroup policyGroup = policyGroupRepository.GetGroup(policySupplierServiceInformation.PolicyGroupId);
            if (policyGroup == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policySupplierServiceInformation.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Model from Form
            try
            {
                UpdateModel(policySupplierServiceInformation);
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
                policySupplierServiceInformationRepository.Add(policySupplierServiceInformation);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            return RedirectToAction("List", new { id = policySupplierServiceInformation.PolicyGroupId });
        }

        // GET: /Edit
        public ActionResult Edit(int id)
        {
            //Get policySupplierServiceInformation
            PolicySupplierServiceInformation policySupplierServiceInformation = new PolicySupplierServiceInformation();
            policySupplierServiceInformation = policySupplierServiceInformationRepository.GetPolicySupplierServiceInformation(id);

            //Check Exists
            if (policySupplierServiceInformation == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policySupplierServiceInformation.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Populate List of policySupplierServiceInformationTypeRepositorys
            PolicySupplierServiceInformationTypeRepository policySupplierServiceInformationTypeRepository = new PolicySupplierServiceInformationTypeRepository();
            SelectList policySupplierServiceInformations = new SelectList(policySupplierServiceInformationTypeRepository.GetAllPolicySupplierServiceInformationTypes().ToList(), "policySupplierServiceInformationTypeId", "policySupplierServiceInformationTypeDescription");
            ViewData["policySupplierServiceInformationList"] = policySupplierServiceInformations;

            //Populate List of Products
            ProductRepository productRepository = new ProductRepository();
            SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
            ViewData["ProductList"] = products;

            //return edit form
            policySupplierServiceInformationRepository.EditItemForDisplay(policySupplierServiceInformation);
            return View(policySupplierServiceInformation);
        }

        // POST /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            PolicySupplierServiceInformation policySupplierServiceInformation = new PolicySupplierServiceInformation();
            policySupplierServiceInformation = policySupplierServiceInformationRepository.GetPolicySupplierServiceInformation(id);

            //Check Exists
            if (policySupplierServiceInformation == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policySupplierServiceInformation.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update policySupplierServiceInformation Model From Form
            try
            {
                UpdateModel(policySupplierServiceInformation);
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
                policySupplierServiceInformationRepository.Update(policySupplierServiceInformation);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicySupplierServiceInformation.mvc/Edit/" + policySupplierServiceInformation.PolicySupplierServiceInformationId;
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }

            //Return to Form
            return RedirectToAction("List", new { id = policySupplierServiceInformation.PolicyGroupId });

        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            //Get policySupplierServiceInformation
            PolicySupplierServiceInformation policySupplierServiceInformation = new PolicySupplierServiceInformation();
            policySupplierServiceInformation = policySupplierServiceInformationRepository.GetPolicySupplierServiceInformation(id);

            //Check Exists
            if (policySupplierServiceInformation == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policySupplierServiceInformation.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            //populate new PolicyCarVendorGroupItem with known PolicyGroup Information
            policySupplierServiceInformationRepository.EditItemForDisplay(policySupplierServiceInformation);

            //Show 'Create' Form
            return View(policySupplierServiceInformation);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            PolicySupplierServiceInformation policySupplierServiceInformation = new PolicySupplierServiceInformation();
            policySupplierServiceInformation = policySupplierServiceInformationRepository.GetPolicySupplierServiceInformation(id);

            //Check Exists
            if (policySupplierServiceInformation == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policySupplierServiceInformation.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                policySupplierServiceInformation.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                policySupplierServiceInformationRepository.Delete(policySupplierServiceInformation);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicySupplierServiceInformation.mvc/Delete/" + policySupplierServiceInformation.PolicySupplierServiceInformationTypeId  ;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            //Return
            return RedirectToAction("List", new { id = policySupplierServiceInformation.PolicyGroupId });
        }

        public ActionResult View(int id)
        {
            //Get policySupplierServiceInformation
            PolicySupplierServiceInformation policySupplierServiceInformation = new PolicySupplierServiceInformation();
            policySupplierServiceInformation = policySupplierServiceInformationRepository.GetPolicySupplierServiceInformation(id);

            //Check Exists
            if (policySupplierServiceInformation == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            policySupplierServiceInformationRepository.EditItemForDisplay(policySupplierServiceInformation);
            return View(policySupplierServiceInformation);
        }
    }
}
