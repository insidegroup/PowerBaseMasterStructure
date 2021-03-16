using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
	public class OptionalFieldGroupVM : CWTBaseViewModel
   {
        public OptionalFieldGroup OptionalFieldGroup { get; set; }
		public IEnumerable<SelectListItem> HierarchyTypes { get; set; }
        
        public OptionalFieldGroupVM()
        {
        }

		public OptionalFieldGroupVM(OptionalFieldGroup optionalFieldGroup, IEnumerable<SelectListItem> hierarchyTypes)
        {
			OptionalFieldGroup = optionalFieldGroup;
            HierarchyTypes = hierarchyTypes;
        }
    }
}