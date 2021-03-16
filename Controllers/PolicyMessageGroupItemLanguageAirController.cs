using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Models;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class PolicyMessageGroupItemLanguageAirController : Controller
    {
        //main repositories
        PolicyMessageGroupItemLanguageRepository policyMessageGroupItemLanguageRepository = new PolicyMessageGroupItemLanguageRepository();
        PolicyMessageGroupItemRepository policyMessageGroupItemRepository = new PolicyMessageGroupItemRepository();
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
        PolicyRoutingRepository policyRoutingRepository = new PolicyRoutingRepository();
        ProductRepository productRepository = new ProductRepository();
        PolicyMessageGroupItemAirRepository policyMessageGroupItemAirRepository = new PolicyMessageGroupItemAirRepository();


        // GET: /Create
        public ActionResult Create(int id)
        {
            PolicyMessageGroupItem policyMessageGroupItem = new PolicyMessageGroupItem();
            policyMessageGroupItem = policyMessageGroupItemRepository.GetPolicyMessageGroupItem(id);

            //Check Exists
            if (policyMessageGroupItem == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroupMessages(policyMessageGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            PolicyMessageGroupItemAirLanguageVM policyMessageGroupItemLanguageVM = new PolicyMessageGroupItemAirLanguageVM();
            policyMessageGroupItemLanguageVM.PolicyMessageGroupItemName = policyMessageGroupItem.PolicyMessageGroupItemName == null ? "[NONE]" : policyMessageGroupItem.PolicyMessageGroupItemName;
            policyMessageGroupItemLanguageVM.PolicyMessageGroupItemId = policyMessageGroupItem.PolicyMessageGroupItemId;

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyMessageGroupItem.PolicyGroupId);
            policyMessageGroupItemLanguageVM.PolicyGroupId = policyGroup.PolicyGroupId;
            policyMessageGroupItemLanguageVM.PolicyGroupName = policyGroup.PolicyGroupName;

            policyMessageGroupItemLanguageVM.PolicyRoutingName = policyMessageGroupItem.PolicyRouting.Name;

            Product product = new Product();
            product = productRepository.GetProduct((int)policyMessageGroupItem.ProductId);
            policyMessageGroupItemLanguageVM.ProductName = product.ProductName;


            PolicyMessageGroupItemLanguage policyMessageGroupItemLanguage = new PolicyMessageGroupItemLanguage();
            policyMessageGroupItemLanguage.PolicyMessageGroupItemId = id;
            policyMessageGroupItemLanguageVM.PolicyMessageGroupItemLanguage = policyMessageGroupItemLanguage;

            SelectList policyMessageGroupItemLanguages = new SelectList(policyMessageGroupItemLanguageRepository.GetUnUsedLanguages(policyMessageGroupItem.PolicyMessageGroupItemId).ToList(), "LanguageCode", "LanguageName");
            policyMessageGroupItemLanguageVM.PolicyMessageGroupItemLanguages = policyMessageGroupItemLanguages;

            return View(policyMessageGroupItemLanguageVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicyMessageGroupItemAirLanguageVM policyMessageGroupItemAirLanguageVM)
        {

            //Get PolicyMessageGroupItem
            int policyMessageGroupItemId = policyMessageGroupItemAirLanguageVM.PolicyMessageGroupItemLanguage.PolicyMessageGroupItemId;
            PolicyMessageGroupItemAir policyMessageGroupItemAir = new PolicyMessageGroupItemAir();
            policyMessageGroupItemAir = policyMessageGroupItemAirRepository.GetPolicyMessageGroupItemAir(policyMessageGroupItemId);

            //Check Exists
            if (policyMessageGroupItemAir == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroupMessages(policyMessageGroupItemAir.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            PolicyMessageGroupItemLanguage policyMessageGroupItemLanguage = new PolicyMessageGroupItemLanguage();
            policyMessageGroupItemLanguage = policyMessageGroupItemAirLanguageVM.PolicyMessageGroupItemLanguage;

            //Update  Model from Form
            try
            {
                UpdateModel<PolicyMessageGroupItemLanguage>(policyMessageGroupItemLanguage, "PolicyMessageGroupItemLanguage");
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
                var md = new MarkdownDeep.Markdown();
                md.SafeMode = true;
                md.ExtraMode = true;
                policyMessageGroupItemLanguage.PolicyMessageGroupItemTranslation = md.Transform(policyMessageGroupItemLanguage.PolicyMessageGroupItemTranslationMarkdown);


                policyMessageGroupItemLanguageRepository.Add(policyMessageGroupItemLanguage);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", "PolicyMessageGroupItemLanguage", new { id = policyMessageGroupItemAir.PolicyMessageGroupItemId });
        }


        // GET: /Edit
        public ActionResult Edit(int id, string languageCode)
        {
            //Get Item 
            PolicyMessageGroupItemLanguage policyMessageGroupItemLanguage = new PolicyMessageGroupItemLanguage();
            policyMessageGroupItemLanguage = policyMessageGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyMessageGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyMessageGroupItem policyMessageGroupItem = new PolicyMessageGroupItem();
            policyMessageGroupItem = policyMessageGroupItemRepository.GetPolicyMessageGroupItem(id);


            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroupMessages(policyMessageGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            PolicyMessageGroupItemAirLanguageVM policyMessageGroupItemLanguageVM = new PolicyMessageGroupItemAirLanguageVM();
            policyMessageGroupItemLanguageVM.PolicyMessageGroupItemName = policyMessageGroupItem.PolicyMessageGroupItemName == null ? "[NONE]" : policyMessageGroupItem.PolicyMessageGroupItemName;
            policyMessageGroupItemLanguageVM.PolicyMessageGroupItemId = policyMessageGroupItem.PolicyMessageGroupItemId;
            policyMessageGroupItemLanguageVM.PolicyMessageGroupItemLanguage = policyMessageGroupItemLanguage;
            policyMessageGroupItemLanguageVM.PolicyRoutingName = policyMessageGroupItem.PolicyRouting.Name;

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyMessageGroupItem.PolicyGroupId);
            policyMessageGroupItemLanguageVM.PolicyGroupId = policyGroup.PolicyGroupId;
            policyMessageGroupItemLanguageVM.PolicyGroupName = policyGroup.PolicyGroupName;


            Product product = new Product();
            product = productRepository.GetProduct((int)policyMessageGroupItem.ProductId);
            policyMessageGroupItemLanguageVM.ProductName = product.ProductName;


            return View(policyMessageGroupItemLanguageVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PolicyMessageGroupItemAirLanguageVM policyMessageGroupItemAirLanguageVM)
        {

            //Get PolicyMessageGroupItem
            int policyMessageGroupItemId = policyMessageGroupItemAirLanguageVM.PolicyMessageGroupItemLanguage.PolicyMessageGroupItemId;
            PolicyMessageGroupItemAir policyMessageGroupItemAir = new PolicyMessageGroupItemAir();
            policyMessageGroupItemAir = policyMessageGroupItemAirRepository.GetPolicyMessageGroupItemAir(policyMessageGroupItemId);

            //Check Exists
            if (policyMessageGroupItemAir == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroupMessages(policyMessageGroupItemAir.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            PolicyMessageGroupItemLanguage policyMessageGroupItemLanguage = new PolicyMessageGroupItemLanguage();
            policyMessageGroupItemLanguage = policyMessageGroupItemAirLanguageVM.PolicyMessageGroupItemLanguage;

            //Update  Model from Form
            try
            {
                UpdateModel<PolicyMessageGroupItemLanguage>(policyMessageGroupItemLanguage, "PolicyMessageGroupItemLanguage");
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
                var md = new MarkdownDeep.Markdown();
                md.SafeMode = true;
                md.ExtraMode = true;
                policyMessageGroupItemLanguage.PolicyMessageGroupItemTranslation = md.Transform(policyMessageGroupItemLanguage.PolicyMessageGroupItemTranslationMarkdown);


                policyMessageGroupItemLanguageRepository.Update(policyMessageGroupItemLanguage);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", "PolicyMessageGroupItemLanguage", new { id = policyMessageGroupItemAir.PolicyMessageGroupItemId });
        }

        // GET: /View
        public ActionResult View(int id, string languageCode)
        {
            //Get Item 
            PolicyMessageGroupItemLanguage policyMessageGroupItemLanguage = new PolicyMessageGroupItemLanguage();
            policyMessageGroupItemLanguage = policyMessageGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyMessageGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            PolicyMessageGroupItem policyMessageGroupItem = new PolicyMessageGroupItem();
            policyMessageGroupItem = policyMessageGroupItemRepository.GetPolicyMessageGroupItem(id);

            PolicyMessageGroupItemAirLanguageVM policyMessageGroupItemLanguageVM = new PolicyMessageGroupItemAirLanguageVM();
            policyMessageGroupItemLanguageVM.PolicyMessageGroupItemName = policyMessageGroupItem.PolicyMessageGroupItemName == null ? "[NONE]" : policyMessageGroupItem.PolicyMessageGroupItemName;
            policyMessageGroupItemLanguageVM.PolicyMessageGroupItemId = policyMessageGroupItem.PolicyMessageGroupItemId;
            policyMessageGroupItemLanguageVM.PolicyMessageGroupItemLanguage = policyMessageGroupItemLanguage;
            policyMessageGroupItemLanguageVM.PolicyRoutingName = policyMessageGroupItem.PolicyRouting.Name;

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyMessageGroupItem.PolicyGroupId);
            policyMessageGroupItemLanguageVM.PolicyGroupId = policyGroup.PolicyGroupId;
            policyMessageGroupItemLanguageVM.PolicyGroupName = policyGroup.PolicyGroupName;

            Product product = new Product();
            product = productRepository.GetProduct((int)policyMessageGroupItem.ProductId);
            policyMessageGroupItemLanguageVM.ProductName = product.ProductName;

            //PolicyLocation policyLocation = new PolicyLocation();
            //policyLocation = policyRoutingRepository.GetPolicyLocation((int)policyMessageGroupItem.PolicyLocationId);
            //policyMessageGroupItemLanguageVM.PolicyLocationName = policyLocation.PolicyLocationName;

            return View(policyMessageGroupItemLanguageVM);
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, string languageCode)
        {
            //Get Item 
            PolicyMessageGroupItemLanguage policyMessageGroupItemLanguage = new PolicyMessageGroupItemLanguage();
            policyMessageGroupItemLanguage = policyMessageGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyMessageGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            PolicyMessageGroupItem policyMessageGroupItem = new PolicyMessageGroupItem();
            policyMessageGroupItem = policyMessageGroupItemRepository.GetPolicyMessageGroupItem(id);

            PolicyMessageGroupItemAirLanguageVM policyMessageGroupItemLanguageVM = new PolicyMessageGroupItemAirLanguageVM();
            policyMessageGroupItemLanguageVM.PolicyMessageGroupItemName = policyMessageGroupItem.PolicyMessageGroupItemName == null ? "[NONE]" : policyMessageGroupItem.PolicyMessageGroupItemName;
            policyMessageGroupItemLanguageVM.PolicyMessageGroupItemId = policyMessageGroupItem.PolicyMessageGroupItemId;
            policyMessageGroupItemLanguageVM.PolicyRoutingName = policyMessageGroupItem.PolicyRouting.Name;
            policyMessageGroupItemLanguageVM.PolicyMessageGroupItemLanguage = policyMessageGroupItemLanguage;

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyMessageGroupItem.PolicyGroupId);
            policyMessageGroupItemLanguageVM.PolicyGroupId = policyGroup.PolicyGroupId;
            policyMessageGroupItemLanguageVM.PolicyGroupName = policyGroup.PolicyGroupName;

            Product product = new Product();
            product = productRepository.GetProduct((int)policyMessageGroupItem.ProductId);
            policyMessageGroupItemLanguageVM.ProductName = product.ProductName;

            //PolicyLocation policyLocation = new PolicyLocation();
            //policyLocation = policyRoutingRepository.GetPolicyLocation((int)policyMessageGroupItem.PolicyLocationId);
            //policyMessageGroupItemLanguageVM.PolicyLocationName = policyLocation.PolicyLocationName;

            return View(policyMessageGroupItemLanguageVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(PolicyMessageGroupItemAirLanguageVM policyMessageGroupItemAirLanguageVM)
        {

            //Get PolicyMessageGroupItem
            int policyMessageGroupItemId = policyMessageGroupItemAirLanguageVM.PolicyMessageGroupItemLanguage.PolicyMessageGroupItemId;
            PolicyMessageGroupItemAir policyMessageGroupItemAir = new PolicyMessageGroupItemAir();
            policyMessageGroupItemAir = policyMessageGroupItemAirRepository.GetPolicyMessageGroupItemAir(policyMessageGroupItemId);

            //Check Exists
            if (policyMessageGroupItemAir == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroupMessages(policyMessageGroupItemAir.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            PolicyMessageGroupItemLanguage policyMessageGroupItemLanguage = new PolicyMessageGroupItemLanguage();
            policyMessageGroupItemLanguage = policyMessageGroupItemAirLanguageVM.PolicyMessageGroupItemLanguage;

            try
            {
                policyMessageGroupItemLanguageRepository.Delete(policyMessageGroupItemLanguage);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", "PolicyMessageGroupItemLanguage", new { id = policyMessageGroupItemAir.PolicyMessageGroupItemId });
        }
    }
}
