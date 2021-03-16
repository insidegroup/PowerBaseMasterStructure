using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.ViewModels
{
	public class ClientSubUnitCDRItemsVM : CWTBaseViewModel
    {
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitCDRItems_v1Result> ClientSubUnitCDRItems { get; set; }
		public bool HasWriteAccess { get; set; }
	
		public ClientSubUnit ClientSubUnit { get; set; }
		public ClientSubUnitClientDefinedReference ClientSubUnitClientDefinedReference { get; set; }

        public ClientSubUnitCDRItemsVM()
        {
            HasWriteAccess = false;
        }

        public ClientSubUnitCDRItemsVM(
			CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitCDRItems_v1Result> clientSubUnitCDRItems,
			bool hasWriteAccess)
        {
			ClientSubUnitCDRItems = clientSubUnitCDRItems;
			HasWriteAccess = hasWriteAccess;
        }
    }
}