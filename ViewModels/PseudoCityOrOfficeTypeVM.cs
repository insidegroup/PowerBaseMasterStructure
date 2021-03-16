using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
	public class PseudoCityOrOfficeTypeVM : CWTBaseViewModel
    {
        public PseudoCityOrOfficeType PseudoCityOrOfficeType { get; set; }

		public bool AllowDelete { get; set; }
		public List<PseudoCityOrOfficeTypeReference> PseudoCityOrOfficeTypeReferences { get; set; }

		public PseudoCityOrOfficeTypeVM()
		{
		}

		public PseudoCityOrOfficeTypeVM(PseudoCityOrOfficeType pseudoCityOrOfficeLocationType, List<PseudoCityOrOfficeTypeReference> pseudoCityOrOfficeLocationTypeReferences)
		{
			PseudoCityOrOfficeType = pseudoCityOrOfficeLocationType;
			PseudoCityOrOfficeTypeReferences = pseudoCityOrOfficeLocationTypeReferences;
		}
    }	
}