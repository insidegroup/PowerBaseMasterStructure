using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class PolicySupplierServiceInformationRepository
    {
        private PolicySupplierServiceInformationDC db = new PolicySupplierServiceInformationDC(Settings.getConnectionString());

        //Get a Page of PolicySupplierServiceInformations - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicySupplierServiceInformations_v1Result> PagePolicySupplierServiceInformations(int policyGroupId, int page, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectPolicySupplierServiceInformations_v1(policyGroupId, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicySupplierServiceInformations_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }


        //Get one Item
        public PolicySupplierServiceInformation GetPolicySupplierServiceInformation(int policySupplierServiceInformationId)
        {
            return db.PolicySupplierServiceInformations.SingleOrDefault(c => c.PolicySupplierServiceInformationId == policySupplierServiceInformationId);
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(PolicySupplierServiceInformation policySupplierServiceInformation)
        {
            //PolicySupplierServiceInformationType
            PolicySupplierServiceInformationTypeRepository policySupplierServiceInformationTypeRepository = new PolicySupplierServiceInformationTypeRepository();
            PolicySupplierServiceInformationType policySupplierServiceInformationType = new PolicySupplierServiceInformationType();
            policySupplierServiceInformationType = policySupplierServiceInformationTypeRepository.GetPolicySupplierServiceInformationType(policySupplierServiceInformation.PolicySupplierServiceInformationTypeId);
            if (policySupplierServiceInformationType != null)
            {
                policySupplierServiceInformation.PolicySupplierServiceInformationTypeDescription = policySupplierServiceInformationType.PolicySupplierServiceInformationTypeDescription;
            }

            //Supplier
            SupplierRepository supplierRepository = new SupplierRepository();
            Supplier supplier = new Supplier();
            supplier = supplierRepository.GetSupplier(policySupplierServiceInformation.SupplierCode, policySupplierServiceInformation.ProductId);
            if (supplier != null)
            {
                policySupplierServiceInformation.SupplierName = supplier.SupplierName;
            }

            //Product
            ProductRepository productRepository = new ProductRepository();
            Product product = new Product();
            product = productRepository.GetProduct(policySupplierServiceInformation.ProductId);
            if (product != null)
            {
                policySupplierServiceInformation.ProductName = product.ProductName;
            }

            //EnabledFlag is nullable
            if (policySupplierServiceInformation.EnabledFlag != true)
            {
                policySupplierServiceInformation.EnabledFlag = false;
            }
            policySupplierServiceInformation.EnabledFlagNonNullable = (bool)policySupplierServiceInformation.EnabledFlag;

            //PolicyGroup
            PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
            PolicyGroup policyGroup = policyGroupRepository.GetGroup(policySupplierServiceInformation.PolicyGroupId);
            policySupplierServiceInformation.PolicyGroupName = policyGroup.PolicyGroupName;

        }


        //Add
        public void Add(PolicySupplierServiceInformation PolicySupplierServiceInformation)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPolicySupplierServiceInformation_v1(
                PolicySupplierServiceInformation.SupplierCode,
                PolicySupplierServiceInformation.PolicySupplierServiceInformationValue,
                PolicySupplierServiceInformation.ProductId,
                PolicySupplierServiceInformation.PolicySupplierServiceInformationTypeId,
                PolicySupplierServiceInformation.PolicyGroupId,
                PolicySupplierServiceInformation.EnabledFlagNonNullable,
                PolicySupplierServiceInformation.EnabledDate,
                PolicySupplierServiceInformation.ExpiryDate,
                adminUserGuid
            );

        }

        //Add
        public void Update(PolicySupplierServiceInformation PolicySupplierServiceInformation)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdatePolicySupplierServiceInformation_v1(
                PolicySupplierServiceInformation.PolicySupplierServiceInformationId,
                PolicySupplierServiceInformation.SupplierCode,
                PolicySupplierServiceInformation.PolicySupplierServiceInformationValue,
                PolicySupplierServiceInformation.ProductId,
                PolicySupplierServiceInformation.PolicySupplierServiceInformationTypeId,
                PolicySupplierServiceInformation.PolicyGroupId,
                PolicySupplierServiceInformation.EnabledFlagNonNullable,
                PolicySupplierServiceInformation.EnabledDate,
                PolicySupplierServiceInformation.ExpiryDate,
                adminUserGuid,
                PolicySupplierServiceInformation.VersionNumber
            );

        }

        //Delete
        public void Delete(PolicySupplierServiceInformation PolicySupplierServiceInformation)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePolicySupplierServiceInformation_v1(
                PolicySupplierServiceInformation.PolicySupplierServiceInformationId,
                adminUserGuid,
                PolicySupplierServiceInformation.VersionNumber
                );
        }

    }
}
