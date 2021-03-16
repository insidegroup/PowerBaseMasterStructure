using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class SystemUserGDSValidation
    {
        [Required(ErrorMessage = "GDS Required")]
        public string GDSCode { get; set; }

        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Home Pseudo City or Office ID must be alphanumeric")]
        public string PseudoCityOrOfficeId { get; set; }

        [RegularExpression("^([a-zA-Z0-9]{2,10})*$", ErrorMessage = "GDS Sign On ID must be alphanumeric and 2-10 characters in length")]
        public string GDSSignOn { get; set; }

    }
}
