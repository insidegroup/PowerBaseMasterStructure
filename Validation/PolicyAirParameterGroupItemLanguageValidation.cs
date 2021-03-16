using System.ComponentModel.DataAnnotations;
namespace CWTDesktopDatabase.Validation
{
	public class PolicyAirParameterGroupItemLanguageValidation
    {
		[Required(ErrorMessage = "This field is required")]
		public string LanguageCode { get; set; }

		[Required(ErrorMessage = "This field is required")]
		public string Translation { get; set; }
	}
}
