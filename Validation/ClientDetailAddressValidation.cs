using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace CWTDesktopDatabase.Validation
{
    public class ClientDetailAddressValidation
    {
        [Required(ErrorMessage = "Address Line1 Required")]
        public string AddressLine1 { get; set; }

        [Required(ErrorMessage = "Town or City Required")]
        public string TownCity { get; set; }

        [Required(ErrorMessage = "Country Required")]
        public string CountryCode { get; set; }

    }
}
