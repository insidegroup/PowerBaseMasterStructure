using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Repository
{
    public class ProductRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //get all Products
        public IQueryable<Product> GetAllProducts()
        {
            return db.Products.OrderBy(c => c.ProductName);
        }

		//Variation of GetAllProducts that gets 3 products for transactionfees
		public IQueryable<Product> GetTransactionFeeProducts()
		{
			return db.Products.OrderBy(c => c.ProductName).Where(c => (c.ProductName.Contains("Air") || c.ProductName.Contains("Car") || c.ProductName.Contains("Hotel")));
		}

		//Variation of GetAllProducts that gets 3 products for Commissionable Route Items
		public IQueryable<Product> GetCommissionableRouteItemProducts()
		{
			return db.Products.OrderBy(c => c.ProductName).Where(c => (c.ProductName.Contains("Air") || c.ProductName.Contains("Car") || c.ProductName.Contains("Hotel")));
		}

		//Variation of GetAllProducts that gets products for Policy Other Group Headers
		public IQueryable<Product> GetPolicyOtherGroupHeaderProducts()
		{
			return db.Products.OrderBy(c => c.ProductName).Where(c => (
				c.ProductName.Contains("Air") || 
				c.ProductName.Contains("Car") || 
				c.ProductName.Contains("Hotel") || 
				c.ProductName.Contains("Other") ||
				c.ProductName.Contains("All")
			));
		}

        //get a single product
        public Product GetProduct(int productId)
        {
            return db.Products.SingleOrDefault(c => c.ProductId == productId);
        }

		//get PriceTrackerEligibleProducts all Products
		public IQueryable<Product> GetPriceTrackerEligibleProducts()
        {
            return db.Products.OrderBy(c => c.ProductName).Where(x => x.PriceTrackerEligibleFlag == true);
        }

        //Variation of GetAllProducts that gets products for PassiveSegments
        /*public List<SelectListItem> GetPassiveSegmentProducts()
        {
            IQueryable<SelectListItem> products = (
                from p in db.Products
                where p.PassiveSegmentEligibleFlag == true orderby p.ProductName
                select new SelectListItem{Text = p.ProductName,Value = p.ProductId.ToString()}
            );


            return products.ToList();
        }*/

        //Variation of GetAllProducts that gets products for PassiveSegments
        /*public List<SelectListItem> GetPassiveSegmentProducts(int? productGroupId)
        {
            IQueryable<SelectListItem> products = (
                from p in db.Products
                where p.PassiveSegmentEligibleFlag == true
                orderby p.ProductName
                select new SelectListItem { Text = p.ProductName, Value = p.ProductId.ToString() }
            );
            if (productGroupId != null)
            {
                List<spDesktopDataAdmin_SelectProductGroupProducts_v1Result> productGroupProducts = new List<spDesktopDataAdmin_SelectProductGroupProducts_v1Result>();
                productGroupProducts = GetProducts((int)productGroupId);

                foreach (SelectListItem product in products)
                {
                    var data = productGroupProducts.Where(p => p.ProductId.ToString() == product.Value);
                    if (data != null && data.Any())
                    {
                        product.Selected = true;      
                    }
                }
              

            }

            return products.ToList();
        }*/

        public List<SelectListItem> GetPassiveSegmentProducts(int? productGroupId)
        {
            IEnumerable<SelectListItem> products = (
            from p in db.spDesktopDataAdmin_SelectProductGroupProducts_v1(productGroupId)
            select new SelectListItem { Text = p.ProductName, Value = p.ProductId.ToString(), Selected = Convert.ToBoolean(p.Selected) });
            return products.ToList();
        }
    }
}
