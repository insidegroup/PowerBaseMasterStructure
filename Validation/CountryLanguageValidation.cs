using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class CountryLanguageValidation
    {
        [Required(ErrorMessage = "Country Name Required")]
        [StringLength(256, MinimumLength = 2, ErrorMessage = "Country Name must have a minimum of 2 characters")]
        public string CountryName { get; set; }

        [Required(ErrorMessage = "Language Required")]
        public string LanguageCode { get; set; }
    }
}