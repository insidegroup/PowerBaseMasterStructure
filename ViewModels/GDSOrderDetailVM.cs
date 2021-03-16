using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
	public class GDSOrderDetailVM : CWTBaseViewModel
    {
        public GDSOrderDetail GDSOrderDetail { get; set; }

		public bool AllowDelete { get; set; }
		public List<GDSOrderDetailReference> GDSOrderDetailReferences { get; set; }

		public GDSOrderDetailVM()
		{
		}

		public GDSOrderDetailVM(GDSOrderDetail gdsOrderDetail, List<GDSOrderDetailReference> gdsOrderDetailReferences)
		{
			GDSOrderDetail = gdsOrderDetail;
			GDSOrderDetailReferences = gdsOrderDetailReferences;
		}
	}
}