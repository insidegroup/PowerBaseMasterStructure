using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace CWTDesktopDatabase.Validation
{
    public class PolicyLocationValidation
    {
        [Required(ErrorMessage = "Name Required")]
        [RegularExpression(@"^([\w\s-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string PolicyLocationName { get; set; }

        [RequiredIfNotEmpty("TravelPortTypeId", ErrorMessage = "Travel Port Name Required")]
        public string TravelPortName { get; set; }

        [RequiredIfFalse("GlobalFlag", ErrorMessage = "Required, or choose Global")]
        public string LocationName { get; set; }
    }
}
