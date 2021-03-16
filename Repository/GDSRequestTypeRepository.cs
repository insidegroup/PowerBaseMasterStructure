using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Repository
{
	public class GDSRequestTypeRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//List GDSRequestTypes
		public CWTPaginatedList<spDesktopDataAdmin_SelectGDSRequestType_v1Result> GetGDSRequestTypes(string sortField, int sortOrder, int page)
		{
			var result = db.spDesktopDataAdmin_SelectGDSRequestType_v1(sortField, sortOrder, page).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectGDSRequestType_v1Result>(result, page, Convert.ToInt32(totalRecords));
			return paginatedView;
		}

		//Get GDSRequestTypes for a GGDSContact
		public List<GDSRequestType> GetGDSContactGDSRequestTypes(int gdsContactId)
		{
			List<GDSRequestType> gdsRequestTypes = new List<GDSRequestType>();
			List<GDSContactGDSRequestType> gdsContactGDSRequestTypes = db.GDSContactGDSRequestTypes.Where(x => x.GDSContactId == gdsContactId).ToList();

			foreach (GDSContactGDSRequestType gdsContactGDSRequestType in gdsContactGDSRequestTypes)
			{
				GDSRequestType gdsRequestType = GetGDSRequestType(gdsContactGDSRequestType.GDSRequestTypeId);
				if (gdsRequestType != null)
				{
					gdsRequestTypes.Add(gdsRequestType);
				}
			}

			return gdsRequestTypes;
		}

		//Get All GDSRequestTypes
		public List<GDSRequestType> GetAllGDSRequestTypes()
		{
			return db.GDSRequestTypes.ToList();
		}

		//Get one GDSRequestType
		public GDSRequestType GetGDSRequestType(int gdsRequestTypeId)
		{
			return db.GDSRequestTypes.SingleOrDefault(c => c.GDSRequestTypeId == gdsRequestTypeId);
		}

		//Add GDSRequestType
		public void Add(GDSRequestTypeVM gdsGDSRequestTypeVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertGDSRequestType_v1(
				gdsGDSRequestTypeVM.GDSRequestType.GDSRequestTypeName,
				adminUserGuid
			);
		}

		//Edit GDSRequestType
		public void Update(GDSRequestTypeVM gdsRequestTypeVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateGDSRequestType_v1(
				gdsRequestTypeVM.GDSRequestType.GDSRequestTypeId,
				gdsRequestTypeVM.GDSRequestType.GDSRequestTypeName,
				adminUserGuid
			);
		}

		//Delete GDSRequestType
		public void Delete(GDSRequestTypeVM gdsRequestTypeVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteGDSRequestType_v1(
				gdsRequestTypeVM.GDSRequestType.GDSRequestTypeId,
				adminUserGuid,
				gdsRequestTypeVM.GDSRequestType.VersionNumber
			);
		}

		public List<GDSRequestTypeReference> GetGDSRequestTypeReferences(int gdsRequestTypeId)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = from n in db.spDesktopDataAdmin_SelectGDSRequestTypeReferences_v1(gdsRequestTypeId, adminUserGuid)
						 select
							 new GDSRequestTypeReference
							 {
								 TableName = n.TableName.ToString()
							 };

			return result.ToList();
		}
	}
}