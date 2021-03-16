using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;


namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(MerchantFeeValidation))]
	public partial class MerchantFee : CWTBaseModel
    {
        public string ProductName { get; set; }
        public string SupplierName { get; set; }
        public string CountryName { get; set; }
        public string CreditCardVendorName { get; set; }
        public decimal MerchantFeePercentMaximum { get; set; }
        
    }
}