using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.Models
{

    [MetadataType(typeof(PolicyMessageGroupItemAirValidation))]
    public class PolicyMessageGroupItemAir : PolicyMessageGroupItem
    {
        public string SupplierName { get; set; }
        public string PolicyLocationName { get; set; }
    }

    [MetadataType(typeof(PolicyMessageGroupItemCarValidation))]
    public class PolicyMessageGroupItemCar : PolicyMessageGroupItem
    {
        public string SupplierName { get; set; }
        public string PolicyLocationName { get; set; }
    }

    [MetadataType(typeof(PolicyMessageGroupItemHotelValidation))]
    public class PolicyMessageGroupItemHotel : PolicyMessageGroupItem
    {
        public string SupplierName { get; set; }
        public string PolicyLocationName { get; set; }
    }

    public class PolicyMessageGroupItemType
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

	public partial class PolicyMessageGroupItem : CWTBaseModel
    {
    }
}