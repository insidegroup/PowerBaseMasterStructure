using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Repository
{
    public class PseudoCityOrOfficeLocationTypeRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//List PseudoCityOrOfficeLocationTypes
        public CWTPaginatedList<spDesktopDataAdmin_SelectPseudoCityOrOfficeLocationType_v1Result> GetPseudoCityOrOfficeLocationTypes(string sortField, int sortOrder, int page)
        {
			var result = db.spDesktopDataAdmin_SelectPseudoCityOrOfficeLocationType_v1(sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPseudoCityOrOfficeLocationType_v1Result>(result, page, Convert.ToInt32(totalRecords));
            return paginatedView;
        }

		//Get PseudoCityOrOfficeLocationTypes
		public List<PseudoCityOrOfficeLocationType> GetAllPseudoCityOrOfficeLocationTypes()
		{
			return db.PseudoCityOrOfficeLocationTypes.OrderBy(x => x.PseudoCityOrOfficeLocationTypeName).ToList();
		}
		
		//Get one PseudoCityOrOfficeLocationType
		public PseudoCityOrOfficeLocationType GetPseudoCityOrOfficeLocationType(int pseudoCityOrOfficeLocationTypeId)
		{
			return db.PseudoCityOrOfficeLocationTypes.SingleOrDefault(c => c.PseudoCityOrOfficeLocationTypeId == pseudoCityOrOfficeLocationTypeId); ;
		}

		//Add PseudoCityOrOfficeLocationType
        public void Add(PseudoCityOrOfficeLocationTypeVM gdsPseudoCityOrOfficeLocationTypeVM)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPseudoCityOrOfficeLocationType_v1(
				gdsPseudoCityOrOfficeLocationTypeVM.PseudoCityOrOfficeLocationType.PseudoCityOrOfficeLocationTypeName, 
				adminUserGuid
			);
        }

		//Edit PseudoCityOrOfficeLocationType
		public void Update(PseudoCityOrOfficeLocationTypeVM pseudoCityOrOfficeLocationTypeVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdatePseudoCityOrOfficeLocationType_v1(
				pseudoCityOrOfficeLocationTypeVM.PseudoCityOrOfficeLocationType.PseudoCityOrOfficeLocationTypeId,
				pseudoCityOrOfficeLocationTypeVM.PseudoCityOrOfficeLocationType.PseudoCityOrOfficeLocationTypeName,
				adminUserGuid
			);
		}

		//Delete PseudoCityOrOfficeLocationType
		public void Delete(PseudoCityOrOfficeLocationTypeVM pseudoCityOrOfficeLocationTypeVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeletePseudoCityOrOfficeLocationType_v1(
				pseudoCityOrOfficeLocationTypeVM.PseudoCityOrOfficeLocationType.PseudoCityOrOfficeLocationTypeId,
				adminUserGuid,
				pseudoCityOrOfficeLocationTypeVM.PseudoCityOrOfficeLocationType.VersionNumber
			);
		}

		public List<PseudoCityOrOfficeLocationTypeReference> GetPseudoCityOrOfficeLocationTypeReferences(int pseudoCityOrOfficeLocationTypeId)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = from n in db.spDesktopDataAdmin_SelectPseudoCityOrOfficeLocationTypeReferences_v1(pseudoCityOrOfficeLocationTypeId, adminUserGuid)
						 select
							 new PseudoCityOrOfficeLocationTypeReference
							 {
								 TableName = n.TableName.ToString()
							 };

			return result.ToList();
		}
    }
}