using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	public partial class SystemUser : CWTBaseModel
    {
        public string LanguageName { get; set; }
        public int LocationId { get; set; }
		public string LocationName { get; set; }
    }
}
