using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientProfileAdminGroupVM : CWTBaseViewModel
    {
        public ClientProfileAdminGroup ClientProfileAdminGroup { get; set; }
        public IEnumerable<SelectListItem> HierarchyTypes { get; set; }
		public IEnumerable<SelectListItem> GDSs { get; set; }
		public IEnumerable<SelectListItem> BackOffices { get; set; }

        public ClientProfileAdminGroupVM()
        {
        }
        public ClientProfileAdminGroupVM(ClientProfileAdminGroup clientProfileAdminGroup, 
									IEnumerable<SelectListItem> hierarchyTypes,
									IEnumerable<SelectListItem> gDSs,
									IEnumerable<SelectListItem> backOffices)
        {
			ClientProfileAdminGroup = clientProfileAdminGroup;
            HierarchyTypes = hierarchyTypes;
			GDSs = gDSs;
			BackOffices = backOffices;
        }
    }
}