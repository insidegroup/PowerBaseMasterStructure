using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.ViewModels
{
    public class PseudoCityOrOfficeTypesVM
    {
        public CWTPaginatedList<spDesktopDataAdmin_SelectPseudoCityOrOfficeType_v1Result> PseudoCityOrOfficeTypes { get; set; }
    }
}