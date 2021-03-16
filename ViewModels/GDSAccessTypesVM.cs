using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.ViewModels
{
    public class GDSAccessTypesVM
    {
        public CWTPaginatedList<spDesktopDataAdmin_SelectGDSAccessType_v1Result> GDSAccessTypes { get; set; }
    }
}