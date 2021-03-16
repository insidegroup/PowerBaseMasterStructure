using System.Collections.Generic;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;


namespace CWTDesktopDatabase.ViewModels
{
    public class ServicingOptionGroupClientSubUnitCountriesVM : CWTBaseViewModel
    {
        public int ServicingOptionGroupId { get; set; }
        public string ServicingOptionGroupName { get; set; }
        public int VersionNumber { get; set; }
        public List<ClientSubUnitCountryVM> ClientSubUnitsAvailable { get; set; }
        public List<ClientSubUnitCountryVM> ClientSubUnitsUnAvailable { get; set; }
        public bool HasWriteAccess { get; set; }

        public ServicingOptionGroupClientSubUnitCountriesVM()
        {
        }
        public ServicingOptionGroupClientSubUnitCountriesVM(int servicingOptionGroupId, string servicingOptionGroupName, int versionNumber, List<ClientSubUnitCountryVM> clientSubUnitsAvailable
            , List<ClientSubUnitCountryVM> clientSubUnitsUnAvailable, bool hasWriteAccess)
        {
            ServicingOptionGroupName = servicingOptionGroupName;
            ServicingOptionGroupId = servicingOptionGroupId;
            VersionNumber = versionNumber;
            ClientSubUnitsAvailable = clientSubUnitsAvailable;
            ClientSubUnitsUnAvailable = clientSubUnitsUnAvailable;
            HasWriteAccess = hasWriteAccess;
        }
    }
}