using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ProductGroupVM : CWTBaseViewModel
    {
        public ProductGroup ProductGroup { get; set; }
        public IEnumerable<SelectListItem> HierarchyTypes { get; set; }

        public ProductGroupVM()
        {
        }
        public ProductGroupVM(ProductGroup productGroup, IEnumerable<SelectListItem> hierarchyTypes)
        {
            ProductGroup = productGroup;
            HierarchyTypes = hierarchyTypes;
        }
    }
}