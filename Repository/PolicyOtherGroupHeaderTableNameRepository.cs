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
	public class PolicyOtherGroupHeaderTableNameRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get one PolicyOtherGroupHeaderTableName
		public PolicyOtherGroupHeaderTableName GetPolicyOtherGroupHeaderTableName(int policyOtherGroupHeaderId)
		{
			return db.PolicyOtherGroupHeaderTableNames.SingleOrDefault(c => c.PolicyOtherGroupHeaderId == policyOtherGroupHeaderId);
		}

		//Get one PolicyOtherGroupHeaderTableName
		public PolicyOtherGroupHeaderTableName GetPolicyOtherGroupHeaderTableNameByPolicyOtherGroupHeaderTableNameId(int policyOtherGroupHeaderTableNameId)
		{
			return db.PolicyOtherGroupHeaderTableNames.SingleOrDefault(c => c.PolicyOtherGroupHeaderTableNameId == policyOtherGroupHeaderTableNameId);
		}
	}
}