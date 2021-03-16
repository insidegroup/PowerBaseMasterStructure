using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientSubUnitTravelerTypeESCInformationVM : CWTBaseViewModel
     {
         public ClientSubUnit ClientSubUnit { get; set; }
         public TravelerType TravelerType { get; set; }
         public ClientDetail ClientDetail { get; set; }
        public ClientDetailESCInformation ClientDetailESCInformation { get; set; }

        public ClientSubUnitTravelerTypeESCInformationVM()
        {
        }
        public ClientSubUnitTravelerTypeESCInformationVM(ClientSubUnit clientSubUnit, TravelerType travelerType, ClientDetail clientDetail, ClientDetailESCInformation clientDetailESCInformation)
        {
            ClientSubUnit = clientSubUnit;
            TravelerType = travelerType;
            ClientDetail = clientDetail;
            ClientDetailESCInformation = clientDetailESCInformation;
        }
    }
}