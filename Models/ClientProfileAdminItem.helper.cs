using CWTDesktopDatabase.Validation;
using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Models
{


   [MetadataType(typeof(ClientProfileAdminItemRowValidation))]
	public class ClientProfileAdminItemRow : CWTBaseModel
    {
        public string ClientProfileDataElementName { get; set; }
        public int? ClientProfileAdminItemId { get; set; }
        public int ClientProfileDataElementId { get; set; }
        public int? ClientProfileAdminGroupId { get; set; }
		public bool MandatoryFlag { get; set; }
		public bool? InheritedFlag { get; set; }
        public string DefaultGDSCommandFormat { get; set; }
        public string DefaultRemark { get; set; }
		public string ToolTip { get; set; }
		public string Source { get; set; }
        public int? ClientProfileMoveStatusId { get; set; }
        public int? ClientProfileAdminItemSequenceNumber { get; set; }
        public int? VersionNumber { get; set; }
    }
}