using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using System.Web.Security;
namespace CWTDesktopDatabase.Repository
{
    public class ClientSubUnitRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
		private ClientSubUnitAttributeRepository clientSubUnitAttributeRepository = new ClientSubUnitAttributeRepository();

        //List of All Items - Sortable
        public IQueryable<ClientSubUnit> GetClientSubUnits(string filter, string sortField, int sortOrder)
        {
            if (sortOrder == 0)
            {
                sortField = sortField + " ascending";
            }
            else
            {
                sortField = sortField + " descending";
            }

            if (filter == "")
            {
                return db.ClientSubUnits.OrderBy(sortField);
            }
            else
            {
                return db.ClientSubUnits.OrderBy(sortField).Where(c => c.ClientSubUnitName.Contains(filter));
            }

        }

        //Get one Item
        public ClientSubUnit GetClientSubUnit(string id)
        {
			ClientSubUnit clientSubUnit = new ClientSubUnit();

			clientSubUnit = db.ClientSubUnits.SingleOrDefault(c => c.ClientSubUnitGuid == id);
			if (clientSubUnit != null)
			{
				//RestrictedClientFlag
				ClientSubUnitAttribute clientSubUnitAttributeRestrictedClient = clientSubUnitAttributeRepository.GetClientSubUnitAttributeByType(clientSubUnit.ClientSubUnitGuid, "RestrictedClientFlag");
				if (clientSubUnitAttributeRestrictedClient != null && clientSubUnitAttributeRestrictedClient.AttributeValue != null)
				{
					//Attribute saved as a varchar
					clientSubUnit.RestrictedClient = (clientSubUnitAttributeRestrictedClient.AttributeValue == "1");
				}

				//PrivateClientFlag
				ClientSubUnitAttribute clientSubUnitAttributePrivateClient = clientSubUnitAttributeRepository.GetClientSubUnitAttributeByType(clientSubUnit.ClientSubUnitGuid, "PrivateClientFlag");
				if (clientSubUnitAttributePrivateClient != null && clientSubUnitAttributePrivateClient.AttributeValue != null)
				{
					//Attribute saved as a varchar
					clientSubUnit.PrivateClient = (clientSubUnitAttributePrivateClient.AttributeValue == "1");
				}

				//CubaBookingAllowedFlag
				ClientSubUnitAttribute cubaBookingAllowedFlag = clientSubUnitAttributeRepository.GetClientSubUnitAttributeByType(clientSubUnit.ClientSubUnitGuid, "CubaBookingAllowedFlag");
				if (cubaBookingAllowedFlag != null && cubaBookingAllowedFlag.AttributeValue != null)
				{
					//Attribute saved as a varchar
					clientSubUnit.CubaBookingAllowed = (cubaBookingAllowedFlag.AttributeValue == "1");
				}

				//InCountryServiceOnlyFlag
				ClientSubUnitAttribute inCountryServiceOnlyFlag = clientSubUnitAttributeRepository.GetClientSubUnitAttributeByType(clientSubUnit.ClientSubUnitGuid, "InCountryServiceOnlyFlag");
				if (inCountryServiceOnlyFlag != null && inCountryServiceOnlyFlag.AttributeValue != null)
				{
					//Attribute saved as a varchar
					clientSubUnit.InCountryServiceOnly = (inCountryServiceOnlyFlag.AttributeValue == "1");
				}

				//24HourServiceCenterTelephoneNumber
				ClientSubUnitAttribute clientSubUnitAttribute24Hours = clientSubUnitAttributeRepository.GetClientSubUnitAttributeByType(clientSubUnit.ClientSubUnitGuid, "24HourServiceCenterTelephoneNumber");
				if (clientSubUnitAttribute24Hours != null && clientSubUnitAttribute24Hours.AttributeValue != null)
				{
					clientSubUnit.DialledNumber24HSC = clientSubUnitAttribute24Hours.AttributeValue;
				}

				//Line of Business
				ClientSubUnitLineOfBusinessRepository clientSubUnitLineOfBusinessRepository = new ClientSubUnitLineOfBusinessRepository();
				ClientSubUnitLineOfBusiness clientSubUnitLineOfBusiness = clientSubUnitLineOfBusinessRepository.GetClientSubUnitLineOfBusiness(clientSubUnit.ClientSubUnitGuid);
				if (clientSubUnitLineOfBusiness != null)
				{
					LineOfBusinessRepository lineOfBusinessRepository = new LineOfBusinessRepository();
					LineOfBusiness lineOfBusiness = lineOfBusinessRepository.GetLineOfBusiness(clientSubUnitLineOfBusiness.LineOfBusinessId);
					if (lineOfBusiness != null)
					{
						clientSubUnit.LineOfBusiness = lineOfBusiness;
					}
					
					clientSubUnit.LineOfBusinessId = clientSubUnitLineOfBusiness.LineOfBusinessId;
				}

				//BranchContactNumber
				ClientSubUnitAttribute clientSubUnitAttributeBranchContactNumber = clientSubUnitAttributeRepository.GetClientSubUnitAttributeByType(clientSubUnit.ClientSubUnitGuid, "BranchContactNumber");
				if (clientSubUnitAttributeBranchContactNumber != null && clientSubUnitAttributeBranchContactNumber.AttributeValue != null)
				{
					clientSubUnit.BranchContactNumber = clientSubUnitAttributeBranchContactNumber.AttributeValue;
				}

				//BranchFaxNumber
				ClientSubUnitAttribute clientSubUnitAttributeBranchFaxNumber = clientSubUnitAttributeRepository.GetClientSubUnitAttributeByType(clientSubUnit.ClientSubUnitGuid, "BranchFaxNumber");
				if (clientSubUnitAttributeBranchFaxNumber != null && clientSubUnitAttributeBranchFaxNumber.AttributeValue != null)
				{
					clientSubUnit.BranchFaxNumber = clientSubUnitAttributeBranchFaxNumber.AttributeValue;
				}

				//BranchEmail
				ClientSubUnitAttribute clientSubUnitAttributeBranchEmail = clientSubUnitAttributeRepository.GetClientSubUnitAttributeByType(clientSubUnit.ClientSubUnitGuid, "BranchEmail");
				if (clientSubUnitAttributeBranchEmail != null && clientSubUnitAttributeBranchEmail.AttributeValue != null)
				{
					clientSubUnit.BranchEmail = clientSubUnitAttributeBranchEmail.AttributeValue;
				}

                //ClientIATA
                ClientSubUnitAttribute clientSubUnitAttributeClientIATA = clientSubUnitAttributeRepository.GetClientSubUnitAttributeByType(clientSubUnit.ClientSubUnitGuid, "ClientIATA");
                if (clientSubUnitAttributeClientIATA != null && clientSubUnitAttributeClientIATA.AttributeValue != null)
                {
                    clientSubUnit.ClientIATA = clientSubUnitAttributeClientIATA.AttributeValue;
                }

                //PortraitStatusDescription
                ClientSubUnitAttribute clientSubUnitAttributePortraitStatusDescription = clientSubUnitAttributeRepository.GetClientSubUnitAttributeByType(clientSubUnit.ClientSubUnitGuid, "PortraitStatusDescription");
                if (clientSubUnitAttributePortraitStatusDescription != null && clientSubUnitAttributePortraitStatusDescription.AttributeValue != null)
                {
                    clientSubUnit.PortraitStatusDescription = clientSubUnitAttributePortraitStatusDescription.AttributeValue;
                }

                //ClientBusinessDescription
                ClientSubUnitAttribute clientSubUnitAttributeClientBusinessDescription = clientSubUnitAttributeRepository.GetClientSubUnitAttributeByType(clientSubUnit.ClientSubUnitGuid, "ClientBusinessDescription");
                if (clientSubUnitAttributeClientBusinessDescription != null && clientSubUnitAttributeClientBusinessDescription.AttributeValue != null)
                {
                    clientSubUnit.ClientBusinessDescription = clientSubUnitAttributeClientBusinessDescription.AttributeValue;
                }
            }

			return clientSubUnit;

        }

        public ClientTopUnit GetClientSubUnitClientTopUnit(string clientSubUnitGuid)
        {
            ClientTopUnit clientTopUnit = new ClientTopUnit();

            if (!string.IsNullOrEmpty(clientSubUnitGuid))
            {
                ClientSubUnit clientSubUnit = GetClientSubUnit(clientSubUnitGuid);
                EditGroupForDisplay(clientSubUnit);

                if (clientSubUnit != null && clientSubUnit.ClientTopUnit != null)
                {
                    clientTopUnit = clientSubUnit.ClientTopUnit;
                }
            }

            return clientTopUnit;
        }

        public string GetClientSubUnitClientTopUnitName(string clientSubUnitGuid)
        {
            string clientTopUnitName = "";

            if (!string.IsNullOrEmpty(clientSubUnitGuid))
            {
                ClientSubUnit clientSubUnit = GetClientSubUnit(clientSubUnitGuid);
                EditGroupForDisplay(clientSubUnit);

                if (clientSubUnit != null && clientSubUnit.ClientTopUnit != null)
                {
                    clientTopUnitName = clientSubUnit.ClientTopUnit.ClientTopUnitName;
                }
            }

            return clientTopUnitName;
        }

        //Add Data From Linked Tables for Display
        public void EditGroupForDisplay(ClientSubUnit clientSubUnit)
        {
            //PortraitStatusRepository portraitStatusRepository = new PortraitStatusRepository();
            //PortraitStatus portraitStatus = new PortraitStatus();
            //portraitStatus = portraitStatusRepository.GetPortraitStatus(clientSubUnit.PortraitStatusId);
            //if (portraitStatus != null)
            //{
               // clientSubUnit.PortraitStatus = portraitStatus.PortraitStatusDescription;
            //}

            CountryRepository countryRepository = new CountryRepository();
            Country country = new Country();
            country = countryRepository.GetCountry(clientSubUnit.CountryCode);
            if (country != null)
            {
                clientSubUnit.CountryName = country.CountryName;
            }

            ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientSubUnit.ClientTopUnitGuid);
            if (clientTopUnit != null)
            {
                clientSubUnit.ClientTopUnitName = clientTopUnit.ClientTopUnitName;
            }

        }

        //List of All ClientSubUnit's TravelerTypes
        public IQueryable<fnDesktopDataAdmin_SelectClientSubUnitTravelerTypes_v1Result> GetClientSubUnitTravelerTypes(string id)
        {
            return db.fnDesktopDataAdmin_SelectClientSubUnitTravelerTypes_v1(id).OrderBy(c => c.TravelerTypeName);

        }

    }
}
