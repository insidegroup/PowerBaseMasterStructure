using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
	public class PolicyOtherGroupHeaderColumnNameValidation
	{
		[RegularExpression(@"^([\w\s()*\-_\.äáâåé]+)$", ErrorMessage = "Special character entered is not allowed")]
		[Required(ErrorMessage = "Column Name Required")]
		public string ColumnName { get; set; }
	}
}