using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
	public class PolicyAirParameterGroupItemValidation
    {
		[Required(ErrorMessage = "This field is required")]
		public string PolicyAirParameterValue { get; set; }
    }
}
