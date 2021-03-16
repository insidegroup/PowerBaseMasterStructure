using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class OptionalFieldValidation
    {
        [Required(ErrorMessage = "Name Required")]
        [RegularExpression(@"^([À-ÿ\w\s-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string OptionalFieldName { get; set; }

        [Required(ErrorMessage = "Style Required")]
        public string OptionalFieldStyleId { get; set; }
    }
}