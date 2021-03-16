using CWTDesktopDatabase.Validation;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{

	[MetadataType(typeof(ClientProfileGroupValidation))]
	public partial class ClientProfileGroup : CWTBaseModel
	{
		public string HierarchyType { get; set; } //Link to Hierarchy     eg Location, CountryRegion, Country, ClientSubUnit etc
		public string HierarchyItem { get; set; } //Text Value            eg London or UK
		public string HierarchyCode { get; set; } //Code                  eg LON or GB
	}

	//JSON lookups use this
	public partial class ClientProfileGroupJSON
	{
		public string ClientProfileGroupId { get; set; }
		public string ClientProfileGroupName { get; set; }
	}
}