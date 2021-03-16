using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
    public class TransactionFeeProductSelectListVM : CWTBaseViewModel
    {
        public int ProductId { get; set; }
        public IEnumerable<SelectListItem> Products { get; set; }
        
        public TransactionFeeProductSelectListVM()
        {
        }
        public TransactionFeeProductSelectListVM(int productId, IEnumerable<SelectListItem> products)
        {
            ProductId = productId;
            Products = products;
        }
    }
}
