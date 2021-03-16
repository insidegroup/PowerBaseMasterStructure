using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;
using System.Web.Security;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Repository
{
    public class FormOfPaymentAdviceMessageGroupItemRepository
    {
        private FormOfPaymentAdviceMessageGroupDC db = new FormOfPaymentAdviceMessageGroupDC(Settings.getConnectionString());

        //Get a Page of FOP Advice Message Group Items - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectFormOfPaymentAdviceMessageGroupItems_v1Result> PageFormOfPaymentAdviceMessageGroupItems(int formOfPaymentAdviceMessageGroupId, int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectFormOfPaymentAdviceMessageGroupItems_v1(formOfPaymentAdviceMessageGroupId, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectFormOfPaymentAdviceMessageGroupItems_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one Item
		public FormOfPaymentAdviceMessageGroupItem GetItem(int formOfPaymentAdviceMessageId)
		{
			return db.FormOfPaymentAdviceMessageGroupItems.SingleOrDefault(c => c.FormOfPaymentAdviceMessageGroupItemId == formOfPaymentAdviceMessageId);
		}

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(FormOfPaymentAdviceMessageGroupItem formOfPaymentAdviceMessageGroupItem)
        {
			//Supplier / Product
			if (formOfPaymentAdviceMessageGroupItem.SupplierCode != null && formOfPaymentAdviceMessageGroupItem.ProductId > 0)
			{
				SupplierRepository supplierRepository = new SupplierRepository();
				Supplier supplier = supplierRepository.GetSupplier(formOfPaymentAdviceMessageGroupItem.SupplierCode, formOfPaymentAdviceMessageGroupItem.ProductId);
				if (supplier != null)
				{
					formOfPaymentAdviceMessageGroupItem.SupplierName = supplier.SupplierName;
				}

				ProductRepository productRepository = new ProductRepository();
				Product product = productRepository.GetProduct(formOfPaymentAdviceMessageGroupItem.ProductId);
				if (product != null)
				{
					formOfPaymentAdviceMessageGroupItem.ProductName = product.ProductName;
				}
			}

			//CountryName
			CountryRepository countryRepository = new CountryRepository();
			Country country = countryRepository.GetCountry(formOfPaymentAdviceMessageGroupItem.CountryCode);
			if (country != null)
			{
				formOfPaymentAdviceMessageGroupItem.CountryName = country.CountryName;
			}

			//FormOfPaymentTypeDescription
			FormOfPaymentTypeRepository formOfPaymentTypeRepository = new FormOfPaymentTypeRepository();
			FormOfPaymentType formOfPaymentType = formOfPaymentTypeRepository.GetFormOfPaymentType(formOfPaymentAdviceMessageGroupItem.FormofPaymentTypeID);
			if (formOfPaymentType != null)
			{
				formOfPaymentAdviceMessageGroupItem.FormOfPaymentTypeDescription = formOfPaymentType.FormOfPaymentTypeDescription;
			}

            //Set LanguageName to en-gb
            LanguageRepository languageRepository = new LanguageRepository();
            Language language = languageRepository.GetLanguage("en-GB");
            if (language != null)
            {
                formOfPaymentAdviceMessageGroupItem.LanguageCode = language.LanguageCode;
                formOfPaymentAdviceMessageGroupItem.LanguageName = language.LanguageName;
            }
        }

        //Add Item 
        public void Add(FormOfPaymentAdviceMessageGroupItem formOfPaymentAdviceMessageGroupItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertFormOfPaymentAdviceMessageGroupItem_v1(
                formOfPaymentAdviceMessageGroupItem.FormOfPaymentAdviceMessageGroupID,
               formOfPaymentAdviceMessageGroupItem.SupplierCode,
				formOfPaymentAdviceMessageGroupItem.ProductId,
				formOfPaymentAdviceMessageGroupItem.CountryCode,
				formOfPaymentAdviceMessageGroupItem.TravelIndicator,
				formOfPaymentAdviceMessageGroupItem.FormofPaymentTypeID,
				formOfPaymentAdviceMessageGroupItem.FormOfPaymentAdviceMessage,
				formOfPaymentAdviceMessageGroupItem.LanguageCode,
                adminUserGuid
            );
        }

        //Edit Item 
        public void Edit(FormOfPaymentAdviceMessageGroupItem formOfPaymentAdviceMessageGroupItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateFormOfPaymentAdviceMessageGroupItem_v1(
                formOfPaymentAdviceMessageGroupItem.FormOfPaymentAdviceMessageGroupItemId,
				formOfPaymentAdviceMessageGroupItem.SupplierCode,
				formOfPaymentAdviceMessageGroupItem.ProductId,
				formOfPaymentAdviceMessageGroupItem.CountryCode,
				formOfPaymentAdviceMessageGroupItem.TravelIndicator,
				formOfPaymentAdviceMessageGroupItem.FormofPaymentTypeID,
				formOfPaymentAdviceMessageGroupItem.FormOfPaymentAdviceMessage,
				formOfPaymentAdviceMessageGroupItem.LanguageCode,
                adminUserGuid,
                formOfPaymentAdviceMessageGroupItem.VersionNumber
            );
        }

        //Delete Item
        public void Delete(FormOfPaymentAdviceMessageGroupItem formOfPaymentAdviceMessageGroupItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteFormOfPaymentAdviceMessageGroupItem_v1(
                formOfPaymentAdviceMessageGroupItem.FormOfPaymentAdviceMessageGroupItemId,
                adminUserGuid,
                formOfPaymentAdviceMessageGroupItem.VersionNumber);
        }
    }
}
