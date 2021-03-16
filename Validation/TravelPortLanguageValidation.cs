using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class TravelPortLanguageValidation
    {
        [Required(ErrorMessage = "Type Required")]
        public int TravelPortTypeId { get; set; }

        [Required(ErrorMessage = "Language Required")]
        public string LanguageCode { get; set; }

        [Required(ErrorMessage = "Name Required")]
        public string TravelPortName { get; set; }
    }
}
