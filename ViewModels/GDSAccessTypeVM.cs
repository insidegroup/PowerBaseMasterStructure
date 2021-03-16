using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
	public class GDSAccessTypeVM : CWTBaseViewModel
    {
        public GDSAccessType GDSAccessType { get; set; }
		public IEnumerable<SelectListItem> GDSs { get; set; }

		public bool AllowDelete { get; set; }
		public List<GDSAccessTypeReference> GDSAccessTypeReferences { get; set; }

		public GDSAccessTypeVM()
		{
		}

		public GDSAccessTypeVM(GDSAccessType gdsAccessType, List<GDSAccessTypeReference> gdsAccessTypeReferences)
		{
			GDSAccessType = gdsAccessType;
			GDSAccessTypeReferences = gdsAccessTypeReferences;
		}
	}
}