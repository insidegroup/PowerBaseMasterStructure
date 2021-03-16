using CWTDesktopDatabase.Models;
using System.Web.Mvc;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class ClientDefinedReferenceItemPNROutputVM : CWTBaseViewModel
	{
		public ClientDefinedReferenceItemPNROutput ClientDefinedReferenceItemPNROutput { get; set; }
		public ClientSubUnit ClientSubUnit { get; set; }

		public IEnumerable<SelectListItem> GDSList { get; set; }
		public IEnumerable<SelectListItem> PNROutputRemarkTypeCodes { get; set; }
		public IEnumerable<SelectListItem> Languages { get; set; }

		public ClientDefinedReferenceItemPNROutputVM()
		{
		}

		public ClientDefinedReferenceItemPNROutputVM(
			ClientDefinedReferenceItemPNROutput clientDefinedReferenceItemPNROutput, 
			ClientDefinedReferenceItem clientDefinedReferenceItem)
		{
			ClientDefinedReferenceItemPNROutput = clientDefinedReferenceItemPNROutput;
		}
	}
}