using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;


namespace CWTDesktopDatabase.Models
{
	public partial class Country : CWTBaseModel
    {
		public string CountryNameWithInternationalPrefixCode
		{
			get
			{
				if (!string.IsNullOrEmpty(InternationalPrefixCode))
				{
					return string.Format("{0} ({1})", CountryName, InternationalPrefixCode);
				}
				return CountryName;
			}
		} 
    }
}
