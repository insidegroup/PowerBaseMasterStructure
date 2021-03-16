using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Repository
{
    public class PseudoCityOrOfficeTypeRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//List PseudoCityOrOfficeTypes
        public CWTPaginatedList<spDesktopDataAdmin_SelectPseudoCityOrOfficeType_v1Result> GetPseudoCityOrOfficeTypes(string sortField, int sortOrder, int page)
        {
			var result = db.spDesktopDataAdmin_SelectPseudoCityOrOfficeType_v1(sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPseudoCityOrOfficeType_v1Result>(result, page, Convert.ToInt32(totalRecords));
            return paginatedView;
        }

		//Get PseudoCityOrOfficeTypes
		public List<PseudoCityOrOfficeType> GetAllPseudoCityOrOfficeTypes()
		{
			return db.PseudoCityOrOfficeTypes.OrderBy(x => x.PseudoCityOrOfficeTypeName).ToList();
		}
		
		//Get one PseudoCityOrOfficeType
		public PseudoCityOrOfficeType GetPseudoCityOrOfficeType(int pseudoCityOrOfficeLocationTypeId)
		{
			return db.PseudoCityOrOfficeTypes.SingleOrDefault(c => c.PseudoCityOrOfficeTypeId == pseudoCityOrOfficeLocationTypeId); ;
		}

		//Add PseudoCityOrOfficeType
        public void Add(PseudoCityOrOfficeTypeVM gdsPseudoCityOrOfficeTypeVM)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPseudoCityOrOfficeType_v1(
				gdsPseudoCityOrOfficeTypeVM.PseudoCityOrOfficeType.PseudoCityOrOfficeTypeName, 
				adminUserGuid
			);
        }

		//Edit PseudoCityOrOfficeType
		public void Update(PseudoCityOrOfficeTypeVM pseudoCityOrOfficeLocationTypeVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdatePseudoCityOrOfficeType_v1(
				pseudoCityOrOfficeLocationTypeVM.PseudoCityOrOfficeType.PseudoCityOrOfficeTypeId,
				pseudoCityOrOfficeLocationTypeVM.PseudoCityOrOfficeType.PseudoCityOrOfficeTypeName,
				adminUserGuid
			);
		}

		//Delete PseudoCityOrOfficeType
		public void Delete(PseudoCityOrOfficeTypeVM pseudoCityOrOfficeLocationTypeVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeletePseudoCityOrOfficeType_v1(
				pseudoCityOrOfficeLocationTypeVM.PseudoCityOrOfficeType.PseudoCityOrOfficeTypeId,
				adminUserGuid,
				pseudoCityOrOfficeLocationTypeVM.PseudoCityOrOfficeType.VersionNumber
			);
		}

		public List<PseudoCityOrOfficeTypeReference> GetPseudoCityOrOfficeTypeReferences(int pseudoCityOrOfficeLocationTypeId)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = from n in db.spDesktopDataAdmin_SelectPseudoCityOrOfficeTypeReferences_v1(pseudoCityOrOfficeLocationTypeId, adminUserGuid)
						 select
							 new PseudoCityOrOfficeTypeReference
							 {
								 TableName = n.TableName.ToString()
							 };

			return result.ToList();
		}
    }
}