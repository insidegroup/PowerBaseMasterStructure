using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Repository
{
    public class ExternalNameRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//List ExternalNames
        public CWTPaginatedList<spDesktopDataAdmin_SelectExternalName_v1Result> GetExternalNames(string sortField, int sortOrder, int page)
        {
			var result = db.spDesktopDataAdmin_SelectExternalName_v1(sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectExternalName_v1Result>(result, page, Convert.ToInt32(totalRecords));
            return paginatedView;
        }

		//Get ExternalNames
		public List<ExternalName> GetAllExternalNames()
		{
			return db.ExternalNames.OrderBy(x => x.ExternalName1).ToList();
		}
		
		//Get one ExternalName
		public ExternalName GetExternalName(int externalNameId)
		{
			return db.ExternalNames.SingleOrDefault(c => c.ExternalNameId == externalNameId); ;
		}

		//Add ExternalName
        public void Add(ExternalNameVM gdsExternalNameVM)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertExternalName_v1(
				gdsExternalNameVM.ExternalName.ExternalName1, 
				adminUserGuid
			);
        }

		//Edit ExternalName
		public void Update(ExternalNameVM externalNameVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateExternalName_v1(
				externalNameVM.ExternalName.ExternalNameId,
				externalNameVM.ExternalName.ExternalName1,
				adminUserGuid
			);
		}

		//Delete ExternalName
		public void Delete(ExternalNameVM externalNameVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteExternalName_v1(
				externalNameVM.ExternalName.ExternalNameId,
				adminUserGuid,
				externalNameVM.ExternalName.VersionNumber
			);
		}

		public List<ExternalNameReference> GetExternalNameReferences(int externalNameId)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = from n in db.spDesktopDataAdmin_SelectExternalNameReferences_v1(externalNameId, adminUserGuid)
						 select
							 new ExternalNameReference
							 {
								 TableName = n.TableName.ToString()
							 };

			return result.ToList();
		}
    }
}