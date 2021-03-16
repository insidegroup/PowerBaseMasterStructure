using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.ViewModels
{
    public class PseudoCityOrOfficeDefinedRegionsVM
    {
        public CWTPaginatedList<spDesktopDataAdmin_SelectPseudoCityOrOfficeDefinedRegion_v1Result> PseudoCityOrOfficeDefinedRegions { get; set; }
    }
}