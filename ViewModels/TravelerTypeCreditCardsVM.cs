using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class TravelerTypeCreditCardsVM : CWTBaseViewModel
     {
		public ClientTopUnit ClientTopUnit { get; set; }
		public ClientSubUnit ClientSubUnit { get; set; }
        public TravelerType TravelerType { get; set; }
        public CreditCardBehavior CreditCardBehavior { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectTravelerTypeCreditCards_v1Result> CreditCards { get; set; }

         public TravelerTypeCreditCardsVM()
        {
        }
         public TravelerTypeCreditCardsVM(
								ClientTopUnit clientTopUnit,
								ClientSubUnit clientSubUnit,
                                TravelerType travelerType,
                                CreditCardBehavior creditCardBehavior,
                                CWTPaginatedList<spDesktopDataAdmin_SelectTravelerTypeCreditCards_v1Result> creditCards
                                )
        {
			ClientTopUnit = clientTopUnit;
			ClientSubUnit = clientSubUnit;
            TravelerType = travelerType;
            CreditCardBehavior = creditCardBehavior;
            CreditCards = creditCards;
        }
       
    }
}