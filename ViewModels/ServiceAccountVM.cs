using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
	public class ServiceAccountVM : CWTBaseViewModel
    {
        public ServiceAccount ServiceAccount { get; set; }

		public ServiceAccountVM()
		{
		}

		public ServiceAccountVM(ServiceAccount serviceAccount)
		{
            ServiceAccount = serviceAccount;
		}
	}
}