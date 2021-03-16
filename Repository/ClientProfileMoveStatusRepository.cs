using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
	public class ClientProfileMoveStatusRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get One Item
		public ClientProfileMoveStatus GetMoveStatus(int? id)
		{
			return db.ClientProfileMoveStatus.SingleOrDefault(c => c.ClientProfileMoveStatusId == id);
		}
		
		//Get All Items
		public IQueryable<ClientProfileMoveStatus> GetAllClientProfileMoveStatuses()
		{
			return db.ClientProfileMoveStatus.OrderBy(c => c.ClientProfileMoveStatusCode);
		}
	}
}
