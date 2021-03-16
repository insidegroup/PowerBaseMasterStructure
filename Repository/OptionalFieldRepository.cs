using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Repository
{
    public class OptionalFieldRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of OptionalFields
        public CWTPaginatedList<spDesktopDataAdmin_SelectOptionalFields_v1Result> PageOptionalFields(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectOptionalFields_v1(filter, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectOptionalFields_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one Item
        public OptionalField GetItem(int? optionalFieldId)
        {
            return db.OptionalFields.SingleOrDefault(c => (c.OptionalFieldId == optionalFieldId));
        }

        //Get All Items
        public IQueryable<OptionalField> GetAllOptionalFields()
        {
            return db.OptionalFields.OrderBy(c => c.OptionalFieldName);
        }

        //Add to DB
        public void Add(OptionalFieldVM optionalFieldVm)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertOptionalField_v1(
                optionalFieldVm.OptionalField.OptionalFieldName,
                optionalFieldVm.OptionalField.OptionalFieldStyleId,
                adminUserGuid
            );
        }

        //Update in DB
        public void Update(OptionalFieldVM optionalFieldVM)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateOptionalField_v1(
                optionalFieldVM.OptionalField.OptionalFieldId,
                optionalFieldVM.OptionalField.OptionalFieldStyleId,
                optionalFieldVM.OptionalField.OptionalFieldName,
                adminUserGuid,
                optionalFieldVM.OptionalField.VersionNumber
                );
        }

        //Delete From DB
        public void Delete(OptionalField optionalField)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteOptionalField_v1(
                optionalField.OptionalFieldId,
                adminUserGuid,
                optionalField.VersionNumber
            );
        }
    }
}