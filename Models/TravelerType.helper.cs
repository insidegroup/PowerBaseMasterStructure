using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Models
{
	public partial class TravelerType : CWTBaseModel
    {
        public string TravelerBackOfficeTypeDescription { get; set; }
		public TravelerTypeSponsor TravelerTypeSponsor { get; set; }
		public ClientTopUnit ClientTopUnit { get; set; }
		public ClientSubUnit ClientSubUnit { get; set; }
    }
}
