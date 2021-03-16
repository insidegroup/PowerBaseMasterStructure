using CWTDesktopDatabase.Helpers;
using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class PolicySupplierDealCodeValidation
    {
       
        [Required(ErrorMessage = "Deal Code Value Required")]
        public string PolicySupplierDealCodeValue { get; set; }

        [Required(ErrorMessage = "GDS Required")]
        public string GDSCode { get; set; }

        [Required(ErrorMessage = "Deal Code Type Required")]
        public int PolicySupplierDealCodeTypeId { get; set; }

        [Required(ErrorMessage = "Product Required")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Policy Location Required")]
        public int PolicyLocationId { get; set; }

        [Required(ErrorMessage = "Supplier Required")]
        public string SupplierName { get; set; }

		[CWTRequiredIf("PolicySupplierDealCodeTypeId", "9", ErrorMessage = "Travel Indicator Required")]
		public string TravelIndicator { get; set; }

    }
}
