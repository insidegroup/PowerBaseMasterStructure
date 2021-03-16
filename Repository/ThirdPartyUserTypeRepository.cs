using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Repository
{
    public class ThirdPartyUserTypeRepository
    {
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		public IQueryable<ThirdPartyUserType> GetAllThirdPartyUserTypes()
		{
			return db.ThirdPartyUserTypes.OrderBy(t => t.ThirdPartyUserTypeName);
		}

		public ThirdPartyUserType GetThirdPartyUserType(int thirdPartyUserTypeId)
		{
			return db.ThirdPartyUserTypes.SingleOrDefault(c => c.ThirdPartyUserTypeId == thirdPartyUserTypeId);
		}
	
    }
}
