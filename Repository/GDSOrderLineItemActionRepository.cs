using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Repository
{
	public class GDSOrderLineItemActionRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get GDSOrderLineItemActions
		public List<GDSOrderLineItemAction> GetAllGDSOrderLineItemActions()
		{
			return db.GDSOrderLineItemActions.OrderBy(x => x.GDSOrderLineItemActionName).ToList();
		}

		//Get one GDSOrderLineItemAction
		public GDSOrderLineItemAction GetGDSOrderLineItemAction(int gdsOrderLineItemActionId)
		{
			return db.GDSOrderLineItemActions.SingleOrDefault(c => c.GDSOrderLineItemActionId == gdsOrderLineItemActionId);
		}

	}
}