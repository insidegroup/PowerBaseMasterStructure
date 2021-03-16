using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class ServiceFundChannelTypeRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get one ServiceFundChannelType
		public ServiceFundChannelType GetItem(int serviceFundChannelTypeId)
        {
			return db.ServiceFundChannelTypes.SingleOrDefault(c => c.ServiceFundChannelTypeId == serviceFundChannelTypeId);
        }

		//Get ServiceFundChannelTypes
		public IQueryable<ServiceFundChannelType> GetAllServiceFundChannelTypes()
        {
			return db.ServiceFundChannelTypes.OrderBy(c => c.ServiceFundChannelTypeName);
        }
    }
}
