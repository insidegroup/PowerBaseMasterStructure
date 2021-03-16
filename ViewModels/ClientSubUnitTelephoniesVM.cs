using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientSubUnitTelephoniesVM : CWTBaseViewModel
    {
        public ClientSubUnit ClientSubUnit { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitTelephony_v1Result> Telephonies { get; set; }
        
        public ClientSubUnitTelephoniesVM()
        {
        }
        public ClientSubUnitTelephoniesVM(ClientSubUnit clientSubUnit, CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitTelephony_v1Result> telephonies)
        {
            ClientSubUnit = clientSubUnit;
            Telephonies = telephonies;
        }
    }
}