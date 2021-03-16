using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class ReasonCodeAlternativeDescriptionValidation
    {
        [Required(ErrorMessage = "Alternative Description Required")]
		[RegularExpression(@"^([\w\s-()*À-ÿ']+)$", ErrorMessage = "Special character entered is not allowed")]
        public string ReasonCodeAlternativeDescription1 { get; set; }

        [Required(ErrorMessage = "Language Required")]
        public string LanguageCode { get; set; }
    }
}
