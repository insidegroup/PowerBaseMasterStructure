using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class ProductReasonItemOrderTypeSelectionVMValidation
    {
         [Required(ErrorMessage = "Reason Code Type Required")]
         public int ReasonCodeTypeId { get; set; }
    }
}