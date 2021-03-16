using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientSubUnitTravelerTypeCreditCardsVM : CWTBaseViewModel
    {
		public ClientTopUnit ClientTopUnit { get; set; }
		public ClientSubUnit ClientSubUnit { get; set; }
        public TravelerType TravelerType { get; set; }
        public CreditCardBehavior CreditCardBehavior { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitTravelerTypeCreditCards_v1Result> CreditCards { get; set; }

         public ClientSubUnitTravelerTypeCreditCardsVM()
        {
        }
         public ClientSubUnitTravelerTypeCreditCardsVM(
								ClientTopUnit clientTopUnit,
								ClientSubUnit clientSubUnit,
                                CreditCardBehavior creditCardBehavior,
                                TravelerType clientSubUnitTravelerType,
                                CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitTravelerTypeCreditCards_v1Result> creditCards
                                )
        {
			ClientTopUnit = clientTopUnit;
			ClientSubUnit = clientSubUnit;
            TravelerType = clientSubUnitTravelerType;
            CreditCardBehavior = creditCardBehavior;
            CreditCards = creditCards;
        }
       
    }
}