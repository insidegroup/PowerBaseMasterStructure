using CWTDesktopDatabase.Models;
using System.Web.Mvc;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class ClientDefinedReferenceItemPNROutputLanguageVM : CWTBaseViewModel
	{
		public ClientDefinedReferenceItemPNROutputLanguage ClientDefinedReferenceItemPNROutputLanguage { get; set; }
		public ClientDefinedReferenceItemPNROutput ClientDefinedReferenceItemPNROutput { get; set; }

		public ClientSubUnit ClientSubUnit { get; set; }
		public IEnumerable<SelectListItem> Languages { get; set; }

		public ClientDefinedReferenceItemPNROutputLanguageVM()
		{
		}

		public ClientDefinedReferenceItemPNROutputLanguageVM(
			ClientDefinedReferenceItemPNROutputLanguage clientDefinedReferenceItemPNROutputLanguage,
			ClientDefinedReferenceItemPNROutput clientDefinedReferenceItemPNROutput)
		{
			ClientDefinedReferenceItemPNROutputLanguage = ClientDefinedReferenceItemPNROutputLanguage;
			ClientDefinedReferenceItemPNROutput = clientDefinedReferenceItemPNROutput;
		}
	}
}