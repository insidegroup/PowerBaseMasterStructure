using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class PolicyCarVendorGroupItemValidation
    {
        [Required(ErrorMessage = "Policy Location Required")]
        public string PolicyLocationId { get; set; }

        [Required(ErrorMessage = "Supplier Required")]
        public string SupplierName { get; set; }

        [Required(ErrorMessage = "Product Required")]
        public string ProductId { get; set; }

		[Required(ErrorMessage = "Status Required")]
		public int PolicyCarStatusId { get; set; }
    }
}
