using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class ContactTypeRepository
    {
        //DataContext
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get All ContactTypes2 - for SelectLists
        public IQueryable<ContactType> GetAllContactTypes()
        {
			return db.ContactTypes.OrderBy(c => c.ContactTypeName);
        }

		//Get a Single ContactType
		public ContactType GetContactType(int contactTypeId)
		{
			return db.ContactTypes.SingleOrDefault(c => c.ContactTypeId == contactTypeId);
		}

		//Get a Single ContactType by Name
		public ContactType GetContactTypeByName(string contactTypeName)
		{
			return db.ContactTypes.Where(c => c.ContactTypeName == contactTypeName).FirstOrDefault();
		}

        //Get a Page of ContactTypes2 - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectContactTypes_v1Result> PageContactTypes(int page)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectContactTypes_v1(page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectContactTypes_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Add to DB
        public void Add(ContactType contactType)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertContactType_v1(
			   contactType.ContactTypeName,
                adminUserGuid
            );

        }

        //Update in DB
        public void Update(ContactType contactType)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateContactType_v1(
                contactType.ContactTypeId,
				contactType.ContactTypeName,
                adminUserGuid,
                contactType.VersionNumber
            );

        }

        //Delete From DB
        public void Delete(ContactType contactType)
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