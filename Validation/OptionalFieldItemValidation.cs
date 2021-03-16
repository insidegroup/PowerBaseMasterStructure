using System.ComponentModel.DataAnnotations;
namespace CWTDesktopDatabase.Validation
{
    public class OptionalFieldItemValidation
    {
		[Required(ErrorMessage = "The Product field is required.")]
		public int ProductId { get; set; }

    }
}