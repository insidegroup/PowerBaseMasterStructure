using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientSubUnitSubProductFormOfPaymentTypesVM : CWTBaseViewModel
    {
        public ClientSubUnit ClientSubUnit { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailSubProductFormOfPaymentTypes_v1Result> SubProductFormOfPaymentTypes { get; set; }
        
        public ClientSubUnitSubProductFormOfPaymentTypesVM()
        {
        }
        public ClientSubUnitSubProductFormOfPaymentTypesVM(ClientSubUnit clientSubUnit, ClientDetail clientDetail, CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailSubProductFormOfPaymentTypes_v1Result> subProductFormOfPaymentTypes)
        {
            ClientSubUnit = clientSubUnit;
            ClientDetail = clientDetail;
            SubProductFormOfPaymentTypes = subProductFormOfPaymentTypes;
        }
    }
}