using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Repository
{
	public class GDSThirdPartyVendorRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//List GDSThirdPartyVendors
		public CWTPaginatedList<spDesktopDataAdmin_SelectGDSThirdPartyVendors_v1Result> GetGDSThirdPartyVendors(string sortField, int sortOrder, int page)
		{
			var result = db.spDesktopDataAdmin_SelectGDSThirdPartyVendors_v1(sortField, sortOrder, page).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectGDSThirdPartyVendors_v1Result>(result, page, Convert.ToInt32(totalRecords));
			return paginatedView;
		}
		
		//Get GDSThirdPartyVendors
		public List<GDSThirdPartyVendor> GetAllGDSThirdPartyVendors()
		{
			return db.GDSThirdPartyVendors.OrderBy(x => x.GDSThirdPartyVendorName).ToList();
		}

		//Get one GDSThirdPartyVendor
		public GDSThirdPartyVendor GetGDSThirdPartyVendor(int gdsThirdPartyVendorId)
		{
			return db.GDSThirdPartyVendors.SingleOrDefault(c => c.GDSThirdPartyVendorId == gdsThirdPartyVendorId);
		}

		//Get PseudoCityOrOfficeMaintenance GDSThirdPartyVendors
		public List<GDSThirdPartyVendor> GetGDSThirdPartyVendorsByPseudoCityOrOfficeMaintenanceId(int pseudoCityOrOfficeMaintenanceId)
		{
			List<GDSThirdPartyVendor> gdsThirdPartyVendors = new List<GDSThirdPartyVendor>();
				
			List<PseudoCityOrOfficeMaintenanceGDSThirdPartyVendor> pseudoCityOrOfficeMaintenanceGDSThirdPartyVendors = db.PseudoCityOrOfficeMaintenanceGDSThirdPartyVendors.Where(x => x.PseudoCityOrOfficeMaintenanceId == pseudoCityOrOfficeMaintenanceId).ToList();
			foreach (PseudoCityOrOfficeMaintenanceGDSThirdPartyVendor pseudoCityOrOfficeMaintenanceGDSThirdPartyVendor in pseudoCityOrOfficeMaintenanceGDSThirdPartyVendors)
			{
				GDSThirdPartyVendor gdsThirdPartyVendor = GetGDSThirdPartyVendor(pseudoCityOrOfficeMaintenanceGDSThirdPartyVendor.GDSThirdPartyVendorId);
				if(gdsThirdPartyVendor != null) {
					gdsThirdPartyVendors.Add(gdsThirdPartyVendor);
				}
			}

			return gdsThirdPartyVendors;
		}

		//Add GDSThirdPartyVendor
		public void Add(GDSThirdPartyVendorVM gdsGDSThirdPartyVendorVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertGDSThirdPartyVendor_v1(
				gdsGDSThirdPartyVendorVM.GDSThirdPartyVendor.GDSThirdPartyVendorName,
				adminUserGuid
			);
		}

		//Edit GDSThirdPartyVendor
		public void Update(GDSThirdPartyVendorVM externalNameVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateGDSThirdPartyVendor_v1(
				externalNameVM.GDSThirdPartyVendor.GDSThirdPartyVendorId,
				externalNameVM.GDSThirdPartyVendor.GDSThirdPartyVendorName,
				adminUserGuid
			);
		}

		//Delete GDSThirdPartyVendor
		public void Delete(GDSThirdPartyVendorVM externalNameVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteGDSThirdPartyVendor_v1(
				externalNameVM.GDSThirdPartyVendor.GDSThirdPartyVendorId,
				adminUserGuid,
				externalNameVM.GDSThirdPartyVendor.VersionNumber
			);
		}

		public List<GDSThirdPartyVendorReference> GetGDSThirdPartyVendorReferences(int externalNameId)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = from n in db.spDesktopDataAdmin_SelectGDSThirdPartyVendorReferences_v1(externalNameId, adminUserGuid)
						 select
							 new GDSThirdPartyVendorReference
							 {
								 TableName = n.TableName.ToString()
							 };

			return result.ToList();
		}
	}
}