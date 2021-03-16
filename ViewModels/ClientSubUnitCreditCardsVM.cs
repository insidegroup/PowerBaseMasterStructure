using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientSubUnitCreditCardsVM : CWTBaseViewModel
    {
        public ClientTopUnit ClientTopUnit { get; set; }
        public ClientSubUnit ClientSubUnit { get; set; }
        public CreditCardBehavior CreditCardBehavior { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectPageClientSubUnitCreditCards_v1Result> CreditCards { get; set; }

         public ClientSubUnitCreditCardsVM()
        {
        }
         public ClientSubUnitCreditCardsVM(
                                ClientTopUnit clientTopUnit,
                                ClientSubUnit clientSubUnit,
                                CreditCardBehavior creditCardBehavior,
                                CWTPaginatedList<spDesktopDataAdmin_SelectPageClientSubUnitCreditCards_v1Result> creditCards
                                )
        {
            ClientTopUnit = clientTopUnit;
            ClientSubUnit = clientSubUnit;
            CreditCardBehavior = creditCardBehavior;
            CreditCards = creditCards;
        }
       
    }
}