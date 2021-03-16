using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class OptionalFieldOutputRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

    }
}