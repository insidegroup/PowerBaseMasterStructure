using CWTDesktopDatabase.Validation;
using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Models
{

	[MetadataType(typeof(ClientProfileAdminGroupValidation))]
	public partial class ClientProfileAdminGroup : CWTBaseModel
	{
		public string HierarchyType { get; set; } //Link to Hierarchy     eg Location, CountryRegion, Country, ClientSubUnit etc
		public string HierarchyItem { get; set; } //Text Value            eg London or UK
		public string HierarchyCode { get; set; } //Code                  eg LON or GB
	}

	//JSON lookups use this
	public partial class ClientProfileAdminGroupJSON
	{
		public string ClientProfileAdminGroupId { get; set; }
		public string ClientProfileAdminGroupName { get; set; }
	}
}