using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class ClientFeeItemValidation
    {
        [Required(ErrorMessage = "Fee Required")]
        public int ClientFeeId { get; set; }

    }
}