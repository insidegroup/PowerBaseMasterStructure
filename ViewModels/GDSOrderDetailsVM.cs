using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.ViewModels
{
    public class GDSOrderDetailsVM
    {
        public CWTPaginatedList<spDesktopDataAdmin_SelectGDSOrderDetail_v1Result> GDSOrderDetails { get; set; }
    }
}