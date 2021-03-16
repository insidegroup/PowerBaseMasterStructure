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
    public class ConfigurationParameterController : Controller
    {
        //main repository
        ConfigurationParameterRepository configurationParameterRepository = new ConfigurationParameterRepository();

        // GET: /List/
        public ActionResult List(string filter, int? page, string sortField, int? sortOrder)
        {
            
            //SortField + SortOrder settings
            if (sortField != "ContextName" && sortField != "SystemUserLoginIdentifier" && sortField != "LastUpdateTimestamp" && sortField != "ConfigurationParameterValue")
            {
                sortField = "ConfigurationParameterName";
            }
            ViewData["CurrentSortField"] = sortField;

            if (sortOrder == 1)
            {
                ViewData["NewSortOrder"] = 0;
                ViewData["CurrentSortOrder"] = 1;
            }
            else
            {
                ViewData["NewSortOrder"] = 1;
                ViewData["CurrentSortOrder"] = 0;
            }

            ConfigurationParametersVM configurationParametersVM = new ConfigurationParametersVM();
            configurationParametersVM.ConfigurationParameters = configurationParameterRepository.PageConfigurationParameters(page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

            //Set Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            configurationParametersVM.HasWriteAccess = rolesRepository.HasWriteAccessToConfigurationParameters(); 
            
            return View(configurationParametersVM);
        }

        // GET: /View
        public ActionResult ViewItem(string id)
        {
            //Check Exists
            ConfigurationParameter configurationParameter = new ConfigurationParameter();
            configurationParameter = configurationParameterRepository.GetConfigurationParameter(id);
            if (configurationParameter == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            ConfigurationParameterVM configurationParameterVM = new ConfigurationParameterVM();
            configurationParameterVM.ConfigurationParameter = configurationParameter;
            return View(configurationParameterVM);
        }

        // GET: /Edit
        public ActionResult Edit(string id)
        {
            //Check Exists
            ConfigurationParameter configurationParameter = new ConfigurationParameter();
            configurationParameter = configurationParameterRepository.GetConfigurationParameter(id);
            if (configurationParameter == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToConfigurationParameters())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ConfigurationParameterVM configurationParameterVM = new ConfigurationParameterVM();
            configurationParameterVM.ConfigurationParameter = configurationParameter;
            return View(configurationParameterVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ConfigurationParameterVM configurationParameterVM)
        {
            //Get Item
            ConfigurationParameter configurationParameter = new ConfigurationParameter();
            configurationParameter = configurationParameterRepository.GetConfigurationParameter(configurationParameterVM.ConfigurationParameter.ConfigurationParameterName);

            //Check Exists
            if (configurationParameter == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToConfigurationParameters())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            try
            {
                UpdateModel<ConfigurationParameter>(configurationParameterVM.ConfigurationParameter, "ConfigurationParameter");
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
                configurationParameterRepository.Edit(configurationParameterVM.ConfigurationParameter);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/QueueMinderItem.mvc/Edit/?id=" + configurationParameter.ConfigurationParameterName;
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List");

        }
    }
}
