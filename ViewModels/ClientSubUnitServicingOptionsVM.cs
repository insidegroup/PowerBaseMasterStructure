using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientSubUnitServicingOptionsVM : CWTBaseViewModel
    {
        public ClientSubUnit ClientSubUnit { get; set; }
        public List<spDDAWizard_SelectClientSubUnitServicingOptions_v1Result> ServicingOptions { get; set; }
        
        public ClientSubUnitServicingOptionsVM()
        {
        }
        public ClientSubUnitServicingOptionsVM(ClientSubUnit clientSubUnit, List<spDDAWizard_SelectClientSubUnitServicingOptions_v1Result> servicingOptions)
        {
            ClientSubUnit = clientSubUnit;
            ServicingOptions = servicingOptions;
        }
    }
}
