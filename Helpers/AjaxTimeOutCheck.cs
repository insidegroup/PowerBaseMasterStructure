using System.Web.Mvc;
using System.Web.Security;

namespace CWTDesktopDatabase.Helpers
{
    public class AjaxTimeOutCheckAttribute : ActionFilterAttribute
    {
        /*
         * Donogh McArdle 27-April-2011
         * 
         * Add a Header to Ajax Response if user is logged in
         * We can check for this header and if not there, we can redirect to login page
         * This fixes the problem in the Wizards, where, on TimeOut, the login page would be loaded within the ajax div
         * Note: Modifying response headers requires 'IIS Integrated Pipeline Mode'
         * 
         * Added to the following Controllers that are called using AJAX from within the Wizard section:
         * AutoComplete, Hierarchy, LocationWizard, TeamWizard, SystemUserWizard, ClientWizard, ServicingOptionItemValue, Country
         * 
         * https://bitbucket.org/stevehorn/mvcajaxformsauthtimeout/downloads
         * http://blog.stevehorn.cc/2010/09/aspnet-mvc-handling-forms.html
         */
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                if (filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    filterContext.HttpContext.Response.AddHeader("X_User_Logged_In", "true");   //IIS6
                    // filterContext.HttpContext.Response.Headers.Add("X_User_Logged_In", "true");   //IIS7 Integrated PipelineMode
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }
}