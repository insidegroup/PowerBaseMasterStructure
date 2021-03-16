using System.Web.Mvc;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;

namespace CWTDesktopDatabase.Controllers
{
    [AjaxTimeOutCheck]
    public class StateProvinceController : Controller
    {
        StateProvinceRepository stateProvinceRepository = new StateProvinceRepository();

        [HttpPost]
        public JsonResult IsValidStateProvince(string searchText, string countryCode)
        {
            var result = stateProvinceRepository.GetStateProvinceByName(searchText, countryCode);
            return Json(result);
        }

    }
}
