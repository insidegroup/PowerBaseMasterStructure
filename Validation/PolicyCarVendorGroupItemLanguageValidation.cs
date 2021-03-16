using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class PolicyCarVendorGroupItemLanguageValidation
    {
        [Required(ErrorMessage = "Car Advice Required")]
        public string CarAdvice { get; set; }

        [Required(ErrorMessage = "Language Required")]
        public string LanguageCode { get; set; }
    }
}
