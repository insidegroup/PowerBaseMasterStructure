using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientTopUnitSubProductFormOfPaymentTypesVM : CWTBaseViewModel
    {
        public ClientTopUnit ClientTopUnit { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailSubProductFormOfPaymentTypes_v1Result> SubProductFormOfPaymentTypes { get; set; }
        
        public ClientTopUnitSubProductFormOfPaymentTypesVM()
        {
        }
        public ClientTopUnitSubProductFormOfPaymentTypesVM(ClientTopUnit clientTopUnit, ClientDetail clientDetail, CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailSubProductFormOfPaymentTypes_v1Result> subProductFormOfPaymentTypes)
        {
            ClientTopUnit = clientTopUnit;
            ClientDetail = clientDetail;
            SubProductFormOfPaymentTypes = subProductFormOfPaymentTypes;
        }
    }
}