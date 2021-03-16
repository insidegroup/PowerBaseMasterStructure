using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace CWTDesktopDatabase.Validation
{
    public class ClientDetailContactValidation
    {
        [Required(ErrorMessage = "FirstName Required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName Required")]
        public string LastName { get; set; }
    }
}
