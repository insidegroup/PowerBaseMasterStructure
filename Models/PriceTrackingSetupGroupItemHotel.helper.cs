using CWTDesktopDatabase.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(PriceTrackingSetupGroupItemHotelValidation))]
    public partial class PriceTrackingSetupGroupItemHotel : CWTBaseModel
    {
        public decimal? CalculatedTotalThresholdAmount { get; set; }

        //XML
        public List<PriceTrackingSetupGroupItemHotelTrackingAlertEmailAddress> PriceTrackingSetupGroupItemHotelTrackingAlertEmailAddressesXML { get; set; }
        
    }
}
