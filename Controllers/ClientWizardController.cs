using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Web.Mvc;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System.Data.SqlClient;
using System.Xml.Serialization;
using System.Text;
using System.IO;

namespace CWTDesktopDatabase.Controllers
{
    [AjaxTimeOutCheck]
    public class ClientWizardController : Controller
    {
        //main repositories
        ClientWizardRepository clientWizardRepository = new ClientWizardRepository();
        HierarchyRepository hierarchyRepository = new HierarchyRepository();
        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
        ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
		private string groupName = "Client Detail";

        //WRAPPER PAGE FOR WIZARD- Returns View
        public ActionResult Index()
        {
            return View();
        }

        //WIZARD STEP 1: Show Search Options  - Returns PartialView
        [HttpPost]
        public ActionResult ClientSelectionScreen()
        {
            //Check Access Rights
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                return PartialView("Error", "You Do Not Have Access to Clients. Please Contact an Administrator");
            }

            return PartialView("ClientSelectionScreen", null);
        }

        //Search for ClientTopUnits or ClientSubUnits (PART OF STEP 1)- Returns PartialView
        [HttpPost]
        public ActionResult ClientSearch(string filterField, string filter)
        {
            filter = System.Web.HttpUtility.UrlDecode(filter);

			if (filter.Length < 2)
			{
				return PartialView("Error", "There was an error with your request");
			} 
			
			List<spDDAWizard_SelectClientTopUnitSubUnitFiltered_v1Result> searchResults = new List<spDDAWizard_SelectClientTopUnitSubUnitFiltered_v1Result>();
            searchResults = clientWizardRepository.GetClientTopUnitAndSubUnits(filter, filterField);

            if (searchResults.Count > 0)
            {

                if (searchResults[0].ReturnType == "ClientSubUnit")
                {
                    return PartialView("ClientSubUnitSearchResults", searchResults);
                }

            }
            return PartialView("ClientTopUnitSearchResults", searchResults);
        }

        //List ClientSubUnits of a ClientTopUnit (PART OF STEP 1)- Returns PartialView
        [HttpPost]
        public ActionResult ListClientSubUnits(string clientTopUnitGuid)
        {
            if (clientTopUnitGuid == "")
            {
                return PartialView("Error", "There was an error with your selection");
            }
            //We only have ID, so here we get the whole item
            ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientTopUnitGuid);

            //Check Exists
            if (clientTopUnit == null)
            {
                return PartialView("Error", "ClientTopUnit Does Not Exist");
            }

            var searchResults = clientTopUnitRepository.GetClientTopUnitSubUnits(clientTopUnitGuid).ToList();
            return PartialView("ClientTopUnitClientSubUnits", searchResults);

        }

        //WIZARD STEP 2: - Show Editable Client Returns PartialView
        [HttpPost]
        public ActionResult ClientDetailsScreen(string clientSubUnitGuid)
        {
            if (clientSubUnitGuid == "")
            {
                return PartialView("Error", "There was an error with your selection");
            }           

            //We only have ID, so here we get the whole item
            ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

            //Check Exists
            if (clientSubUnit == null)
            {
                return PartialView("Error", "ClientSubUnit Does Not Exist");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid))
            {
                return PartialView("Error", "You Do Not Have Access to This Item");
            }

            //get the associated ClientTopUnit
            ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientSubUnit.ClientTopUnitGuid);

            //add info from linked tables
            //clientTopUnitRepository.EditGroupForDisplay(clientTopUnit);
            clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);

            //Model to Store ClientTopUnit and ClientSubUnit(s) - will be passed to page
            ClientWizardVM clientWizardViewModel = new ClientWizardVM();

            //ClientSubUnitTelephony - Added v 2.02.1 (US654) - March 2013
            CallerEnteredDigitDefinitionTypeRepository callerEnteredDigitDefinitionTypeRepository = new CallerEnteredDigitDefinitionTypeRepository();
            clientWizardViewModel.CallerEnteredDigitDefinitionTypes = new SelectList(callerEnteredDigitDefinitionTypeRepository.GetAllCallerEnteredDigitDefinitionTypes().ToList(), "CallerEnteredDigitDefinitionTypeId", "CallerEnteredDigitDefinitionTypeDescription");

            ClientSubUnitTelephonyRepository clientSubUnitTelephonyRepository = new ClientSubUnitTelephonyRepository();
            List<ClientSubUnitTelephony> clientSubUnitTelephonies = new List<ClientSubUnitTelephony>();
            clientSubUnitTelephonies = clientSubUnitTelephonyRepository.GetClientSubUnitTelephonies(clientSubUnitGuid);
            clientWizardViewModel.ClientSubUnitTelephonies = clientSubUnitTelephonies;

			//Check Restricted Client Access
			clientWizardViewModel.RestrictedClientAccess = false;
			if (hierarchyRepository.AdminHasDomainWriteAccess("Restricted Client"))
			{
				clientWizardViewModel.RestrictedClientAccess = true;
			}

			//Check Compliance Access
			clientWizardViewModel.ComplianceAdministratorAccess = false;
			if (hierarchyRepository.AdminHasDomainWriteAccess("Compliance Administrator"))
			{
				clientWizardViewModel.ComplianceAdministratorAccess = true;
			}

            //Populate Model
			clientWizardViewModel.ClientSubUnit = clientSubUnit;
			clientWizardViewModel.ClientTopUnit = clientTopUnit;
			
			//Portrait Status
			PortraitStatusRepository portraitStatusRepository = new PortraitStatusRepository();
            clientWizardViewModel.ClientSubUnitPortraitStatuses = new SelectList(portraitStatusRepository.GetAllPortraitStatuses().ToList(), "PortraitStatusId", "PortraitStatusDescription", clientWizardViewModel.ClientSubUnit.PortraitStatusId);

			//Line of Business
			LineOfBusinessRepository lineOfBusinessRepository = new LineOfBusinessRepository();
			clientWizardViewModel.ClientSubUnitLineOfBusinesses = new SelectList(lineOfBusinessRepository.GetAllLineOfBusinesses().ToList(), "LineofBusinessId", "LineofBusinessDescription", clientWizardViewModel.ClientSubUnit.LineOfBusinessId);


            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = clientWizardRepository.GetClientSubUnitPolicyGroup(clientSubUnitGuid);
            if (policyGroup != null)
            {
                policyGroup.HierarchyType = "ClientSubUnit";
                policyGroup.HierarchyCode = clientSubUnitGuid;
                clientWizardViewModel.PolicyGroup = policyGroup;
            }
            //Return Details to Users
            return PartialView("ClientDetailsScreen", clientWizardViewModel);

        }

        //WIZARD STEP 2B: Validate Client on Submit (no save)- Returns STEP 3 as JSON
        [HttpPost]
        public ActionResult ValidateClient(ClientWizardVM clientWizardViewModel)
        {


            //Validate data against Tables - WE should never get any errors here because we only show optional fields
            if (!ModelState.IsValid)
            {
                string n = "";
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        n += error.ErrorMessage;
                    }
                }

                //Need to repopulate Lists
                PortraitStatusRepository portraitStatusRepository = new PortraitStatusRepository();
                clientWizardViewModel.ClientSubUnitPortraitStatuses = new SelectList(portraitStatusRepository.GetAllPortraitStatuses().ToList(), "PortraitStatusId", "PortraitStatusDescription", clientWizardViewModel.ClientSubUnit.PortraitStatusId);

                //Validation Error - retrun to Details Screen
                return Json(new WizardJSONResponse
                {
                    html = ControllerExtension.RenderPartialViewToString(this, "ClientDetailsScreen", clientWizardViewModel),
                    message = "ValidationError (" + n + ")",
                    success = false
                });
            }

            //Location
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientWizardViewModel.ClientSubUnit;
            ClientSubUnitTeamsVM clientSubUnitTeams = new ClientSubUnitTeamsVM();
           //clientTeams.Location = locationWizardViewModel.Location;

            // Systemusers
            ClientSubUnitTeamRepository clientSubUnitTeamRepository = new ClientSubUnitTeamRepository();
            List<spDesktopDataAdmin_SelectClientSubUnitTeams_v1Result> teams = new List<spDesktopDataAdmin_SelectClientSubUnitTeams_v1Result>();
            teams = clientSubUnitTeamRepository.GetClientSubUnitTeams(clientSubUnit.ClientSubUnitGuid);
            clientSubUnitTeams.Teams = teams.ToList();
            clientSubUnitTeams.ClientSubUnit = clientSubUnit;

            //Show ClientSubUnits's Teams
            return Json(new WizardJSONResponse
            {
                html = ControllerExtension.RenderPartialViewToString(this, "CSUTeamsScreen", clientSubUnitTeams),
                message = "Success",
                success = true
            });

        }

        //Search for Teams (PART OF STEP 3)- Returns PartialView
        [HttpPost]
        public ActionResult TeamSearch(string filterField, string filter)
        {
            filter = System.Web.HttpUtility.UrlDecode(filter);
            var searchResults = clientWizardRepository.GetTeams(filter, filterField);
            return PartialView("TeamSearchResults", searchResults);
        }

        //WIZARD STEP 4: Show a list of ClientAccounts for a ClientSubUnit  - Returns PartialView
        [HttpPost]
        public ActionResult ClientAccountsScreen(string clientSubUnitGuid)
        {

            //parameter only contains the Id, so we get the whole item
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

            //Check Exists
            if (clientSubUnit == null)
            {
                return PartialView("Error", "This Client Does Not Exist");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnit.ClientSubUnitGuid))
            {
                return PartialView("Error", "You Do Not Have Access to This Item");
            }

            List<spDDAWizard_SelectClientSubUnitClientAccounts_v1Result> clientSubUnitClientAccounts = new List<spDDAWizard_SelectClientSubUnitClientAccounts_v1Result>();
            clientSubUnitClientAccounts = clientWizardRepository.GetClientSubUnitClientAccounts(clientSubUnit.ClientSubUnitGuid);

            ClientSubUnitClientAccountsVM clientSubUnitClientAccountsViewModel = new ClientSubUnitClientAccountsVM();
            clientSubUnitClientAccountsViewModel.ClientAccounts = clientSubUnitClientAccounts;
            clientSubUnitClientAccountsViewModel.ClientSubUnit = clientSubUnit;

            return PartialView("CSUClientAccountsScreen", clientSubUnitClientAccountsViewModel);

        }

        //Search for CllientAccounts (PART OF STEP 4)- Returns PartialView
        [HttpPost]
        public ActionResult ClientAccountSearch(string filterField1, string filter1, string filterField2, string filter2, string filterField3, string filter3)
        {
            filter1 = System.Web.HttpUtility.UrlDecode(filter1);
            filter2 = System.Web.HttpUtility.UrlDecode(filter2);
            filter3 = System.Web.HttpUtility.UrlDecode(filter3);

			if ((!string.IsNullOrEmpty(filterField1) && (filter1 != null && filter1.Length < 2)) || 
				(!string.IsNullOrEmpty(filterField2) && (filter2 != null && filter2.Length < 2)) || 
				(!string.IsNullOrEmpty(filterField3) && (filter3 != null && filter3.Length < 2))) {
				return PartialView("Error", "There was an error with your request");
			} 
			
			var searchResults = clientWizardRepository.GetClientAccountsFiltered(filter1, filterField1, filter2, filterField2, filter3, filterField3);
            return PartialView("ClientAccountSearchResults", searchResults);
        }

        //WIZARD STEP 5: Show a list of Servicing Options for a ClientSubUnit  - Returns PartialView
        [HttpPost]
        public ActionResult ServicingOptionsScreen(string clientSubUnitGuid)
        {

            //parameter only contains the Id, so we get the whole item
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

            //Check Exists
            if (clientSubUnit == null)
            {
                return PartialView("Error", "This Client Does Not Exist");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnit.ClientSubUnitGuid))
            {
                return PartialView("Error", "You Do Not Have Access to This Item");
            }

           

            //Model to Return
            ClientServicingOptionItemsVM clientServicingOptionItemsVM = new ClientServicingOptionItemsVM();

            ServicingOptionGroup servicingOptionGroup = new ServicingOptionGroup();
            servicingOptionGroup = clientWizardRepository.GetClientSubUnitServicingOptionGroup(clientSubUnitGuid);
            clientServicingOptionItemsVM.ServicingOptionGroupId = servicingOptionGroup.ServicingOptionGroupId;

            //GDSs
            GDSRepository gdsRepository = new GDSRepository();
            clientServicingOptionItemsVM.GDSs = new SelectList(gdsRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName");


            //Check Access Rights to ClientSubUnit's Servicing Options
            clientServicingOptionItemsVM.ServicingOptionWriteAccess = false;
            if (hierarchyRepository.AdminHasDomainHierarchyWriteAccess("ClientSubUnit", clientSubUnitGuid, "", "Servicing Option"))
            {
                clientServicingOptionItemsVM.ServicingOptionWriteAccess = true;
            }

            //Loop Client's ServicingOptions and put into compatible format
            List<spDDAWizard_SelectClientSubUnitServicingOptions_v1Result> clientSubUnitServicingOptions = new List<spDDAWizard_SelectClientSubUnitServicingOptions_v1Result>();
            clientSubUnitServicingOptions = clientWizardRepository.GetClientSubUnitServicingOptions(clientSubUnit.ClientSubUnitGuid);
            foreach (spDDAWizard_SelectClientSubUnitServicingOptions_v1Result item in clientSubUnitServicingOptions)
            {
                ServicingOptionVM servicingOptionVM = new ServicingOptionVM();
                ServicingOptionItemValueRepository servicingOptionItemValueRepository = new ServicingOptionItemValueRepository();
                GDSRepository gDSRepository = new GDSRepository();
                

                //ServicingOptionItem main information
                ServicingOptionItem servicingOptionItem = new ServicingOptionItem();
                if (item.GDSCode!=null)
                {
                    GDS gds = new GDS();
                    gds = gDSRepository.GetGDS(item.GDSCode);
                    servicingOptionItem.GDSCode = item.GDSCode;
                    servicingOptionItem.GDSName = gds.GDSName;
                }
                servicingOptionItem.ServicingOptionId = Convert.ToInt32(item.ServicingOptionId);
                servicingOptionItem.ServicingOptionItemId = Convert.ToInt32(item.ServicingOptionItemId);
                servicingOptionItem.ServicingOptionName = item.ServicingOptionName;
                servicingOptionItem.ServicingOptionItemInstruction = item.ServicingOptionItemInstruction;
                servicingOptionItem.ServicingOptionItemValue = item.ServicingOptionItemValue;
                servicingOptionItem.ServicingOptionItemSequence = Convert.ToInt32(item.ServicingOptionItemSequence);
                //servicingOptionItem.DisplayInApplicationFlag = item.DisplayInApplicationFlag;
				servicingOptionItem.VersionNumber = (int)item.VersionNumber;
                servicingOptionItem.Source = item.Source;
                servicingOptionItem.SourceName = item.SourceName;
                servicingOptionItem.ServicingOptionGroupName = item.ServicingOptionGroupName;
                servicingOptionVM.ServicingOptionItem = servicingOptionItem;
                

                servicingOptionItem.DepartureTimeWindowMinutes = Convert.ToInt32(item.DepartureTimeWindowMinutes);
				servicingOptionItem.ArrivalTimeWindowMinutes = item.ArrivalTimeWindowMinutes;
				servicingOptionItem.MaximumConnectionTimeMinutes = Convert.ToInt32(item.MaximumConnectionTimeMinutes); 
				servicingOptionItem.MaximumStops = item.MaximumStops;
				servicingOptionItem.UseAlternateAirportFlag = item.UseAlternateAirportFlag;
				servicingOptionItem.NoPenaltyFlag = item.NoPenaltyFlag;
				servicingOptionItem.NoRestrictionsFlag = item.NoRestrictionsFlag;
                


                //Parameters
                servicingOptionVM.ShowEditParameterButton = (item.ServicingOptionId == 1 || item.ServicingOptionId == 2 || item.ServicingOptionId == 90 || item.ServicingOptionId == 291);

				//ServicingOption GDSRequiredFlag
				ServicingOption servicingOption = new ServicingOption();
				ServicingOptionRepository servicingOptionRepository = new ServicingOptionRepository();
				servicingOption = servicingOptionRepository.GetServicingOption(servicingOptionItem.ServicingOptionId);
				servicingOptionItem.GDSRequiredFlag = servicingOption.GDSRequiredFlag;
				
				//Possible Values of ServicingOptionItem
                servicingOptionVM.ServicingOptionItemValues = servicingOptionItemValueRepository.GetServicingOptionServicingOptionItemValues(Convert.ToInt32(item.ServicingOptionId), clientServicingOptionItemsVM.ServicingOptionGroupId);

                //Add to Model
                clientServicingOptionItemsVM.Add(servicingOptionVM);

            }

            //return to User
            return PartialView("CSUServicingOptionsScreen", clientServicingOptionItemsVM);

        }

		//Show Popup for adding Servicing Options (PART OF STEP 5)- Returns PartialView
		public ActionResult ServicingOptionPopup(string csu)
		{
			ServicingOptionRepository servicingOptionRepository = new ServicingOptionRepository();
			SelectList servicingOptionsList = new SelectList(servicingOptionRepository.GetAvailableServicingOptionsClientSubUnit(csu).ToList(), "ServicingOptionId", "ServicingOptionName");
			ViewData["ServicingOptions"] = servicingOptionsList;

			GDSRepository gdsRepository = new GDSRepository();
			SelectList gDSList = new SelectList(gdsRepository.GetAllGDSsExceptALL().ToList(), "GDSCode", "GDSName");
			ViewData["GDSs"] = gDSList;

			ViewData["DepartureTimeWindowMinutesList"] = new SelectList(
				servicingOptionRepository.GetServicingOptionDepartureTimeWindows().Select(
					x => new { value = x, text = x }
				), "value", "text");

			ViewData["ArrivalTimeWindowMinutesList"] = new SelectList(
				servicingOptionRepository.GetServicingOptionArrivalTimeWindows().Select(
					x => new { value = x, text = x }
				), "value", "text");

			ViewData["MaximumStopsList"] = new SelectList(
				servicingOptionRepository.GetServicingOptionMaximumStops().Select(
					x => new { value = x, text = x }
				), "value", "text");

			return PartialView("ServicingOptionsPopup", null);
		}

		//Show Popup for editing Servicing Options (PART OF STEP 5)- Returns PartialView
		public ActionResult ServicingOptionPopupEdit(string csu, int servicingOptionItemId)
		{
			ServicingOptionItemRepository servicingOptionItemRepository = new ServicingOptionItemRepository();
			ServicingOptionItem servicingOptionItem = servicingOptionItemRepository.GetItem(servicingOptionItemId);
			servicingOptionItemRepository.EditItemForDisplay(servicingOptionItem);

			ServicingOptionRepository servicingOptionRepository = new ServicingOptionRepository();
			SelectList servicingOptionsList = new SelectList(
				servicingOptionRepository.GetAvailableServicingOptionsClientSubUnit(csu).ToList(), 
				"ServicingOptionId", 
				"ServicingOptionName",
				servicingOptionItem.ServicingOptionId);

			ViewData["ServicingOptions"] = servicingOptionsList;
		
			GDSRepository gdsRepository = new GDSRepository();
			SelectList gDSList = new SelectList(gdsRepository.GetAllGDSsExceptALL().ToList(), "GDSCode", "GDSName", servicingOptionItem.GDSCode);
			ViewData["GDSs"] = gDSList;

			ViewData["DepartureTimeWindowMinutesList"] = new SelectList(
				servicingOptionRepository.GetServicingOptionDepartureTimeWindows().Select(
					x => new { value = x, text = x }
				), "value", "text", servicingOptionItem.DepartureTimeWindowMinutes);

			ViewData["ArrivalTimeWindowMinutesList"] = new SelectList(
				servicingOptionRepository.GetServicingOptionArrivalTimeWindows().Select(
					x => new { value = x, text = x }
				), "value", "text", servicingOptionItem.ArrivalTimeWindowMinutes);

			ViewData["MaximumStopsList"] = new SelectList(
				servicingOptionRepository.GetServicingOptionMaximumStops().Select(
					x => new { value = x, text = x }
				), "value", "text", servicingOptionItem.MaximumStops);

			ServicingOptionVM servicingOptionVM = new ServicingOptionVM();
			servicingOptionVM.ServicingOptionItem = servicingOptionItem;

			return PartialView("ServicingOptionsPopupEdit", servicingOptionVM);
		}

        //WIZARD STEP 6: Show a list of ReasonCodes for a ClientSubUnit  - Returns PartialView
        [HttpPost]
        public ActionResult ReasonCodesScreen(string clientSubUnitGuid, bool inherited)
        {

            //parameter only contains the Id, so we get the whole item
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

            //Check Exists
            if (clientSubUnit == null)
            {
                return PartialView("Error", "This Client Does Not Exist");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnit.ClientSubUnitGuid))
            {
                return PartialView("Error", "You Do Not Have Access to This Item"); 
            }

            ClientSubUnitReasonCodesVM clientSubUnitReasonCodesVM = new ClientSubUnitReasonCodesVM(); 
            clientSubUnitReasonCodesVM = clientWizardRepository.GetClientSubUnitReasonCodes(clientSubUnit.ClientSubUnitGuid);
            clientSubUnitReasonCodesVM.Inherited = inherited;

            //Check Access Rights to ClientSubUnit's ReasonCodes
            clientSubUnitReasonCodesVM.ReasonCodeGroupWriteAccess = false;
            if (hierarchyRepository.AdminHasDomainHierarchyWriteAccess("ClientSubUnit", clientSubUnitGuid, "", "Reason Code"))
            {
                clientSubUnitReasonCodesVM.ReasonCodeGroupWriteAccess = true;
            }

            //Check Access Rights to ClientSubUnit's Policies - (for Next Step, skip or continue)
            clientSubUnitReasonCodesVM.PolicyGroupWriteAccess = false;
            if (hierarchyRepository.AdminHasDomainHierarchyWriteAccess("ClientSubUnit", clientSubUnitGuid, "", "Policy Hierarchy"))
            {
                clientSubUnitReasonCodesVM.PolicyGroupWriteAccess = true;
            }

            //return data to user
            return PartialView("CSUReasonCodesScreen", clientSubUnitReasonCodesVM);

        }

		/* Removed as of 2.09.1 - US835
        //ReasonCodeGroups Grid(PART OF STEP 6)- Returns PartialView
        [HttpPost]
        public ActionResult ReasonCodesGridScreen(string clientSubUnitGuid)
        {
            //proc returns ProductReasonCodes as XML
            List<spDDAWizard_SelectClientSubUnitReasonCodeGroups_v1Result> reasonCodeGroups = new List<spDDAWizard_SelectClientSubUnitReasonCodeGroups_v1Result>();
            reasonCodeGroups = clientWizardRepository.GetReasonCodeGroups(clientSubUnitGuid);
            string xml = "<doc>" + reasonCodeGroups[0].ProductReasonCodeTypes.ToString() + "</doc>";

            //put xml into stringreader
            var doc = System.Xml.Linq.XElement.Parse(xml);
            var productReasonCodeTypes = (from x in doc.Elements("ProductReasonCodeTypes") select x).FirstOrDefault();
            XmlSerializer serializer = new XmlSerializer(typeof(ProductReasonCodeTypes));
            var sr = new StringReader(productReasonCodeTypes.ToString());
            
            //objects
            ProductReasonCodeTypesVM productReasonCodeTypesVM = new ProductReasonCodeTypesVM();
            ProductRepository productRepository = new ProductRepository();
            ReasonCodeTypeRepository reasonCodeTypeRepository = new ReasonCodeTypeRepository();

            // Use the Deserialize method to restore the object's state.
            productReasonCodeTypesVM.ProductReasonCodeTypes = (ProductReasonCodeTypes)serializer.Deserialize(sr);
            productReasonCodeTypesVM.Products = productRepository.GetAllProducts().ToList();
            productReasonCodeTypesVM.ReasonCodeTypes = reasonCodeTypeRepository.GetAllReasonCodeTypes().ToList();

            
            //return data to user
            return PartialView("ReasonCodesGridScreen", productReasonCodeTypesVM);

        }*/
     
        //ReasonCodePopup Grid(PART OF STEP 6)- Returns PartialView
        //a custom item has productId + rctId but no groupID
        //a default item has groupID but no productId + rctId
        public ActionResult ReasonCodesPopup(int? groupId, int productId, int reasonCodeTypeId, string reasonCode = "")
         {
             ClientWizardReasonCodesVM clientWizardReasonCodesVM = new ClientWizardReasonCodesVM();
             clientWizardReasonCodesVM.ProductId = productId;
             clientWizardReasonCodesVM.ReasonCodeTypeId = reasonCodeTypeId;

             if (groupId.HasValue)  //add items if they exist
             {
                 clientWizardReasonCodesVM.ReasonCodes = clientWizardRepository.GetReasonCodeGroupReasonCodeItems(groupId.Value, productId, reasonCodeTypeId);
                 clientWizardReasonCodesVM.ReasonCodeGroupId = groupId.Value;
             }
             else  //else null
             {
                 List<spDDAWizard_SelectReasonCodeGroupReasonCodeItems_v1Result> reasonCodes = new List<spDDAWizard_SelectReasonCodeGroupReasonCodeItems_v1Result>();
                 clientWizardReasonCodesVM.ReasonCodes = reasonCodes;
             }
             
             //returns all items OR all items available for group
             clientWizardReasonCodesVM.AvailableReasonCodes = clientWizardRepository.GetReasonCodeGroupAvailableReasonCodeItems(groupId, productId, reasonCodeTypeId);

			//Get Product
			Product product = new Product();
			ProductRepository productRepository = new ProductRepository();
			product = productRepository.GetProduct(productId);
			if (product != null)
			{
				ViewData["ProductId"] = product.ProductId;
				ViewData["ProductName"] = product.ProductName;
			}

			//Get ReasonCode
			ReasonCodeType reasonCodeType = new ReasonCodeType();
			ReasonCodeTypeRepository reasonCodeTypeRepository = new ReasonCodeTypeRepository();
			reasonCodeType = reasonCodeTypeRepository.GetItem(reasonCodeTypeId);
			if (reasonCodeType != null)
			{
				ViewData["ReasonCodeTypeId"] = reasonCodeType.ReasonCodeTypeId;
				ViewData["ReasonCodeTypeDescription"] = reasonCodeType.ReasonCodeTypeDescription;
			}

			//Get Group 
			if (groupId != null)
			{
				CWTDesktopDatabase.Models.ReasonCodeGroup reasonCodeGroup = new CWTDesktopDatabase.Models.ReasonCodeGroup();
				ReasonCodeGroupRepository reasonCodeGroupRepository = new ReasonCodeGroupRepository();
				reasonCodeGroup = reasonCodeGroupRepository.GetGroup((int)groupId);
				if(reasonCodeGroup != null) {
					ViewData["ReasonCodeGroupID"] = reasonCodeGroup.ReasonCodeGroupId;
					ViewData["ReasonCodeGroupDescription"] = reasonCodeGroup.ReasonCodeGroupName;
				}
			}

			SelectList availableReasonCodesList = new SelectList(clientWizardReasonCodesVM.AvailableReasonCodes, "ReasonCode", "ReasonCodeProductDescription");
			
			//If we are editing, then we need to add in current item
			if (!string.IsNullOrEmpty(reasonCode))
			{
				var availableReasonCodesListSelected = availableReasonCodesList.ToList();
				availableReasonCodesListSelected.Add(new SelectListItem { Text = reasonCode, Value = reasonCode });
				availableReasonCodesList = new SelectList(availableReasonCodesListSelected.OrderBy(x => x.Value), "Value", "Text", reasonCode);
				ViewData["reasonCode"] = reasonCode;
			}

			ViewData["AvailableReasonCodesList"] = availableReasonCodesList;

             return PartialView("ReasonCodesPopup", clientWizardReasonCodesVM);
         }

		 //ReasonCodePopup Grid(PART OF STEP 6)- Returns PartialView
        //a custom item has productId + rctId but no groupID
        //a default item has groupID but no productId + rctId
		public ActionResult ReasonCodesPopupDelete(int? groupId, int productId, int reasonCodeTypeId, string reasonCode)
		{
			ClientWizardReasonCodesVM clientWizardReasonCodesVM = new ClientWizardReasonCodesVM();
			clientWizardReasonCodesVM.ProductId = productId;
			clientWizardReasonCodesVM.ReasonCodeTypeId = reasonCodeTypeId;

			//Get Product
			Product product = new Product();
			ProductRepository productRepository = new ProductRepository();
			product = productRepository.GetProduct(productId);
			if (product != null)
			{
				ViewData["ProductId"] = product.ProductId;
				ViewData["ProductName"] = product.ProductName;
			}

			//Get ReasonCode
			ReasonCodeType reasonCodeType = new ReasonCodeType();
			ReasonCodeTypeRepository reasonCodeTypeRepository = new ReasonCodeTypeRepository();
			reasonCodeType = reasonCodeTypeRepository.GetItem(reasonCodeTypeId);
			if (reasonCodeType != null)
			{
				ViewData["ReasonCodeTypeId"] = reasonCodeType.ReasonCodeTypeId;
				ViewData["ReasonCodeTypeDescription"] = reasonCodeType.ReasonCodeTypeDescription;
			}

			//Get Group 
			if (groupId != null)
			{
				CWTDesktopDatabase.Models.ReasonCodeGroup reasonCodeGroup = new CWTDesktopDatabase.Models.ReasonCodeGroup();
				ReasonCodeGroupRepository reasonCodeGroupRepository = new ReasonCodeGroupRepository();
				reasonCodeGroup = reasonCodeGroupRepository.GetGroup((int)groupId);
				if (reasonCodeGroup != null)
				{
					ViewData["reasonCodeGroupID"] = reasonCodeGroup.ReasonCodeGroupId;
					ViewData["reasonCodeGroupDescription"] = reasonCodeGroup.ReasonCodeGroupName;
				}
			}

			if (!string.IsNullOrEmpty(reasonCode))
			{
				ViewData["reasonCode"] = reasonCode;
			}

			return PartialView("ReasonCodesPopupDelete", clientWizardReasonCodesVM);
		}

        //WIZARD STEP 7: Show a blank PolicyGroup for a ClientSubUnit  - Returns PartialView
        // only shown if ClientSubUnit has no Policy Group
        [HttpPost]
        public ActionResult PolicyGroupScreen(string clientSubUnitGuid)
        {

            //parameter only contains the Id, so we get the whole item
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

            //Check Exists
            if (clientSubUnit == null)
            {
                return PartialView("Error", "This Client Does Not Exist");
            }

            //Check AccessRights to ClientSubUnit
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnit.ClientSubUnitGuid))
            {
                return PartialView("Error", "You Do Not Have Access to This Client");
            }

            //Check Access Rights to ClientSubUnit's Policies
            if (!hierarchyRepository.AdminHasDomainHierarchyWriteAccess("ClientSubUnit", clientSubUnitGuid, "", "Policy Hierarchy"))
            {
                return PartialView("Error", "You Do Not Have Access to This Client's Policies");
            }

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup.HierarchyCode = clientSubUnit.ClientSubUnitGuid;
            policyGroup.HierarchyItem = clientSubUnit.ClientSubUnitName;
            policyGroup.InheritFromParentFlag = true;
            policyGroup.HierarchyType = "ClientSubUnit";

            TripTypeRepository tripTypeRepository = new TripTypeRepository();
            SelectList tripTypesList = new SelectList(tripTypeRepository.GetAllTripTypes().ToList(), "TripTypeId", "TripTypeDescription");
            ViewData["TripTypes"] = tripTypesList;

            GDSRepository gdsRepository = new GDSRepository();
            SelectList gDSList = new SelectList(gdsRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName");
            ViewData["GDSs"] = gDSList;

            PNROutputTypeRepository pNROutputTypeRepository = new PNROutputTypeRepository();
            SelectList pNROutputTypeList = new SelectList(pNROutputTypeRepository.GetAllPNROutputTypes().ToList(), "PNROutputTypeId", "PNROutputTypeName");
            ViewData["PNROutputTypes"] = pNROutputTypeList;

            TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
            SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchies("Policy Hierarchy").ToList(), "HierarchyLevelTableName", "HierarchyLevelTableName");
            ViewData["HierarchyTypes"] = hierarchyTypesList;

            //Aero_Connect_001_ClientTopUnit_Policy
            GroupNameBuilderRepository groupNameBuilderRepository = new GroupNameBuilderRepository();
            string identifier = groupNameBuilderRepository.ClientSubUnitIdentifier(clientSubUnitGuid, "Policy Hierarchy");

            string autoName = clientSubUnit.ClientSubUnitName + "_" + identifier + "_ClientSubUnit_Policy";
            if (clientSubUnit.ClientSubUnitName.Length > 79)
            {
                autoName = clientSubUnit.ClientSubUnitName.Substring(0, 79) + "_" + identifier + "_ClientSubUnit_Policy";
            }
            policyGroup.PolicyGroupName = autoName.Replace(" ", "_");

            
            return PartialView("CSUPolicyGroupScreen", policyGroup);
        }

        //WIZARD STEP 7: Validate Client PolicyGroup on Submit (no save)- Returns STEP 3 as JSON
        [HttpPost]
        public ActionResult ValidateClientPolicyGroup(ClientWizardVM clientWizardViewModel)
        {

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = clientWizardViewModel.PolicyGroup;

            ClientSubUnitPoliciesVM clientSubUnitPoliciesVM = new ClientSubUnitPoliciesVM();
            clientSubUnitPoliciesVM = clientWizardRepository.GetClientSubUnitPolicies(clientWizardViewModel.ClientSubUnit.ClientSubUnitGuid);
            
            //we change here to posted item
            clientSubUnitPoliciesVM.PolicyGroup = clientWizardViewModel.PolicyGroup;

            //Show ClientSubUnits's Teams
            return Json(new WizardJSONResponse
            {
                html = ControllerExtension.RenderPartialViewToString(this, "CSUPoliciesScreen", clientSubUnitPoliciesVM),
                message = "Success",
                success = true
            });

        }

        //WIZARD STEP 7: Show a list of PolicyGroup Items for a ClientSubUnit  - Returns PartialView
        [HttpPost]
        public ActionResult PoliciesScreen(string clientSubUnitGuid)
         {

            //parameter only contains the Id, so we get the whole item
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

            //Check Exists
            if (clientSubUnit == null)
            {
                return PartialView("Error", "This Client Does Not Exist");
            }

            //Check AccessRights to ClientSubUnit
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnit.ClientSubUnitGuid))
            {
                return PartialView("Error", "You Do Not Have Access to This Item");
            }
            ClientSubUnitPoliciesVM clientSubUnitPoliciesVM = new ClientSubUnitPoliciesVM();
            clientSubUnitPoliciesVM = clientWizardRepository.GetClientSubUnitPolicies(clientSubUnitGuid);

           
            return PartialView("CSUPoliciesScreen", clientSubUnitPoliciesVM);

         }

        //WIZARD STEP 7.01A: PolicyAirCabinGroupItemPopup - Returns PartialView
        public ActionResult PolicyAirCabinGroupItemPopup(int policyAirCabinGroupItemId)
         {

             PolicyAirCabinGroupItemRepository policyAirCabinGroupItemRepository = new PolicyAirCabinGroupItemRepository();
             PolicyRoutingRepository policyRoutingRepository = new PolicyRoutingRepository();
             PolicyAirCabinGroupItemViewModel policyAirCabinGroupItemViewModel = new PolicyAirCabinGroupItemViewModel();

             PolicyAirCabinGroupItem policyAirCabinGroupItem = new PolicyAirCabinGroupItem();
             policyAirCabinGroupItem.EnabledFlag = true;
             PolicyRouting policyRouting = new PolicyRouting();

             if (policyAirCabinGroupItemId != 0)
             {
                 policyAirCabinGroupItem = policyAirCabinGroupItemRepository.GetPolicyAirCabinGroupItem(policyAirCabinGroupItemId);
                 policyAirCabinGroupItemRepository.EditItemForDisplay(policyAirCabinGroupItem);
                 if (policyAirCabinGroupItem.PolicyRoutingId != null)
                 {
                     policyRouting = policyRoutingRepository.GetPolicyRouting((int)policyAirCabinGroupItem.PolicyRoutingId);
                     policyRoutingRepository.EditPolicyRouting(policyRouting);
                 }    
             }
             
             policyAirCabinGroupItemViewModel.PolicyAirCabinGroupItem = policyAirCabinGroupItem;              
             policyAirCabinGroupItemViewModel.PolicyRouting = policyRouting;

             //Populate List of AirlineCabins
             AirlineCabinRepository airlineCabinRepository = new AirlineCabinRepository();
             SelectList airlineCabins = new SelectList(airlineCabinRepository.GetAllAirlineCabins().ToList(), "AirlineCabinCode", "AirlineCabinDefaultDescription");
             ViewData["AirlineCabinCodeList"] = airlineCabins;

             return PartialView("PolicyAirCabinGroupItemPopup", policyAirCabinGroupItemViewModel);
         }

        //WIZARD STEP 7.01B: Validate PolicyAirCabinGroupItemPopup (no save)- Returns JSON
        [HttpPost]
        public ActionResult PolicyAirCabinGroupItemValidation(PolicyAirCabinGroupItemViewModel policyAirCabinGroupItemViewModel)
        {

            //Validate data against Tables
            if (!ModelState.IsValid)
            {

                string n = "";
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    //if(modelState.Value = ""
                    foreach (ModelError error in modelState.Errors)
                    {
                        if (error.ErrorMessage != "Name Required")
                        {
                            n += error.ErrorMessage;
                        }
                    }
                }

                if (n != "")
                {
                    //Populate List of AirlineCabins
                    AirlineCabinRepository airlineCabinRepository = new AirlineCabinRepository();
                    SelectList airlineCabins = new SelectList(airlineCabinRepository.GetAllAirlineCabins().ToList(), "AirlineCabinCode", "AirlineCabinDefaultDescription");
                    ViewData["AirlineCabinCodeList"] = airlineCabins;

                    //Validation Error - return to Form
                    return Json(new WizardJSONResponse
                    {
                        html = ControllerExtension.RenderPartialViewToString(this, "PolicyAirCabinGroupItemPopup", policyAirCabinGroupItemViewModel),
                        message = "ValidationError (" + n + ")",
                        success = false
                    });
                }
            }

            //No Errors - return to Main Screen
            return Json(new WizardJSONResponse
            {
                html = "",
                message = "success",
                success = true
            });


        }

        //WIZARD STEP 7.01C: PolicyAirCabinGroupItemPolicyRoutingPopup - Returns PartialView
		[HttpPost]
        public ActionResult PolicyAirCabinGroupItemPolicyRoutingPopup(PolicyAirCabinGroupItem policyAirCabinGroupItem)
        {

            PolicyAirCabinGroupItemRepository policyAirCabinGroupItemRepository = new PolicyAirCabinGroupItemRepository();
            PolicyRoutingRepository policyRoutingRepository = new PolicyRoutingRepository();
            PolicyAirCabinGroupItemViewModel policyAirCabinGroupItemViewModel = new PolicyAirCabinGroupItemViewModel();
            PolicyRouting policyRouting = new PolicyRouting();

            policyAirCabinGroupItemViewModel.PolicyAirCabinGroupItem = policyAirCabinGroupItem;
            policyAirCabinGroupItemViewModel.PolicyRouting = policyRouting;

            return PartialView("PolicyAirCabinGroupItemPolicyRoutingPopup", policyAirCabinGroupItemViewModel);
        }

        //WIZARD STEP 7.02A: PolicyAirMissedSavingsThresholdGroupItem - Returns PartialView
		[HttpGet]
        public ActionResult PolicyAirMissedSavingsThresholdGroupItemPopup(int policyAirMissedSavingsThresholdGroupItemId)
        {

            PolicyAirMissedSavingsThresholdGroupItemRepository PolicyAMSTGroupItemRepository = new PolicyAirMissedSavingsThresholdGroupItemRepository();
            PolicyAirMissedSavingsThresholdGroupItem policyAMSTGroupItem = new PolicyAirMissedSavingsThresholdGroupItem();
            policyAMSTGroupItem.PolicyProhibitedFlag = true;
            policyAMSTGroupItem.SavingsZeroedOutFlag = true;
            policyAMSTGroupItem.EnabledFlag = true;

            if (policyAirMissedSavingsThresholdGroupItemId != 0)
            {
                policyAMSTGroupItem = PolicyAMSTGroupItemRepository.GetPolicyAirMissedSavingsThresholdGroupItem(policyAirMissedSavingsThresholdGroupItemId);
            }

            PolicyAirMissedSavingsThresholdGroupItemVM policyAMSTGroupItemVM = new PolicyAirMissedSavingsThresholdGroupItemVM();
            policyAMSTGroupItemVM.PolicyAirMissedSavingsThresholdGroupItem = policyAMSTGroupItem;

            //Currencies
            CurrencyRepository currencyRepository = new CurrencyRepository();
            policyAMSTGroupItemVM.Currencies = new SelectList(currencyRepository.GetAllCurrencies().ToList(), "CurrencyCode", "Name", policyAMSTGroupItem.CurrencyCode);

            //RoutingCodes
            PolicyAirMissedSavingsThresholdRoutingRepository routingRepository = new PolicyAirMissedSavingsThresholdRoutingRepository();
            policyAMSTGroupItemVM.RoutingCodes = new SelectList(routingRepository.GetAllPolicyAirMissedSavingsThresholdRoutings().ToList(), "RoutingCode", "RoutingDescription", policyAMSTGroupItem.RoutingCode);

            return PartialView("PolicyAirMissedSavingsThresholdGroupItemPopup", policyAMSTGroupItemVM);
        }

        //WIZARD STEP 7.02B: Validate PolicyAirMissedSavingsThresholdGroupItem (no save)- Returns JSON
        [HttpPost]
        public ActionResult PolicyAirMissedSavingsThresholdGroupItemValidation(PolicyAirMissedSavingsThresholdGroupItemVM policyAMSTGroupItemVM)
        {
            //Validate data against Tables
            try
            {
                UpdateModel<PolicyAirMissedSavingsThresholdGroupItem>(policyAMSTGroupItemVM.PolicyAirMissedSavingsThresholdGroupItem, "PolicyAirMissedSavingsThresholdGroupItem");
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


                CurrencyRepository currencyRepository = new CurrencyRepository();
                policyAMSTGroupItemVM.Currencies = new SelectList(currencyRepository.GetAllCurrencies().ToList(), "CurrencyCode", "Name", policyAMSTGroupItemVM.PolicyAirMissedSavingsThresholdGroupItem.CurrencyCode);

                //RoutingCodes
                PolicyAirMissedSavingsThresholdRoutingRepository routingRepository = new PolicyAirMissedSavingsThresholdRoutingRepository();
                policyAMSTGroupItemVM.RoutingCodes = new SelectList(routingRepository.GetAllPolicyAirMissedSavingsThresholdRoutings().ToList(), "RoutingCode", "RoutingDescription", policyAMSTGroupItemVM.PolicyAirMissedSavingsThresholdGroupItem.RoutingCode);

                //Validation Error - return to Form
                return Json(new WizardJSONResponse
                {
                    html = ControllerExtension.RenderPartialViewToString(this, "PolicyAirMissedSavingsThresholdGroupItemPopup", policyAMSTGroupItemVM),
                    message = "ValidationError (" + n + ")",
                    success = false
                });
            }

            //No Errors - return to Main Screen
            return Json(new WizardJSONResponse
            {
                html = "",
                message = "success",
                success = true
            });
        }


		//WIZARD STEP 7.03A: PolicyAirParameterGroupItemPopup - Returns PartialView
		[HttpGet]
		public ActionResult PolicyAirParameterGroupItemPopup(int policyAirParameterGroupItemId, int policyAirParameterTypeId)
		{

			PolicyAirParameterGroupItemRepository policyAirParameterGroupItemRepository = new PolicyAirParameterGroupItemRepository();
			PolicyRoutingRepository policyRoutingRepository = new PolicyRoutingRepository();
			PolicyAirParameterGroupItemVM policyAirParameterGroupItemViewModel = new PolicyAirParameterGroupItemVM();

			PolicyAirParameterGroupItem policyAirParameterGroupItem = new PolicyAirParameterGroupItem();
			policyAirParameterGroupItem.EnabledFlag = true;
			policyAirParameterGroupItem.PolicyAirParameterTypeId = policyAirParameterTypeId;
			PolicyRouting policyRouting = new PolicyRouting();
			if (policyAirParameterGroupItemId != 0)
			{
				policyAirParameterGroupItem = policyAirParameterGroupItemRepository.GetPolicyAirParameterGroupItem(policyAirParameterGroupItemId);
				policyAirParameterGroupItemRepository.EditItemForDisplay(policyAirParameterGroupItem);
				policyRouting = policyRoutingRepository.GetPolicyRouting(policyAirParameterGroupItem.PolicyRoutingId ?? 0);
				policyRoutingRepository.EditPolicyRouting(policyRouting);
			}
			policyAirParameterGroupItemViewModel.PolicyAirParameterGroupItem = policyAirParameterGroupItem;
			policyAirParameterGroupItemViewModel.PolicyRouting = policyRouting;

			return PartialView("PolicyAirParameterGroupItemPopup", policyAirParameterGroupItemViewModel);
		}

		//WIZARD STEP 7.03B: Validate PolicyAirParameterGroupItemPopup (no save)- Returns JSON
		[HttpPost]
		public ActionResult PolicyAirParameterGroupItemValidation(PolicyAirParameterGroupItemVM policyAirParameterGroupItemViewModel)
		{
			//Validate data against Tables
			if (!ModelState.IsValid)
			{
				string n = "";
				foreach (ModelState modelState in ViewData.ModelState.Values)
				{
					foreach (ModelError error in modelState.Errors)
					{
						n += error.ErrorMessage;
					}
				}

				//Validation Error - return to Form
				return Json(new WizardJSONResponse
				{
					html = ControllerExtension.RenderPartialViewToString(this, "PolicyAirParameterGroupItemPopup", policyAirParameterGroupItemViewModel),
					message = "ValidationError (" + n + ")",
					success = false
				});
			}

			//No Errors - return to Main Screen
			return Json(new WizardJSONResponse
			{
				html = "",
				message = "success",
				success = true
			});
		}
		
		//WIZARD STEP 7.03A: PolicyAirVendorGroupItemPopup - Returns PartialView
        public ActionResult PolicyAirVendorGroupItemPopup(int policyAirVendorGroupItemId)
        {

            PolicyAirVendorGroupItemRepository policyAirVendorGroupItemRepository = new PolicyAirVendorGroupItemRepository();
            PolicyRoutingRepository policyRoutingRepository = new PolicyRoutingRepository();
            PolicyAirVendorGroupItemVM policyAirVendorGroupItemViewModel = new PolicyAirVendorGroupItemVM();

            PolicyAirVendorGroupItem policyAirVendorGroupItem = new PolicyAirVendorGroupItem();
            policyAirVendorGroupItem.EnabledFlag = true;
            PolicyRouting policyRouting = new PolicyRouting();
            if (policyAirVendorGroupItemId != 0)
            {
                policyAirVendorGroupItem = policyAirVendorGroupItemRepository.GetPolicyAirVendorGroupItem(policyAirVendorGroupItemId);
                policyAirVendorGroupItemRepository.EditItemForDisplay(policyAirVendorGroupItem);
                policyRouting = policyRoutingRepository.GetPolicyRouting(policyAirVendorGroupItem.PolicyRoutingId);
                policyRoutingRepository.EditPolicyRouting(policyRouting);
            }
            policyAirVendorGroupItemViewModel.PolicyAirVendorGroupItem = policyAirVendorGroupItem;   
            policyAirVendorGroupItemViewModel.PolicyRouting = policyRouting;

            //Populate List of PolicyAirStatuses
            PolicyAirStatusRepository policyAirStatusRepository = new PolicyAirStatusRepository();
            SelectList policyAirStatuses = new SelectList(policyAirStatusRepository.GetAllPolicyAirStatuses().ToList(), "PolicyAirStatusId", "PolicyAirStatusDescription");
            ViewData["PolicyAirStatusList"] = policyAirStatuses;

            //Populate List of AirVendorRankings
            SelectList airVendorRankings = new SelectList(policyAirVendorGroupItemRepository.AirVendorRankings().ToList(), "Value", "Text", policyAirVendorGroupItem.AirVendorRanking);
            ViewData["AirVendorRankings"] = airVendorRankings;

            return PartialView("PolicyAirVendorGroupItemPopup", policyAirVendorGroupItemViewModel);
        }

        //WIZARD STEP 7.03B: Validate PolicyAirVendorGroupItemPopup (no save)- Returns JSON
        [HttpPost]
        public ActionResult PolicyAirVendorGroupItemValidation(PolicyAirVendorGroupItemVM policyAirVendorGroupItemViewModel)
        {


            //Validate data against Tables
            if (!ModelState.IsValid)
            {
                string n = "";
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        n += error.ErrorMessage;
                    }
                }

				//Populate List of PolicyAirStatuses
                PolicyAirStatusRepository policyAirStatusRepository = new PolicyAirStatusRepository();
                SelectList policyAirStatuses = new SelectList(policyAirStatusRepository.GetAllPolicyAirStatuses().ToList(), "PolicyAirStatusId", "PolicyAirStatusDescription");
                ViewData["PolicyAirStatusList"] = policyAirStatuses;

				//Populate List of AirVendorRankings
				PolicyAirVendorGroupItemRepository policyAirVendorGroupItemRepository = new PolicyAirVendorGroupItemRepository();
				SelectList airVendorRankings = new SelectList(policyAirVendorGroupItemRepository.AirVendorRankings().ToList(), "Value", "Text");
				ViewData["AirVendorRankings"] = airVendorRankings;
				
				//Populate List of Products
                ProductRepository productRepository = new ProductRepository();
                SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
                ViewData["ProductList"] = products;

                //Validation Error - return to Form
                return Json(new WizardJSONResponse
                {
                    html = ControllerExtension.RenderPartialViewToString(this, "PolicyAirVendorGroupItemPopup", policyAirVendorGroupItemViewModel),
                    message = "ValidationError (" + n + ")",
                    success = false
                });
            }

            //No Errors - return to Main Screen
            return Json(new WizardJSONResponse
            {
                html = "",
                message = "success",
                success = true
            });


        }

        //WIZARD STEP 7.03C: PolicyAirVendorGroupItemPolicyRoutingPopup - Returns PartialView
		[HttpPost]
        public ActionResult PolicyAirVendorGroupItemPolicyRoutingPopup(PolicyAirVendorGroupItem policyAirVendorGroupItem)
        {
            PolicyAirVendorGroupItemRepository policyAirVendorGroupItemRepository = new PolicyAirVendorGroupItemRepository();
            PolicyRoutingRepository policyRoutingRepository = new PolicyRoutingRepository();
            PolicyAirVendorGroupItemVM policyAirVendorGroupItemViewModel = new PolicyAirVendorGroupItemVM();
            PolicyRouting policyRouting = new PolicyRouting();

            policyAirVendorGroupItemRepository.EditItemForDisplay(policyAirVendorGroupItem);
            policyAirVendorGroupItemViewModel.PolicyAirVendorGroupItem = policyAirVendorGroupItem;
            policyAirVendorGroupItemViewModel.PolicyRouting = policyRouting;

            return PartialView("PolicyAirVendorGroupItemPolicyRoutingPopup", policyAirVendorGroupItemViewModel);
        }
        
        //WIZARD STEP 7.04A: PolicyCarTypeGroupItemPopup - Returns PartialView
        public ActionResult PolicyCarTypeGroupItemPopup(int policyCarTypeGroupItemId)
        {

            PolicyCarTypeGroupItemRepository policyCarTypeGroupItemRepository = new PolicyCarTypeGroupItemRepository();
            PolicyCarTypeGroupItem policyCarTypeGroupItem = new PolicyCarTypeGroupItem();
            policyCarTypeGroupItem.EnabledFlag = true;

            if (policyCarTypeGroupItemId != 0)
            {
                policyCarTypeGroupItem = policyCarTypeGroupItemRepository.GetPolicyCarTypeGroupItem(policyCarTypeGroupItemId);
                policyCarTypeGroupItemRepository.EditItemForDisplay(policyCarTypeGroupItem);
            }

            //Populate List of PolicyLocations
            PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
            SelectList policyLocations = new SelectList(policyLocationRepository.GetAllPolicyLocations().ToList(), "PolicyLocationId", "PolicyLocationName");
            ViewData["PolicyLocationList"] = policyLocations;

            //Populate List of PolicyCarStatuses 
            PolicyCarStatusRepository policyCarStatusRepository = new PolicyCarStatusRepository();
            SelectList carStatuses = new SelectList(policyCarStatusRepository.GetAllPolicyCarStatuses().ToList(), "PolicyCarStatusId", "PolicyCarStatusDescription");
            ViewData["PolicyCarStatusList"] = carStatuses;

            //Populate List of CarTypeCategories
            CarTypeCategoryRepository carTypeCategoryRepository = new CarTypeCategoryRepository();
            SelectList carTypes = new SelectList(carTypeCategoryRepository.GetAllCarTypeCategories().ToList(), "CarTypeCategoryId", "CarTypeCategoryName");
            ViewData["CarTypeCategoryList"] = carTypes;

            return PartialView("PolicyCarTypeGroupItemPopup", policyCarTypeGroupItem);
        }

        //WIZARD STEP 7.04B: Validate PolicyCarTypeGroupItem (no save)- Returns JSON
        [HttpPost]
        public ActionResult PolicyCarTypeGroupItemValidation(PolicyCarTypeGroupItem policyCarTypeGroupItem)
        {
            //Validate data against Tables
            if (!ModelState.IsValid)
            {
                string n = "";
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        n += error.ErrorMessage;
                    }
                }

                //Populate List of PolicyLocations
                PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
                SelectList policyLocations = new SelectList(policyLocationRepository.GetAllPolicyLocations().ToList(), "PolicyLocationId", "PolicyLocationName");
                ViewData["PolicyLocationList"] = policyLocations;

                //Populate List of PolicyCarStatuses 
                PolicyCarStatusRepository policyCarStatusRepository = new PolicyCarStatusRepository();
                SelectList carStatuses = new SelectList(policyCarStatusRepository.GetAllPolicyCarStatuses().ToList(), "PolicyCarStatusId", "PolicyCarStatusDescription");
                ViewData["PolicyCarStatusList"] = carStatuses;

                //Populate List of CarTypeCategories
                CarTypeCategoryRepository carTypeCategoryRepository = new CarTypeCategoryRepository();
                SelectList carTypes = new SelectList(carTypeCategoryRepository.GetAllCarTypeCategories().ToList(), "CarTypeCategoryId", "CarTypeCategoryName");
                ViewData["CarTypeCategoryList"] = carTypes;

                //Validation Error - return to Form
                return Json(new WizardJSONResponse
                {
                    html = ControllerExtension.RenderPartialViewToString(this, "PolicyCarTypeGroupItemPopup", policyCarTypeGroupItem),
                    message = "ValidationError (" + n + ")",
                    success = false
                });
            }

            //No Errors - return to Main Screen
            return Json(new WizardJSONResponse
            {
                html = "",
                message = "success",
                success = true
            });
        }

        //WIZARD STEP 7.05A: PolicyCarVendorGroupItemPopup - Returns PartialView
        public ActionResult PolicyCarVendorGroupItemPopup(int policyCarVendorGroupItemId)
        {

            PolicyCarVendorGroupItemRepository policyCarVendorGroupItemRepository = new PolicyCarVendorGroupItemRepository();
            PolicyCarVendorGroupItem policyCarVendorGroupItem = new PolicyCarVendorGroupItem();
            policyCarVendorGroupItem.EnabledFlag = true;

            if (policyCarVendorGroupItemId != 0)
            {
                policyCarVendorGroupItem = policyCarVendorGroupItemRepository.GetPolicyCarVendorGroupItem(policyCarVendorGroupItemId);
                policyCarVendorGroupItemRepository.EditItemForDisplay(policyCarVendorGroupItem);
            }

            //Populate List of PolicyLocations
            PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
            SelectList policyLocations = new SelectList(policyLocationRepository.GetAllPolicyLocations().ToList(), "PolicyLocationId", "PolicyLocationName");
            ViewData["PolicyLocationList"] = policyLocations;

            //Populate List of PolicyCarStatuses 
            PolicyCarStatusRepository policyCarStatusRepository = new PolicyCarStatusRepository();
            SelectList policyCarStatuses = new SelectList(policyCarStatusRepository.GetAllPolicyCarStatuses().ToList(), "PolicyCarStatusId", "PolicyCarStatusDescription");
            ViewData["PolicyCarStatusList"] = policyCarStatuses;

            //Populate List of Products 
            ProductRepository productRepository = new ProductRepository();
            SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
            ViewData["Products"] = products;

            return PartialView("PolicyCarVendorGroupItemPopup", policyCarVendorGroupItem);
        }

        //WIZARD STEP 7.05B: Validate PolicyCarVendorGroupItem (no save)- Returns JSON
        [HttpPost]
        public ActionResult PolicyCarVendorGroupItemValidation(PolicyCarVendorGroupItem policyCarVendorGroupItem)
        {
            //Validate data against Tables
            if (!ModelState.IsValid)
            {
                string n = "";
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        n += error.ErrorMessage;
                    }
                }

                //Populate List of PolicyLocations
                PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
                SelectList policyLocations = new SelectList(policyLocationRepository.GetAllPolicyLocations().ToList(), "PolicyLocationId", "PolicyLocationName");
                ViewData["PolicyLocationList"] = policyLocations;

                //Populate List of PolicyCarStatuses 
                PolicyCarStatusRepository policyCarStatusRepository = new PolicyCarStatusRepository();
                SelectList policyCarStatuses = new SelectList(policyCarStatusRepository.GetAllPolicyCarStatuses().ToList(), "PolicyCarStatusId", "PolicyCarStatusDescription");
                ViewData["PolicyCarStatusList"] = policyCarStatuses;

                //Populate List of Products 
                ProductRepository productRepository = new ProductRepository();
                SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
                ViewData["Products"] = products;

                //Validation Error - return to Form
                return Json(new WizardJSONResponse
                {
                    html = ControllerExtension.RenderPartialViewToString(this, "PolicyCarVendorGroupItemPopup", policyCarVendorGroupItem),
                    message = "ValidationError (" + n + ")",
                    success = false
                });
            }

            //No Errors - return to Main Screen
            return Json(new WizardJSONResponse
            {
                html = "",
                message = "success",
                success = true
            });
        }

        //WIZARD STEP 7.06A: PolicyCityGroupItemPopup - Returns PartialView
        public ActionResult PolicyCityGroupItemPopup(int policyCityGroupItemId)
        {

            PolicyCityGroupItemRepository policyCityGroupItemRepository = new PolicyCityGroupItemRepository();
            PolicyCityGroupItem policyCityGroupItem = new PolicyCityGroupItem();
            policyCityGroupItem.EnabledFlag = true;

            if (policyCityGroupItemId != 0)
            {
                policyCityGroupItem = policyCityGroupItemRepository.GetPolicyCityGroupItem(policyCityGroupItemId);
            }

            PolicyCityGroupItemVM policyCityGroupItemVM = new PolicyCityGroupItemVM();
            policyCityGroupItemVM.PolicyCityGroupItem = policyCityGroupItem;

            //Populate List of PolicyCityStatuses
            PolicyCityStatusRepository policyCityStatusRepository = new PolicyCityStatusRepository();
            policyCityGroupItemVM.PolicyCityStatuses = new SelectList(policyCityStatusRepository.GetAllPolicyCityStatuses().ToList(), "PolicyCityStatusId", "PolicyCityStatusDescription", policyCityGroupItem.PolicyCityStatusId);

            return PartialView("PolicyCityGroupItemPopup", policyCityGroupItemVM);
        }

        //WIZARD STEP 7.06B: Validate PolicyCityGroupItem (no save)- Returns JSON
        [HttpPost]
        public ActionResult PolicyCityGroupItemValidation(PolicyCityGroupItemVM policyCityGroupItemVM)
        {
            //Validate data against Tables
             try
            {
                UpdateModel<PolicyCityGroupItem>(policyCityGroupItemVM.PolicyCityGroupItem, "PolicyCityGroupItem");
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

               // PolicyCityGroupItemVM policyCityGroupItemVM = new PolicyCityGroupItemVM();
                
                if (policyCityGroupItemVM.PolicyCityGroupItem.CityCode != null)
                {
                    CityRepository cityRepository = new CityRepository();
                    City city = new City();
                    city = cityRepository.GetCity(policyCityGroupItemVM.PolicyCityGroupItem.CityCode);
                    policyCityGroupItemVM.PolicyCityGroupItem.City = city;
                }
                //policyCityGroupItemVM.PolicyCityGroupItem = policyCityGroupItem;

                //Populate List of PolicyCityStatuses
                PolicyCityStatusRepository policyCityStatusRepository = new PolicyCityStatusRepository();
                policyCityGroupItemVM.PolicyCityStatuses = new SelectList(policyCityStatusRepository.GetAllPolicyCityStatuses().ToList(), "PolicyCityStatusId", "PolicyCityStatusDescription", policyCityGroupItemVM.PolicyCityGroupItem.PolicyCityStatusId);

                //Validation Error - return to Form
                return Json(new WizardJSONResponse
                {
                    html = ControllerExtension.RenderPartialViewToString(this, "PolicyCityGroupItemPopup", policyCityGroupItemVM),
                    message = "ValidationError (" + n + ")",
                    success = false
                });
            }

            //No Errors - return to Main Screen
            return Json(new WizardJSONResponse
            {
                html = "",
                message = "success",
                success = true
            });
        }

        //WIZARD STEP 7.07A: PolicyCountryGroupItemPopup - Returns PartialView
        public ActionResult PolicyCountryGroupItemPopup(int policyCountryGroupItemId)
        {

            PolicyCountryGroupItemRepository policyCountryGroupItemRepository = new PolicyCountryGroupItemRepository();
            PolicyCountryGroupItem policyCountryGroupItem = new PolicyCountryGroupItem();
            policyCountryGroupItem.EnabledFlag = true;

            if (policyCountryGroupItemId != 0)
            {
                policyCountryGroupItem = policyCountryGroupItemRepository.GetPolicyCountryGroupItem(policyCountryGroupItemId);
                policyCountryGroupItemRepository.EditItemForDisplay(policyCountryGroupItem);
            }

            //Populate List of PolicyCountryStatuses
            PolicyCountryStatusRepository policyCountryStatusRepository = new PolicyCountryStatusRepository();
            SelectList policyCountryStatuses = new SelectList(policyCountryStatusRepository.GetAllPolicyCountryStatuses().ToList(), "PolicyCountryStatusId", "PolicyCountryStatusDescription");
            ViewData["PolicyCountryStatusList"] = policyCountryStatuses;

            return PartialView("PolicyCountryGroupItemPopup", policyCountryGroupItem);
        }

        //WIZARD STEP 7.07B: Validate PolicyCountryGroupItem (no save)- Returns JSON
        [HttpPost]
        public ActionResult PolicyCountryGroupItemValidation(PolicyCountryGroupItem policyCountryGroupItem)
        {     
            //Validate data against Tables
            if (!ModelState.IsValid)
            {
                string n = "";
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        n += error.ErrorMessage;
                    }
                }
            
                //Populate List of PolicyCountryStatuses
                PolicyCountryStatusRepository policyCountryStatusRepository = new PolicyCountryStatusRepository();
                SelectList policyCountryStatuses = new SelectList(policyCountryStatusRepository.GetAllPolicyCountryStatuses().ToList(), "PolicyCountryStatusId", "PolicyCountryStatusDescription");
                ViewData["PolicyCountryStatusList"] = policyCountryStatuses;

                //Validation Error - return to Form
                return Json(new WizardJSONResponse
                {
                    html = ControllerExtension.RenderPartialViewToString(this, "PolicyCountryGroupItemPopup", policyCountryGroupItem),
                    message = "ValidationError (" + n + ")",
                    success = false
                });
            }

            //No Errors - return to Main Screen
            return Json(new WizardJSONResponse
            {
                html = "",
                message = "success",
                success = true
            });
        }

        //WIZARD STEP 7.08A: PolicyHotelCapRateGroupItemPopup - Returns PartialView
        public ActionResult PolicyHotelCapRateGroupItemPopup(int policyHotelCapRateGroupItemId)
        {

            PolicyHotelCapRateGroupItemRepository policyHotelCapRateGroupItemRepository = new PolicyHotelCapRateGroupItemRepository();
            PolicyHotelCapRateGroupItem policyHotelCapRateGroupItem = new PolicyHotelCapRateGroupItem();
            policyHotelCapRateGroupItem.EnabledFlag = true;

            if (policyHotelCapRateGroupItemId != 0)
            {
                policyHotelCapRateGroupItem = policyHotelCapRateGroupItemRepository.GetPolicyHotelCapRateGroupItem(policyHotelCapRateGroupItemId);
                policyHotelCapRateGroupItemRepository.EditItemForDisplay(policyHotelCapRateGroupItem);
            }

            //Populate List of PolicyLocations
            PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
            SelectList policyLocations = new SelectList(policyLocationRepository.GetAllPolicyLocations().ToList(), "PolicyLocationId", "PolicyLocationName");
            ViewData["PolicyLocationList"] = policyLocations;

            //Populate List of currencies 
            CurrencyRepository currencyRepository = new CurrencyRepository();
            SelectList currencies = new SelectList(currencyRepository.GetAllCurrencies().ToList(), "CurrencyCode", "Name");
            ViewData["CurrencyList"] = currencies;

            return PartialView("PolicyHotelCapRateGroupItemPopup", policyHotelCapRateGroupItem);
        }

        //WIZARD STEP 7.08B: Validate PolicyHotelCapRateGroupItem (no save)- Returns JSON
        [HttpPost]
        public ActionResult PolicyHotelCapRateGroupItemValidation(PolicyHotelCapRateGroupItem policyHotelCapRateGroupItem)
        {
            //Validate data against Tables
            if (!ModelState.IsValid)
            {
                string n = "";
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        n += error.ErrorMessage;
                    }
                }

                //Populate List of PolicyLocations
                PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
                SelectList policyLocations = new SelectList(policyLocationRepository.GetAllPolicyLocations().ToList(), "PolicyLocationId", "PolicyLocationName");
                ViewData["PolicyLocationList"] = policyLocations;

                //Populate List of currencies 
                CurrencyRepository currencyRepository = new CurrencyRepository();
                SelectList currencies = new SelectList(currencyRepository.GetAllCurrencies().ToList(), "CurrencyCode", "Name");
                ViewData["CurrencyList"] = currencies;

                //Validation Error - return to Form
                return Json(new WizardJSONResponse
                {
                    html = ControllerExtension.RenderPartialViewToString(this, "PolicyHotelCapRateGroupItemPopup", policyHotelCapRateGroupItem),
                    message = "ValidationError (" + n + ")",
                    success = false
                });
            }

            //No Errors - return to Main Screen
            return Json(new WizardJSONResponse
            {
                html = "",
                message = "success",
                success = true
            });
        }

        //WIZARD STEP 7.09A: PolicyHotelPropertyGroupItemPopup - Returns PartialView
        public ActionResult PolicyHotelPropertyGroupItemPopup(int policyHotelPropertyGroupItemId)
        {

            PolicyHotelPropertyGroupItemRepository policyHotelPropertyGroupItemRepository = new PolicyHotelPropertyGroupItemRepository();
            PolicyHotelPropertyGroupItem policyHotelPropertyGroupItem = new PolicyHotelPropertyGroupItem();
            policyHotelPropertyGroupItem.EnabledFlag = true;

            if (policyHotelPropertyGroupItemId != 0)
            {
                policyHotelPropertyGroupItem = policyHotelPropertyGroupItemRepository.GetPolicyHotelPropertyGroupItem(policyHotelPropertyGroupItemId);
                policyHotelPropertyGroupItemRepository.EditItemForDisplay(policyHotelPropertyGroupItem);
            }

            //Populate List of PolicyHotelStatuses
            PolicyHotelStatusRepository policyHotelStatusRepository = new PolicyHotelStatusRepository();
            SelectList policyHotelStatuses = new SelectList(policyHotelStatusRepository.GetAllPolicyHotelStatuses().ToList(), "PolicyHotelStatusId", "PolicyHotelStatusDescription");
            ViewData["PolicyHotelStatusList"] = policyHotelStatuses;

            //Populate List of HarpHotels 
            HarpHotelRepository harpHotelRepository = new HarpHotelRepository();
            SelectList harpHotels = new SelectList(harpHotelRepository.GetAllHarpHotels().ToList(), "HarpHotelId", "HarpHotelName");
            ViewData["HarpHotelList"] = harpHotels;

            return PartialView("PolicyHotelPropertyGroupItemPopup", policyHotelPropertyGroupItem);
        }

        //WIZARD STEP 7.09B: Validate PolicyHotelPropertyGroupItem (no save)- Returns JSON
        [HttpPost]
        public ActionResult PolicyHotelPropertyGroupItemValidation(PolicyHotelPropertyGroupItem policyHotelPropertyGroupItem)
        {
            //Validate data against Tables
            if (!ModelState.IsValid)
            {
                string n = "";
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        n += error.ErrorMessage;
                    }
                }

                //Populate List of PolicyHotelStatuses
                PolicyHotelStatusRepository policyHotelStatusRepository = new PolicyHotelStatusRepository();
                SelectList policyHotelStatuses = new SelectList(policyHotelStatusRepository.GetAllPolicyHotelStatuses().ToList(), "PolicyHotelStatusId", "PolicyHotelStatusDescription");
                ViewData["PolicyHotelStatusList"] = policyHotelStatuses;

                //Populate List of HarpHotels 
                HarpHotelRepository harpHotelRepository = new HarpHotelRepository();
                SelectList harpHotels = new SelectList(harpHotelRepository.GetAllHarpHotels().ToList(), "HarpHotelId", "HarpHotelName");
                ViewData["HarpHotelList"] = harpHotels;

                //Validation Error - return to Form
                return Json(new WizardJSONResponse
                {
                    html = ControllerExtension.RenderPartialViewToString(this, "PolicyHotelPropertyGroupItemPopup", policyHotelPropertyGroupItem),
                    message = "ValidationError (" + n + ")",
                    success = false
                });
            }

            //No Errors - return to Main Screen
            return Json(new WizardJSONResponse
            {
                html = "",
                message = "success",
                success = true
            });
        }

        //WIZARD STEP 7.10A: PolicyHotelVendorGroupItemPopup - Returns PartialView
        public ActionResult PolicyHotelVendorGroupItemPopup(int policyHotelVendorGroupItemId)
        {

            PolicyHotelVendorGroupItemRepository policyHotelVendorGroupItemRepository = new PolicyHotelVendorGroupItemRepository();
            PolicyHotelVendorGroupItem policyHotelVendorGroupItem = new PolicyHotelVendorGroupItem();
            policyHotelVendorGroupItem.EnabledFlag = true;

            if (policyHotelVendorGroupItemId != 0)
            {
                policyHotelVendorGroupItem = policyHotelVendorGroupItemRepository.GetPolicyHotelVendorGroupItem(policyHotelVendorGroupItemId);
                policyHotelVendorGroupItemRepository.EditItemForDisplay(policyHotelVendorGroupItem);
            }

            //Populate List of PolicyLocations
            PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
            SelectList policyLocations = new SelectList(policyLocationRepository.GetAllPolicyLocations().ToList(), "PolicyLocationId", "PolicyLocationName");
            ViewData["PolicyLocationList"] = policyLocations;

            //Populate List of PolicyHotelStatuses 
            PolicyHotelStatusRepository policyHotelStatusRepository = new PolicyHotelStatusRepository();
            SelectList policyHotelStatuses = new SelectList(policyHotelStatusRepository.GetAllPolicyHotelStatuses().ToList(), "PolicyHotelStatusId", "PolicyHotelStatusDescription");
            ViewData["PolicyHotelStatusList"] = policyHotelStatuses;

            //Populate List of Products
            ProductRepository productRepository = new ProductRepository();
            SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
            ViewData["ProductList"] = products;

            return PartialView("PolicyHotelVendorGroupItemPopup", policyHotelVendorGroupItem);
        }

        //WIZARD STEP 7.10B: Validate PolicyHotelVendorGroupItem (no save)- Returns JSON
        [HttpPost]
        public ActionResult PolicyHotelVendorGroupItemValidation(PolicyHotelVendorGroupItem policyHotelVendorGroupItem)
        {            
            //Validate data against Tables
            if (!ModelState.IsValid)
            {
                string n = "";
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        n += error.ErrorMessage;
                    }
                }

                //Populate List of PolicyLocations
                PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
                SelectList policyLocations = new SelectList(policyLocationRepository.GetAllPolicyLocations().ToList(), "PolicyLocationId", "PolicyLocationName");
                ViewData["PolicyLocationList"] = policyLocations;

                //Populate List of PolicyHotelStatuses 
                PolicyHotelStatusRepository policyHotelStatusRepository = new PolicyHotelStatusRepository();
                SelectList policyHotelStatuses = new SelectList(policyHotelStatusRepository.GetAllPolicyHotelStatuses().ToList(), "PolicyHotelStatusId", "PolicyHotelStatusDescription");
                ViewData["PolicyHotelStatusList"] = policyHotelStatuses;

                //Populate List of Products
                ProductRepository productRepository = new ProductRepository();
                SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
                ViewData["ProductList"] = products;

                //Validation Error - return to Form
                return Json(new WizardJSONResponse
                {
                    html = ControllerExtension.RenderPartialViewToString(this, "PolicyHotelVendorGroupItemPopup", policyHotelVendorGroupItem),
                    message = "ValidationError (" + n + ")",
                    success = false
                });
            }

            //No Errors - return to Main Screen
            return Json(new WizardJSONResponse
            {
                html = "",
                message = "success",
                success = true
            });
        }

        //WIZARD STEP 7.11A: PolicySupplierDealCodePopup - Returns PartialView
        public ActionResult PolicySupplierDealCodePopup(int policySupplierDealCodeId)
        {

            PolicySupplierDealCodeRepository policySupplierDealCodeRepository = new PolicySupplierDealCodeRepository();
            PolicySupplierDealCode policySupplierDealCode = new PolicySupplierDealCode();
            policySupplierDealCode.EnabledFlagNonNullable = true;

            if (policySupplierDealCodeId != 0)
            {
                policySupplierDealCode = policySupplierDealCodeRepository.GetPolicySupplierDealCode(policySupplierDealCodeId);
                policySupplierDealCodeRepository.EditItemForDisplay(policySupplierDealCode);
            }

            GDSRepository gdsRepository = new GDSRepository();
            SelectList gdss = new SelectList(gdsRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName");
            ViewData["GDSList"] = gdss;

            //Populate List of PolicySupplierDealCodeTypes
            PolicySupplierDealCodeTypeRepository policySupplierDealCodeType = new PolicySupplierDealCodeTypeRepository();
            SelectList policySupplierDealCodeTypes = new SelectList(policySupplierDealCodeType.GetAllPolicySupplierDealCodeTypes().ToList(), "PolicySupplierDealCodeTypeId", "PolicySupplierDealCodeTypeDescription");
            ViewData["PolicySupplierDealCodeTypeList"] = policySupplierDealCodeTypes;

            //Populate List of Products
            ProductRepository productRepository = new ProductRepository();
            SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
            ViewData["ProductList"] = products;

            //Populate List of Policy Locations
            PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
            SelectList policyLocations = new SelectList(policyLocationRepository.GetAllPolicyLocations().ToList(), "PolicyLocationId", "PolicyLocationName");
            ViewData["PolicyLocationList"] = policyLocations;


            return PartialView("PolicySupplierDealCodePopup", policySupplierDealCode);
        }

        //WIZARD STEP 7.11B: Validate PolicySupplierDealCode (no save)- Returns JSON
        [HttpPost]
        public ActionResult PolicySupplierDealCodeValidation(PolicySupplierDealCode policySupplierDealCode)
        {

            //PolicySupplierDealCodeRepository policySupplierDealCodeRepository = new PolicySupplierDealCodeRepository();
            //policySupplierDealCodeRepository.EditItemForDisplay(policySupplierDealCode);

            //Validate data against Tables
            if (!ModelState.IsValid)
            {
                string n = "";
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        n += error.ErrorMessage;
                    }
                }

                GDSRepository gdsRepository = new GDSRepository();
                SelectList gdss = new SelectList(gdsRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName");
                ViewData["GDSList"] = gdss;

                //Populate List of PolicySupplierDealCodeTypes
                PolicySupplierDealCodeTypeRepository policySupplierDealCodeType = new PolicySupplierDealCodeTypeRepository();
                SelectList policySupplierDealCodeTypes = new SelectList(policySupplierDealCodeType.GetAllPolicySupplierDealCodeTypes().ToList(), "PolicySupplierDealCodeTypeId", "PolicySupplierDealCodeTypeDescription");
                ViewData["PolicySupplierDealCodeTypeList"] = policySupplierDealCodeTypes;

                //Populate List of Products
                ProductRepository productRepository = new ProductRepository();
                SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
                ViewData["ProductList"] = products;

                //Populate List of Policy Locations
                PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
                SelectList policyLocations = new SelectList(policyLocationRepository.GetAllPolicyLocations().ToList(), "PolicyLocationId", "PolicyLocationName");
                ViewData["PolicyLocationList"] = policyLocations;

           
                

                //Validation Error - return to Form
                return Json(new WizardJSONResponse
                {
                    html = ControllerExtension.RenderPartialViewToString(this, "PolicySupplierDealCodePopup", policySupplierDealCode),
                    message = "ValidationError (" + n + ")",
                    success = false
                });
            }

            //No Errors - return to Main Screen
            return Json(new WizardJSONResponse
            {
                html = "",
                message = "success",
                success = true
            });


        }

        //WIZARD STEP 7.12A: PolicySupplierServiceInformationPopup - Returns PartialView
        public ActionResult PolicySupplierServiceInformationPopup(int policySupplierServiceInformationId)
        {
            PolicySupplierServiceInformationRepository policySupplierServiceInformationRepository = new PolicySupplierServiceInformationRepository();
            PolicySupplierServiceInformation policySupplierServiceInformation = new PolicySupplierServiceInformation();
            policySupplierServiceInformation.EnabledFlagNonNullable = true;

            if (policySupplierServiceInformationId != 0)
            {
                policySupplierServiceInformation = policySupplierServiceInformationRepository.GetPolicySupplierServiceInformation(policySupplierServiceInformationId);
                policySupplierServiceInformationRepository.EditItemForDisplay(policySupplierServiceInformation);
            }

            //Populate List of policySupplierServiceInformationTypeRepositorys
            PolicySupplierServiceInformationTypeRepository policySupplierServiceInformationTypeRepository = new PolicySupplierServiceInformationTypeRepository();
            SelectList policySupplierServiceInformations = new SelectList(policySupplierServiceInformationTypeRepository.GetAllPolicySupplierServiceInformationTypes().ToList(), "policySupplierServiceInformationTypeId", "policySupplierServiceInformationTypeDescription");
            ViewData["PolicySupplierServiceInformationList"] = policySupplierServiceInformations;

            //Populate List of Products
            ProductRepository productRepository = new ProductRepository();
            SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
            ViewData["ProductList"] = products;

            //Populate List of Policy Locations
            PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
            SelectList policyLocations = new SelectList(policyLocationRepository.GetAllPolicyLocations().ToList(), "PolicyLocationId", "PolicyLocationName");
            ViewData["PolicyLocationList"] = policyLocations;

            return PartialView("PolicySupplierServiceInformationPopup", policySupplierServiceInformation);
        }

        //WIZARD STEP 7.12B: Validate PolicySupplierServiceInformation (no save)- Returns JSON
        [HttpPost]
        public ActionResult PolicySupplierServiceInformationValidation(PolicySupplierServiceInformation policySupplierServiceInformation)
        {
            //Validate data against Tables
            if (!ModelState.IsValid)
            {
                string n = "";
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        n += error.ErrorMessage;
                    }
                }

                //Populate List of policySupplierServiceInformationTypeRepositorys
                PolicySupplierServiceInformationTypeRepository policySupplierServiceInformationTypeRepository = new PolicySupplierServiceInformationTypeRepository();
                SelectList policySupplierServiceInformations = new SelectList(policySupplierServiceInformationTypeRepository.GetAllPolicySupplierServiceInformationTypes().ToList(), "policySupplierServiceInformationTypeId", "policySupplierServiceInformationTypeDescription");
                ViewData["PolicySupplierServiceInformationList"] = policySupplierServiceInformations;

                //Populate List of Products
                ProductRepository productRepository = new ProductRepository();
                SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
                ViewData["ProductList"] = products;

                //Populate List of Policy Locations
                PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
                SelectList policyLocations = new SelectList(policyLocationRepository.GetAllPolicyLocations().ToList(), "PolicyLocationId", "PolicyLocationName");
                ViewData["PolicyLocationList"] = policyLocations;

                //Validation Error - return to Form
                return Json(new WizardJSONResponse
                {
                    html = ControllerExtension.RenderPartialViewToString(this, "PolicySupplierServiceInformationPopup", policySupplierServiceInformation),
                    message = "ValidationError (" + n + ")",
                    success = false
                });
            }

            //No Errors - return to Main Screen
            return Json(new WizardJSONResponse
            {
                html = "",
                message = "success",
                success = true
            });
        }

        //WIZARD STEP 8: Show A list of Changes made by User - Returns JSON
        [HttpPost]
        public ActionResult ConfirmChangesScreen(ClientWizardVM updatedClient)
        {
           //Model to store messages for user
            WizardMessages wizardMessages = new WizardMessages();

            //get original TopUnit+SubUnit for comparison
            ClientSubUnit originalCientSubUnit = new ClientSubUnit();
            originalCientSubUnit = clientSubUnitRepository.GetClientSubUnit(updatedClient.ClientSubUnit.ClientSubUnitGuid);
            ClientTopUnit originalClientTopUnit = new ClientTopUnit();
            originalClientTopUnit = clientTopUnitRepository.GetClientTopUnit(updatedClient.ClientTopUnit.ClientTopUnitGuid);

            //Build Model of original data for comparison
            ClientWizardVM originalClient = new ClientWizardVM();
            originalClient.ClientSubUnit = originalCientSubUnit;
            originalClient.ClientTopUnit = originalClientTopUnit;

            if (updatedClient.PolicyGroup != null)
            {
                PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
                originalClient.PolicyGroup = policyGroupRepository.GetGroup(updatedClient.PolicyGroup.PolicyGroupId);
            }

            //Build list of Messages
            clientWizardRepository.BuildClientChangeMessages(wizardMessages, originalClient, updatedClient);

            //Return Response to User
            return Json(new WizardJSONResponse
            {
                html = ControllerExtension.RenderPartialViewToString(this, "ConfirmChangesScreen", wizardMessages),
                message = "Success",
                success = true
            });
        }

        [HttpPost]
        public ActionResult CommitClientDetails(ClientWizardVM updatedClient)
        {
            WizardMessages wizardMessages = new WizardMessages();

            ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
            ClientSubUnit originalCientSubUnit = new ClientSubUnit();
            originalCientSubUnit = clientSubUnitRepository.GetClientSubUnit(updatedClient.ClientSubUnit.ClientSubUnitGuid);

            ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
            ClientTopUnit originalClientTopUnit = new ClientTopUnit();
            originalClientTopUnit = clientTopUnitRepository.GetClientTopUnit(updatedClient.ClientTopUnit.ClientTopUnitGuid);

            //we may need this for ServicingOptionGroup naming
            //it s not edited in the wizard so we take it from the original version
            updatedClient.ClientSubUnit.ClientSubUnitName = originalCientSubUnit.ClientSubUnitName;

            //ClientSubUnit Changes
            bool telephonyChanges = false;
            if (updatedClient.ClientSubUnitTelephoniesAdded != null && updatedClient.ClientSubUnitTelephoniesAdded.Count > 0)
            {
                telephonyChanges = true;
            }
            if (updatedClient.ClientSubUnitTelephoniesRemoved != null && updatedClient.ClientSubUnitTelephoniesRemoved.Count > 0)
            {
                telephonyChanges = true;
            }

            if ((originalCientSubUnit.PortraitStatusId != updatedClient.ClientSubUnit.PortraitStatusId) ||
                (originalCientSubUnit.PortraitStatusDescription != updatedClient.ClientSubUnit.PortraitStatusDescription) ||
                (originalCientSubUnit.ClientBusinessDescription != updatedClient.ClientSubUnit.ClientBusinessDescription) ||
                (originalCientSubUnit.LineOfBusinessId != updatedClient.ClientSubUnit.LineOfBusinessId) ||
                (originalCientSubUnit.ClientSubUnitDisplayName != updatedClient.ClientSubUnit.ClientSubUnitDisplayName) ||
                (originalCientSubUnit.RestrictedClient != updatedClient.ClientSubUnit.RestrictedClient) ||
                (originalCientSubUnit.PrivateClient != updatedClient.ClientSubUnit.PrivateClient) ||
                (originalCientSubUnit.CubaBookingAllowed != updatedClient.ClientSubUnit.CubaBookingAllowed) ||
                (originalCientSubUnit.InCountryServiceOnly != updatedClient.ClientSubUnit.InCountryServiceOnly) ||
                (originalCientSubUnit.DialledNumber24HSC != updatedClient.ClientSubUnit.DialledNumber24HSC) ||
                (originalCientSubUnit.BranchContactNumber != updatedClient.ClientSubUnit.BranchContactNumber) ||
                (originalCientSubUnit.BranchFaxNumber != updatedClient.ClientSubUnit.BranchFaxNumber) ||
                (originalCientSubUnit.BranchEmail != updatedClient.ClientSubUnit.BranchEmail) ||
                (originalCientSubUnit.ClientIATA != updatedClient.ClientSubUnit.ClientIATA) ||

                 telephonyChanges)
            {
                try
                {
                    clientWizardRepository.UpdateClientSubUnit(updatedClient.ClientSubUnit);
                    wizardMessages.AddMessage("ClientSubUnit successfully updated", true);
                }
                catch (SqlException ex)
                {
                    if (ex.Message == "SQLVersioningError")
                    {
                        wizardMessages.AddMessage("ClientSubUnit has been modified by another user, PortraitStatus and/or DisplayName were not updated", false);
                    }
                    else
                    {
                        wizardMessages.AddMessage("Error updating ClientSubUnit, please see Log for details", false);
                        LogRepository logRepository = new LogRepository();
                        logRepository.LogError(ex.Message);
                    }

                    return Json(new
                    {
                        message = "Error",
                        success = false,
                        errorMessages = wizardMessages
                    });
                }
            }

            //Telephonies
            try
            {
                wizardMessages = clientWizardRepository.UpdateClientSubUnitTelephonies(updatedClient, wizardMessages);

            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                wizardMessages.AddMessage("Telephonies were not changed, please check Log for details", false);
                wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);

                return Json(new
                {
                    message = "Error",
                    success = false,
                    errorMessages = wizardMessages
                });
            }

            return Json(new
            {
                message = "Success",
                success = true
            });
        }

        [HttpPost]
        public ActionResult CommitTeamsChanges(ClientWizardVM updatedClient)
        {
            WizardMessages wizardMessages = new WizardMessages();

            //Teams
            try
            {
                wizardMessages = clientWizardRepository.UpdateClientSubUnitTeams(updatedClient, wizardMessages);

            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                wizardMessages.AddMessage("Teams were not changed, please check Event Log for details", false);
                wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);


                return Json(new
                {
                    message = "Error",
                    success = false,
                    errorMessages = wizardMessages
                });
            }

            return Json(new
            {
                message = "Success",
                success = true
            });
        }

        [HttpPost]
        public ActionResult CommitClientAccountsChanges(ClientWizardVM updatedClient)
        {
            WizardMessages wizardMessages = new WizardMessages();

            //ClientAccounts
            try
            {
                wizardMessages = clientWizardRepository.UpdateClientSubUnitClientAccounts(updatedClient, wizardMessages);

            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                wizardMessages.AddMessage("Accounts were not changed, please check Event Log for details", false);
                wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);

                return Json(new
                {
                    message = "Error",
                    success = false,
                    errorMessages = wizardMessages
                });
            }

            return Json(new
            {
                message = "Success",
                success = true
            });
        }

        [HttpPost]
        public ActionResult CommitClientServicesChanges(ClientWizardVM updatedClient)
        {
            WizardMessages wizardMessages = new WizardMessages();

            ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
            ClientSubUnit originalCientSubUnit = new ClientSubUnit();
            originalCientSubUnit = clientSubUnitRepository.GetClientSubUnit(updatedClient.ClientSubUnit.ClientSubUnitGuid);

            ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
            ClientTopUnit originalClientTopUnit = new ClientTopUnit();
            originalClientTopUnit = clientTopUnitRepository.GetClientTopUnit(updatedClient.ClientTopUnit.ClientTopUnitGuid);

            //we may need this for ServicingOptionGroup naming
            //it s not edited in the wizard so we take it from the original version
            updatedClient.ClientSubUnit.ClientSubUnitName = originalCientSubUnit.ClientSubUnitName;

            //ServicingOptions
            if (hierarchyRepository.AdminHasDomainHierarchyWriteAccess("ClientSubUnit", originalCientSubUnit.ClientSubUnitGuid, "", "Servicing Option"))
            {
                try
                {
                    wizardMessages = clientWizardRepository.UpdateClientSubUnitServicingOptions(updatedClient, wizardMessages);

                }
                catch (SqlException ex)
                {
                    LogRepository logRepository = new LogRepository();
                    logRepository.LogError(ex.Message);

                    wizardMessages.AddMessage("Servicing Options were not changed, please check Event Log for details", false);
                    wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);

                    return Json(new
                    {
                        message = "Error",
                        success = false,
                        errorMessages = wizardMessages
                    });
                }
            }

            return Json(new
            {
                message = "Success",
                success = true
            });
        }

        [HttpPost]
        public ActionResult CommitReasonCodeChanges(ClientWizardVM updatedClient)
        {
            WizardMessages wizardMessages = new WizardMessages();

            ClientSubUnit originalCientSubUnit = new ClientSubUnit();
            originalCientSubUnit = clientSubUnitRepository.GetClientSubUnit(updatedClient.ClientSubUnit.ClientSubUnitGuid);

            //ReasonCodes
            if (hierarchyRepository.AdminHasDomainHierarchyWriteAccess("ClientSubUnit", originalCientSubUnit.ClientSubUnitGuid, "", "Reason Code"))
            {
                try
                {
                    wizardMessages = clientWizardRepository.UpdateClientSubUnitReasonCodes(updatedClient, wizardMessages);

                }
                catch (SqlException ex)
                {
                    LogRepository logRepository = new LogRepository();
                    logRepository.LogError(ex.Message);

                    wizardMessages.AddMessage("ReasonCodes were not changed, please check Event Log for details", false);
                    wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);

                    return Json(new
                    {
                        message = "Error",
                        success = false,
                        errorMessages = wizardMessages
                    });
                }
            }

            return Json(new
            {
                message = "Success",
                success = true
            });
        }

        [HttpPost]
        public ActionResult CommitPolicyChanges(ClientWizardVM updatedClient)
        {
            WizardMessages wizardMessages = new WizardMessages();
            ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();

            ClientSubUnit originalCientSubUnit = new ClientSubUnit();
            originalCientSubUnit = clientSubUnitRepository.GetClientSubUnit(updatedClient.ClientSubUnit.ClientSubUnitGuid);

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = updatedClient.PolicyGroup;
            if (updatedClient.PolicyGroup != null)
            {

                //Check Access Rights to ClientSubUnit's Policies
                if (hierarchyRepository.AdminHasDomainHierarchyWriteAccess("ClientSubUnit", originalCientSubUnit.ClientSubUnitGuid, "", "Policy Hierarchy"))
                {
                    //PolicyGroup
                    try
                    {
                        wizardMessages = clientWizardRepository.UpdateClientSubUnitPolicyGroup(updatedClient, wizardMessages);

                    }
                    catch (SqlException ex)
                    {
                        //Versioning Error
                        if (ex.Message == "SQLVersioningError")
                        {
                            wizardMessages.AddMessage("Policy Group has been modified or deleted by another user, Inheritance was not updated", false);
                        }
                        else //Other Error
                        {
                            LogRepository logRepository = new LogRepository();
                            logRepository.LogError(ex.Message);

                            wizardMessages.AddMessage("Policy Group Inheritance was not updated, please check Event Log for details", false);
                            wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
                        }

                        return Json(new
                        {
                            message = "Error",
                            success = false,
                            errorMessages = wizardMessages
                        });

                    }

                    //Need to get PolicyGroupId as it may have just been created
                    //PolicyGroup policyGroup = new PolicyGroup();
                    policyGroup = clientWizardRepository.GetClientSubUnitPolicyGroup(updatedClient.ClientSubUnit.ClientSubUnitGuid);
                    PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
                    policyGroupRepository.EditGroupForDisplay(policyGroup);
                    updatedClient.PolicyGroup = policyGroup;

                    //Policies - PolicyAirParameterGroupItem
                    try
                    {
                        wizardMessages = clientWizardRepository.UpdateClientSubUnitPolicyAirParameterGroupItems(updatedClient, wizardMessages);

                    }
                    catch (SqlException ex)
                    {
                        LogRepository logRepository = new LogRepository();
                        logRepository.LogError(ex.Message);

                        wizardMessages.AddMessage("Items were not changed, please check Event Log for details", false);
                        wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);

                        return Json(new
                        {
                            message = "Error",
                            success = false,
                            errorMessages = wizardMessages
                        });
                    }

                    //Policies - PolicyAirCabinGroupItem
                    try
                    {
                        wizardMessages = clientWizardRepository.UpdateClientSubUnitPolicyAirCabinGroupItems(updatedClient, wizardMessages);

                    }
                    catch (SqlException ex)
                    {
                        LogRepository logRepository = new LogRepository();
                        logRepository.LogError(ex.Message);

                        wizardMessages.AddMessage("Policy Air Cabin Group Items were not changed, please check Event Log for details", false);
                        wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);

                        return Json(new
                        {
                            message = "Error",
                            success = false,
                            errorMessages = wizardMessages
                        });
                    }

                    //Policies - PolicyAirMissedSavingsThreshold
                    try
                    {
                        wizardMessages = clientWizardRepository.UpdateClientSubUnitPolicyAirMissedSavingsThresholdGroupItems(updatedClient, wizardMessages);

                    }
                    catch (SqlException ex)
                    {
                        LogRepository logRepository = new LogRepository();
                        logRepository.LogError(ex.Message);

                        wizardMessages.AddMessage("Policy Air Missed Savings Threshold Group Items were not changed, please check Event Log for details", false);
                        wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);

                        return Json(new
                        {
                            message = "Error",
                            success = false,
                            errorMessages = wizardMessages
                        });
                    }

                    //Policies - PolicyAirVendorGroupItem
                    try
                    {
                        wizardMessages = clientWizardRepository.UpdateClientSubUnitPolicyAirVendorGroupItems(updatedClient, wizardMessages);

                    }
                    catch (SqlException ex)
                    {
                        LogRepository logRepository = new LogRepository();
                        logRepository.LogError(ex.Message);

                        wizardMessages.AddMessage("Policy Air Vendor Group Items were not changed, please check Event Log for details", false);
                        wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);

                        return Json(new
                        {
                            message = "Error",
                            success = false,
                            errorMessages = wizardMessages
                        });
                    }


                    //Policies - PolicyCarTypeGroupItem
                    try
                    {
                        wizardMessages = clientWizardRepository.UpdateClientSubUnitPolicyCarTypeGroupItems(updatedClient, wizardMessages);

                    }
                    catch (SqlException ex)
                    {
                        LogRepository logRepository = new LogRepository();
                        logRepository.LogError(ex.Message);

                        wizardMessages.AddMessage("Policy Car Type Group Items were not changed, please check Event Log for details", false);
                        wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);

                        return Json(new
                        {
                            message = "Error",
                            success = false,
                            errorMessages = wizardMessages
                        });
                    }


                    //Policies - PolicyCarVendorGroupItem
                    try
                    {
                        wizardMessages = clientWizardRepository.UpdateClientSubUnitPolicyCarVendorGroupItems(updatedClient, wizardMessages);

                    }
                    catch (SqlException ex)
                    {
                        LogRepository logRepository = new LogRepository();
                        logRepository.LogError(ex.Message);

                        wizardMessages.AddMessage("Policy Car Vendor Group Items were not changed, please check Event Log for details", false);
                        wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);

                        return Json(new
                        {
                            message = "Error",
                            success = false,
                            errorMessages = wizardMessages
                        });
                    }


                    //Policies - PolicyCityGroupItem
                    try
                    {
                        wizardMessages = clientWizardRepository.UpdateClientSubUnitPolicyCityGroupItems(updatedClient, wizardMessages);

                    }
                    catch (SqlException ex)
                    {
                        LogRepository logRepository = new LogRepository();
                        logRepository.LogError(ex.Message);

                        wizardMessages.AddMessage("Policy City Group Items were not changed, please check Event Log for details", false);
                        wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);

                        return Json(new
                        {
                            message = "Error",
                            success = false,
                            errorMessages = wizardMessages
                        });
                    }

                    //Policies - PolicyCountryGroupItem
                    try
                    {
                        wizardMessages = clientWizardRepository.UpdateClientSubUnitPolicyCountryGroupItems(updatedClient, wizardMessages);

                    }
                    catch (SqlException ex)
                    {
                        LogRepository logRepository = new LogRepository();
                        logRepository.LogError(ex.Message);

                        wizardMessages.AddMessage("Policy Country Group Items were not changed, please check Event Log for details", false);
                        wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);

                        return Json(new
                        {
                            message = "Error",
                            success = false,
                            errorMessages = wizardMessages
                        });
                    }

                    //Policies - PolicyHotelCapRateGroupItem
                    try
                    {
                        wizardMessages = clientWizardRepository.UpdateClientSubUnitPolicyHotelCapRateGroupItems(updatedClient, wizardMessages);

                    }
                    catch (SqlException ex)
                    {
                        LogRepository logRepository = new LogRepository();
                        logRepository.LogError(ex.Message);

                        wizardMessages.AddMessage("Policy Hotel Cap Rate Group Items were not changed, please check Event Log for details", false);
                        wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);

                        return Json(new
                        {
                            message = "Error",
                            success = false,
                            errorMessages = wizardMessages
                        });
                    }


                    //Policies - PolicyHotelPropertyGroupItem
                    try
                    {
                        wizardMessages = clientWizardRepository.UpdateClientSubUnitPolicyHotelPropertyGroupItems(updatedClient, wizardMessages);

                    }
                    catch (SqlException ex)
                    {
                        LogRepository logRepository = new LogRepository();
                        logRepository.LogError(ex.Message);

                        wizardMessages.AddMessage("Policy Hotel Property Group Items were not changed, please check Event Log for details", false);
                        wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);

                        return Json(new
                        {
                            message = "Error",
                            success = false,
                            errorMessages = wizardMessages
                        });
                    }


                    //Policies - PolicyHotelVendorGroupItem
                    try
                    {
                        wizardMessages = clientWizardRepository.UpdateClientSubUnitPolicyHotelVendorGroupItems(updatedClient, wizardMessages);

                    }
                    catch (SqlException ex)
                    {
                        LogRepository logRepository = new LogRepository();
                        logRepository.LogError(ex.Message);

                        wizardMessages.AddMessage("Policy Hotel Vendor Group Items were not changed, please check Event Log for details", false);
                        wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);

                        return Json(new
                        {
                            message = "Error",
                            success = false,
                            errorMessages = wizardMessages
                        });
                    }


                    //Policies - PolicySupplierDealCode
                    try
                    {
                        wizardMessages = clientWizardRepository.UpdateClientSubUnitPolicySupplierDealCodes(updatedClient, wizardMessages);

                    }
                    catch (SqlException ex)
                    {
                        LogRepository logRepository = new LogRepository();
                        logRepository.LogError(ex.Message);

                        wizardMessages.AddMessage("Policy Supplier Deal Codes were not changed, please check Event Log for details", false);
                        wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);

                        return Json(new
                        {
                            message = "Error",
                            success = false,
                            errorMessages = wizardMessages
                        });
                    }


                    //Policies - PolicySupplierServiceInformation
                    try
                    {
                        wizardMessages = clientWizardRepository.UpdateClientSubUnitPolicySupplierServiceInformations(updatedClient, wizardMessages);

                    }
                    catch (SqlException ex)
                    {
                        LogRepository logRepository = new LogRepository();
                        logRepository.LogError(ex.Message);

                        wizardMessages.AddMessage("Policy Supplier Service Information items were not changed, please check Event Log for details", false);
                        wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);

                        return Json(new
                        {
                            message = "Error",
                            success = false,
                            errorMessages = wizardMessages
                        });
                    }
                }
            }

            return Json(new
            {
                message = "Success",
                success = true
            });
        }

        //WIZARD STEP 9: On COMPLETION - Commit Changes to the Database - Returns JSON
        [HttpPost]
        public ActionResult CommitChanges(ClientWizardVM updatedClient)
        {
            WizardMessages wizardMessages = new WizardMessages();

            ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
            ClientSubUnit originalCientSubUnit = new ClientSubUnit();
            originalCientSubUnit = clientSubUnitRepository.GetClientSubUnit(updatedClient.ClientSubUnit.ClientSubUnitGuid);

            ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
            ClientTopUnit originalClientTopUnit = new ClientTopUnit();
            originalClientTopUnit = clientTopUnitRepository.GetClientTopUnit(updatedClient.ClientTopUnit.ClientTopUnitGuid);

            //we may need this for ServicingOptionGroup naming
            //it s not edited in the wizard so we take it from the original version
            updatedClient.ClientSubUnit.ClientSubUnitName = originalCientSubUnit.ClientSubUnitName;

            //ClientSubUnit Changes
            bool telephonyChanges = false;
            if (updatedClient.ClientSubUnitTelephoniesAdded != null && updatedClient.ClientSubUnitTelephoniesAdded.Count > 0)
            {
                telephonyChanges = true;
            }
            if (updatedClient.ClientSubUnitTelephoniesRemoved != null && updatedClient.ClientSubUnitTelephoniesRemoved.Count > 0)
            {
                telephonyChanges = true;
            }

			if ((originalCientSubUnit.PortraitStatusId != updatedClient.ClientSubUnit.PortraitStatusId) ||
                (originalCientSubUnit.PortraitStatusDescription != updatedClient.ClientSubUnit.PortraitStatusDescription) ||
                (originalCientSubUnit.ClientBusinessDescription != updatedClient.ClientSubUnit.ClientBusinessDescription) ||
                (originalCientSubUnit.LineOfBusinessId != updatedClient.ClientSubUnit.LineOfBusinessId) ||
				(originalCientSubUnit.ClientSubUnitDisplayName != updatedClient.ClientSubUnit.ClientSubUnitDisplayName) ||
				(originalCientSubUnit.RestrictedClient != updatedClient.ClientSubUnit.RestrictedClient) ||
				(originalCientSubUnit.PrivateClient != updatedClient.ClientSubUnit.PrivateClient) ||
				(originalCientSubUnit.CubaBookingAllowed != updatedClient.ClientSubUnit.CubaBookingAllowed) ||
				(originalCientSubUnit.InCountryServiceOnly != updatedClient.ClientSubUnit.InCountryServiceOnly) ||
				(originalCientSubUnit.DialledNumber24HSC != updatedClient.ClientSubUnit.DialledNumber24HSC) ||
				(originalCientSubUnit.BranchContactNumber != updatedClient.ClientSubUnit.BranchContactNumber) ||
				(originalCientSubUnit.BranchFaxNumber != updatedClient.ClientSubUnit.BranchFaxNumber) ||
				(originalCientSubUnit.BranchEmail != updatedClient.ClientSubUnit.BranchEmail) ||
				(originalCientSubUnit.ClientIATA != updatedClient.ClientSubUnit.ClientIATA) ||

                 telephonyChanges)
            {
                try
                {
                    clientWizardRepository.UpdateClientSubUnit(updatedClient.ClientSubUnit);
                    wizardMessages.AddMessage("ClientSubUnit successfully updated", true);
                }
                catch (SqlException ex)
                {
                    if (ex.Message == "SQLVersioningError")
                    {
                        wizardMessages.AddMessage("ClientSubUnit has been modified by another user, PortraitStatus and/or DisplayName were not updated", false);
                    }
                    else
                    {
                        wizardMessages.AddMessage("Error updating ClientSubUnit, please see Log for details", false);
                        LogRepository logRepository = new LogRepository();
                        logRepository.LogError(ex.Message);
                    }
                }
            }

            //Telephonies
            try
            {
                wizardMessages = clientWizardRepository.UpdateClientSubUnitTelephonies(updatedClient, wizardMessages);

            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                wizardMessages.AddMessage("Telephonies were not changed, please check Log for details", false);
                wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
            }

            //Teams
            try
            {
                wizardMessages = clientWizardRepository.UpdateClientSubUnitTeams(updatedClient, wizardMessages);

            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                wizardMessages.AddMessage("Teams were not changed, please check Event Log for details", false);
                wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
            }

            //ClientAccounts
            try
            {
                wizardMessages = clientWizardRepository.UpdateClientSubUnitClientAccounts(updatedClient, wizardMessages);

            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                wizardMessages.AddMessage("Accounts were not changed, please check Event Log for details", false);
                wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
            }

            //ServicingOptions
            if (hierarchyRepository.AdminHasDomainHierarchyWriteAccess("ClientSubUnit", originalCientSubUnit.ClientSubUnitGuid, "", "Servicing Option"))
            {
                try
                {
                    wizardMessages = clientWizardRepository.UpdateClientSubUnitServicingOptions(updatedClient, wizardMessages);

                }
                catch (SqlException ex)
                {
                    LogRepository logRepository = new LogRepository();
                    logRepository.LogError(ex.Message);

                    wizardMessages.AddMessage("Servicing Options were not changed, please check Event Log for details", false);
                    wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
                }
            }

            //ReasonCodes
            if (hierarchyRepository.AdminHasDomainHierarchyWriteAccess("ClientSubUnit", originalCientSubUnit.ClientSubUnitGuid, "", "Reason Code"))
            {
                try
                {
                    wizardMessages = clientWizardRepository.UpdateClientSubUnitReasonCodes(updatedClient, wizardMessages);

                }
                catch (SqlException ex)
                {
                    LogRepository logRepository = new LogRepository();
                    logRepository.LogError(ex.Message);

                    wizardMessages.AddMessage("ReasonCodes were not changed, please check Event Log for details", false);
                    wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
                }
            }
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = updatedClient.PolicyGroup;
            if (updatedClient.PolicyGroup != null)
            {

                //Check Access Rights to ClientSubUnit's Policies
                if (hierarchyRepository.AdminHasDomainHierarchyWriteAccess("ClientSubUnit", originalCientSubUnit.ClientSubUnitGuid, "", "Policy Hierarchy"))
                {
                    //PolicyGroup
                    try
                    {
                        wizardMessages = clientWizardRepository.UpdateClientSubUnitPolicyGroup(updatedClient, wizardMessages);

                    }
                    catch (SqlException ex)
                    {
                        //Versioning Error
                        if (ex.Message == "SQLVersioningError")
                        {
                            wizardMessages.AddMessage("Policy Group has been modified or deleted by another user, Inheritance was not updated", false);
                        }
                        else //Other Error
                        {
                            LogRepository logRepository = new LogRepository();
                            logRepository.LogError(ex.Message);

                            wizardMessages.AddMessage("Policy Group Inheritance was not updated, please check Event Log for details", false);
                            wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
                        }

                    }

                    //Need to get PolicyGroupId as it may have just been created
                    //PolicyGroup policyGroup = new PolicyGroup();
                    policyGroup = clientWizardRepository.GetClientSubUnitPolicyGroup(updatedClient.ClientSubUnit.ClientSubUnitGuid);
                    PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
                    policyGroupRepository.EditGroupForDisplay(policyGroup);
                    updatedClient.PolicyGroup = policyGroup;

					//Policies - PolicyAirParameterGroupItem
					try
					{
						wizardMessages = clientWizardRepository.UpdateClientSubUnitPolicyAirParameterGroupItems(updatedClient, wizardMessages);

					}
					catch (SqlException ex)
					{
						LogRepository logRepository = new LogRepository();
						logRepository.LogError(ex.Message);

						wizardMessages.AddMessage("Items were not changed, please check Event Log for details", false);
						wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
					}

					//Policies - PolicyAirCabinGroupItem
					try
					{
						wizardMessages = clientWizardRepository.UpdateClientSubUnitPolicyAirCabinGroupItems(updatedClient, wizardMessages);

					}
					catch (SqlException ex)
					{
						LogRepository logRepository = new LogRepository();
						logRepository.LogError(ex.Message);

						wizardMessages.AddMessage("Policy Air Cabin Group Items were not changed, please check Event Log for details", false);
						wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
					}

                    //Policies - PolicyAirMissedSavingsThreshold
                    try
                    {
                        wizardMessages = clientWizardRepository.UpdateClientSubUnitPolicyAirMissedSavingsThresholdGroupItems(updatedClient, wizardMessages);

                    }
                    catch (SqlException ex)
                    {
                        LogRepository logRepository = new LogRepository();
                        logRepository.LogError(ex.Message);

                        wizardMessages.AddMessage("Policy Air Missed Savings Threshold Group Items were not changed, please check Event Log for details", false);
                        wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
                    }

                    //Policies - PolicyAirVendorGroupItem
                    try
                    {
                        wizardMessages = clientWizardRepository.UpdateClientSubUnitPolicyAirVendorGroupItems(updatedClient, wizardMessages);

                    }
                    catch (SqlException ex)
                    {
                        LogRepository logRepository = new LogRepository();
                        logRepository.LogError(ex.Message);

                        wizardMessages.AddMessage("Policy Air Vendor Group Items were not changed, please check Event Log for details", false);
                        wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
                    }


                    //Policies - PolicyCarTypeGroupItem
                    try
                    {
                        wizardMessages = clientWizardRepository.UpdateClientSubUnitPolicyCarTypeGroupItems(updatedClient, wizardMessages);

                    }
                    catch (SqlException ex)
                    {
                        LogRepository logRepository = new LogRepository();
                        logRepository.LogError(ex.Message);

                        wizardMessages.AddMessage("Policy Car Type Group Items were not changed, please check Event Log for details", false);
                        wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
                    }


                    //Policies - PolicyCarVendorGroupItem
                    try
                    {
                        wizardMessages = clientWizardRepository.UpdateClientSubUnitPolicyCarVendorGroupItems(updatedClient, wizardMessages);

                    }
                    catch (SqlException ex)
                    {
                        LogRepository logRepository = new LogRepository();
                        logRepository.LogError(ex.Message);

                        wizardMessages.AddMessage("Policy Car Vendor Group Items were not changed, please check Event Log for details", false);
                        wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
                    }


                    //Policies - PolicyCityGroupItem
                    try
                    {
                        wizardMessages = clientWizardRepository.UpdateClientSubUnitPolicyCityGroupItems(updatedClient, wizardMessages);

                    }
                    catch (SqlException ex)
                    {
                        LogRepository logRepository = new LogRepository();
                        logRepository.LogError(ex.Message);

                        wizardMessages.AddMessage("Policy City Group Items were not changed, please check Event Log for details", false);
                        wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
                    }

                    //Policies - PolicyCountryGroupItem
                    try
                    {
                        wizardMessages = clientWizardRepository.UpdateClientSubUnitPolicyCountryGroupItems(updatedClient, wizardMessages);

                    }
                    catch (SqlException ex)
                    {
                        LogRepository logRepository = new LogRepository();
                        logRepository.LogError(ex.Message);

                        wizardMessages.AddMessage("Policy Country Group Items were not changed, please check Event Log for details", false);
                        wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
                    }

                    //Policies - PolicyHotelCapRateGroupItem
                    try
                    {
                        wizardMessages = clientWizardRepository.UpdateClientSubUnitPolicyHotelCapRateGroupItems(updatedClient, wizardMessages);

                    }
                    catch (SqlException ex)
                    {
                        LogRepository logRepository = new LogRepository();
                        logRepository.LogError(ex.Message);

                        wizardMessages.AddMessage("Policy Hotel Cap Rate Group Items were not changed, please check Event Log for details", false);
                        wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
                    }


                    //Policies - PolicyHotelPropertyGroupItem
                    try
                    {
                        wizardMessages = clientWizardRepository.UpdateClientSubUnitPolicyHotelPropertyGroupItems(updatedClient, wizardMessages);

                    }
                    catch (SqlException ex)
                    {
                        LogRepository logRepository = new LogRepository();
                        logRepository.LogError(ex.Message);

                        wizardMessages.AddMessage("Policy Hotel Property Group Items were not changed, please check Event Log for details", false);
                        wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
                    }


                    //Policies - PolicyHotelVendorGroupItem
                    try
                    {
                        wizardMessages = clientWizardRepository.UpdateClientSubUnitPolicyHotelVendorGroupItems(updatedClient, wizardMessages);

                    }
                    catch (SqlException ex)
                    {
                        LogRepository logRepository = new LogRepository();
                        logRepository.LogError(ex.Message);

                        wizardMessages.AddMessage("Policy Hotel Vendor Group Items were not changed, please check Event Log for details", false);
                        wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
                    }


                    //Policies - PolicySupplierDealCode
                    try
                    {
                        wizardMessages = clientWizardRepository.UpdateClientSubUnitPolicySupplierDealCodes(updatedClient, wizardMessages);

                    }
                    catch (SqlException ex)
                    {
                        LogRepository logRepository = new LogRepository();
                        logRepository.LogError(ex.Message);

                        wizardMessages.AddMessage("Policy Supplier Deal Codes were not changed, please check Event Log for details", false);
                        wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
                    }


                    //Policies - PolicySupplierServiceInformation
                    try
                    {
                        wizardMessages = clientWizardRepository.UpdateClientSubUnitPolicySupplierServiceInformations(updatedClient, wizardMessages);

                    }
                    catch (SqlException ex)
                    {
                        LogRepository logRepository = new LogRepository();
                        logRepository.LogError(ex.Message);

                        wizardMessages.AddMessage("Policy Supplier Service Information items were not changed, please check Event Log for details", false);
                        wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
                    }
                }
            }
            
             return Json(new
             {
                 html = ControllerExtension.RenderPartialViewToString(this, "FinishedScreen", wizardMessages),
                 message = "Success",
                 success = true
             });
                
        }
    }
}
