using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientTopUnitAndClientSubUnitsVM : CWTBaseViewModel
   {
        public ClientTopUnit ClientTopUnit { get; set; }
        public List<ClientSubUnit> ClientSubUnits { get; set; }
        
        public ClientTopUnitAndClientSubUnitsVM()
        {
        }
        public ClientTopUnitAndClientSubUnitsVM(ClientTopUnit clientTopUnit, List<ClientSubUnit> clientSubUnits)
        {
            ClientTopUnit = clientTopUnit;
            ClientSubUnits = clientSubUnits;
        }
    }
}
