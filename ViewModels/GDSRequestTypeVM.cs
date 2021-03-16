using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
	public class GDSRequestTypeVM : CWTBaseViewModel
    {
        public GDSRequestType GDSRequestType { get; set; }

		public bool AllowDelete { get; set; }
		public List<GDSRequestTypeReference> GDSRequestTypeReferences { get; set; }

		public GDSRequestTypeVM()
		{
		}

		public GDSRequestTypeVM(GDSRequestType gdsRequestType, List<GDSRequestTypeReference> gdsRequestTypeReferences)
		{
			GDSRequestType = gdsRequestType;
			GDSRequestTypeReferences = gdsRequestTypeReferences;
		}
    }	
}