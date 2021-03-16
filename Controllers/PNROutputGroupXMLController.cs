using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;

namespace CWTDesktopDatabase.Controllers
{
    public class PNROutputGroupXMLController : Controller
    {
        //main repositories
        PNROutputGroupLanguageRepository pnrOutputGroupLanguageRepository = new PNROutputGroupLanguageRepository();
        PNROutputGroupRepository pnrOutputGroupRepository = new PNROutputGroupRepository();

        //GET:List
        public ActionResult ListLanguages(int id, int? page, string sortField, int? sortOrder)
        {
            //Get PNROutputGroup
            PNROutputGroup pnrOutputGroup = new PNROutputGroup();
            pnrOutputGroup = pnrOutputGroupRepository.GetGroup(id);

            //Check Exists
            if (pnrOutputGroup == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToPNROutputGroup(pnrOutputGroup.PNROutputGroupId))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Parent Information
            ViewData["PNROutputGroupId"] = pnrOutputGroup.PNROutputGroupId;
            ViewData["PNROutputGroupName"] = pnrOutputGroup.PNROutputGroupName;


            //SortField+SortOrder settings
            if (sortField != "HasRemarks")
            {
                sortField = "LanguageName";
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
            var cwtPaginatedList = pnrOutputGroupLanguageRepository.PagePNROutputGroupLanguages(id, page ?? 1, "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        //GET: List Remarks
        public ActionResult List(int id, string languageCode)
        {
            PNROutputGroup pnrOutputGroup = new PNROutputGroup();
            pnrOutputGroup = pnrOutputGroupRepository.GetGroup(id);

            if (pnrOutputGroup == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToPNROutputGroup(pnrOutputGroup.PNROutputGroupId))
            {
                ViewData["Access"] = "WriteAccess";
            }

            PNROutputGroupLanguage pnrOutputGroupLanguage = new PNROutputGroupLanguage();
            pnrOutputGroupLanguage = pnrOutputGroupLanguageRepository.GetItem(id, languageCode);
            pnrOutputGroupLanguageRepository.EditItemForDisplay(pnrOutputGroupLanguage);

            return View(pnrOutputGroupLanguage);
        }

        //GET: View
        public ActionResult View(int id, string languageCode, int node)
        {
            PNROutputGroupLanguage pnrOutputGroupLanguage = new PNROutputGroupLanguage();
            pnrOutputGroupLanguage = pnrOutputGroupLanguageRepository.GetItem(id, languageCode);

            if (pnrOutputGroupLanguage == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPNROutputGroup(pnrOutputGroupLanguage.PNROutputGroupId))
            {
                ViewData["Access"] = "WriteAccess";
            }

            PNROutputGroupXMLItem pnrOutputGroupXMLItem = new PNROutputGroupXMLItem();
            pnrOutputGroupLanguageRepository.EditItemForDisplay(pnrOutputGroupLanguage);
            pnrOutputGroupXMLItem = pnrOutputGroupLanguageRepository.GetPNROutputGroupXMLItem(node, pnrOutputGroupLanguage);

            return View(pnrOutputGroupXMLItem);
        }

    }
}
