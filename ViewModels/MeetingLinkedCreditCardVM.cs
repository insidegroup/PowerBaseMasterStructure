using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
	public class MeetingLinkedCreditCardVM : CWTBaseViewModel
	{
		public int CreditCardId { get; set; }
		public string CreditCardHolderName { get; set; }
		public string MaskedCreditCardNumber { get; set; }

		public string ProductName { get; set; }
		public string SupplierName { get; set; }

		public string ClientTopUnitGuid { get; set; }
		public string ClientSubUnitGuid { get; set; }
		public string SourceSystemCode { get; set; }
		public string ClientAccountNumber { get; set; }
		public string TravelerTypeGuid { get; set; }

		public string HierarchyItem { get; set; }
		public string HierarchyType { get; set; }

		public bool HasWriteAccess { get; set; }

		public MeetingLinkedCreditCardVM()
		{
		}
	}
}