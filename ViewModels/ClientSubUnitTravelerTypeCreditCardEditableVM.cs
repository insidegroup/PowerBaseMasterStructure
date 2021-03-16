using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientSubUnitTravelerTypeCreditCardEditableVM : CWTBaseViewModel
    {
        public CreditCardEditable CreditCard { get; set; }
        public TravelerType TravelerType { get; set; }
        public ClientSubUnit ClientSubUnit { get; set; }
        public IEnumerable<SelectListItem> CreditCardTypes { get; set; }
        public IEnumerable<SelectListItem> CreditCardVendors { get; set; }

         public ClientSubUnitTravelerTypeCreditCardEditableVM()
        {
        }
         public ClientSubUnitTravelerTypeCreditCardEditableVM(
                                CreditCardEditable creditCard,
                                TravelerType travelerType,
                                ClientSubUnit clientSubUnit,
                                IEnumerable<SelectListItem> creditCardTypes,
                                IEnumerable<SelectListItem> creditCardVendors
                                )
        {
            CreditCard = creditCard;
            TravelerType = travelerType;
            ClientSubUnit = clientSubUnit;
            CreditCardTypes = creditCardTypes;
            CreditCardVendors = creditCardVendors;
        }
       
    }
}