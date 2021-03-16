using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class ContactType2Repository
    {
        //DataContext
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get All ContactTypes2 - for SelectLists
        public IQueryable<ContactType2> GetAllContactTypes2()
        {
            return db.ContactType2s.OrderBy(c => c.ContactTypeDescription);
        }

        //Get a Single ContactType
        public ContactType2 GetContactType(int contactTypeId)
        {
            return db.ContactType2s.SingleOrDefault(c => c.ContactTypeId == contactTypeId);
        }

        //Get a Page of ContactTypes2 - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectContactTypes2_v1Result> PageContactTypes2(int page)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectContactTypes2_v1(page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectContactTypes2_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Add to DB
        public void Add(ContactType2 contactType)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertContactType_v1(
			   contactType.ContactTypeDescription,
                adminUserGuid
            );

        }

        //Update in DB
        public void Update(ContactType2 contactType)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateContactType_v1(
                contactType.ContactTypeId,
				contactType.ContactTypeDescription,
                adminUserGuid,
                contactType.VersionNumber
            );

        }

        //Delete From DB
        public void Delete(ContactType2 contactType)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteContactType_v1(
                contactType.ContactTypeId,
                adminUserGuid,
                contactType.VersionNumber
            );
        }

    }
}