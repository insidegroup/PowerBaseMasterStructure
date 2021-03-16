using ClientProfileServiceBusiness;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace CWTDesktopDatabase.Controllers
{
	public class ClientProfileGroupController : Controller
	{
		// Main repository
		ClientProfileGroupRepository clientProfileGroupRepository = new ClientProfileGroupRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();

		private string groupName = "Client Profile Builder";

		//
		// GET: /ClientProfileGroup/ListUnDeleted

		public ActionResult ListUnDeleted(string filter, int? page, string sortField, int? sortOrder)
		{
			//SortField
			if (sortField == string.Empty)
			{
				sortField = "UniqueID";
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

			ClientProfileGroupsVM clientProfileGroupsVM = new ClientProfileGroupsVM();

			//Set Access Rights
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				clientProfileGroupsVM.HasDomainWriteAccess = true;
			}

			if (clientProfileGroupRepository != null)
			{
				var clientProfileGroups = clientProfileGroupRepository.PageClientProfileGroups(false, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

				if (clientProfileGroups != null)
				{
					clientProfileGroupsVM.ClientProfileGroups = clientProfileGroups;
				}
			}
			
			//return items
			return View(clientProfileGroupsVM);
		}

		//
		// GET: /ClientProfileGroup/Details/5

		public ActionResult Details(int id)
		{
			return View();
		}

		//
		// GET: /ClientProfileGroup/Create

		public ActionResult Create()
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			ClientProfileGroupVM clientProfileGroupVM = new ClientProfileGroupVM();

			//GDS List
			GDSRepository gDSRepository = new GDSRepository();
			SelectList gDSs = new SelectList(gDSRepository.GetClientProfileBuilderGDSs().ToList(), "GDSCode", "GDSName");
			clientProfileGroupVM.GDSs = gDSs;

			//BackOfficeSystem List removing 'All' option
			BackOfficeSystemRepository backOfficeSystemRepository = new BackOfficeSystemRepository();
			SelectList backOffices = new SelectList(backOfficeSystemRepository.GetAllBackOfficeSystems().ToList(), "BackOfficeSytemId", "BackOfficeSystemDescription");
			clientProfileGroupVM.BackOffices = backOffices;

			//Hierarchy List
			TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
			SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchies(groupName).ToList(), "HierarchyLevelTableName", "HierarchyLevelTableName");
			clientProfileGroupVM.HierarchyTypes = hierarchyTypesList;

			ClientProfileGroup ClientProfileGroup = new ClientProfileGroup();
			clientProfileGroupVM.ClientProfileGroup = ClientProfileGroup;

			return View(clientProfileGroupVM);
		}

		// POST: /Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ClientProfileGroupVM clientProfileGroupVM)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//We need to extract group from groupVM
			ClientProfileGroup clientProfileGroup = new ClientProfileGroup();
			clientProfileGroup = clientProfileGroupVM.ClientProfileGroup;
			if (clientProfileGroup == null)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			//Check Access Rights to Domain Hierarchy
			//if (!hierarchyRepository.AdminHasDomainHierarchyWriteAccess( ClientProfileGroup.HierarchyType, ClientProfileGroup.HierarchyCode, ClientProfileGroup.SourceSystemCode, groupName))
			//{
			//	ViewData["Message"] = "You cannot add to this hierarchy item";
			//	return View("Error");
			//}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<ClientProfileGroup>(clientProfileGroup, "ClientProfileGroup");
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
				clientProfileGroupRepository.Add(clientProfileGroup);
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
			return RedirectToAction("ListUnDeleted");
		}

		// GET: /Edit
		public ActionResult Edit(int id)
		{
			//Get Item From Database
			ClientProfileGroup clientProfileGroup = new ClientProfileGroup();
			clientProfileGroup = clientProfileGroupRepository.GetGroup(id);

			//Check Exists
			if (clientProfileGroup == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientProfileGroup(id))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			ClientProfileGroupVM clientProfileGroupVM = new ClientProfileGroupVM();
			clientProfileGroupRepository.EditGroupForDisplay(clientProfileGroup);
			clientProfileGroupVM.ClientProfileGroup = clientProfileGroup;

			ViewData["UniqueName"] = clientProfileGroup.UniqueName;

			return View(clientProfileGroupVM);
		}

		// POST: /Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(ClientProfileGroupVM clientProfileGroupVM)
		{
			//Get Item
			ClientProfileGroup clientProfileGroup = new ClientProfileGroup();
			clientProfileGroup = clientProfileGroupRepository.GetGroup(clientProfileGroupVM.ClientProfileGroup.ClientProfileGroupId);

			//Check Exists
			if (clientProfileGroup == null)
			{
				ViewData["ActionMethod"] = "EditPost";
				return View("RecordDoesNotExistError");
			}
			//Check Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientProfileGroup(clientProfileGroup.ClientProfileGroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<ClientProfileGroup>(clientProfileGroup, "ClientProfileGroup");
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
				clientProfileGroupRepository.Edit(clientProfileGroup);
			}
			catch (SqlException ex)
			{
				//Versioning Error
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/ClientProfileGroup.mvc/Edit/" + clientProfileGroup.ClientProfileGroupId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			string redirectAction = (clientProfileGroup.DeletedFlag) ? "ListDeleted" : "ListUnDeleted";

			return RedirectToAction(redirectAction);
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
		{
			//Get Item From Database
			ClientProfileGroup clientProfileGroup = new ClientProfileGroup();
			clientProfileGroup = clientProfileGroupRepository.GetGroup(id);

			//Check Exists
			if (clientProfileGroup == null || clientProfileGroup.DeletedFlag == true)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientProfileGroup(id))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			ClientProfileGroupVM clientProfileGroupVM = new ClientProfileGroupVM();
			clientProfileGroupRepository.EditGroupForDisplay(clientProfileGroup);
			clientProfileGroupVM.ClientProfileGroup = clientProfileGroup;

			ViewData["UniqueName"] = clientProfileGroup.UniqueName;

			return View(clientProfileGroupVM);
		}

		// POST: /Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(ClientProfileGroupVM clientProfileGroupVM)
		{
			//Check Valid Item passed in Form       
			if (clientProfileGroupVM.ClientProfileGroup == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Get Item From Database
			ClientProfileGroup clientProfileGroup = new ClientProfileGroup();
			clientProfileGroup = clientProfileGroupRepository.GetGroup(clientProfileGroupVM.ClientProfileGroup.ClientProfileGroupId);

			//Check Exists in Databsase
			if (clientProfileGroup == null || clientProfileGroup.DeletedFlag == true)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientProfileGroup(clientProfileGroup.ClientProfileGroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Delete Form Item
			try
			{
				clientProfileGroupVM.ClientProfileGroup.DeletedFlag = true;
				clientProfileGroupRepository.UpdateGroupDeletedStatus(clientProfileGroupVM.ClientProfileGroup);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/ClientProfileGroup.mvc/Delete/" + clientProfileGroup.ClientProfileGroupId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("ListUnDeleted");
		}

		//
		// GET: /ClientProfileGroup/ListDeleted

		public ActionResult ListDeleted(string filter, int? page, string sortField, int? sortOrder)
		{
			//SortField
			if (sortField == string.Empty)
			{
				sortField = "UniqueID";
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

			ClientProfileGroupsVM clientProfileGroupsVM = new ClientProfileGroupsVM();

			//Set Access Rights
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				clientProfileGroupsVM.HasDomainWriteAccess = true;
			}

			//return items
			clientProfileGroupsVM.ClientProfileGroups = clientProfileGroupRepository.PageClientProfileGroups(true, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

			return View(clientProfileGroupsVM);
		}

		// GET: /View
		public ActionResult View(int id)
		{
			ClientProfileGroup clientProfileGroup = new ClientProfileGroup();
			clientProfileGroup = clientProfileGroupRepository.GetGroup(id);

			//Check Exists
			if (clientProfileGroup == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}
			ClientProfileGroupVM clientProfileGroupVM = new ClientProfileGroupVM();

			clientProfileGroupRepository.EditGroupForDisplay(clientProfileGroup);
			clientProfileGroupVM.ClientProfileGroup = clientProfileGroup;

			ViewData["UniqueName"] = clientProfileGroup.UniqueName;

			return View(clientProfileGroupVM);
		}

		// GET: /UnDelete
		public ActionResult UnDelete(int id)
		{
			//Get Item From Database
			ClientProfileGroup clientProfileGroup = new ClientProfileGroup();
			clientProfileGroup = clientProfileGroupRepository.GetGroup(id);

			//Check Exists
			if (clientProfileGroup == null || clientProfileGroup.DeletedFlag == false)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientProfileGroup(id))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			ClientProfileGroupVM clientProfileGroupVM = new ClientProfileGroupVM();

			clientProfileGroupRepository.EditGroupForDisplay(clientProfileGroup);
			clientProfileGroupVM.ClientProfileGroup = clientProfileGroup;

			ViewData["UniqueName"] = clientProfileGroup.UniqueName;

			return View(clientProfileGroupVM);
		}

		// POST: /UnDelete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UnDelete(ClientProfileGroupVM clientProfileGroupVM)
		{
			//Check Valid Item passed in Form       
			if (clientProfileGroupVM.ClientProfileGroup == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Get Item From Database
			ClientProfileGroup clientProfileGroup = new ClientProfileGroup();
			clientProfileGroup = clientProfileGroupRepository.GetGroup(clientProfileGroupVM.ClientProfileGroup.ClientProfileGroupId);

			//Check Exists in Databsase
			if (clientProfileGroup == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientProfileGroup(clientProfileGroup.ClientProfileGroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Delete Form Item
			try
			{
				clientProfileGroupVM.ClientProfileGroup.DeletedFlag = false;
				clientProfileGroupRepository.UpdateGroupDeletedStatus(clientProfileGroupVM.ClientProfileGroup);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/ClientProfileGroup.mvc/UnDelete/" + clientProfileGroup.ClientProfileGroupId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("ListDeleted");
		}

		// GET: /Export
		public FileStreamResult Export(int id)
		{
			string clientProfileBuilderText = GDSIntegration.WriteGDSProfile(id);

			var byteArray = Encoding.ASCII.GetBytes(clientProfileBuilderText);

			var stream = new MemoryStream(byteArray);

			return File(stream, "text/plain", string.Format("ClientProfileGroup-{0}.txt", id.ToString()));
		}

		// GET: /Publish
		public ActionResult Publish(int id)
		{
			ClientProfileGroup clientProfileGroup = new ClientProfileGroup();
			clientProfileGroup = clientProfileGroupRepository.GetGroup(id);

			//Check Exists
			if (clientProfileGroup == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}
			ClientProfileGroupVM clientProfileGroupVM = new ClientProfileGroupVM();

			clientProfileGroupRepository.EditGroupForDisplay(clientProfileGroup);
			clientProfileGroupVM.ClientProfileGroup = clientProfileGroup;

			//Get ClientTopUnit
			if (clientProfileGroup.HierarchyType == "ClientSubUnit")
			{
				ClientProfileGroupClientSubUnit clientProfileGroupClientSubUnit = clientProfileGroup.ClientProfileGroupClientSubUnits.SingleOrDefault();
				if (clientProfileGroupClientSubUnit != null)
				{
					ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
					ClientSubUnit clientSubUnit = new ClientSubUnit();
					clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientProfileGroupClientSubUnit.ClientSubUnitGuid);
					if (clientSubUnit != null)
					{
						if (clientSubUnit.ClientTopUnit != null)
						{
							ViewData["ClientTopUnitName"] = clientSubUnit.ClientTopUnit.ClientTopUnitName;
						}
					}
				}
			}

			//GDS Window
			string clientProfileBuilderText = GDSIntegration.WriteGDSProfile(id);
			if (!string.IsNullOrEmpty(clientProfileBuilderText))
			{
				ViewData["clientProfileText"] = clientProfileBuilderText;
			}

			return View(clientProfileGroupVM);
		}

		// POST: /Publish
		[HttpPost]
		public JsonResult Publish(int clientProfileGroupId, string sabreStatus, string gdsCode)
		{
			//Get Item From Database
			ClientProfileGroup clientProfileGroup = new ClientProfileGroup();
			clientProfileGroup = clientProfileGroupRepository.GetGroup(clientProfileGroupId);

			//Check Exists in Database
			if (clientProfileGroup == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return Json("Record Does Not Exist Error");
			}

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientProfileGroup(clientProfileGroup.ClientProfileGroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return Json("Error");
			}

			GDS gds = new GDS();
			GDSRepository gdsRepository = new GDSRepository();
			gds = gdsRepository.GetGDS(clientProfileGroup.GDSCode);

			string gdsName = gds.GDSName;

			switch (gdsCode)
			{
				case "1S":
					gdsName = sabreStatus;
					break;
			}

			string response = DoPublish(clientProfileGroup, gds, gdsName, clientProfileGroup.PseudoCityOrOfficeId, false);
			
			return Json(response);
		}

		// POST: /Verify
		[HttpPost]
		public JsonResult Verify(int clientProfileGroupId, string sabreStatus, string gdsCode)
		{
			//Get Item From Database
			ClientProfileGroup clientProfileGroup = new ClientProfileGroup();
			clientProfileGroup = clientProfileGroupRepository.GetGroup(clientProfileGroupId);

			//Check Exists in Databsase
			if (clientProfileGroup == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return Json("Record Does Not Exist Error");
			}

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientProfileGroup(clientProfileGroup.ClientProfileGroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return Json("Error");
			}

			GDS gds = new GDS();
			GDSRepository gdsRepository = new GDSRepository();
			gds = gdsRepository.GetGDS(clientProfileGroup.GDSCode);

			string verifyPCC = string.Empty;
			string gdsName = gds.GDSName;
			
			switch (gdsCode)
			{
				case "1G":
					verifyPCC = ConfigurationManager.AppSettings["VerifyPCC_Galileo"].ToString();
					break;
				case "1V":
					verifyPCC = ConfigurationManager.AppSettings["VerifyPCC_Apollo"].ToString();
					break;
				case "1A":
					verifyPCC = ConfigurationManager.AppSettings["VerifyPCC_Amadeus"].ToString();
					break;
				case "1S":
					verifyPCC = ConfigurationManager.AppSettings["VerifyPCC_Sabre"].ToString();
					gdsName = sabreStatus;
					break;
			}

			if (string.IsNullOrEmpty(verifyPCC))
			{
				//Need to add values into web config
				LogRepository logRepository = new LogRepository();
				logRepository.LogError("Please ensure verify PCC is present for GDS");
				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return Json("There was a problem with your request, please see the log file or contact an administrator for details");
			}

			string response = DoPublish(clientProfileGroup, gds, gdsName, verifyPCC, true);

			return Json(response);
		}

		public string DoPublish([Bind(Exclude = "HierarchyType, HierarchyItem, HierarchyCode")] ClientProfileGroup clientProfileGroup,
								[Bind(Include = "GDSCode, GDSName")] GDS gds, string gdsName, string formattedPcc, bool verify = false)
		{
            /*GDS Integration*/
            //https://docs.google.com/document/d/1TMOvJzZmePKjFTt0qFUC6_JGRz0x508uIiV3jGCa7b0/

            //TLS Update
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            ClientProfileService lService = null;
			CWTResponse CWTResponse = null;

			string errorMessage = string.Empty;

			try
			{				
				//This is a singleton, and should be used strictly to retrieve existing profiles from the GDS
				lService = ClientProfileService.getInstance;

				if (lService == null)
				{
					return "Service Error";
				}
				
				ClientProfile clientProfile = null;

				ClientProfileResponse clientProfileResponse = lService.GetProfile(gdsName, formattedPcc, "", clientProfileGroup.ClientProfileGroupName, "");

				if (clientProfileResponse != null && clientProfileResponse.ClientProfile != null)
				{
					clientProfile = clientProfileResponse.ClientProfile;
				}

				if (clientProfileResponse != null && clientProfileResponse.MessageList != null && clientProfileResponse.MessageList.Count > 0)
				{
					errorMessage = string.Format("These errors were returned by the GDS for PCC when retrieving profile:");

					foreach (string message in clientProfileResponse.MessageList)
					{
						errorMessage += string.Format("{0}<br/>", message);
					}

					LogRepository logRepository = new LogRepository();
					logRepository.LogError(errorMessage);
					return errorMessage;
				}
						
				//Get Lines
				List<ClientProfileItemVM> clientProfileItemsList = GDSIntegration.GetProfileLineItems(clientProfileGroup.ClientProfileGroupId);
				List<ClientProfileLine> clientProfileItemLines = GDSIntegration.WriteProfileLines(clientProfileItemsList, gds.GDSCode);

				//Modify Profile
				if (clientProfile != null)
				{
					//Replace current lines with new lines
					clientProfile.ProfileLines = clientProfileItemLines;
                    CWTResponse = clientProfile.DeleteProfileLines();
                    CWTResponse = clientProfile.ModifyProfile();
				}
				//Create New
				else
				{
					clientProfile = new ClientProfile(
						gds.GDSName,
						formattedPcc,
						clientProfileGroup.ClientProfileGroupName,
						string.Empty,
						clientProfileItemLines);

					CWTResponse = clientProfile.SaveProfile();
				}
			}
			catch (Exception ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return "There was a problem with your request, please see the log file or contact an administrator for details";
			}

			//Update Client Profile Publish Date
			if (CWTResponse != null && CWTResponse.Result != CWTResponse.ResultStatus.Error)
			{
				try
				{
					//Don't update timestamp for verify process
					if (!verify)
					{
						clientProfileGroupRepository.UpdateGroupPublishDate(clientProfileGroup);
					}
				}
				catch (SqlException ex)
				{
					//Versioning Error
					if (ex.Message == "SQLVersioningError")
					{
						ViewData["ReturnURL"] = "/ClientProfileGroup.mvc/Publish/" + clientProfileGroup.ClientProfileGroupId;
						return "Version Error";
					}

					LogRepository logRepository = new LogRepository();
					logRepository.LogError(ex.Message);
					ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details.";
					return "Error";
				}
			}
			else if (CWTResponse != null && CWTResponse.MessageList != null && CWTResponse.MessageList.Count > 0)
			{
				errorMessage = string.Format("These errors were returned by the GDS for PCC when retrieving profile:");

				foreach (string message in CWTResponse.MessageList)
				{
					errorMessage += string.Format("{0}<br/>", message);
				}

				errorMessage += "<br/>Please correct the profile and try to re-publish again.";

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(errorMessage);
				return errorMessage;
			}
			else
			{
				errorMessage = "There was a problem with your request, please see the log file or contact an administrator for details.";
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(errorMessage);
				return errorMessage;
			}
			
			return "true";
		}

		/// <summary>
		/// A function to check if the profile is complete prior to publishing.
		/// </summary>
		/// <param name="clientProfileGroupId"></param>
		/// <returns></returns>
		[HttpPost]
		public JsonResult IsProfileReadytoPublish(string clientProfileGroupId)
		{		
			if (clientProfileGroupId == null)
			{
				return Json("No Id");
			}

			int id = Int32.Parse(clientProfileGroupId);

			string retval = "true";

			//Get Item From Database
			ClientProfileGroup clientProfileGroup = new ClientProfileGroup();
			clientProfileGroup = clientProfileGroupRepository.GetGroup(id);

			//Get GDS
			GDS gds = new GDS();
			GDSRepository gdsRepository = new GDSRepository();
			gds = gdsRepository.GetGDS(clientProfileGroup.GDSCode);

			//Check Exists
			if (clientProfileGroup == null)
			{
				retval = "RecordDoesNotExistError";
			}

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientProfileGroup(id))
			{
				retval = "You do not have access to this item";
			}

			List<ClientProfileItemVM> clientProfileItemsList = GDSIntegration.GetProfileLineItems(id, true);

			bool doIncompleteFieldsExists = false;

			//Check if all mandatory fields are complete
			foreach (ClientProfileItemVM clientProfileItemVM in clientProfileItemsList) {
				
				ClientProfileItemRow clientProfileItemRow = clientProfileItemVM.ClientProfileItem;
				
				if(clientProfileItemRow.MandatoryFlag == true && (
						clientProfileItemRow.ClientProfileMoveStatusId == null || 
						string.IsNullOrEmpty(clientProfileItemRow.Remark) || 
						string.IsNullOrEmpty(clientProfileItemRow.GDSCommandFormat))) {

							doIncompleteFieldsExists = true;
				}
			}

			if (doIncompleteFieldsExists)
			{
				return Json("There are incomplete mandatory elements.<br/><br/>Please return to the Items page and correct the error.");
			}

			//Check Line Counts
			int lineCount = clientProfileItemsList.Count();
			int maxLineCount = 0;

			switch (gds.GDSName)
			{
				case "Apollo":
					maxLineCount = 200;
					break;
				case "Amadeus":
					maxLineCount = 100;
					break;
				case "Galileo":
					maxLineCount = 200;
					break;
				case "Sabre":
					maxLineCount = 200;
					break;
			}

			if(lineCount > maxLineCount) {
				retval = "The client profile exceeds the maximum number of lines.<br/><br/>Please return to the Items page and correct the error.";
			}

			return Json(retval);
		}

		/// <summary>
		/// A function to check if the profile has been updated whilst user currently editing.
		/// </summary>
		/// <param name="clientProfileGroupId"></param>
		/// <param name="lastUpdateTimestamp"></param>
		/// <returns></returns>
		[HttpPost]
		public JsonResult HasClientProfileGroupBeenUpdatedSinceLoad(string clientProfileGroupId, string lastUpdateTimestamp)
		{
			string retval = string.Empty;

			int id = Int32.Parse(clientProfileGroupId);

			//Get Item From Database
			ClientProfileGroup clientProfileGroup = new ClientProfileGroup();
			clientProfileGroup = clientProfileGroupRepository.GetGroup(id);

			//Check Exists
			if (clientProfileGroup == null)
			{
				retval = "RecordDoesNotExistError";
			}

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientProfileGroup(id))
			{
				retval = "You do not have access to this item";
			}

			if (!string.IsNullOrEmpty(lastUpdateTimestamp))
			{
				DateTime profileLoadedTimestamp = CWTDateHelpers.RoundToSecond(Convert.ToDateTime(lastUpdateTimestamp));
				DateTime currentTimestamp = CWTDateHelpers.RoundToSecond(Convert.ToDateTime(clientProfileGroup.LastUpdateTimestamp));
				retval = (currentTimestamp > profileLoadedTimestamp) ? "true" : "false";
			}
			else
			{
				retval = "false";
			}

			return Json(retval);
		}
	}
}
