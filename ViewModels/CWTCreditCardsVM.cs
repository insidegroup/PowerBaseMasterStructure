using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class CWTCreditCardsVM : CWTBaseViewModel
    {
        public CreditCardBehavior CreditCardBehavior { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectCWTCreditCards_v1Result> CreditCards { get; set; }
        
        public CWTCreditCardsVM()
        {
        }
        public CWTCreditCardsVM(CreditCardBehavior creditCardBehavior, CWTPaginatedList<spDesktopDataAdmin_SelectCWTCreditCards_v1Result> creditCards)
        {
            CreditCardBehavior = creditCardBehavior;
            CreditCards = creditCards;
        }
    }
}