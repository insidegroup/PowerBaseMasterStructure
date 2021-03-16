using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class PolicyOtherGroupHeaderSequenceVM : CWTBaseViewModel
	{
		[Required(ErrorMessage = "You must choose a Service Type")]
		public int PolicyOtherGroupHeaderServiceTypeId { get; set; }

		[Required(ErrorMessage = "You must choose a ProductId")]
		public int ProductId { get; set; }

		public int SubProductId { get; set; }
		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupHeaderSequences_v1Result> PolicyOtherGroupHeaderSequences { get; set; }

		public IEnumerable<SelectListItem> PolicyOtherGroupHeaderServiceTypes { get; set; }
		public IEnumerable<SelectListItem> Products { get; set; }
		public IEnumerable<SelectListItem> SubProducts { get; set; }

		public PolicyOtherGroupHeaderServiceType PolicyOtherGroupHeaderServiceType { get; set; }
		public Product Product { get; set; }
		public SubProduct SubProduct { get; set; }

        public PolicyOtherGroupHeaderSequenceVM()
        {
          
        }

		public PolicyOtherGroupHeaderSequenceVM(
			IEnumerable<SelectListItem> policyOtherGroupHeaderServiceTypes,
			IEnumerable<SelectListItem> products,
			IEnumerable<SelectListItem> subProducts)
        {
			PolicyOtherGroupHeaderServiceTypes = policyOtherGroupHeaderServiceTypes;
			Products = products;
			SubProducts = subProducts;
        }
    }
}