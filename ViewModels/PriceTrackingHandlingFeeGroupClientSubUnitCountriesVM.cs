using System.Collections.Generic;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;


namespace CWTDesktopDatabase.ViewModels
{
    public class PriceTrackingHandlingFeeGroupClientSubUnitCountriesVM : CWTBaseViewModel
    {
        public int PriceTrackingHandlingFeeGroupId { get; set; }
        public string PriceTrackingHandlingFeeGroupName { get; set; }
        public int VersionNumber { get; set; }
        public List<ClientSubUnitCountryVM> ClientSubUnitsAvailable { get; set; }
        public List<ClientSubUnitCountryVM> ClientSubUnitsUnAvailable { get; set; }
        public bool HasWriteAccess { get; set; }

        public PriceTrackingHandlingFeeGroupClientSubUnitCountriesVM()
        {
        }
        public PriceTrackingHandlingFeeGroupClientSubUnitCountriesVM(int priceTrackingHandlingFeeGroupId, string priceTrackingHandlingFeeGroupName, int versionNumber, List<ClientSubUnitCountryVM> clientSubUnitsAvailable
            , List<ClientSubUnitCountryVM> clientSubUnitsUnAvailable, bool hasWriteAccess)
        {
            PriceTrackingHandlingFeeGroupName = priceTrackingHandlingFeeGroupName;
            PriceTrackingHandlingFeeGroupId = priceTrackingHandlingFeeGroupId;
            VersionNumber = versionNumber;
            ClientSubUnitsAvailable = clientSubUnitsAvailable;
            ClientSubUnitsUnAvailable = clientSubUnitsUnAvailable;
            HasWriteAccess = hasWriteAccess;
        }
    }
}