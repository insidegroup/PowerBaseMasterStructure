using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class TripTypeItemValidation
    {
        [Required(ErrorMessage = "TripType Required")]
        public string TripTypeId { get; set; }

    }
}
