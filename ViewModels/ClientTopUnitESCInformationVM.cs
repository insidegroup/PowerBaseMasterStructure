using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientTopUnitESCInformationVM : CWTBaseViewModel
     {
        public ClientTopUnit ClientTopUnit { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public ClientDetailESCInformation ClientDetailESCInformation { get; set; }

        public ClientTopUnitESCInformationVM()
        {
        }
        public ClientTopUnitESCInformationVM(ClientTopUnit clientTopUnit, ClientDetail clientDetail, ClientDetailESCInformation clientDetailESCInformation)
        {
            ClientTopUnit = clientTopUnit;
            ClientDetail = clientDetail;
            ClientDetailESCInformation = clientDetailESCInformation;
        }
    }
}