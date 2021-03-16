using CWTDesktopDatabase.Models;
using System.Web.Mvc;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class ClientDefinedReferenceItemVM : CWTBaseViewModel
	{
		public ClientSubUnit ClientSubUnit { get; set; }
		public ClientDefinedReferenceItem ClientDefinedReferenceItem { get; set; }

		public List<string> ProductList { get; set; }
		public IEnumerable<SelectListItem> Products { get; set; }

		public List<string> ContextList { get; set; }
		public IEnumerable<SelectListItem> Contexts { get; set; }

		public ClientDefinedReferenceItemVM()
		{
		}

		public ClientDefinedReferenceItemVM(ClientDefinedReferenceItem clientDefinedReferenceItem, ClientSubUnit clientSubUnit)
		{
			ClientSubUnit = clientSubUnit;
			ClientDefinedReferenceItem = clientDefinedReferenceItem;
		}
	}
}