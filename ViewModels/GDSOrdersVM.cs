using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.ViewModels
{
	public class GDSOrdersVM
    {
        public CWTPaginatedList<spDesktopDataAdmin_SelectGDSOrders_v1Result> GDSOrders { get; set; }
    }
}