using CWTDesktopDatabase.Models;
using System.Web.Mvc;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class ClientDefinedReferenceItemValueVM : CWTBaseViewModel
	{
		public ClientDefinedReferenceItemValue ClientDefinedReferenceItemValue { get; set; }
		public ClientDefinedReferenceItem ClientDefinedReferenceItem { get; set; }
		public ClientSubUnit ClientSubUnit { get; set; }
		public ClientSubUnitClientAccount ClientSubUnitClientAccount { get; set; }

		public ClientDefinedReferenceItemValueVM()
		{
		}

		public ClientDefinedReferenceItemValueVM(
			ClientDefinedReferenceItemValue clientDefinedReferenceItemValue,
			ClientDefinedReferenceItem clientDefinedReferenceItem, 
			ClientSubUnit clientSubUnit,
			ClientSubUnitClientAccount clientSubUnitClientAccount)
		{
			ClientDefinedReferenceItemValue = clientDefinedReferenceItemValue;
			ClientDefinedReferenceItem = clientDefinedReferenceItem; 
			ClientSubUnit = clientSubUnit;
			ClientSubUnitClientAccount = clientSubUnitClientAccount;
		}
	}
}