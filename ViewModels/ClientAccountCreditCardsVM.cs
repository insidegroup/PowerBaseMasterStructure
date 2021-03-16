using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientAccountCreditCardsVM : CWTBaseViewModel
    {
        public ClientAccount ClientAccount { get; set; }
		public ClientTopUnit ClientTopUnit { get; set; }
		public ClientSubUnit ClientSubUnit { get; set; }
        public CreditCardBehavior CreditCardBehavior { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientAccountCreditCards_v1Result> CreditCards { get; set; }

        public ClientAccountCreditCardsVM()
        {
        }
        public ClientAccountCreditCardsVM(
                                ClientAccount clientAccount,
								ClientTopUnit clientTopUnit,
                                ClientSubUnit clientSubUnit,
                                CreditCardBehavior creditCardBehavior,
                                CWTPaginatedList<spDesktopDataAdmin_SelectClientAccountCreditCards_v1Result> creditCards
                                )
        {
            ClientAccount = clientAccount;
			ClientTopUnit = clientTopUnit;
			ClientSubUnit = clientSubUnit;
            CreditCardBehavior = creditCardBehavior;
            CreditCards = creditCards;
        }
       
    }
}