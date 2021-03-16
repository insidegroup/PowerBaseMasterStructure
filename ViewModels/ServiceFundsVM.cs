using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.ViewModels
{
	public class ServiceFundsVM : CWTBaseViewModel
	{
		public CWTPaginatedList<spDesktopDataAdmin_SelectServiceFunds_v1Result> ServiceFunds { get; set; }
        public bool HasWriteAccess { get; set; }
		public bool CanEditOrder { get; set; }

        public ServiceFundsVM()
        {
            HasWriteAccess = false;
			CanEditOrder = false;
        }

		public ServiceFundsVM(
			GDSEndWarningConfiguration gdsEndWarningConfiguration,
			CWTPaginatedList<spDesktopDataAdmin_SelectServiceFunds_v1Result> serviceFunds,
			bool hasWriteAccess,
			bool canEditOrder)
        {
			ServiceFunds = serviceFunds;
			HasWriteAccess = hasWriteAccess;
			CanEditOrder = canEditOrder;
        }
    }
}