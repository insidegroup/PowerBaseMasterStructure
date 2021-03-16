using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class TravelerTypeVM : CWTBaseViewModel
    {
        public TravelerType TravelerType { get; set; }
        public TravelerTypeSponsor TravelerTypeSponsor { get; set; }
        public ClientSubUnit ClientSubUnit { get; set; }
        public ClientTopUnit ClientTopUnit { get; set; }

        public TravelerTypeVM()
        {
        }
        public TravelerTypeVM(TravelerType travelerType, TravelerTypeSponsor travelerTypeSponsor, ClientTopUnit clientTopUnit, ClientSubUnit clientSubUnit)
        {
            TravelerType = travelerType;
            TravelerTypeSponsor = travelerTypeSponsor;
            ClientSubUnit = clientSubUnit;
            ClientTopUnit = clientTopUnit;
        }
    }
}