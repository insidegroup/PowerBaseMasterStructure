using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class ReasonCodeItemValidation
    {
        [Required(ErrorMessage = "ReasonCode Required")]
        public string ReasonCode { get; set; }

        [Required(ErrorMessage = "Product Required")]
        public string ProductId { get; set; }

        [Required(ErrorMessage = "ReasonCodeType Required")]
        public string ReasonCodeTypeId { get; set; }

    }
}
