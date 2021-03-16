using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
	public class DesktopUsedTypeRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public DesktopUsedType GetDesktopUsedType(int id)
        {
            return db.DesktopUsedTypes.Where(c => c.DesktopUsedTypeId == id).SingleOrDefault();
        }

        public DesktopUsedType GetDesktopUsedTypeByDescription(string desktopUsedTypeDescription)
        {
            return db.DesktopUsedTypes.Where(c => c.DesktopUsedTypeDescription == desktopUsedTypeDescription).SingleOrDefault();
        }

        public IQueryable<DesktopUsedType> GetAllDesktopUsedTypes()
		{
			return db.DesktopUsedTypes.OrderBy(c => c.DesktopUsedTypeDescription);
		}
	}
}