using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class PolicyHotelPropertyGroupItemValidation
    {

        [Required(ErrorMessage = "Policy Hotel Status Required")]
        public int PolicyHotelStatusId { get; set; }

        [Required(ErrorMessage = "Harp Hotel Id Required")]
        public int HarpHotelId { get; set; }
    }
}
