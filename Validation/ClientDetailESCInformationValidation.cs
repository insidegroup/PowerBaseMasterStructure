using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace CWTDesktopDatabase.Validation
{
    public class ClientDetailESCInformationValidation
    {
        [Required(ErrorMessage = "ESC Information Required")]
        public string ESCInformation { get; set; }
    }
}
