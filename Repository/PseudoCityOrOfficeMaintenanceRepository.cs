using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.ViewModels;
using System.Xml;
using System.Text;

namespace CWTDesktopDatabase.Repository
{
	public class PseudoCityOrOfficeMaintenanceRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//List PseudoCityOrOfficeMaintenances
        public CWTPaginatedList<spDesktopDataAdmin_SelectPseudoCityOrOfficeMaintenance_v1Result> GetPseudoCityOrOfficeMaintenances(string sortField, int sortOrder, string filter, int page, bool deletedFlagNonNullable)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectPseudoCityOrOfficeMaintenance_v1(filter, sortField, sortOrder, page, deletedFlagNonNullable, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPseudoCityOrOfficeMaintenance_v1Result>(result, page, Convert.ToInt32(totalRecords));
            return paginatedView;
        }

		//Edit PseudoCityOrOfficeMaintenance
		public void EditForDisplay(PseudoCityOrOfficeMaintenance pseudoCityOrOfficeMaintenance)
		{
			//
			PseudoCityOrOfficeAddressRepository pseudoCityOrOfficeAddressRepository = new PseudoCityOrOfficeAddressRepository();
			PseudoCityOrOfficeAddressCountryGlobalRegionJSON pseudoCityOrOfficeAddressCountryGlobalRegionJSON = pseudoCityOrOfficeAddressRepository.GetPseudoCityOrOfficeAddressCountryGlobalRegion(
				 pseudoCityOrOfficeMaintenance.PseudoCityOrOfficeAddressId
			 ).FirstOrDefault();

			if (pseudoCityOrOfficeAddressCountryGlobalRegionJSON != null)
			{
				pseudoCityOrOfficeMaintenance.CountryCode = pseudoCityOrOfficeAddressCountryGlobalRegionJSON.CountryCode;
				pseudoCityOrOfficeMaintenance.CountryName = pseudoCityOrOfficeAddressCountryGlobalRegionJSON.CountryName;

				pseudoCityOrOfficeMaintenance.GlobalRegionCode = pseudoCityOrOfficeAddressCountryGlobalRegionJSON.GlobalRegionCode;
				pseudoCityOrOfficeMaintenance.GlobalRegionName = pseudoCityOrOfficeAddressCountryGlobalRegionJSON.GlobalRegionName;
			}

			//GDSThirdPartyVendors
			GDSThirdPartyVendorRepository gdsThirdPartyVendorRepository = new GDSThirdPartyVendorRepository();
			List<GDSThirdPartyVendor> gdsThirdPartyVendors = gdsThirdPartyVendorRepository.GetGDSThirdPartyVendorsByPseudoCityOrOfficeMaintenanceId(pseudoCityOrOfficeMaintenance.PseudoCityOrOfficeMaintenanceId);
			if (gdsThirdPartyVendors != null)
			{
				pseudoCityOrOfficeMaintenance.GDSThirdPartyVendorsList = gdsThirdPartyVendors;
			}

			//ClientSubUnits
			List<ClientSubUnit> clientSubUnits = GetAllPseudoCityOrOfficeMaintenanceClientSubUnits(pseudoCityOrOfficeMaintenance.PseudoCityOrOfficeMaintenanceId);
			if (clientSubUnits != null)
			{
				pseudoCityOrOfficeMaintenance.ClientSubUnitsList = clientSubUnits;
			}

			//ActiveFlag
			if (pseudoCityOrOfficeMaintenance.ActiveFlag == true)
			{
				pseudoCityOrOfficeMaintenance.ActiveFlagNonNullable = true;
			}
			else
			{
				pseudoCityOrOfficeMaintenance.ActiveFlagNonNullable = false;
			}

			//SharedPseudoCityOrOfficeFlag
			if (pseudoCityOrOfficeMaintenance.SharedPseudoCityOrOfficeFlag == true)
			{
				pseudoCityOrOfficeMaintenance.SharedPseudoCityOrOfficeFlagNonNullable = true;
			}
			else
			{
				pseudoCityOrOfficeMaintenance.SharedPseudoCityOrOfficeFlagNonNullable = false;
			}

			//CWTOwnedPseudoCityOrOfficeFlag
			if (pseudoCityOrOfficeMaintenance.CWTOwnedPseudoCityOrOfficeFlag == true)
			{
				pseudoCityOrOfficeMaintenance.CWTOwnedPseudoCityOrOfficeFlagNonNullable = true;
			}
			else
			{
				pseudoCityOrOfficeMaintenance.CWTOwnedPseudoCityOrOfficeFlagNonNullable = false;
			}

			//ClientDedicatedPseudoCityOrOfficeFlag
			if (pseudoCityOrOfficeMaintenance.ClientDedicatedPseudoCityOrOfficeFlag == true)
			{
				pseudoCityOrOfficeMaintenance.ClientDedicatedPseudoCityOrOfficeFlagNonNullable = true;
			}
			else
			{
				pseudoCityOrOfficeMaintenance.ClientDedicatedPseudoCityOrOfficeFlagNonNullable = false;
			}

			//ClientGDSAccessFlag
			if (pseudoCityOrOfficeMaintenance.ClientGDSAccessFlag == true)
			{
				pseudoCityOrOfficeMaintenance.ClientGDSAccessFlagNonNullable = true;
			}
			else
			{
				pseudoCityOrOfficeMaintenance.ClientGDSAccessFlagNonNullable = false;
			}

			//DevelopmentOrInternalPseudoCityOrOfficeFlag
			if (pseudoCityOrOfficeMaintenance.DevelopmentOrInternalPseudoCityOrOfficeFlag == true)
			{
				pseudoCityOrOfficeMaintenance.DevelopmentOrInternalPseudoCityOrOfficeFlagNonNullable = true;
			}
			else
			{
				pseudoCityOrOfficeMaintenance.DevelopmentOrInternalPseudoCityOrOfficeFlagNonNullable = false;
			}

			//CubaPseudoCityOrOfficeFlag
			if (pseudoCityOrOfficeMaintenance.CubaPseudoCityOrOfficeFlag == true)
			{
				pseudoCityOrOfficeMaintenance.CubaPseudoCityOrOfficeFlagNonNullable = true;
			}
			else
			{
				pseudoCityOrOfficeMaintenance.CubaPseudoCityOrOfficeFlagNonNullable = false;
			}

			//GovernmentPseudoCityOrOfficeFlag
			if (pseudoCityOrOfficeMaintenance.GovernmentPseudoCityOrOfficeFlag == true)
			{
				pseudoCityOrOfficeMaintenance.GovernmentPseudoCityOrOfficeFlagNonNullable = true;
			}
			else
			{
				pseudoCityOrOfficeMaintenance.GovernmentPseudoCityOrOfficeFlagNonNullable = false;
			}

			//GDSThirdPartyVendorFlag
			if (pseudoCityOrOfficeMaintenance.GDSThirdPartyVendorFlag == true)
			{
				pseudoCityOrOfficeMaintenance.GDSThirdPartyVendorFlagNonNullable = true;
			}
			else
			{
				pseudoCityOrOfficeMaintenance.GDSThirdPartyVendorFlagNonNullable = false;
			}

		}

		//Get PseudoCityOrOfficeMaintenances
		public List<PseudoCityOrOfficeMaintenance> GetPseudoCityOrOfficeMaintenances(int pseudoCityOrOfficeMaintenanceId)
		{
			return db.PseudoCityOrOfficeMaintenances.ToList();
		}
		
		//Get one PseudoCityOrOfficeMaintenance
		public PseudoCityOrOfficeMaintenance GetPseudoCityOrOfficeMaintenance(int pseudoCityOrOfficeMaintenanceId)
		{
			return db.PseudoCityOrOfficeMaintenances.SingleOrDefault(c => c.PseudoCityOrOfficeMaintenanceId == pseudoCityOrOfficeMaintenanceId);
		}

		//Get All PseudoCityOrOfficeMaintenance
		public IQueryable<PseudoCityOrOfficeMaintenance> GetAllPseudoCityOrOfficeMaintenances()
		{
			return db.PseudoCityOrOfficeMaintenances.OrderBy(c => c.PseudoCityOrOfficeMaintenanceId);
		}

		//Get PseudoCityOrOfficeDefinedRegions By GlobalRegionCode
		public List<PseudoCityOrOfficeDefinedRegionJSON> GetPseudoCityOrOfficeDefinedRegionsByGlobalRegionCode(string globalRegionCode)
		{
			var result = from n in db.PseudoCityOrOfficeDefinedRegions.Where(x => x.GlobalRegionCode == globalRegionCode).OrderBy(x => x.GlobalRegion.GlobalRegionName)
						 select new PseudoCityOrOfficeDefinedRegionJSON
						 {
							 PseudoCityOrOfficeDefinedRegionId = n.PseudoCityOrOfficeDefinedRegionId,
							 PseudoCityOrOfficeDefinedRegionName = n.PseudoCityOrOfficeDefinedRegionName.Trim(),
						 };
			return result.ToList();
		}

		//Get All PseudoCityOrOfficeMaintenance ClientSubUnits
		public List<ClientSubUnit> GetAllPseudoCityOrOfficeMaintenanceClientSubUnits(int pseudoCityOrOfficeMaintenanceId)
		{
			ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();

			List<ClientSubUnit> clientSubUnits = new List<ClientSubUnit>();

			List<PseudoCityOrOfficeMaintenanceClientSubUnit> pseudoCityOrOfficeMaintenanceClientSubUnits = db.PseudoCityOrOfficeMaintenanceClientSubUnits.Where(x => x.PseudoCityOrOfficeMaintenanceId == pseudoCityOrOfficeMaintenanceId).ToList();
			foreach (PseudoCityOrOfficeMaintenanceClientSubUnit pseudoCityOrOfficeMaintenanceClientSubUnit in pseudoCityOrOfficeMaintenanceClientSubUnits)
			{
				ClientSubUnit clientSubUnit = clientSubUnitRepository.GetClientSubUnit(pseudoCityOrOfficeMaintenanceClientSubUnit.ClientSubUnitGuid);
				if(clientSubUnit != null) {
					clientSubUnits.Add(clientSubUnit);
				}
			}

			return clientSubUnits;
		}
		
		//Add PseudoCityOrOfficeMaintenance
		public void Add(PseudoCityOrOfficeMaintenanceVM pseudoCityOrOfficeMaintenanceVM)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			XmlDocument clientSubUnitsDoc = GetClientSubUnitGuidsToXML(pseudoCityOrOfficeMaintenanceVM.ClientSubUnitGuids);
			XmlDocument gdsThirdPartyVendorsDoc = GetGDSThirdPartyVendorsToXML(pseudoCityOrOfficeMaintenanceVM.GDSThirdPartyVendors);
			
			db.spDesktopDataAdmin_InsertPseudoCityOrOfficeMaintenance_v1(
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeId,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.GDSCode,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.IATAId,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.AmadeusId,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeAddressId,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.LocationContactName,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.LocationPhone,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeDefinedRegionId,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.ActiveFlagNonNullable,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.InternalSiteName,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.ExternalNameId,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeTypeId,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeLocationTypeId,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.FareRedistributionId,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.GovernmentContract,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.CIDBPIN,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.SharedPseudoCityOrOfficeFlagNonNullable,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.CWTOwnedPseudoCityOrOfficeFlagNonNullable,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.ClientDedicatedPseudoCityOrOfficeFlagNonNullable,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.ClientGDSAccessFlagNonNullable,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.DevelopmentOrInternalPseudoCityOrOfficeFlagNonNullable,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.CubaPseudoCityOrOfficeFlagNonNullable,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.GovernmentPseudoCityOrOfficeFlagNonNullable,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.GDSThirdPartyVendorFlagNonNullable,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.VendorAssignedDate,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.InternalRemarks,
				System.Xml.Linq.XElement.Parse(clientSubUnitsDoc.OuterXml),
				System.Xml.Linq.XElement.Parse(gdsThirdPartyVendorsDoc.OuterXml),
				adminUserGuid
			);
        }

		//Edit PseudoCityOrOfficeMaintenance
		public void Update(PseudoCityOrOfficeMaintenanceVM pseudoCityOrOfficeMaintenanceVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			XmlDocument clientSubUnitsDoc = GetClientSubUnitGuidsToXML(pseudoCityOrOfficeMaintenanceVM.ClientSubUnitGuids);
			XmlDocument gdsThirdPartyVendorsDoc = GetGDSThirdPartyVendorsToXML(pseudoCityOrOfficeMaintenanceVM.GDSThirdPartyVendors); 
			
			db.spDesktopDataAdmin_UpdatePseudoCityOrOfficeMaintenance_v1(
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeMaintenanceId,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeId,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.GDSCode,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.IATAId,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.AmadeusId,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeAddressId,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.LocationContactName,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.LocationPhone,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeDefinedRegionId,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.ActiveFlagNonNullable,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.InternalSiteName,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.ExternalNameId,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeTypeId,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeLocationTypeId,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.FareRedistributionId,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.GovernmentContract,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.CIDBPIN,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.SharedPseudoCityOrOfficeFlagNonNullable,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.CWTOwnedPseudoCityOrOfficeFlagNonNullable,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.ClientDedicatedPseudoCityOrOfficeFlagNonNullable,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.ClientGDSAccessFlagNonNullable,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.DevelopmentOrInternalPseudoCityOrOfficeFlagNonNullable,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.CubaPseudoCityOrOfficeFlagNonNullable,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.GovernmentPseudoCityOrOfficeFlagNonNullable,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.GDSThirdPartyVendorFlagNonNullable,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.VendorAssignedDate,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.InternalRemarks,
				System.Xml.Linq.XElement.Parse(clientSubUnitsDoc.OuterXml),
				System.Xml.Linq.XElement.Parse(gdsThirdPartyVendorsDoc.OuterXml), 
				adminUserGuid,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.VersionNumber
			);
		}

		//Update Deleted Status for PseudoCityOrOfficeMaintenance
		public void UpdateDeletedStatus(PseudoCityOrOfficeMaintenanceVM pseudoCityOrOfficeMaintenanceVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdatePseudoCityOrOfficeMaintenanceDeletedStatus_v1(
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeMaintenanceId,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.DeletedFlag,
				adminUserGuid,
				pseudoCityOrOfficeMaintenanceVM.PseudoCityOrOfficeMaintenance.VersionNumber
			);
		}

		//ClientSubUnits to XML
		private XmlDocument GetClientSubUnitGuidsToXML(List<string> clientSubUnitGuids)
		{
			XmlDocument clientSubUnitsDoc = new XmlDocument();
			XmlDeclaration clientSubUnitsDec = clientSubUnitsDoc.CreateXmlDeclaration("1.0", null, null);
			clientSubUnitsDoc.AppendChild(clientSubUnitsDec);
			XmlElement root = clientSubUnitsDoc.CreateElement("ClientSubUnits");
			clientSubUnitsDoc.AppendChild(root);

			if (clientSubUnitGuids != null)
			{
				foreach (string clientSubUnitGuid in clientSubUnitGuids)
				{
					if (!string.IsNullOrEmpty(clientSubUnitGuid))
					{
						XmlElement xmlClientSubUnit = clientSubUnitsDoc.CreateElement("ClientSubUnit");

						XmlElement xmlClientSubUnitGuid = clientSubUnitsDoc.CreateElement("ClientSubUnitGuid");
						xmlClientSubUnitGuid.InnerText = clientSubUnitGuid;
						xmlClientSubUnit.AppendChild(xmlClientSubUnitGuid);

						root.AppendChild(xmlClientSubUnit);
					}
				}
			}

			return clientSubUnitsDoc;
		}

		//GDSThirdPartyVendors to XML
		private XmlDocument GetGDSThirdPartyVendorsToXML(List<GDSThirdPartyVendor> gdsThirdPartyVendors)
		{
			XmlDocument gdsThirdPartyVendorsDoc = new XmlDocument();
			XmlDeclaration gdsThirdPartyVendorsDec = gdsThirdPartyVendorsDoc.CreateXmlDeclaration("1.0", null, null);
			gdsThirdPartyVendorsDoc.AppendChild(gdsThirdPartyVendorsDec);
			XmlElement root = gdsThirdPartyVendorsDoc.CreateElement("GDSThirdPartyVendors");
			gdsThirdPartyVendorsDoc.AppendChild(root);

			if (gdsThirdPartyVendors != null)
			{
				foreach (GDSThirdPartyVendor gdsThirdPartyVendor in gdsThirdPartyVendors)
				{
					if (gdsThirdPartyVendor.GDSThirdPartyVendorId > 0)
					{
						XmlElement xmlGDSThirdPartyVendor = gdsThirdPartyVendorsDoc.CreateElement("GDSThirdPartyVendor");

						XmlElement xmlGDSThirdPartyVendorGuid = gdsThirdPartyVendorsDoc.CreateElement("GDSThirdPartyVendorId");
						xmlGDSThirdPartyVendorGuid.InnerText = gdsThirdPartyVendor.GDSThirdPartyVendorId.ToString();
						xmlGDSThirdPartyVendor.AppendChild(xmlGDSThirdPartyVendorGuid);

						root.AppendChild(xmlGDSThirdPartyVendor);
					}
				}
			}

			return gdsThirdPartyVendorsDoc;
		}

		//Export Items to CSV
		public byte[] Export()
		{
			StringBuilder sb = new StringBuilder();

						//Add Headers
			List<string> headers = new List<string>();
			headers.Add("Pseudo City/Office ID");
			headers.Add("IATA");
			headers.Add("GDS");
			headers.Add("Amadeus ID");
			headers.Add("Address");
			headers.Add("Location Contact Name");
			headers.Add("Location Phone");
			headers.Add("Country");
			headers.Add("Global Region");
			headers.Add("Pseudo City/Office ID Defined Region");
			headers.Add("Active");
			headers.Add("Internal Site Name");
			headers.Add("External Name");
			headers.Add("Client SubUnit");
			headers.Add("Pseudo City/Office ID Type");
			headers.Add("Pseudo City/Office ID Location Type");
			headers.Add("Fare Redistribution");
			headers.Add("Government Contract");
			headers.Add("CIDB/PIN");
			headers.Add("Shared PCC/Office ID?");
			headers.Add("CWT Owned PCC/Office ID?");
			headers.Add("Client Dedicated PCC/Office ID?");
			headers.Add("Client GDS Access?");
			headers.Add("Development/Internal PCC/Office ID?");
			headers.Add("Cuba PCC/Office ID?");
			headers.Add("Government PCC/Office ID?");
			headers.Add("3rd Party Vendor?");
			headers.Add("3rd Party Vendor(s)");
			headers.Add("Vendor Assigned Date");
			headers.Add("Internal Remarks");
			headers.Add("Creation TimeStamp");
			headers.Add("Last Update Time Stamp");
			headers.Add("Deleted Flag");
			headers.Add("Deleted Date Time");

			sb.AppendLine(String.Join(",", headers.Select(x => x.ToString()).ToArray()));

			//Add Items
			List<PseudoCityOrOfficeMaintenance> pseudoCityOrOfficeMaintenances = db.PseudoCityOrOfficeMaintenances.ToList();
			foreach (PseudoCityOrOfficeMaintenance item in pseudoCityOrOfficeMaintenances)
			{

				string date_format = "MM/dd/yy HH:mm";

				EditForDisplay(item);

				sb.AppendFormat(
					"{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33}",
					!string.IsNullOrEmpty(item.PseudoCityOrOfficeId) ? item.PseudoCityOrOfficeId : " ",
					!string.IsNullOrEmpty(item.IATA.IATANumber) ? item.IATA.IATANumber : " ",
					!string.IsNullOrEmpty(item.GDS.GDSName) ? item.GDS.GDSName : " ",
					!string.IsNullOrEmpty(item.AmadeusId) ? item.AmadeusId : " ",
					item.PseudoCityOrOfficeAddress != null && !string.IsNullOrEmpty(item.PseudoCityOrOfficeAddress.FirstAddressLine) ? item.PseudoCityOrOfficeAddress.FirstAddressLine : " ",
					!string.IsNullOrEmpty(item.LocationContactName) ? item.LocationContactName : " ",
					!string.IsNullOrEmpty(item.LocationPhone) ? item.LocationPhone : " ",
					!string.IsNullOrEmpty(item.CountryName) ? item.CountryName : " ",
					!string.IsNullOrEmpty(item.GlobalRegionCode) ? item.GlobalRegionCode : " ",
					item.PseudoCityOrOfficeDefinedRegion != null && !string.IsNullOrEmpty(item.PseudoCityOrOfficeDefinedRegion.PseudoCityOrOfficeDefinedRegionName) ? item.PseudoCityOrOfficeDefinedRegion.PseudoCityOrOfficeDefinedRegionName : " ",
					item.ActiveFlag == true ? "True" : "False",
					!string.IsNullOrEmpty(item.InternalSiteName) ? item.InternalSiteName : " ",
					item.ExternalName != null && !string.IsNullOrEmpty(item.ExternalName.ExternalName1) ? item.ExternalName.ExternalName1 : " ",
					item.ClientSubUnitsList != null ? string.Join(" | ", item.ClientSubUnitsList.Select(x => x.ClientSubUnitName)) : "",
					item.PseudoCityOrOfficeType != null && !string.IsNullOrEmpty(item.PseudoCityOrOfficeType.PseudoCityOrOfficeTypeName) ? item.PseudoCityOrOfficeType.PseudoCityOrOfficeTypeName : " ",
					item.PseudoCityOrOfficeLocationType != null && !string.IsNullOrEmpty(item.PseudoCityOrOfficeLocationType.PseudoCityOrOfficeLocationTypeName) ? item.PseudoCityOrOfficeLocationType.PseudoCityOrOfficeLocationTypeName : " ",
					item.FareRedistribution != null && !string.IsNullOrEmpty(item.FareRedistribution.FareRedistributionName) ? item.FareRedistribution.FareRedistributionName : " ",
					item.GovernmentPseudoCityOrOfficeFlag == true ? "True" : "False",
					!string.IsNullOrEmpty(item.CIDBPIN) ? item.CIDBPIN : " ",				
					item.SharedPseudoCityOrOfficeFlag == true ? "True" : "False",
					item.CWTOwnedPseudoCityOrOfficeFlag == true ? "True" : "False",
					item.ClientDedicatedPseudoCityOrOfficeFlag == true ? "True" : "False",
					item.ClientGDSAccessFlag == true ? "True" : "False",
					item.DevelopmentOrInternalPseudoCityOrOfficeFlag == true ? "True" : "False",
					item.CubaPseudoCityOrOfficeFlag == true ? "True" : "False",
					item.GovernmentPseudoCityOrOfficeFlag == true ? "True" : "False",
					item.GDSThirdPartyVendorFlag == true ? "True" : "False",
					item.GDSThirdPartyVendorsList != null ? string.Join(" | ", item.GDSThirdPartyVendorsList.Select(x => x.GDSThirdPartyVendorName)) : "",
					item.VendorAssignedDate.HasValue ? item.VendorAssignedDate.Value.ToString("MMM dd yyyy") : " ",
					!string.IsNullOrEmpty(item.InternalRemarks) ? item.InternalRemarks : " ",
					item.CreationTimestamp.HasValue ? item.CreationTimestamp.Value.ToString(date_format) : " ",
					item.LastUpdateTimestamp.HasValue ? item.LastUpdateTimestamp.Value.ToString(date_format) : " ",
					item.DeletedFlag == true ? "True" : "False",
					item.DeletedDateTime.HasValue ? item.LastUpdateTimestamp.Value.ToString(date_format) : " "
				);

				sb.Append(Environment.NewLine);
			}

			return Encoding.ASCII.GetBytes(sb.ToString());
		}
	}
}