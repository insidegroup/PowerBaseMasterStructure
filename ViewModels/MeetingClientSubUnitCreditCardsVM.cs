using System.Collections.Generic;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;


namespace CWTDesktopDatabase.ViewModels
{
    public class MeetingClientSubUnitCreditCardsVM : CWTBaseViewModel
    {
        public int MeetingId { get; set; }
        public string MeetingName { get; set; }
		public string ClientTopUnitName { get; set; }
        public int VersionNumber { get; set; }
		public List<MeetingLinkedCreditCardVM> CreditCardsAvailable { get; set; }
		public List<MeetingLinkedCreditCardVM> CreditCardsUnAvailable { get; set; }
        public bool HasWriteAccess { get; set; }

        public MeetingClientSubUnitCreditCardsVM()
        {
        }

		public MeetingClientSubUnitCreditCardsVM(
			int meetingId, 
			string meetingName, 
			string clientTopUnitName,
			int versionNumber,
			List<MeetingLinkedCreditCardVM> creditCardsAvailable,
			List<MeetingLinkedCreditCardVM> creditCardsUnAvailable,
			bool hasWriteAccess)
        {
            MeetingName = meetingName;
            MeetingId = meetingId;
			ClientTopUnitName = clientTopUnitName;
            VersionNumber = versionNumber;
			CreditCardsAvailable = creditCardsAvailable;
			CreditCardsUnAvailable = creditCardsUnAvailable;
            HasWriteAccess = hasWriteAccess;
        }
    }
}