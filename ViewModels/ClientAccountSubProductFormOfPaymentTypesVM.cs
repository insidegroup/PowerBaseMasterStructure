using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientAccountSubProductFormOfPaymentTypesVM : CWTBaseViewModel
    {
        public ClientAccount ClientAccount { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailSubProductFormOfPaymentTypes_v1Result> SubProductFormOfPaymentTypes { get; set; }
        
        public ClientAccountSubProductFormOfPaymentTypesVM()
        {
        }
        public ClientAccountSubProductFormOfPaymentTypesVM(ClientAccount clientAccount, ClientDetail clientDetail, CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailSubProductFormOfPaymentTypes_v1Result> subProductFormOfPaymentTypes)
        {
            ClientAccount = clientAccount;
            ClientDetail = clientDetail;
            SubProductFormOfPaymentTypes = subProductFormOfPaymentTypes;
        }
    }
}