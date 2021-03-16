using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.Helpers;
using System.Web.Profile;
using System.Collections.Specialized;
using System.Globalization;
using System.Configuration;
using CWTAuthentication;


namespace MvcApplication1.Controllers
{
    [HandleError]
	[Bind(Include = "FormsService")]
	public class AccountController : Controller
    {
        public IFormsAuthenticationService FormsService { get; set; }
        
		private LogRepository logRepository = new LogRepository();
		private CWTAuthenticationHelper CWTAuthenticationHelper = new CWTAuthenticationHelper();

        //Initialise
        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            base.Initialize(requestContext);
        }

        //**************************************
        // URL: /Account/LogOn
        // will return this view if invalid user, will login and go to home if valid user
        // **************************************

		public ActionResult LogOn()
		{
			//Check A Parameter
			string a = this.HttpContext.Request.QueryString["a"];

			if (string.IsNullOrEmpty(a))
			{
				//No A Parameter
				string errorMessage = "No A Parameter detected - user not authorised";
				logRepository.LogApplicationUsage(16, "", "", errorMessage, "", null, false);
				return View();
			}

			try
			{
				string decodedURL = CWTAuthenticationHelper.Decrypt(a);
				if (!string.IsNullOrEmpty(decodedURL))
				{
					//Split Parameters and put into NameValueCollection for easy access
					string[] urlParameters = urlParameters = decodedURL.Split(';');
					NameValueCollection qsValues = new NameValueCollection();
					string[] nameAndValue;
					foreach (string parameter in urlParameters)
					{
						if (parameter != "")
						{
							nameAndValue = parameter.Split(new char[] { '=' });
							qsValues.Add(nameAndValue[0], nameAndValue[1]);
						}
					}

					//Process User Id
					if (qsValues["cwt_user_OID"] == null || string.IsNullOrEmpty(qsValues["cwt_user_OID"]))
					{
						string errorMessage = "(Err 0002) Failed Login Attempt - UID Parameter Missing";
						logRepository.LogError(errorMessage);
						logRepository.LogApplicationUsage(16, "", "", errorMessage, "", null, false);
                        return View();
					};

					//Process Timestamp
					if (qsValues["timestamp"] == null || string.IsNullOrEmpty(qsValues["timestamp"]))
					{
						string errorMessage = "(Err 0004) Failed Login Attempt - Timestamp Parameter Missing";
						logRepository.LogError(errorMessage);
						logRepository.LogApplicationUsage(16, "", "", errorMessage, "", null, false);
                        return View();
					};

					//Check URL timestamp
					long urlTimeStamp = Int64.Parse(qsValues["timestamp"]);

					//Get current timestamp (based on UTC time)
					DateTime times = new DateTime();
					DateTime date1970 = new DateTime(1970, 1, 1);
					times = DateTime.Now.ToUniversalTime(); //change Server time to UTC time
					TimeSpan t = new TimeSpan();
					t = times - date1970;
					TimeZone localTimeZone = TimeZone.CurrentTimeZone;

					long currentTimeStamp = new long();
					currentTimeStamp = System.Convert.ToInt64(t.TotalMilliseconds - (60000 * ((localTimeZone.GetUtcOffset(times).Hours * 60) + (localTimeZone.GetUtcOffset(times).Minutes))));

					//Check if more than 60 seconds since Login URL created
					if ((Math.Abs(currentTimeStamp - urlTimeStamp) > 60000))
					{
						string errorMessage = "(Err 0005) Failed Login Attempt - Out Of Range TimeStamp, currentTimeStamp=" + currentTimeStamp.ToString() + ", urlTimeStamp=" + urlTimeStamp.ToString();
						logRepository.LogError(errorMessage);
						logRepository.LogApplicationUsage(16, "", "", errorMessage, "", null, false);
                        return View();
					}

					//Try login user
					AccountRepository accountRepository = new AccountRepository();
					SystemUser systemUser = accountRepository.GetUserBySystemUserGuid(qsValues["cwt_user_OID"]);
					if (systemUser == null)
					{
						logRepository.LogError("(Err 0006) Failed Login Attempt - No Such User(" + qsValues["cwt_user_OID"].ToString() + ")");
						logRepository.LogApplicationUsage(16, "", "", "No Such User(" + qsValues["cwt_user_OID"].ToString() + ")", "", null, false);
                        return View();
					}

					//SUCCESSFUL LOGIN
					//Store userdata in cookie for Forms Authentication                 
					accountRepository.persistUser(systemUser.SystemUserGuid, ConfigurationManager.AppSettings["DefaultConnectionStringName"]);

					//Log
					logRepository.LogApplicationUsageFirstLogin(7, "", "", "", "", null, true, systemUser.SystemUserGuid);

					//Update login TimeStamp
					SystemUserRepository systemUserRepository = new SystemUserRepository();
					systemUserRepository.UpdateSystemUserLastLoginTimestamp(qsValues["cwt_user_OID"]);

					//go to Home Page
					return RedirectToAction("Index", "Home");
				}
			}
			catch (Exception ex)
			{
				//Other error
				string errorMessage = "(Err 0007) Failed Login Attempt - " + ex.Message.ToString();
				logRepository.LogApplicationUsage(16, "", "", errorMessage, "", null, false);
				logRepository.LogError(errorMessage);
			}

			return View();
		}

        // **************************************
        // URL: /Account/LogOff
        // **************************************
        public ActionResult LogOff()
        {
            //Sign Out
            FormsService.SignOut();

            //Log
            logRepository.LogApplicationUsage(3, "", "", "", "", null, true);

			//PCI
			Session.Clear();
			Session.Abandon();

			//Expire All Cookies
			string[] myCookies = Request.Cookies.AllKeys;
			foreach (string cookieName in myCookies)
			{
                HttpCookie cookie = Request.Cookies[cookieName];
                Response.Cookies.Remove(cookieName);
                cookie.Expires = DateTime.Now.AddDays(-10);
                cookie.Secure = Security.IsHttps();
                cookie.Value = null;
                Response.SetCookie(cookie);
			}

			//Create Blank ASP.NET SessionID Cookie
			HttpCookie sessionCookie = new HttpCookie("ASP.NET_SessionId", "")
			{
				Secure = Security.IsHttps(),
				Expires = DateTime.Now.AddDays(-1)
			};
			Response.Cookies.Add(sessionCookie);

            return View();
        }
    }
}
