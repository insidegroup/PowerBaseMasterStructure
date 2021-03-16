using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class SupplierRepository
    {
        private SupplierDC db = new SupplierDC(Settings.getConnectionString());

		//Get a Page of Suppliers
		public CWTPaginatedList<spDesktopDataAdmin_SelectSuppliers_v1Result> PageSuppliers(int page, string filter, string sortField, int sortOrder)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectSuppliers_v1(filter, sortField, sortOrder, page, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectSuppliers_v1Result>(result, page, Convert.ToInt32(totalRecords));
			return paginatedView;
		}

		public Supplier EditItemForDisplay(Supplier supplier)
		{
			//Product Name
			if (supplier.ProductId > 0)
			{
				ProductRepository productRepository = new ProductRepository();
				Product product = productRepository.GetProduct(supplier.ProductId);
				if(product != null) {
					supplier.ProductName = product.ProductName;
				}
			}
			
			return supplier;
		}

		//Add to DB
		public void AddSupplier(Supplier supplier)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertSupplier_v1(
				supplier.SupplierCode,
				supplier.SupplierName,
				supplier.ProductId,
				adminUserGuid
			);
		}

		//Update in DB
		public void UpdateSupplier(Supplier supplier)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateSupplier_v1(
				supplier.SupplierCode,
				supplier.SupplierName,
				supplier.ProductId,
				supplier.VersionNumber,
				adminUserGuid
			);
		}

		//Delete in DB
		public void DeleteSupplier(Supplier supplier)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteSupplier_v1(
				supplier.SupplierCode,
				supplier.ProductId, 
				adminUserGuid,
				supplier.VersionNumber
			);
		}

		public IQueryable<fnDesktopDataAdmin_SelectSupplierProductsResult> GetSupplierProducts(string supplierCode)
        {
            var result = db.fnDesktopDataAdmin_SelectSupplierProducts(supplierCode);
            return result;
        }

		public List<SupplierReference> GetSupplierReferences(string supplierCode, int productId)
        {
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = from n in db.spDesktopDataAdmin_SelectSupplierReferences_v1(supplierCode, productId, adminUserGuid)
						 select
							 new SupplierReference
							 {
								 TableName = n.TableName.ToString()
							 };

			return result.ToList();
        }

        public void Add(Supplier supplier)
        {
            db.Suppliers.InsertOnSubmit(supplier);
        }

        public void Delete(Supplier supplier)
        {
            db.Suppliers.DeleteOnSubmit(supplier);
        }

        public Supplier GetSupplier(string supplierCode, int productId)
        {
            return db.Suppliers.SingleOrDefault(c => (c.SupplierCode == supplierCode) && (c.ProductId == productId));
        }

        public IQueryable<Supplier> GetAllSuppliers()
        {
            return db.Suppliers.OrderBy(c => c.SupplierName);
        }
        public void Save()
        {
            db.SubmitChanges();
        }

        internal List<Supplier> LookUpSuppliers(string searchText, int maxResults)
        {
            var result = from n in GetAllSuppliers() where n.SupplierName.StartsWith(searchText) orderby n.SupplierName select n;
            return result.Take(maxResults).ToList();

        }
        internal List<Supplier> LookUpProductSuppliers(string searchText, int productId)
        {
            var result = from n in db.Suppliers where n.SupplierName.StartsWith(searchText) && n.ProductId == productId orderby n.SupplierName select n;

            return result.Take(10).ToList();

        }


        internal List<fnDesktopDataAdmin_SelectSupplierProductsResult> LookUpSupplierProducts(string searchText, string supplierCode, int maxResults)
        {
            var result = from n in GetSupplierProducts(supplierCode) where n.ProductName.StartsWith(searchText) orderby n.ProductName select n;
            return result.Take(maxResults).ToList();

        }

        public List<SupplierJSON> GetSupplierByName(string supplierName)
        {
            var result = from n in db.Suppliers
                         where n.SupplierName.Trim().Equals(supplierName)
                         select
                             new SupplierJSON
                             {
                                 SupplierCode = n.SupplierCode.Trim(),
                                 SupplierName = n.SupplierName.ToString()
                             };
            return result.ToList();
        }
    }
}

