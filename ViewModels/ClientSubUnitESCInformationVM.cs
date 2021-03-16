using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientSubUnitESCInformationVM : CWTBaseViewModel
     {
        public ClientSubUnit ClientSubUnit { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public ClientDetailESCInformation ClientDetailESCInformation { get; set; }

        public ClientSubUnitESCInformationVM()
        {
        }
        public ClientSubUnitESCInformationVM(ClientSubUnit clientSubUnit, ClientDetail clientDetail, ClientDetailESCInformation clientDetailESCInformation)
        {
            ClientSubUnit = clientSubUnit;
            ClientDetail = clientDetail;
            ClientDetailESCInformation = clientDetailESCInformation;
        }
    }
}