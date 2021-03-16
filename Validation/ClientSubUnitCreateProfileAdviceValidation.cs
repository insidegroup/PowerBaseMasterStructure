using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class ClientSubUnitCreateProfileAdviceValidation
    {
        [Required(ErrorMessage = "CreateProfile Advice Required")]
        public string CreateProfileAdvice { get; set; }

        [Required(ErrorMessage = "Language Required")]
        public string LanguageCode { get; set; }
    }
}
