using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.ViewModels
{
	public class PolicyOtherGroupHeadersVM : CWTBaseViewModel
	{
		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupHeaders_v1Result> PolicyOtherGroupHeaders { get; set; }
        public bool HasWriteAccess { get; set; }
		public bool CanEditOrder { get; set; }

        public PolicyOtherGroupHeadersVM()
        {
            HasWriteAccess = false;
			CanEditOrder = false;
        }

		public PolicyOtherGroupHeadersVM(
			GDSEndWarningConfiguration gdsEndWarningConfiguration,
			CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupHeaders_v1Result> policyOtherGroupHeaders,
			bool hasWriteAccess,
			bool canEditOrder)
        {
			PolicyOtherGroupHeaders = policyOtherGroupHeaders;
			HasWriteAccess = hasWriteAccess;
			CanEditOrder = canEditOrder;
        }
    }
}