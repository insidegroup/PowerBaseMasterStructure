using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;


namespace CWTDesktopDatabase.Controllers
{
    public class TravelPortTypeController : Controller
    {
        //Update Select List
        [HttpPost]
        public JsonResult GetLanguages(int travelPortTypeId, string travelPortCode)
        {
            TravelPortTypeRepository travelPortTypeRepository = new TravelPortTypeRepository();
            var result = travelPortTypeRepository.LookUpAvailableLanguages(travelPortTypeId, travelPortCode);
            return Json(result);
        }

    }
}
