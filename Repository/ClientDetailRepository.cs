using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using CWTDesktopDatabase.Helpers;
using System.Text.RegularExpressions;


namespace CWTDesktopDatabase.Repository
{
    public class ClientDetailRepository
    {
        private ClientDetailDC db = new ClientDetailDC(Settings.getConnectionString());

        //Get a Page of ClientDetail Addresses
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailAddresses_v1Result> ListClientDetailAddresses(int clientDetailId, int page)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectClientDetailAddresses_v1(clientDetailId, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailAddresses_v1Result>(result, page, Convert.ToInt32(totalRecords));
            return paginatedView;
        }

        //Get a Page of ClientDetail Contacts
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

        //Get a Page of ClientDetail Supplier Prroducts
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailSupplierProducts_v1Result> ListClientDetailSupplierProducts(int clientDetailId, int page)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectClientDetailSupplierProducts_v1(clientDetailId, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailSupplierProducts_v1Result>(result, page, Convert.ToInt32(totalRecords));
            return paginatedView;
        }

        //Get a Page of ClientDetail SubProductFormOfPaymentTypes
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailSubProductFormOfPaymentTypes_v1Result> ListClientDetailSubProductFormOfPaymentTypes(int clientDetailId, int page)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectClientDetailSubProductFormOfPaymentTypes_v1(clientDetailId, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailSubProductFormOfPaymentTypes_v1Result>(result, page, Convert.ToInt32(totalRecords));
            return paginatedView;
        }

        //Get one ClientDetail ESCInformation
        public ClientDetailESCInformation GetClientDetailESCInformation(int clientDetailId)
        {
            return db.ClientDetailESCInformations.SingleOrDefault(c => c.ClientDetailId == clientDetailId);
        }

        //Get one Item
        public ClientDetail GetGroup(int clientDetailId)
        {
            return db.ClientDetails.SingleOrDefault(c => c.ClientDetailId == clientDetailId);
        }
        
        //Add Data From Linked Tables for Display
        public void EditGroupForDisplay(ClientDetail group)
        {
            TripTypeRepository tripTypeRepository = new TripTypeRepository();
            TripType tripType = new TripType();
            tripType = tripTypeRepository.GetTripType(group.TripTypeId);
            if (tripType != null)
            {
                group.TripType = tripType.TripTypeDescription;
            }

            HierarchyRepository hierarchyRepository = new HierarchyRepository();

            fnDesktopDataAdmin_SelectClientDetailHierarchy_v1Result hierarchy = new fnDesktopDataAdmin_SelectClientDetailHierarchy_v1Result();
            hierarchy = GetGroupHierarchy(group.ClientDetailId);

            group.HierarchyType = hierarchy.HierarchyType;
            group.HierarchyCode = hierarchy.HierarchyCode.ToString();
            group.HierarchyItem = hierarchy.HierarchyName.Trim();
            group.SourceSystemCode = hierarchy.SourceSystemCode;
            group.ClientDetailName = Regex.Replace(group.ClientDetailName, @"[^\w\s\-()*]", "-");

            if (hierarchy.HierarchyType == "ClientSubUnitTravelerType")
            {
                group.ClientSubUnitGuid = hierarchy.HierarchyCode.ToString();
                group.ClientSubUnitName = hierarchy.HierarchyName.Trim();
                group.TravelerTypeGuid = hierarchy.TravelerTypeGuid;
                group.TravelerTypeName = hierarchy.TravelerTypeName.Trim();
            }


        }
        
        //Get Hierarchy Details
        public fnDesktopDataAdmin_SelectClientDetailHierarchy_v1Result GetGroupHierarchy(int id)
        {
            var result = db.fnDesktopDataAdmin_SelectClientDetailHierarchy_v1(id).FirstOrDefault();
            return result;
        }
 
        //Change the deleted status on an item
        public void UpdateGroupDeletedStatus(ClientDetail clientDetail)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateClientDetailDeletedStatus_v1(
                    clientDetail.ClientDetailId,
                    clientDetail.DeletedFlag,
                    adminUserGuid,
                    clientDetail.VersionNumber
                    );

        }
        
    }
}


