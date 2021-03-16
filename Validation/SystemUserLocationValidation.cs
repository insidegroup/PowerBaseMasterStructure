using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class SystemUserLocationValidation
    {

        [Required(ErrorMessage = "Location Required")]
        public string LocationName { get; set; }
    }
}