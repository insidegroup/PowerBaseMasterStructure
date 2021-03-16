using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace CWTDesktopDatabase.Validation
{
    public class ClientDetailSubProductFormOfPaymentTypeValidation
    {
        [Required(ErrorMessage = "SubProduct Required")]
        public string SubProductId { get; set; }

        [Required(ErrorMessage = "FormOfPaymentType Required")]
        public string FormOfPaymentTypeId { get; set; }
    }
}
