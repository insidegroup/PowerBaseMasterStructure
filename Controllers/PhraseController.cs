using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;

namespace CWTDesktopDatabase.Controllers
{
    public class PhraseController : Controller
    {
        PhraseRepository phraseRepository = new PhraseRepository();

        //GET: A list of Cities
        public ActionResult List(string filter, int? page, string sortField, int? sortOrder)
        {

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToReferenceInfo())
            {
                ViewData["Access"] = "WriteAccess";
            }

            //SortField + SortOrder settings
            if (sortField != "PhraseName")
            {
                sortField = "PhraseName";
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
            }

            var items = phraseRepository.PagePhrases(page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(items);
        }

        // GET: A SIngle Phrase
        public ActionResult View(int id = 0)
        {
            Phrase phrase = new Phrase();
            phrase = phraseRepository.GetPhrase(id);

            //Check Exists
            if (phrase == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            return View(phrase);
        }
    }
}
