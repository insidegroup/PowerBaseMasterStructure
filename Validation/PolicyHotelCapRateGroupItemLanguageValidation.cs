using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class PolicyHotelCapRateGroupItemLanguageValidation
    {
        [Required(ErrorMessage = "Hotel Cap Rate Required")]
        public string HotelCapRateAdvice { get; set; }

        [Required(ErrorMessage = "Language Required")]
        public string LanguageCode { get; set; }
    }
}
