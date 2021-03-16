using CWTDesktopDatabase.Models;
using System.Web.Mvc;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.ViewModels
{
   
    public class ClientSubUnitCDRItemVM : CWTBaseViewModel
    {
		public ClientSubUnitClientDefinedReferenceItem ClientSubUnitClientDefinedReferenceItem { get; set; }
		public ClientSubUnit ClientSubUnit { get; set; }

		public ClientSubUnitCDRItemVM()
        {
        }

		public ClientSubUnitCDRItemVM(ClientSubUnitClientDefinedReferenceItem clientSubUnitClientDefinedReferenceItem)
        {
			ClientSubUnitClientDefinedReferenceItem = clientSubUnitClientDefinedReferenceItem;
        }
    }
}