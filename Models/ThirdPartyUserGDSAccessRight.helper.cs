using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(ThirdPartyUserGDSAccessRightValidation))]
	public partial class ThirdPartyUserGDSAccessRight : CWTBaseModel
	{
		public string ThirdPartyUserGDSAccessRightInternalRemark { get; set; }
    }

	public partial class Entitlement
	{
		//GDSSIgnOn
		public string tpAgentID { get; set; }

		//PseudoCityOrOfficeId
		public string tpPCC { get; set; }
		
		//GDSName
		public string tpServiceID { get; set; }

		public bool DeletedFlag { get; set; }

		public DateTime? DeletedTimestamp { get; set; }

	}
}
