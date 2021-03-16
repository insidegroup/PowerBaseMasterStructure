using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(ClientTopUnitMatrixDPCodeValidation))]
	public partial class ClientTopUnitMatrixDPCode : CWTBaseModel
    {
        public string MatrixDPCode { get; set; }
        public int VersionNumber { get; set; }

        public ClientTopUnit ClientTopUnit { get; set; }

        public string HierarchyType { get; set; }   //Link to Hierarchy     eg TravelerType / Client SubUnit
        public string HierarchyItem { get; set; }   //Text Value            eg TravelerTypeName / ClientSubUnitName
        public string HierarchyCode { get; set; }   //Code                  eg TravelerTypeGuid / ClientSubUnitGuid

        public string TravelerTypeGuid { get; set; }
        public string TravelerTypeName { get; set; }

        public string ClientSubUnitGuid { get; set; }
        public string ClientSubUnitName { get; set; }
        
    }
}