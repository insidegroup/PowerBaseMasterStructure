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
    public class PolicyCarVendorGroupItemRepository
    {
        private PolicyCarVendorGroupItemDC db = new PolicyCarVendorGroupItemDC(Settings.getConnectionString());

        //Sortable List
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCarVendorGroupItems_v1Result> GetPolicyCarVendorGroupItems(int policyGroupID, string sortField, int sortOrder, int page)
        {
            //query db
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectPolicyCarVendorGroupItems_v1(policyGroupID, adminUserGuid, sortField, Convert.ToBoolean(sortOrder), page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCarVendorGroupItems_v1Result>(result, page, totalRecords);
            return paginatedView;

        }

        //Add
        public void Add(PolicyCarVendorGroupItem policyCarVendorGroupItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPolicyCarVendorGroupItem_v1(
                policyCarVendorGroupItem.PolicyGroupId,
                policyCarVendorGroupItem.PolicyLocationId,
                policyCarVendorGroupItem.SupplierCode,
                policyCarVendorGroupItem.ProductId,
                policyCarVendorGroupItem.PolicyCarStatusId,
                policyCarVendorGroupItem.EnabledFlag,
                policyCarVendorGroupItem.EnabledDate,
                policyCarVendorGroupItem.ExpiryDate,
                policyCarVendorGroupItem.TravelDateValidFrom,
                policyCarVendorGroupItem.TravelDateValidTo,
                adminUserGuid
            );

        }

        //Add
        public void Update(PolicyCarVendorGroupItem policyCarVendorGroupItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdatePolicyCarVendorGroupItem_v1(
                policyCarVendorGroupItem.PolicyCarVendorGroupItemId,
                policyCarVendorGroupItem.PolicyGroupId,
                policyCarVendorGroupItem.PolicyLocationId,
                policyCarVendorGroupItem.SupplierCode,
                policyCarVendorGroupItem.ProductId,
                policyCarVendorGroupItem.PolicyCarStatusId,
                policyCarVendorGroupItem.EnabledFlag,
                policyCarVendorGroupItem.EnabledDate,
                policyCarVendorGroupItem.ExpiryDate,
                policyCarVendorGroupItem.TravelDateValidFrom,
                policyCarVendorGroupItem.TravelDateValidTo,
                adminUserGuid,
                policyCarVendorGroupItem.VersionNumber
            );

        }

        //Delete
        public void Delete(PolicyCarVendorGroupItem policyCarVendorGroupItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePolicyCarVendorGroupItem_v1(
                policyCarVendorGroupItem.PolicyCarVendorGroupItemId,
                adminUserGuid,
                policyCarVendorGroupItem.VersionNumber
                );
        }

        //Get one Item
        public PolicyCarVendorGroupItem GetPolicyCarVendorGroupItem(int policyCarVendorGroupItemId)
        {


            return db.PolicyCarVendorGroupItems.SingleOrDefault(c => c.PolicyCarVendorGroupItemId == policyCarVendorGroupItemId);

        }


        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(PolicyCarVendorGroupItem policyCarVendorGroupItem)
        {
            //PolicyCarStatusDescription 
            if (policyCarVendorGroupItem.PolicyCarStatusId != null)
            {
                int policyCarStatusId = (int)policyCarVendorGroupItem.PolicyCarStatusId;
                PolicyCarStatusRepository policyCarStatusRepository = new PolicyCarStatusRepository();
                PolicyCarStatus policyCarStatus = new PolicyCarStatus();
                policyCarStatus = policyCarStatusRepository.GetPolicyCarStatus(policyCarStatusId);
                policyCarVendorGroupItem.PolicyCarStatus = policyCarStatus.PolicyCarStatusDescription;
            }

            //PolicyGroupName    
            PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyCarVendorGroupItem.PolicyGroupId);
            policyCarVendorGroupItem.PolicyGroupName = policyGroup.PolicyGroupName;

            //SupplierName
            SupplierRepository supplierRepository = new SupplierRepository();
            Supplier supplier = new Supplier();
            supplier = supplierRepository.GetSupplier(policyCarVendorGroupItem.SupplierCode, policyCarVendorGroupItem.ProductId);
            if (supplier != null)
            {
                policyCarVendorGroupItem.SupplierName = supplier.SupplierName;
            }

            //PolicyLocationName
            PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
            PolicyLocation policyLocation = new PolicyLocation();
            policyLocation = policyLocationRepository.GetPolicyLocation(policyCarVendorGroupItem.PolicyLocationId);
            if (policyLocation != null)
            {
                policyCarVendorGroupItem.PolicyLocationName = policyLocation.PolicyLocationName;
            }

            ProductRepository productRepository = new ProductRepository();
            Product product = new Product();
            product = productRepository.GetProduct(policyCarVendorGroupItem.ProductId);
            if (product != null)
            {
                policyCarVendorGroupItem.ProductName = product.ProductName;
            }

        }

    }
}
