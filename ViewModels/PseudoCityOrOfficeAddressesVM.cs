using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.ViewModels
{
    public class PseudoCityOrOfficeAddressesVM
    {
        public CWTPaginatedList<spDesktopDataAdmin_SelectPseudoCityOrOfficeAddress_v1Result> PseudoCityOrOfficeAddresses { get; set; }
    }
}