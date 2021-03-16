using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class ExternalSystemLoginSystemUserRepository
    {
        private ExternalSystemLoginSystemUserDC db = new ExternalSystemLoginSystemUserDC(Settings.getConnectionString());

        public ExternalSystemLoginSystemUser GetExternalSystemLoginSystemUser(string systemUserGuid)
        {
            return db.ExternalSystemLoginSystemUsers.SingleOrDefault(c => c.SystemUserGuid == systemUserGuid);
        }
    }
}