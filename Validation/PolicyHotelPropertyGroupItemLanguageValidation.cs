using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class PolicyHotelPropertyGroupItemLanguageValidation
    {
        [Required(ErrorMessage = "Hotel Advice Required")]
        public string HotelAdvice { get; set; }

        [Required(ErrorMessage = "Language Required")]
        public string LanguageCode { get; set; }
    }
}
