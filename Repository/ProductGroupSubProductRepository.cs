using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Repository
{
    public class ProductGroupSubProductRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //get SubProducts of a ProdcutGroup
        public List<SubProduct> GetProductGroupSubProductsXXX(int? productGroupId)
        {
            var subProducts = (
                from p in db.spDesktopDataAdmin_SelectProductGroupSubProducts_v1(productGroupId)
                select new SubProduct { SubProductId = p.SubProductId, SubProductName = p.SubProductName }
            );
            return subProducts.ToList();
        }

        //get SubProducts not in a ProductGroup
        public List<SubProduct> GetProductGroupAvailableSubProductsXXX(int? productGroupId)
        {
            var subProducts = (
                from p in db.spDesktopDataAdmin_SelectProductGroupAvailableSubProducts_v1(productGroupId)
                select new SubProduct { SubProductId = p.SubProductId, SubProductName = p.SubProductName }
            );
            return subProducts.ToList();
        }

        public List<SelectListItem> GetProductGroupSubProducts(int? productGroupId)
        {
            IEnumerable<SelectListItem> subProducts = (
            from p in db.spDesktopDataAdmin_SelectProductGroupSubProducts_v1(productGroupId)
            select new SelectListItem { Text = p.SubProductName, Value = p.SubProductId.ToString(), Selected = false });
            return subProducts.ToList();
        }

        public List<SelectListItem> GetProductGroupAvailableSubProducts(int? productGroupId)
        {
            IEnumerable<SelectListItem> subProducts = (
            from p in db.spDesktopDataAdmin_SelectProductGroupAvailableSubProducts_v1(productGroupId)
            select new SelectListItem { Text = p.SubProductName, Value = p.SubProductId.ToString(), Selected = false });
            return subProducts.ToList();
        }

    }
}