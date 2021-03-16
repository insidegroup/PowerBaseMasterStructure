using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
    public class PriceTrackingSetupGroupClientSubUnitCountriesVM : CWTBaseViewModel
    {
        public int PriceTrackingSetupGroupId { get; set; }
        public string PriceTrackingSetupGroupName { get; set; }
        public int VersionNumber { get; set; }
        public List<ClientSubUnitCountryVM> ClientSubUnitsAvailable { get; set; }
        public List<ClientSubUnitCountryVM> ClientSubUnitsUnAvailable { get; set; }

        public PriceTrackingSetupGroupClientSubUnitCountriesVM()
        {
        }

        public PriceTrackingSetupGroupClientSubUnitCountriesVM(
            int priceTrackingSetupGroupId, 
            string priceTrackingSetupGroupName, 
            int versionNumber, 
            List<ClientSubUnitCountryVM> clientSubUnitsAvailable, 
            List<ClientSubUnitCountryVM> clientSubUnitsUnAvailable
        )
        {
            PriceTrackingSetupGroupName = priceTrackingSetupGroupName;
            PriceTrackingSetupGroupId = priceTrackingSetupGroupId;
            VersionNumber = versionNumber;
            ClientSubUnitsAvailable = clientSubUnitsAvailable;
            ClientSubUnitsUnAvailable = clientSubUnitsUnAvailable;
        }
    }
}