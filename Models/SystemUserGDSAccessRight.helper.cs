using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(SystemUserGDSAccessRightValidation))]
	public partial class SystemUserGDSAccessRight : CWTBaseModel
	{
		public string SystemUserGDSAccessRightInternalRemark { get; set; }
		public SystemUser SystemUser { get; set; }
    }
}
