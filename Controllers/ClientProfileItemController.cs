using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClientProfileServiceBusiness;
using System.Text;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Controllers
{
	public class ClientProfileItemController : Controller
	{
		
		// Main repository
		ClientProfileGroupRepository clientProfileGroupRepository = new ClientProfileGroupRepository();
		ClientProfileItemRepository clientProfileItemRepository = new ClientProfileItemRepository();
		ClientProfileMoveStatusRepository clientProfileMoveStatusRepository = new ClientProfileMoveStatusRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();

		private string groupName = "Client Profile Builder Administrator";

		//
		// GET: /ClientProfileItem/List

        public ActionResult List(int id, int? clientProfilePanelId)
		{

			ClientProfileItemsVM clientProfileItemsVM = new ClientProfileItemsVM();

			//Set Access Rights
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				clientProfileItemsVM.HasDomainWriteAccess = true;
			}

			//Get the ClientProfileGroup
			ClientProfileGroup clientProfileGroup = new ClientProfileGroup();
			clientProfileGroup= clientProfileGroupRepository.GetGroup(id);
			clientProfileGroupRepository.EditGroupForDisplay(clientProfileGroup);
			clientProfileItemsVM.ClientProfileGroup = clientProfileGroup;

            clientProfileItemsVM.ClientProfileGroupId = clientProfileGroup.ClientProfileGroupId;
            clientProfileItemsVM.ClientProfileGroupClientProfileGroupName = clientProfileGroup.ClientProfileGroupName;
            
			GDS gds = new GDS();
			GDSRepository gdsRepository = new GDSRepository();
			gds = gdsRepository.GetGDS(clientProfileGroup.GDSCode);
			clientProfileItemsVM.ClientProfileGroupGDSName = gds.GDSName;

            clientProfileItemsVM.ClientProfileGroupHierarchyItem = clientProfileGroup.HierarchyItem;
            clientProfileItemsVM.ClientProfileGroupBackOfficeSystemDescription = clientProfileGroup.BackOfficeSystem.BackOfficeSystemDescription;
            clientProfileItemsVM.ClientProfilePanelId = clientProfilePanelId ?? 0;

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

			//Get a list of ClientProfileItems for each panel
			clientProfileItemsVM.ClientProfileItemsClientDetails = clientProfileItemRepository.GetClientProfilePanelClientProfileDataElements(id, 6);
            clientProfileItemsVM.ClientProfileItemsMidOffice = clientProfileItemRepository.GetClientProfilePanelClientProfileDataElements(id, 3);
            clientProfileItemsVM.ClientProfileItemsBackOffice = clientProfileItemRepository.GetClientProfilePanelClientProfileDataElements(id, 5);
			clientProfileItemsVM.ClientProfileItemsAirRailPolicy = clientProfileItemRepository.GetClientProfilePanelClientProfileDataElements(id, 7);
			clientProfileItemsVM.ClientProfileItemsItinerary = clientProfileItemRepository.GetClientProfilePanelClientProfileDataElements(id, 9);
            clientProfileItemsVM.ClientProfileItems24Hours = clientProfileItemRepository.GetClientProfilePanelClientProfileDataElements(id, 10);
            clientProfileItemsVM.ClientProfileItemsAmadeusTPM = clientProfileItemRepository.GetClientProfilePanelClientProfileDataElements(id, 11);

            /*GDS Integration*/
            //https://docs.google.com/document/d/1TMOvJzZmePKjFTt0qFUC6_JGRz0x508uIiV3jGCa7b0/

            //TLS Update
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            ClientProfileService lService = null; 
			StringBuilder clientProfileText = new StringBuilder();

			//Format PCC String
			string formattedPcc = GDSIntegration.FormatPccString(clientProfileGroup.PseudoCityOrOfficeId);

			try
			{
				//This is a singleton, and should be used strictly to retrieve existing profiles from the GDS
				lService = ClientProfileService.getInstance;
			}
			catch (Exception ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				//Generic Error
				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			if (lService != null)
			{
				try
				{
					//string pGds, string pPcc, string pZeroLevel, string pCompanyProfileName, stringpTravellerProfileName
					ClientProfileResponse clientProfileResponse = lService.GetProfile(gds.GDSName, formattedPcc, "", clientProfileGroup.ClientProfileGroupName, "");
					if (clientProfileResponse != null)
					{
						if (clientProfileResponse.Result != CWTResponse.ResultStatus.Error)
						{
							if (clientProfileResponse.ClientProfile != null)
							{
								ClientProfile clientProfile = clientProfileResponse.ClientProfile;
								if (clientProfile != null)
								{
									if (clientProfile.ProfileLines.Count > 0)
									{
										//Loop through the profile and write out the lines
										foreach (ClientProfileLine line in clientProfile.ProfileLines)
										{
											//&#13;&#10; is new line
											clientProfileText.AppendFormat("{0} {1}&#13;&#10;", line.LineNumber, line.LineText);
										}
											
										//Pass content into view if exists
										if (!string.IsNullOrEmpty(clientProfileText.ToString()))
										{
											ViewData["clientProfileText"] = clientProfileText.ToString();
										}
									}
								}
							}
						}
						else {
							
							//Log Get Profile error messages and show error in GDS window
							if (clientProfileResponse.MessageList.Count > 0)
							{
								string errorMessage = string.Empty;

								foreach (string message in clientProfileResponse.MessageList)
								{
									errorMessage += string.Format("{0}&#13;&#10;", message);
								}

								if (!string.IsNullOrEmpty(errorMessage))
								{
									ViewData["clientProfileText"] = errorMessage;
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					LogRepository logRepository = new LogRepository();
					logRepository.LogError(ex.Message);

					//Generic Error
					ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
					return View("Error");
				}
			}

			return View(clientProfileItemsVM);
		}

		// POST: /List
        [HttpPost]
        public ActionResult List(ClientProfileItemsVM clientProfileItemsVM, FormCollection formCollection)
        {
            try
            {
				clientProfileItemRepository.UpdateClientProfileItems(clientProfileItemsVM);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

			if (formCollection["redirectToHome"] == "1")
			{
				return RedirectToAction("ListUnDeleted", "ClientProfileGroup");
			}
            
			return RedirectToAction("List", new { id = clientProfileItemsVM.ClientProfileGroupId, clientProfilePanelId = clientProfileItemsVM.ClientProfilePanelId });
        }
	}
}