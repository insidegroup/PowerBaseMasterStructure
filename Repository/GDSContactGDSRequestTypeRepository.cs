using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using System.Web.Security;
using CWTDesktopDatabase.Helpers;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace CWTDesktopDatabase.Repository
{
	public class GDSContactGDSRequestTypeRepository
    {
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get GDSContactGDSRequestTypes
		public IQueryable<GDSContactGDSRequestType> GetGDSContactGDSRequestTypes(int gdsContactId)
        {
            return db.GDSContactGDSRequestTypes.Where(c => c.GDSContactId == gdsContactId);
        }
    }
}
