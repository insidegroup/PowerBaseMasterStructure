using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Repository
{
	public class IATARepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//List IATAs
		public CWTPaginatedList<spDesktopDataAdmin_SelectIATA_v1Result> GetIATAs(string filter, string sortField, int sortOrder, int page, bool deletedFlag)
		{
			var result = db.spDesktopDataAdmin_SelectIATA_v1(filter, sortField, sortOrder, page, deletedFlag).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectIATA_v1Result>(result, page, Convert.ToInt32(totalRecords));
			return paginatedView;
		}

		//Get IATAs
		public List<IATA> GetAllIATAs()
		{
			return db.IATAs.OrderBy(x => x.IATANumber).ToList();
		}

		//Get one IATA
		public IATA GetIATA(int iataId)
		{
			return db.IATAs.SingleOrDefault(c => c.IATAId == iataId);
		}

		//Add IATA
		public void Add(IATAVM iataVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertIATA_v1(
				iataVM.IATA.IATANumber,
				iataVM.IATA.IATABranchOrGLString, 
				adminUserGuid
			);
		}

		//Edit IATA
		public void Update(IATAVM iataVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateIATA_v1(
				iataVM.IATA.IATAId,
				iataVM.IATA.IATANumber,
				iataVM.IATA.IATABranchOrGLString, 
				adminUserGuid
			);
		}

		//Delete IATA
		public void UpdateDeletedStatus(IATAVM iataVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateIATADeletedStatus_v1(
				iataVM.IATA.IATAId,
				iataVM.IATA.DeletedFlag,
				adminUserGuid,
				iataVM.IATA.VersionNumber
			);
		}

		/// <summary>
		/// The Delete page will show the user any PCC/OIDs that are attached to that IATA number separated by a comma.  
		/// It will not prevent the user from deleting the IATA, but it will make them aware that there are items attached
		/// </summary>
		/// <param name="iataId"></param>
		/// <returns></returns>
		public List<IATAReference> GetIATAReferences(int iataId)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			var result = from n in db.PseudoCityOrOfficeMaintenances.OrderBy(x => x.PseudoCityOrOfficeId).Where(x => x.IATAId == iataId)
						 select
							 new IATAReference
							 {
								 ReferenceName = n.PseudoCityOrOfficeId.ToString()
							 };

			return result.ToList();
		}
	}
}