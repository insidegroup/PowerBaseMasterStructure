using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
	public class PartnerVM : CWTBaseViewModel
    {
		public Partner Partner { get; set; }

		public IEnumerable<SelectListItem> Countries { get; set; }

		public bool AllowDelete { get; set; }
		public List<PartnerReference> PartnerReferences { get; set; }

		public PartnerVM()
		{
		}

		public PartnerVM(Partner partner, List<PartnerReference> partnerReferences)
		{
			Partner = partner;
			PartnerReferences = partnerReferences;
		}
    }	
}