using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class ControlValueValidation
    {
        [Required(ErrorMessage = "Control Value Required")]
        [RegularExpression(@"^([\w\s-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string ControlValue1 { get; set; }

        [Required(ErrorMessage = "Control Value Required")]
        public string ControlPropertyId { get; set; }

        [Required(ErrorMessage = "Name Required")]
        public string ControlNameId { get; set; }
    }
}
