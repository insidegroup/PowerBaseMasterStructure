using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.ViewModels
{
    public class GDSRequestTypesVM
    {
        public CWTPaginatedList<spDesktopDataAdmin_SelectGDSRequestType_v1Result> GDSRequestTypes { get; set; }
    }
}