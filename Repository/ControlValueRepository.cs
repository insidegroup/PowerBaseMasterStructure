using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using CWTDesktopDatabase.Helpers;


namespace CWTDesktopDatabase.Repository
{
    public class ControlValueRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of ControlValues - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectControlValues_v1Result> PageControlValues(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectControlValues_v1(filter, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectControlValues_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get a Single ControlValue
        public ControlValue GetControlValue(int id)
        {
            return db.ControlValues.SingleOrDefault(c => c.ControlValueId == id);
        }

        //Add Data From Linked Tables for Display
        public void EditForDisplay(ControlValue controlValue)
        {
            ControlPropertyRepository controlPropertyRepository = new ControlPropertyRepository();
            ControlProperty controlProperty = new ControlProperty();
            controlProperty = controlPropertyRepository.GetControlProperty(controlValue.ControlPropertyId);
            if (controlProperty != null)
            {
                controlValue.ControlPropertyDescription = controlProperty.ControlPropertyDescription;
            }
            //ControlNameRepository controlNameRepository = new ControlNameRepository();
            //ControlName controlName = new ControlName();
            //controlName = controlNameRepository.GetControlName(controlValue.ControlNameId);
            //if (controlName != null)
            //{
            //    controlValue.ControlName = controlName.ControlName1;
           // }
        }

        //Add to DB
        public void Add(ControlValue controlValue)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertControlValue_v1(
                controlValue.ControlValueId,
                controlValue.ControlPropertyId,
                controlValue.ControlNameId,
                controlValue.DefaultValueFlag,
                controlValue.ControlValue1,
                adminUserGuid
            );

        }

        //Update in DB
        public void Update(ControlValue controlValue)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateControlValue_v1(
                controlValue.ControlValueId,
                controlValue.ControlPropertyId,
                controlValue.ControlNameId,
                controlValue.DefaultValueFlag,
                controlValue.ControlValue1,
                adminUserGuid,
                controlValue.VersionNumber
            );

        }

        //Delete From DB
        public void Delete(ControlValue controlValue)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteControlValue_v1(
                controlValue.ControlValueId,
                adminUserGuid,
                controlValue.VersionNumber
            );
        }

        //REMOVED : List of All Items - Sortable
        /*public IQueryable<fnDesktopDataAdmin_SelectControlValues_v1Result> GetControlValues(string filter, string sortField, int sortOrder)
        {
            if (sortOrder == 0)
            {
                sortField = sortField + " ascending";
            }
            else
            {
                sortField = sortField + " descending";
            }

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            if (filter == "")
            {
                return db.fnDesktopDataAdmin_SelectControlValues_v1(adminUserGuid).OrderBy(sortField);
            }
            else
            {
                return db.fnDesktopDataAdmin_SelectControlValues_v1(adminUserGuid).OrderBy(sortField).Where
                        (c => (c.ControlValue.Contains(filter)) || (c.ControlName.Contains(filter)));
            }
        }
        */

    }
}
