using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Web.Security;
using System.Configuration;

namespace CWTDesktopDatabase.Repository
{
    public class AccountRepository
    {

        //Used for Authentication (/Account/LogOn)
        public SystemUser GetUserBySystemUserGuid(string guid)
        {
            string connectionString = ""; 
            string connectionStringName = ConfigurationManager.AppSettings["DefaultConnectionStringName"];
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[connectionStringName];
            if (settings != null)
            {
                connectionString = settings.ConnectionString;
            }

            SystemUserDC db = new SystemUserDC(connectionString);

            SystemUser systemUser = new SystemUser();
            systemUser = (from u in db.SystemUsers where u.SystemUserGuid == guid select u).FirstOrDefault();
            return systemUser;
        }


        // **************************************
        // URL: /persistUser - add details to cookie
        // **************************************       
        public void persistUser(string systemUserGuid, string connectionString)
        {
            //string db = "user," + ConfigurationManager.AppSettings["DefaultConnectionStringName"];
            string db = systemUserGuid + "|" + connectionString;

            // Create ticket
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1,
                db,                                                                 //name
                DateTime.Now,                                                       //starting from now
                DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes),  //timeout from web.config
                false,                                                              //persistent = false
                "user",                                                             //"user" = setup in web.config, but has no meaning other than allow access
                FormsAuthentication.FormsCookiePath                                 //no child path
            );

            // Create encrypted cookie
            string hash = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);
            cookie.HttpOnly = true; //more secure cannot be accessed by client scripts
			cookie.Secure = Helpers.Security.IsHttps();
		
            //if (ticket.IsPersistent){cookie.Expires = ticket.Expiration;}

            // Set and done
            HttpContext.Current.Response.Cookies.Add(cookie); //Necessary, otherwise UserData property gets lost
        }

       
    }
}