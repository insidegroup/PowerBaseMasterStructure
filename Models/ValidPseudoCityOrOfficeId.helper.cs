using CWTDesktopDatabase.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Models
{
	public partial class ValidPseudoCityOrOfficeId : CWTBaseModel
	{
	}

	public class ValidPseudoCityOrOfficeIdJSON
    {
		public string PseudoCityOrOfficeId { get; set; }
        public string GDSCode { get; set; }
    }
}
