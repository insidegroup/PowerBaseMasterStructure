using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientProfileGroupVM : CWTBaseViewModel
    {
        public ClientProfileGroup ClientProfileGroup { get; set; }
        public IEnumerable<SelectListItem> HierarchyTypes { get; set; }
		public IEnumerable<SelectListItem> GDSs { get; set; }
		public IEnumerable<SelectListItem> BackOffices { get; set; }
		public string SabreFormat { get; set; }

        public ClientProfileGroupVM()
        {
        }
        public ClientProfileGroupVM(ClientProfileGroup clientProfileGroup, 
									IEnumerable<SelectListItem> hierarchyTypes,
									IEnumerable<SelectListItem> gDSs,
									IEnumerable<SelectListItem> backOffices)
        {
			ClientProfileGroup = clientProfileGroup;
            HierarchyTypes = hierarchyTypes;
			GDSs = gDSs;
			BackOffices = backOffices;
        }
    }
}