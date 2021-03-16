using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.ViewModels
{
    public class PseudoCityOrOfficeLocationTypesVM
    {
        public CWTPaginatedList<spDesktopDataAdmin_SelectPseudoCityOrOfficeLocationType_v1Result> PseudoCityOrOfficeLocationTypes { get; set; }
    }
}