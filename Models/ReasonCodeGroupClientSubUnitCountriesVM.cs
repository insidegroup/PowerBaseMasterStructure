using System.Collections.Generic;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Models
{
    public class ReasonCodeGroupClientSubUnitCountriesVM
    {
        public int ReasonCodeGroupId { get; set; }
        public string ReasonCodeGroupName { get; set; }
        public int VersionNumber { get; set; }
        public List<ReasonCodeClientSubUnitCountryVM> ClientSubUnitsAvailable { get; set; }
        public List<ReasonCodeClientSubUnitCountryVM> ClientSubUnitsUnAvailable { get; set; }

        public ReasonCodeGroupClientSubUnitCountriesVM()
        {
        }
        public ReasonCodeGroupClientSubUnitCountriesVM(int reasonCodeGroupId, string reasonCodeGroupName, int versionNumber, List<ReasonCodeClientSubUnitCountryVM> clientSubUnitsAvailable
            , List<ReasonCodeClientSubUnitCountryVM> clientSubUnitsUnAvailable)
        {
            ReasonCodeGroupName = reasonCodeGroupName;
            ReasonCodeGroupId = reasonCodeGroupId;
            VersionNumber = versionNumber;
            ClientSubUnitsAvailable = clientSubUnitsAvailable;
            ClientSubUnitsUnAvailable = clientSubUnitsUnAvailable;
        }
    }
}