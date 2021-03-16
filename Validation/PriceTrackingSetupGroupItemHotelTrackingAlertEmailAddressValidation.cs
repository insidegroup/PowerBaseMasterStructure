using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class PriceTrackingSetupGroupItemHotelTrackingAlertEmailAddressValidation
    {
        [RegularExpression(@"^([À-ÿ\w\s-()._@\\\/]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string EmailAddress { get; set; }
    }
}