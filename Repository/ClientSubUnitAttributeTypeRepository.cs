using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using System.Web.Security;
namespace CWTDesktopDatabase.Repository
{
    public class ClientSubUnitAttributeTypeRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get one Item
		public ClientSubUnitAttributeType GetClientSubUnitAttributeType(string attributeTypeName)
        {
			return db.ClientSubUnitAttributeTypes.SingleOrDefault(c => c.AttributeTypeName == attributeTypeName);
        }

    }
}
