using System.Collections.Generic;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class PolicyGroupClientSubUnitCountriesVM : CWTBaseViewModel
    {
        public int PolicyGroupId { get; set; }
        public string PolicyGroupName { get; set; }
        public int VersionNumber { get; set; }
        public List<ClientSubUnitCountryVM> ClientSubUnitsAvailable { get; set; }
        public List<ClientSubUnitCountryVM> ClientSubUnitsUnAvailable { get; set; }

        public PolicyGroupClientSubUnitCountriesVM()
        {
        }
        public PolicyGroupClientSubUnitCountriesVM(int policyGroupId, string policyGroupName, int versionNumber, List<ClientSubUnitCountryVM> clientSubUnitsAvailable
            , List<ClientSubUnitCountryVM> clientSubUnitsUnAvailable)
        {
            PolicyGroupName = policyGroupName;
            PolicyGroupId = policyGroupId;
            VersionNumber = versionNumber;
            ClientSubUnitsAvailable = clientSubUnitsAvailable;
            ClientSubUnitsUnAvailable = clientSubUnitsUnAvailable;
        }
    }
}