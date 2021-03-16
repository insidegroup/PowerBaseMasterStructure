using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.ViewModels
{
    public class GDSOrderTypesVM
    {
        public CWTPaginatedList<spDesktopDataAdmin_SelectGDSOrderType_v1Result> GDSOrderTypes { get; set; }
    }
}