using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Repository
{
	public class ClientDefinedReferenceItemPNROutputRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//List of ClientDefinedReferenceItems for a ClientSubUnit - Paged
		public CWTPaginatedList<spDesktopDataAdmin_SelectClientDefinedReferenceItemPNROutputItems_v1Result> PageClientDefinedReferenceItemPNROutputItems(string filter, string id, int page, string sortField, int sortOrder)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectClientDefinedReferenceItemPNROutputItems_v1(id, filter, sortField, sortOrder, page).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientDefinedReferenceItemPNROutputItems_v1Result>(result, page, Convert.ToInt32(totalRecords));
			return paginatedView;

		}

		//Get one Item from ClientDefinedReferenceItemPNROutput
		public ClientDefinedReferenceItemPNROutput GetClientDefinedReferenceItemPNROutput(int clientDefinedReferenceItemPNROutputId)
		{
			return db.ClientDefinedReferenceItemPNROutputs.SingleOrDefault(c => c.ClientDefinedReferenceItemPNROutputId == clientDefinedReferenceItemPNROutputId);
		}

		//Add Item
		public void Add(ClientDefinedReferenceItemPNROutput clientDefinedReferenceItemPNROutput)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertClientDefinedReferenceItemPNROutput_v1(
				clientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemId,
				clientDefinedReferenceItemPNROutput.PNROutputRemarkTypeCode,
				clientDefinedReferenceItemPNROutput.DefaultLanguageCode,
				clientDefinedReferenceItemPNROutput.GDSCode,
				clientDefinedReferenceItemPNROutput.GDSRemarkQualifier,
				clientDefinedReferenceItemPNROutput.DefaultRemark,
				adminUserGuid
			);
		}

		//Update Item
		public void Update(ClientDefinedReferenceItemPNROutput clientDefinedReferenceItemPNROutput)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateClientDefinedReferenceItemPNROutput_v1(
				clientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemPNROutputId,
				clientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemId,
				clientDefinedReferenceItemPNROutput.PNROutputRemarkTypeCode,
				clientDefinedReferenceItemPNROutput.DefaultLanguageCode,
				clientDefinedReferenceItemPNROutput.GDSCode,
				clientDefinedReferenceItemPNROutput.GDSRemarkQualifier,
				clientDefinedReferenceItemPNROutput.DefaultRemark,
				clientDefinedReferenceItemPNROutput.VersionNumber,
				adminUserGuid
			);
		}

		//Delete Item
		public void Delete(ClientDefinedReferenceItemPNROutput clientDefinedReferenceItemPNROutput)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteClientDefinedReferenceItemPNROutput_v1(
				clientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemPNROutputId,
				clientDefinedReferenceItemPNROutput.VersionNumber,
				adminUserGuid
			);
		}

    }
}