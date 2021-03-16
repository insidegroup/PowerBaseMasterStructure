using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Repository
{
	public class PseudoCityOrOfficeDefinedRegionRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//List PseudoCityOrOfficeDefinedRegions
        public CWTPaginatedList<spDesktopDataAdmin_SelectPseudoCityOrOfficeDefinedRegion_v1Result> GetPseudoCityOrOfficeDefinedRegions(string sortField, int sortOrder, int page)
        {
			var result = db.spDesktopDataAdmin_SelectPseudoCityOrOfficeDefinedRegion_v1(sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPseudoCityOrOfficeDefinedRegion_v1Result>(result, page, Convert.ToInt32(totalRecords));
            return paginatedView;
        }

		//Get PseudoCityOrOfficeDefinedRegions
		public List<PseudoCityOrOfficeDefinedRegion> GetPseudoCityOrOfficeDefinedRegions(int pseudoCityOrOfficeDefinedRegionId)
		{
			return db.PseudoCityOrOfficeDefinedRegions.ToList();
		}
		
		//Get one PseudoCityOrOfficeDefinedRegion
		public PseudoCityOrOfficeDefinedRegion GetPseudoCityOrOfficeDefinedRegion(int pseudoCityOrOfficeDefinedRegionId)
		{
			return db.PseudoCityOrOfficeDefinedRegions.SingleOrDefault(c => c.PseudoCityOrOfficeDefinedRegionId == pseudoCityOrOfficeDefinedRegionId);
		}

		//Get All PseudoCityOrOfficeDefinedRegion
		public IQueryable<PseudoCityOrOfficeDefinedRegion> GetAllPseudoCityOrOfficeDefinedRegions()
		{
			return db.PseudoCityOrOfficeDefinedRegions.OrderBy(c => c.PseudoCityOrOfficeDefinedRegionName);
		}


		//Get one PseudoCityOrOfficeDefinedRegion
		public IQueryable<PseudoCityOrOfficeDefinedRegion> GetPseudoCityOrOfficeDefinedRegionsForGlobalRegionCode(string globalRegionCode)
		{
			return db.PseudoCityOrOfficeDefinedRegions.OrderBy(x => x.GlobalRegion.GlobalRegionName).Where(c => c.GlobalRegionCode == globalRegionCode);
		}

		//Add PseudoCityOrOfficeDefinedRegion
		public void Add(PseudoCityOrOfficeDefinedRegionVM pseudoCityOrOfficeDefinedRegionVM)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPseudoCityOrOfficeDefinedRegion_v1(
				pseudoCityOrOfficeDefinedRegionVM.PseudoCityOrOfficeDefinedRegion.PseudoCityOrOfficeDefinedRegionName,
				pseudoCityOrOfficeDefinedRegionVM.PseudoCityOrOfficeDefinedRegion.GlobalRegionCode,
				adminUserGuid
			);
        }

		//Edit PseudoCityOrOfficeDefinedRegion
		public void Update(PseudoCityOrOfficeDefinedRegionVM pseudoCityOrOfficeDefinedRegionVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdatePseudoCityOrOfficeDefinedRegion_v1(
				pseudoCityOrOfficeDefinedRegionVM.PseudoCityOrOfficeDefinedRegion.PseudoCityOrOfficeDefinedRegionId,
				pseudoCityOrOfficeDefinedRegionVM.PseudoCityOrOfficeDefinedRegion.PseudoCityOrOfficeDefinedRegionName,
				pseudoCityOrOfficeDefinedRegionVM.PseudoCityOrOfficeDefinedRegion.GlobalRegionCode,
				adminUserGuid
			);
		}

		//Delete PseudoCityOrOfficeDefinedRegion
		public void Delete(PseudoCityOrOfficeDefinedRegionVM pseudoCityOrOfficeDefinedRegionVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeletePseudoCityOrOfficeDefinedRegion_v1(
				pseudoCityOrOfficeDefinedRegionVM.PseudoCityOrOfficeDefinedRegion.PseudoCityOrOfficeDefinedRegionId,
				adminUserGuid,
				pseudoCityOrOfficeDefinedRegionVM.PseudoCityOrOfficeDefinedRegion.VersionNumber
			);
		}

		public List<PseudoCityOrOfficeDefinedRegionReference> GetPseudoCityOrOfficeDefinedRegionReferences(int pseudoCityOrOfficeDefinedRegionId)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = from n in db.spDesktopDataAdmin_SelectPseudoCityOrOfficeDefinedRegionReferences_v1(pseudoCityOrOfficeDefinedRegionId, adminUserGuid)
						 select
							 new PseudoCityOrOfficeDefinedRegionReference
							 {
								 TableName = n.TableName.ToString()
							 };

			return result.ToList();
		}

		public List<PseudoCityOrOfficeDefinedRegionJSON> GetPseudoCityOrOfficeDefinedRegions(string globalRegionCode)
		{
			var result = from PseudoCityOrOfficeDefinedRegions in db.PseudoCityOrOfficeDefinedRegions
						 where PseudoCityOrOfficeDefinedRegions.GlobalRegionCode.Equals(globalRegionCode)
						 orderby PseudoCityOrOfficeDefinedRegions.PseudoCityOrOfficeDefinedRegionName
						 select
							 new PseudoCityOrOfficeDefinedRegionJSON
							 {
								 PseudoCityOrOfficeDefinedRegionName = PseudoCityOrOfficeDefinedRegions.PseudoCityOrOfficeDefinedRegionName.Trim(),
								 PseudoCityOrOfficeDefinedRegionId = PseudoCityOrOfficeDefinedRegions.PseudoCityOrOfficeDefinedRegionId
							 };
			return result.ToList();
		}
	}
}