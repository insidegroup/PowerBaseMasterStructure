using System.Collections.Generic;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
	public class GSTIdentificationNumberVM : CWTBaseViewModel
	{
		public GSTIdentificationNumber GSTIdentificationNumber { get; set; }

		public GSTIdentificationNumberVM()
		{
		}

		public GSTIdentificationNumberVM(GSTIdentificationNumber gstIdentificationNumber)
		{
            GSTIdentificationNumber = gstIdentificationNumber;
		}
	}
}