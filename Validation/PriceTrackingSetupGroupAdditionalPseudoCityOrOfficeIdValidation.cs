using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
	public class PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeIdValidation
    {
        [Required(ErrorMessage = "GDS Required")]
        public string GDSCode { get; set; }

        [Required(ErrorMessage = "PCC/OID Required")]
        public string PseudoCityOrOfficeId { get; set; }
    }
}