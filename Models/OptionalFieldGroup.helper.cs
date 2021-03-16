using CWTDesktopDatabase.Validation;
using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Models
{

	[MetadataType(typeof (OptionalFieldGroupValidation))]
	public partial class OptionalFieldGroup : CWTBaseModel
	{
		public string HierarchyType { get; set; } //Link to Hierarchy     eg Location, CountryRegion, Country, ClientSubUnit etc
		public string HierarchyItem { get; set; } //Text Value            eg London or UK
		public string HierarchyCode { get; set; } //Code                  eg LON or GB

		//only used when a ClientAccount is chosen
		public string SourceSystemCode { get; set; } //CLientAccountNumber is stored in HierarchyCode

		//does this item connect to multiple Hierarchy items
		public bool IsMultipleHierarchy { get; set; }
	}

	//JSON lookups use this
	public partial class OptionalFieldGroupJSON
	{
		public string OptionalFieldGroupId { get; set; }
		public string OptionalFieldGroupName { get; set; }
	}
}