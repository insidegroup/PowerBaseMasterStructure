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
    public class FormOfPaymentTypeController : Controller
    {
        FormOfPaymentTypeRepository formOfPaymentTypeRepository = new FormOfPaymentTypeRepository();

        // POST: AutoComplete Suppliers
        [HttpPost]
        public JsonResult AutoCompleteFormOfPaymentTypes(string searchText, int maxResults)
        {
            var result = formOfPaymentTypeRepository.LookUpFormOfPaymentTypes(searchText, maxResults);
            return Json(result);
        }

        [HttpPost]
        public JsonResult IsValidFormOfPaymentType(string searchText)
        {

            var result = formOfPaymentTypeRepository.GetFormOfPaymentTypeByName(searchText);
            return Json(result);
        }

    }
}
