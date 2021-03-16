using System.Collections.Generic;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
	public class ApprovalGroupApprovalTypeVM : CWTBaseViewModel
	{
		public ApprovalGroupApprovalType ApprovalGroupApprovalType { get; set; }

		public bool AllowDelete { get; set; }
		public List<ApprovalGroupApprovalTypeReference> ApprovalGroupApprovalTypeReferences { get; set; }

		public ApprovalGroupApprovalTypeVM()
		{
		}

		public ApprovalGroupApprovalTypeVM(ApprovalGroupApprovalType approvalGroupApprovalType)
		{
			ApprovalGroupApprovalType = approvalGroupApprovalType;
		}
	}
}