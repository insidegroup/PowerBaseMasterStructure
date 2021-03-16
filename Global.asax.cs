using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CWTDesktopDatabase.Validation;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Web.SessionState;
using System.Web.Security;
using System.Security.Principal;
using CWTDesktopDatabase.Controllers;

namespace CWTDesktopDatabase
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
    {
        /*
        * If the Session Ends before the FormsAuthentication cookie, then we have problems
        * http://completedevelopment.blogspot.com/2009/12/caution-with-using-sessiontimeout-and.html
        */
        /*protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            //Only access session state if it is available
            if (Context.Handler is IRequiresSessionState || Context.Handler is IReadOnlySessionState)
            {
                //If we are authenticated AND we dont have a session here.. redirect to login page.
                HttpCookie authenticationCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authenticationCookie != null)
                {
                    FormsAuthenticationTicket authenticationTicket = FormsAuthentication.Decrypt(authenticationCookie.Value);
                    if (!authenticationTicket.Expired)
                    {
                        if (Session["ConnectionStringName"] == null)
                        {
                            //This means for some reason the session expired before the authentication ticket. Force a logout.
                            FormsAuthentication.SignOut();
                            Response.Redirect(FormsAuthentication.LoginUrl, true);
                            return;
                        }
                    }
                }
            }
        }*/

		
		/// <summary>
		/// Handle Common Errors
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Application_Error(object sender, EventArgs e)
		{
			if (
					//A potentially dangerous Request.QueryString value was detected from the client
					typeof(System.Web.HttpRequestValidationException) == Server.GetLastError().GetType() 
				
					||

					//The parameters dictionary contains a null entry for parameter 
					typeof(System.ArgumentException) == Server.GetLastError().GetType()
				)
			{
				
				Response.Clear(); 
				
				RouteData routeData = new RouteData();

				routeData.Values.Add("controller", "Error");

				routeData.Values.Add("action", "Error");
				
				// Clear the error on server
				Server.ClearError();

				// Avoid IIS7 getting in the middle
				Response.TrySkipIisCustomErrors = true;

				// Call target Controller and pass the routeData.
				IController errorController = new ErrorController();
				errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));

				return;
			}
		}

        protected void Application_AuthenticateRequest(Object sender, EventArgs e) {

            HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null) {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                string[] roles = authTicket.UserData.Split(new Char[] { ',' });
                GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(authTicket.Name), roles);
                Context.User = userPrincipal;
            }
        }

        internal protected void Application_PreRequestHandlerExecute()
        {
            //Added v2.08.1 for Caching for OWASP - Information Disclosure - Inadequate Cache Control
            //THis should disbale caching for MVC files, but leave enabled for static files
            if (HttpContext.Current.CurrentHandler is MvcHandler)
            {
                HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
                HttpContext.Current.Response.Cache.SetValidUntilExpires(false);
                HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetNoStore();
            }
        }

        /* ROUTES           TYPE    SORTABLE    PARAM1          PARAM2      EXAMPLE
         * ListMain -       LIST    YES                                     Items/List/page/sortField/sortOrder[?id=1]
         * 
         * List -           LIST    YES         ID(int)                     Items/List/1/page/sortField/sortOrder
         * 
         * Main  -          VIEW    NO                                      Items/View[?id=1]
         *                  LIST    NO                                      Items/List[?id=1]
         *                  
         * Default -        VIEW    NO          ID(int)                     Items/View/1
         *                  LIST    NO          ID(int)                     Items/List/1
         * 
         * LanguageView     VIEW    NO          CODE(string)    ID(int)      Items/View/1/en
         * 
         */
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //FINAL VERSION
            //Go to ViewPage of a Language (for items with INT ID and a STRING LanguageCode)
            //we allow string in the URL in this case as it contains no illegal characters
            //Translation/View.mvc/1/en-gb
            routes.MapRoute(
               "LanguageView",
               "{controller}.mvc/{action}/{id}/{languageCode}",
               new { action = "View", },
               new { id = @"\d+" }
            );

            //FINAL VERSION
            //passes an Id only - used for some Create,Edit,Delete,View (for items with VARCHAR ID)
            //Client/View.mvc?id=ABC123
            routes.MapRoute(
                "Main", // Route name
                "{controller}.mvc/{action}", // URL with parameters
                new { controller = "Home", action = "Index" }
            );

            //FINAL VERSION
            //passes an Id only - used for most Create,Edit,Delete,View (for items with INT ID)
            //Client/View.mvc/123
            routes.MapRoute(
                "Default", // Route name
                "{controller}.mvc/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index" },
                new { id = @"\d+" }
            );


            //FINAL VERSION
            //sorting on ListPage (for items with INT ID)
            //Client/List.mvc/123/1/Name/0
            routes.MapRoute(
               "List",
               "{controller}.mvc/{action}/{id}/{page}/{sortField}/{sortOrder}",
               new { action = "List", page = 0, sortField = "Name", sortOrder = 0 },
               new { id = @"\d+", page = @"\d+", sortOrder = @"\d+" }
            );


            //FINAL VERSION
            //sorting on ListPage (for items with VARCHAR ID)
            //Client/List.mvc/1/Name/0?id=ABC123
            routes.MapRoute(
               "ListMain",
               "{controller}.mvc/{action}/{page}/{sortField}/{sortOrder}",
               new { action = "List", page = 0, sortField = "Name", sortOrder = 0 },
               new { page = @"\d+", sortOrder = @"\d+" }
            );


            routes.MapRoute(
                "Root",
                "",
                new { controller = "Home", action = "Index", id = "" }
            );

        }

        protected void Application_Start()
        {
			//Removing the X-AspNetMvc-Version HTTP Header
			MvcHandler.DisableMvcResponseHeader = true;

			AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);
            
            // Initialize log4net.
            //log4net.Config.XmlConfigurator.Configure();

            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RemoteUID_Attribute), typeof(RemoteValidator));
        }

        //protected void Session_Start()
        //{
        //    Session.Timeout = 60;
        //}

		protected void Application_PreSendRequestHeaders()
		{
			Response.Headers.Remove("Server");
		}

    }
}