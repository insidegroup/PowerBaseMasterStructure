using System.Collections.Generic;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
	public class CityVM : CWTBaseViewModel
	{
		public City City { get; set; }

		public bool AllowDelete { get; set; }
		public List<CityReference> CityReferences { get; set; }

		public CityVM()
		{
		}

		public CityVM(City city)
		{
			City = city;
		}
	}
}