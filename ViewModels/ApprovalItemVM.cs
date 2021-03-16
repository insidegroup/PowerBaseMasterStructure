using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
	public class ApprovalItemVM : CWTBaseViewModel
	{
		public ApprovalItem ApprovalItem { get; set; }
		public ApprovalGroup ApprovalGroup { get; set; }

		public ApprovalItemVM()
		{
		}

		public ApprovalItemVM(ApprovalItem approvalItem, ApprovalGroup approvalGroup)
		{
			ApprovalItem = approvalItem;
			ApprovalGroup = approvalGroup;
		}
	}
}