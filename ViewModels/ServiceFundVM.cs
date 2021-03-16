using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class ServiceFundVM : CWTBaseViewModel
	{
		public ServiceFund ServiceFund { get; set; }

		public IEnumerable<SelectListItem> GDSs { get; set; }
		public IEnumerable<SelectListItem> Countries { get; set; }
		public IEnumerable<SelectListItem> FundUseStatuses { get; set; }
		public IEnumerable<SelectListItem> TimeZoneRules { get; set; }
		public IEnumerable<SelectListItem> Products { get; set; }
		public IEnumerable<SelectListItem> Suppliers { get; set; }
		public IEnumerable<SelectListItem> Currencies { get; set; }
		public IEnumerable<SelectListItem> ServiceFundRoutings { get; set; }
		public IEnumerable<SelectListItem> ServiceFundChannelTypes { get; set; }

		public ServiceFundVM()
		{

		}

		public ServiceFundVM(
			ServiceFund serviceFund,
			IEnumerable<SelectListItem> serviceFundServiceTypes,
			IEnumerable<SelectListItem> products,
			IEnumerable<SelectListItem> subProducts,
			IEnumerable<SelectListItem> languages)
		{
			ServiceFund = serviceFund;
		}
	}
}