using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class PolicyCarTypeGroupItemLanguageValidation
    {
        [Required(ErrorMessage = "Car Type Advice Required")]
        public string CarTypeAdvice { get; set; }

        [Required(ErrorMessage = "Language Required")]
        public string LanguageCode { get; set; }
    }
   
}
