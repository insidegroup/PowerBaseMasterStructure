using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace CWTDesktopDatabase.Repository
{
	public class PolicyOtherGroupHeaderLabelRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get one PolicyOtherGroupHeaderLabel
		public PolicyOtherGroupHeaderLabel GetPolicyOtherGroupHeaderLabel(int policyOtherGroupHeaderId)
		{
			return db.PolicyOtherGroupHeaderLabels.SingleOrDefault(c => c.PolicyOtherGroupHeaderId == policyOtherGroupHeaderId);
		}

		//Get one PolicyOtherGroupHeaderLabel
		public PolicyOtherGroupHeaderLabel GetPolicyOtherGroupHeaderLabelByPolicyOtherGroupHeaderLabelId(int policyOtherGroupHeaderLabelId)
		{
			return db.PolicyOtherGroupHeaderLabels.SingleOrDefault(c => c.PolicyOtherGroupHeaderLabelId == policyOtherGroupHeaderLabelId);
		}
	}
}