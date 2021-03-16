using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.ViewModels
{
    public class GDSThirdPartyVendorsVM
    {
        public CWTPaginatedList<spDesktopDataAdmin_SelectGDSThirdPartyVendors_v1Result> GDSThirdPartyVendors { get; set; }
    }
}