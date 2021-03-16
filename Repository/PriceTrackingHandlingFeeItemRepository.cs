using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;
using System.Text.RegularExpressions;

namespace CWTDesktopDatabase.Repository
{
    public class PriceTrackingHandlingFeeItemRepository
    {
        private PriceTrackingDC db = new PriceTrackingDC(Settings.getConnectionString());

		//Get a Page of PriceTrackingHandlingFeeItems - for Page Listings
		public CWTPaginatedList<spDesktopDataAdmin_SelectPriceTrackingHandlingFeeItems_v1Result> PagePriceTrackingHandlingFeeItems(int id, int page, string filter, string sortField, int sortOrder)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectPriceTrackingHandlingFeeItems_v1(id, filter, sortField, sortOrder, page, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPriceTrackingHandlingFeeItems_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

		//Get One PriceTrackingHandlingFeeItem
		public PriceTrackingHandlingFeeItem PriceTrackingHandlingFeeItem(int id)
		{
			return db.PriceTrackingHandlingFeeItems.SingleOrDefault(c => c.PriceTrackingHandlingFeeItemId == id);
		}

		//Add Data From Linked Tables for Display
		public void EditForDisplay(PriceTrackingHandlingFeeItem priceTrackingHandlingFeeItem)
		{
			ProductRepository productRepository = new ProductRepository();
			Product product = productRepository.GetProduct(priceTrackingHandlingFeeItem.ProductId);
			if(product != null)
			{
				priceTrackingHandlingFeeItem.Product = product;
			}
		}

		//Add to DB
		public void Add(PriceTrackingHandlingFeeItem priceTrackingHandlingFeeItem)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			int? priceTrackingHandlingFeeItemId = new Int32();

			db.spDesktopDataAdmin_InsertPriceTrackingHandlingFeeItem_v1(
				ref priceTrackingHandlingFeeItemId,
				priceTrackingHandlingFeeItem.PriceTrackingHandlingFeeGroupId,
				priceTrackingHandlingFeeItem.ProductId,
				priceTrackingHandlingFeeItem.PriceTrackingSystemId,
				priceTrackingHandlingFeeItem.SavingAmountPercentage,
				priceTrackingHandlingFeeItem.HandlingFee, 
				adminUserGuid
			);

			priceTrackingHandlingFeeItem.PriceTrackingHandlingFeeItemId = (int)priceTrackingHandlingFeeItemId;
		}

		//Update in DB
		public void Update(PriceTrackingHandlingFeeItem priceTrackingHandlingFeeItem)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdatePriceTrackingHandlingFeeItem_v1(
				priceTrackingHandlingFeeItem.PriceTrackingHandlingFeeItemId,
				priceTrackingHandlingFeeItem.ProductId,
				priceTrackingHandlingFeeItem.PriceTrackingSystemId,
				priceTrackingHandlingFeeItem.SavingAmountPercentage,
				priceTrackingHandlingFeeItem.HandlingFee,
				adminUserGuid,
				priceTrackingHandlingFeeItem.VersionNumber
			);
		}

		//Delete From DB
		public void Delete(PriceTrackingHandlingFeeItem PriceTrackingHandlingFeeItem)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeletePriceTrackingHandlingFeeItem_v1(
				PriceTrackingHandlingFeeItem.PriceTrackingHandlingFeeItemId,
				adminUserGuid,
				PriceTrackingHandlingFeeItem.VersionNumber
			);
		}

	}
}
