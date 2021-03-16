using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
	public class CommissionableRouteItemVM : CWTBaseViewModel
	{
		public CommissionableRouteItem CommissionableRouteItem { get; set; }
		//public CommissionableRouteGroup CommissionableRouteGroup { get; set; }
		public PolicyRouting PolicyRouting { get; set; }
		public Supplier Supplier { get; set; }
		public Product Product { get; set; }

		public CommissionableRouteItemVM()
		{
		}

		public CommissionableRouteItemVM(
				CommissionableRouteItem commissionableRouteItem,
				//CommissionableRouteGroup commissionableRouteGroup,
				PolicyRouting policyRouting,
				Supplier supplier,
				Product product)
		{
			CommissionableRouteItem = commissionableRouteItem;
			//CommissionableRouteGroup = commissionableRouteGroup;
			PolicyRouting = policyRouting;
			Product = product;
			Supplier = supplier;
		}
	}
}