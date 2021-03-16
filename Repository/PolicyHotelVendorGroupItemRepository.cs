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
    public class PolicyHotelVendorGroupItemRepository
    {
        private PolicyHotelVendorGroupItemDC db = new PolicyHotelVendorGroupItemDC(Settings.getConnectionString());

        //Sortable List
		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyHotelVendorGroupItems_v1Result> GetPolicyHotelVendorGroupItemsByPolicyGroup(int policyGroupID, int page, string sortField, int sortOrder)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectPolicyHotelVendorGroupItems_v1(policyGroupID, sortField, sortOrder, page, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyHotelVendorGroupItems_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

        //Add
        public void Add(PolicyHotelVendorGroupItem policyHotelVendorGroupItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPolicyHotelVendorGroupItem_v1(
                policyHotelVendorGroupItem.PolicyGroupId,
                policyHotelVendorGroupItem.PolicyLocationId,
                policyHotelVendorGroupItem.SupplierCode,
                policyHotelVendorGroupItem.ProductId,
                policyHotelVendorGroupItem.PolicyHotelStatusId,
                policyHotelVendorGroupItem.EnabledFlag,
                policyHotelVendorGroupItem.EnabledDate,
                policyHotelVendorGroupItem.ExpiryDate,
                policyHotelVendorGroupItem.TravelDateValidFrom,
                policyHotelVendorGroupItem.TravelDateValidTo,
                adminUserGuid
            );

        }

        //Add
        public void Update(PolicyHotelVendorGroupItem policyHotelVendorGroupItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdatePolicyHotelVendorGroupItem_v1(
                policyHotelVendorGroupItem.PolicyHotelVendorGroupItemId,
                policyHotelVendorGroupItem.PolicyGroupId,
                policyHotelVendorGroupItem.PolicyLocationId,
                policyHotelVendorGroupItem.SupplierCode,
                policyHotelVendorGroupItem.ProductId,
                policyHotelVendorGroupItem.PolicyHotelStatusId,
                policyHotelVendorGroupItem.EnabledFlag,
                policyHotelVendorGroupItem.EnabledDate,
                policyHotelVendorGroupItem.ExpiryDate,
                policyHotelVendorGroupItem.TravelDateValidFrom,
                policyHotelVendorGroupItem.TravelDateValidTo,
                adminUserGuid,
                policyHotelVendorGroupItem.VersionNumber
            );

        }

        //Delete
        public void Delete(PolicyHotelVendorGroupItem policyHotelVendorGroupItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePolicyHotelVendorGroupItem_v1(
                policyHotelVendorGroupItem.PolicyHotelVendorGroupItemId,
                adminUserGuid,
                policyHotelVendorGroupItem.VersionNumber
                );
        }

        //Get one Item
        public PolicyHotelVendorGroupItem GetPolicyHotelVendorGroupItem(int policyHotelVendorGroupItemId)
        {


            return db.PolicyHotelVendorGroupItems.SingleOrDefault(c => c.PolicyHotelVendorGroupItemId == policyHotelVendorGroupItemId);

        }


        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(PolicyHotelVendorGroupItem policyHotelVendorGroupItem)
        {
            //PolicyHotelStatusDescription 
            if (policyHotelVendorGroupItem.PolicyHotelStatusId != null)
            {
                int policyHotelStatusId = (int)policyHotelVendorGroupItem.PolicyHotelStatusId;
                PolicyHotelStatusRepository policyHotelStatusRepository = new PolicyHotelStatusRepository();
                PolicyHotelStatus policyHotelStatus = new PolicyHotelStatus();
                policyHotelStatus = policyHotelStatusRepository.GetPolicyHotelStatus(policyHotelStatusId);
                policyHotelVendorGroupItem.PolicyHotelStatus = policyHotelStatus.PolicyHotelStatusDescription;
            }

            //PolicyGroupName    
            PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyHotelVendorGroupItem.PolicyGroupId);
            policyHotelVendorGroupItem.PolicyGroupName = policyGroup.PolicyGroupName;
            policyHotelVendorGroupItem.PolicyGroupId = policyGroup.PolicyGroupId;

            //SupplierName
            SupplierRepository supplierRepository = new SupplierRepository();
            Supplier supplier = new Supplier();
            supplier = supplierRepository.GetSupplier(policyHotelVendorGroupItem.SupplierCode, policyHotelVendorGroupItem.ProductId);
            if (supplier != null)
            {
                policyHotelVendorGroupItem.SupplierName = supplier.SupplierName;
            }

            //PolicyLocationName
            PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
            PolicyLocation policyLocation = new PolicyLocation();
            policyLocation = policyLocationRepository.GetPolicyLocation(policyHotelVendorGroupItem.PolicyLocationId);
            if (policyLocation != null)
            {
                policyHotelVendorGroupItem.PolicyLocationName = policyLocation.PolicyLocationName;
            }

            ProductRepository productRepository = new ProductRepository();
            Product product = new Product();
            product = productRepository.GetProduct(policyHotelVendorGroupItem.ProductId);
            if (product != null)
            {
                policyHotelVendorGroupItem.ProductName = product.ProductName;
            }

        }

    }
}
