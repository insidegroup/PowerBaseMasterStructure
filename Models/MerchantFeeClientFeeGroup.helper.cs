
namespace CWTDesktopDatabase.Models
{
	public partial class MerchantFeeClientFeeGroup : CWTBaseModel
    {
        public string MerchantFeeDescription { get; set; }
        public string CreditCardVendorName { get; set; }
        public string CountryName { get; set; }
        public string ProductName { get; set; }
        public string SupplierName { get; set; }
        public double MerchantFeePercent { get; set; }

    }
}