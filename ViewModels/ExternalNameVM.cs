using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
	public class ExternalNameVM : CWTBaseViewModel
    {
        public ExternalName ExternalName { get; set; }

		public bool AllowDelete { get; set; }
		public List<ExternalNameReference> ExternalNameReferences { get; set; }

		public ExternalNameVM()
		{
		}

		public ExternalNameVM(ExternalName externalName, List<ExternalNameReference> externalNameReferences)
		{
			ExternalName = externalName;
			ExternalNameReferences = externalNameReferences;
		}
    }	
}