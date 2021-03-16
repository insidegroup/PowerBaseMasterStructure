using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class ClientDetailSupplierProductValidation
    {
        [Required(ErrorMessage = "Supplier Required")]
        public string SupplierName { get; set; }

        [Required(ErrorMessage = "Product Required")]
        public int ProductId { get; set; }
    }
}
