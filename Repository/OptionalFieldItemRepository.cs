using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
	public class OptionalFieldItemRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get a Page of OptionalFields - for Page Listings
		public CWTPaginatedList<spDesktopDataAdmin_SelectOptionalFieldGroupOptionalFieldItems_v1Result> PageOptionalFieldItems(int optionalFieldGroupId, int page, string sortField, int sortOrder)
		{
			//get a page of records
			var result = db.spDesktopDataAdmin_SelectOptionalFieldGroupOptionalFieldItems_v1(optionalFieldGroupId, sortField, sortOrder, page).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectOptionalFieldGroupOptionalFieldItems_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

		//Get one Item
		public OptionalFieldItem GetItem(int optionalFieldItemId)
		{
			return db.OptionalFieldItems.SingleOrDefault(c => (c.OptionalFieldItemId == optionalFieldItemId));
		}

		
		//Add to DB
		public void Add(OptionalFieldItem optionalFieldItem)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertOptionalFieldItem_v1(
				optionalFieldItem.OptionalFieldGroupId,
				optionalFieldItem.OptionalFieldId,
				optionalFieldItem.ProductId,
				optionalFieldItem.SupplierCode,
				optionalFieldItem.Mandatory,
				adminUserGuid
			);
		}

		//Update in DB
		public void Update(OptionalFieldItem optionalFieldItem)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateOptionalFieldItem_v1(
				optionalFieldItem.OptionalFieldItemId,
				optionalFieldItem.OptionalFieldId,
				optionalFieldItem.ProductId,
				optionalFieldItem.SupplierCode,
				optionalFieldItem.Mandatory,
				adminUserGuid,
				optionalFieldItem.VersionNumber
			);
		}

		//Delete From DB
		public void Delete(OptionalFieldItem optionalFieldItem)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteOptionalFieldItem_v1(
				optionalFieldItem.OptionalFieldItemId,
				adminUserGuid,
				optionalFieldItem.VersionNumber
			);
		}

		//Sequencing
		public List<Product> GetOptionalFieldItemOptionalFieldTypes(int optionalFieldGroupId)
		{
			return (from n in db.spDesktopDataAdmin_SelectOptionalFieldGroupOptionalFields_v1(optionalFieldGroupId)
					select new
						Product
					{
						ProductId = n.ProductId,
						ProductName = n.ProductName
					}).ToList();
		}
	}
}
