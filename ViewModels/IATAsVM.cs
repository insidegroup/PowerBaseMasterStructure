using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.ViewModels
{
    public class IATAsVM
    {
        public CWTPaginatedList<spDesktopDataAdmin_SelectIATA_v1Result> IATAs { get; set; }
    }
}