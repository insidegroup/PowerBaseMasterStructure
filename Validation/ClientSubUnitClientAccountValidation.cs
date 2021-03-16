using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class ClientSubUnitClientAccountValidation
    {
        [Required(ErrorMessage = "Client Account Required")]
        public string ClientAccountName { get; set; }

        [Required(ErrorMessage = "Account Line Of Business Required")]
        public int ClientAccountLineOfBusinessId { get; set; }
    }
}
