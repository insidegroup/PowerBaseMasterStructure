using System.Collections.Generic;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;


namespace CWTDesktopDatabase.ViewModels
{
    public class MeetingClientSubUnitCountriesVM : CWTBaseViewModel
    {
        public int MeetingId { get; set; }
        public string MeetingName { get; set; }
		public string ClientTopUnitName { get; set; }
        public int VersionNumber { get; set; }
        public List<ClientSubUnitCountryVM> ClientSubUnitsAvailable { get; set; }
        public List<ClientSubUnitCountryVM> ClientSubUnitsUnAvailable { get; set; }
        public bool HasWriteAccess { get; set; }

        public MeetingClientSubUnitCountriesVM()
        {
        }
        public MeetingClientSubUnitCountriesVM(
			int meetingId, 
			string meetingName, 
			string clientTopUnitName,
			int versionNumber, List<ClientSubUnitCountryVM> clientSubUnitsAvailable, 
			List<ClientSubUnitCountryVM> clientSubUnitsUnAvailable,
			bool hasWriteAccess)
        {
            MeetingName = meetingName;
            MeetingId = meetingId;
			ClientTopUnitName = clientTopUnitName;
            VersionNumber = versionNumber;
            ClientSubUnitsAvailable = clientSubUnitsAvailable;
            ClientSubUnitsUnAvailable = clientSubUnitsUnAvailable;
            HasWriteAccess = hasWriteAccess;
        }
    }
}