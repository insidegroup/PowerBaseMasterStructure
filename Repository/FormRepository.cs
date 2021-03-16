using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class FormRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public Form GetForm(int formId)
        {
            return db.Forms.SingleOrDefault(c => c.FormId == formId);
        }
    }
}
