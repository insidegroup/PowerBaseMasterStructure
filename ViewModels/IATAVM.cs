using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
	public class IATAVM : CWTBaseViewModel
    {
        public IATA IATA { get; set; }
		public List<IATAReference> IATAReferences { get; set; }

		public IATAVM()
		{
		}

		public IATAVM(IATA iata, List<IATAReference> iataReferences)
		{
			IATA = iata;
			IATAReferences = iataReferences;
		}
	}
}