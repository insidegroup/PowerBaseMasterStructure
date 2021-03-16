using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.ViewModels
{
    public class ExternalNamesVM
    {
        public CWTPaginatedList<spDesktopDataAdmin_SelectExternalName_v1Result> ExternalNames { get; set; }
    }
}