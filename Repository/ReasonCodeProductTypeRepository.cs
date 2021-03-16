using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class ReasonCodeProductTypeRepository
    {
        private ReasonCodeProductTypeDC db = new ReasonCodeProductTypeDC(Settings.getConnectionString());

        //Get a Page of ReasonCodeProductTypes Items - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectReasonCodeProductTypes_v1Result> PageReasonCodeProductTypes(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectReasonCodeProductTypes_v1(filter, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectReasonCodeProductTypes_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }


        public List<ReasonCode> GetAllReasonCodeProductTypes()
        {
            //List<ReasonCodeProductType> reasonCodeProductTypes = new List<ReasonCodeProductType>();
            var result = from n in db.spDesktopDataAdmin_SelectReasonCodes_v1()
                         select new ReasonCode { ReasonCodeValue = n.ReasonCode };



            return result.ToList();
        }

        public List<Product> LookUpReasonCodeProducts(string reasonCode)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = from n in db.spDesktopDataAdmin_SelectReasonCodeProducts_v1(reasonCode)

                         select
                             new Product
                             {
                                 ProductName = n.ProductName.Trim(),
                                 ProductId = n.ProductId
                             };
            return result.ToList();
        }

        public List<ReasonCodeType> LookUpAvailableReasonCodeProductReasonCodeTypes(int? reasonCodeItemId, int reasonCodeGroupId,string reasonCode, int productId)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = from n in db.spDesktopDataAdmin_SelectReasonCodeItemAvailableReasonCodeTypes_v1(reasonCodeItemId, reasonCodeGroupId, reasonCode, productId)

                         select
                             new ReasonCodeType
                             {
                                 ReasonCodeTypeId = n.ReasonCodeTypeId,
                                 ReasonCodeTypeDescription = n.ReasonCodeTypeDescription
                             };
            return result.ToList();
        }

        public ReasonCodeProductType GetReasonCodeProductType(string reasonCode, int? productId, int? reasonCodeTypeId)
        {
            if (!productId.HasValue || !reasonCodeTypeId.HasValue)
            {
                return null;
            }
            else
            {
                return db.ReasonCodeProductTypes.FirstOrDefault(
                   (n => n.ReasonCode.Trim().Equals(reasonCode) &&
                   n.ProductId == productId
                   &&
                    n.ReasonCodeTypeId == reasonCodeTypeId));
                    
                    

                /*var result = from n in db.ReasonCodeProductTypes.Where().SingleOrDefault().
                             where n.ReasonCode.Trim().Equals(reasonCode)
                             && n.ProductId.Equals(productId)
                             && n.ReasonCodeTypeId.Equals(reasonCodeTypeId);
                             

                return result;*/
            }
        }
    }
}
