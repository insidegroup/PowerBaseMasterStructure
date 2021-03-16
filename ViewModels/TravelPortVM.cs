using System.Collections.Generic;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
	public class TravelPortVM : CWTBaseViewModel
	{
		public TravelPort TravelPort { get; set; }

		public bool AllowDelete { get; set; }
		public List<TravelPortReference> TravelPortReferences { get; set; }

		public TravelPortVM()
		{
		}

		public TravelPortVM(TravelPort travelPort)
		{
			TravelPort = travelPort;
		}
	}
}