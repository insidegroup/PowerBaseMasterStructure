using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	public partial class ClientAccount : CWTBaseModel
    {
        public string CountryName { get; set; }

    }

   
}
