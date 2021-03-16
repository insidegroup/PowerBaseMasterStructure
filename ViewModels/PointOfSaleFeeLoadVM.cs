using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class PointOfSaleFeeLoadVM : CWTBaseViewModel
    {
        public PointOfSaleFeeLoad PointOfSaleFeeLoad { get; set; }

        public IEnumerable<SelectListItem> FeeLoadDescriptionTypeCodes { get; set; }
        public IEnumerable<SelectListItem> Products { get; set; }
        public IEnumerable<SelectListItem> TravelIndicators { get; set; }
        public IEnumerable<SelectListItem> Currencies { get; set; }

        public PointOfSaleFeeLoadVM()
        {
           
        }

        public PointOfSaleFeeLoadVM(
			PointOfSaleFeeLoad pointOfSaleFeeLoad
        )
        {
			PointOfSaleFeeLoad = pointOfSaleFeeLoad;
        }
    }
}