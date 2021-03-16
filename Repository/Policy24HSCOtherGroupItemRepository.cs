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
    public class Policy24HSCOtherGroupItemRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Sortable List
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicy24HSCOtherGroupItems_v1Result> GetPolicy24HSCOtherGroupItems(int id, string filter, string sortField, int sortOrder, int page)
        {
            //query db
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectPolicy24HSCOtherGroupItems_v1(id, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return1
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicy24HSCOtherGroupItems_v1Result>(result,page,totalRecords);
            return paginatedView;

        }

		//Get one Item
		public Policy24HSCOtherGroupItem GetPolicy24HSCOtherGroupItem(int policyId, int policyOtherGroupHeaderId)
		{
			return db.Policy24HSCOtherGroupItems.SingleOrDefault(c => c.PolicyGroupId == policyId && c.PolicyOtherGroupHeaderId == policyOtherGroupHeaderId);
		}

		//public List<SelectListItem> AirVendorRankings()
		//{
		//	var numbers = (from p in Enumerable.Range(1, 9)
		//				   select new SelectListItem
		//				   {
		//					   Text = p.ToString(),
		//					   Value = p.ToString()
		//				   });
		//	return numbers.ToList();
		//}

		////Add
		//public void Add(Policy24HSCOtherGroupItem policy24HSCOtherGroupItem, PolicyRouting policyRouting)
		//{
		//	string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
		//	int? policy24HSCOtherGroupItemId = new Int32();

		//	db.spDesktopDataAdmin_InsertPolicy24HSCOtherGroupItem_v1(
		//		ref policy24HSCOtherGroupItemId,
		//		policy24HSCOtherGroupItem.Policy24HSCStatusId,
		//		policy24HSCOtherGroupItem.EnabledFlag,
		//		policy24HSCOtherGroupItem.EnabledDate,
		//		policy24HSCOtherGroupItem.ExpiryDate,
		//		policy24HSCOtherGroupItem.TravelDateValidFrom,
		//		policy24HSCOtherGroupItem.TravelDateValidTo,
		//		policy24HSCOtherGroupItem.PolicyGroupId,
		//		policy24HSCOtherGroupItem.SupplierCode,
		//		policy24HSCOtherGroupItem.ProductId,
		//		policy24HSCOtherGroupItem.AirVendorRanking,
		//		policyRouting.Name,
		//		policyRouting.FromGlobalFlag,
		//		policyRouting.FromGlobalRegionCode,
		//		policyRouting.FromGlobalSubRegionCode,
		//		policyRouting.FromCountryCode,
		//		policyRouting.FromCityCode,
		//		policyRouting.FromTravelPortCode,
		//		policyRouting.FromTraverlPortTypeId,
		//		policyRouting.ToGlobalFlag,
		//		policyRouting.ToGlobalRegionCode,
		//		policyRouting.ToGlobalSubRegionCode,
		//		policyRouting.ToCountryCode,
		//		policyRouting.ToCityCode,
		//		policyRouting.ToTravelPortCode,
		//		policyRouting.ToTravelPortTypeId,
		//		policyRouting.RoutingViceVersaFlag,
		//		adminUserGuid
		//	);

		//	policy24HSCOtherGroupItem.Policy24HSCOtherGroupItemId = (int)policy24HSCOtherGroupItemId;
		//}

		////Edit
		//public void Update(Policy24HSCOtherGroupItem policy24HSCOtherGroupItem, PolicyRouting policyRouting)
		//{
		//	string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

		//	db.spDesktopDataAdmin_UpdatePolicy24HSCOtherGroupItem_v1(
		//		policy24HSCOtherGroupItem.Policy24HSCOtherGroupItemId,
		//		policy24HSCOtherGroupItem.Policy24HSCStatusId,
		//		policy24HSCOtherGroupItem.EnabledFlag,
		//		policy24HSCOtherGroupItem.EnabledDate,
		//		policy24HSCOtherGroupItem.ExpiryDate,
		//		policy24HSCOtherGroupItem.TravelDateValidFrom,
		//		policy24HSCOtherGroupItem.TravelDateValidTo,
		//		policy24HSCOtherGroupItem.PolicyGroupId,
		//		policy24HSCOtherGroupItem.SupplierCode,
		//		policy24HSCOtherGroupItem.ProductId,
		//		policy24HSCOtherGroupItem.AirVendorRanking,
		//		policyRouting.Name,
		//		policyRouting.FromGlobalFlag,
		//		policyRouting.FromGlobalRegionCode,
		//		policyRouting.FromGlobalSubRegionCode,
		//		policyRouting.FromCountryCode,
		//		policyRouting.FromCityCode,
		//		policyRouting.FromTravelPortCode,
		//		policyRouting.FromTraverlPortTypeId,
		//		policyRouting.ToGlobalFlag,
		//		policyRouting.ToGlobalRegionCode,
		//		policyRouting.ToGlobalSubRegionCode,
		//		policyRouting.ToCountryCode,
		//		policyRouting.ToCityCode,
		//		policyRouting.ToTravelPortCode,
		//		policyRouting.ToTravelPortTypeId,
		//		policyRouting.RoutingViceVersaFlag,
		//		adminUserGuid,
		//		policy24HSCOtherGroupItem.VersionNumber
		//	);

		//}

		////Delete
		//public void Delete(Policy24HSCOtherGroupItem policy24HSCOtherGroupItem)
		//{
		//	string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

		//	db.spDesktopDataAdmin_DeletePolicy24HSCOtherGroupItem_v1(
		//		policy24HSCOtherGroupItem.Policy24HSCOtherGroupItemId,
		//		adminUserGuid,
		//		policy24HSCOtherGroupItem.VersionNumber
		//		);
		//}

		////Get one Item
		//public Policy24HSCOtherGroupItem GetPolicy24HSCOtherGroupItem(int policy24HSCOtherGroupItemId)
		//{
		//	Policy24HSCOtherGroupItemDC db = new Policy24HSCOtherGroupItemDC(Settings.getConnectionString());
		//	return db.Policy24HSCOtherGroupItems.SingleOrDefault(c => c.Policy24HSCOtherGroupItemId == policy24HSCOtherGroupItemId);
		//}

		////Add Data From Linked Tables for Display
		//public void EditItemForDisplay(Policy24HSCOtherGroupItem policy24HSCOtherGroupItem)
		//{
		//	if (policy24HSCOtherGroupItem.Policy24HSCStatusId != null)
		//	{
		//		int policy24HSCStatusId = (int)policy24HSCOtherGroupItem.Policy24HSCStatusId;
		//		Policy24HSCStatusRepository policy24HSCStatusRepository = new Policy24HSCStatusRepository();
		//		Policy24HSCStatus policy24HSCStatus = new Policy24HSCStatus();
		//		policy24HSCStatus = policy24HSCStatusRepository.GetPolicy24HSCStatus(policy24HSCStatusId);
		//		policy24HSCOtherGroupItem.Policy24HSCStatus = policy24HSCStatus.Policy24HSCStatusDescription;
		//	}
		//	else
		//	{
		//		policy24HSCOtherGroupItem.Policy24HSCStatus = "None";
		//	}

		//	//populate new Policy24HSCOtherGroupItem with PolicyGroupName    
		//	if (policy24HSCOtherGroupItem.PolicyGroupId != 0)
		//	{
		//		PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
		//		PolicyGroup policyGroup = new PolicyGroup();
		//		policyGroup = policyGroupRepository.GetGroup(policy24HSCOtherGroupItem.PolicyGroupId);
		//		policy24HSCOtherGroupItem.PolicyGroupName = policyGroup.PolicyGroupName;
		//	}
		//	//Supplier
		//	SupplierRepository supplierRepository = new SupplierRepository();
		//	Supplier supplier = new Supplier();
		//	supplier = supplierRepository.GetSupplier(policy24HSCOtherGroupItem.SupplierCode, policy24HSCOtherGroupItem.ProductId);
		//	if (supplier != null)
		//	{
		//		policy24HSCOtherGroupItem.SupplierName = supplier.SupplierName;
		//	}

		//	//Product
		//	ProductRepository productRepository = new ProductRepository();
		//	Product product = new Product();
		//	product = productRepository.GetProduct(policy24HSCOtherGroupItem.ProductId);
		//	if (product != null)
		//	{
		//		policy24HSCOtherGroupItem.ProductName = product.ProductName;
		//	}

		//}
    
    }
}
