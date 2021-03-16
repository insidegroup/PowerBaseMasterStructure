using CWTDesktopDatabase.Validation;
using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(ClientProfileItemRowValidation))]
	public class ClientProfileItemRow : CWTBaseModel
	{
        public string GDSCode { get; set; } //Added to help with Validation
        public string ClientProfileDataElementName { get; set; }
        public int? ClientProfileItemId { get; set; }
		public int ClientProfileDataElementId { get; set; }
		public int? ClientProfileGroupId { get; set; }
		public bool MandatoryFlag { get; set; }
		public string GDSCommandFormat { get; set; }
		public string Remark { get; set; }
		public string ToolTip { get; set; }
		public string SourceItem { get; set; }
		public string SourceName { get; set; }
		public int? ClientProfileMoveStatusId { get; set; }
		public int? ClientProfileAdminItemId { get; set; }
		public int? ClientProfileItemSequenceNumber { get; set; }
		public int? VersionNumber { get; set; }
		public bool? InheritedFlag { get; set; }
		public bool? InheritedMoveStatusFlag { get; set; }
		public bool? InheritedGDSCommandFormat { get; set; }
		public bool? InheritedRemark { get; set; }
	}
}