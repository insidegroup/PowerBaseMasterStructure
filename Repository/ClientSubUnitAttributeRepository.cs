using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using System.Web.Security;
namespace CWTDesktopDatabase.Repository
{
    public class ClientSubUnitAttributeRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
		private ClientSubUnitAttributeTypeRepository clientSubUnitAttributeTypeRepository = new ClientSubUnitAttributeTypeRepository();

		//Get one Item
		public ClientSubUnitAttribute GetClientSubUnitAttribute(string id, int attributeTypeId)
		{
			return db.ClientSubUnitAttributes.SingleOrDefault(c => c.ClientSubUnitGuid == id && c.AttributeTypeId == attributeTypeId);
		}

		//Get one Item
		public ClientSubUnitAttribute GetClientSubUnitAttributeByType(string id, string attributeTypeValue)
		{
			
			ClientSubUnitAttribute clientSubUnitAttribute = new ClientSubUnitAttribute();
			ClientSubUnitAttributeType clientSubUnitAttributeType = clientSubUnitAttributeTypeRepository.GetClientSubUnitAttributeType(attributeTypeValue);
			if (clientSubUnitAttributeType != null)
			{
				int clientSubUnitAttributeTypeId = clientSubUnitAttributeType.AttributeTypeId;
				if (clientSubUnitAttributeTypeId > 0)
				{
					clientSubUnitAttribute = GetClientSubUnitAttribute(id, clientSubUnitAttributeTypeId);

				}
			}
			return clientSubUnitAttribute;
		}

    }
}
