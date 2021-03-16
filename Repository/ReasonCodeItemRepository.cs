using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;
using System.Web.Security;

namespace CWTDesktopDatabase.Repository
{
    public class ReasonCodeItemRepository
    {
        private ReasonCodeItemDC db = new ReasonCodeItemDC(Settings.getConnectionString());
		private	ReasonCodeAlternativeDescriptionDC reasonCodeAlternativeDescriptionDC = new ReasonCodeAlternativeDescriptionDC(Settings.getConnectionString());

        //Get a Page of Reason Code Items - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectReasonCodeItems_v1Result> PageReasonCodeItems(int reasonCodeGroupId, int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectReasonCodeItems_v1(reasonCodeGroupId, filter, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectReasonCodeItems_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one Item
		public ReasonCodeItem GetItem(int reasonCodeItemId)
		{
			return db.ReasonCodeItems.SingleOrDefault(c => c.ReasonCodeItemId == reasonCodeItemId);
		}

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(ReasonCodeItem reasonCodeItem)
        {
            ProductRepository productRepository = new ProductRepository();
            Product product = new Product();
            product = productRepository.GetProduct(reasonCodeItem.ProductId);
            if (product != null)
            {
                reasonCodeItem.ProductName = product.ProductName;
            }

            ReasonCodeTypeRepository reasonCodeTypeRepository = new ReasonCodeTypeRepository();
            ReasonCodeType reasonCodeType = new ReasonCodeType();
            reasonCodeType = reasonCodeTypeRepository.GetItem(reasonCodeItem.ReasonCodeTypeId);
            if (reasonCodeType != null)
            {
                reasonCodeItem.ReasonCodeTypeDescription = reasonCodeType.ReasonCodeTypeDescription;
            }

			ReasonCodeProductTypeDescriptionRepository reasonCodeProductTypeDescriptionRepository = new ReasonCodeProductTypeDescriptionRepository();
			ReasonCodeProductTypeDescription reasonCodeProductTypeDescription = new ReasonCodeProductTypeDescription();
			reasonCodeProductTypeDescription = reasonCodeProductTypeDescriptionRepository.GetItem(
					"en-GB",
					reasonCodeItem.ReasonCode,
					reasonCodeItem.ProductId,
					reasonCodeItem.ReasonCodeTypeId
			);
			reasonCodeItem.ReasonCodeDescription = (reasonCodeProductTypeDescription != null) ? reasonCodeProductTypeDescription.ReasonCodeProductTypeDescription1 : String.Empty;
			
            ReasonCodeGroupRepository reasonCodeGroupRepository = new ReasonCodeGroupRepository();
            ReasonCodeGroup reasonCodeGroup = new ReasonCodeGroup();
            reasonCodeGroup = reasonCodeGroupRepository.GetGroup(reasonCodeItem.ReasonCodeGroupId);
			if (reasonCodeGroup != null)
			{
				reasonCodeItem.ReasonCodeGroupName = reasonCodeGroup.ReasonCodeGroupName;
			}
        }

        //Add Item 
        public void Add(ReasonCodeItem reasonCodeItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertReasonCodeItem_v1(
                reasonCodeItem.ReasonCodeGroupId,
                reasonCodeItem.ReasonCode,
                reasonCodeItem.ProductId,
                reasonCodeItem.ReasonCodeTypeId,
				reasonCodeItem.TravelerFacingFlag,
                adminUserGuid
            );
        }

       public List<ReasonCodeType> GetReasonCodeItemReasonCodeTypes(int reasonCodeGroupId){

            // return db.ReasonCodeItems.SingleOrDefault(c => c.ReasonCodeItemId == reasonCodeItemId);
            return (from n in db.spDesktopDataAdmin_SelectReasonCodeGroupReasonCodeTypes_v1(reasonCodeGroupId)
                    select new
                        ReasonCodeType
                        {
                            ReasonCodeTypeId = n.ReasonCodeTypeId,
                            ReasonCodeTypeDescription = n.ReasonCodeTypeDescription
                        }).ToList();
        }
        //Add Item 
        public void Edit(ReasonCodeItem reasonCodeItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateReasonCodeItem_v1(
                reasonCodeItem.ReasonCodeItemId,
                reasonCodeItem.ReasonCode,
                reasonCodeItem.ProductId,
                reasonCodeItem.ReasonCodeTypeId,
				reasonCodeItem.TravelerFacingFlag,
                adminUserGuid,
                reasonCodeItem.VersionNumber
            );
        }

        //Delete Item
        public void Delete(ReasonCodeItem reasonCodeItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteReasonCodeItem_v1(
                reasonCodeItem.ReasonCodeItemId,
                adminUserGuid,
                reasonCodeItem.VersionNumber);
        }
    }
}
