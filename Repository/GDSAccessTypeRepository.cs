using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Repository
{
	public class GDSAccessTypeRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//List GDSAccessTypes
		public CWTPaginatedList<spDesktopDataAdmin_SelectGDSAccessType_v1Result> PageGDSAccessTypes(string filter, string sortField, int sortOrder, int page)
		{
			var result = db.spDesktopDataAdmin_SelectGDSAccessType_v1(filter, sortField, sortOrder, page).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectGDSAccessType_v1Result>(result, page, Convert.ToInt32(totalRecords));
			return paginatedView;
		}

		//Get GDSAccessTypes
		public List<GDSAccessType> GetAllGDSAccessTypes()
		{
			return db.GDSAccessTypes.ToList();
		}

		//Get one GDSAccessType
		public GDSAccessType GetGDSAccessType(int gdsAccessTypeId)
		{
			return db.GDSAccessTypes.SingleOrDefault(c => c.GDSAccessTypeId == gdsAccessTypeId);
		}

		//Add GDSAccessType
		public void Add(GDSAccessTypeVM gdsAccessTypeVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertGDSAccessType_v1(
				gdsAccessTypeVM.GDSAccessType.GDSAccessTypeName,
				gdsAccessTypeVM.GDSAccessType.GDSCode, 
				adminUserGuid
			);
		}

		//Edit GDSAccessType
		public void Update(GDSAccessTypeVM gdsAccessTypeVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateGDSAccessType_v1(
				gdsAccessTypeVM.GDSAccessType.GDSAccessTypeId,
				gdsAccessTypeVM.GDSAccessType.GDSAccessTypeName,
				gdsAccessTypeVM.GDSAccessType.GDSCode, 
				adminUserGuid
			);
		}

		//Delete GDSAccessType
		public void Delete(GDSAccessTypeVM gdsAccessTypeVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteGDSAccessType_v1(
				gdsAccessTypeVM.GDSAccessType.GDSAccessTypeId,
				adminUserGuid,
				gdsAccessTypeVM.GDSAccessType.VersionNumber
			);
		}

		public List<GDSAccessTypeReference> GetGDSAccessTypeReferences(int gdsAccessTypeId)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = from n in db.spDesktopDataAdmin_SelectGDSAccessTypeReferences_v1(gdsAccessTypeId, adminUserGuid)
						 select
							 new GDSAccessTypeReference
							 {
								 TableName = n.TableName.ToString()
							 };

			return result.ToList();
		}

		//Get GDSAccessTypes by GDSCode
		public List<ValidGDSAccessTypeJSON> GetGDSAccessTypesByGDSCode(string gdsCode)
		{
			return (from n in db.GDSAccessTypes.Where(x => x.GDSCode == gdsCode).OrderBy(x => x.GDSAccessTypeName)
					select
						new ValidGDSAccessTypeJSON
						{
							GDSAccessTypeId = n.GDSAccessTypeId,
							GDSAccessTypeName = n.GDSAccessTypeName.Trim()
						}
			).ToList();
		}
	}
}