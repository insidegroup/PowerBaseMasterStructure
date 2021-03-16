using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class TripTypeValidation
    {
        [RegularExpression(@"^([\w\s-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
        [Required(ErrorMessage = "Description Required")]
        public string TripTypeDescription { get; set; }

    }
}
