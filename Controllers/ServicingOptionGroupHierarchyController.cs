using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class ServicingOptionGroupHierarchyController : Controller
    {
        //main repositories
        ServicingOptionGroupRepository servicingOptionGroupRepository = new ServicingOptionGroupRepository();
        HierarchyRepository hierarchyRepository = new HierarchyRepository();

    }
}
