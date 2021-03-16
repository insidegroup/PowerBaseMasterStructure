using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class ClientSummaryController : Controller
    {
        //main repository
        ClientSummaryRepository clientSummaryRepository = new ClientSummaryRepository();
        ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
        HierarchyRepository hierarchyRepository = new HierarchyRepository();
        //private string groupName = "Client Detail";

        // GET: /SearchForm
        public ActionResult SearchForm()
        {
            return View();
        }

        // post: /SearchForm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchForm(string filter, int? page)
        {
            if (filter == "")
            {
                return RedirectToAction("SearchForm");
            }
            return RedirectToAction("SearchResults", new { filter = filter, page = page });
            
        }

        // GET: /SearchForm
        public ActionResult SearchResults(string filter, int? page)
        {
            if (page < 0)
            {
                page = 1;
            }

            var items = clientSummaryRepository.GetClientSummaryTopUnits(page ?? 1, filter ?? "");
            return View(items);
        }

        //Shows TopUnit.SubUnits and their associated data
        public ActionResult ClientSubUnitsMatrix(string id, int? page, string filter)
        {
            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(id);
            if (clientTopUnit == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            ViewData["ClientTopUnitGuid"] = id;
            ViewData["ClientTopUnitName"] = clientTopUnit.ClientTopUnitName;

            if (page < 1)
            {
                page = 1;
            }

            ViewData["filter"] = filter;
            var items = clientSummaryRepository.GetClientSummaryClientSubUnitMatrix(id, page ?? 1);
            return View(items);
        }

        //Shows SubUnit.ClientAccounts and their associated data
        public ActionResult ClientAccountsMatrix(string id, int? page, string filter)
        {
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            ViewData["ClientSubUnitGuid"] = id;
            ViewData["ClientSubUnitName"] = clientSubUnit.ClientSubUnitName;

            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientSubUnit.ClientTopUnitGuid);
            ViewData["ClientTopUnitGuid"] = id;
            ViewData["ClientTopUnitName"] = clientTopUnit.ClientTopUnitName;

            if (page < 0)
            {
                page = 1;
            }

            ViewData["filter"] = filter;
            var items = clientSummaryRepository.GetClientSummaryClientAccountMatrix(id, page ?? 1);
            return View(items);
        }

        /*
        // post: /SearchForm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchFormUNUSED(string filter, string searchParameter)
        {
            if (searchParameter == "ClientTopUnit")
            {
                return RedirectToAction("ByClientTopUnit", new { filter = filter });
            }
            if (searchParameter == "ClientSubUnit")
            {
                return RedirectToAction("ByClientAccount", new { filter = filter });
            }
            return RedirectToAction("ByClientAccount", new { filter = filter });
            
        }

        //GET
        public ActionResult ByClientAccountUNUSED(string filter, int? page, string sortField, int? sortOrder)
        {
            //SortField
            if (sortField != "ClientAccountName" && sortField != "ClientSubUnitName")
            {
                sortField = "ClientTopUnitName";
            }
            ViewData["CurrentSortField"] = sortField;

            //SortOrder
            if (sortOrder == 1)
            {
                ViewData["NewSortOrder"] = 0;
                ViewData["CurrentSortOrder"] = 1;
            }
            else
            {
                ViewData["NewSortOrder"] = 1;
                ViewData["CurrentSortOrder"] = 0;
            }

            // const int pageSize = 15;
            var items = clientSummaryRepository.GetSummaryByClientAccount(page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            //var paginatedView = new fnDesktopDataAdmin_SelectClientSummaryView_v1Result(items, page ?? 0, pageSize);
            return View(items);
        }

        //GET
        public ActionResult ByClientTopUnitUNUSED(string filter, int? page, string sortField, int? sortOrder)
        {
            //SortField
            if (sortField != "ClientAccountName" && sortField != "ClientSubUnitName")
            {
                sortField = "ClientTopUnitName";
            }
            ViewData["CurrentSortField"] = sortField;

            //SortOrder
            if (sortOrder == 1)
            {
                ViewData["NewSortOrder"] = 0;
                ViewData["CurrentSortOrder"] = 1;
            }
            else
            {
                ViewData["NewSortOrder"] = 1;
                ViewData["CurrentSortOrder"] = 0;
            }

            // const int pageSize = 15;
            var items = clientSummaryRepository.GetSummaryByClientTopUnit(page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(items);
        }

        //GET
        public ActionResult ByClientSubUnitUNUSED(string filter, int? page, string sortField, int? sortOrder)
        {
            //SortField
            if (sortField != "ClientAccountName" && sortField != "ClientSubUnitName")
            {
                sortField = "ClientTopUnitName";
            }
            ViewData["CurrentSortField"] = sortField;

            //SortOrder
            if (sortOrder == 1)
            {
                ViewData["NewSortOrder"] = 0;
                ViewData["CurrentSortOrder"] = 1;
            }
            else
            {
                ViewData["NewSortOrder"] = 1;
                ViewData["CurrentSortOrder"] = 0;
            }

            // const int pageSize = 15;
            var items = clientSummaryRepository.GetSummaryByClientSubUnit(page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(items);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult List(string filter, int? page, string sortField, int? sortOrder, string searchField)
        {
//SortField
            if (sortField != "ClientAccountName" && sortField != "ClientSubUnitName")
            {
                sortField = "ClientTopUnitName";
            }
            ViewData["CurrentSortField"] = sortField;

            //SortOrder
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

            const int pageSize = 15;
            var items = clientSummaryRepository.GetSummary(filter ?? "", sortField, sortOrder ?? 0);
            var paginatedView = new PaginatedList<fnDesktopDataAdmin_SelectClientSummaryView_v1Result>(items, page ?? 0, pageSize);
            return View(paginatedView);
        }
       

        //Shows a TopUnitList and associated SubUnots and ClientAccounts
        public ActionResult TopUnitList(string id, string filter, int? page, string sortField, int? sortOrder)
        {
            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(id);
            if (clientTopUnit == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            ViewData["ClientTopUnitGuid"] = id;
            ViewData["ClientTopUnitName"] = clientTopUnit.ClientTopUnitName;

            //SortField
            if (sortField != "ClientAccountName" && sortField != "ClientSubUnitName")
            {
                sortField = "ClientTopUnitName";
            }
            ViewData["CurrentSortField"] = sortField;

            //SortOrder
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

            const int pageSize = 15;
            var items = clientSummaryRepository.GetTopUnitSummary(id, filter ?? "", sortField, sortOrder ?? 0);
            var paginatedView = new PaginatedList<fnDesktopDataAdmin_SelectClientTopUnitSummaryView_v1Result>(items, page ?? 0, pageSize);
            return View(paginatedView);
        }
        public ActionResult SubUnitList(string id, string filter, int? page, string sortField, int? sortOrder)
        {
            //SortField
            if (sortField != "ClientAccountName" && sortField != "ClientTopUnitName")
            {
                sortField = "ClientSubUnitName";
            }
            ViewData["CurrentSortField"] = sortField;

            //SortOrder
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

            const int pageSize = 15;
            var items = clientSummaryRepository.GetSubUnitSummary(id, filter ?? "", sortField, sortOrder ?? 0);
            var paginatedView = new PaginatedList<fnDesktopDataAdmin_SelectClientSubUnitSummaryView_v1Result>(items, page ?? 0, pageSize);
            return View(paginatedView);
        }
         */
        /*
                public ActionResult List_OLD(string filter, int? page, string sortField, int? sortOrder)
                {
                    return View("Error");

                    //SortField
                    if (sortField != "ClientAccountName" && sortField != "ClientSubUnitName")
                    {
                        sortField = "ClientTopUnitName";
                    }
                    ViewData["CurrentSortField"] = sortField;

                    //SortOrder
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

                    const int pageSize = 15;
                    var items = clientSummaryRepository.GetSummary(filter ?? "", sortField, sortOrder ?? 0);
                    var paginatedView = new PaginatedList<fnDesktopDataAdmin_SelectClientSummaryView_v1Result>(items, page ?? 0, pageSize);
                    return View(paginatedView);
                }
                */
    }
}
