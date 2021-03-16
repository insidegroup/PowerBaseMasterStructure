using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CWTDesktopDatabase.Helpers
{
    public class CWTStringHelpers
    {
        //Trims a string to a length adding "..." to the end 
        //EXAMPLE: TrimString(s,30) would trim s to 27 charachters + "..."
        //to use, add '<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>' to View
        //Call as follows:  =CWTStringHelpers.TrimString(String, length)
        public static string TrimString(string s, int maxLength)
        {
            if (String.IsNullOrEmpty(s))
                return String.Empty;

            if (s.Length > maxLength)
                return s.Substring(0, maxLength - 3) + "...";

            return s;
        }

        public static string ShortHierarchyType(string s)
        {
            if (String.IsNullOrEmpty(s))
                return String.Empty;

			switch (s)
			{
				case "ClientTopUnit":
					s = "ClientTop";
					break;
				case "ClientSubUnit":
					s = "ClientSub";
					break;
				case "ClientAcc":
					s = "";
					break;
				case "TravelerType":
					s = "TravType";
					break;
				case "ClientSubUnitTravelerType":
					s = "CSUTT";
					break;
				case "ClientDefinedRuleGroup":
					s = "BusRules";
					break;
			}

            return s;
        }

        //adds zero-length spaces to a long string
        public static string WrapString(string s)
        {
            if (String.IsNullOrEmpty(s))
                return String.Empty;

            if(s.Length > 50)
             s = s.Insert(50, "&#8203;");

            return s;

        }

		public static string FormatWysiwygText(string s)
		{
			s = System.Web.HttpUtility.HtmlDecode(s);
			
			if (s.Contains(""))
			{
				s = s.Replace("•", "<br/>•");
			}

			if (s.Contains("\n")) {
				s = s.Replace("\n", "<br/>");
			}

			return s;
		}

		private static bool ContainsHTML(string CheckString)
		{
			return Regex.IsMatch(CheckString, "<(.|\n)*?>");
		}

		public static string GenerateDateBasedId()
		{
			return ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString() + DateTime.UtcNow.Millisecond.ToString();
		}

        public static string AlphaNumericOnly(string s)
        {
            Regex regex = new Regex("[^a-zA-Z0-9 ]");

            return regex.Replace(s, "");
        }

        public static string UnescapeQuotes(string cellValue)
        {
            if (string.IsNullOrEmpty(cellValue))
            {
                return "";
            }

            if (cellValue.StartsWith("\""))
            {
                cellValue = cellValue.Substring(1);
            }

            if (cellValue.EndsWith("\""))
            {
                cellValue = cellValue.Substring(0, cellValue.Length - 1);
            }

            return cellValue;
        }

        public static string NullToEmpty(string cellValue)
        {
            if (cellValue.ToUpper() == "NULL")
            {
                return string.Empty;
            }

            return cellValue;
        }

		public static string EncloseCellValue(string cellValue)
		{
			return string.Format("\"{0}\"", cellValue);
		}


		/// <summary>
		/// Checks a string and builds a date time from hours, minutes and seconds
		/// </summary>
		/// <param name="datetime"></param>
		/// <returns></returns>
		public static DateTime BuildDateTime(string datetime)
		{
			if (string.IsNullOrEmpty(datetime))
			{
				return new DateTime();
			}

			string[] dateTimeParts = datetime.Split(':');

			//Hours
			int hour = 0;
			if (dateTimeParts.Length >= 1)
			{
				hour = Convert.ToInt32(dateTimeParts[0]);
			}

			//Minutes
			int minutes = 0;
			if (dateTimeParts.Length >= 2)
			{
				minutes = Convert.ToInt32(dateTimeParts[1]);
			}

			//Seconds
			int seconds = 0;
			if (dateTimeParts.Length >= 3)
			{
				seconds = Convert.ToInt32(dateTimeParts[2]);
			}

			return new DateTime(1900, 1, 1, hour, minutes, seconds);
		}
    }
}
