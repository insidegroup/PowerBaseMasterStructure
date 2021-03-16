using System.Collections.Generic;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
	public class OptionalFieldItemVM : CWTBaseViewModel
	{
		public OptionalFieldItem OptionalFieldItem { get; set; }
		public OptionalFieldGroup OptionalFieldGroup { get; set; }
		public IEnumerable<SelectListItem> OptionalFields { get; set; }
		public IEnumerable<SelectListItem> Products { get; set; }
		public IEnumerable<SelectListItem> Suppliers { get; set; }

        public OptionalFieldItemVM()
        {
        }

		public OptionalFieldItemVM(OptionalFieldItem optionalFieldItem, IEnumerable<SelectListItem> products, IEnumerable<SelectListItem> suppliers)
		{
			OptionalFieldItem = optionalFieldItem;
			Products = products;
			Suppliers = suppliers;
		}
	}
}