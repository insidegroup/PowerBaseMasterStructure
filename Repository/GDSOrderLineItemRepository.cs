using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Repository
{
	public class GDSOrderLineItemRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get GDSOrderLineItems
		public List<GDSOrderLineItem> GetAllGDSOrderLineItems(int gdsOrderId)
		{
			return db.GDSOrderLineItems.Where(x => x.GDSOrderId == gdsOrderId).OrderBy(x => x.GDSOrderDetail.GDSOrderDetailName).ToList();
		}

		//Get one GDSOrderLineItem
		public GDSOrderLineItem GetGDSOrderLineItem(int gdsOrderLineItemId)
		{
			return db.GDSOrderLineItems.SingleOrDefault(c => c.GDSOrderLineItemId == gdsOrderLineItemId);
		}

	}
}