using System.Collections.Generic;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
	public class SupplierVM : CWTBaseViewModel
	{
		public Supplier Supplier { get; set; }

		public bool AllowDelete { get; set; }
		public List<SupplierReference> SupplierReferences { get; set; }

		public SupplierVM()
		{
		}

		public SupplierVM(Supplier supplier)
		{
			Supplier = supplier;
		}
	}
}