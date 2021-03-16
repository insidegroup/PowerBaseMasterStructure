using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using System.Web.Security;
using CWTDesktopDatabase.Helpers;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using CWTDesktopDatabase.ViewModels;
using System.Text.RegularExpressions;
using System.Data;

namespace CWTDesktopDatabase.Repository
{
	public class PriceTrackingSetupGroupRepository
    {
		private PriceTrackingDC db = new PriceTrackingDC(Settings.getConnectionString());
        private CurrencyRepository currencyRepository = new CurrencyRepository();
        PriceTrackingContactRepository priceTrackingContactRepository = new PriceTrackingContactRepository();

        //Get a Page of Price Tracking Setup Groups - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectPriceTrackingSetupGroups_v1Result> PagePriceTrackingSetupGroups(bool? deletedFlag, int page, string filter, string sortField, int sortOrder)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectPriceTrackingSetupGroups_v1(deletedFlag, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPriceTrackingSetupGroups_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one PriceTrackingSetupGroup
        public PriceTrackingSetupGroup GetPriceTrackingSetupGroup(int id)
        {
            return db.PriceTrackingSetupGroups.SingleOrDefault(c => c.PriceTrackingSetupGroupId == id);
        }

        //Add Data From Linked Tables for Display
        public void EditForDisplay(PriceTrackingSetupGroup group)
        {
            group.PriceTrackingSetupGroupName = Regex.Replace(group.PriceTrackingSetupGroupName, @"[^\w\-()*]", "-");

            //Hierarchy
            HierarchyRepository hierarchyRepository = new HierarchyRepository();

            List<fnDesktopDataAdmin_SelectPriceTrackingSetupGroupHierarchy_v1Result> hierarchy = GetGroupHierarchy(group.PriceTrackingSetupGroupId);
            if (hierarchy.Count > 0)
            {
                if (hierarchy.Count == 1)
                {
                    HierarchyGroup hierarchyGroup = hierarchyRepository.GetHierarchyGroup(
                        hierarchy[0].HierarchyType ?? "",
                        hierarchy[0].HierarchyCode ?? "",
                        hierarchy[0].HierarchyName ?? "",
                        hierarchy[0].TravelerTypeGuid ?? "",
                        hierarchy[0].TravelerTypeName ?? "",
                        hierarchy[0].SourceSystemCode ?? ""
                    );

                    if (hierarchyGroup != null)
                    {
                        group.HierarchyType = hierarchyGroup.HierarchyType;
                        group.HierarchyCode = hierarchyGroup.HierarchyCode;
                        group.HierarchyItem = hierarchyGroup.HierarchyItem;
                        group.ClientSubUnitGuid = hierarchyGroup.ClientSubUnitGuid;
                        group.ClientSubUnitName = hierarchyGroup.ClientSubUnitName;
                        group.TravelerTypeGuid = hierarchyGroup.TravelerTypeGuid;
                        group.TravelerTypeName = hierarchyGroup.TravelerTypeName;
                        group.ClientTopUnitName = hierarchyGroup.ClientTopUnitName;
                        group.SourceSystemCode = hierarchyGroup.SourceSystemCode;
                    }
                }
                else
                {
                    List<MultipleHierarchyDefinition> multipleHierarchies = new List<MultipleHierarchyDefinition>();
                    foreach (fnDesktopDataAdmin_SelectPriceTrackingSetupGroupHierarchy_v1Result item in hierarchy)
                    {
                        multipleHierarchies.Add(new MultipleHierarchyDefinition()
                        {
                            HierarchyType = item.HierarchyType,
                            HierarchyItem = item.HierarchyName,
                            HierarchyCode = item.HierarchyCode,
                            TravelerTypeGuid = item.TravelerTypeGuid,
                            SourceSystemCode = item.SourceSystemCode
                        });
                    }
                    group.ClientSubUnitsHierarchy = hierarchyRepository.GetClientSubUnitHierarchies(multipleHierarchies);

                    ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
                    group.ClientTopUnitName = clientSubUnitRepository.GetClientSubUnitClientTopUnitName(group.ClientSubUnitsHierarchy.First().ClientSubUnitGuid);
                }
            }

            if (hierarchy.Count > 1)
            {
                group.IsMultipleHierarchy = true;
                group.HierarchyType = "Multiple";
                group.HierarchyItem = "Multiple";
                group.HierarchyCode = "Multiple";
            }
            else
            {
                group.IsMultipleHierarchy = false;
            }

            //True False Defaults

            if (group.SharedPseudoCityOrOfficeIdFlag == null)
            {
                group.SharedPseudoCityOrOfficeIdFlag = false;
            }

            if (group.MidOfficeUsedForQCTicketingFlag == null)
            {
                group.MidOfficeUsedForQCTicketingFlag = true;
            }

            if (group.USGovernmentContractorFlag == null)
            {
                group.USGovernmentContractorFlag = false;
            }

            //PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeIds
            if (group.PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeIds != null && group.PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeIds.Count > 0)
            {
                foreach (PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId item in group.PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeIds)
                {
                    CommonRepository commonRepository = new CommonRepository();
                    item.SharedPseudoCityOrOfficeIdFlagList = new SelectList(commonRepository.GetTrueFalseList().ToList(), "Value", "Text", item.SharedPseudoCityOrOfficeIdFlag);
                }
            }

            //PriceTrackingSetupGroupExcludedTravelerTypes
            if (group.PriceTrackingSetupGroupExcludedTravelerTypes != null && group.PriceTrackingSetupGroupExcludedTravelerTypes.Count > 0)
            {
                foreach (PriceTrackingSetupGroupExcludedTravelerType item in group.PriceTrackingSetupGroupExcludedTravelerTypes)
                {
                    if (item.TravelerTypeGuid != null)
                    {
                        TravelerTypeRepository travelerTypeRepository = new TravelerTypeRepository();
                        TravelerType travelerType = travelerTypeRepository.GetTravelerType(item.TravelerTypeGuid);
                        if (travelerType != null)
                        {
                            item.TravelerTypeGuid = travelerType.TravelerTypeGuid ?? "";
                            item.TravelerTypeName = travelerType.TravelerTypeName ?? "";
                        }
                    }
                }
            }

            //GDS
            if (group.GDSCode != null)
            {
                GDSRepository gdsRepository = new GDSRepository();
                GDS gds = gdsRepository.GetGDS(group.GDSCode);
                if (gds != null)
                {
                    group.GDS = gds;
                }
            }

            //BackOfficeSystem
            if (group.BackOfficeSystemId != null)
            {
                BackOfficeSystemRepository backOfficeSystemRepository = new BackOfficeSystemRepository();
                BackOfficeSystem backOfficeSystem = backOfficeSystemRepository.GetBackOfficeSystem(int.Parse(group.BackOfficeSystemId.ToString()));
                if (backOfficeSystem != null)
                {
                    group.BackOfficeSystem = backOfficeSystem;
                }
            }

            //DesktopUsedType
            if (group.DesktopUsedTypeId != null)
            {
                DesktopUsedTypeRepository desktopUsedTypeRepository = new DesktopUsedTypeRepository();
                DesktopUsedType desktopUsedType = desktopUsedTypeRepository.GetDesktopUsedType(System.Convert.ToInt32(group.DesktopUsedTypeId));

                if (desktopUsedType != null)
                {
                    group.DesktopUsedType = desktopUsedType;
                }

            }




            PriceTrackingBillingModelRepository priceTrackingBillingModelRepository = new PriceTrackingBillingModelRepository();

            //AirPriceTrackingBillingModel
            if (group.AirPriceTrackingBillingModelId != null)
            {
                PriceTrackingBillingModel airPriceTrackingBillingModel = priceTrackingBillingModelRepository.GetPriceTrackingBillingModel(int.Parse(group.AirPriceTrackingBillingModelId.ToString()));
                if (airPriceTrackingBillingModel != null)
                {
                    group.AirPriceTrackingBillingModel = airPriceTrackingBillingModel;
                }
            }

            //HotelPriceTrackingBillingModel
            if (group.HotelPriceTrackingBillingModelId != null)
            {
                PriceTrackingBillingModel hotelPriceTrackingBillingModel = priceTrackingBillingModelRepository.GetPriceTrackingBillingModel(int.Parse(group.HotelPriceTrackingBillingModelId.ToString()));
                if (hotelPriceTrackingBillingModel != null)
                {
                    group.HotelPriceTrackingBillingModel = hotelPriceTrackingBillingModel;
                }
            }

            //PreTicketPriceTrackingBillingModel
            if (group.PreTicketPriceTrackingBillingModelId != null)
            {
                PriceTrackingBillingModel preTicketPriceTrackingBillingModel = priceTrackingBillingModelRepository.GetPriceTrackingBillingModel(int.Parse(group.PreTicketPriceTrackingBillingModelId.ToString()));
                if (preTicketPriceTrackingBillingModel != null)
                {
                    group.PreTicketPriceTrackingBillingModel = preTicketPriceTrackingBillingModel;
                }
            }
        }

        //Get Hierarchy Details
        public List<fnDesktopDataAdmin_SelectPriceTrackingSetupGroupHierarchy_v1Result> GetGroupHierarchy(int id)
        {
            var result = db.fnDesktopDataAdmin_SelectPriceTrackingSetupGroupHierarchy_v1(id);
            return result.ToList();
        }

        //Add to DB
        public void Add(PriceTrackingSetupGroup priceTrackingSetupGroup)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            XElement priceTrackingSetupGroupAdditionalPseudoCityOrOfficeIdsXML = GetPriceTrackingSetupGroupAdditionalPseudoCityOrOfficeIdsXML(priceTrackingSetupGroup);
            XElement priceTrackingSetupGroupExcludedTravelerTypesXML = GetPriceTrackingSetupGroupExcludedTravelerTypesXML(priceTrackingSetupGroup);

            db.spDesktopDataAdmin_InsertPriceTrackingSetupGroup_v1(
                priceTrackingSetupGroup.PriceTrackingSetupGroupName,
                priceTrackingSetupGroup.PriceTrackingSetupTypeId,
                priceTrackingSetupGroup.GDSCode,
                priceTrackingSetupGroup.PseudoCityOrOfficeId,
                priceTrackingSetupGroup.SharedPseudoCityOrOfficeIdFlag,
                priceTrackingSetupGroup.FIQID,
                priceTrackingSetupGroup.DesktopUsedTypeId,
                priceTrackingSetupGroup.PriceTrackingMidOfficePlatformId,
                priceTrackingSetupGroup.MidOfficeUsedForQCTicketingFlag,
                priceTrackingSetupGroup.PriceTrackingItinerarySolutionId,
                priceTrackingSetupGroup.PriceTrackingSystemRuleId,
                priceTrackingSetupGroup.USGovernmentContractorFlag,
                priceTrackingSetupGroup.BackOfficeSystemId,
                priceTrackingSetupGroup.OtherExclusions,
                priceTrackingSetupGroup.AirPriceTrackingBillingModelId,
                priceTrackingSetupGroup.HotelPriceTrackingBillingModelId,
                priceTrackingSetupGroup.PreTicketPriceTrackingBillingModelId,
                priceTrackingSetupGroup.HierarchyType,
                priceTrackingSetupGroup.HierarchyCode,
                priceTrackingSetupGroup.TravelerTypeGuid,
                priceTrackingSetupGroup.ClientSubUnitGuid,
                priceTrackingSetupGroup.SourceSystemCode,
                priceTrackingSetupGroupAdditionalPseudoCityOrOfficeIdsXML,
                priceTrackingSetupGroupExcludedTravelerTypesXML,
                adminUserGuid
            );
        }

		//Update in DB
		public void Update(PriceTrackingSetupGroup priceTrackingSetupGroup)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            XElement priceTrackingSetupGroupAdditionalPseudoCityOrOfficeIdsXML = GetPriceTrackingSetupGroupAdditionalPseudoCityOrOfficeIdsXML(priceTrackingSetupGroup);
            XElement priceTrackingSetupGroupExcludedTravelerTypesXML = GetPriceTrackingSetupGroupExcludedTravelerTypesXML(priceTrackingSetupGroup);

            db.spDesktopDataAdmin_UpdatePriceTrackingSetupGroup_v1(
                priceTrackingSetupGroup.PriceTrackingSetupGroupId,
                priceTrackingSetupGroup.PriceTrackingSetupGroupName,
                priceTrackingSetupGroup.PriceTrackingSetupTypeId,
                priceTrackingSetupGroup.GDSCode,
                priceTrackingSetupGroup.PseudoCityOrOfficeId,
                priceTrackingSetupGroup.SharedPseudoCityOrOfficeIdFlag,
                priceTrackingSetupGroup.FIQID,
                priceTrackingSetupGroup.DesktopUsedTypeId,
                priceTrackingSetupGroup.PriceTrackingMidOfficePlatformId,
                priceTrackingSetupGroup.MidOfficeUsedForQCTicketingFlag,
                priceTrackingSetupGroup.PriceTrackingItinerarySolutionId,
                priceTrackingSetupGroup.PriceTrackingSystemRuleId,
                priceTrackingSetupGroup.USGovernmentContractorFlag,
                priceTrackingSetupGroup.BackOfficeSystemId,
                priceTrackingSetupGroup.OtherExclusions,
                priceTrackingSetupGroup.AirPriceTrackingBillingModelId,
                priceTrackingSetupGroup.HotelPriceTrackingBillingModelId,
                priceTrackingSetupGroup.PreTicketPriceTrackingBillingModelId,
                priceTrackingSetupGroup.HierarchyType,
                priceTrackingSetupGroup.HierarchyCode,
                priceTrackingSetupGroup.TravelerTypeGuid,
                priceTrackingSetupGroup.ClientSubUnitGuid,
                priceTrackingSetupGroup.SourceSystemCode,
                priceTrackingSetupGroup.IsMultipleHierarchy,
                priceTrackingSetupGroupAdditionalPseudoCityOrOfficeIdsXML,
                priceTrackingSetupGroupExcludedTravelerTypesXML,
                adminUserGuid,
                priceTrackingSetupGroup.VersionNumber
            );
        }

        //PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeIds to XML
        public XElement GetPriceTrackingSetupGroupAdditionalPseudoCityOrOfficeIdsXML(PriceTrackingSetupGroup priceTrackingSetupGroup)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeIds");
            doc.AppendChild(root);

            if (priceTrackingSetupGroup.PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeIdsXML != null)
            {
                foreach (PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId priceTrackingSetupGroupAdditionalPseudoCityOrOfficeId in priceTrackingSetupGroup.PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeIdsXML)
                {
                    XmlElement xmlPriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId = doc.CreateElement("PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId");

                    if (!string.IsNullOrEmpty(priceTrackingSetupGroupAdditionalPseudoCityOrOfficeId.PseudoCityOrOfficeId))
                    {
                        //PseudoCityOrOfficeId
                        XmlElement xmlPseudoCityOrOfficeId = doc.CreateElement("PseudoCityOrOfficeId");
                        xmlPseudoCityOrOfficeId.InnerText = priceTrackingSetupGroupAdditionalPseudoCityOrOfficeId.PseudoCityOrOfficeId.ToString();
                        xmlPriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId.AppendChild(xmlPseudoCityOrOfficeId);

                        //SharedPseudoCityOrOfficeIdFlag
                        XmlElement xmlSharedPseudoCityOrOfficeIdFlag = doc.CreateElement("SharedPseudoCityOrOfficeIdFlag");
                        xmlSharedPseudoCityOrOfficeIdFlag.InnerText = priceTrackingSetupGroupAdditionalPseudoCityOrOfficeId.SharedPseudoCityOrOfficeIdFlag.ToString();
                        xmlPriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId.AppendChild(xmlSharedPseudoCityOrOfficeIdFlag);

                        root.AppendChild(xmlPriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId);
                    }
                }
            }

            return System.Xml.Linq.XElement.Parse(doc.OuterXml);
        }

        //PriceTrackingSetupGroupExcludedTravelerTypes to XML
        public XElement GetPriceTrackingSetupGroupExcludedTravelerTypesXML(PriceTrackingSetupGroup priceTrackingSetupGroup)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("PriceTrackingSetupGroupExcludedTravelerTypes");
            doc.AppendChild(root);

            if (priceTrackingSetupGroup.PriceTrackingSetupGroupExcludedTravelerTypesXML != null)
            {
				foreach (PriceTrackingSetupGroupExcludedTravelerType priceTrackingSetupGroupExcludedTravelerType in priceTrackingSetupGroup.PriceTrackingSetupGroupExcludedTravelerTypesXML)
                {
                    XmlElement xmlPriceTrackingSetupGroupExcludedTravelerType = doc.CreateElement("PriceTrackingSetupGroupExcludedTravelerType");

					if (!string.IsNullOrEmpty(priceTrackingSetupGroupExcludedTravelerType.TravelerTypeGuid))
                    {
                        //TravelerTypeGuid
                        XmlElement xmlTravelerTypeGuid = doc.CreateElement("TravelerTypeGuid");
						xmlTravelerTypeGuid.InnerText = priceTrackingSetupGroupExcludedTravelerType.TravelerTypeGuid.ToString();
                        xmlPriceTrackingSetupGroupExcludedTravelerType.AppendChild(xmlTravelerTypeGuid);

                        root.AppendChild(xmlPriceTrackingSetupGroupExcludedTravelerType);
                    }
                }
            }

            return System.Xml.Linq.XElement.Parse(doc.OuterXml);
        }

        //Change the deleted status on an item
        public void UpdateGroupDeletedStatus(PriceTrackingSetupGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdatePriceTrackingSetupGroupDeletedStatus_v1(
                group.PriceTrackingSetupGroupId,
                group.DeletedFlag,
                adminUserGuid,
                group.VersionNumber
                );
        }

        //UpdateLinkedClientSubUnit
        public void UpdateLinkedClientSubUnit(int priceTrackingSetupGroupId, string clientSubUnitGuid)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdatePriceTrackingSetupGroupLinkedClientSubUnit_v1(
                priceTrackingSetupGroupId,
                clientSubUnitGuid,
                adminUserGuid
            );
        }

        //Get one ClientSubUnits Linked to a PriceTrackingSetupGroup
        public List<ClientSubUnitCountryVM> GetLinkedClientSubUnits(int priceTrackingSetupGroupId, bool linked)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            var result = from n in db.spDesktopDataAdmin_SelectPriceTrackingSetupGroupLinkedClientSubUnits_v1(priceTrackingSetupGroupId, adminUserGuid, linked)
                         select new ClientSubUnitCountryVM
                         {
                             ClientSubUnitName = n.ClientSubUnitName.Trim(),
                             ClientSubUnitGuid = n.ClientSubUnitGuid,
                             CountryName = n.CountryName,
                             HasWriteAccess = (bool)n.HasWriteAccess,
                             IsClientExpiredFlag = n.IsClientExpiredFlag
                         };
            return result.ToList();
        }

        /*
         * AutoComplete ClientAccounts
         */
        public List<PriceTrackingSetupGroupClientAccountJSON> AutoCompleteClientAccounts(string searchText, string hierarchyType, string hierarchyItem, int priceTrackingSetupGroupId = 0)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            var result = from n in db.spDesktopDataAdmin_SelectPriceTrackingSetupGroupClientAccounts_v1(hierarchyType, hierarchyItem, priceTrackingSetupGroupId, searchText)
                         select
                             new PriceTrackingSetupGroupClientAccountJSON
                             {
                                 ClientAccountName = n.ClientAccountName.Trim(),
                                 ClientAccountNumber = n.ClientAccountNumber.ToString(),
                                 SourceSystemCode = n.SourceSystemCode.ToString(),
                                 ClientSubUnitGuid = n.ClientSubUnitGuid,
                                 ClientSubUnitName = n.ClientSubUnitName,
                                 ClientMasterCode = n.ClientMasterCode
                             };
            return result.ToList();
        }

        /*
         * AutoComplete ClientAccounts
         */
        public List<PriceTrackingSetupGroupTravelerTypeJSON> AutoCompleteTravelerTypes(string searchText, string hierarchyType, string hierarchyItem, int priceTrackingSetupGroupId = 0)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            var result = from n in db.spDesktopDataAdmin_SelectPriceTrackingSetupGroupTravelerTypes_v1(hierarchyType, hierarchyItem, priceTrackingSetupGroupId, searchText)
                         select
                             new PriceTrackingSetupGroupTravelerTypeJSON
                             {
                                 TravelerTypeName = n.TravelerTypeName.Trim(),
                                 TravelerTypeGuid = n.TravelerTypeGuid.Trim(),
                                 ParentName = n.ClientSubUnitName.Trim(),
                                 GrandParentName = n.ClientTopUnitName.Trim()
                             };
            return result.ToList();
        }

        /* Export */

        public DataSet GetPriceTrackingSetupGroupExcelData(int priceTrackingSetupGroupId)
        {
            DataSet ds = new DataSet();

            //Client Details
            DataTable clientDetailsExport = GetClientDetails(priceTrackingSetupGroupId);
            ds.Tables.Add(clientDetailsExport);

            //Air
            DataTable airExport = GetAirDetails(priceTrackingSetupGroupId);
            ds.Tables.Add(airExport);

            //Hotel
            DataTable hotelExport = GetHotelDetails(priceTrackingSetupGroupId);
            ds.Tables.Add(hotelExport);

            //Contacts
            DataTable contactsExport = GetContactDetails(priceTrackingSetupGroupId);
            ds.Tables.Add(contactsExport);

            return ds;
        }
        
        /* Client Details*/

        private DataTable GetClientDetails(int priceTrackingSetupGroupId)
        {
            DataTable datatable = new DataTable("Client Details");

            //Columns
            List<string> columnHeaders = new List<string>()
            {
                "Price Tracking Group Name",
                "Client TopUnit Name",
                "Client TopUnit GUID",
                "Client SubUnit Name",
                "Client SubUnit GUID",
                "Location",
                "Hierarchy Type",
                "GDS",
                "PCC/OID",
                "Shared PCC/OID?",
                "Additional PCC/OID and Shared PCC/OID?",
                "FIQID",
                "System Rules",
                "Account Number(s)/SSC",
                "US Government Contractor",
                "Client Master Code (CMC)",
                "Back Office System",
                "Traveler Types to Exclude",
                "Other Exclusions",
                "Air Billing Model",
                "Hotel Billing Model",
                "Pre-Ticket Billing Model"
            };

            foreach (string columnHeader in columnHeaders)
            {
                datatable.Columns.Add(columnHeader, typeof(string));
            }

            //Get Item From Database
            PriceTrackingSetupGroup priceTrackingSetupGroup = new PriceTrackingSetupGroup();
            priceTrackingSetupGroup = GetPriceTrackingSetupGroup(priceTrackingSetupGroupId);

            //Check Exists
            if (priceTrackingSetupGroup == null)
            {
                return datatable;
            }

            EditForDisplay(priceTrackingSetupGroup);

            //Add Rows

            if (priceTrackingSetupGroup.IsMultipleHierarchy && priceTrackingSetupGroup.ClientSubUnitsHierarchy.Count > 0)
            {
                foreach (ClientSubUnit clientSubUnit in priceTrackingSetupGroup.ClientSubUnitsHierarchy.OrderBy(x => x.ClientSubUnitName))
                {
                    DataRow workRow = datatable.NewRow();

                    ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
                    clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);

                    workRow["Price Tracking Group Name"] = priceTrackingSetupGroup.PriceTrackingSetupGroupName;

                    workRow["Client TopUnit Name"] = clientSubUnit.ClientTopUnitName;
                    workRow["Client TopUnit GUID"] = clientSubUnit.ClientTopUnitGuid;
                    workRow["Client SubUnit Name"] = clientSubUnit.ClientSubUnitName;
                    workRow["Client SubUnit GUID"] = clientSubUnit.ClientSubUnitGuid;
                    workRow["Location"] = clientSubUnit.CountryCode ?? "";
                    workRow["Hierarchy Type"] = "Client SubUnit";

                    workRow = GetClientDetailsRow(ref workRow, priceTrackingSetupGroup);

                    datatable.Rows.Add(workRow);
                }
            }
            else
            {
                DataRow workRow = datatable.NewRow();

                workRow["Price Tracking Group Name"] = priceTrackingSetupGroup.PriceTrackingSetupGroupName;

                if (priceTrackingSetupGroup.HierarchyType == "ClientTopUnit")
                {
                    workRow["Client TopUnit Name"] = priceTrackingSetupGroup.HierarchyItem;
                    workRow["Client TopUnit GUID"] = priceTrackingSetupGroup.HierarchyCode;
                    workRow["Client SubUnit Name"] = "NULL";
                    workRow["Client SubUnit GUID"] = "NULL";
                    workRow["Location"] = "NULL";
                }
                else
                {
                    ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
                    ClientSubUnit clientSubUnit = clientSubUnitRepository.GetClientSubUnit(priceTrackingSetupGroup.HierarchyCode);
                    clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);

                    workRow["Client TopUnit Name"] = clientSubUnit.ClientTopUnitName;
                    workRow["Client TopUnit GUID"] = clientSubUnit.ClientTopUnitGuid;
                    workRow["Client SubUnit Name"] = clientSubUnit.ClientSubUnitName;
                    workRow["Client SubUnit GUID"] = clientSubUnit.ClientSubUnitGuid;
                    workRow["Location"] = clientSubUnit.CountryCode ?? "";
                }

                workRow["Hierarchy Type"] = priceTrackingSetupGroup.HierarchyType;

                workRow = GetClientDetailsRow(ref workRow, priceTrackingSetupGroup);
                
                datatable.Rows.Add(workRow);
            }

            return datatable;
        }

        private DataRow GetClientDetailsRow(ref DataRow workRow, PriceTrackingSetupGroup priceTrackingSetupGroup)
        {
			//Get ClientSubUnit Accounts
			List<ClientAccount> clientAccounts = new List<ClientAccount>();
			List<string> clientMasterCodes = new List<string>();

			ClientSubUnitClientAccountRepository clientSubUnitClientAccountRepository = new ClientSubUnitClientAccountRepository();
			if (priceTrackingSetupGroup.PriceTrackingSetupGroupClientSubUnits != null && priceTrackingSetupGroup.PriceTrackingSetupGroupClientSubUnits.Count > 0)
			{
				foreach (PriceTrackingSetupGroupClientSubUnit priceTrackingSetupGroupClientSubUnit in priceTrackingSetupGroup.PriceTrackingSetupGroupClientSubUnits)
				{
					List<ClientAccount> priceTrackingSetupGroupClientSubUnitClientAccounts = clientSubUnitClientAccountRepository.GetClientAccountsBySubUnit(priceTrackingSetupGroupClientSubUnit.ClientSubUnitGuid);
					if (priceTrackingSetupGroupClientSubUnitClientAccounts != null && priceTrackingSetupGroupClientSubUnitClientAccounts.Count > 0)
					{
						foreach (ClientAccount clientAccount in priceTrackingSetupGroupClientSubUnitClientAccounts)
						{
							//Client Account
							clientAccounts.Add(clientAccount);

							//ClientMasterCodes
							clientMasterCodes.Add(clientAccount.ClientMasterCode);
						}
					}
				}
			}

            workRow["GDS"] = priceTrackingSetupGroup.GDS.GDSName;
            
			workRow["PCC/OID"] = priceTrackingSetupGroup.PseudoCityOrOfficeId;
            
			workRow["Shared PCC/OID?"] = priceTrackingSetupGroup.SharedPseudoCityOrOfficeIdFlag == true ? "Yes" : "No";

            //PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeIds: "24LY Yes, 34RD No"
            string priceTrackingSetupGroupAdditionalPseudoCityOrOfficeIds = "";
            if (priceTrackingSetupGroup.PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeIds != null && priceTrackingSetupGroup.PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeIds.Count > 0)
            {
                priceTrackingSetupGroupAdditionalPseudoCityOrOfficeIds = string.Join(", ", priceTrackingSetupGroup.PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeIds.Select(x => x.PseudoCityOrOfficeId + "/" + x.SharedPseudoCityOrOfficeIdFlag).ToList());
            }
            workRow["Additional PCC/OID and Shared PCC/OID?"] = priceTrackingSetupGroupAdditionalPseudoCityOrOfficeIds;

			workRow["FIQID"] = priceTrackingSetupGroup.FIQID;
            
			workRow["System Rules"] = priceTrackingSetupGroup.PriceTrackingSystemRule != null ? priceTrackingSetupGroup.PriceTrackingSystemRule.PriceTrackingSystemRuleName : "";

			workRow["Account Number(s)/SSC"] = (clientAccounts.Count > 0) ? string.Join(", ", clientAccounts.Select(x => x.ClientAccountNumber + " " + x.SourceSystemCode)) : "";

			workRow["Client Master Code (CMC)"] = (clientMasterCodes.Count > 0) ? string.Join(", ", clientMasterCodes.ToList()) : "";
			
			workRow["Back Office System"] = priceTrackingSetupGroup.BackOfficeSystem != null ? priceTrackingSetupGroup.BackOfficeSystem.BackOfficeSystemDescription : "";

            //ExcludedTravelerTypes: "General 14:12345, Contractor 14:54323"
            string priceTrackingSetupGroupExcludedTravelerTypes = "";
            if (priceTrackingSetupGroup.PriceTrackingSetupGroupExcludedTravelerTypes != null && priceTrackingSetupGroup.PriceTrackingSetupGroupExcludedTravelerTypes.Count > 0)
            {
                priceTrackingSetupGroupExcludedTravelerTypes = string.Join(", ", priceTrackingSetupGroup.PriceTrackingSetupGroupExcludedTravelerTypes.Select(x => x.TravelerTypeName + " " + x.TravelerTypeGuid).ToList());
            }
            workRow["Traveler Types to Exclude"] = priceTrackingSetupGroupExcludedTravelerTypes;

            workRow["Other Exclusions"] = priceTrackingSetupGroup.OtherExclusions;

            workRow["Air Billing Model"] = priceTrackingSetupGroup.AirPriceTrackingBillingModel != null && priceTrackingSetupGroup.AirPriceTrackingBillingModel.PriceTrackingBillingModelName != null ? priceTrackingSetupGroup.AirPriceTrackingBillingModel.PriceTrackingBillingModelName : "";

            workRow["Hotel Billing Model"] = priceTrackingSetupGroup.HotelPriceTrackingBillingModel != null && priceTrackingSetupGroup.HotelPriceTrackingBillingModel.PriceTrackingBillingModelName != null ? priceTrackingSetupGroup.HotelPriceTrackingBillingModel.PriceTrackingBillingModelName : "";

            workRow["Pre-Ticket Billing Model"] = priceTrackingSetupGroup.PreTicketPriceTrackingBillingModel != null && priceTrackingSetupGroup.PreTicketPriceTrackingBillingModel.PriceTrackingBillingModelName != null ? priceTrackingSetupGroup.PreTicketPriceTrackingBillingModel.PriceTrackingBillingModelName : "";

            return workRow;
        }

        /* Air */

        private DataTable GetAirDetails(int priceTrackingSetupGroupId)
        {
            DataTable datatable = new DataTable("Air");

            List<string> columnHeaders = new List<string>()
            {
                "Price Tracking Group Name",
                "Client TopUnit Name",
                "Client TopUnit GUID",
                "Client SubUnit Name",
                "Client SubUnit GUID",
                "Location",
                "Hierarchy Type",
                "Client has provided written approval",
                "Shared Savings",
                "Shared Savings Percentage",
                "Transaction Fee",
                "Transaction Amount",
                "Central Fulfillment Time Zone",
                "Central Fulfillment Business Hours",
                "Comments",
                "Number of Annual Trx",
                "Annual Spend",
                "Currency",
                "Estimated CWT Rebooking Fees",
                "CWT Void/Refund Fees",
                "No-Penalty/Void Window Threshold",
                "Outside Void (Penalty) Threshold",
                "Ticketing PCC/OID Time Zone",
                "Refundable to Refundable with penalty for refund",
                "If allowed, number of day predeparture to track Refundable to Refundable",
                "No-Penalty Refundable to penalty Refundable threshold amount",
                "Refundable to Non-Refundable",
                "If allowed, number of days predeparture to track Refundable to Non-Refundable",
                "No-Penalty Refundable to penalty Non-Refundable threshold amount",
                "Void window",
                "Refundable to Refundable outside void vindow (Exchange with partial refund)",
                "Non-Refundable to Non-Refundable outside void window (MCO)",
                "Exchanges",
                "Void an exchange",
                "Exchange a previous Exchange",
                "Non-Refundable to lower Non-Refundable with different change fee",
                "Refundable to lower Non-Refundable",
                "Refundable to lower Refundable",
                "Non-penalty Refundable to lower penalty Refundable",
                "Charge change fee up front for specific carriers (eg. United)",
                "Change fee must be used from residual value",
                "Track Private/Negotiated fares",
                "Negotiated pricing codes to track",
                "Company Profiles/STARS to add FIQID",
                "Realised Savings Code",
                "Missed Savings Code",
                "Enabled Date",
                "Deactivation Date",
                "Import PCC/OID",
                "Import Queue in Client PCC/OID",
                "Pricing PCC/OID",
                "Savings Queue ID",
                "Alpha Code Remarks Field",
                "Automatic Reticketing"
            };

            foreach (string columnHeader in columnHeaders)
            {
                datatable.Columns.Add(columnHeader, typeof(string));
            }

            //Get Item From Database
            PriceTrackingSetupGroup priceTrackingSetupGroup = new PriceTrackingSetupGroup();
            priceTrackingSetupGroup = GetPriceTrackingSetupGroup(priceTrackingSetupGroupId);

            //Check Exists
            if (priceTrackingSetupGroup == null)
            {
                return datatable;
            }

            EditForDisplay(priceTrackingSetupGroup);

            //Add Rows

            if (priceTrackingSetupGroup.IsMultipleHierarchy && priceTrackingSetupGroup.ClientSubUnitsHierarchy.Count > 0)
            {
                foreach (ClientSubUnit clientSubUnit in priceTrackingSetupGroup.ClientSubUnitsHierarchy.OrderBy(x => x.ClientSubUnitName))
                {
                    DataRow workRow = datatable.NewRow();

                    ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
                    clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);

                    workRow["Price Tracking Group Name"] = priceTrackingSetupGroup.PriceTrackingSetupGroupName;

                    workRow["Client TopUnit Name"] = clientSubUnit.ClientTopUnitName;
                    workRow["Client TopUnit GUID"] = clientSubUnit.ClientTopUnitGuid;
                    workRow["Client SubUnit Name"] = clientSubUnit.ClientSubUnitName;
                    workRow["Client SubUnit GUID"] = clientSubUnit.ClientSubUnitGuid;
                    workRow["Location"] = clientSubUnit.CountryCode ?? "";
                    workRow["Hierarchy Type"] = "Client SubUnit";

                    workRow = GetAirDetailsRow(ref workRow, priceTrackingSetupGroup);

                    datatable.Rows.Add(workRow);
                }
            }
            else
            {
                DataRow workRow = datatable.NewRow();

                workRow["Price Tracking Group Name"] = priceTrackingSetupGroup.PriceTrackingSetupGroupName;

                if (priceTrackingSetupGroup.HierarchyType == "ClientTopUnit")
                {
                    workRow["Client TopUnit Name"] = priceTrackingSetupGroup.HierarchyItem;
                    workRow["Client TopUnit GUID"] = priceTrackingSetupGroup.HierarchyCode;
                    workRow["Client SubUnit Name"] = "NULL";
                    workRow["Client SubUnit GUID"] = "NULL";
                    workRow["Location"] = "NULL";
                }
                else
                {
                    ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
                    ClientSubUnit clientSubUnit = clientSubUnitRepository.GetClientSubUnit(priceTrackingSetupGroup.HierarchyCode);
                    clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);

                    workRow["Client TopUnit Name"] = clientSubUnit.ClientTopUnitName;
                    workRow["Client TopUnit GUID"] = clientSubUnit.ClientTopUnitGuid;
                    workRow["Client SubUnit Name"] = clientSubUnit.ClientSubUnitName;
                    workRow["Client SubUnit GUID"] = clientSubUnit.ClientSubUnitGuid;
                    workRow["Location"] = clientSubUnit.CountryCode ?? "";
                }

                workRow["Hierarchy Type"] = priceTrackingSetupGroup.HierarchyType;

                workRow = GetAirDetailsRow(ref workRow, priceTrackingSetupGroup);

                datatable.Rows.Add(workRow);
            }

            return datatable;
        }

        private DataRow GetAirDetailsRow(ref DataRow workRow, PriceTrackingSetupGroup priceTrackingSetupGroup)
        {
            PriceTrackingSetupGroupItemAirRepository priceTrackingSetupGroupItemAirRepository = new PriceTrackingSetupGroupItemAirRepository();
            PriceTrackingSetupGroupItemAir priceTrackingSetupGroupItemAir = priceTrackingSetupGroupItemAirRepository.GetPriceTrackingSetupGroupItemAirByGroupId(priceTrackingSetupGroup.PriceTrackingSetupGroupId);

            if (priceTrackingSetupGroupItemAir == null)
            {
                return workRow;
            }

            priceTrackingSetupGroupItemAirRepository.EditForDisplay(priceTrackingSetupGroupItemAir);

            workRow["Client has provided written approval"] = priceTrackingSetupGroupItemAir.ClientHasProvidedWrittenApprovalFlag == true ? "Yes" : "No";

            //Pricing Model
            workRow["Shared Savings"] = priceTrackingSetupGroupItemAir.SharedSavingsFlag == true ? "Yes" : "No";
            workRow["Shared Savings Percentage"] = priceTrackingSetupGroupItemAir.SharedSavingsAmount;
            workRow["Transaction Fee"] = priceTrackingSetupGroupItemAir.TransactionFeeFlag == true ? "Yes" : "No";
            workRow["Transaction Amount"] = priceTrackingSetupGroupItemAir.TransactionFeeAmount;

            workRow["Central Fulfillment Time Zone"] = priceTrackingSetupGroupItemAir.CentralFulfillmentTimeZoneRuleCode;
            workRow["Central Fulfillment Business Hours"] = priceTrackingSetupGroupItemAir.CentralFulfillmentBusinessHours;

            //Comments
            workRow["Comments"] = priceTrackingSetupGroupItemAir.Comment;

            //Annual Air Volume
            workRow["Number of Annual Trx"] = priceTrackingSetupGroupItemAir.AnnualTransactionCount;
            workRow["Annual Spend"] = priceTrackingSetupGroupItemAir.AnnualSpendAmount;

            //Thresholds
            workRow["Currency"] = currencyRepository.GetCurrencyName(priceTrackingSetupGroupItemAir.CurrencyCode);
            workRow["Estimated CWT Rebooking Fees"] = priceTrackingSetupGroupItemAir.EstimatedCWTRebookingFeeAmount;
            workRow["CWT Void/Refund Fees"] = priceTrackingSetupGroupItemAir.CWTVoidRefundFeeAmount;
            workRow["No-Penalty/Void Window Threshold"] = priceTrackingSetupGroupItemAir.NoPenaltyVoidWindowThresholdAmount;
            workRow["Outside Void (Penalty) Threshold"] = priceTrackingSetupGroupItemAir.OutsideVoidPenaltyThresholdAmount;
            workRow["Ticketing PCC/OID Time Zone"] = priceTrackingSetupGroupItemAir.TimeZoneRuleCode;

            //Switch Window
            workRow["Refundable to Refundable with penalty for refund"] = priceTrackingSetupGroupItemAir.RefundableToRefundableWithPenaltyForRefundAllowedFlag == true ? "Allowed" : "Not Allowed";
            workRow["If allowed, number of day predeparture to track Refundable to Refundable"] = priceTrackingSetupGroupItemAir.RefundableToRefundablePreDepartureDayAmount;
            workRow["No-Penalty Refundable to penalty Refundable threshold amount"] = priceTrackingSetupGroupItemAir.NoPenaltyRefundableToPenaltyRefundableThresholdAmount;
            workRow["Refundable to Non-Refundable"] = priceTrackingSetupGroupItemAir.RefundableToNonRefundableAllowedFlag == true ? "Allowed" : "Not Allowed";
            workRow["If allowed, number of days predeparture to track Refundable to Non-Refundable"] = priceTrackingSetupGroupItemAir.RefundableToNonRefundablePreDepartureDayAmount;
            workRow["No-Penalty Refundable to penalty Non-Refundable threshold amount"] = priceTrackingSetupGroupItemAir.NoPenaltyRefundableToPenaltyNonRefundableThresholdAmount;

            //Refunds/Exchange
            workRow["Void window"] = priceTrackingSetupGroupItemAir.VoidWindowAllowedFlag == true ? "Allowed" : "Not Allowed";
            workRow["Refundable to Refundable outside void vindow (Exchange with partial refund)"] = priceTrackingSetupGroupItemAir.RefundableToRefundableOutsideVoidWindowAllowedFlag == true ? "Allowed" : "Not Allowed";
            workRow["Non-Refundable to Non-Refundable outside void window (MCO)"] = priceTrackingSetupGroupItemAir.NonRefundableToNonRefundableOutsideVoidWindowAllowedFlag == true ? "Allowed" : "Not Allowed";
            workRow["Exchanges"] = priceTrackingSetupGroupItemAir.ExchangesAllowedFlag == true ? "Allowed" : "Not Allowed";
            workRow["Void an exchange"] = priceTrackingSetupGroupItemAir.VoidExchangeAllowedFlag == true ? "Allowed" : "Not Allowed";
            workRow["Exchange a previous Exchange"] = ParseBooleanAllowedValue(priceTrackingSetupGroupItemAir.ExchangePreviousExchangeAllowedFlag);
            workRow["Non-Refundable to lower Non-Refundable with different change fee"] = priceTrackingSetupGroupItemAir.NonRefundableToLowerNonRefundableWithDifferentChangeFeeAllowedFlag == true ? "Allowed" : "Not Allowed";
            workRow["Refundable to lower Non-Refundable"] = priceTrackingSetupGroupItemAir.RefundableToLowerNonRefundableAllowedFlag == true ? "Allowed" : "Not Allowed";
            workRow["Refundable to lower Refundable"] = priceTrackingSetupGroupItemAir.RefundableToLowerRefundableAllowedFlag == true ? "Allowed" : "Not Allowed";
            workRow["Non-penalty Refundable to lower penalty Refundable"] = priceTrackingSetupGroupItemAir.NonPenaltyRefundableToLowerPenaltyRefundableAllowedFlag == true ? "Allowed" : "Not Allowed";

            //Airline Change Fees (NORAM POS only)
            workRow["Charge change fee up front for specific carriers (eg. United)"] = priceTrackingSetupGroupItemAir.ChargeChangeFeeUpFrontForSpecificCarriersFlag == true ? "Allowed" : "Not Allowed";
            workRow["Change fee must be used from residual value"] = priceTrackingSetupGroupItemAir.ChangeFeeMustBeUsedFromResidualValueFlag == true ? "Allowed" : "Not Allowed";

            //Private/Negotiated Fares
            workRow["Track Private/Negotiated fares"] = ParseBooleanAllowedValue(priceTrackingSetupGroupItemAir.TrackPrivateNegotiatedFareFlag);
            workRow["Negotiated pricing codes to track"] = string.Join(", ", GetPriceTrackingSetupGroupItemAirNegotiatedPricingTrackingCodes(priceTrackingSetupGroupItemAir));

            //Shared PCC/OID
            workRow["Company Profiles/STARS to add FIQID"] = priceTrackingSetupGroupItemAir.CompanyProfilesRequiringFIQID;

            //Reason Codes
            workRow["Realised Savings Code"] = priceTrackingSetupGroupItemAir.RealisedSavingsCode;
            workRow["Missed Savings Code"] = priceTrackingSetupGroupItemAir.MissedSavingsCode;

            //Admin only
            workRow["Enabled Date"] = priceTrackingSetupGroupItemAir.EnabledDate.HasValue ? priceTrackingSetupGroupItemAir.EnabledDate.Value.ToString("yyyy/MM/dd") : "";
            workRow["Deactivation Date"] = priceTrackingSetupGroupItemAir.DeactivationDate.HasValue ? priceTrackingSetupGroupItemAir.DeactivationDate.Value.ToString("yyyy/MM/dd") : "";
            workRow["Import PCC/OID"] = priceTrackingSetupGroupItemAir.ImportPseudoCityOrOfficeId;
            workRow["Import Queue in Client PCC/OID"] = priceTrackingSetupGroupItemAir.ImportQueueInClientPseudoCityOrOfficeId;
            workRow["Pricing PCC/OID"] = priceTrackingSetupGroupItemAir.PricingPseudoCityOrOfficeId;
            workRow["Savings Queue ID"] = priceTrackingSetupGroupItemAir.SavingsQueueId;
            workRow["Alpha Code Remarks Field"] = priceTrackingSetupGroupItemAir.AlphaCodeRemarkField;
            workRow["Automatic Reticketing"] = priceTrackingSetupGroupItemAir.AutomaticReticketingFlag == true ? "Yes" : "No";

            return workRow;
        }

        /* Hotel */

        private DataTable GetHotelDetails(int priceTrackingSetupGroupId)
        {
            DataTable datatable = new DataTable("Hotel");

            List<string> columnHeaders = new List<string>()
            {
                "Price Tracking Group Name",
                "Client TopUnit Name",
                "Client TopUnit GUID",
                "Client SubUnit Name",
                "Client SubUnit GUID",
                "Location",
                "Hierarchy Type",
                "Client has provided written approval",
                "Client has hotel fees in MSA",
                "Client uses Conferma virtual cards",
                "Shared Savings",
                "Shared Savings Percentage",
                "Transaction Fee",
                "Transaction Amount",
                "Central Fulfillment Time Zone",
                "Central Fulfillment Business Hours",
                "Comments",
                "Number of Annual Trx",
                "Annual Spend",
                "Currency Name",
                "Estimated Rebooking Fees",
                "CWT Refund Fees",
                "Threshold",
                "Calculated Total Threshold",
                "Corporate Rate Codes",
                "Enabled Date",
                "Deactivation Date",
                "Enable Hotel Value Tracking",
                "Value Threshold Rebooking Fee",
                "Breakfast Changes",
                "Breakfast Value",
                "Parking Value",
                "Internet Access Value",
                "Room Type Changes",
                "Bedding Type Changes",
                "King/Queen to Non King/Queen",
                "Hotel Rate Code Changes",
                "Cancellation Policy Changes",
                "CWT Rate Codes to Track",
                "Import PCC / OID",
                "Import Queue in Client PCC / OID",
                "Pricing PCC / OID",
                "Savings Queue ID",
                "Alpha Code Remarks Field",
                "Tracking Alerts Email"
            };

            foreach (string columnHeader in columnHeaders)
            {
                datatable.Columns.Add(columnHeader, typeof(string));
            }

            //Get Item From Database
            PriceTrackingSetupGroup priceTrackingSetupGroup = new PriceTrackingSetupGroup();
            priceTrackingSetupGroup = GetPriceTrackingSetupGroup(priceTrackingSetupGroupId);

            //Check Exists
            if (priceTrackingSetupGroup == null)
            {
                return datatable;
            }

            EditForDisplay(priceTrackingSetupGroup);

            //Add Rows

            if (priceTrackingSetupGroup.IsMultipleHierarchy && priceTrackingSetupGroup.ClientSubUnitsHierarchy.Count > 0)
            {
                foreach (ClientSubUnit clientSubUnit in priceTrackingSetupGroup.ClientSubUnitsHierarchy.OrderBy(x => x.ClientSubUnitName))
                {
                    DataRow workRow = datatable.NewRow();

                    ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
                    clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);

                    workRow["Price Tracking Group Name"] = priceTrackingSetupGroup.PriceTrackingSetupGroupName;

                    workRow["Client TopUnit Name"] = clientSubUnit.ClientTopUnitName;
                    workRow["Client TopUnit GUID"] = clientSubUnit.ClientTopUnitGuid;
                    workRow["Client SubUnit Name"] = clientSubUnit.ClientSubUnitName;
                    workRow["Client SubUnit GUID"] = clientSubUnit.ClientSubUnitGuid;
                    workRow["Location"] = clientSubUnit.CountryCode ?? "";
                    workRow["Hierarchy Type"] = "Client SubUnit";

                    workRow = GetHotelDetailsRow(ref workRow, priceTrackingSetupGroup);

                    datatable.Rows.Add(workRow);
                }
            }
            else
            {
                DataRow workRow = datatable.NewRow();

                workRow["Price Tracking Group Name"] = priceTrackingSetupGroup.PriceTrackingSetupGroupName;

                if (priceTrackingSetupGroup.HierarchyType == "ClientTopUnit")
                {
                    workRow["Client TopUnit Name"] = priceTrackingSetupGroup.HierarchyItem;
                    workRow["Client TopUnit GUID"] = priceTrackingSetupGroup.HierarchyCode;
                    workRow["Client SubUnit Name"] = "NULL";
                    workRow["Client SubUnit GUID"] = "NULL";
                    workRow["Location"] = "NULL";
                }
                else
                {
                    ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
                    ClientSubUnit clientSubUnit = clientSubUnitRepository.GetClientSubUnit(priceTrackingSetupGroup.HierarchyCode);
                    clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);

                    workRow["Client TopUnit Name"] = clientSubUnit.ClientTopUnitName;
                    workRow["Client TopUnit GUID"] = clientSubUnit.ClientTopUnitGuid;
                    workRow["Client SubUnit Name"] = clientSubUnit.ClientSubUnitName;
                    workRow["Client SubUnit GUID"] = clientSubUnit.ClientSubUnitGuid;
                    workRow["Location"] = clientSubUnit.CountryCode ?? "";
                }

                workRow["Hierarchy Type"] = priceTrackingSetupGroup.HierarchyType;

                workRow = GetHotelDetailsRow(ref workRow, priceTrackingSetupGroup);

                datatable.Rows.Add(workRow);
            }

            return datatable;
        }

        private DataRow GetHotelDetailsRow(ref DataRow workRow, PriceTrackingSetupGroup priceTrackingSetupGroup)
        {
            PriceTrackingSetupGroupItemHotelRepository priceTrackingSetupGroupItemHotelRepository = new PriceTrackingSetupGroupItemHotelRepository();
            PriceTrackingSetupGroupItemHotel priceTrackingSetupGroupItemHotel = priceTrackingSetupGroupItemHotelRepository.GetPriceTrackingSetupGroupItemHotelByGroupId(priceTrackingSetupGroup.PriceTrackingSetupGroupId);

            if (priceTrackingSetupGroupItemHotel == null)
            {
                return workRow;
            }

            priceTrackingSetupGroupItemHotelRepository.EditForDisplay(priceTrackingSetupGroupItemHotel);

            workRow["Client has provided written approval"] = priceTrackingSetupGroupItemHotel.ClientHasProvidedWrittenApprovalFlag == true ? "Yes" : "No";
            workRow["Client has hotel fees in MSA"] = ParseBooleanValue(priceTrackingSetupGroupItemHotel.ClientHasHotelFeesInMSAFlag);
            workRow["Client uses Conferma virtual cards"] = ParseBooleanValue(priceTrackingSetupGroupItemHotel.ClientUsesConfermaVirtualCardsFlag);

            //Pricing Model
            workRow["Shared Savings"] = priceTrackingSetupGroupItemHotel.SharedSavingsFlag == true ? "Yes" : "No";
            workRow["Shared Savings Percentage"] = priceTrackingSetupGroupItemHotel.SharedSavingsAmount;
            workRow["Transaction Fee"] = priceTrackingSetupGroupItemHotel.TransactionFeeFlag == true ? "Yes" : "No";
            workRow["Transaction Amount"] = priceTrackingSetupGroupItemHotel.TransactionFeeAmount;

            workRow["Central Fulfillment Time Zone"] = priceTrackingSetupGroupItemHotel.CentralFulfillmentTimeZoneRuleCode;
            workRow["Central Fulfillment Business Hours"] = priceTrackingSetupGroupItemHotel.CentralFulfillmentBusinessHours;
            workRow["Comments"] = priceTrackingSetupGroupItemHotel.Comment;

            //Annual Hotel Volume
            workRow["Number of Annual Trx"] = priceTrackingSetupGroupItemHotel.AnnualTransactionCount;
            workRow["Annual Spend"] = priceTrackingSetupGroupItemHotel.AnnualSpendAmount;

            //Thresholds and Settings
            workRow["Currency Name"] = currencyRepository.GetCurrencyName(priceTrackingSetupGroupItemHotel.CurrencyCode);

            workRow["Estimated Rebooking Fees"] = priceTrackingSetupGroupItemHotel.EstimatedCWTRebookingFeeAmount;
            workRow["CWT Refund Fees"] = priceTrackingSetupGroupItemHotel.CWTVoidRefundFeeAmount;
            workRow["Threshold"] = priceTrackingSetupGroupItemHotel.ThresholdAmount;
            workRow["Calculated Total Threshold"] = priceTrackingSetupGroupItemHotel.CalculatedTotalThresholdAmount;

            //Corporate Rate Codes
            workRow["Corporate Rate Codes"] = string.Join(", ", GetPriceTrackingSetupGroupItemHotelCorporateRateTrackingCodes(priceTrackingSetupGroupItemHotel));

            //Admin only
            workRow["Enabled Date"] = priceTrackingSetupGroupItemHotel.EnabledDate.HasValue ? priceTrackingSetupGroupItemHotel.EnabledDate.Value.ToString("yyyy/MM/dd") : ""; ;
            workRow["Deactivation Date"] = priceTrackingSetupGroupItemHotel.DeactivationDate.HasValue ? priceTrackingSetupGroupItemHotel.DeactivationDate.Value.ToString("yyyy/MM/dd") : ""; ;
            workRow["Enable Hotel Value Tracking"] = ParseBooleanValue(priceTrackingSetupGroupItemHotel.EnableValueTrackingFlag);
            workRow["Value Threshold Rebooking Fee"] = priceTrackingSetupGroupItemHotel.TMCFeeThreshold;
            workRow["Breakfast Changes"] = priceTrackingSetupGroupItemHotel.BreakfastChangesAllowedFlag == true ? "Allowed" : "Not Allowed";
            workRow["Breakfast Value"] = priceTrackingSetupGroupItemHotel.BreakfastValue;
            workRow["Parking Value"] = priceTrackingSetupGroupItemHotel.ParkingValue;
            workRow["Internet Access Value"] = priceTrackingSetupGroupItemHotel.InternetAccessValue;
            workRow["Room Type Changes"] = priceTrackingSetupGroupItemHotel.RoomTypeUpgradeAllowedFlag == true ? "Allowed" : "Not Allowed";
            workRow["Bedding Type Changes"] = priceTrackingSetupGroupItemHotel.BeddingTypeUpgradeAllowedFlag == true ? "Allowed" : "Not Allowed";
            workRow["King/Queen to Non King/Queen"] = priceTrackingSetupGroupItemHotel.KingQueenUpgradeAllowedFlag == true ? "Allowed" : "Not Allowed";
            workRow["Hotel Rate Code Changes"] = priceTrackingSetupGroupItemHotel.HotelRateCodeUpgradeAllowedFlag == true ? "Allowed" : "Not Allowed";
            workRow["Cancellation Policy Changes"] = priceTrackingSetupGroupItemHotel.CancellationPolicyUpgradeAllowedFlag == true ? "Allowed" : "Not Allowed";
            workRow["CWT Rate Codes to Track"] = string.Join(", ", GetPriceTrackingSetupGroupItemHotelCWTRateTrackingCodes(priceTrackingSetupGroupItemHotel));
            workRow["Import PCC / OID"] = priceTrackingSetupGroupItemHotel.ImportPseudoCityOrOfficeId;
            workRow["Import Queue in Client PCC / OID"] = priceTrackingSetupGroupItemHotel.ImportQueueInClientPseudoCityOrOfficeId;
            workRow["Pricing PCC / OID"] = priceTrackingSetupGroupItemHotel.PricingPseudoCityOrOfficeId;
            workRow["Savings Queue ID"] = priceTrackingSetupGroupItemHotel.SavingsQueueId;
            workRow["Alpha Code Remarks Field"] = priceTrackingSetupGroupItemHotel.AlphaCodeRemarkField;
            workRow["Tracking Alerts Email"] = GetPriceTrackingSetupGroupItemHotelTrackingAlertEmailAddresses(priceTrackingSetupGroupItemHotel);

            return workRow;
        }

        /* Contacts */

        private DataTable GetContactDetails(int priceTrackingSetupGroupId)
        {
            DataTable datatable = new DataTable("Contacts");

            List<string> columnHeaders = new List<string>()
            {
                "Price Tracking Group Name",
                "Client TopUnit Name",
                "Client TopUnit GUID",
                "Client SubUnit Name",
                "Client SubUnit GUID",
                "Location",
                "Hierarchy Type",
                "Contact Type",
                "Last Name",
                "First Name",
                "Email",
                "User Type",
                "Access to Dashboard",
                "Receive email alerts?"
            };

            foreach (string columnHeader in columnHeaders)
            {
                datatable.Columns.Add(columnHeader, typeof(string));
            }

            //Get Item From Database
            PriceTrackingSetupGroup priceTrackingSetupGroup = new PriceTrackingSetupGroup();
            priceTrackingSetupGroup = GetPriceTrackingSetupGroup(priceTrackingSetupGroupId);

            //Check Exists
            if (priceTrackingSetupGroup == null)
            {
                return datatable;
            }

            EditForDisplay(priceTrackingSetupGroup);

            //Contacts
            List<PriceTrackingContact> priceTrackingContacts = priceTrackingContactRepository.GetPriceTrackingContactByPriceTrackingSetupGroupId(priceTrackingSetupGroup.PriceTrackingSetupGroupId);

            //Add Rows

            if (priceTrackingSetupGroup.IsMultipleHierarchy && priceTrackingSetupGroup.ClientSubUnitsHierarchy.Count > 0)
            {
                foreach (ClientSubUnit clientSubUnit in priceTrackingSetupGroup.ClientSubUnitsHierarchy.OrderBy(x => x.ClientSubUnitName))
                {
                    foreach (PriceTrackingContact priceTrackingContact in priceTrackingContacts)
                    {
                        DataRow workRow = datatable.NewRow();

                        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
                        clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);

                        workRow["Price Tracking Group Name"] = priceTrackingSetupGroup.PriceTrackingSetupGroupName;

                        workRow["Client TopUnit Name"] = clientSubUnit.ClientTopUnitName;
                        workRow["Client TopUnit GUID"] = clientSubUnit.ClientTopUnitGuid;
                        workRow["Client SubUnit Name"] = clientSubUnit.ClientSubUnitName;
                        workRow["Client SubUnit GUID"] = clientSubUnit.ClientSubUnitGuid;
                        workRow["Location"] = clientSubUnit.CountryCode ?? "";
                        workRow["Hierarchy Type"] = "Client SubUnit";

                        workRow = GetContactDetailsRow(ref workRow, priceTrackingContact);

                        datatable.Rows.Add(workRow);
                    }
                }
            }
            else
            {
                foreach (PriceTrackingContact priceTrackingContact in priceTrackingContacts)
                {
                    DataRow workRow = datatable.NewRow();

                    workRow["Price Tracking Group Name"] = priceTrackingSetupGroup.PriceTrackingSetupGroupName;

                    if (priceTrackingSetupGroup.HierarchyType == "ClientTopUnit")
                    {
                        workRow["Client TopUnit Name"] = priceTrackingSetupGroup.HierarchyItem;
                        workRow["Client TopUnit GUID"] = priceTrackingSetupGroup.HierarchyCode;
                        workRow["Client SubUnit Name"] = "NULL";
                        workRow["Client SubUnit GUID"] = "NULL";
                        workRow["Location"] = "NULL";
                    }
                    else
                    {
                        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
                        ClientSubUnit clientSubUnit = clientSubUnitRepository.GetClientSubUnit(priceTrackingSetupGroup.HierarchyCode);
                        clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);

                        workRow["Client TopUnit Name"] = clientSubUnit.ClientTopUnitName;
                        workRow["Client TopUnit GUID"] = clientSubUnit.ClientTopUnitGuid;
                        workRow["Client SubUnit Name"] = clientSubUnit.ClientSubUnitName;
                        workRow["Client SubUnit GUID"] = clientSubUnit.ClientSubUnitGuid;
                        workRow["Location"] = clientSubUnit.CountryCode ?? "";
                    }

                    workRow["Hierarchy Type"] = priceTrackingSetupGroup.HierarchyType;

                    workRow = GetContactDetailsRow(ref workRow, priceTrackingContact);

                    datatable.Rows.Add(workRow);
                }
            }

            return datatable;
        }

        private DataRow GetContactDetailsRow(ref DataRow workRow, PriceTrackingContact priceTrackingContact)
        {
            priceTrackingContactRepository.EditForDisplay(priceTrackingContact);

            workRow["Contact Type"] = priceTrackingContact.ContactType != null && priceTrackingContact.ContactType.ContactTypeName != null ? priceTrackingContact.ContactType.ContactTypeName : "";
            workRow["Last Name"] = priceTrackingContact.LastName;
            workRow["First Name"] = priceTrackingContact.FirstName;
            workRow["Email"] = priceTrackingContact.EmailAddress;
			workRow["User Type"] = "NULL";
			if (priceTrackingContact.PriceTrackingContactUserType != null && priceTrackingContact.PriceTrackingContactUserType.PriceTrackingContactUserTypeName != null)
			{
				workRow["User Type"] = priceTrackingContact.PriceTrackingContactUserType.PriceTrackingContactUserTypeName;
			}
			workRow["Access to Dashboard"] = ParseBooleanValue(priceTrackingContact.PriceTrackingDashboardAccessFlag);
            workRow["Receive email alerts?"] = "NULL";
			if (priceTrackingContact.PriceTrackingEmailAlertType != null && priceTrackingContact.PriceTrackingEmailAlertType.PriceTrackingEmailAlertTypeName != null)
			{
				workRow["Receive email alerts?"] = priceTrackingContact.PriceTrackingEmailAlertType.PriceTrackingEmailAlertTypeName;
			}

            return workRow;
        }

        /* Helpers */

        private List<string> GetPriceTrackingSetupGroupItemAirNegotiatedPricingTrackingCodes(PriceTrackingSetupGroupItemAir priceTrackingSetupGroupItemAir)
        {
            List<string> negotiatedPricingTrackingCodes = new List<string>();

            if (!string.IsNullOrEmpty(priceTrackingSetupGroupItemAir.NegotiatedPricingTrackingCode1))
            {
                negotiatedPricingTrackingCodes.Add(priceTrackingSetupGroupItemAir.NegotiatedPricingTrackingCode1);
            }

            if (!string.IsNullOrEmpty(priceTrackingSetupGroupItemAir.NegotiatedPricingTrackingCode2))
            {
                negotiatedPricingTrackingCodes.Add(priceTrackingSetupGroupItemAir.NegotiatedPricingTrackingCode2);
            }

            if (!string.IsNullOrEmpty(priceTrackingSetupGroupItemAir.NegotiatedPricingTrackingCode3))
            {
                negotiatedPricingTrackingCodes.Add(priceTrackingSetupGroupItemAir.NegotiatedPricingTrackingCode3);
            }

            if (!string.IsNullOrEmpty(priceTrackingSetupGroupItemAir.NegotiatedPricingTrackingCode4))
            {
                negotiatedPricingTrackingCodes.Add(priceTrackingSetupGroupItemAir.NegotiatedPricingTrackingCode4);
            }

            return negotiatedPricingTrackingCodes;
        }

        private List<string> GetPriceTrackingSetupGroupItemHotelCWTRateTrackingCodes(PriceTrackingSetupGroupItemHotel priceTrackingSetupGroupItemHotel)
        {
            List<string> cwtRateTrackingCodes = new List<string>();

            if (!string.IsNullOrEmpty(priceTrackingSetupGroupItemHotel.CWTRateTrackingCode1))
            {
                cwtRateTrackingCodes.Add(priceTrackingSetupGroupItemHotel.CWTRateTrackingCode1);
            }

            if (!string.IsNullOrEmpty(priceTrackingSetupGroupItemHotel.CWTRateTrackingCode2))
            {
                cwtRateTrackingCodes.Add(priceTrackingSetupGroupItemHotel.CWTRateTrackingCode2);
            }

            return cwtRateTrackingCodes;
        }

        private List<string> GetPriceTrackingSetupGroupItemHotelCorporateRateTrackingCodes(PriceTrackingSetupGroupItemHotel priceTrackingSetupGroupItemHotel)
        {
            List<string> corporateRateTrackingCodes = new List<string>();

            if (!string.IsNullOrEmpty(priceTrackingSetupGroupItemHotel.CorporateRateTrackingCode1))
            {
                corporateRateTrackingCodes.Add(priceTrackingSetupGroupItemHotel.CorporateRateTrackingCode1);
            }

            if (!string.IsNullOrEmpty(priceTrackingSetupGroupItemHotel.CorporateRateTrackingCode2))
            {
                corporateRateTrackingCodes.Add(priceTrackingSetupGroupItemHotel.CorporateRateTrackingCode2);
            }

            if (!string.IsNullOrEmpty(priceTrackingSetupGroupItemHotel.CorporateRateTrackingCode3))
            {
                corporateRateTrackingCodes.Add(priceTrackingSetupGroupItemHotel.CorporateRateTrackingCode3);
            }

            if (!string.IsNullOrEmpty(priceTrackingSetupGroupItemHotel.CorporateRateTrackingCode4))
            {
                corporateRateTrackingCodes.Add(priceTrackingSetupGroupItemHotel.CorporateRateTrackingCode4);
            }

            return corporateRateTrackingCodes;
        }

        private string GetPriceTrackingSetupGroupItemHotelTrackingAlertEmailAddresses(PriceTrackingSetupGroupItemHotel priceTrackingSetupGroupItemHotel)
        {
            string priceTrackingSetupGroupItemHotelTrackingAlertEmailAddresses = "";

            if (
                priceTrackingSetupGroupItemHotel.PriceTrackingSetupGroupItemHotelTrackingAlertEmailAddresses != null &&
                priceTrackingSetupGroupItemHotel.PriceTrackingSetupGroupItemHotelTrackingAlertEmailAddresses.Count > 0)
            {
                priceTrackingSetupGroupItemHotelTrackingAlertEmailAddresses = 
                    string.Join(", ", priceTrackingSetupGroupItemHotel.PriceTrackingSetupGroupItemHotelTrackingAlertEmailAddresses.Select(x => x.EmailAddress).ToList());
            }

            return priceTrackingSetupGroupItemHotelTrackingAlertEmailAddresses;
        }

        private string ParseBooleanValue(bool? value)
        {
            string returnValue = "NULL";

            if (value == true)
            {
                returnValue = "Yes";
            }
            else if (value == false)
            {
                returnValue = "No";
            }

            return returnValue;
        }

        private string ParseBooleanAllowedValue(bool? value)
        {
            string returnValue = "NULL";

            if (value == true)
            {
                returnValue = "Allowed";
            }
            else if (value == false)
            {
                returnValue = "Not Allowed";
            }

            return returnValue;
        }
    }
}
