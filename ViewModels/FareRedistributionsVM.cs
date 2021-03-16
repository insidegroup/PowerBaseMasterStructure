using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.ViewModels
{
    public class FareRedistributionsVM
    {
        public CWTPaginatedList<spDesktopDataAdmin_SelectFareRedistribution_v1Result> FareRedistributions { get; set; }
    }
}