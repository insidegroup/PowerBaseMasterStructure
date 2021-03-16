using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Repository
{
    public class MerchantFeeRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of MerchantFees - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectMerchantFees_v1Result> PageMerchantFees(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectMerchantFees_v1(filter, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectMerchantFees_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one Item
        public MerchantFee GetItem(int merchantFeeId)
        {
            return db.MerchantFees.SingleOrDefault(c => (c.MerchantFeeId == merchantFeeId));
        }

        //Get one Item
       // public IQueryable<MerchantFee> GetAllItems()
        //{
        //    return db.MerchantFees.OrderBy(c => c.MerchantFeeDescription);
        //}

        public void EditForDisplay(MerchantFee merchantFee)
        {
			CountryRepository countryRepository = new CountryRepository();
			Country country = new Country();
			if (merchantFee.ProductId != null)
			{
				country = countryRepository.GetCountry(merchantFee.CountryCode);
				if (country != null)
				{
					merchantFee.CountryName = country.CountryName;
				}
			} 

			CreditCardVendorRepository creditCardVendorRepository = new CreditCardVendorRepository();
			CreditCardVendor creditCardVendor = new CreditCardVendor();
			if (merchantFee.ProductId != null)
			{
				creditCardVendor = creditCardVendorRepository.GetCreditCardVendor(merchantFee.CreditCardVendorCode);
				if (creditCardVendor != null)
				{
					merchantFee.CreditCardVendorName = creditCardVendor.CreditCardVendorName;
				}
			}
			
			SupplierRepository supplierRepository = new SupplierRepository();
            Supplier supplier = new Supplier();
            if (merchantFee.ProductId != null)
            {
                supplier = supplierRepository.GetSupplier(merchantFee.SupplierCode, (int)merchantFee.ProductId);
                if (supplier != null)
                {
                    merchantFee.SupplierName = supplier.SupplierName;
                }
            }

            ProductRepository productRepository = new ProductRepository();
            Product product = new Product();
            if (merchantFee.ProductId != null)
            {
                product = productRepository.GetProduct((int)merchantFee.ProductId);
                if (product != null)
                {
                    merchantFee.ProductName = product.ProductName;
                }
            }
        }
        //Add to DB
        public void Add(MerchantFee merchantFee)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertMerchantFee_v1(
                merchantFee.CountryCode,
                merchantFee.CreditCardVendorCode,
                merchantFee.ProductId,
                merchantFee.SupplierCode,
                merchantFee.EnabledDate,
                merchantFee.ExpiryDate,
                merchantFee.MerchantFeePercent,
                merchantFee.MerchantFeeDescription,
                adminUserGuid
            );
        }

        //Update in DB
        public void Update(MerchantFee merchantFee)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateMerchantFee_v1(
                merchantFee.MerchantFeeId,
                merchantFee.CountryCode,
                merchantFee.CreditCardVendorCode,
                merchantFee.ProductId,
                merchantFee.SupplierCode,
                merchantFee.EnabledDate,
                merchantFee.ExpiryDate,
                merchantFee.MerchantFeePercent,
                merchantFee.MerchantFeeDescription,
                adminUserGuid,
                merchantFee.VersionNumber
            );
        }

        //Delete From DB
        public void Delete(MerchantFee merchantFee)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteMerchantFee_v1(
                merchantFee.MerchantFeeId,
                adminUserGuid,
                merchantFee.VersionNumber
            );
        }
    
    }
}