using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
	public class PNRNameStatementInformationValidation
	{
		[Required(ErrorMessage = "Field Required")]
		public string GDSCode { get; set; }
	}
}