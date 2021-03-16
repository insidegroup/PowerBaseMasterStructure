using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(GDSOrderDetailValidation))]
	public partial class GDSOrderDetail : CWTBaseModel
    {
		public bool AbacusFlagNullable { get; set; }
		public bool AllGDSSystemsFlagNullable { get; set; }
		public bool AmadeusFlagNullable { get; set; }
		public bool ApolloFlagNullable { get; set; }
		public bool EDSFlagNullable { get; set; }
		public bool GalileoFlagNullable { get; set; }
		public bool RadixxFlagNullable { get; set; }
		public bool SabreFlagNullable { get; set; }
		public bool TravelskyFlagNullable { get; set; }
		public bool WorldspanFlagNullable { get; set; }
		public bool IsThirdPartyFlagNullable { get; set; }
    }

	public class ValidGDSOrderDetailJSON
	{
		public int GDSOrderDetailId { get; set; }
		public string GDSOrderDetailName { get; set; }
	}

	public partial class GDSOrderDetailReference
	{
		public string TableName { get; set; }
	}
}
