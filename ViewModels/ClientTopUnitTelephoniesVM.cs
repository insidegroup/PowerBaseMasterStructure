using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientTopUnitTelephoniesVM : CWTBaseViewModel
    {
        public ClientTopUnit ClientTopUnit { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientTopUnitTelephony_v1Result> Telephonies { get; set; }
        
        public ClientTopUnitTelephoniesVM()
        {
        }
        public ClientTopUnitTelephoniesVM(ClientTopUnit clientTopUnit, ClientDetail clientDetail, CWTPaginatedList<spDesktopDataAdmin_SelectClientTopUnitTelephony_v1Result> telephonies)
        {
            ClientTopUnit = clientTopUnit;
            Telephonies = telephonies;
        }
    }
}