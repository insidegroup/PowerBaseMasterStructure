using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientAccountESCInformationVM : CWTBaseViewModel
     {
        public ClientAccount ClientAccount { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public ClientDetailESCInformation ClientDetailESCInformation { get; set; }

        public ClientAccountESCInformationVM()
        {
        }
        public ClientAccountESCInformationVM(ClientAccount clientAccount, ClientDetail clientDetail, ClientDetailESCInformation clientDetailESCInformation)
        {
            ClientAccount = clientAccount;
            ClientDetail = clientDetail;
            ClientDetailESCInformation = clientDetailESCInformation;
        }
    }
}