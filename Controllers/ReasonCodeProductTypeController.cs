using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;

namespace CWTDesktopDatabase.Controllers
{
    public class ReasonCodeProductTypeController : Controller
    {
        ReasonCodeProductTypeRepository reasonCodeProductTypeRepository = new ReasonCodeProductTypeRepository();

        // GET: /List
        public ActionResult List(int? page, string filter, string sortField, int? sortOrder)
        {
            //no Editing Allowed
            ViewData["Access"] = "";

            //SortField+SortOrder settings
            if (sortField != "ReasonCode" && sortField != "ReasonCodeTypeDescription")
            {
                sortField = "ProductName";
            }
            ViewData["CurrentSortField"] = sortField;

            if (sortOrder == 1)
            {
                ViewData["NewSortOrder"] = 0;
                ViewData["CurrentSortOrder"] = 1;
            }
            else
            {
                ViewData["NewSortOrder"] = 1;
                ViewData["CurrentSortOrder"] = 0;
                sortOrder = 0;
            }

            //return items
            var cwtPaginatedList = reasonCodeProductTypeRepository.PageReasonCodeProductTypes(page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

    }
}
