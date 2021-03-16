using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class ContactRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public Contact GetContact(int contactId)
        {
            return db.Contacts.SingleOrDefault(c => c.ContactId == contactId);
        }

        //Add Data From Linked Tables for Display
        public void EditForDisplay(Contact contact)
        {

            ContactType contactType = new ContactType();
            contactType = db.ContactTypes.SingleOrDefault(c => c.ContactTypeId == contact.ContactTypeId);
			contact.ContactTypeName = contactType.ContactTypeName;
        }

        //Edit Contact
        public void Edit(Contact contact)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateContact_v1(
                contact.ContactId,
                contact.FirstName,
                contact.LastName,
				contact.Title,
				contact.JobTitle,
				contact.WorkPhone,
				contact.WorkMobile,
                contact.EmailAddress,
                contact.MiddleName,
                contact.ContactTypeId,
                adminUserGuid,
                contact.VersionNumber
            );

        }

        //Delete Contact
        public void Delete(Contact contact)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteContact_v1(
                contact.ContactId,
                adminUserGuid,
                contact.VersionNumber 
            );

        }
    }
}