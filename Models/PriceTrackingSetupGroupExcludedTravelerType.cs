using CWTDesktopDatabase.Validation;
using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(PriceTrackingSetupGroupExcludedTravelerTypeValidation))]
    public partial class PriceTrackingSetupGroupExcludedTravelerType : CWTBaseModel
    {
        public string TravelerTypeName { get; set; }
    }
}
