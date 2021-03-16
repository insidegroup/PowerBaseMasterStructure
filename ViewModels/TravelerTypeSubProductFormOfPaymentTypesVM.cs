using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class TravelerTypeSubProductFormOfPaymentTypesVM : CWTBaseViewModel
    {
        public ClientSubUnit ClientSubUnit { get; set; }
        public TravelerType TravelerType { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailSubProductFormOfPaymentTypes_v1Result> SubProductFormOfPaymentTypes { get; set; }
        
        public TravelerTypeSubProductFormOfPaymentTypesVM()
        {
        }
        public TravelerTypeSubProductFormOfPaymentTypesVM(ClientSubUnit clientSubUnit, TravelerType travelerType, ClientDetail clientDetail, CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailSubProductFormOfPaymentTypes_v1Result> subProductFormOfPaymentTypes)
        {
            ClientSubUnit = clientSubUnit;
            TravelerType = travelerType;
            ClientDetail = clientDetail;
            SubProductFormOfPaymentTypes = subProductFormOfPaymentTypes;
        }
    }
}