using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using CWTDesktopDatabase.Repository;

namespace CWTDesktopDatabase.Helpers
{
	public static class CWTEmailHelper
	{
		public static bool SendEmail(List<string> to, string from, string subject, string body, List<string> attachments)
		{
			bool success = false;
			
			try
			{
				//Create new email mesage
				MailMessage objMail = new MailMessage();

				//List of To Addresses
				foreach (string toAddress in to)
				{
					if (!string.IsNullOrEmpty(toAddress))
					{
						objMail.To.Add(new MailAddress(toAddress));
					}
				}

				objMail.From = new MailAddress(from);
				objMail.Body = body;
				objMail.Subject = subject;

				//Add Attachments
				if (attachments != null)
				{
					foreach (string attachment in attachments)
					{
						objMail.Attachments.Add(new System.Net.Mail.Attachment(attachment));
					}
				}

				//Get values from config
				string mailServer = ConfigurationManager.AppSettings["SMTP_MailServer"];
				if (string.IsNullOrEmpty(mailServer))
				{
					LogRepository logRepository = new LogRepository();
					logRepository.LogError("GDS Order Email Configuration: Mail Server value missing");
					return false;
				}

				//Send email
				SmtpClient smtpClient = new SmtpClient(mailServer)
				{
					UseDefaultCredentials = false,
					EnableSsl = false
				};

				smtpClient.Send(objMail);
				
				success = true;
			}
			catch (Exception ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);
			}

			return success;

		}
	}
}