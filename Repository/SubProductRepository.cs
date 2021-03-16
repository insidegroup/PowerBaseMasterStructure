using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;


namespace CWTDesktopDatabase.Repository
{
    public class SubProductRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
  
        public SubProduct GetSubProduct(int subProductId)
        {
            return db.SubProducts.SingleOrDefault(c => c.SubProductId == subProductId);
        }

        //get the parent Product of a SubProduct
        public Product GetSubProductProduct(int subProductId)
        {
            HierarchyDC db2 = new HierarchyDC(Settings.getConnectionString());
            Product product = new Product();
            product.ProductId = db.SubProducts.First(a => a.SubProductId == subProductId).ProductId;
            product.ProductName = db2.Products.First(a => a.ProductId == product.ProductId).ProductName;

            return product;
        }

        public IQueryable<SubProduct> GetAllSubProducts()
        {
            return db.SubProducts;
        }
      
        internal List<SubProduct> LookUpSubProducts(string searchText, int maxResults)
        {
            var result = from n in GetAllSubProducts() where n.SubProductName.Contains(searchText) orderby n.SubProductName select n;
            return result.Take(maxResults).ToList();

        }

        internal List<SubProduct> ListProductSubProducts(int productId)
        {
            var result = from n in db.SubProducts where n.ProductId == productId orderby n.SubProductName select n;
            return result.ToList();

        }

        public List<SubProductJSON> GetSubProductByName(string subProductName)
        {
            var result = from n in db.SubProducts
                         where n.SubProductName.Trim().Equals(subProductName)
                         select
                             new SubProductJSON
                             {
                                 SubProductId = n.SubProductId,
                                 SubProductName = n.SubProductName.ToString()
                             };
            return result.ToList();
        }

		//Variation of GetAllSubProducts that gets products for Policy Other Group Headers
		public IQueryable<SubProduct> GetPolicyOtherGroupHeaderSubProducts()
		{
			return db.SubProducts.OrderBy(c => c.SubProductName).Where(c => (
				c.SubProductName.Equals("Limo") ||
				c.SubProductName.Equals("Taxi") ||
				c.SubProductName.Equals("Surface") ||
				c.SubProductName.Equals("Rail") ||
				c.SubProductName.Equals("Bus") ||
				c.SubProductName.Equals("Ferry") ||
				c.SubProductName.Equals("Sea") ||
				c.SubProductName.Equals("Insurance")
			));
		}
    }
}
