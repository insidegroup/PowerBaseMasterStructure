using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;

namespace CWTDesktopDatabase.Controllers
{
    [AjaxTimeOutCheck]
    public class ServicingOptionItemValueController : Controller
    {
		// POST:  Get ServicingOption GDSRequiredFlag
		[HttpPost]
		public JsonResult GetServicingOptionGDSRequired(int servicingOptionId)
		{
			ServicingOptionRepository servicingOptionRepository = new ServicingOptionRepository();
			ServicingOption servicingOption = new ServicingOption();
			servicingOption = servicingOptionRepository.GetServicingOption(servicingOptionId);

			var result = (servicingOption != null && servicingOption.GDSRequiredFlag == true) ? true : false;

			return Json(result);
		}

		// POST:  Get ServicingOption Parameters Required
		[HttpPost]
		public JsonResult GetServicingOptionParametersRequired(int servicingOptionId)
		{
			ServicingOptionRepository servicingOptionRepository = new ServicingOptionRepository();
			ServicingOption servicingOption = new ServicingOption();
			servicingOption = servicingOptionRepository.GetServicingOption(servicingOptionId);

			var result = (servicingOption != null && (
														servicingOption.ServicingOptionId == 1 ||
														servicingOption.ServicingOptionId == 2 ||
														servicingOption.ServicingOptionId == 90 ||
														servicingOption.ServicingOptionId == 291)
													) ? true : false;

			return Json(result);
		}
        
		// POST:  CountryRegions of a Values for ServicingOptionItem
        [HttpPost]
        public JsonResult GetServicingOptionItemValues(int servicingOptionId, int id)
        {
            ServicingOptionItemValueRepository servicingOptionItemValueRepository = new ServicingOptionItemValueRepository();

            var result = servicingOptionItemValueRepository.GetServicingOptionServicingOptionItemValues(servicingOptionId, id);
            return Json(result);
        }

         // POST:  CountryRegions of a Values for ServicingOptionItem
        [HttpPost]
        public JsonResult GetClientSubUnitServicingOptionItemValues(int servicingOptionId, string id)
        {
            ServicingOptionItemValueRepository servicingOptionItemValueRepository = new ServicingOptionItemValueRepository();

            var result = servicingOptionItemValueRepository.GetClientSubUnitServicingOptionServicingOptionItemValues(servicingOptionId, id);
            return Json(result);
        }
    }
}
