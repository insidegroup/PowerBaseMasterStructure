using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace CWTDesktopDatabase.Repository
{
	public class PolicyOtherGroupHeaderServiceTypeRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get ServiceTypes
		public IQueryable<PolicyOtherGroupHeaderServiceType> GetAllPolicyOtherGroupHeaderServiceTypes()
		{
			return db.PolicyOtherGroupHeaderServiceTypes.OrderBy(c => c.PolicyOtherGroupHeaderServiceTypeDescription);
		}

		//Get one ServiceType
		public PolicyOtherGroupHeaderServiceType GetPolicyOtherGroupHeaderServiceType(int policyOtherGroupHeaderServiceTypeId)
		{
			return db.PolicyOtherGroupHeaderServiceTypes.SingleOrDefault(c => c.PolicyOtherGroupHeaderServiceTypeId == policyOtherGroupHeaderServiceTypeId);
		}
	}
}