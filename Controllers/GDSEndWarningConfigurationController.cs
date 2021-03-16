using ClientProfileServiceBusiness;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;

namespace CWTDesktopDatabase.Controllers
{
	public class GDSEndWarningConfigurationController : Controller
	{
		// Main repository
		GDSEndWarningConfigurationRepository gdsEndWarningConfigurationRepository = new GDSEndWarningConfigurationRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		RolesRepository rolesRepository = new RolesRepository();

		private string groupName = "GDS Response Message Administrator ";

		//
		// GET: /GDSEndWarningConfiguration/List

		public ActionResult List(string filter, int? page, string sortField, int? sortOrder)
		{
			//SortField
			if (sortField == string.Empty)
			{
				sortField = "GDSResponseId";
			}
			ViewData["CurrentSortField"] = sortField;

			//SortOrder
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

			GDSEndWarningConfigurationsVM gdsEndWarningConfigurationsVM = new GDSEndWarningConfigurationsVM();

			//Set Access Rights
			if (rolesRepository.HasWriteAccessToGDSEndWarningConfiguration())
			{
				gdsEndWarningConfigurationsVM.HasWriteAccess = true;
			}

			if (gdsEndWarningConfigurationRepository != null)
			{
				var gdsEndWarningConfigurations = gdsEndWarningConfigurationRepository.PageGDSEndWarningConfigurations(page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

				if (gdsEndWarningConfigurations != null)
				{
					gdsEndWarningConfigurationsVM.GDSEndWarningConfigurations = gdsEndWarningConfigurations;
				}
			}

			//return items
			return View(gdsEndWarningConfigurationsVM);
		}

		//
		// GET: /GDSEndWarningConfiguration/Create

		public ActionResult Create()
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			GDSEndWarningConfigurationVM gdsEndWarningConfigurationVM = new GDSEndWarningConfigurationVM();

			//GDS List
			GDSRepository gDSRepository = new GDSRepository();
			SelectList gDSs = new SelectList(gDSRepository.GetClientProfileBuilderGDSs().ToList(), "GDSCode", "GDSName");
			gdsEndWarningConfigurationVM.GDSs = gDSs;

			//GDSEndWarningBehaviorTypes
			GDSEndWarningBehaviorTypeRepository gdsEndWarningBehaviorTypeRepository = new GDSEndWarningBehaviorTypeRepository();
			SelectList gdsEndWarningBehaviorTypes = new SelectList(gdsEndWarningBehaviorTypeRepository.GetAllGDSEndWarningBehaviorTypes().ToList(), "GDSEndWarningBehaviorTypeId", "GDSEndWarningBehaviorTypeDescription");
			gdsEndWarningConfigurationVM.GDSEndWarningBehaviorTypes = gdsEndWarningBehaviorTypes;

			//Automated Commands
			List<AutomatedCommand> automatedCommands = new List<AutomatedCommand>();
			AutomatedCommand automatedCommand = new AutomatedCommand();
			automatedCommands.Add(automatedCommand);
			gdsEndWarningConfigurationVM.AutomatedCommands = automatedCommands;

			GDSEndWarningConfiguration gdsEndWarningConfiguration = new GDSEndWarningConfiguration();
			gdsEndWarningConfigurationVM.GDSEndWarningConfiguration = gdsEndWarningConfiguration;

			return View(gdsEndWarningConfigurationVM);
		}

		// POST: /GDSEndWarningConfiguration/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(GDSEndWarningConfigurationVM GDSEndWarningConfigurationVM, FormCollection formCollection)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//We need to extract group from groupVM
			GDSEndWarningConfiguration gdsEndWarningConfiguration = new GDSEndWarningConfiguration();
			gdsEndWarningConfiguration = GDSEndWarningConfigurationVM.GDSEndWarningConfiguration;
			if (gdsEndWarningConfiguration == null)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			//Create Automated Commands from Post values
			System.Data.Linq.EntitySet<AutomatedCommand> AutomatedCommands = new System.Data.Linq.EntitySet<AutomatedCommand>();

			foreach(string key in formCollection) {
				
				if (key.StartsWith("AutomatedCommand") && !string.IsNullOrEmpty(formCollection[key])) {
					
					AutomatedCommand automatedCommand = new AutomatedCommand()
					{
						CommandText = formCollection[key],
						CommandExecutionSequenceNumber = int.Parse(key.Replace("AutomatedCommand_", ""))
					};
			
					AutomatedCommands.Add(automatedCommand);
				}
			}

			if (AutomatedCommands != null && AutomatedCommands.Count > 0)
			{
				gdsEndWarningConfiguration.AutomatedCommands = AutomatedCommands;
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<GDSEndWarningConfiguration>(gdsEndWarningConfiguration, "GDSEndWarningConfiguration");
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
				gdsEndWarningConfigurationRepository.Add(gdsEndWarningConfiguration);
			}
			catch (SqlException ex)
			{
				//Non-Unique Name
				if (ex.Message == "NonUniqueName")
				{
					return View("NonUniqueNameError");
				} 
				
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			ViewData["NewSortOrder"] = 0;
			return RedirectToAction("List");
		}

		// GET: /GDSEndWarningConfiguration/Edit
		public ActionResult Edit(int id)
		{
			//Get Item From Database
			GDSEndWarningConfiguration gdsEndWarningConfiguration = new GDSEndWarningConfiguration();
			gdsEndWarningConfiguration = gdsEndWarningConfigurationRepository.GetGroup(id);

			//Check Exists
			if (gdsEndWarningConfiguration == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}
			//Check Access
			if (!rolesRepository.HasWriteAccessToGDSEndWarningConfiguration())
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			GDSEndWarningConfigurationVM gdsEndWarningConfigurationVM = new GDSEndWarningConfigurationVM();
			gdsEndWarningConfigurationVM.GDSEndWarningConfiguration = gdsEndWarningConfiguration;

			//GDS List
			GDSRepository gDSRepository = new GDSRepository();
			SelectList gDSs = new SelectList(gDSRepository.GetClientProfileBuilderGDSs().ToList(), "GDSCode", "GDSName", gdsEndWarningConfiguration.GDSCode);
			gdsEndWarningConfigurationVM.GDSs = gDSs;

			//GDSEndWarningBehaviorTypes
			GDSEndWarningBehaviorTypeRepository gdsEndWarningBehaviorTypeRepository = new GDSEndWarningBehaviorTypeRepository();
			SelectList gdsEndWarningBehaviorTypes = new SelectList(gdsEndWarningBehaviorTypeRepository.GetAllGDSEndWarningBehaviorTypes().ToList(), "GDSEndWarningBehaviorTypeId", "GDSEndWarningBehaviorTypeDescription", gdsEndWarningConfiguration.GDSEndWarningBehaviorTypeId);
			gdsEndWarningConfigurationVM.GDSEndWarningBehaviorTypes = gdsEndWarningBehaviorTypes;

			//Automated Commands
			if (gdsEndWarningConfigurationVM.GDSEndWarningConfiguration.AutomatedCommands == null ||
				(gdsEndWarningConfigurationVM.GDSEndWarningConfiguration.AutomatedCommands != null && gdsEndWarningConfigurationVM.GDSEndWarningConfiguration.AutomatedCommands.Count < 1))
			{
				List<AutomatedCommand> automatedCommands = new List<AutomatedCommand>();
				AutomatedCommand automatedCommand = new AutomatedCommand()
				{
					CommandExecutionSequenceNumber = 1
				};
				automatedCommands.Add(automatedCommand);
				ViewData["AutomatedCommands"] = automatedCommands;
			}
			else
			{
				ViewData["AutomatedCommands"] = gdsEndWarningConfigurationVM.GDSEndWarningConfiguration.AutomatedCommands;
			}

			ViewData["WarningMessage"] = gdsEndWarningConfiguration.IdentifyingWarningMessage;

			return View(gdsEndWarningConfigurationVM);
		}

		// POST: /GDSEndWarningConfiguration/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(GDSEndWarningConfigurationVM gdsEndWarningConfigurationVM, FormCollection formCollection)
		{
			//Get Item
			GDSEndWarningConfiguration gdsEndWarningConfiguration = new GDSEndWarningConfiguration();
			gdsEndWarningConfiguration = gdsEndWarningConfigurationRepository.GetGroup(gdsEndWarningConfigurationVM.GDSEndWarningConfiguration.GDSEndWarningConfigurationId);
			
			//Check Exists
			if (gdsEndWarningConfiguration == null)
			{
				ViewData["ActionMethod"] = "EditPost";
				return View("RecordDoesNotExistError");
			}

			//Check Access Rights
			if (!rolesRepository.HasWriteAccessToGDSEndWarningConfiguration())
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Create Automated Commands from Post values
			System.Data.Linq.EntitySet<AutomatedCommand> AutomatedCommands = new System.Data.Linq.EntitySet<AutomatedCommand>();

			foreach (string key in formCollection)
			{

				if (key.StartsWith("AutomatedCommand") && !string.IsNullOrEmpty(formCollection[key]))
				{

					AutomatedCommand automatedCommand = new AutomatedCommand()
					{
						CommandText = formCollection[key],
						CommandExecutionSequenceNumber = int.Parse(key.Replace("AutomatedCommand_", ""))
					};

					AutomatedCommands.Add(automatedCommand);
				}
			}

			//Remove Automated Commands if not set, otherwise add new ones in
			gdsEndWarningConfiguration.AutomatedCommands = (AutomatedCommands != null && AutomatedCommands.Count > 0) ? AutomatedCommands : null;
			
			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<GDSEndWarningConfiguration>(gdsEndWarningConfiguration, "GDSEndWarningConfiguration");
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
				gdsEndWarningConfigurationRepository.Edit(gdsEndWarningConfiguration);
			}
			catch (SqlException ex)
			{
				//Non-Unique Name
				if (ex.Message == "NonUniqueName")
				{
					return View("NonUniqueNameError");
				}
				//Versioning Error
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/GDSEndWarningConfiguration.mvc/Edit/" + gdsEndWarningConfiguration.GDSEndWarningConfigurationId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List");
		}

		// GET: /GDSEndWarningConfiguration/View
		public ActionResult View(int id)
		{
			GDSEndWarningConfiguration gdsEndWarningConfiguration = new GDSEndWarningConfiguration();
			gdsEndWarningConfiguration = gdsEndWarningConfigurationRepository.GetGroup(id);

			//Check Exists
			if (gdsEndWarningConfiguration == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			GDSEndWarningConfigurationVM GDSEndWarningConfigurationVM = new GDSEndWarningConfigurationVM();

			GDSEndWarningConfigurationVM.GDSEndWarningConfiguration = gdsEndWarningConfiguration;

			ViewData["WarningMessage"] = gdsEndWarningConfiguration.IdentifyingWarningMessage;

			return View(GDSEndWarningConfigurationVM);
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
		{
			GDSEndWarningConfiguration gdsEndWarningConfiguration = new GDSEndWarningConfiguration();
			gdsEndWarningConfiguration = gdsEndWarningConfigurationRepository.GetGroup(id);

			//Check Exists
			if (gdsEndWarningConfiguration == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			GDSEndWarningConfigurationVM GDSEndWarningConfigurationVM = new GDSEndWarningConfigurationVM();

			GDSEndWarningConfigurationVM.GDSEndWarningConfiguration = gdsEndWarningConfiguration;

			ViewData["WarningMessage"] = gdsEndWarningConfiguration.IdentifyingWarningMessage;

			return View(GDSEndWarningConfigurationVM);
		}

		// POST: /GDSEndWarningConfiguration/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(GDSEndWarningConfigurationVM gdsEndWarningConfigurationVM)
		{
			//Check Valid Item passed in Form       
			if (gdsEndWarningConfigurationVM.GDSEndWarningConfiguration == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Get Item From Database
			GDSEndWarningConfiguration gdsEndWarningConfiguration = new GDSEndWarningConfiguration();
			gdsEndWarningConfiguration = gdsEndWarningConfigurationRepository.GetGroup(gdsEndWarningConfigurationVM.GDSEndWarningConfiguration.GDSEndWarningConfigurationId);

			//Check Exists
			if (gdsEndWarningConfiguration == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check Access
			if (!rolesRepository.HasWriteAccessToGDSEndWarningConfiguration())
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Delete Form Item
			try
			{
				gdsEndWarningConfigurationRepository.Delete(gdsEndWarningConfigurationVM.GDSEndWarningConfiguration);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/GDSEndWarningConfiguration.mvc/Delete/" + gdsEndWarningConfigurationVM.GDSEndWarningConfiguration.GDSEndWarningConfigurationId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List");
		}
	}
}
