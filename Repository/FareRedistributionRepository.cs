using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Repository
{
	public class FareRedistributionRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//List FareRedistributions
		public CWTPaginatedList<spDesktopDataAdmin_SelectFareRedistribution_v1Result> GetFareRedistributions(string filter, string sortField, int sortOrder, int page)
		{
			var result = db.spDesktopDataAdmin_SelectFareRedistribution_v1(filter, sortField, sortOrder, page).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectFareRedistribution_v1Result>(result, page, Convert.ToInt32(totalRecords));
			return paginatedView;
		}

		//Get FareRedistributions
		public List<FareRedistribution> GetAllFareRedistributions()
		{
			return db.FareRedistributions.OrderBy(x => x.FareRedistributionName).ToList();
		}

		//Get one FareRedistribution
		public FareRedistribution GetFareRedistribution(int fareRedistributionId)
		{
			return db.FareRedistributions.SingleOrDefault(c => c.FareRedistributionId == fareRedistributionId);
		}

		//Add FareRedistribution
		public void Add(FareRedistributionVM fareRedistributionVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertFareRedistribution_v1(
				fareRedistributionVM.FareRedistribution.FareRedistributionName,
				fareRedistributionVM.FareRedistribution.GDSCode, 
				adminUserGuid
			);
		}

		//Edit FareRedistribution
		public void Update(FareRedistributionVM fareRedistributionVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateFareRedistribution_v1(
				fareRedistributionVM.FareRedistribution.FareRedistributionId,
				fareRedistributionVM.FareRedistribution.FareRedistributionName,
				fareRedistributionVM.FareRedistribution.GDSCode, 
				adminUserGuid
			);
		}

		//Delete FareRedistribution
		public void Delete(FareRedistributionVM fareRedistributionVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteFareRedistribution_v1(
				fareRedistributionVM.FareRedistribution.FareRedistributionId,
				adminUserGuid,
				fareRedistributionVM.FareRedistribution.VersionNumber
			);
		}

		//Get FareRedistributions by GDS
		public List<FareRedistributionJSON> GetFareRedistributionsByGDSCode(string gdsCode)
		{
			var result = from n in db.FareRedistributions.OrderBy(x => x.FareRedistributionName).Where(x => x.GDSCode == gdsCode)
						 select
							 new FareRedistributionJSON
							 {
								 FareRedistributionId = n.FareRedistributionId,
								 FareRedistributionName = n.FareRedistributionName.ToString()
							 };
			return result.ToList();
		}
		
		public List<FareRedistributionReference> GetFareRedistributionReferences(int fareRedistributionId)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = from n in db.spDesktopDataAdmin_SelectFareRedistributionReferences_v1(fareRedistributionId, adminUserGuid)
						 select
							 new FareRedistributionReference
							 {
								 TableName = n.TableName.ToString()
							 };

			return result.ToList();
		}
	}
}