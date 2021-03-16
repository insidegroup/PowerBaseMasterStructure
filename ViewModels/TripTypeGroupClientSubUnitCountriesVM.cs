using System.Collections.Generic;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class TripTypeGroupClientSubUnitCountriesVM : CWTBaseViewModel
    {
        public int TripTypeGroupId { get; set; }
        public string TripTypeGroupName { get; set; }
        public int VersionNumber { get; set; }
        public List<ClientSubUnitCountryVM> ClientSubUnitsAvailable { get; set; }
        public List<ClientSubUnitCountryVM> ClientSubUnitsUnAvailable { get; set; }

        public TripTypeGroupClientSubUnitCountriesVM()
        {
        }
        public TripTypeGroupClientSubUnitCountriesVM(int tripTypeGroupId, string tripTypeGroupName, int versionNumber, List<ClientSubUnitCountryVM> clientSubUnitsAvailable
            , List<ClientSubUnitCountryVM> clientSubUnitsUnAvailable)
        {
			TripTypeGroupName = tripTypeGroupName;
			TripTypeGroupId = tripTypeGroupId;
            VersionNumber = versionNumber;
            ClientSubUnitsAvailable = clientSubUnitsAvailable;
            ClientSubUnitsUnAvailable = clientSubUnitsUnAvailable;
        }
    }
}