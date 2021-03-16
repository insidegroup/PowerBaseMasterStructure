using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
	public class GDSOrderTypeVM : CWTBaseViewModel
    {
        public GDSOrderType GDSOrderType { get; set; }

		public bool AllowDelete { get; set; }
		public List<GDSOrderTypeReference> GDSOrderTypeReferences { get; set; }

		public GDSOrderTypeVM()
		{
		}

		public GDSOrderTypeVM(GDSOrderType gdsOrderType, List<GDSOrderTypeReference> gdsOrderTypeReferences)
		{
			GDSOrderType = gdsOrderType;
			GDSOrderTypeReferences = gdsOrderTypeReferences;
		}
	}
}