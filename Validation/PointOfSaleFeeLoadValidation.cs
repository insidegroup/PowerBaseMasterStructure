using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace CWTDesktopDatabase.Validation
{
	public class PointOfSaleFeeLoadValidation
    {
        [Required(ErrorMessage = "Client Top Unit Required")]
        public string ClientTopUnitGuid { get; set; }

        [Required(ErrorMessage = "POS Fee Description Required")]
        public string FeeLoadDescriptionTypeCode { get; set; }

        [Required(ErrorMessage = "Product Required")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Travel Indicator Required")]
        public string TravelIndicator { get; set; }

        [RegularExpression(@"^(\d{1,8}|\d{0,8}\.\d{1,4})$", ErrorMessage = "Fee Load Amount should be in format 00000000.0000 (maximum 4 decimal places)")]
        [Required(ErrorMessage = "Fee Amount Required")]
        public decimal FeeLoadAmount { get; set; }

        [Required(ErrorMessage = "Fee Currency Required")]
        public string FeeLoadCurrencyCode { get; set; }
    }
}
