using System.Collections.Generic;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class QueueMinderGroupClientSubUnitCountriesVM : CWTBaseViewModel
    {
        public int QueueMinderGroupId { get; set; }
        public string QueueMinderGroupName { get; set; }
        public int VersionNumber { get; set; }
        public List<ClientSubUnitCountryVM> ClientSubUnitsAvailable { get; set; }
        public List<ClientSubUnitCountryVM> ClientSubUnitsUnAvailable { get; set; }

        public QueueMinderGroupClientSubUnitCountriesVM()
        {
        }
        public QueueMinderGroupClientSubUnitCountriesVM(int queueMinderGroupId, string queueMinderGroupName, int versionNumber, List<ClientSubUnitCountryVM> clientSubUnitsAvailable
            , List<ClientSubUnitCountryVM> clientSubUnitsUnAvailable)
        {
			QueueMinderGroupName = queueMinderGroupName;
			QueueMinderGroupId = queueMinderGroupId;
            VersionNumber = versionNumber;
            ClientSubUnitsAvailable = clientSubUnitsAvailable;
            ClientSubUnitsUnAvailable = clientSubUnitsUnAvailable;
        }
    }
}