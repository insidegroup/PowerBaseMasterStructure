using System.ComponentModel.DataAnnotations;
namespace CWTDesktopDatabase.Validation
{
    public class PolicyAirVendorGroupItemLanguageValidation
    {
        [Required(ErrorMessage = "Airline Advice Required")]
        public string AirlineAdvice { get; set; }

        [Required(ErrorMessage = "Language Required")]
        public string LanguageCode { get; set; }
    }
}
