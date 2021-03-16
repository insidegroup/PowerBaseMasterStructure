using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
	public class PriceTrackingHandlingFeeItemVM : CWTBaseViewModel
	{
		public PriceTrackingHandlingFeeItem PriceTrackingHandlingFeeItem { get; set; }
		public ApprovalGroup ApprovalGroup { get; set; }

		public PriceTrackingHandlingFeeItemVM()
		{
		}

		public PriceTrackingHandlingFeeItemVM(PriceTrackingHandlingFeeItem priceTrackingHandlingFeeItem, ApprovalGroup approvalGroup)
		{
			PriceTrackingHandlingFeeItem = priceTrackingHandlingFeeItem;
			ApprovalGroup = approvalGroup;
		}
	}
}