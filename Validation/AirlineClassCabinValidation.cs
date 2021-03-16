using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class AirlineClassCabinValidation
    {
        [Required(ErrorMessage = "Airline Class Code Required")]
        public string AirlineClassCode { get; set; }

        [Required(ErrorMessage = "Airline Cabin Code Required")]
        public string AirlineCabinCode { get; set; }

        [Required(ErrorMessage = "Supplier Name Required")]
        public string SupplierName { get; set; }

        [Required(ErrorMessage = "Product Required")]
        public int ProductId { get; set; }
    }
}
