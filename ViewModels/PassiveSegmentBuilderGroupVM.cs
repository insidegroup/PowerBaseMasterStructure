using CWTDesktopDatabase.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
    public class PassiveSegmentBuilderGroupVM : CWTBaseViewModel
   {
        public ProductGroup ProductGroup { get; set; }
        public IEnumerable<SelectListItem> HierarchyTypes { get; set; }
        public List<SelectListItem> Products { get; set; }
        public List<SelectListItem> SubProducts { get; set; }
        public List<SelectListItem> AvailableSubProducts { get; set; }

        public PassiveSegmentBuilderGroupVM()
        {
        }
        public PassiveSegmentBuilderGroupVM(List<SelectListItem> products, List<SelectListItem> subProducts, List<SelectListItem> availableSubProducts, ProductGroup productGroup, IEnumerable<SelectListItem> hierarchyTypes)
        {
            ProductGroup = productGroup;
            HierarchyTypes = hierarchyTypes;
            Products = products;
            SubProducts = subProducts;
            AvailableSubProducts = availableSubProducts;
        }
    }
}