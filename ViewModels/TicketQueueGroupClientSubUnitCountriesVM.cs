using System.Collections.Generic;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class TicketQueueGroupClientSubUnitCountriesVM : CWTBaseViewModel
    {
        public int TicketQueueGroupId { get; set; }
        public string TicketQueueGroupName { get; set; }
        public int VersionNumber { get; set; }
        public List<ClientSubUnitCountryVM> ClientSubUnitsAvailable { get; set; }
        public List<ClientSubUnitCountryVM> ClientSubUnitsUnAvailable { get; set; }

        public TicketQueueGroupClientSubUnitCountriesVM()
        {
        }
        public TicketQueueGroupClientSubUnitCountriesVM(int ticketQueueGroupId, string ticketQueueGroupName, int versionNumber, List<ClientSubUnitCountryVM> clientSubUnitsAvailable
            , List<ClientSubUnitCountryVM> clientSubUnitsUnAvailable)
        {
			TicketQueueGroupName = ticketQueueGroupName;
			TicketQueueGroupId = ticketQueueGroupId;
            VersionNumber = versionNumber;
            ClientSubUnitsAvailable = clientSubUnitsAvailable;
            ClientSubUnitsUnAvailable = clientSubUnitsUnAvailable;
        }
    }
}