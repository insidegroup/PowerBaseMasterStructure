using System.Collections.Generic;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ReasonCodeGroupClientSubUnitCountriesVM : CWTBaseViewModel
    {
        public int ReasonCodeGroupId { get; set; }
        public string ReasonCodeGroupName { get; set; }
        public int VersionNumber { get; set; }
        public List<ClientSubUnitCountryVM> ClientSubUnitsAvailable { get; set; }
        public List<ClientSubUnitCountryVM> ClientSubUnitsUnAvailable { get; set; }

        public ReasonCodeGroupClientSubUnitCountriesVM()
        {
        }
        public ReasonCodeGroupClientSubUnitCountriesVM(int reasonCodeGroupId, string reasonCodeGroupName, int versionNumber, List<ClientSubUnitCountryVM> clientSubUnitsAvailable
            , List<ClientSubUnitCountryVM> clientSubUnitsUnAvailable)
        {
            ReasonCodeGroupName = reasonCodeGroupName;
            ReasonCodeGroupId = reasonCodeGroupId;
            VersionNumber = versionNumber;
            ClientSubUnitsAvailable = clientSubUnitsAvailable;
            ClientSubUnitsUnAvailable = clientSubUnitsUnAvailable;
        }
    }
}