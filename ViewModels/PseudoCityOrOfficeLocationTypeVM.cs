using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
	public class PseudoCityOrOfficeLocationTypeVM : CWTBaseViewModel
    {
        public PseudoCityOrOfficeLocationType PseudoCityOrOfficeLocationType { get; set; }

		public bool AllowDelete { get; set; }
		public List<PseudoCityOrOfficeLocationTypeReference> PseudoCityOrOfficeLocationTypeReferences { get; set; }

		public PseudoCityOrOfficeLocationTypeVM()
		{
		}

		public PseudoCityOrOfficeLocationTypeVM(PseudoCityOrOfficeLocationType pseudoCityOrOfficeLocationType, List<PseudoCityOrOfficeLocationTypeReference> pseudoCityOrOfficeLocationTypeReferences)
		{
			PseudoCityOrOfficeLocationType = pseudoCityOrOfficeLocationType;
			PseudoCityOrOfficeLocationTypeReferences = pseudoCityOrOfficeLocationTypeReferences;
		}
    }	
}