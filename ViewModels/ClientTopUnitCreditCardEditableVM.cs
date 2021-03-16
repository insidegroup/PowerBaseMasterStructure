using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientTopUnitCreditCardEditableVM : CWTBaseViewModel
   {
        public CreditCardEditable CreditCard { get; set; }
        public ClientTopUnit ClientTopUnit { get; set; }
        public IEnumerable<SelectListItem> CreditCardTypes { get; set; }
        public IEnumerable<SelectListItem> CreditCardVendors { get; set; }

         public ClientTopUnitCreditCardEditableVM()
        {
        }
         public ClientTopUnitCreditCardEditableVM(
                                CreditCardEditable creditCard,
                                ClientTopUnit clientTopUnit,
                                IEnumerable<SelectListItem> creditCardTypes,
                                IEnumerable<SelectListItem> creditCardVendors
                                )
        {
            CreditCard = creditCard;
            ClientTopUnit = clientTopUnit;
            CreditCardTypes = creditCardTypes;
            CreditCardVendors = creditCardVendors;
        }
       
    }
}