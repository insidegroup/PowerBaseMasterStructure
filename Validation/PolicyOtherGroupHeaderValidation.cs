using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
	public class PolicyOtherGroupHeaderValidation
	{
		[Required(ErrorMessage = "Service Type Required")]
		public string PolicyOtherGroupHeaderServiceTypeId { get; set; }

		[Required(ErrorMessage = "Product Required")]
		public string ProductId { get; set; }

		[RegularExpression(@"^([\w\s()*\-_\.äáâåé]+)$", ErrorMessage = "Special character entered is not allowed")]
		[Required(ErrorMessage = "Label Required")]
		public string Label { get; set; }

		[RegularExpression(@"^([\w\s()*\-_\.äáâåé]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string TableName { get; set; }

	}
}