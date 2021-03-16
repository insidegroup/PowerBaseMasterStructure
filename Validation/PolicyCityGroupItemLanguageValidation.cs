using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class PolicyCityGroupItemLanguageValidation
    {
        [Required(ErrorMessage = "City Advice Required")]
        public string CityAdvice { get; set; }

        [Required(ErrorMessage = "Language Required")]
        public string LanguageCode { get; set; }
    }
}
