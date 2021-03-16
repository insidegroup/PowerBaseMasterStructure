using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientAccountClientDetailsVM : CWTBaseViewModel
    {
        public ClientAccount ClientAccount { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientAccountClientDetails_v1Result> ClientDetails { get; set; }
        
        public ClientAccountClientDetailsVM()
        {
        }
        public ClientAccountClientDetailsVM(ClientAccount clientAccount, CWTPaginatedList<spDesktopDataAdmin_SelectClientAccountClientDetails_v1Result> clientDetails)
        {
            ClientAccount = clientAccount;
            ClientDetails = clientDetails;
        }
    }
}