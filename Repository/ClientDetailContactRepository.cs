using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System.Linq.Dynamic;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class ClientDetailContactRepository
    {
        private ClientDetailDC db = new ClientDetailDC(Settings.getConnectionString());

        //Get a Page of Contacts
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailContacts_v1Result> ListClientDetailContacts(int clientDetailId, int page)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectClientDetailContacts_v1(clientDetailId, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailContacts_v1Result>(result, page, Convert.ToInt32(totalRecords));
            return paginatedView;
        }


        //Add ClientDetail Contact
        public void Add(ClientDetail clientDetail, Contact contact)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertClientDetailContact_v1(
                clientDetail.ClientDetailId,
                contact.FirstName,
                contact.LastName,
				contact.Title,
				contact.JobTitle,
				contact.WorkPhone,
				contact.WorkMobile,
                contact.EmailAddress,
                contact.MiddleName,
                contact.ContactTypeId,
                adminUserGuid
            );

        }


        //Get one Item from Contact
        public ClientDetailContact GetContactClientDetail(int contactId)
        {
            return db.ClientDetailContacts.SingleOrDefault(c => c.ContactId == contactId);
        }

        //Get one Item from ClientDetail
        public ClientDetailContact GetClientDetailContact(int clientDetailId)
        {
            return db.ClientDetailContacts.SingleOrDefault(c => c.ClientDetailId == clientDetailId);
        }
    }
}
