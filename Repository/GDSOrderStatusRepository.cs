using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Repository
{
	public class GDSOrderStatusRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get GDSOrderStatuses
		public List<GDSOrderStatus> GetAllGDSOrderStatuses()
		{
			return db.GDSOrderStatus.ToList();
		}

		//Get One GDSOrderStatus
		public GDSOrderStatus GetGDSOrderStatus(int gdsOrderStatusId)
		{
			return db.GDSOrderStatus.SingleOrDefault(c => c.GDSOrderStatusId == gdsOrderStatusId);
		}
	}
}