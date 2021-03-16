using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class PolicyAirCabinGroupItemLanguageValidation
    {
        [Required(ErrorMessage = "Air Cabin Advice Required")]
        public string AirCabinAdvice { get; set; }

        [Required(ErrorMessage = "Language Required")]
        public string LanguageCode { get; set; }
    }
}