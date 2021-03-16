using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class CityLanguageValidation
    {
        [Required(ErrorMessage = "City Name Required")]
        [StringLength(256, MinimumLength = 2, ErrorMessage = "City Name must have a minimum of 2 characters")]
        public string CityName { get; set; }

        [Required(ErrorMessage = "Language Required")]
        public string LanguageCode { get; set; }
    }
}