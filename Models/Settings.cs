
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Helpers;
using System.Configuration;
using System.Security.Principal;
using System.Web.Security;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Models
{

    public static class Settings
    {
        public static string ApplicationName() { return "DDB Admin"; }
		public static string ApplicationVersion() { return "v20.2.2"; }
       
        public static string getConnectionStringName()
        {
            string connectionStringName = "";

            //If AuthCookie is not null, get from there, otherwise from config
            if (HttpContext.Current.User.Identity.Name != "")
            {
                connectionStringName = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[1];
            }else{
                //we use this connectionstring if none specified
                connectionStringName = ConfigurationManager.AppSettings["DefaultConnectionStringName"];
            }
            return connectionStringName;
        }

        public static string getConnectionString()
        {

            string connectionStringName = getConnectionStringName();
            string returnValue = ""; 

            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[connectionStringName];
            
            // Look for the name in the connectionStrings section.
            // If found, return the connection string.
            if (settings != null)
            {
                returnValue = settings.ConnectionString;
            }

            return returnValue;
        }

		/// <summary>
		/// Increase command timeout to handle large datasets
		/// </summary>
		/// <returns></returns>
		public static int GetCommandTimeout()
		{
			int commandTimeout = 30; //Default;

			if (HttpContext.Current.Session["CommandTimeout"] == null)
			{
				try
				{
					using (SqlConnection conn = new SqlConnection())
					{
						conn.ConnectionString = getConnectionString();
						conn.Open();

						SqlCommand command = new SqlCommand(@"
							SELECT TOP 1 ConfigurationParameterValue 
							FROM ConfigurationParameter 
							WHERE 
								ConfigurationParameterName = @configurationParameterName 
								AND
								ContextId = @ContextId", conn);

						command.Parameters.Add(new SqlParameter("ConfigurationParameterName", "PowerBaseCommandTimeoutInSeconds"));
						command.Parameters.Add(new SqlParameter("ContextId", 10));

						//Get value from database
						if (command.ExecuteScalar() != null)
						{
							//Save value
							string configurationParameterValue = command.ExecuteScalar().ToString();

							//Convert to number
							if (!Int32.TryParse(configurationParameterValue, out commandTimeout))
							{
								commandTimeout = 30; //Reset to default otherwise will be set to 0 which is infinite timeout
							}

							//Store in Session
							HttpContext.Current.Session["CommandTimeout"] = commandTimeout;
						}
					}
				}
				catch (Exception ex)
				{
					//No database connection
				}
			}
			else
			{
				Int32.TryParse(HttpContext.Current.Session["CommandTimeout"].ToString(), out commandTimeout);
			}

			return commandTimeout;
		}
    }
}