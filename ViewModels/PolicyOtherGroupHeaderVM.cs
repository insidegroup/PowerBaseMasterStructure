using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class PolicyOtherGroupHeaderVM : CWTBaseViewModel
	{
		public PolicyOtherGroupHeader PolicyOtherGroupHeader { get; set; }
		public IEnumerable<SelectListItem> PolicyOtherGroupHeaderServiceTypes { get; set; }
		public IEnumerable<SelectListItem> Products { get; set; }
		public IEnumerable<SelectListItem> SubProducts { get; set; }
		public IEnumerable<SelectListItem> Languages { get; set; }

		public PolicyOtherGroupHeaderVM()
		{

		}

		public PolicyOtherGroupHeaderVM(
			PolicyOtherGroupHeader policyOtherGroupHeader,
			IEnumerable<SelectListItem> policyOtherGroupHeaderServiceTypes,
			IEnumerable<SelectListItem> products,
			IEnumerable<SelectListItem> subProducts,
			IEnumerable<SelectListItem> languages)
		{
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			PolicyOtherGroupHeaderServiceTypes = policyOtherGroupHeaderServiceTypes;
			Products = products;
			SubProducts = subProducts;
			Languages = languages;
		}
	}
}