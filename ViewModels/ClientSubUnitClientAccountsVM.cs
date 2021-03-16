using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientSubUnitClientAccountsVM : CWTBaseViewModel
    {
        public ClientSubUnit ClientSubUnit { get; set; }
        public List<spDDAWizard_SelectClientSubUnitClientAccounts_v1Result> ClientAccounts { get; set; }
        
        public ClientSubUnitClientAccountsVM()
        {
        }
        public ClientSubUnitClientAccountsVM(ClientSubUnit clientSubUnit, List<spDDAWizard_SelectClientSubUnitClientAccounts_v1Result> clientAccounts)
        {
            ClientSubUnit = clientSubUnit;
            ClientAccounts = clientAccounts;
        }
    }
}
