using System.ComponentModel.DataAnnotations;
using Foolproof;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Validation
{
	public class ApprovalItemValidation
    {
		[Required(ErrorMessage = "Approver Name Required")]
		public string ApproverDescription { get; set; }

		[Required(ErrorMessage = "Remark Text Required")]
		public string RemarkText { get; set; }
    }
}
