using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class ClientSubUnitTelephonyValidation
    {
        [Required(ErrorMessage = "Dialed Number Required")]
		[RegularExpression(@"^[0-9]+$", ErrorMessage = "Special character entered is not allowed")]
        public string DialedNumber { get; set; }

        [Required(ErrorMessage = "Identifier Required")]
        public int CallerEnteredDigitDefinitionTypeId { get; set; }

    }
}