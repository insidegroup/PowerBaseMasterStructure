using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientTopUnitCreditCardsVM : CWTBaseViewModel
     {
        public ClientTopUnit ClientTopUnit { get; set; }
        public CreditCardBehavior CreditCardBehavior { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientTopUnitCreditCards_v1Result> CreditCards { get; set; }

         public ClientTopUnitCreditCardsVM()
        {
        }
         public ClientTopUnitCreditCardsVM(
                                ClientTopUnit clientTopUnit,
                                CreditCardBehavior creditCardBehavior,
                                CWTPaginatedList<spDesktopDataAdmin_SelectClientTopUnitCreditCards_v1Result> creditCards
                                )
        {
            ClientTopUnit = clientTopUnit;
            CreditCardBehavior = creditCardBehavior;
            CreditCards = creditCards;
        }
       
    }
}