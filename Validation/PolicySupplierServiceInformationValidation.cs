using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class PolicySupplierServiceInformationValidation
    {
        [Required(ErrorMessage = "Value Required")]
        public string PolicySupplierServiceInformationValue { get; set; }

        [Required(ErrorMessage = "Type Required")]
        public string PolicySupplierServiceInformationTypeId { get; set; }

        [Required(ErrorMessage = "Supplier Required")]
        public string SupplierName { get; set; }

        [Required(ErrorMessage = "Product Required")]
        public int ProductId { get; set; }
    }
}
