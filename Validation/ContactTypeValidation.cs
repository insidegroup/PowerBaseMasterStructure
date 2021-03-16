using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class ContactTypeValidation
    {
        [Required(ErrorMessage = "Contact Type Required")]
		public string ContactTypeName { get; set; }
    }
}