using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Repository
{
	public class ClientDefinedReferenceItemValueRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//List of ClientDefinedReferenceItemValues for a ClientDefinedReferenceItem - Paged
		public CWTPaginatedList<spDesktopDataAdmin_SelectClientDefinedReferenceItemValues_v1Result> PageClientDefinedReferenceItemValues(string filter, string id, int page, string sortField, int sortOrder)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectClientDefinedReferenceItemValues_v1(filter, id, sortField, sortOrder, page).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientDefinedReferenceItemValues_v1Result>(result, page, Convert.ToInt32(totalRecords));
			return paginatedView;

		}

		//Get one Item
		public ClientDefinedReferenceItemValue GetClientDefinedReferenceItemValue(string id)
		{
			return db.ClientDefinedReferenceItemValues.SingleOrDefault(c => c.ClientDefinedReferenceItemValueId == id);
		}

		//Add Item
		public void Add(ClientDefinedReferenceItemValue clientDefinedReferenceItemValue)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertClientDefinedReferenceItemValue_v1(
				clientDefinedReferenceItemValue.ClientDefinedReferenceItemId,
				clientDefinedReferenceItemValue.Value,
				clientDefinedReferenceItemValue.ValueDescription,
				adminUserGuid
			);
		}

		//Update Item
		public void Update(ClientDefinedReferenceItemValue clientDefinedReferenceItemValue)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateClientDefinedReferenceItemValue_v1(
				clientDefinedReferenceItemValue.ClientDefinedReferenceItemValueId,
				clientDefinedReferenceItemValue.Value,
				clientDefinedReferenceItemValue.ValueDescription,
				clientDefinedReferenceItemValue.VersionNumber,
				adminUserGuid
			);
		}

		//Delete Item
		public void Delete(ClientDefinedReferenceItemValue clientDefinedReferenceItemValue)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteClientDefinedReferenceItemValue_v1(
				clientDefinedReferenceItemValue.ClientDefinedReferenceItemValueId,
				clientDefinedReferenceItemValue.VersionNumber,
				adminUserGuid
			);
		}

	}
}