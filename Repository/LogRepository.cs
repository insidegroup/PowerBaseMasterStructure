using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Web;
using System.Configuration;
using log4net;
using System.Text.RegularExpressions;

namespace CWTDesktopDatabase.Repository
{
    public class LogRepository
    {
  
        //Write a message to the event log
        public void LogError(string message)
        {
            //AdminUser
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            //FileName
            string fileName = adminUserGuid;
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '_');
            }

            //URL
            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            if(url.IndexOf("LogOn?a=") != -1){
                string[] lines = Regex.Split(url, "\\?a=");
                url = lines[0]; //dont store the hash
            }
            //Logging Variables
            log4net.GlobalContext.Properties["Referrer"] = url;
            log4net.GlobalContext.Properties["FileName"] = fileName;
            log4net.GlobalContext.Properties["SystemUserGuid"] = adminUserGuid;

            log4net.Config.XmlConfigurator.Configure();
            log4net.ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            log.Error(message);
        }

        //Add to DB
        public void LogApplicationUsage(int applicationEventID, string clientSubUnitGuid, string travelerGuid, string additionalInformation, string cardIdentifier, int? creditCardTypeID, bool eventStatus)
        {
            /*
            applicationEventID
            3	Exit Application
            7	System User Logged In
            8	System User Viewed Credit Card
            11	Create Credit Card
            12	Update Credit Card
            13	Delete Credit Card
            14	Add Role
            15	Remove Role
            16	Application Error
            */

            HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

            //These items are the same for every event
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];


            db.spDesktopDataAdmin_InsertApplicationUsage_v1(
                Settings.ApplicationName(),
                Settings.ApplicationVersion(),
                null,
                GetComputerName(),
                adminUserGuid,
                null,
                null,
                applicationEventID,
                clientSubUnitGuid,
                travelerGuid,
                additionalInformation,
                cardIdentifier,
                creditCardTypeID,
                eventStatus
            );



        }

        //Add to DB - Override used on first login when HttpContext is unavailable
        public void LogApplicationUsageFirstLogin(int applicationEventID, string clientSubUnitGuid, string travelerGuid, string additionalInformation, string cardIdentifier, int? creditCardTypeID, bool eventStatus, string systemUserGuid)
        {
            /*
            applicationEventID
            3	Exit Application
            7	System User Logged In
            8	System User Viewed Credit Card
            11	Create Credit Card
            12	Update Credit Card
            13	Delete Credit Card
            14	Add Role
            15	Remove Role
            16	Application Error
            */

            HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

            db.spDesktopDataAdmin_InsertApplicationUsage_v1(
                Settings.ApplicationName(),
                Settings.ApplicationVersion(),
                null,
                GetComputerName(),
                systemUserGuid,
                null,
                null,
                applicationEventID,
                clientSubUnitGuid,
                travelerGuid,
                additionalInformation,
                cardIdentifier,
                creditCardTypeID,
                eventStatus
            );



        }

        //Returns Users Computer Name or IP
        public string GetComputerName()
        {
            string userIP ="";
            try
            {
                userIP = HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];

                //try to get computer name from IP
                return System.Net.Dns.GetHostEntry(userIP).HostName;
            }
            catch (Exception)
            {
                //cannnot get computer name, use IP
                return userIP;
            }

        }
    }
}
