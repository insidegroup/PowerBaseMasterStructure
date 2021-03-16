using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class ReasonCodeProductTypeDescriptionValidation
    {        
        [Required(ErrorMessage = "Description Required")]
		[RegularExpression(@"^([\w\s-()*À-ÿ']+)$", ErrorMessage = "Special character entered is not allowed")]
        public string ReasonCodeProductTypeDescription1 { get; set; }

        [Required(ErrorMessage = "Language Required")]
        public string LanguageCode { get; set; }
    }
}
