using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
	public class FareRedistributionVM : CWTBaseViewModel
    {
        public FareRedistribution FareRedistribution { get; set; }
		public IEnumerable<SelectListItem> GDSs { get; set; }

		public bool AllowDelete { get; set; }
		public List<FareRedistributionReference> FareRedistributionReferences { get; set; }

		public FareRedistributionVM()
		{
		}

		public FareRedistributionVM(FareRedistribution fareRedistribution, List<FareRedistributionReference> fareRedistributionReferences)
		{
			FareRedistribution = fareRedistribution;
			FareRedistributionReferences = fareRedistributionReferences;
		}
	}
}