using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(ApprovalGroupApprovalTypeValidation))]
	public partial class ApprovalGroupApprovalType : CWTBaseModel
    {
        public int NewApprovalGroupApprovalTypeId { get; set; }
    }

    public partial class ApprovalGroupApprovalTypeReference
	{
		public string TableName { get; set; }
	}
}