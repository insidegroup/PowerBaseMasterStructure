using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class TravelerTypeContactsVM : CWTBaseViewModel
    {
        public ClientSubUnit ClientSubUnit { get; set; }
        public TravelerType TravelerType { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailContacts_v1Result> Contacts { get; set; }
        
        public TravelerTypeContactsVM()
        {
        }
        public TravelerTypeContactsVM(ClientSubUnit clientSubUnit, TravelerType travelerType, ClientDetail clientDetail, CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailContacts_v1Result> contacts)
        {
            ClientSubUnit = clientSubUnit;
            TravelerType = travelerType;
            ClientDetail = clientDetail;
            Contacts = contacts;
        }
    }
}