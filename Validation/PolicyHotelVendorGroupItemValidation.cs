using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class PolicyHotelVendorGroupItemValidation
    {
        [Required(ErrorMessage = "Policy Location Required")]
        public int PolicyLocationId { get; set; }

        [Required(ErrorMessage = "Supplier Required")]
        public string SupplierName { get; set; }

        [Required(ErrorMessage = "Product Required")]
        public int ProductId { get; set; }

		[Required(ErrorMessage = "Status Required")]
		public int PolicyHotelStatusId { get; set; }
    }
}
