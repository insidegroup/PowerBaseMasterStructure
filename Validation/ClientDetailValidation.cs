using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class ClientDetailValidation
    {
        [Required(ErrorMessage = "Client Detail Name Required")]
        [RegularExpression(@"^([\w\s-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string ClientDetailName { get; set; }

    }
}
