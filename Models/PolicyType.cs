using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	public class PolicyType
	{
		public string PolicyTypeTableName { get; set; }
		public string NavigationLinkPolicyTypeName { get; set; }
		public int? NavigationLinkDisplayOrder { get; set; }
		public int? ItemCount { get; set; }
	}
}
