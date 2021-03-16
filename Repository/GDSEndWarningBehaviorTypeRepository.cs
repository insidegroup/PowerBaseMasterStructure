using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Repository
{
	public class GDSEndWarningBehaviorTypeRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		public List<GDSEndWarningBehaviorType> GetAllGDSEndWarningBehaviorTypes()
		{
			var result = from n in db.GDSEndWarningBehaviorTypes select n;
			return result.ToList();
		}
	}
}